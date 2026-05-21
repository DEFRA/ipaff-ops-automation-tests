using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ThereAreRulesInYourCSVFileThatAlreadyExistPage : IThereAreRulesInYourCSVFileThatAlreadyExistPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement notificationBannerHeading => _driver.WaitForElement(By.XPath("//div[contains(@class,'govuk-notification-banner')]//h2[contains(@class,'govuk-notification-banner__heading') and normalize-space()='There are rules in your CSV file that already exist']"), true);
        private IWebElement yesRadioButton => _driver.FindElement(By.Id("replacerules"));
        private IWebElement continueButton => _driver.FindElement(By.XPath("//button[@type='submit' and normalize-space()='Continue']"));
        #endregion

        public ThereAreRulesInYourCSVFileThatAlreadyExistPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => notificationBannerHeading.Displayed;

        public void SelectYesAndClickContinue()
        {
            yesRadioButton.Click();
            continueButton.Click();
        }
    }
}