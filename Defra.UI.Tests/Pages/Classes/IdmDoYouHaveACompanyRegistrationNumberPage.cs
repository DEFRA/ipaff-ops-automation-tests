using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmDoYouHaveACompanyRegistrationNumberPage : IIdmDoYouHaveACompanyRegistrationNumberPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Do you have a company registration number?']"));
        private IWebElement rdoOption(string option) => _driver.WaitForElementExists(By.XPath($"//input[@type='radio' and @id=//label[normalize-space()='{option}']/@for]"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmDoYouHaveACompanyRegistrationNumberPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void SelectOptionAndContinue(string option)
        {
            rdoOption(option).Click();
            btnContinue.Click();
        }
    }
}
