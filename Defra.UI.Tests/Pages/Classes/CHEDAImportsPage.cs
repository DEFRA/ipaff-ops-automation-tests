using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDAImportsPage : ICHEDAImportsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CHED-A imports']"), true);
        private IWebElement lnkBulkUploadCommodityRules => _driver.WaitForElement(By.XPath("//a[normalize-space()='Bulk upload commodity rules']"));
        private IWebElement lnkImportsCommodityRulesReport => _driver.WaitForElement(By.XPath("//a[normalize-space()='Imports commodity rules report']"));
        private IWebElement lnkIndividualCommodityRules => _driver.WaitForElement(By.XPath("//a[normalize-space()='Individual commodity rules']"));
        #endregion

        public CHEDAImportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-A imports");

        public void ClickBulkUploadCommodityRulesLink() => lnkBulkUploadCommodityRules.Click();

        public void ClickImportsCommodityRulesReportLink() => lnkImportsCommodityRulesReport.Click();

        public void ClickIndividualCommodityRulesLink() => lnkIndividualCommodityRules.Click();
    }
}