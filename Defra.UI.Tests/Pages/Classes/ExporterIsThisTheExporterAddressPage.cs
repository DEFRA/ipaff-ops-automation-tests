using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterIsThisTheExporterAddressPage : IExporterIsThisTheExporterAddressPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Is this the exporter address?']"), true);
        private IWebElement rdoYes => _driver.WaitForElementExists(By.Id("default_exporter_address-yes"), true);
        private IWebElement rdoNo => _driver.WaitForElementExists(By.Id("default_exporter_address-no"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("ExporterAddressConfirmation-Continue"));
        #endregion

        public ExporterIsThisTheExporterAddressPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Is this the exporter address?");

        public void SelectOption(string option)
        {
            if (option.Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase))
                rdoYes.Click();
            else
                rdoNo.Click();
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}
