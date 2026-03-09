using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ContactDetailsPage : IContactDetailsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//*[@class='govuk-fieldset__heading']"), true);
        private IWebElement txtName => _driver.FindElement(By.Id("name"));
        private IWebElement txtEmailAddress => _driver.FindElement(By.Id("email"));
        private IWebElement txtMobileNumber => _driver.FindElement(By.Id("telephone"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ContactDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Trim().Contains("Contact details");
        }

        public bool ValidateIfContactDetailsArePopulated()
        {
            return !string.IsNullOrEmpty(txtName.GetAttribute("value")) &&
                !string.IsNullOrEmpty(txtEmailAddress.GetAttribute("value")) &&
                !string.IsNullOrEmpty(txtMobileNumber.GetAttribute("value"));
        }

        public string[] GetContactDetails()
        {
            return [txtName.GetAttribute("value"), txtEmailAddress.GetAttribute("value"), txtMobileNumber.GetAttribute("value")];
        }
    }
}