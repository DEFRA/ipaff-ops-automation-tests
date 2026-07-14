using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDAReportsPage : ICHEDAReportsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CHED-A reports']"), true);
        private By byLnkImportsCommodityRulesReport => By.XPath("//a[normalize-space()='Imports commodity rules report']");
        private By byLnkRiskDecisionReport => By.XPath("//a[normalize-space()='Risk decision report']");
        private By byLnkCountryRulesReport => By.XPath("//a[normalize-space()='Country rules report']");
        #endregion

        public CHEDAReportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-A reports");

        public void ClickImportsCommodityRulesReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkImportsCommodityRulesReport);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
        }

        public void ClickRiskDecisionReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkRiskDecisionReport);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
        }

        public void ClickCountryRulesReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkCountryRulesReport);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
        }
    }
}