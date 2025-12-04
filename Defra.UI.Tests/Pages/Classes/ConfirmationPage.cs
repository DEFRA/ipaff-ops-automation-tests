using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;

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
            return chedReference.Text.Trim();
        }

        public string GetCustomsDeclarationReference()
        {
            return customsDeclarationReference.Text.Trim();
        }

        public string GetCustomsDocumentCode()
        {
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
    }
}