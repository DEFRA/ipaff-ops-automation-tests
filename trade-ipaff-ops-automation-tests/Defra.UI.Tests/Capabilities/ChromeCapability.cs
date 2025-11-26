
using Defra.UI.Tests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace Defra.UI.Tests.Capabilities
{
    public class ChromeCapability : IDriverOptions
    {

        private static ScenarioContext _scenarioContext;

        public ChromeCapability(BaseConfiguration baseConfiguration, ScenarioContext context)
        {
            _scenarioContext = context;
        }

        private static ChromeOptions GetChromeOptions(List<string> arguments)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--diable-inforbars");
            chromeOptions.AddArgument("--start-maximized");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AcceptInsecureCertificates = true;


            if (ConfigSetup.BaseConfiguration.TestConfiguration.Headless)
            {
                chromeOptions.AddArgument("--headless");
            }

            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    if (!argument.Contains("accept_languages"))
                    {
                        chromeOptions.AddArgument(argument);
                    }

                }
            }

            if (ConfigSetup.BaseConfiguration.TestConfiguration.IsEmulationEnabled)
            {
                SetChromiumDevice(chromeOptions);
            }

            return chromeOptions;

        }
        private static void SetChromiumDevice(ChromeOptions chromeOptions)
        {
            chromeOptions.EnableMobileEmulation(ConfigSetup.BaseConfiguration.TestConfiguration.EmulateDeviceInfo);
        }

        public DriverOptions GetDriverOptions(Dictionary<string, string> overrideCapDict = null)
        {
            var arguments = GetArgumentsFromOverrides(ref overrideCapDict);

            var driverOptions = GetChromeOptions(arguments);

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
