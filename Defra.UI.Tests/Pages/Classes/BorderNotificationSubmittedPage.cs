using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class BorderNotificationSubmittedPage : IBorderNotificationSubmittedPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@class='govuk-panel__title']"), true);
        private IWebElement txtBNNumber => _driver.FindElement(By.Id("reference-number"));
        private IWebElement btnReturnToDashboard => _driver.FindElement(By.Id("return-to-dashboard"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public BorderNotificationSubmittedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Your border notification has been submitted");
        }

        public string GetBNNumber()
        {
            Console.WriteLine("BN Number: " + txtBNNumber.Text.Trim());
            return txtBNNumber.Text.Trim();
        }

        public void ClickReturnToDashboard()
        {
            btnReturnToDashboard.Click();
        }
    }
}