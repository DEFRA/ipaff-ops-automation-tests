using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CommodityRuleSubmittedPage : ICommodityRuleSubmittedPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-panel__title' and normalize-space()='Commodity rule submitted']"), true);
        private IWebElement viewAllCHEDAImportsCommodityRulesLink => _driver.WaitForElement(By.XPath("//a[@href='/Rules/View/Commodity/Import/CHEDA' and normalize-space()='View all CHED-A imports commodity rules']"), true);
        private IWebElement viewAllCHEDDImportsCommodityRulesLink => _driver.WaitForElement(By.XPath("//a[@href='/Rules/View/Commodity/Import/CHEDD' and normalize-space()='View all CHED-D imports commodity rules']"), true);
        #endregion

        public CommodityRuleSubmittedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Commodity rule submitted");

        public void ClickViewAllCHEDAImportsCommodityRulesLink() => viewAllCHEDAImportsCommodityRulesLink.Click();

        public void ClickViewAllCHEDDImportsCommodityRulesLink() => viewAllCHEDDImportsCommodityRulesLink.Click();
    }
}