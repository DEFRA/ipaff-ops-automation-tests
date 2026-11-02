using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmWhatIsYourNamePage : IIdmWhatIsYourNamePage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What is your name?']"));
        private IWebElement txtFirstName => _driver.WaitForElement(By.Id("firstName"));
        private IWebElement txtLastName => _driver.WaitForElement(By.Id("lastName"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmWhatIsYourNamePage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void EnterNameAndContinue(string firstName, string lastName)
        {
            txtFirstName.Clear();
            txtFirstName.SendKeys(firstName);
            txtLastName.Clear();
            txtLastName.SendKeys(lastName);
            btnContinue.Click();
        }
    }
}
