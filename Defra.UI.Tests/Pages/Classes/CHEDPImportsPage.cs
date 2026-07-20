using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDPImportsPage : ICHEDPImportsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CHED-P imports']"), true);
        private IWebElement lnkBulkUploadCommodityRules => _driver.WaitForElement(By.XPath("//a[normalize-space()='Bulk upload commodity rules']"));
        private IWebElement lnkIndividualCommodityRules => _driver.WaitForElement(By.XPath("//a[normalize-space()='Individual commodity rules']"));
        private IWebElement lnkCountryRules => _driver.WaitForElement(By.XPath("//a[normalize-space()='Country rules']"));
        #endregion

        public CHEDPImportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-P imports");

        public void ClickBulkUploadCommodityRulesLink() => lnkBulkUploadCommodityRules.Click();

        public void ClickIndividualCommodityRulesLink() => lnkIndividualCommodityRules.Click();

        public void ClickCountryRulesLink() => lnkCountryRules.Click();
    }
}