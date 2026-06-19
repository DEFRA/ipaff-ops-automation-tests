using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDDImportsPage : ICHEDDImportsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CHED-D imports']"), true);
        private IWebElement lnkBulkUploadCommodityRules => _driver.WaitForElement(By.XPath("//a[normalize-space()='Bulk upload commodity rules']"));
        private IWebElement lnkImportsCommodityRulesReport => _driver.WaitForElement(By.XPath("//a[normalize-space()='Imports commodity rules report']"));
        #endregion

        public CHEDDImportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-D imports");

        public void ClickBulkUploadCommodityRulesLink() => lnkBulkUploadCommodityRules.Click();

        public void ClickImportsCommodityRulesReportLink() => lnkImportsCommodityRulesReport.Click();
    }
}