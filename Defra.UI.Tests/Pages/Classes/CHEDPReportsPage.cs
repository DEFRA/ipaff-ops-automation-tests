using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDPReportsPage : ICHEDPReportsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CHED-P reports']"), true);
        private By byLnkImportsCommodityRulesReport => By.XPath("//a[normalize-space()='Imports commodity rules report']");
        private By byLnkRiskDecisionReport => By.XPath("//a[normalize-space()='Risk decision report']");
        private By byLnkCountryRulesReport => By.XPath("//a[normalize-space()='Country rules report']");
        #endregion

        public CHEDPReportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-P reports");

        public void ClickImportsCommodityRulesReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkImportsCommodityRulesReport);
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
            catch (WebDriverException ex) when (ex.Message.Contains("timed out"))
            {
                // The click succeeded and triggered navigation to a slow-loading page.
                // The /execute/sync command times out waiting for page load, but the
                // navigation is already in progress — safe to continue.
            }
        }

        public void ClickRiskDecisionReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkRiskDecisionReport);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
        }

        public void ClickCountryRulesReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkCountryRulesReport);
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
            catch (WebDriverException ex) when (ex.Message.Contains("timed out"))
            {
                // The click succeeded and triggered navigation to a slow-loading page.
                // The /execute/sync command times out waiting for page load, but the
                // navigation is already in progress — safe to continue.
            }
        }
    }
}