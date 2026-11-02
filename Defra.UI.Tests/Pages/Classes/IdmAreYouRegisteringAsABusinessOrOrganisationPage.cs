using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmAreYouRegisteringAsABusinessOrOrganisationPage : IIdmAreYouRegisteringAsABusinessOrOrganisationPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Are you registering as a business or organisation?']"));
        private IWebElement lblOption(string option) => _driver.WaitForElement(By.XPath($"//label[normalize-space()='{option}']"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmAreYouRegisteringAsABusinessOrOrganisationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void SelectOptionAndContinue(string option)
        {
            var label = option.Equals("Yes", StringComparison.OrdinalIgnoreCase) ? "Yes, and I have permission to create an account" : option;
            _driver.WaitForElement(By.Id(lblOption(label).GetAttribute("for"))).Click();
            btnContinue.Click();
        }
    }
}
