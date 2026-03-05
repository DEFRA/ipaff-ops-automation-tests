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
        private static bool _isPimsActive = false;

        public AfterStepHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 200)]
        public void BeforeScenario()
        {
            // Reset flags at start of each scenario
            _isPimsActive = false;
            _scenarioContext["IsPimsActive"] = false;
        }

        [AfterStep(Order = 100)]
        public void AfterStep()
        {
            // Client.BrowserInitiated checks if the browser has been launched WITHOUT creating it.
            // Accessing Driver directly would lazily instantiate a new browser.
            if (!_isPimsActive && Client.BrowserInitiated)
            {
                _isPimsActive = true;
                _scenarioContext["IsPimsActive"] = true;
            }

            if (!_isPimsActive)
            {
                return;
            }

            // Only log to the Extent report for PIMS steps that have failed
            if (_scenarioContext.TestError != null)
            {
                try
                {
                    var windowHandles = Driver.WindowHandles;
                    if (windowHandles != null && windowHandles.Count > 0 && _scenarioContext.ContainsKey("ExtentScenario"))
                    {
                        var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                        var stepInfo = _scenarioContext.StepContext.StepInfo.Text;
                        var screenshotPath = CaptureScreenshotForPIMS();
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
                // Log passing PIMS steps without a screenshot
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
                    if (!context.Key.Equals("ExtentScenario") && !context.Key.Equals("IsPimsActive"))
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
            var uniqueFileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(screenshotsDir, uniqueFileName);
            screenshot.SaveAsFile(filePath);

            // Return relative path so the Extent HTML report is portable when zipped
            return $"./Screenshots/{uniqueFileName}";
        }
    }
}