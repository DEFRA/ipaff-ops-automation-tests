using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmWhatIsYourPhoneNumberPage : IIdmWhatIsYourPhoneNumberPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What is your phone number?']"));
        private IWebElement txtPhone => _driver.WaitForElement(By.Id("telephoneNumber"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmWhatIsYourPhoneNumberPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void EnterPhoneAndContinue(string phone)
        {
            txtPhone.SendKeys(phone);
            btnContinue.Click();
        }
    }
}
