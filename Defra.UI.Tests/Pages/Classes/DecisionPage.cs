using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DecisionPage : IDecisionPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement acceptableForRadio(string acceptableForRadioOption) => _driver.FindElement(By.XPath($"//label[contains(text(),'{acceptableForRadioOption}')]/preceding-sibling::input"));
        private IWebElement internalMarketSubRadio(string internalMarketSubOption) => _driver.FindElement(By.XPath($"//label[contains(text(),'{internalMarketSubOption}')]/preceding-sibling::input"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public DecisionPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Decision");
        }

        public bool IsAcceptableForRadioSelected(string acceptableForRadioOption)
        {
            return acceptableForRadio(acceptableForRadioOption).GetAttribute("aria-expanded").Contains("true");
        }

        public bool IsInternalMarketSubRadioSelected(string internalMarketSubOption)
        {
            return internalMarketSubRadio(internalMarketSubOption).GetAttribute("checked") != null;
        }
    }
}