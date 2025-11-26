using Defra.UI.Tests.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace Defra.UI.Tests.Capabilities
{
    public class BrowserStackCapability : IDriverOptions
    {
        private BaseConfiguration _configuration => ConfigSetup.BaseConfiguration;
        private readonly ScenarioContext _scenarioContext;
        private readonly Dictionary<string, object> _capDictionary = [];
        private readonly Dictionary<string, object> _browserstackOptions = [];
        private static readonly string[] _osList = ["WINDOWS", "OS X"];

        private readonly string _target;
        private readonly string _deviceName;
        private readonly string _bs_os_version;
        private readonly string _bs_browser_version;

        public BrowserStackCapability(BaseConfiguration baseConfiguration, ScenarioContext context)
        {
            _scenarioContext = context;
            _target = _configuration.UiFrameworkConfiguration.Target;
            _deviceName = _configuration.TestConfiguration.DeviceName;
            _bs_os_version = _configuration.TestConfiguration.BSOSVersion;
            _bs_browser_version = _configuration.TestConfiguration.BSBrowserVersion;
        }

        public DriverOptions GetDriverOptions(Dictionary<string, string> capDictionary = null)
        {
            GetBrowserStackConfig();
            GetProjectDriverOptions();
            GetTestNameDriverOptions();

            _browserstackOptions.Add("acceptInsecureCerts", true);

            _capDictionary.Add("autoGrantPermission:", true);
            _capDictionary.Add("osVersion", _bs_os_version);

            if (_osList.Contains(_deviceName.ToUpper()))
            {
                _capDictionary.Add("os", _deviceName);
                _browserstackOptions.Add("os", _deviceName);
                _browserstackOptions.Add("browser", _target);
                _browserstackOptions.Add("browserVersion", _bs_browser_version);
            }
            else
            {
                _capDictionary.Add("deviceName", _deviceName);
                _browserstackOptions.Add("deviceName", _deviceName);
                _browserstackOptions.Add("browserName", _target);
                _browserstackOptions.Add("deviceOrientation", "portrait"); 
            }

            _browserstackOptions.Add("local", "false");

            var driverOptions = new ChromeOptions();
            AddDictionaryValuesInDriverOptions(driverOptions, _capDictionary);
            driverOptions.AddAdditionalOption("bstack:options", _browserstackOptions);

            return driverOptions;
        }

        private void GetBrowserStackConfig()
        {
            if (!_browserstackOptions.ContainsKey("debug"))
            {
                _browserstackOptions.Add("debug", true);
                _browserstackOptions.Add("userName", _configuration.BrowserStackConfiguration.CloudDeviceUserName);
                _browserstackOptions.Add("accessKey", _configuration.BrowserStackConfiguration.CloudDeviceUserKey);
                _browserstackOptions.Add("idleTimeout", 300);
            }

            _capDictionary.Add("acceptSslCerts", "true");
        }

        private void GetProjectDriverOptions()
        {
            if (!_browserstackOptions.ContainsKey("projectName"))
            {
                _browserstackOptions.Add("projectName", ConfigSetup.BaseConfiguration.TestConfiguration.Project);
                _browserstackOptions.Add("buildName", ConfigSetup.BaseConfiguration.TestConfiguration.Build);
            }
        }

        protected virtual void GetTestNameDriverOptions()
        {
            if (!_browserstackOptions.ContainsKey("sessionName"))
            {
                _browserstackOptions.Add("sessionName", TestContext.CurrentContext.Test.ClassName);
            }
        }

        private void AddDictionaryValuesInDriverOptions(DriverOptions driverOptions, Dictionary<string, object> capDictionary)
        {
            if (capDictionary != null)
            {
                foreach (var androidDictionary in capDictionary)
                {
                    driverOptions.AddAdditionalOption(androidDictionary.Key.ToString(), androidDictionary.Value);
                }
            }
        }
    }
}
