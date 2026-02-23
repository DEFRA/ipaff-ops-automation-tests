using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ConfirmationToDeclareGMSPage : IConfirmationToDeclareGMSPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement confirmationOptionYes => _driver.FindElement(By.Id("declaration-accepted-yes"));
        private IWebElement confirmationOptionNo => _driver.FindElement(By.Id("declaration-accepted-no"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ConfirmationToDeclareGMSPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Confirmation to declare GMS");
        }

        public void SelectConfirmationOption(string option)
        {
            if (option.Equals("Yes"))
                confirmationOptionYes.Click();
            else
                confirmationOptionNo.Click();
        }
    }
}