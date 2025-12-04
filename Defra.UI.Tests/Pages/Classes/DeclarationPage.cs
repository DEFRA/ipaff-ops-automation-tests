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
    public class DeclarationPage : IDeclarationPage
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
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public DeclarationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Declaration");
        }

        public void CheckDeclarationAgreement()
        {
            chkDeclarationAgree.Click();
        }

        public void ClickSubmitNotification()
        {
            btnSubmit.Click();
        }        
    }
}