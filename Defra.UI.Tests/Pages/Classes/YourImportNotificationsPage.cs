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

        // Snapshot of window handles taken immediately before the PDF tab is opened.
        // Must be set before any action that opens the PDF (either ClickShowNotification
        // or an external page class via RecordHandlesBeforePdfOpen).
        private IList<string> _handlesBeforePdfOpen = [];

        // Snapshot of the tab count taken at the start of ClosePDFBrowserTab, used by
        // VerifyBrowserTabClosed to assert exactly one tab was closed.
        private int _handleCountBeforePdfClose;

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
        private IWebElement GetNotificationStatus(string chedRef) => _driver.FindElement(By.Id("status-0"));
        private IWebElement lnkAddressBook => _driver.FindElement(By.Id("address-book-link"));
        private IWebElement lnkViewDetails => _driver.FindElement(By.Name("viewDetails"));
        private IWebElement lnkCopyAsNew => _driver.FindElement(By.CssSelector("button[id*='copy-as-new']"));
        private IWebElement lnkContact => _driver.FindElement(By.XPath("//a[contains(text(),'Contact')]"));
        private IWebElement lnkCookies => _driver.FindElement(By.Id("button-cookies"));
        private IWebElement lnkManageTradePartners => _driver.FindElement(By.Id("manage-trade-partners-link"));
        private By NotificationStatusBy => By.Id("status-0");
        private By GetAmendLinkBy(string chedRef) => By.Id($"amend-details-{chedRef}");
        private By GetCopyAsNewLinkBy(string chedRef) => By.Id($"copy-as-new-{chedRef}");
        private By GetViewDetailsLinkBy(string chedRef) => By.Id($"view-details-{chedRef}");
        private By GetShowNotificationLinkBy(string chedRef) => By.Id($"show-certificate-{chedRef}");
        private IWebElement txtStatus => _driver.FindElement(By.XPath("//*[normalize-space()='CHED status']/following-sibling::dd"));
        private IWebElement btnClone => _driver.FindElement(By.Id("search-to-clone"));
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

        /// <summary>
        /// Snapshots window handles then clicks Show notification. The snapshot is taken
        /// first so that VerifyCertificateInNewTab can detect the new tab even if the
        /// browser opens it synchronously before the verify step runs.
        /// </summary>
        public void ClickShowNotification(string chedReference)
        {
            GetShowNotificationLink(chedReference).Click();
        }

        /// <summary>
        /// Records the current window handles before a PDF tab is opened by an external
        /// page class (e.g. CHEDOverviewPage.ClickShowChed). Must be called immediately
        /// before the action that opens the new tab.
        /// </summary>
        public void RecordHandlesBeforePdfOpen()
        {
            _handlesBeforePdfOpen = _driver.WindowHandles.ToList();
        }

        /// <summary>
        /// Switches to the PDF tab that was opened after the handles snapshot and verifies
        /// the URL contains '/certificate/pdf'. Waits up to 10 seconds for the new tab to
        /// appear. Works for both single-browser flows and the multi-tab Dynamics hand-off.
        /// </summary>
        public bool VerifyCertificateInNewTab()
        {
            if (_handlesBeforePdfOpen.Count == 0)
            {
                _handlesBeforePdfOpen = [_driver.CurrentWindowHandle];
            }

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IList<string> allHandles = _handlesBeforePdfOpen;

            try
            {
                wait.Until(d =>
                {
                    allHandles = d.WindowHandles.ToList();
                    return allHandles.Count > _handlesBeforePdfOpen.Count;
                });
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException)
            {
                return false;
            }

            var newHandle = allHandles.Except(_handlesBeforePdfOpen).FirstOrDefault();
            if (newHandle == null)
            {
                return false;
            }
            _driver.SwitchTo().Window(newHandle);

            var urlWait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            try
            {
                urlWait.Until(d =>
                {
                    var url = d.Url;
                    return !string.IsNullOrEmpty(url) && url != "about:blank" && url != "data:,";
                });
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException)
            {
                // URL did not stabilise within timeout — proceed with current URL.
            }

            return _driver.Url.Contains("/certificate/pdf");
        }
        public string getPDFUrl()
        {
            return _driver.Url;
        }

        public bool VerifyDataInCertificate(string chedReference)
        {
            return _driver.Url.Contains($"/{chedReference}/");
        }

        /// <summary>
        /// Closes the current PDF tab and returns focus to the previous tab.
        /// Switches to the last remaining handle that is not the closed one, which
        /// correctly returns to the IPAFFS tab in both single-browser and multi-tab flows
        /// (rather than always switching to index [0] which may be the Dynamics tab).
        /// </summary>
        public void ClosePDFBrowserTab()
        {
            var currentHandle = _driver.CurrentWindowHandle;
            var allHandles = _driver.WindowHandles.ToList();
            _handleCountBeforePdfClose = allHandles.Count;

            if (allHandles.Count > 1)
            {
                _driver.Close();

                var remainingHandles = _driver.WindowHandles.ToList();
                var previousHandle = remainingHandles
                    .LastOrDefault(h => h != currentHandle)
                    ?? remainingHandles.First();

                _driver.SwitchTo().Window(previousHandle);
            }
        }

        /// <summary>
        /// Verifies that exactly one tab was closed. Works for single-browser (2→1)
        /// and the Dynamics hand-off flow (3→2).
        /// </summary>
        public bool VerifyBrowserTabClosed()
        {
            return _driver.WindowHandles.Count == _handleCountBeforePdfClose - 1;
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
        public void ClickViewDetailsLink() => lnkViewDetails.Click();
        public void ClickContactLink() => lnkContact.Click();
        public void ClickCopyAsNewLink() => lnkCopyAsNew.Click();
        public void ClickManageTradePartnersLink() => lnkManageTradePartners.Click();

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

        public bool VerifyNotificationStatus(string status)
        {
            return txtStatus.Text.Trim().Equals(status);
        }

        public void ClickCloneButton()
        {
            btnClone.Click();
        }
    }
}