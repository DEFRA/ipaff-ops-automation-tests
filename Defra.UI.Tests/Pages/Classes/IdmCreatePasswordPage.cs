using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmCreatePasswordPage : IIdmCreatePasswordPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Create a password']"));
        private IWebElement txtPassword => _driver.WaitForElement(By.Id("newPassword"));
        private IWebElement txtConfirmPassword => _driver.WaitForElement(By.Id("confirmPassword"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("continue"));
        #endregion

        public IdmCreatePasswordPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void EnterPasswordAndContinue(string password)
        {
            txtPassword.SendKeys(password);
            txtConfirmPassword.SendKeys(password);
            btnContinue.Click();
        }
    }
}
