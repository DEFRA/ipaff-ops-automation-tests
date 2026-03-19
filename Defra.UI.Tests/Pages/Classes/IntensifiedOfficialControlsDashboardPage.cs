using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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
        private IWebElement txtSearchCommodity => _driver.FindElement(By.Id("search-commodity"));
        private IWebElement drpSearchStatus => _driver.FindElement(By.Id("search-status"));
        private IWebElement btnSearchNotifications => _driver.FindElement(By.Id("search-notifications"));
        private IWebElement lnkFirstViewDetails => _driver.FindElement(By.XPath("(//div[contains(@class,'reenforcedcheck-list__links')]//a)[1]"));
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

        public void FilterByStatusAndCommodity(string status, string commodity)
        {
            if (!string.IsNullOrEmpty(status))
            {
                new SelectElement(drpSearchStatus).SelectByText(status);
            }

            txtSearchCommodity.Clear();
            txtSearchCommodity.SendKeys(commodity);

            btnSearchNotifications.Click();
        }

        public bool HasSearchResults()
        {
            try
            {
                return lnkFirstViewDetails.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void ClickFirstResult()
        {
            lnkFirstViewDetails.Click();
        }

        public void ClickViewDetailsForIOCNumber(string iocNumber)
        {
            GetViewDetailsLinkByIOCNumber(iocNumber).Click();
        }

        #endregion
    }
}