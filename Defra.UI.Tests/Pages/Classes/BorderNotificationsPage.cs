using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class BorderNotificationsPage : IBorderNotificationsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement txtBorderNotification => _driver.FindElement(By.Id("border-notification-number"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search-notifications"));
        private IWebElement lnkBNNumSearcResult => _driver.FindElement(By.XPath("//*[normalize-space()='Reference Number']//following-sibling::dd"));
        private IWebElement lnkBNStatusSearcResult => _driver.FindElement(By.XPath("//*[normalize-space()='Status']//following-sibling::dd"));
        private IReadOnlyCollection<IWebElement> lnkViewDetails => _driver.FindElements(By.XPath("//a[normalize-space()='View details']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public BorderNotificationsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Border notifications");
        }

        public void SearchForNotification(string borderNotification)
        {
            txtBorderNotification.Clear();
            txtBorderNotification.SendKeys(borderNotification);
            btnSearch.Click();
        }

        public bool VerifyNotificationStatus(string borderNotification, string status)
        {
            return lnkBNNumSearcResult.Text.Trim().Contains(borderNotification)
                   && lnkBNStatusSearcResult.Text.Trim().Equals(status);
        }

        public void ClickViewDetails()
        {
            lnkViewDetails.First().Click();
        }
    }
}