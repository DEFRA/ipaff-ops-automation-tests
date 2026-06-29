using OpenQA.Selenium;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;

namespace Defra.UI.Tests.Shared.Tools
{
    /// <summary>
    /// Centralized screenshot service for both Dynamics and non-Dynamics test hooks.
    /// Provides simplified, containerized-environment-optimized screenshot capture.
    /// Used by WebDriverHook (non-Dynamics), AfterStepHooks (Dynamics), and AfterScenarioHooks (Dynamics).
    /// </summary>
    public class ScreenshotService
    {
        private static readonly DirectoryInfo ScreenshotsFolder = new DirectoryInfo(
            Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Reports",
                "Screenshots"
            )
        );

        /// <summary>
        /// Captures a screenshot using the provided driver.
        /// In Docker/containerized environments, viewport screenshot is preferred over full-page
        /// as full-page composition can be unreliable with headless Chrome.
        /// </summary>
        /// <param name="driver">The WebDriver instance to capture from</param>
        /// <param name="scenarioTitle">The scenario title for filename generation</param>
        /// <returns>Relative path to the screenshot (e.g., "./Screenshots/filename.png"), or empty string if capture failed</returns>
        public static string CaptureScreenshot(IWebDriver driver, string scenarioTitle)
        {
            EnsureScreenshotDirectory();

            try
            {
                SwitchToValidWindow(driver);

                var uniqueFileName = GenerateScreenshotFileName(scenarioTitle);
                var filePath = Path.Combine(ScreenshotsFolder.FullName, uniqueFileName);

                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filePath);

                return $"./Screenshots/{uniqueFileName}";
            }
            catch (Exception)
            {
                // Screenshot capture failed silently — callers should handle empty return
                return string.Empty;
            }
        }

        /// <summary>
        /// Attempts to capture a full-page screenshot with fallback to viewport.
        /// Optimized for containerized environments by using viewport as primary fallback.
        /// </summary>
        /// <param name="driver">The WebDriver instance to capture from</param>
        /// <param name="scenarioTitle">The scenario title for filename generation</param>
        /// <returns>Relative path to the screenshot, or empty string if capture failed</returns>
        public static string CaptureScreenshotFullPage(IWebDriver driver, string scenarioTitle)
        {
            EnsureScreenshotDirectory();

            try
            {
                SwitchToValidWindow(driver);

                var uniqueFileName = GenerateScreenshotFileName(scenarioTitle);
                var filePath = Path.Combine(ScreenshotsFolder.FullName, uniqueFileName);

                // Guard against PDF tabs and other non-HTML contexts
                var js = (IJavaScriptExecutor)driver;
                var bodyExists = js.ExecuteScript("return document.body !== null && document.body !== undefined");

                if (bodyExists == null || !(bool)bodyExists)
                {
                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filePath);
                    return $"./Screenshots/{uniqueFileName}";
                }

                try
                {
                    int totalHeight = Convert.ToInt32(js.ExecuteScript(
                        "return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)"));
                    int viewportHeight = Convert.ToInt32(js.ExecuteScript("return window.innerHeight"));
                    int viewportWidth = Convert.ToInt32(js.ExecuteScript("return window.innerWidth"));

                    // If page fits in viewport, take single screenshot
                    if (totalHeight <= viewportHeight)
                    {
                        ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filePath);
                        return $"./Screenshots/{uniqueFileName}";
                    }

                    // Attempt full-page composition
                    return CaptureFullPageComposite(driver, js, filePath, totalHeight, viewportHeight, viewportWidth, uniqueFileName);
                }
                catch (Exception)
                {
                    // Full-page composition failed — fall back to viewport screenshot
                    try
                    {
                        ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filePath);
                        return $"./Screenshots/{uniqueFileName}";
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                // Screenshot failed silently
                return string.Empty;
            }
        }

        /// <summary>
        /// Composes multiple viewport screenshots into a single full-page image.
        /// </summary>
        private static string CaptureFullPageComposite(
            IWebDriver driver,
            IJavaScriptExecutor js,
            string filePath,
            int totalHeight,
            int viewportHeight,
            int viewportWidth,
            string uniqueFileName)
        {
            js.ExecuteScript("window.scrollTo(0, 0)");
            Thread.Sleep(200);

            var screenshots = new List<Bitmap>();
            int scrollPosition = 0;

            try
            {
                while (scrollPosition < totalHeight)
                {
                    js.ExecuteScript($"window.scrollTo(0, {scrollPosition})");
                    Thread.Sleep(200);

                    var screenshotBytes = ((ITakesScreenshot)driver).GetScreenshot().AsByteArray;
                    using var ms = new MemoryStream(screenshotBytes);
                    var bmp = new Bitmap(ms);

                    // Crop overlapping sections on subsequent scrolls
                    if (scrollPosition + viewportHeight > totalHeight && scrollPosition > 0)
                    {
                        int overlap = (scrollPosition + viewportHeight) - totalHeight;
                        var cropped = bmp.Clone(
                            new Rectangle(0, overlap, bmp.Width, bmp.Height - overlap),
                            bmp.PixelFormat
                        );
                        screenshots.Add(cropped);
                        bmp.Dispose();
                    }
                    else
                    {
                        screenshots.Add(bmp);
                    }

                    scrollPosition += viewportHeight;
                }

                // Compose final image
                int finalHeight = screenshots.Sum(s => s.Height);
                int finalWidth = screenshots.Max(s => s.Width);

                using var finalImage = new Bitmap(finalWidth, finalHeight);
                using (var graphics = Graphics.FromImage(finalImage))
                {
                    graphics.Clear(Color.White);
                    int yOffset = 0;

                    foreach (var screenshot in screenshots)
                    {
                        graphics.DrawImage(screenshot, 0, yOffset);
                        yOffset += screenshot.Height;
                    }
                }

                finalImage.Save(filePath, ImageFormat.Png);

                foreach (var screenshot in screenshots)
                {
                    screenshot.Dispose();
                }

                js.ExecuteScript("window.scrollTo(0, 0)");

                return $"./Screenshots/{uniqueFileName}";
            }
            finally
            {
                // Ensure cleanup
                foreach (var screenshot in screenshots)
                {
                    screenshot?.Dispose();
                }
            }
        }

        /// <summary>
        /// Generates a formatted log string from scenario context key-value pairs.
        /// Filters out internal framework keys used for the browser hand-off mechanism.
        /// </summary>
        public static string CreateLogForContextValues(IEnumerable<KeyValuePair<string, object>> contextItems)
        {
            var internalKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ExtentScenario",
                "IsDynamicsActive",
                "DynamicsWindowHandle",
                "IpaffsInDynamicsBrowserHandle",
                "DynamicsIpaffsDriver"
            };

            var log = new StringBuilder("<pre>");
            try
            {
                foreach (var context in contextItems)
                {
                    if (!internalKeys.Contains(context.Key))
                    {
                        log.AppendLine($"{context.Key} : <b>{FormatValue(context.Value)}</b><br>");
                    }
                }
            }
            catch (Exception ex)
            {
                log.AppendLine($"Error capturing context values: {ex.Message}<br>");
            }

            log.Append("</pre>");
            return log.ToString();
        }

        /// <summary>
        /// Formats a value for display in HTML logs.
        /// </summary>
        public static string FormatValue(object value)
        {
            if (value == null)
                return "null";

            if (value is Array array)
                return string.Join(", ", array.Cast<object>());

            if (value is IEnumerable<object> list)
                return string.Join(", ", list);

            if (value is System.Collections.IEnumerable enumerable && value is not string)
                return string.Join(", ", enumerable.Cast<object>());

            return value.ToString();
        }

        /// <summary>
        /// Sanitizes a filename by removing invalid characters, replacing spaces with underscores,
        /// and truncating to 80 characters to avoid path-length issues.
        /// </summary>
        public static string SanitiseFileName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Guid.NewGuid().ToString();

            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitised = new string(input.Where(c => !invalidChars.Contains(c)).ToArray());
            sanitised = sanitised.Replace(' ', '_');

            if (sanitised.Length > 80)
            {
                sanitised = sanitised[..80];
            }

            return string.IsNullOrWhiteSpace(sanitised) ? Guid.NewGuid().ToString() : sanitised;
        }

        private static string GenerateScreenshotFileName(string scenarioTitle)
        {
            try
            {
                var sanitised = SanitiseFileName(scenarioTitle);
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                return $"{sanitised}_{timestamp}.png";
            }
            catch
            {
                return $"{Guid.NewGuid()}.png";
            }
        }

        private static void SwitchToValidWindow(IWebDriver driver)
        {
            try
            {
                var handles = driver.WindowHandles;
                if (handles != null && handles.Count > 0)
                {
                    var currentHandle = driver.CurrentWindowHandle;
                    if (!handles.Contains(currentHandle))
                    {
                        driver.SwitchTo().Window(handles.Last());
                    }
                }
            }
            catch
            {
                // Silently handle window switch errors
            }
        }

        private static void EnsureScreenshotDirectory()
        {
            if (!ScreenshotsFolder.Exists)
            {
                Directory.CreateDirectory(ScreenshotsFolder.FullName);
            }
        }
    }
}