using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class BillingDetailsPage : IBillingDetailsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IReadOnlyCollection<IWebElement> primaryTitle => _driver.WaitForElements(By.Id("page-primary-title"), true);
        private IReadOnlyCollection<IWebElement> secondaryTitle => _driver.WaitForElements(By.Id("page-secondary-title"), true);
        private IReadOnlyCollection<IWebElement> btnSaveAndContinueList => _driver.FindElements(By.XPath("//button[text()='Save and continue']"));
        private IWebElement lnkRatesAndEligibility => _driver.FindElement(By.Id("read-rates"));
        private IWebElement lnkTermsAndConditions => _driver.FindElement(By.Id("read-terms"));
        private IWebElement ratesPageTitle => _driver.FindElement(By.XPath("//h1[@class='gem-c-heading__text govuk-heading-l']"));
        private IWebElement termsAndConditionsPageTitle => _driver.FindElement(By.Id("read-rates"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public BillingDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            if (secondaryTitle.Count > 0 && primaryTitle.Count > 0)
            {
                return secondaryTitle.FirstOrDefault().Text.Contains("Billing")
                    && primaryTitle.FirstOrDefault().Text.Contains("Confirm billing details");
            }
            return true;
        }

        public void ClickSaveAndContinue()
        {
            if (btnSaveAndContinueList.Count > 0)
            {
                btnSaveAndContinueList.FirstOrDefault().Click();
            }
        }

        public void ClickRatesAndEligibilityLink()
        {
            lnkRatesAndEligibility.Click();
        }

        public bool VerifyPageOpensInNewTab(string pageName)
        {
            var windowHandles = _driver.WindowHandles;
            if (windowHandles.Count > 1)
            {
                _driver.SwitchTo().Window(windowHandles.Last());
                return ratesPageTitle.Text.Trim().Contains(pageName);
            }
            return false;
        }

        public void CloseTheNewTab()
        {
            var windowHandles = _driver.WindowHandles;
            if (windowHandles.Count > 1)
            {
                _driver.Close();
                _driver.SwitchTo().Window(windowHandles.First());
            }
        }

        public bool VerifyNewTabClosed()
        {
            return _driver.WindowHandles.Count == 1;
        }

        public void ClickTermsAndConditionsLink()
        {
            lnkTermsAndConditions.Click();
        }
    }
}