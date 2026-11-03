using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmWhatAreTheBusinessContactDetailsPage : IIdmWhatAreTheBusinessContactDetailsPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What are the business contact details?']"));
        private IWebElement txtPhone => _driver.WaitForElement(By.Id("companyTelephoneNumber"));
        private IWebElement txtEmail => _driver.WaitForElement(By.Id("companyEmailAddress"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));
        #endregion

        public IdmWhatAreTheBusinessContactDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void EnterContactDetailsAndContinue(string phone, string email)
        {
            var phoneInput = txtPhone;
            phoneInput.Clear();
            phoneInput.SendKeys(phone);
            var emailInput = txtEmail;
            emailInput.Clear();
            emailInput.SendKeys(email);
            btnContinue.Click();
        }
    }
}
