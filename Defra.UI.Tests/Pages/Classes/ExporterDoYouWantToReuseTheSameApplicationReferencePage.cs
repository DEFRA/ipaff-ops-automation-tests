using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterDoYouWantToReuseTheSameApplicationReferencePage : IExporterDoYouWantToReuseTheSameApplicationReferencePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Do you want to reuse the same application reference?']"), true);
        private IWebElement rdoYes => _driver.WaitForElementExists(By.Id("keep_reference_number_yes"), true);
        private IWebElement rdoNo => _driver.WaitForElementExists(By.Id("keep_reference_number_no"), true);
        private IWebElement txtReference => _driver.WaitForElement(By.Id("reference_number"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterDoYouWantToReuseTheSameApplicationReferencePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Do you want to reuse the same application reference?");

        public void SelectOption(string option)
        {
            if (option.Trim().StartsWith("Yes", StringComparison.OrdinalIgnoreCase))
                rdoYes.Click();
            else
                rdoNo.Click();
        }

        public void EnterNewReference(string reference)
        {
            txtReference.Clear();
            txtReference.SendKeys(reference);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}
