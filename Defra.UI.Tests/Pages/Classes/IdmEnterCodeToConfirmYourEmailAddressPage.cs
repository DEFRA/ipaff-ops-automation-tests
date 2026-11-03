using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmEnterCodeToConfirmYourEmailAddressPage : IIdmEnterCodeToConfirmYourEmailAddressPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("enterCodeTitle"));
        private IWebElement txtCode => _driver.WaitForElement(By.Id("code"));
        private IWebElement btnConfirm => _driver.WaitForElementClickable(By.Id("continue"));
        #endregion

        public IdmEnterCodeToConfirmYourEmailAddressPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Text.Contains("Enter code to confirm your email address");
        public void EnterCodeAndConfirm(string code)
        {
            txtCode.SendKeys(code);
            btnConfirm.Click();
        }
    }
}
