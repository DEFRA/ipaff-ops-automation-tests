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

        // Documents
        private IWebElement healthCertificateReference => _driver.FindElement(By.Id("latest-health-document-reference"));
        private IWebElement healthCertificateDateOfIssue => _driver.FindElement(By.Id("latest-health-document-issue-date"));
        private IReadOnlyCollection<IWebElement> accompanyingDocumentsTableRows =>
            _driver.FindElements(By.XPath("//table[@id='accompanying-documents-table']//tbody//tr"));

        // Decision
        private IWebElement acceptanceDecision => _driver.FindElement(By.Id("acceptance-decision"));
        private IWebElement certifiedFor =>
            _driver.FindElement(By.XPath("//td[@id='certified_for']//following-sibling::td"));
        private IWebElement consignmentUse =>
            _driver.FindElement(By.XPath("//tr[@id='decision/consignmentacceptable']//td[contains(@class, 'check-status')]"));

        // Seal Numbers
        private IWebElement sealNumbersStatus => _driver.FindElement(By.Id("notifications-not-found"));

        // Controlled Destination
        private IWebElement controlledDestinationDetails =>
            _driver.FindElement(By.XPath("//tr[@id='controlled-destination']//td[@class='govuk-table__cell']"));

        // Helper methods for dynamic row-based elements
        private IWebElement GetReviewTableCellByRowId(string rowId) =>
            _driver.FindElement(By.XPath($"//tr[@id='{rowId}']//td[contains(@class, 'check-status')]"));

        private IWebElement GetRowById(string rowId) =>
            _driver.FindElement(By.XPath($"//tr[@id='{rowId}']"));

        // Helper to get check-status cell from a row
        private IWebElement GetCheckStatusCellFromRow(IWebElement row) =>
            row.FindElement(By.XPath(".//td[contains(@class, 'check-status')]"));

        // Helper to get check-type header from a row
        private IWebElement GetCheckTypeHeaderFromRow(IWebElement row) =>
            row.FindElement(By.XPath(".//th[contains(@class, 'check-type')]"));

        // Helper to get all table cells from a row
        private IReadOnlyCollection<IWebElement> GetTableCellsFromRow(IWebElement row) =>
            row.FindElements(By.XPath(".//td[contains(@class, 'govuk-table__cell')]"));
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
            try { return borderControlPostReference.Text.Trim(); }
            catch { return null; }
        }

        // Checks - Documentary
        public string? GetDocumentaryCheckDecision()
        {
            try { return GetReviewTableCellByRowId("parttwo/consignmentcheck").Text.Trim(); }
            catch { return null; }
        }

        // Checks - Identity (handles both CHED-A and CHED-P)
        public string? GetIdentityCheckType()
        {
            try
            {
                var row = GetRowById("consignmentcheck/identitycheckdone");
                return GetCheckTypeHeaderFromRow(row).Text.Trim();
            }
            catch { return null; }
        }

        public string? GetIdentityCheckDecision()
        {
            try
            {
                // Try CHED-P first (consignmentcheck/identitycheckdone)
                return GetReviewTableCellByRowId("consignmentcheck/identitycheckdone").Text.Trim();
            }
            catch
            {
                try
                {
                    // Fallback to CHED-A (consignmentcheck/identitycheckresult)
                    return GetReviewTableCellByRowId("consignmentcheck/identitycheckresult").Text.Trim();
                }
                catch { return null; }
            }
        }

        // Checks - Physical
        public string? GetPhysicalCheckDecision()
        {
            try
            {
                // Try CHED-P first (consignmentcheck/physicalcheckdone)
                return GetReviewTableCellByRowId("consignmentcheck/physicalcheckdone").Text.Trim();
            }
            catch
            {
                try
                {
                    // Fallback to CHED-A (consignmentcheck/physicalcheckresult)
                    return GetReviewTableCellByRowId("consignmentcheck/physicalcheckresult").Text.Trim();
                }
                catch { return null; }
            }
        }

        // Checks - Animals
        public string? GetNumberOfAnimalsChecked()
        {
            try { return GetReviewTableCellByRowId("consignmentcheck/numberofanimalschecked").Text.Trim(); }
            catch { return null; }
        }

        public string? GetWelfareCheckDecision()
        {
            try { return GetReviewTableCellByRowId("consignmentcheck/welfarecheck").Text.Trim(); }
            catch { return null; }
        }

        // Impact of transport - Dead Animals
        public string? GetNumberOfDeadAnimals()
        {
            try
            {
                var row = GetRowById("impactoftransportonanimals/numberofdeadanimals");
                return GetCheckStatusCellFromRow(row).Text.Trim();
            }
            catch { return null; }
        }

        public string? GetNumberOfDeadAnimalsUnit()
        {
            try
            {
                var row = GetRowById("impactoftransportonanimals/numberofdeadanimals");
                var cells = GetTableCellsFromRow(row);
                return cells.Count > 1 ? cells.ElementAt(1).Text.Trim() : null;
            }
            catch { return null; }
        }

        // Impact of transport - Unfit Animals
        public string? GetNumberOfUnfitAnimals()
        {
            try
            {
                var row = GetRowById("impactoftransportonanimals/numberofunfitanimals");
                return GetCheckStatusCellFromRow(row).Text.Trim();
            }
            catch { return null; }
        }

        public string? GetNumberOfUnfitAnimalsUnit()
        {
            try
            {
                var row = GetRowById("impactoftransportonanimals/numberofunfitanimals");
                var cells = GetTableCellsFromRow(row);
                return cells.Count > 1 ? cells.ElementAt(1).Text.Trim() : null;
            }
            catch { return null; }
        }

        // Impact of transport - Births/Abortions
        public string? GetNumberOfBirthsOrAbortions()
        {
            try
            {
                var row = GetRowById("impactoftransportonanimals/numberofbirthsorabortions");
                return GetCheckStatusCellFromRow(row).Text.Trim();
            }
            catch { return null; }
        }

        // Seal Numbers
        public string? GetSealNumbersStatus()
        {
            try { return sealNumbersStatus.Text.Trim(); }
            catch { return null; }
        }

        // Laboratory Tests
        public string? GetLaboratoryTestsRequired()
        {
            try { return GetReviewTableCellByRowId("parttwo/laboratorytestsrequired").Text.Trim(); }
            catch { return null; }
        }

        // Documents
        public string? GetHealthCertificateReference()
        {
            try { return healthCertificateReference.Text.Trim(); }
            catch { return null; }
        }

        public string? GetHealthCertificateDateOfIssue()
        {
            try
            {
                var text = healthCertificateDateOfIssue.Text.Trim();
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MM yyyy");
                }
                return text;
            }
            catch { return null; }
        }

        public string? GetAdditionalDocumentType()
        {
            try
            {
                var rows = accompanyingDocumentsTableRows;
                return rows.Count > 0 ? rows.First().FindElements(By.TagName("td"))[0].Text.Trim() : null;
            }
            catch { return null; }
        }

        public string? GetAdditionalDocumentReference()
        {
            try
            {
                var rows = accompanyingDocumentsTableRows;
                return rows.Count > 0 ? rows.First().FindElements(By.TagName("td"))[1].Text.Trim() : null;
            }
            catch { return null; }
        }

        public string? GetAdditionalDocumentDateOfIssue()
        {
            try
            {
                var rows = accompanyingDocumentsTableRows;
                if (rows.Count > 0)
                {
                    var text = rows.First().FindElements(By.TagName("td"))[2].Text.Trim();
                    if (DateTime.TryParse(text, out DateTime date))
                    {
                        return date.ToString("dd MM yyyy");
                    }
                    return text;
                }
                return null;
            }
            catch { return null; }
        }

        // Decision
        public string? GetAcceptanceDecision()
        {
            try { return acceptanceDecision.Text.Trim(); }
            catch { return null; }
        }

        public string? GetCertifiedFor()
        {
            try { return certifiedFor.Text.Trim(); }
            catch { return null; }
        }

        public string? GetConsignmentUse()
        {
            try { return consignmentUse.Text.Trim(); }
            catch { return null; }
        }

        // Controlled Destination
        public string? GetControlledDestinationName()
        {
            try
            {
                var fullText = controlledDestinationDetails.Text.Trim();
                return ExtractNameFromText(fullText);
            }
            catch { return null; }
        }

        public string? GetControlledDestinationAddress()
        {
            try
            {
                var fullText = controlledDestinationDetails.Text.Trim();
                return ExtractAddressFromText(fullText);
            }
            catch { return null; }
        }

        // Helper methods for text extraction
        private string ExtractNameFromText(string? fullText)
        {
            if (string.IsNullOrEmpty(fullText))
                return "";

            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            return lines.Length > 0 ? lines[0].Trim() : "";
        }

        private string ExtractAddressFromText(string? fullText)
        {
            if (string.IsNullOrEmpty(fullText))
                return "";

            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            return lines.Length > 1 ? lines[1].Trim() : "";
        }
    }
}