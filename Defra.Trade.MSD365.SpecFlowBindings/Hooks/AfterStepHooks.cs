using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin;
using Capgemini.PowerApps.SpecFlowBindings;
using OpenQA.Selenium;
using Reqnroll;
using System.Reflection;
using System.Text;

namespace Defra.Trade.MSD365.SpecFlowBindings.Hooks
{
    [Binding]
    public class AfterStepHooks : PowerAppsStepDefiner
    {
        private readonly ScenarioContext _scenarioContext;
        private static bool _isDynamicsActive = false;

        public AfterStepHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 200)]
        public void BeforeScenario()
        {
            // Reset flags at start of each scenario
            _isDynamicsActive = false;
            _scenarioContext["IsDynamicsActive"] = false;
        }

        [AfterStep(Order = 100)]
        public void AfterStep()
        {
            // Only check BrowserInitiated AFTER the flag has been set via ScenarioContext.
            // Accessing Client.BrowserInitiated lazily instantiates WebClient, spawning a second browser.
            // Instead, rely solely on the ScenarioContext flag which is explicitly set by the login step.
            if (!_isDynamicsActive)
            {
                // Check the scenario context flag - this is set by the Dynamics login step
                // NOT by checking Client.BrowserInitiated which would spawn a second browser
                var isDynamicsActiveInContext = _scenarioContext.ContainsKey("IsDynamicsActive") &&
                                               _scenarioContext.Get<bool>("IsDynamicsActive");

                if (!isDynamicsActiveInContext)
                {
                    return;
                }

                _isDynamicsActive = true;
            }

            // Only log to the Extent report for Dynamics steps that have failed
            if (_scenarioContext.TestError != null)
            {
                try
                {
                    var windowHandles = Driver.WindowHandles;
                    if (windowHandles != null && windowHandles.Count > 0 && _scenarioContext.ContainsKey("ExtentScenario"))
                    {
                        var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                        var stepInfo = _scenarioContext.StepContext.StepInfo.Text;
                        var screenshotPath = CaptureScreenshotForDynamics();
                        var scenario = _scenarioContext.Get<ExtentTest>("ExtentScenario");

                        var stepNode = scenario.CreateNode(new GherkinKeyword(stepType), stepInfo)
                                               .Fail(_scenarioContext.TestError.Message)
                                               .AddScreenCaptureFromPath(screenshotPath);

                        var log = CreateLogForContextValues();
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
                // Log passing Dynamics steps without a screenshot
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

        private string CreateLogForContextValues()
        {
            var log = new StringBuilder("<pre>");
            try
            {
                foreach (var context in _scenarioContext)
                {
                    if (!context.Key.Equals("ExtentScenario") && !context.Key.Equals("IsDynamicsActive"))
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

        private static string FormatValue(object value)
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

        private string CaptureScreenshotForDynamics()
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
            catch { }

            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            var uniqueFileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(screenshotsDir, uniqueFileName);
            screenshot.SaveAsFile(filePath);

            return $"./Screenshots/{uniqueFileName}";
        }
    }
}