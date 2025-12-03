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
        private IWebElement initialAssessmentTitle => _driver.WaitForElement(By.Id("risk-assessment-banner-title"));
        private IWebElement chedReference => _driver.WaitForElement(By.Id("reference-number"));
        private IWebElement customsDeclarationReference => _driver.WaitForElement(By.Id("reference-number-customs"));
        private IWebElement customsDocumentCode => _driver.WaitForElement(By.Id("reference-number-document"));
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
            Console.WriteLine("CHED Reference: " + chedReference.Text.Trim());
            return chedReference.Text.Trim();
        }

        public string GetCustomsDeclarationReference()
        {
            Console.WriteLine("customsDeclarationReference: " + customsDeclarationReference.Text.Trim());
            return customsDeclarationReference.Text.Trim();
        }

        public string GetCustomsDocumentCode()
        {
            Console.WriteLine("customsDocumentCode: " + customsDocumentCode.Text.Trim());
            return customsDocumentCode.Text.Trim();
        }
    }
}