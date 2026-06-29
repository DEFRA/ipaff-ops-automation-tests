using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin;
using Capgemini.PowerApps.SpecFlowBindings.Hooks;
using Defra.UI.Framework.Object;
using Defra.UI.Tests.Capabilities;
using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.HelperMethods;
using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Shared.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Reqnroll;
using Reqnroll.BoDi;
using System.Net.Http.Headers;
using System.Reflection;

namespace Defra.UI.Tests.Hooks
{
    [Binding]
    public class WebDriverHook
    {
        private const string FailureScreenshotPathKey = "FailureScreenshotPath";

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

        /// <summary>
        /// Selenium Grid hub URL pulled from configuration. Required by the screenshot service
        /// to issue CDP commands against the grid when the driver is a RemoteWebDriver.
        /// </summary>
        private static string GridUrl =>
            ConfigSetup.BaseConfiguration?.UiFrameworkConfiguration?.SeleniumGrid;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _extent = ExtentReportManager.GetInstance();
            isRunOnce = true;
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Collapse duplicate scenarios (created by earlier retry attempts) so the final
            // HTML report shows only the latest outcome per test, annotated with retry history.
            ExtentReportManager.FinalizeReport();
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
                    var log = ScreenshotService.CreateLogForContextValues(_scenarioContext);
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

        /// <summary>
        /// Attaches the failure screenshot to the TRX report.
        /// Prefers the full-page image captured during <see cref="AfterStep"/> (stashed in
        /// ScenarioContext) so the rich screenshot already on the Extent report is reused.
        /// Falls back to a viewport capture only when the step-level capture is missing AND
        /// the Dynamics hook is not the owner of this scenario — re-capturing in a pure-Dynamics
        /// run would shoot Browser 1 (unused IPAFFS browser) and overwrite the Dynamics image
        /// the Extent report already references.
        /// </summary>
        private void AttachScreenShotToXmlReport()
        {
            string fullScreenshotPath = null;

            if (_scenarioContext.ContainsKey(FailureScreenshotPathKey))
            {
                var stashed = _scenarioContext.Get<string>(FailureScreenshotPathKey);
                if (!string.IsNullOrWhiteSpace(stashed) && File.Exists(stashed))
                {
                    fullScreenshotPath = stashed;
                }
            }

            if (fullScreenshotPath == null)
            {
                var isDynamicsActive = _scenarioContext.ContainsKey("IsDynamicsActive")
                                       && _scenarioContext.Get<bool>("IsDynamicsActive");

                if (isDynamicsActive)
                {
                    // Defensive: AfterStepHooks should have written the screenshot at the canonical
                    // path. Reuse the file if it exists rather than re-capturing through ActiveDriver,
                    // which in a pure-Dynamics run is Browser 1 (wrong window).
                    var dynamicsPath = ScreenshotService.GetScreenshotPathForScenario(_scenarioContext.ScenarioInfo.Title);
                    if (File.Exists(dynamicsPath))
                    {
                        fullScreenshotPath = dynamicsPath;
                    }
                    else
                    {
                        // No screenshot is available and capturing here would shoot the wrong browser.
                        // Skip the attachment rather than corrupt the report.
                        return;
                    }
                }
                else
                {
                    // No step-level capture available — take a viewport screenshot as a fallback.
                    var relativePath = ScreenshotService.CaptureScreenshot(ActiveDriver, _scenarioContext.ScenarioInfo.Title);
                    if (string.IsNullOrWhiteSpace(relativePath))
                    {
                        return;
                    }

                    fullScreenshotPath = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "Reports",
                        relativePath.TrimStart('.', '/'));
                }
            }

            _reqnrollOutputHelper.AddAttachment(fullScreenshotPath);
            Logger.Debug($"SCREENSHOT {fullScreenshotPath}");
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
                // (Browser 2 IPAFFS tab after hand-off, Browser 1 before).
                // GridUrl enables full-page CDP capture when running against the dockerized Selenium Grid.
                var screenshotPath = ScreenshotService.CaptureScreenshotFullPage(
                    ActiveDriver,
                    _scenarioContext.ScenarioInfo.Title,
                    GridUrl);

                if (!string.IsNullOrWhiteSpace(screenshotPath))
                {
                    // Stash the absolute path so AfterScenario can reuse this full-page image for
                    // the TRX attachment instead of overwriting it with a viewport screenshot.
                    _scenarioContext[FailureScreenshotPathKey] = ScreenshotService.GetScreenshotPathForScenario(
                        _scenarioContext.ScenarioInfo.Title);

                    var stepNode = _scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                                 .Fail(_scenarioContext.TestError.Message)
                                 .AddScreenCaptureFromPath(screenshotPath);

                    var log = ScreenshotService.CreateLogForContextValues(_scenarioContext);
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

                    var log = ScreenshotService.CreateLogForContextValues(_scenarioContext);
                    if (!string.IsNullOrWhiteSpace(log) && log != "<pre></pre>")
                    {
                        stepNode.Info(log);
                    }
                }
            }

            Thread.Sleep(1000);
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