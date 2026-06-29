using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin;
using Capgemini.PowerApps.SpecFlowBindings;
using Defra.UI.Tests.Shared.Tools;
using OpenQA.Selenium;
using Reqnroll;

namespace Defra.Trade.MSD365.SpecFlowBindings.Hooks
{
    [Binding]
    public class AfterStepHooks : PowerAppsStepDefiner
    {
        // Must match WebDriverHook.FailureScreenshotPathKey — both hooks live in different
        // assemblies and communicate the failure screenshot path via ScenarioContext.
        private const string FailureScreenshotPathKey = "FailureScreenshotPath";

        private readonly ScenarioContext _scenarioContext;

        public AfterStepHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 200)]
        public void BeforeScenario()
        {
            _scenarioContext["IsDynamicsActive"] = false;
        }

        [AfterStep(Order = 100)]
        public void AfterStep()
        {
            // Always read directly from ScenarioContext — the flag can be toggled
            // mid-scenario (e.g. set false when handing off to the IPAFFS tab,
            // restored to true when switching back to the Dynamics tab).
            var isDynamicsActive = _scenarioContext.ContainsKey("IsDynamicsActive")
                                   && _scenarioContext.Get<bool>("IsDynamicsActive");

            if (!isDynamicsActive)
            {
                return;
            }

            if (_scenarioContext.TestError != null)
            {
                try
                {
                    var windowHandles = Driver.WindowHandles;
                    if (windowHandles != null && windowHandles.Count > 0 && _scenarioContext.ContainsKey("ExtentScenario"))
                    {
                        var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                        var stepInfo = _scenarioContext.StepContext.StepInfo.Text;
                        var screenshotPath = ScreenshotService.CaptureScreenshot(Driver, _scenarioContext.ScenarioInfo.Title);
                        var scenario = _scenarioContext.Get<ExtentTest>("ExtentScenario");

                        var stepNode = scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                                               .Fail(_scenarioContext.TestError.Message);

                        if (!string.IsNullOrWhiteSpace(screenshotPath))
                        {
                            stepNode.AddScreenCaptureFromPath(screenshotPath);

                            // Stash the absolute path so WebDriverHook.AfterScenario reuses this
                            // Dynamics screenshot for the TRX attachment. Without this, the fallback
                            // recapture in AttachScreenShotToXmlReport would re-shoot via ActiveDriver
                            // (Browser 1, parked on IPAFFS/blank in pure-Dynamics runs) and overwrite
                            // this file — so the Extent report would render the wrong image.
                            _scenarioContext[FailureScreenshotPathKey] =
                                ScreenshotService.GetScreenshotPathForScenario(_scenarioContext.ScenarioInfo.Title);
                        }

                        var log = ScreenshotService.CreateLogForContextValues(_scenarioContext);
                        if (!string.IsNullOrWhiteSpace(log) && log != "<pre></pre>")
                        {
                            stepNode.Info(log);
                        }
                    }
                }
                catch { }
            }
            else
            {
                try
                {
                    var windowHandles = Driver.WindowHandles;
                    if (windowHandles != null && windowHandles.Count > 0 && _scenarioContext.ContainsKey("ExtentScenario"))
                    {
                        var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                        var stepInfo = _scenarioContext.StepContext.StepInfo.Text;
                        var scenario = _scenarioContext.Get<ExtentTest>("ExtentScenario");

                        scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                                .Pass("Step passed");
                    }
                }
                catch { }
            }
        }
    }
}