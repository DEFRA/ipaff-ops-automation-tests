using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin;
using Capgemini.PowerApps.SpecFlowBindings;
using OpenQA.Selenium;
using Reqnroll;
using System.Reflection;

namespace Defra.Trade.MSD365.SpecFlowBindings.Hooks
{
    [Binding]
    public class AfterStepHooks : PowerAppsStepDefiner
    {
        private readonly ScenarioContext _scenarioContext;
        private static bool _isPimsActive = false;

        public AfterStepHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 200)]
        public void BeforeScenario()
        {
            // Reset flag at start of each scenario
            _isPimsActive = false;
        }

        [AfterStep(Order = 100)]
        public void AfterStep()
        {
            var stepText = _scenarioContext.StepContext.StepInfo.Text;

            bool isPimsStep = stepText.Contains("PIMS", StringComparison.OrdinalIgnoreCase) ||
                             stepText.Contains("logged in to the", StringComparison.OrdinalIgnoreCase) ||
                             stepText.Contains("sub area", StringComparison.OrdinalIgnoreCase) ||
                             stepText.Contains("Importer Notifications", StringComparison.OrdinalIgnoreCase) ||
                             stepText.Contains("open the record", StringComparison.OrdinalIgnoreCase) ||
                             stepText.Contains("in the grid", StringComparison.OrdinalIgnoreCase);

            if (isPimsStep)
            {
                _isPimsActive = true;
            }

            if (_isPimsActive && Driver != null)
            {
                try
                {
                    var windowHandles = Driver.WindowHandles;
                    if (windowHandles != null && windowHandles.Count > 0)
                    {
                        var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                        var stepInfo = _scenarioContext.StepContext.StepInfo.Text;

                        if (_scenarioContext.ContainsKey("ExtentScenario"))
                        {
                            var screenshotPath = CaptureScreenshotForPIMS();
                            var scenario = _scenarioContext.Get<ExtentTest>("ExtentScenario");

                            if (_scenarioContext.TestError == null)
                            {
                                scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                                    .Pass("Step passed")
                                    .AddScreenCaptureFromPath(screenshotPath);
                            }
                            else
                            {
                                scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                                         .Fail(_scenarioContext.TestError.Message)
                                         .AddScreenCaptureFromPath(screenshotPath);
                            }
                        }
                    }
                }
                // Silently handle screenshot errors
                catch { }
            }
        }

        private string CaptureScreenshotForPIMS()
        {
            var screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Reports", "Screenshots");

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            try
            {
                var handles = Driver.WindowHandles;
                if (handles.Count > 0)
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
            // Silently handle window switch errors
            catch { }

            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            var filePath = Path.Combine(screenshotsDir, $"{Guid.NewGuid()}.png");
            screenshot.SaveAsFile(filePath);
            return filePath;
        }
    }
}