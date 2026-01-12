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
        private IWebElement txtCertificateNumber => _driver.FindElement(By.Id("certificate-number"));
        private IWebElement notificationCount => _driver.FindElement(By.Id("notification-count"));
        private IWebElement GetNotificationReferenceInList(string chedRef) => _driver.FindElement(By.XPath($"//dd[@id='reference-number-0' and contains(text(), '{chedRef}')]"));
        private IWebElement GetShowNotificationLink(string chedRef) => _driver.FindElement(By.Id($"show-certificate-{chedRef}"));
        private IWebElement GetAmendLink(string chedRef) => _driver.FindElement(By.Id($"amend-details-{chedRef}"));
        private IWebElement GetNotificationStatus(string chedRef) => _driver.FindElement(By.Id("status-0"));
        private IWebElement lnkCookies => _driver.FindElement(By.Id("button-cookies"));
        private By NotificationStatusBy => By.Id("status-0");
        private By GetAmendLinkBy(string chedRef) => By.Id($"amend-details-{chedRef}");
        private By GetCopyAsNewLinkBy(string chedRef) => By.Id($"copy-as-new-{chedRef}");
        private By GetViewDetailsLinkBy(string chedRef) => By.Id($"view-details-{chedRef}");
        private By GetShowNotificationLinkBy(string chedRef) => By.Id($"show-certificate-{chedRef}");

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

        public void SearchForNotification(string notificationNumber)
        {
            txtCertificateNumber.Clear();
            txtCertificateNumber.SendKeys(notificationNumber);
            txtCertificateNumber.SendKeys(Keys.Enter);
        }

        public bool VerifyNotificationInList(string chedReference)
        {
            try
            {
                return notificationCount.Text.Equals("1") &&
                       GetNotificationReferenceInList(chedReference).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void ClickShowNotification(string chedReference)
        {
            GetShowNotificationLink(chedReference).Click();
        }

        public bool VerifyCertificateInNewTab()
        {
            var windowHandles = _driver.WindowHandles;
            if (windowHandles.Count > 1)
            {
                _driver.SwitchTo().Window(windowHandles.Last());

                // Optional Wait for PDF to load (helps with screenshot rendering) - can remove if not needed
                Thread.Sleep(2000);

                return _driver.Url.Contains("/certificate/pdf");
            }
            return false;
        }

        public bool VerifyDataInCertificate(string chedReference)
        {
            return _driver.Url.Contains($"/{chedReference}/certificate/pdf");
        }

        public void ClosePDFBrowserTab()
        {
            var windowHandles = _driver.WindowHandles;
            if (windowHandles.Count > 1)
            {
                _driver.Close();
                _driver.SwitchTo().Window(windowHandles.First());
            }
        }

        public bool VerifyBrowserTabClosed()
        {
            return _driver.WindowHandles.Count == 1;
        }

        public void ClickCookiesLink()
        {
            lnkCookies.Click();
        }

        public void ClickAmend(string chedReference)
        {
            GetAmendLink(chedReference).Click();
        }

        public string GetNotificationStatus()
        {
            return _driver.SafelyGetText(NotificationStatusBy);
        }

        public bool IsAmendLinkPresent(string chedReference)
        {
            return _driver.IsElementDisplayed(GetAmendLinkBy(chedReference));
        }

        public bool IsAmendLinkNotPresent(string chedReference)
        {
            return !_driver.IsElementDisplayed(GetAmendLinkBy(chedReference));
        }

        public bool IsCopyAsNewLinkPresent(string chedReference)
        {
            return _driver.IsElementDisplayed(GetCopyAsNewLinkBy(chedReference));
        }

        public bool IsViewDetailsLinkPresent(string chedReference)
        {
            return _driver.IsElementDisplayed(GetViewDetailsLinkBy(chedReference));
        }

        public bool IsShowNotificationLinkPresent(string chedReference)
        {
            return _driver.IsElementDisplayed(GetShowNotificationLinkBy(chedReference));
        }

        public void ClickViewDetails(string chedReference)
        {
            _driver.FindElement(GetViewDetailsLinkBy(chedReference)).Click();
        }
    }
}