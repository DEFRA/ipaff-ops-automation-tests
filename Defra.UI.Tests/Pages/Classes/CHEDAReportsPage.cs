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
        private IWebElement lnkImportsCommodityRulesReport => _driver.WaitForElement(By.XPath("//a[normalize-space()='Imports commodity rules report']"));
        private IWebElement lnkRiskDecisionReport => _driver.WaitForElement(By.XPath("//a[normalize-space()='Risk decision report']"));
        #endregion

        public CHEDAReportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-A reports");

        public void ClickImportsCommodityRulesReportLink() => lnkImportsCommodityRulesReport.Click();

        public void ClickRiskDecisionReportLink() => lnkRiskDecisionReport.Click();
    }
}