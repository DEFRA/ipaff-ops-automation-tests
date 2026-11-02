using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmWhatKindOfBusinessOrOrganisationAreYouPage : IIdmWhatKindOfBusinessOrOrganisationAreYouPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What kind of business or organisation are you?']"));
        private IWebElement lblOption(string option) => _driver.WaitForElement(By.XPath($"//label[normalize-space()='{option}']"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmWhatKindOfBusinessOrOrganisationAreYouPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void SelectOptionAndContinue(string option)
        {
            _driver.WaitForElement(By.Id(lblOption(option).GetAttribute("for"))).Click();
            btnContinue.Click();
        }
    }
}
