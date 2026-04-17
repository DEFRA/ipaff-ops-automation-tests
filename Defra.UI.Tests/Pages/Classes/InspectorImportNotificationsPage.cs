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
        private IWebElement lnkRiskOutcomeSearchResult => _driver.WaitForElement(By.XPath("//*[normalize-space()='Risk outcome']//following-sibling::dd"));
        private IWebElement lnkViewCHED => _driver.FindElement(By.XPath("//a[normalize-space()='View CHED']"));
        private IWebElement lnkRecordControl => _driver.FindElement(By.Id("control-dashboard-nav"));
        private IWebElement lnkRecordDecision => _driver.FindElement(By.Id("decision-dashboard-nav"));
        private IWebElement lnkCreateNotificationAsAdmin => _driver.FindElement(By.Id("notification-dashboard-nav"));
        private IWebElement txtNoNotificationsFound => _driver.FindElement(By.Id("notifications-not-found"));
        private IWebElement lnkHeader(string link) => _driver.FindElement(By.XPath($"//*[normalize-space(text())='{link}']"));
        private IWebElement lblImportNotificationPage(string label) => _driver.FindElement(By.XPath($"//*[normalize-space(text())='{label}']"));
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
            txtCHEDRefInput.Clear();
            txtCHEDRefInput.SendKeys(chedRef);
            btnSearch.Click();
        }

        public void VerifyNotificationStatusAndClick(string chedRef, string status)
        {
            if (lnkChedRefNumSearcResult.Text.Trim().Contains(chedRef)
                && lnkChedStatusSearcResult.Text.Trim().Equals(status, StringComparison.OrdinalIgnoreCase))
            {
                lnkChedRefNumSearcResult.Click();
            }
        }

        public void SearchAndClickNotification(string chedRef)
        {
            SearchForChed(chedRef);
            lnkChedRefNumSearcResult.Click();
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

        public void ClickCreateNotification()
        {
            lnkCreateNotificationAsAdmin.Click();
        }

        public void ClickRecordDecision()
        {
            lnkRecordDecision.Click();
        }

        public bool VerifyNotificationIsPresentWithStatus(string type, string chedRef, string replacementChedReference, string status)
        {
            if (type.Equals("original"))
                return lnkChedRefNumSearcResult.Text.Trim().Contains(chedRef)
                    && lnkChedStatusSearcResult.Text.Trim().Equals(status, StringComparison.OrdinalIgnoreCase);
            else if (type.Equals("replacement"))
                return lnkChedRefNumSearcResult.Text.Trim().Contains(replacementChedReference)
                    && lnkChedStatusSearcResult.Text.Trim().Equals(status, StringComparison.OrdinalIgnoreCase);
            else
                return false;
        }

        public void ClickNotification()
        {
            lnkChedRefNumSearcResult.Click();
        }

        public bool VerifyNotificationIsNotPresent()
        {
            try
            {
                return txtNoNotificationsFound.Text.Trim().Equals("No notifications have been found");
            }
            catch (NoSuchElementException)
            {
                // If the element doesn't exist, it means results were found
                return false;
            }
        }

        public string GetNotificationStatus()
        {
            return lnkChedStatusSearcResult.Text.Trim();
        }

        public bool VerifyNotificationHeader(string link)
        {
            return lnkHeader(link).Text.Equals(link);
        }
        
        public bool VerifyLabel(string label)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lblImportNotificationPage(label));

            int index = lblImportNotificationPage(label).Text.IndexOf("\r\n");

            return  (index >= 0
                ? lblImportNotificationPage(label).Text.Substring(0, index)
                : lblImportNotificationPage(label).Text).Equals(label);
        }

        public bool VerifyRiskOutcome(string chedRef, string riskOutcome)
        {
            return lnkChedRefNumSearcResult.Text.Trim().Contains(chedRef)
                && lnkRiskOutcomeSearchResult.Text.Trim().Equals(riskOutcome);
        }
    }
}