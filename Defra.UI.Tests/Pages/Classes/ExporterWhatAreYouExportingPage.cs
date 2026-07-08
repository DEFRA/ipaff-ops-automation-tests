using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhatAreYouExportingPage : IExporterWhatAreYouExportingPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What are you exporting?']"), true);
        private IWebElement optionLabel(string exportType) => _driver.WaitForElement(By.XPath($"//label[contains(normalize-space(),'{exportType}')]"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("choose-form-continue-button"));
        #endregion

        public ExporterWhatAreYouExportingPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("What are you exporting?");

        public void SelectExportType(string exportType) => optionLabel(exportType).Click();

        public void ClickContinueButton() => btnContinue.Click();
    }
}