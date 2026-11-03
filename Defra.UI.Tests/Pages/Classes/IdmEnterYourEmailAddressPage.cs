using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmEnterYourEmailAddressPage : IIdmEnterYourEmailAddressPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Enter your email address']"));
        private IWebElement txtEmail => _driver.WaitForElement(By.Id("emailAddress"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("continue"));
        #endregion

        public IdmEnterYourEmailAddressPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Displayed;
        public void EnterEmailAddress(string email) => txtEmail.SendKeys(email);
        public void ClickContinueButton() => btnContinue.Click();
    }
}
