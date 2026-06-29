using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;

namespace Defra.UI.Tests.Shared.Tools
{
    /// <summary>
    /// Centralized screenshot service for both Dynamics and non-Dynamics test hooks.
    /// Optimized for headless Chrome using Chrome DevTools Protocol (CDP) for full-page screenshots.
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
        /// For headless Chrome, uses viewport screenshot.
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
        /// Captures a full-page screenshot optimized for headless Chrome using CDP.
        /// Automatically detects if the driver is Chrome and uses CDP protocol for best results.
        /// Falls back to viewport screenshot if CDP fails or driver is not Chrome.
        /// </summary>
        /// <param name="driver">The WebDriver instance to capture from</param>
        /// <param name="scenarioTitle">The scenario title for filename generation</param>
        /// <returns>Relative path to the screenshot, or empty string if capture failed</returns>
        public static string CaptureScreenshotFullPage(IWebDriver driver, string scenarioTitle)
        {
            EnsureScreenshotDirectory();

            var uniqueFileName = GenerateScreenshotFileName(scenarioTitle);
            var filePath = Path.Combine(ScreenshotsFolder.FullName, uniqueFileName);

            try
            {
                SwitchToValidWindow(driver);

                // Try CDP method first (works great for headless Chrome)
                if (TryCaptureCdpScreenshot(driver, filePath))
                {
                    return $"./Screenshots/{uniqueFileName}";
                }

                // Fall back to viewport screenshot
                return CaptureViewportScreenshot(driver, filePath, uniqueFileName);
            }
            catch (Exception)
            {
                // Screenshot failed silently
                return string.Empty;
            }
        }

        /// <summary>
        /// Attempts to capture full-page screenshot using Chrome DevTools Protocol (CDP).
        /// This method works reliably with headless Chrome and handles dynamic content.
        /// </summary>
        private static bool TryCaptureCdpScreenshot(IWebDriver driver, string filePath)
        {
            try
            {
                // Only works with Chrome drivers
                if (!(driver is ChromeDriver chromeDriver))
                {
                    return false;
                }

                // Guard against non-HTML contexts (PDFs, etc.)
                var js = (IJavaScriptExecutor)driver;
                var bodyExists = js.ExecuteScript("return document.body !== null && document.body !== undefined");
                if (bodyExists == null || !(bool)bodyExists)
                {
                    return false;
                }

                // Get page dimensions using CDP
                var pageMetricsResult = chromeDriver.ExecuteCdpCommand("Page.getLayoutMetrics", new Dictionary<string, object>());

                // Cast the result to a dictionary
                if (!(pageMetricsResult is Dictionary<string, object> pageMetrics))
                {
                    return false;
                }

                if (!pageMetrics.ContainsKey("contentSize"))
                {
                    return false;
                }

                // Extract width and height from contentSize
                if (!(pageMetrics["contentSize"] is Dictionary<string, object> contentSize))
                {
                    return false;
                }

                if (!contentSize.ContainsKey("width") || !contentSize.ContainsKey("height"))
                {
                    return false;
                }

                var width = Convert.ToInt32(contentSize["width"]);
                var height = Convert.ToInt32(contentSize["height"]);

                // Capture screenshot with full page dimensions using CDP
                var screenshotParams = new Dictionary<string, object>
                {
                    { "format", "png" },
                    { "clip", new Dictionary<string, object>
                        {
                            { "x", 0 },
                            { "y", 0 },
                            { "width", width },
                            { "height", height },
                            { "scale", 1 }
                        }
                    }
                };

                var screenshotResult = chromeDriver.ExecuteCdpCommand("Page.captureScreenshot", screenshotParams);

                // Cast the result to a dictionary
                if (!(screenshotResult is Dictionary<string, object> screenshot))
                {
                    return false;
                }

                if (!screenshot.ContainsKey("data"))
                {
                    return false;
                }

                // Decode base64 screenshot data and save
                var screenshotData = screenshot["data"] as string;
                if (string.IsNullOrEmpty(screenshotData))
                {
                    return false;
                }

                var imageBytes = Convert.FromBase64String(screenshotData);
                File.WriteAllBytes(filePath, imageBytes);

                return true;
            }
            catch (Exception)
            {
                // CDP method failed — will fall back to viewport
                return false;
            }
        }

        /// <summary>
        /// Captures a viewport screenshot as a fallback when CDP is unavailable.
        /// </summary>
        private static string CaptureViewportScreenshot(IWebDriver driver, string filePath, string uniqueFileName)
        {
            try
            {
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filePath);
                return $"./Screenshots/{uniqueFileName}";
            }
            catch (Exception)
            {
                return string.Empty;
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