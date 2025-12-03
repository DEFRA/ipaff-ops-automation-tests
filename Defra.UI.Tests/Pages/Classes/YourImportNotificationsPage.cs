using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class YourImportNotificationsPage : IYourImportNotificationsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.XPath("//h1[@id='page-primary-title']"), true);
        private IWebElement lnkCreateNotification => _driver.WaitForElement(By.LinkText("Create a new notification"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public YourImportNotificationsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Your import notifications");
        }

        public void ClickCreateNotification()
        {
            lnkCreateNotification.Click();
        }
    }
}