using Reqnroll.BoDi;
using Defra.UI.Tests.Capabilities;
using Defra.UI.Tests.Configuration;
using Reqnroll;

namespace Defra.UI.Tests.Hooks
{
    [Binding]
    public class CapabilityHook
    {

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        public CapabilityHook(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = (int)HookRunOrder.Capability)]
        public void BeforeScenario()
        {
            var seleniumGrid = ConfigSetup.BaseConfiguration.UiFrameworkConfiguration.SeleniumGrid;

            if (seleniumGrid.Contains("browserstack", StringComparison.InvariantCultureIgnoreCase))
            {
                _objectContainer.RegisterInstanceAs(LoadCapabilityClass<BrowserStackCapability, IDriverOptions>());
            }
            else if (seleniumGrid.Contains("localhost"))
            {
                var browser = ConfigSetup.BaseConfiguration.UiFrameworkConfiguration.Target;

                switch (browser.ToUpper())
                {
                    case "CHROME":
                        _objectContainer.RegisterInstanceAs(LoadCapabilityClass<ChromeCapability, IDriverOptions>());
                        break;
                    case "FIREFOX":
                        _objectContainer.RegisterInstanceAs(LoadCapabilityClass<FirefoxCapability, IDriverOptions>());
                        break;
                    case "EDGE":
                        _objectContainer.RegisterInstanceAs(LoadCapabilityClass<EdgeCapability, IDriverOptions>());
                        break;
                    default:
                        _objectContainer.RegisterInstanceAs(LoadCapabilityClass<ChromeCapability, IDriverOptions>());
                        break;
                }
            }
            else
            {
                _objectContainer.RegisterInstanceAs(LoadCapabilityClass<ChromeCapability, IDriverOptions>());
            }
        }

        private TU LoadCapabilityClass<T, TU>() where T : TU =>
            (TU)Activator.CreateInstance(typeof(T), ConfigSetup.BaseConfiguration, _scenarioContext);

    }
}
