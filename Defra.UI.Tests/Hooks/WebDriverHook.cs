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

        /// <summary>
        /// Returns the currently active driver.
        /// After the IPAFFS tab hand-off, BoDi's IWebDriver registration is swapped to Browser 2
        /// by SignOutSteps.SwapDriver, so resolving from the container always returns the correct driver.
        /// Before any hand-off, this returns the original Browser 1 driver.
        /// </summary>
        private IWebDriver ActiveDriver =>
            _objectContainer.IsRegistered<IWebDriver>()
                ? _objectContainer.Resolve<IWebDriver>()
                : Driver;

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

            // Store scenario in ScenarioContext so Dynamics hook can access it
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
                if (_scenarioContext.TestError == null)
                {
                    var log = CreateLogForContextValues();
                    if (!string.IsNullOrWhiteSpace(log) && log != "<pre></pre>")
                    {
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
                    try
                    {
                        AttachScreenShotToXmlReport();
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug($"Screenshot skipped — driver unavailable during teardown: {ex.Message}");
                    }
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

            try
            {
                ((ITakesScreenshot)ActiveDriver).GetScreenshot().SaveAsFile(fileName);
                _reqnrollOutputHelper.AddAttachment(fileName);
                Logger.Debug($"SCREENSHOT {fileName} ");
            }
            catch (Exception ex)
            {
                Logger.Debug($"Screenshot skipped — driver unavailable during teardown: {ex.Message}");
            }
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
            // AfterStepHooks (Order = 100) runs first and sets this flag for Dynamics steps.
            // When true, Dynamics AfterStepHooks owns reporting — stand down here.
            if (_scenarioContext.ContainsKey("IsDynamicsActive") && _scenarioContext.Get<bool>("IsDynamicsActive"))
            {
                return;
            }

            var stepInfo = _scenarioContext.StepContext.StepInfo.Text;
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

            if (_scenarioContext.TestError == null)
            {
                _scenario.CreateNode(new GherkinKeyword(stepType), stepInfo);
            }
            else
            {
                // Use ActiveDriver so the screenshot is taken on the correct browser
                // (Browser 2 IPAFFS tab after hand-off, Browser 1 before)
                var screenshotPath = CaptureScreenshotFullPage();

                if (!string.IsNullOrWhiteSpace(screenshotPath))
                {
                    var stepNode = _scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                                 .Fail(_scenarioContext.TestError.Message)
                                 .AddScreenCaptureFromPath(screenshotPath);

                    var log = CreateLogForContextValues();
                    if (!string.IsNullOrWhiteSpace(log) && log != "<pre></pre>")
                    {
                        stepNode.Info(log);
                    }
                }
                else
                {
                    // Screenshot not available — log failure and context without it
                    var stepNode = _scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                                 .Fail(_scenarioContext.TestError.Message);

                    var log = CreateLogForContextValues();
                    if (!string.IsNullOrWhiteSpace(log) && log != "<pre></pre>")
                    {
                        stepNode.Info(log);
                    }
                }
            }

            Thread.Sleep(1000);
        }

        private string CreateLogForContextValues()
        {
            // Keys used internally for the browser hand-off mechanism — not useful test diagnostics
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
                foreach (var context in _scenarioContext)
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
        /// Captures a full-page screenshot using the currently active driver.
        /// Uses ActiveDriver so that after the IPAFFS tab hand-off, screenshots are
        /// taken on Browser 2's IPAFFS tab rather than the disposed Browser 1.
        /// Falls back to a viewport screenshot, and returns an empty string if
        /// the driver is unavailable — callers must guard against an empty return value.
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

            var driver = ActiveDriver;

            try
            {
                SwitchToValidWindow(driver);

                // Guard against PDF tabs and other non-HTML contexts where document.body is null,
                // which causes NullReferenceException inside Convert.ToInt32 before the outer
                // catch can protect it.
                var js = (IJavaScriptExecutor)driver;

                var bodyExists = js.ExecuteScript("return document.body !== null && document.body !== undefined");
                if (bodyExists == null || !(bool)bodyExists)
                {
                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filePath);
                    return $"./Screenshots/{uniqueFileName}";
                }

                int totalHeight = Convert.ToInt32(js.ExecuteScript(
                    "return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)"));
                int viewportHeight = Convert.ToInt32(js.ExecuteScript("return window.innerHeight"));
                int viewportWidth = Convert.ToInt32(js.ExecuteScript("return window.innerWidth"));

                if (totalHeight <= viewportHeight)
                {
                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filePath);
                    return $"./Screenshots/{uniqueFileName}";
                }

                js.ExecuteScript("window.scrollTo(0, 0)");
                Thread.Sleep(200);

                var screenshots = new List<Bitmap>();
                int scrollPosition = 0;

                while (scrollPosition < totalHeight)
                {
                    js.ExecuteScript($"window.scrollTo(0, {scrollPosition})");
                    Thread.Sleep(200);

                    var screenshotBytes = ((ITakesScreenshot)driver).GetScreenshot().AsByteArray;
                    using var ms = new MemoryStream(screenshotBytes);
                    var bmp = new Bitmap(ms);

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
            catch (Exception ex)
            {
                Logger.Debug($"Full-page screenshot failed, using viewport screenshot: {ex.Message}");

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

        /// <summary>
        /// Switches to a valid browser window handle on the given driver.
        /// </summary>
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

        /// <summary>
        /// Retained for backwards compatibility in case revert to original method is needed.
        /// </summary>
        private string CaptureScreenshot()
        {
            var screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Reports", "Screenshots");

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            var driver = ActiveDriver;

            try
            {
                var handles = driver.WindowHandles;
                if (handles != null && handles.Count > 0)
                {
                    var currentHandle = driver.CurrentWindowHandle;
                    if (handles.Contains(currentHandle))
                    {
                        driver.SwitchTo().Window(currentHandle);
                    }
                    else
                    {
                        driver.SwitchTo().Window(handles.Last());
                    }
                }
            }
            catch
            {
                // Silently handle window switch errors
            }

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var uniqueFileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(screenshotsDir, uniqueFileName);

            screenshot.SaveAsFile(filePath);

            return $"./Screenshots/{uniqueFileName}";
        }
    }
}