using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDPPReportsPage : ICHEDPPReportsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CHED-PP reports']"), true);
        private By byLnkPhsiImportsCommodityRulesReport => By.XPath("//a[normalize-space()='PHSI imports commodity rules report']");
        private By byLnkRiskDecisionReport => By.XPath("//a[normalize-space()='Risk decision report']");
        private By byLnkHmiImportsCommodityRulesReport => By.XPath("//a[normalize-space()='HMI imports commodity rules report']");
        private By byLnkHmiExportsCommodityRulesReport => By.XPath("//a[normalize-space()='HMI exports commodity rules report']");
        private IWebElement lnkCountryRulesReport => _driver.FindElement(By.XPath("//a[normalize-space()='Country rules report']"));
        #endregion

        public CHEDPPReportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-PP reports");

        public void ClickPHSIImportsCommodityRulesReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkPhsiImportsCommodityRulesReport);
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
            catch (WebDriverException ex) when (ex.Message.Contains("timed out"))
            {
                // The click succeeded and triggered navigation to a slow-loading page.
            }
        }

        public void ClickRiskDecisionReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkRiskDecisionReport);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
        }

        public void ClickHMIImportsCommodityRulesReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkHmiImportsCommodityRulesReport);
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
            catch (WebDriverException ex) when (ex.Message.Contains("timed out"))
            {
                // The click succeeded and triggered navigation to a slow-loading page.
            }
        }

        public void ClickHMIExportsCommodityRulesReportLink()
        {
            var element = _driver.WaitForElementClickable(byLnkHmiExportsCommodityRulesReport);
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
            catch (WebDriverException ex) when (ex.Message.Contains("timed out"))
            {
                // The click succeeded and triggered navigation to a slow-loading page.
            }
        }

        public void ClickCountryRulesReportLink() => lnkCountryRulesReport.Click();
    }
}