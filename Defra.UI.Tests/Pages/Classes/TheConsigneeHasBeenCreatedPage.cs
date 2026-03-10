using Defra.UI.Framework.Driver;
using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class TheConsigneeHasBeenCreatedPage : ITheConsigneeHasBeenCreatedPage
    {

        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-')]"), true);
        private IWebElement btnAddToNotification => _driver.FindElement(By.Id("button-add"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public TheConsigneeHasBeenCreatedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("The consignee has been created");
        }

        public void clickAddToNotificationButton()
        {
            btnAddToNotification.Click();
        }
        #endregion
    }
}
