using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddCatchCertificateDetailsPage : IAddCatchCertificateDetailsPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement setCatchCertificateReference(int index) => _driver.WaitForElement(By.Id($"catch-certificate-reference-{index}"));
        private IWebElement addCatchCertificateText(string text) => _driver.FindElement(By.XPath($"//*[normalize-space(text())='{text}']"));
        private IWebElement dateOfIssue(string date, int index) => _driver.FindElement(By.Id($"date-of-issue-{date}-{index}"));
        private IWebElement imgCalendar => _driver.FindElement(By.XPath("//div[@id='date-of-issue-1']//button[contains(@class, 'date-picker')]//span"));
        private IWebElement setFlagStateOfCatchingVessel(int index) => _driver.FindElement(By.Id($"flag-state-{index}"));
        private IWebElement selectSpecies(int position) => _driver.FindElement(By.XPath($"(//*[contains(@id,'hidden-species')]//input[@type='checkbox'])[{position}]"));
        private IWebElement lnkChange => _driver.FindElement(By.XPath("//span[contains(@class, 'govuk-details__summary-text')]"));
        private IWebElement txtNoOfCatchCertificates => _driver.FindElement(By.Id("number-of-catch-certificates"));
        private IWebElement lstNoOfCatchCertificatRefereceSections => _driver.FindElement(By.Id("catch-certificate-details-3"));
        private IWebElement btnUpdate => _driver.FindElement(By.Id("update-number-of-catch-certificates"));
        private IWebElement setUpdateDetails(int index) => _driver.FindElement(By.Id($"update-catch-certificate-details-{index}"));
        private IWebElement lnkSaveAndReturnToManageCertificate => _driver.FindElement(By.Id("save-and-return-manage-catch-certificates"));
        private IWebElement lnkSaveAndReturnToHub => _driver.FindElement(By.Id("save-and-return"));
        private IWebElement selectSpeciesByName(string name) => _driver.FindElement(By.XPath($"//div[@id='checkbox-{name}-1']//input"));
        private IWebElement selectSpecies(string name, int certificateNumber) => _driver.FindElement(By.XPath($"//div[@id='checkbox-{name}-{certificateNumber}']//input"));
        private IWebElement attachmentCaption => _driver.FindElement(By.XPath("//span[contains(@class, 'govuk-caption-l')]"));
        private IWebElement numberOfCertificatesLabel => _driver.FindElement(By.XPath("//div[@class='govuk-summary-list__key']"));
        private IWebElement changeLink => _driver.FindElement(By.XPath("//span[contains(@class, 'govuk-details__summary-text')]"));
        private IWebElement catchCertificateReferenceField => _driver.FindElement(By.Id("catch-certificate-reference-1"));
        private IWebElement catchCertificateReferenceLabel => _driver.FindElement(By.XPath("//label[@for='catch-certificate-reference-1']"));
        private IWebElement dateOfIssueLabel => _driver.FindElement(By.XPath("//div[contains(@class, 'govuk-label--s')]"));
        private IWebElement dateOfIssueDayField => _driver.FindElement(By.Id("date-of-issue-day-1"));
        private IWebElement dateOfIssueMonthField => _driver.FindElement(By.Id("date-of-issue-month-1"));
        private IWebElement dateOfIssueYearField => _driver.FindElement(By.Id("date-of-issue-year-1"));
        private IWebElement calendarIcon => _driver.FindElement(By.XPath("//div[@id='date-of-issue-1']//button[contains(@class, 'date-picker')]"));
        private IWebElement flagStateLabel => _driver.FindElement(By.XPath("//label[@for='flag-state-1']"));
        private IWebElement flagStateField => _driver.FindElement(By.Id("flag-state-1"));
        private IWebElement selectSpeciesHeading => _driver.FindElement(By.XPath("//h2[contains(@class, 'govuk-heading-m')]"));
        private IWebElement selectAllCheckbox => _driver.FindElement(By.XPath("//label[@for='select-all-checkbox-1']"));
        private IWebElement saveAndContinueButton => _driver.FindElement(By.Id("button-save-and-continue"));
        private IWebElement saveAndReturnToManageCatchCertificatesLink => _driver.FindElement(By.Id("save-and-return-manage-catch-certificates"));
        private IWebElement lnkAddTheCommodity => _driver.FindElement(By.LinkText("add the commodity"));
        private IWebElement chkSelectAll => _driver.FindElement(By.Id("select-all-checkbox-1"));
        private IReadOnlyCollection<IWebElement> speciesCheckboxes => _driver.FindElements(By.XPath("//div[@id='commodities-species-table-1']//input[@type='checkbox' and contains(@id, 'species-')]"));
        private IReadOnlyCollection<IWebElement> catchCertificateReferenceErrors => _driver.FindElements(By.XPath("//p[@class='govuk-error-message' and contains(text(), 'catch certificate reference')]"));
        private IReadOnlyCollection<IWebElement> flagStateErrors => _driver.FindElements(By.XPath("//p[@class='govuk-error-message' and contains(text(), 'flag state')]"));
        private IReadOnlyCollection<IWebElement> dateOfIssueErrors => _driver.FindElements(By.Id("date-of-issue-error"));
        private IReadOnlyCollection<IWebElement> dropdownOptions => _driver.FindElements(By.XPath("//ul[@id='flag-state-1__listbox']//li[@role='option']"));

        // Error validation
        private IReadOnlyCollection<IWebElement> lblErrorMessages => _driver.FindElements(errorMessageListItemsBy);
        private By errorSummaryBy => By.XPath("//div[@class='govuk-error-summary']");
        private By errorMessageListItemsBy => By.XPath("//div[@class='govuk-error-summary']//ul[@class='govuk-list govuk-error-summary__list']/li");
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AddCatchCertificateDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        private static string NormalizeWhitespace(string text)
        {
            return Regex.Replace(text?.Trim() ?? string.Empty, @"\s+", " ");
        }

        public bool IsPageLoaded(string pageTitle)
        {
            return NormalizeWhitespace(primaryTitle.Text).Equals(pageTitle, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyContent(string content)
        {
            return NormalizeWhitespace(addCatchCertificateText(content).Text).Equals(content, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyCalendar()
        {
            return NormalizeWhitespace(imgCalendar.Text).Equals("Choose date", StringComparison.OrdinalIgnoreCase);
        }

        public void EnterCatchCertificateReference(string reference, int index = 1)
        {
            setCatchCertificateReference(index).SendKeys(reference);
        }

        public void EnterDateOfIssue(string day, string month, string year, int index = 1)
        {
            dateOfIssue("day", index).SendKeys(day);
            dateOfIssue("month", index).SendKeys(month);
            dateOfIssue("year", index).SendKeys(year);
        }

        public void EnterFlagStateOfCatchingVessel(string FlagState, int index = 1)
        {
            setFlagStateOfCatchingVessel(index).Clear();
            setFlagStateOfCatchingVessel(index).SendKeys($"{FlagState}{Keys.ArrowDown}{Keys.Enter}");
        }

        public void SelectSpecies(int species) => selectSpecies(species).Click();

        public void SelectSpeciesByName(string species) => selectSpeciesByName(species).Click();
        public void SelectSpecies(string species, int certificateNumber) => selectSpecies(species, certificateNumber).Click();

        public void ClickChangeLink() => lnkChange.Click();

        public void EnterNumberOfCatchCertificates(string noOfCertificateRef)
        {
            txtNoOfCatchCertificates.Clear();
            txtNoOfCatchCertificates.SendKeys(noOfCertificateRef.ToString());
        }

        public bool VerifyNoOfCatchReferenceSections(int numberOfRefBlocks)
        {
            return lstNoOfCatchCertificatRefereceSections.Displayed;
        }

        public void ClickUpdate() => btnUpdate.Click();

        public void ClickUpdate(int index) => setUpdateDetails(index).Click();

        public void ClickSaveAndReturnToManageCertificateLink() => lnkSaveAndReturnToManageCertificate.Click();

        public void ClickAddTheCommodityLink() => lnkAddTheCommodity.Click();

        public bool VerifyAttachmentNumberDisplayed(int attachmentNumber, int totalAttachments)
        {
            return attachmentCaption.Displayed &&
                   NormalizeWhitespace(attachmentCaption.Text).Equals($"Attachment {attachmentNumber} of {totalAttachments}", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyNumberOfCatchCertificatesWithChangeLinkDisplayed()
        {
            return numberOfCertificatesLabel.Displayed &&
                   NormalizeWhitespace(numberOfCertificatesLabel.Text).Equals("Number of catch certificates in this attachment", StringComparison.OrdinalIgnoreCase) &&
                   changeLink.Displayed &&
                   NormalizeWhitespace(changeLink.Text).Equals("Change", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyCatchCertificateReferenceFieldDisplayed()
        {
            return catchCertificateReferenceLabel.Displayed &&
                   NormalizeWhitespace(catchCertificateReferenceLabel.Text).Contains("Catch certificate reference", StringComparison.OrdinalIgnoreCase) &&
                   catchCertificateReferenceField.Displayed;
        }

        public bool VerifyDateOfIssueFieldsWithCalendarDisplayed()
        {
            return dateOfIssueLabel.Displayed &&
                   NormalizeWhitespace(dateOfIssueLabel.Text).Equals("Date of issue", StringComparison.OrdinalIgnoreCase) &&
                   dateOfIssueDayField.Displayed &&
                   dateOfIssueMonthField.Displayed &&
                   dateOfIssueYearField.Displayed &&
                   calendarIcon.Displayed;
        }

        public bool VerifyFlagStateFieldDisplayed()
        {
            return flagStateLabel.Displayed &&
                   NormalizeWhitespace(flagStateLabel.Text).Contains("Flag state of catching vessel", StringComparison.OrdinalIgnoreCase) &&
                   flagStateField.Displayed;
        }

        public bool VerifySelectSpeciesWithSelectAllDisplayed()
        {
            return selectSpeciesHeading.Displayed &&
                   NormalizeWhitespace(selectSpeciesHeading.Text).Equals("Select species being imported under this catch certificate", StringComparison.OrdinalIgnoreCase) &&
                   selectAllCheckbox.Displayed &&
                   NormalizeWhitespace(selectAllCheckbox.Text).Equals("Select all", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifySaveAndContinueButtonDisplayed()
        {
            return saveAndContinueButton.Displayed &&
                   NormalizeWhitespace(saveAndContinueButton.Text).Equals("Save and continue", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifySaveAndReturnToManageCatchCertificatesLinkDisplayed()
        {
            return saveAndReturnToManageCatchCertificatesLink.Displayed &&
                   NormalizeWhitespace(saveAndReturnToManageCatchCertificatesLink.Text).Equals("Save and return to manage catch certificates", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifySaveAndReturnToHubLinkDisplayed()
        {
            return lnkSaveAndReturnToHub.Displayed &&
                   NormalizeWhitespace(lnkSaveAndReturnToHub.Text).Equals("Save and return to hub", StringComparison.OrdinalIgnoreCase);
        }

        public void StartTypingInFlagState(string partialText, int index = 1)
        {
            var flagStateField = setFlagStateOfCatchingVessel(index);
            flagStateField.Clear();
            flagStateField.SendKeys(partialText);
        }

        public bool VerifyDropdownOptionsInclude(string optionText)
        {
            return dropdownOptions.Any(option =>
                NormalizeWhitespace(option.Text).Contains(optionText, StringComparison.OrdinalIgnoreCase));
        }

        public void SelectFromDropdown(string optionText)
        {
            var matchingOption = dropdownOptions.FirstOrDefault(option =>
                NormalizeWhitespace(option.Text).Equals(optionText, StringComparison.OrdinalIgnoreCase));

            matchingOption?.Click();
        }

        public bool IsFieldHighlighted(string fieldName, int index = 1)
        {
            if (fieldName.Contains("catch certificate reference", StringComparison.OrdinalIgnoreCase))
                return catchCertificateReferenceErrors.Count > 0 && catchCertificateReferenceErrors.First().Displayed;

            if (fieldName.Contains("flag state", StringComparison.OrdinalIgnoreCase))
                return flagStateErrors.Count > 0 && flagStateErrors.First().Displayed;

            if (fieldName.Contains("date of issue", StringComparison.OrdinalIgnoreCase))
                return dateOfIssueErrors.Count > 0 && dateOfIssueErrors.First().Displayed;

            return false;
        }

        public (bool allErrorsPresent, string errorMessages) VerifySpecificErrorsDisplayed(params string[] expectedErrors)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                wait.Until(driver => driver.FindElements(errorSummaryBy).Count > 0);
                wait.Until(driver => driver.FindElements(errorMessageListItemsBy).Count > 0);
                wait.Until(driver =>
                {
                    var messages = driver.FindElements(errorMessageListItemsBy);
                    return messages.Any(msg => !string.IsNullOrWhiteSpace(msg.Text));
                });
            }
            catch (WebDriverTimeoutException)
            {
                return (false, "No error messages displayed (timed out waiting for error list to populate)");
            }

            var errorSummaryElements = _driver.FindElements(errorSummaryBy);
            if (errorSummaryElements.Count == 0)
                return (false, "No error banner displayed");

            var errorMessagesList = new List<string>();
            foreach (var element in lblErrorMessages)
            {
                var errorText = NormalizeWhitespace(element.Text);
                if (!string.IsNullOrWhiteSpace(errorText))
                    errorMessagesList.Add(errorText);
            }

            if (errorMessagesList.Count == 0)
                return (false, "Error banner displayed but no error messages found in the list");

            var allErrorMessages = string.Join("; ", errorMessagesList);

            var missingErrors = expectedErrors
                .Where(e => !errorMessagesList.Any(msg => msg.Contains(e, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (missingErrors.Any())
                return (false, $"Missing errors: [{string.Join("; ", missingErrors)}]. All errors displayed: {allErrorMessages}");

            return (true, allErrorMessages);
        }

        public void ClickSelectAllCheckbox() => chkSelectAll.Click();

        public bool VerifyAllSpeciesAreSelected()
        {
            if (!chkSelectAll.Selected)
                return false;

            foreach (var checkbox in speciesCheckboxes)
            {
                if (!checkbox.Selected)
                    return false;
            }

            return true;
        }

        public void ClickSaveAndReturnToHubLink() => lnkSaveAndReturnToHub.Click();
    }
}