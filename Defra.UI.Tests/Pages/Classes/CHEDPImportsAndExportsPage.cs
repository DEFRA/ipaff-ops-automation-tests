using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDPImportsAndExportsPage : ICHEDPImportsAndExportsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CHED-P imports and exports']"), true);
        private IWebElement lnkBulkUploadCommodityRules => _driver.WaitForElement(By.XPath("//a[normalize-space()='Bulk upload commodity rules']"));
        #endregion

        public CHEDPImportsAndExportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-P imports and exports");

        public void ClickBulkUploadCommodityRulesLink() => lnkBulkUploadCommodityRules.Click();
    }
}