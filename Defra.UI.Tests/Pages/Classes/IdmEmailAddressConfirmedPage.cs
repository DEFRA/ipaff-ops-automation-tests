using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmEmailAddressConfirmedPage : IIdmEmailAddressConfirmedPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Email address confirmed']"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("continue"));
        #endregion

        public IdmEmailAddressConfirmedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void ClickContinueButton() => btnContinue.Click();
    }
}
