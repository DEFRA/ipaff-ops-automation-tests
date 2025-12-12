using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Contracts;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReviewOutcomeDecisionPage : IReviewOutcomeDecisionPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.FindElement(By.XPath("//*[@class=' govuk-heading-xl heading-with-help']"));
        private IWebElement btnSubmitDecision => _driver.WaitForElement(By.Id("submit-decision"));
        private IWebElement inputDay => _driver.WaitForElement(By.Id("date-of-checks-day"));
        private IWebElement inputMonth => _driver.WaitForElement(By.Id("date-of-checks-month"));
        private IWebElement inputYear => _driver.WaitForElement(By.Id("date-of-checks-year"));
        private IWebElement inputHour => _driver.WaitForElement(By.Id("time-of-checks-hour"));
        private IWebElement inputMinutes => _driver.WaitForElement(By.Id("time-of-checks-minute"));
        private IWebElement rdoCertifyingOfficer => _driver.FindElement(By.Id("signed-by-official"));

        // Border Control Post
        private IWebElement borderControlPostReference =>
            _driver.FindElement(By.XPath("//th[text()='Border Control Post reference number']//following-sibling::td"));

        // Decision
        private IWebElement acceptanceDecision => _driver.FindElement(By.Id("acceptance-decision"));

        // Seal Numbers
        private IWebElement sealNumbersStatus => _driver.FindElement(By.Id("notifications-not-found"));

        // Helper methods for dynamic row-based elements
        private IWebElement GetReviewTableCellByRowId(string rowId) =>
            _driver.FindElement(By.XPath($"//tr[@id='{rowId}']//td[contains(@class, 'check-status')]"));

        private IWebElement GetRowById(string rowId) =>
            _driver.FindElement(By.XPath($"//tr[@id='{rowId}']"));

        private IWebElement GetCheckStatusCellFromRow(IWebElement row) =>
            row.FindElement(By.XPath(".//td[contains(@class, 'check-status')]"));

        private IWebElement GetCheckTypeHeaderFromRow(IWebElement row) =>
            row.FindElement(By.XPath(".//th[contains(@class, 'check-type')]"));

        private IReadOnlyCollection<IWebElement> GetTableCellsFromRow(IWebElement row) =>
            row.FindElements(By.XPath(".//td[contains(@class, 'govuk-table__cell')]"));
        #endregion

        #region Utility Methods

        /// <summary>
        /// Safely retrieves text from an element by locator, returning null if element not found.
        /// </summary>
        private string? SafelyGetElementTextByLocator(By locator)
        {
            try
            {
                var element = _driver.FindElement(locator);
                return element.Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Safely retrieves text from an element, returning null if element not found.
        /// </summary>
        private string? SafelyGetElementText(IWebElement element)
        {
            try
            {
                return element.Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Safely retrieves text from a dynamic row by ID.
        /// </summary>
        private string? SafelyGetRowText(string rowId)
        {
            try
            {
                return GetReviewTableCellByRowId(rowId).Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Safely retrieves text from a row using helper method.
        /// </summary>
        private string? SafelyGetRowCellText(string rowId)
        {
            try
            {
                var row = GetRowById(rowId);
                return GetCheckStatusCellFromRow(row).Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Safely retrieves text from a cell by row ID and cell index.
        /// </summary>
        private string? SafelyGetRowCellByIndex(string rowId, int cellIndex)
        {
            try
            {
                var row = GetRowById(rowId);
                var cells = GetTableCellsFromRow(row);
                return cells.Count > cellIndex ? cells.ElementAt(cellIndex).Text.Trim() : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Safely retrieves and formats date as "dd MM yyyy" by locator.
        /// </summary>
        private string? SafelyGetFormattedDateByLocator(By locator)
        {
            try
            {
                var element = _driver.FindElement(locator);
                var text = element.Text.Trim();
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MM yyyy");
                }
                return text;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Safely finds elements by XPath, returning empty collection if not found.
        /// </summary>
        private IReadOnlyCollection<IWebElement> SafelyFindElements(By locator)
        {
            try
            {
                return _driver.FindElements(locator);
            }
            catch (NoSuchElementException)
            {
                return Array.Empty<IWebElement>();
            }
            catch (StaleElementReferenceException)
            {
                return Array.Empty<IWebElement>();
            }
        }

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ReviewOutcomeDecisionPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Trim().Contains("Review outcome decision");
        }

        public void ClickSubmitDecision()
        {
            btnSubmitDecision.Click();
        }

        public void EnterCurrentDateAndTime(string day, string month, string year, string hours, string minutes)
        {
            inputDay.Click();
            inputDay.SendKeys(day);
            inputMonth.Click();
            inputMonth.SendKeys(month);
            inputYear.Click();
            inputYear.SendKeys(year);
            inputHour.Click();
            inputHour.SendKeys(hours);
            inputMinutes.Click();
            inputMinutes.SendKeys(minutes);
        }

        public void SelectCertifyingOfficerRadioButton()
        {
            rdoCertifyingOfficer.Click();
        }

        // Border Control Post
        public string? GetBorderControlPostReference()
        {
            return SafelyGetElementText(borderControlPostReference);
        }

        // Checks - Documentary
        public string? GetDocumentaryCheckDecision()
        {
            return SafelyGetRowText("parttwo/consignmentcheck");
        }

        // Checks - Identity (handles both CHED-A and CHED-P)
        public string? GetIdentityCheckType()
        {
            try
            {
                var row = GetRowById("consignmentcheck/identitycheckdone");
                return GetCheckTypeHeaderFromRow(row).Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public string? GetIdentityCheckDecision()
        {
            // Try CHED-P first
            var result = SafelyGetRowText("consignmentcheck/identitycheckdone");
            if (result != null) return result;

            // Fallback to CHED-A
            return SafelyGetRowText("consignmentcheck/identitycheckresult");
        }

        // Checks - Physical
        public string? GetPhysicalCheckDecision()
        {
            // Try CHED-P first
            var result = SafelyGetRowText("consignmentcheck/physicalcheckdone");
            if (result != null) return result;

            // Fallback to CHED-A
            return SafelyGetRowText("consignmentcheck/physicalcheckresult");
        }

        // Checks - Animals (CHED-A only)
        public string? GetNumberOfAnimalsChecked()
        {
            return SafelyGetRowText("consignmentcheck/numberofanimalschecked");
        }

        public string? GetWelfareCheckDecision()
        {
            return SafelyGetRowText("consignmentcheck/welfarecheck");
        }

        // Impact of transport - Dead Animals (CHED-A only)
        public string? GetNumberOfDeadAnimals()
        {
            return SafelyGetRowCellText("impactoftransportonanimals/numberofdeadanimals");
        }

        public string? GetNumberOfDeadAnimalsUnit()
        {
            return SafelyGetRowCellByIndex("impactoftransportonanimals/numberofdeadanimals", 1);
        }

        // Impact of transport - Unfit Animals (CHED-A only)
        public string? GetNumberOfUnfitAnimals()
        {
            return SafelyGetRowCellText("impactoftransportonanimals/numberofunfitanimals");
        }

        public string? GetNumberOfUnfitAnimalsUnit()
        {
            return SafelyGetRowCellByIndex("impactoftransportonanimals/numberofunfitanimals", 1);
        }

        // Impact of transport - Births/Abortions (CHED-A only)
        public string? GetNumberOfBirthsOrAbortions()
        {
            return SafelyGetRowCellText("impactoftransportonanimals/numberofbirthsorabortions");
        }

        // Seal Numbers
        public string? GetSealNumbersStatus()
        {
            return SafelyGetElementText(sealNumbersStatus);
        }

        // Laboratory Tests
        public string? GetLaboratoryTestsRequired()
        {
            return SafelyGetRowText("parttwo/laboratorytestsrequired");
        }

        // Documents - Health Certificate (CHED-A only)
        public string? GetHealthCertificateReference()
        {
            return SafelyGetElementTextByLocator(By.Id("latest-health-document-reference"));
        }

        public string? GetHealthCertificateDateOfIssue()
        {
            return SafelyGetFormattedDateByLocator(By.Id("latest-health-document-issue-date"));
        }

        public string? GetHealthCertificateFileName()
        {
            return SafelyGetElementTextByLocator(By.XPath("//table[@id='latest-health-document-table']//a[contains(@id,'attachment-view')]"));
        }

        // Documents - Additional Documents
        public string? GetAdditionalDocumentType()
        {
            try
            {
                var rows = SafelyFindElements(By.XPath("//table[@id='accompanying-documents-table']//tbody//tr"));
                if (rows.Count > 0)
                {
                    var cells = rows.First().FindElements(By.TagName("td"));
                    return cells.Count > 0 ? cells[0].Text.Trim() : null;
                }
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public string? GetAdditionalDocumentReference()
        {
            try
            {
                var rows = SafelyFindElements(By.XPath("//table[@id='accompanying-documents-table']//tbody//tr"));
                if (rows.Count > 0)
                {
                    var cells = rows.First().FindElements(By.TagName("td"));
                    return cells.Count > 1 ? cells[1].Text.Trim() : null;
                }
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public string? GetAdditionalDocumentDateOfIssue()
        {
            try
            {
                var rows = SafelyFindElements(By.XPath("//table[@id='accompanying-documents-table']//tbody//tr"));
                if (rows.Count > 0)
                {
                    var cells = rows.First().FindElements(By.TagName("td"));
                    if (cells.Count > 2)
                    {
                        var text = cells[2].Text.Trim();
                        if (DateTime.TryParse(text, out DateTime date))
                        {
                            return date.ToString("dd MM yyyy");
                        }
                        return text;
                    }
                }
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public string? GetAdditionalDocumentFileName()
        {
            return SafelyGetElementTextByLocator(By.XPath("//table[@id='accompanying-documents-table']//a[contains(@id,'attachment-')]"));
        }

        // Decision
        public string? GetAcceptanceDecision()
        {
            return SafelyGetElementText(acceptanceDecision);
        }

        // CHED-A specific
        public string? GetCertifiedFor()
        {
            return SafelyGetElementTextByLocator(By.XPath("//td[@id='certified_for']//following-sibling::td"));
        }

        // CHED-P specific
        public string? GetConsignmentUse()
        {
            return SafelyGetElementTextByLocator(By.XPath("//tr[@id='decision/consignmentacceptable']//td[contains(@class, 'check-status')]"));
        }

        // Controlled Destination
        public string? GetControlledDestinationName()
        {
            try
            {
                var element = _driver.FindElement(By.XPath("//tr[@id='controlled-destination']//td[@class='govuk-table__cell']"));
                var fullText = element.Text.Trim();
                return ExtractNameFromText(fullText);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public string? GetControlledDestinationAddress()
        {
            try
            {
                var element = _driver.FindElement(By.XPath("//tr[@id='controlled-destination']//td[@class='govuk-table__cell']"));
                var fullText = element.Text.Trim();
                return ExtractAddressFromText(fullText);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        // Helper methods for text extraction
        private string ExtractNameFromText(string? fullText)
        {
            if (string.IsNullOrEmpty(fullText))
                return string.Empty;

            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            return lines.Length > 0 ? lines[0].Trim() : string.Empty;
        }

        private string ExtractAddressFromText(string? fullText)
        {
            if (string.IsNullOrEmpty(fullText))
                return string.Empty;

            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            return lines.Length > 1 ? lines[1].Trim() : string.Empty;
        }
    }
}