using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class TheAddressHasBeenAddedPage : ITheAddressHasBeenAddedPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-panel__title']"), true);
        private IWebElement btnReturnToAddressBook => _driver.WaitForElement(By.XPath("//button[@type='submit' and @value='Return to address book']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public TheAddressHasBeenAddedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("The address has been added to your address book");
        }

        public void ClickReturnToAddressBook()
        {
            btnReturnToAddressBook.Click();
        }
    }
}