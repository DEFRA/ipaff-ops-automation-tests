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
                if (_scenarioContext.TestError != null)
                {
                    takeScreenShot = true;
                    var error = _scenarioContext.TestError;
                    Logger.LogMessage("An error ocurred:" + error.Message);
                    Logger.Debug("It was of type:" + error.GetType().Name);
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("Not able to take screenshot" + ex.Message);
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
            var lastStep = "print all captured values in report";

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
                var screenshotPath = CaptureScreenshotFullPage(Driver);

                stepNode = _scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                         .Fail(_scenarioContext.TestError.Message)
                         .AddScreenCaptureFromPath(screenshotPath);
            }

            if (stepInfo.ToLower().Contains(lastStep))
            {
                var log = new StringBuilder("<pre>");
                foreach (var context in _scenarioContext)
                {
                    log.AppendLine($"{context.Key} : <b>{FormatValue(context.Value)}</b><br>");
                }

                log.Append("</pre>");

                stepNode.Info(log.ToString());
            }
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

        private string CaptureScreenshotFullPage(IWebDriver driver)
        {
            var screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Reports", "Screenshots");
            var uniqueFileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(screenshotsDir, uniqueFileName);

            var js = (IJavaScriptExecutor)driver;

            Func<IWebElement> getScrollElement = () =>
                                                    (IWebElement)js.ExecuteScript(@"
                                    let el = document.scrollingElement;
                                    if (!el) el = document.documentElement;
                                    return el;");

            int totalHeight = Convert.ToInt32(js.ExecuteScript(
                "return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)"
            ));

            int viewportWidth = Convert.ToInt32(js.ExecuteScript("return Math.max(document.body.scrollWidth, document.documentElement.scrollWidth)")); 
            
            var screenshots = new List<Bitmap>();
            int scrollPosition = 0;

            while (scrollPosition < totalHeight)
            {
                var scrollElement = getScrollElement();

                js.ExecuteScript("arguments[0].scrollTop = arguments[1];", scrollElement, scrollPosition);
                // allow rendering
                Thread.Sleep(250);

                // Capture screenshot
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                using var ms = new MemoryStream(screenshot.AsByteArray);
                var bmp = new Bitmap(ms);

                int actualScreenshotHeight = bmp.Height;

                // Recalculate total height (lazy loading)
                totalHeight = Convert.ToInt32(js.ExecuteScript(
                    "return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)"
                ));

                int remainingHeight = totalHeight - scrollPosition;
                int cropHeight = Math.Min(actualScreenshotHeight, remainingHeight);

                // Crop only the visible part
                var cropped = bmp.Clone(
                    new Rectangle(0, 0, viewportWidth, cropHeight),
                    bmp.PixelFormat
                );

                screenshots.Add(cropped);

                scrollPosition += actualScreenshotHeight;
            }

            // Stitch vertically
            var finalImage = new Bitmap(viewportWidth, totalHeight);
            using (var g = Graphics.FromImage(finalImage))
            {
                int offset = 0;
                foreach (var img in screenshots)
                {
                    g.DrawImage(img, new Rectangle(0, offset, img.Width, img.Height));
                    offset += img.Height;
                }
            }

            finalImage.Save(filePath, ImageFormat.Png);

            return $"./Screenshots/{uniqueFileName}";
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
    }
}