using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin;
using Capgemini.PowerApps.SpecFlowBindings.Hooks;
using Defra.UI.Framework.Object;
using Defra.UI.Tests.Capabilities;
using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.HelperMethods;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Reqnroll;
using Reqnroll.BoDi;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Defra.UI.Tests.Hooks
{
    [Binding]
    public class WebDriverHook
    {
        public IWebDriver Driver { get; set; }

        private ScenarioContext _scenarioContext;
        private IObjectContainer _objectContainer;
        private IReqnrollOutputHelper _reqnrollOutputHelper;

        private IFetchCodeFromEmail fetchCodeFromEmail => _objectContainer.IsRegistered<IFetchCodeFromEmail>() ? _objectContainer.Resolve<IFetchCodeFromEmail>() : null;

        private static bool isRunOnce = false;

        private static ExtentReports _extent;
        [ThreadStatic]
        private static ExtentTest _feature;
        [ThreadStatic]
        private static ExtentTest _scenario;

        public WebDriverHook(ScenarioContext context, ObjectContainer objectContainer,
            IReqnrollOutputHelper reqnrollOutputHelper)
        {
            _scenarioContext = context;
            _objectContainer = objectContainer;
            _reqnrollOutputHelper = reqnrollOutputHelper;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _extent = ExtentReportManager.GetInstance();
            isRunOnce = true;
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _feature = _extent.CreateTest<AventStack.ExtentReports.Gherkin.Model.Feature>(featureContext.FeatureInfo.Title);
        }

        [BeforeScenario(Order = (int)HookRunOrder.WebDriver)]
        public void BeforeTestScenario()
        {
            Logger.Debug("Starting set Capability");

            var site = new Site();
            site.With(GetDriverOptions());
            Driver = site.WebDriver.Driver;

            if (ConfigSetup.BaseConfiguration.UiFrameworkConfiguration.IsDebug)
            {
                PrintNodeInfo("http://localhost:4444/status");
            }

            _objectContainer.RegisterInstanceAs(Driver);

            if (ConfigSetup.BaseConfiguration.TestConfiguration.IsAccessibilityEnabled)
            {
                var reportPath = Path.Combine($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}", "Accessibility");
                Console.WriteLine(reportPath);
                //Cognizant.WCAG.Compliance.Checker.Start.Init(Driver, reportPath);
            }

            _scenario = _feature.CreateNode<AventStack.ExtentReports.Gherkin.Model.Scenario>(_scenarioContext.ScenarioInfo.Title);

            // Store scenario in ScenarioContext so PIMS hook can access it
            _scenarioContext.Set(_scenario, "ExtentScenario");

            if (isRunOnce)
            {
                GovernmentGateway.Initialize(_objectContainer);

                if (ConfigSetup.BaseConfiguration.TestConfiguration.IsLiveUserAccount)
                {
                    fetchCodeFromEmail.DeleteAllMessagesFromInbox();
                    isRunOnce = false;

                    GovernmentGateway.Instance.GetUserDetails();
                }
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            bool takeScreenShot = false;
            try
            {
                // Log context values at the end of successful scenarios only
                // (Failed scenarios already have context values attached to the failing step)
                if (_scenarioContext.TestError == null)
                {
                    var log = CreateLogForContextValues();
                    if (!string.IsNullOrWhiteSpace(log) && log != "<pre></pre>")
                    {
                        // Create a summary node at the end of the scenario
                        _scenario.CreateNode(new GherkinKeyword("*"), "LOG: Captured Scenario Context Values")
                                .Info(log);
                    }
                }
                else
                {
                    takeScreenShot = true;
                    var error = _scenarioContext.TestError;
                    Logger.LogMessage("An error ocurred:" + error.Message);
                    Logger.Debug("It was of type:" + error.GetType().Name);
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("Not able to process scenario end: " + ex.Message);
            }
            finally
            {
                if (takeScreenShot)
                {
                    AttachScreenShotToXmlReport();
                }

                CloseBrowsers();

                if (ConfigSetup.BaseConfiguration.TestConfiguration.IsAccessibilityEnabled)
                {
                    Cognizant.WCAG.Compliance.Checker.Reporter.HtmlReport.GenerateByCategory();
                    Cognizant.WCAG.Compliance.Checker.Reporter.HtmlReport.GenerateByGuideline();
                }

                _extent.Flush();
            }
        }

        private void AttachScreenShotToXmlReport()
        {
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filePath = Path.Combine(filePath, "TestResults");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                Logger.Debug($"{filePath} directory created....");
            }

            var fileTitle = _scenarioContext.ScenarioInfo.Title;
            var fileName = Path.Combine(filePath, $"{fileTitle}_TestFailures_{DateTime.Now:yyyyMMdd_hhss}" + ".png");

            ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(fileName);

            _reqnrollOutputHelper.AddAttachment(fileName);
            Logger.Debug($"SCREENSHOT {fileName} ");
        }

        private DriverOptions GetDriverOptions()
        {
            return _objectContainer.Resolve<IDriverOptions>().GetDriverOptions();
        }

        public void PrintNodeInfo(string gridIpAddress)
        {
            string endpoint = string.Empty;
            try
            {
                var remoteWebDriver = (RemoteWebDriver)Driver;
                var sessionId = remoteWebDriver.SessionId.ToString();
                gridIpAddress = gridIpAddress.Replace("/wd/hub", "");
                endpoint = $"{gridIpAddress}status";

                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                var resp = client.GetAsync(new Uri(endpoint)).Result.Content.ReadAsStringAsync().Result;

                Logger.Debug($"Appium node details: {resp}");
            }
            catch (Exception)
            {
                Logger.LogMessage($"Not able to print Node information for {endpoint}, most likely running against manually started appium server.");
            }
        }

        [AfterStep]
        public void AfterStep()
        {
            var stepInfo = _scenarioContext.StepContext.StepInfo.Text;

            // Skip if this is a PIMS step (let PIMS hook handle it)
            bool isPimsStep = stepInfo.Contains("PIMS", StringComparison.OrdinalIgnoreCase) ||
                             stepInfo.Contains("logged in to the", StringComparison.OrdinalIgnoreCase) ||
                             stepInfo.Contains("sub area", StringComparison.OrdinalIgnoreCase) ||
                             stepInfo.Contains("Importer Notifications", StringComparison.OrdinalIgnoreCase) ||
                             stepInfo.Contains("open the record", StringComparison.OrdinalIgnoreCase) ||
                             stepInfo.Contains("in the grid", StringComparison.OrdinalIgnoreCase);

            if (isPimsStep)
            {
                return;
            }

            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            ExtentTest stepNode;

            if (_scenarioContext.TestError == null)
            {
                stepNode = _scenario.CreateNode(new GherkinKeyword(stepType), stepInfo);
            }
            else
            {
                // Use full-page screenshot for failures
                var screenshotPath = CaptureScreenshotFullPage();

                stepNode = _scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                         .Fail(_scenarioContext.TestError.Message)
                         .AddScreenCaptureFromPath(screenshotPath);

                // Log context values on the failing step
                var log = CreateLogForContextValues();
                if (!string.IsNullOrWhiteSpace(log) && log != "<pre></pre>")
                {
                    stepNode.Info(log);
                }
            }

            Thread.Sleep(1000);
        }

        /// <summary>
        /// Automatically captures and logs all scenario context values.
        /// For passing tests: Creates a summary node at scenario end.
        /// For failing tests: Attaches context to the failing step.
        /// </summary>
        private string CreateLogForContextValues()
        {
            var log = new StringBuilder("<pre>");
            try
            {
                foreach (var context in _scenarioContext)
                {
                    if (!context.Key.Equals("ExtentScenario"))
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

        private string FormatValue(object value)
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

        private void CloseBrowsers()
        {
            try
            {
                Driver.Quit();
                Driver.Dispose();
                AfterScenarioHooks.TestCleanup();
            }
            catch { }
        }

        /// <summary>
        /// Captures a full-page screenshot by scrolling through the entire page and stitching images together.
        /// Falls back to viewport screenshot if full-page capture fails.
        /// </summary>
        private string CaptureScreenshotFullPage()
        {
            var screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Reports", "Screenshots");

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            var uniqueFileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(screenshotsDir, uniqueFileName);

            try
            {
                // Ensure we're on a valid window
                SwitchToValidWindow();

                var js = (IJavaScriptExecutor)Driver;

                // Get page dimensions
                int totalHeight = Convert.ToInt32(js.ExecuteScript(
                    "return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)"));
                int viewportHeight = Convert.ToInt32(js.ExecuteScript("return window.innerHeight"));
                int viewportWidth = Convert.ToInt32(js.ExecuteScript("return window.innerWidth"));

                // If page fits in viewport, just take a simple screenshot
                if (totalHeight <= viewportHeight)
                {
                    ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(filePath);
                    return $"./Screenshots/{uniqueFileName}";
                }

                // Scroll to top first
                js.ExecuteScript("window.scrollTo(0, 0)");
                Thread.Sleep(200);

                var screenshots = new List<Bitmap>();
                int scrollPosition = 0;

                while (scrollPosition < totalHeight)
                {
                    // Scroll to position
                    js.ExecuteScript($"window.scrollTo(0, {scrollPosition})");
                    Thread.Sleep(200);

                    // Capture current viewport
                    var screenshotBytes = ((ITakesScreenshot)Driver).GetScreenshot().AsByteArray;
                    using var ms = new MemoryStream(screenshotBytes);
                    var bmp = new Bitmap(ms);

                    // Calculate how much of this screenshot to use
                    int remainingHeight = totalHeight - scrollPosition;
                    int usableHeight = Math.Min(bmp.Height, remainingHeight);

                    // For the last screenshot, we may need to crop from the bottom
                    if (scrollPosition + viewportHeight > totalHeight && scrollPosition > 0)
                    {
                        int overlap = (scrollPosition + viewportHeight) - totalHeight;
                        var cropped = bmp.Clone(new Rectangle(0, overlap, bmp.Width, bmp.Height - overlap), bmp.PixelFormat);
                        screenshots.Add(cropped);
                        bmp.Dispose();
                    }
                    else
                    {
                        screenshots.Add(bmp);
                    }

                    scrollPosition += viewportHeight;
                }

                // Stitch all screenshots together vertically
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

                // Dispose individual screenshots
                foreach (var screenshot in screenshots)
                {
                    screenshot.Dispose();
                }

                // Scroll back to top
                js.ExecuteScript("window.scrollTo(0, 0)");

                return $"./Screenshots/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                Logger.Debug($"Full-page screenshot failed, using viewport screenshot: {ex.Message}");

                // Fallback to simple viewport screenshot
                try
                {
                    ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(filePath);
                }
                catch
                {
                    // If even fallback fails, return empty path
                    return string.Empty;
                }

                return $"./Screenshots/{uniqueFileName}";
            }
        }

        /// <summary>
        /// Switches to a valid browser window handle.
        /// </summary>
        private void SwitchToValidWindow()
        {
            try
            {
                var handles = Driver.WindowHandles;
                if (handles != null && handles.Count > 0)
                {
                    var currentHandle = Driver.CurrentWindowHandle;
                    if (!handles.Contains(currentHandle))
                    {
                        Driver.SwitchTo().Window(handles.Last());
                    }
                }
            }
            catch
            {
                // Silently handle window switch errors
            }
        }

        /// <summary>
        /// Retained old CaptureScreenshot just for backwards compatability in case we need to revert to original method.
        /// </summary>
        private string CaptureScreenshot()
        {
            var screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Reports", "Screenshots");

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            try
            {
                var handles = Driver.WindowHandles;
                if (handles != null && handles.Count > 0)
                {
                    var currentHandle = Driver.CurrentWindowHandle;
                    if (handles.Contains(currentHandle))
                    {
                        Driver.SwitchTo().Window(currentHandle);
                    }
                    else
                    {
                        Driver.SwitchTo().Window(handles.Last());
                    }
                }
            }
            catch
            {
                // Silently handle window switch errors
            }

            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            var uniqueFileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(screenshotsDir, uniqueFileName);

            screenshot.SaveAsFile(filePath);

            return $"./Screenshots/{uniqueFileName}";
        }
    }
}