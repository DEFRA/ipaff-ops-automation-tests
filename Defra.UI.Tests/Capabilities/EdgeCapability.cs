using Defra.UI.Tests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Reqnroll;

namespace Defra.UI.Tests.Capabilities
{
    public class EdgeCapability : IDriverOptions
    {

        private readonly ScenarioContext _scenarioContext;

        public EdgeCapability(BaseConfiguration baseConfiguration, ScenarioContext context)
        {
            _scenarioContext = context;
        }

        private static EdgeOptions GetEdgeOptions(List<string> arguments)
        {
            var options = new EdgeOptions();
            options.AddArgument("start-maximized");
            options.AddArgument("disable-extensions");
            options.AddArgument("disable-gpu");

            options.AcceptInsecureCertificates = true;

            if (ConfigSetup.BaseConfiguration.TestConfiguration.Headless)
            {
                options.AddArgument("headless");
            }

            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    if (!argument.Contains("accept_languages"))
                    {
                        options.AddArgument(argument);
                    }
                }
            }

            return options;
        }

        public DriverOptions GetDriverOptions(Dictionary<string, string> overrideCapDict = null)
        {
            var arguments = GetArgumentsFromOverrides(ref overrideCapDict);

            var driverOptions = GetEdgeOptions(arguments);

            return driverOptions;
        }

        private List<string> GetArgumentsFromOverrides(ref Dictionary<string, string> overrideCapDict)
        {
            if (overrideCapDict == null || !overrideCapDict.ContainsKey(BrowserConfigurationValue.BrowserArguments))
            {
                return null;
            }

            List<string> args = [overrideCapDict[BrowserConfigurationValue.BrowserArguments]];

            overrideCapDict.Remove(BrowserConfigurationValue.BrowserArguments);

            return args;
        }
    }
}