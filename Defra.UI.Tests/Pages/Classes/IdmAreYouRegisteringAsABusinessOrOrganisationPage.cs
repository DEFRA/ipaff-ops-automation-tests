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

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Are you registering as a business or organisation?']"));
        private IWebElement rdoOption(string option) => _driver.WaitForElementExists(By.XPath($"//input[@type='radio' and @id=//label[normalize-space()='{option}']/@for]"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));
        #endregion

        public IdmAreYouRegisteringAsABusinessOrOrganisationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void SelectOptionAndContinue(string option)
        {
            var label = option.Equals("Yes", StringComparison.OrdinalIgnoreCase) ? "Yes, and I have permission to create an account" : option;
            rdoOption(label).Click();
            btnContinue.Click();
        }
    }
}
