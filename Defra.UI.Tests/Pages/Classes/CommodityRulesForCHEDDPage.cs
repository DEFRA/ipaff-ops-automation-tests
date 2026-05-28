using System;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CommodityRulesForCHEDDPage : ICommodityRulesForCHEDDPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Commodity rules for CHED-D']"), true);
        private By inProgressCaptionBy => By.XPath("//caption[normalize-space()='File submission in progress']");
        private By inProgressFirstRowBy => By.XPath("//caption[normalize-space()='File submission in progress']/parent::table/tbody/tr[1]");
        private By previousFirstRowBy => By.XPath("//caption[normalize-space()='Previous submissions']/parent::table/tbody/tr[1]");
        private By rowFileNameBy => By.XPath("./td[1]");
        private By rowStatusTagBy => By.XPath("./td[3]//span[contains(@class,'govuk-tag')]");
        private By confirmAndSubmitLinkBy => By.XPath(".//a[normalize-space()='Confirm and submit']");
        private By viewSummaryLinkBy => By.XPath(".//a[normalize-space()='View summary']");
        #endregion

        public CommodityRulesForCHEDDPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;

        public bool IsFileSubmissionInProgressSectionPresent() => _driver.FindElements(inProgressCaptionBy).Count > 0;

        public string GetFirstInProgressFileName() =>
            _driver.FindElement(inProgressFirstRowBy).FindElement(rowFileNameBy).Text.Trim();

        public string GetFirstInProgressStatus() =>
            _driver.FindElement(inProgressFirstRowBy).FindElement(rowStatusTagBy).Text.Trim();

        public bool WaitForFirstInProgressStatus(string expectedStatus, int timeoutSeconds = 60)
        {
            var endTime = DateTime.UtcNow.AddSeconds(timeoutSeconds);
            while (DateTime.UtcNow < endTime)
            {
                try
                {
                    if (IsFileSubmissionInProgressSectionPresent() &&
                        string.Equals(GetFirstInProgressStatus(), expectedStatus, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                catch (NoSuchElementException) { /* not rendered yet */ }
                catch (StaleElementReferenceException) { /* refresh in progress */ }

                _driver.Navigate().Refresh();
                _driver.Wait(2);
            }
            return false;
        }

        public bool WaitForFileSubmissionInProgressSectionToBeRemoved(int timeoutSeconds = 60)
        {
            var endTime = DateTime.UtcNow.AddSeconds(timeoutSeconds);
            while (DateTime.UtcNow < endTime)
            {
                if (!IsFileSubmissionInProgressSectionPresent())
                    return true;

                _driver.Navigate().Refresh();
                _driver.Wait(2);
            }
            return false;
        }

        public void ClickConfirmAndSubmitLinkForFirstInProgress() =>
            _driver.FindElement(inProgressFirstRowBy).FindElement(confirmAndSubmitLinkBy).Click();

        public string GetFirstPreviousSubmissionStatus() =>
            _driver.FindElement(previousFirstRowBy).FindElement(rowStatusTagBy).Text.Trim();

        public void ClickViewSummaryLinkForFirstPreviousSubmission() =>
            _driver.FindElement(previousFirstRowBy).FindElement(viewSummaryLinkBy).Click();

        public void RefreshPage() => _driver.Navigate().Refresh();
    }
}