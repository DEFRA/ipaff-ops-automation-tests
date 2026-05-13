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
        private IWebElement lnkPhsiImportsCommodityRulesReport => _driver.FindElement(By.XPath("//a[normalize-space()='PHSI imports commodity rules report']"));
        private IWebElement lnkRiskDecisionReport => _driver.FindElement(By.XPath("//a[normalize-space()='Risk decision report']"));
        #endregion

        public CHEDPPReportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-PP reports");

        public void ClickPHSIImportsCommodityRulesReportLink() => lnkPhsiImportsCommodityRulesReport.Click();

        public void ClickRiskDecisionReportLink() => lnkRiskDecisionReport.Click();
    }
}