using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterAreYouSureYouWantToCopyThisApplicationPage : IExporterAreYouSureYouWantToCopyThisApplicationPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Are you sure you want to copy this application?']"), true);
        private IWebElement rdoYes => _driver.WaitForElementExists(By.Id("proceedClone"), true);
        private IWebElement rdoNo => _driver.WaitForElementExists(By.Id("cancelClone"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("Clone-Continue"));
        #endregion

        public ExporterAreYouSureYouWantToCopyThisApplicationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Are you sure you want to copy this application?");

        public void SelectOption(string option)
        {
            if (option.Trim().StartsWith("Yes", StringComparison.OrdinalIgnoreCase))
                rdoYes.Click();
            else
                rdoNo.Click();
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}
