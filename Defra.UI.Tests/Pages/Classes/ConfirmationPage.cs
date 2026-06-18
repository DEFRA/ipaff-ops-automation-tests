using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ConfirmationPage : IConfirmationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement chkDeclarationAgree => _driver.FindElement(By.Id("declaration-agree"));
        private IWebElement btnSubmit => _driver.WaitForElement(By.Id("accept-and-submit"));
        private IWebElement initialAssessmentTitle => _driver.WaitForElement(By.Id("risk-assessment-banner-title"));
        private IWebElement chedReference => _driver.WaitForElement(By.Id("reference-number"));
        private IWebElement customsDeclarationReference => _driver.WaitForElement(By.Id("reference-number-customs"));
        private IWebElement customsDocumentCode => _driver.WaitForElement(By.Id("reference-number-document"));
        private IWebElement signOut => _driver.WaitForElement(By.Id("sign-out-link"));
        private IWebElement logOutPageHeading => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl']"), true);
        private IWebElement lnkReturnToDashboard => _driver.FindElement(By.Id("manage-notifications"));
        private IWebElement lblBanner => _driver.FindElement(By.Id("border-notification-banner"));
        private IWebElement lnkReturnToDashboardLink => _driver.FindElement(By.Id("return-to-home-page"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ConfirmationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool VerifyInitialAssessmentPage()
        {
            return initialAssessmentTitle.Text.Trim().Contains("Initial risk assessment");
        }

        public string GetCHEDReference()
        {
            Console.WriteLine("[NOTIFICATION SUBMITTED] CHED Reference: " + chedReference.Text.Trim());
            return chedReference.Text.Trim();
        }

        public string GetCustomsDeclarationReference()
        {
            Console.WriteLine("[NOTIFICATION SUBMITTED] Customs Declaration Reference: " + customsDeclarationReference.Text.Trim());
            return customsDeclarationReference.Text.Trim();
        }

        public string GetCustomsDocumentCode()
        {
            Console.WriteLine("[NOTIFICATION SUBMITTED] Customs Document Code: " + customsDocumentCode.Text.Trim());
            return customsDocumentCode.Text.Trim();
        }

        public void SignedOut()
        {
            signOut.Click();
        }

        public bool VerifySignedOutPage()
        {
            return logOutPageHeading.Text.Contains("You have signed out")
               || logOutPageHeading.Text.Contains("Your Defra account");
        }

        public void ClickReturnToDashboard()
        {
            lnkReturnToDashboard.Click();
        }

        public bool VerifyBannerMessage(string message)
        {
            var bannerText = System.Text.RegularExpressions.Regex.Replace(lblBanner.Text.Trim(), @"\s+", " ").Trim();
            return bannerText.Contains(message);
        }

        public void ClickReturnToDashboardLink()
        {
            lnkReturnToDashboardLink.Click();
        }
    }
}