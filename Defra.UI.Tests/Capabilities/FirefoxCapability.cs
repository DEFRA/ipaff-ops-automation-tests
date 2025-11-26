using Defra.UI.Tests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Reqnroll;

namespace Defra.UI.Tests.Capabilities
{
    public class FirefoxCapability : IDriverOptions
    {

        private readonly ScenarioContext _scenarioContext;

        public FirefoxCapability(BaseConfiguration baseConfiguration, ScenarioContext context)
        {
            _scenarioContext = context;
        }

        private static FirefoxOptions GetFirefoxOptions(List<string> arguments)
        {
            var firefoxOptions = new FirefoxOptions();
            firefoxOptions.AddArgument("--diable-inforbars");
            firefoxOptions.AddArgument("--disable-extensions");
            firefoxOptions.AddArgument("--start-maximized");
            firefoxOptions.AcceptInsecureCertificates = true;

            if (ConfigSetup.BaseConfiguration.TestConfiguration.Headless)
            {
                firefoxOptions.AddArgument("--headless");
            }

            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    if (!argument.Contains("accept_languages"))
                    {
                        firefoxOptions.AddArgument(argument);
                    }

                }
            }

            return firefoxOptions;

        }

        public DriverOptions GetDriverOptions(Dictionary<string, string> overrideCapDict = null)
        {
            var arguments = GetArgumentsFromOverrides(ref overrideCapDict);

            var driverOptions = GetFirefoxOptions(arguments);

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
