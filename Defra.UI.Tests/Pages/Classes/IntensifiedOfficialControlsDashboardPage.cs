using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IntensifiedOfficialControlsDashboardPage : IIntensifiedOfficialControlsDashboardPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement btnCreateNewIntensifiedControlCheck => _driver.FindElement(By.Id("create-re-enforced-check"));
        private IWebElement lnkSignOut => _driver.WaitForElement(By.Id("sign-out-link"));
        private IWebElement GetViewDetailsLinkByIOCNumber(string iocNumber) => _driver.WaitForElement(By.Id(iocNumber));
        private IWebElement GetIOCRowContainer(string iocNumber) => GetViewDetailsLinkByIOCNumber(iocNumber).FindElement(By.XPath("ancestor::dl[contains(@class,'reenforcedcheck-list__row')]"));
        private IWebElement GetIOCStatusElement(string iocNumber) => GetIOCRowContainer(iocNumber).FindElement(By.XPath(".//dt[normalize-space()='Status']/following-sibling::dd[1]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public IntensifiedOfficialControlsDashboardPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Intensified official controls");
        }

        public void ClickCreateNewIntensifiedControlCheck()
        {
            btnCreateNewIntensifiedControlCheck.Click();
        }

        public string GetStatusForIOCNumber(string iocNumber)
        {
            return GetIOCStatusElement(iocNumber).Text.Trim();
        }

        public void ClickSignOut()
        {
            lnkSignOut.Click();
        }

        #endregion
    }
}