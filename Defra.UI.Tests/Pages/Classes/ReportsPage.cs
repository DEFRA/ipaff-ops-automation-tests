using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReportsPage : IReportsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Reports']"), true);
        private IWebElement lnkChedPPReports => _driver.WaitForElement(By.XPath("//a[normalize-space()='CHED-PP reports']"));
        private IWebElement lnkChedAReports => _driver.WaitForElement(By.XPath("//a[normalize-space()='CHED-A reports']"));
        private IWebElement lnkChedDReports => _driver.WaitForElement(By.XPath("//a[normalize-space()='CHED-D reports']"));
        #endregion

        public ReportsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Reports");

        public void ClickChedPPReportsLink() => lnkChedPPReports.Click();

        public void ClickChedAReportsLink() => lnkChedAReports.Click();

        public void ClickChedDReportsLink() => lnkChedDReports.Click();
    }
}