using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDOverviewPage : ICHEDOverviewPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl ']"), true);
        private IWebElement btnRaiseBorderNotification => _driver.FindElement(By.Id("raise-border-notification"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CHEDOverviewPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("CHED overview");
        }

        public void ClickRaiseBorderNotification()
        {
            btnRaiseBorderNotification.Click();
        }
    }
}