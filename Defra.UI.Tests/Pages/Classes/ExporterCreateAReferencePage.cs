using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterCreateAReferencePage : IExporterCreateAReferencePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Create a reference']"), true);
        private IWebElement txtReference => _driver.WaitForElement(By.Id("application-reference"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterCreateAReferencePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Create a reference");

        public void EnterReference(string reference)
        {
            txtReference.Clear();
            txtReference.SendKeys(reference);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}