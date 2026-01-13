using System.ComponentModel;
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
        
        private IWebElement searchNotificationsPanel => _driver.FindElement(By.XPath("//div[contains(@class,'search-panel')]"));
        private IWebElement txtCertificateNumber => _driver.FindElement(By.Id("certificate-number"));
        private IWebElement txtCommodity => _driver.FindElement(By.Id("commodity-code-or-desc"));
        private IWebElement ddlBCPorPOE => _driver.FindElement(By.Id("bcp"));
        private IWebElement ddlStatus => _driver.FindElement(By.Id("certificate-status"));
        private IWebElement ddlCountryOfOrigin => _driver.FindElement(By.Id("country-of-origin"));
        private IWebElement txtConsignee => _driver.FindElement(By.Id("consignee"));
        private IWebElement ddlNotificationType => _driver.FindElement(By.Id("certificate-type"));
        private IWebElement ddlMicroChipNum => _driver.FindElement(By.Id("microchip-number"));
        private IWebElement arrivalImportDateRangeBlock => _driver.FindElement(By.XPath("//legend[contains(text(),'Import date range')]/following-sibling::div"));

        private IWebElement txtStartDateDay => _driver.FindElement(By.Id("start-date-day"));
        private IWebElement txtStartDateMonth => _driver.FindElement(By.Id("start-date-month"));
        private IWebElement txtStartDateYear => _driver.FindElement(By.Id("start-date-year"));
        private IWebElement txtStartDateDatePicker => _driver.FindElement(By.XPath("//div[@id='start-date']//div[contains(@class,'defra-datepicker')]/button"));

        private IWebElement txtEndDateDay => _driver.FindElement(By.Id("end-date-day"));
        private IWebElement txtEndDateMonth => _driver.FindElement(By.Id("end-date-month"));
        private IWebElement txtEndDateYear => _driver.FindElement(By.Id("end-date-year"));
        private IWebElement txtEndDateDatePicker => _driver.FindElement(By.XPath("//div[@id='end-date']//div[contains(@class,'defra-datepicker')]/button"));

        private IWebElement notificationCount => _driver.FindElement(By.Id("notification-count"));
        private IWebElement GetNotificationReferenceInList(string chedRef) => _driver.FindElement(By.XPath($"//dd[@id='reference-number-0' and contains(text(), '{chedRef}')]"));
        private IWebElement GetShowNotificationLink(string chedRef) => _driver.FindElement(By.Id($"show-certificate-{chedRef}"));
        private IWebElement GetAmendLink(string chedRef) => _driver.FindElement(By.Id($"amend-details-{chedRef}"));
        private IWebElement lnkAddressBook => _driver.FindElement(By.Id("address-book-link"));
        private IWebElement lnkContact => _driver.FindElement(By.XPath("//a[contains(text(),'Contact')]"));
        private IWebElement lnkCookies => _driver.FindElement(By.Id("button-cookies"));

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

        public void ClickAddressBookLink() => lnkAddressBook.Click();

        public void ClickContactLink() => lnkContact.Click();

        public bool IsSearchNotiByPanelDisplayed => searchNotificationsPanel != null && searchNotificationsPanel.Displayed;
        public bool AreAllSearchFieldsDisplayed()
        {
            return IsCertificateNumberDisplayed &&
                   IsCommodityDisplayed &&
                   IsBCPorPOEDisplayed &&
                   IsStatusDisplayed &&
                   IsCountryOfOriginDisplayed &&
                   IsConsigneeDisplayed &&
                   IsNotificationTypeDisplayed &&
                   IsMicroChipNumDisplayed &&
                   IsArrivalImportDateRangeBlockDisplayed &&
                   AreArrivalImportDateRangeLinksDisplayed() &&
                   IsStartDateDayDisplayed &&
                   IsStartDateMonthDisplayed &&
                   IsStartDateYearDisplayed &&
                   IsStartDateDatePickerDisplayed &&
                   IsEndDateDayDisplayed &&
                   IsEndDateMonthDisplayed &&
                   IsEndDateYearDisplayed &&
                   IsEndDateDatePickerDisplayed;
        }

        private bool IsCertificateNumberDisplayed => txtCertificateNumber.Displayed;
        private bool IsCommodityDisplayed => txtCommodity.Displayed;
        private bool IsBCPorPOEDisplayed => ddlBCPorPOE.Displayed;
        private bool IsStatusDisplayed => ddlStatus.Displayed;
        private bool IsCountryOfOriginDisplayed => ddlCountryOfOrigin.Displayed;
        private bool IsConsigneeDisplayed => txtConsignee.Displayed;
        private bool IsNotificationTypeDisplayed => ddlNotificationType.Displayed;
        private bool IsMicroChipNumDisplayed => ddlMicroChipNum.Displayed;
        private bool IsArrivalImportDateRangeBlockDisplayed => arrivalImportDateRangeBlock.Displayed;
        private bool AreArrivalImportDateRangeLinksDisplayed()
        {
            var buttons = arrivalImportDateRangeBlock.FindElements(By.CssSelector("button[name='date-range']"));
            return buttons.Count == 4;
        }

        private bool IsStartDateDayDisplayed => txtStartDateDay.Displayed;
        private bool IsStartDateMonthDisplayed => txtStartDateMonth.Displayed;
        private bool IsStartDateYearDisplayed => txtStartDateYear.Displayed;
        private bool IsStartDateDatePickerDisplayed => txtStartDateDatePicker.Displayed;

        private bool IsEndDateDayDisplayed => txtEndDateDay.Displayed;
        private bool IsEndDateMonthDisplayed => txtEndDateMonth.Displayed;
        private bool IsEndDateYearDisplayed => txtEndDateYear.Displayed;
        private bool IsEndDateDatePickerDisplayed => txtEndDateDatePicker.Displayed;
    }
}