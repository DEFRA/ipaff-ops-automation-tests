using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmWhatWeNeedFromYouPage : IIdmWhatWeNeedFromYouPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What we need from you']"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));
        #endregion

        public IdmWhatWeNeedFromYouPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void ClickContinueButton() => btnContinue.Click();
    }
}
