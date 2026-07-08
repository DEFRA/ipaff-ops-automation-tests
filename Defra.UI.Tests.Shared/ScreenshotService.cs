using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Shared.Tools
{
    /// <summary>
    /// Centralized screenshot service for both Dynamics and non-Dynamics test hooks.
    /// Optimized for headless Chrome using Chrome DevTools Protocol (CDP) for full-page screenshots.
    /// Supports both local ChromeDriver (direct CDP) and Selenium Grid (CDP via /session/{id}/goog/cdp/execute).
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

        // Primary identifier: matches "SPS-7383", "SPS_7383", case-insensitive.
        private static readonly Regex SpsPattern =
            new(@"SPS[-_ ]?\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Fallback identifier: matches "CHEDA Happy Path", "CHEDP Happy Path", "CHED Happy Path", etc.
        private static readonly Regex ChedHappyPathPattern =
            new(@"CHED[A-Z]?\s+Happy\s+Path", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Captures a viewport screenshot using the provided driver.
        /// Useful for TRX attachments where a smaller image is acceptable.
        /// </summary>
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
        /// Order of attempts:
        ///   1) Direct CDP via <see cref="ChromeDriver"/> (local non-grid runs).
        ///   2) CDP over HTTP against the Selenium Grid session endpoint (RemoteWebDriver/grid runs).
        ///   3) Viewport screenshot fallback.
        /// </summary>
        /// <param name="driver">The WebDriver instance to capture from.</param>
        /// <param name="scenarioTitle">The scenario title for filename generation.</param>
        /// <param name="gridUrl">Optional Selenium Grid hub URL (e.g. "http://localhost:4444").
        /// Required for full-page capture when the driver is a <see cref="RemoteWebDriver"/>.</param>
        public static string CaptureScreenshotFullPage(IWebDriver driver, string scenarioTitle, string gridUrl = null)
        {
            EnsureScreenshotDirectory();

            var uniqueFileName = GenerateScreenshotFileName(scenarioTitle);
            var filePath = Path.Combine(ScreenshotsFolder.FullName, uniqueFileName);

            try
            {
                SwitchToValidWindow(driver);

                // 1) Local ChromeDriver — direct CDP.
                if (TryCaptureCdpScreenshot(driver, filePath))
                {
                    return $"./Screenshots/{uniqueFileName}";
                }

                // 2) RemoteWebDriver (Selenium Grid) — CDP via grid HTTP endpoint.
                if (TryCaptureCdpScreenshotViaGrid(driver, filePath, gridUrl))
                {
                    return $"./Screenshots/{uniqueFileName}";
                }

                // 3) Viewport fallback.
                return CaptureViewportScreenshot(driver, filePath, uniqueFileName);
            }
            catch (Exception)
            {
                // Screenshot failed silently
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the absolute path that <see cref="CaptureScreenshotFullPage"/> would write to
        /// for the given scenario title. Allows hooks to locate the existing screenshot without
        /// re-capturing (preserves the rich full-page image).
        /// </summary>
        public static string GetScreenshotPathForScenario(string scenarioTitle)
        {
            return Path.Combine(ScreenshotsFolder.FullName, GenerateScreenshotFileName(scenarioTitle));
        }

        /// <summary>
        /// Attempts to capture full-page screenshot using Chrome DevTools Protocol (CDP) via a
        /// directly-instantiated <see cref="ChromeDriver"/>. Will return false when running
        /// against Selenium Grid (driver is <see cref="RemoteWebDriver"/>, not <see cref="ChromeDriver"/>).
        /// </summary>
        private static bool TryCaptureCdpScreenshot(IWebDriver driver, string filePath)
        {
            try
            {
                if (driver is not ChromeDriver chromeDriver)
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

                // 1) Page dimensions
                var pageMetricsResult = chromeDriver.ExecuteCdpCommand("Page.getLayoutMetrics", new Dictionary<string, object>());
                if (pageMetricsResult is not Dictionary<string, object> pageMetrics) return false;
                if (!pageMetrics.TryGetValue("contentSize", out var contentSizeObj)) return false;
                if (contentSizeObj is not Dictionary<string, object> contentSize) return false;
                if (!contentSize.TryGetValue("width", out var wObj) || !contentSize.TryGetValue("height", out var hObj)) return false;

                var width = (int)Math.Ceiling(Convert.ToDouble(wObj));
                var height = (int)Math.Ceiling(Convert.ToDouble(hObj));

                // 2) Expand viewport to full content size so lazy-rendered sections paint.
                var emulateParams = new Dictionary<string, object>
                {
                    { "width", width },
                    { "height", height },
                    { "deviceScaleFactor", 1 },
                    { "mobile", false }
                };
                chromeDriver.ExecuteCdpCommand("Emulation.setDeviceMetricsOverride", emulateParams);

                try
                {
                    // 3) Capture using captureBeyondViewport for true full-page output.
                    var screenshotParams = new Dictionary<string, object>
                    {
                        { "format", "png" },
                        { "captureBeyondViewport", true },
                        { "fromSurface", true },
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
                    if (screenshotResult is not Dictionary<string, object> screenshot) return false;
                    if (!screenshot.TryGetValue("data", out var dataObj)) return false;

                    var screenshotData = dataObj as string;
                    if (string.IsNullOrEmpty(screenshotData)) return false;

                    File.WriteAllBytes(filePath, Convert.FromBase64String(screenshotData));
                    return true;
                }
                finally
                {
                    // 4) Always reset the emulation override.
                    chromeDriver.ExecuteCdpCommand("Emulation.clearDeviceMetricsOverride", new Dictionary<string, object>());
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts full-page CDP capture against a Selenium Grid by POSTing to
        /// <c>{gridUrl}/session/{sessionId}/goog/cdp/execute</c>. This is required when the
        /// pipeline runs against the Dockerized grid (driver is RemoteWebDriver, not ChromeDriver).
        /// </summary>
        private static bool TryCaptureCdpScreenshotViaGrid(IWebDriver driver, string filePath, string gridUrl)
        {
            if (string.IsNullOrWhiteSpace(gridUrl)) return false;
            if (driver is not RemoteWebDriver remoteDriver) return false;
            if (driver is ChromeDriver) return false; // already handled by direct path

            try
            {
                var sessionId = remoteDriver.SessionId.ToString();

                // 1) Page dimensions
                var metricsBody = SendCdpCommandViaGrid(gridUrl, sessionId, "Page.getLayoutMetrics", "{}");
                if (string.IsNullOrEmpty(metricsBody)) return false;

                int width, height;
                using (var doc = JsonDocument.Parse(metricsBody))
                {
                    if (!doc.RootElement.TryGetProperty("value", out var value)) return false;
                    if (!value.TryGetProperty("contentSize", out var contentSize)) return false;
                    if (!contentSize.TryGetProperty("width", out var wEl)) return false;
                    if (!contentSize.TryGetProperty("height", out var hEl)) return false;

                    width = (int)Math.Ceiling(wEl.GetDouble());
                    height = (int)Math.Ceiling(hEl.GetDouble());
                }

                // 2) Expand viewport so lazy-rendered sections paint before capture.
                var emulateParams = $"{{\"width\":{width},\"height\":{height},\"deviceScaleFactor\":1,\"mobile\":false}}";
                SendCdpCommandViaGrid(gridUrl, sessionId, "Emulation.setDeviceMetricsOverride", emulateParams);

                try
                {
                    // 3) Capture — captureBeyondViewport ensures the full content height is rendered.
                    var clipParams =
                        $"{{\"format\":\"png\",\"captureBeyondViewport\":true,\"fromSurface\":true," +
                        $"\"clip\":{{\"x\":0,\"y\":0,\"width\":{width},\"height\":{height},\"scale\":1}}}}";

                    var sshotBody = SendCdpCommandViaGrid(gridUrl, sessionId, "Page.captureScreenshot", clipParams);
                    if (string.IsNullOrEmpty(sshotBody)) return false;

                    using var sshotDoc = JsonDocument.Parse(sshotBody);
                    if (!sshotDoc.RootElement.TryGetProperty("value", out var sshotValue)) return false;
                    if (!sshotValue.TryGetProperty("data", out var dataEl)) return false;

                    var data = dataEl.GetString();
                    if (string.IsNullOrEmpty(data)) return false;

                    File.WriteAllBytes(filePath, Convert.FromBase64String(data));
                    return true;
                }
                finally
                {
                    // 4) Always reset the emulation override.
                    SendCdpCommandViaGrid(gridUrl, sessionId, "Emulation.clearDeviceMetricsOverride", "{}");
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sends a CDP command through the Selenium Grid's pass-through endpoint and returns the
        /// raw JSON response body, or null on failure.
        /// </summary>
        private static string SendCdpCommandViaGrid(string gridUrl, string sessionId, string command, string paramsJson)
        {
            try
            {
                var endpoint = $"{gridUrl.TrimEnd('/')}/session/{sessionId}/goog/cdp/execute";
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };

                var payload = $"{{\"cmd\":\"{command}\",\"params\":{paramsJson}}}";
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, content).Result;

                if (!response.IsSuccessStatusCode) return null;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                return null;
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
                "DynamicsIpaffsDriver",
                "FailureScreenshotPath"
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

        /// <summary>
        /// Extracts a concise, meaningful identifier from the scenario title for use in screenshot filenames.
        /// Priority:
        ///   1. SPS reference (e.g. "SPS-7383") — preferred when present in the title.
        ///   2. CHED Happy Path label (e.g. "CHEDA Happy Path") — fallback for happy-path scenarios.
        ///   3. Sanitised full scenario title (truncated) — last resort.
        /// </summary>
        public static string ExtractScenarioIdentifier(string scenarioTitle)
        {
            if (string.IsNullOrWhiteSpace(scenarioTitle))
                return Guid.NewGuid().ToString();

            var spsMatch = SpsPattern.Match(scenarioTitle);
            if (spsMatch.Success)
            {
                // Normalise to canonical "SPS-1234" form regardless of original separator/casing.
                var digits = new string(spsMatch.Value.Where(char.IsDigit).ToArray());
                return $"SPS-{digits}";
            }

            var chedMatch = ChedHappyPathPattern.Match(scenarioTitle);
            if (chedMatch.Success)
            {
                return SanitiseFileName(chedMatch.Value);
            }

            return SanitiseFileName(scenarioTitle);
        }

        private static string GenerateScreenshotFileName(string scenarioTitle)
        {
            try
            {
                return $"{ExtractScenarioIdentifier(scenarioTitle)}.png";
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