using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmWhatIsYourFullNamePage : IIdmWhatIsYourFullNamePage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.CssSelector("label[for='name']"));
        private IWebElement txtName => _driver.WaitForElement(By.Id("name"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("continue"));

        public IdmWhatIsYourFullNamePage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Text.Contains("What is your full name?");
        public void EnterNameAndContinue(string name)
        {
            txtName.SendKeys(name);
            btnContinue.Click();
        }
    }
}
