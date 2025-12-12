using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class InspectorImportNotificationsPage : IInspectorImportNotificationsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.XPath("//h1[@id='page-primary-title']"), true);
        private IWebElement lnkCreateNotification => _driver.WaitForElement(By.LinkText("Create a new notification"));
        private IWebElement txtCHEDRefInput => _driver.WaitForElement(By.Id("keywords"));
        private IWebElement btnSearch => _driver.WaitForElement(By.Id("search-notifications"));
        private IWebElement lnkChedRefNumSearcResult => _driver.WaitForElement(By.XPath("//*[normalize-space()='Reference Number']//following-sibling::dd"));
        private IWebElement lnkChedStatusSearcResult => _driver.WaitForElement(By.XPath("//*[normalize-space()='CHED status']//following-sibling::dd"));
        private IWebElement lnkViewCHED => _driver.FindElement(By.XPath("//a[normalize-space()='View CHED']"));
        private IWebElement lnkRecordControl => _driver.FindElement(By.Id("control-dashboard-nav"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public InspectorImportNotificationsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Import notifications");
        }

        public void SearchForChed(string chedRef)
        {
            txtCHEDRefInput.Click();
            txtCHEDRefInput.Clear();
            txtCHEDRefInput.SendKeys(chedRef);
            btnSearch.Click();
            Thread.Sleep(2000);
        }

        public void VerifyNotificationStatus(string chedRef, string status)
        {
            if (lnkChedRefNumSearcResult.Text.Trim().Contains(chedRef)
                && lnkChedStatusSearcResult.Text.Trim().Equals(status))
            {
                lnkChedRefNumSearcResult.Click();
            }
        }

        public bool VerifyNotificationIsPresent(string chedRef)
        {
            return lnkChedRefNumSearcResult.Text.Trim().Contains(chedRef);
        }

        public void ClickViewCHED()
        {
            lnkViewCHED.Click();
        }

        public void ClickRecordControl()
        {
            lnkRecordControl.Click();
        }
    }
}