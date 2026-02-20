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
        private IWebElement selectAll => _driver.FindElement(By.Id("select-all-checkbox-1"));
        private IWebElement selectSpecies(int position) => _driver.FindElement(By.XPath($"(//*[contains(@id,'hidden-species')]//input[@type='checkbox'])[{position}]"));
        private IWebElement lnkChange => _driver.FindElement(By.XPath("//span[contains(@class, 'govuk-details__summary-text')]"));
        private IWebElement txtNoOfCatchCertificates => _driver.FindElement(By.Id("number-of-catch-certificates"));
        private IWebElement lstNoOfCatchCertificatRefereceSections => _driver.FindElement(By.Id("catch-certificate-details-3"));
        private IWebElement btnUpdate => _driver.FindElement(By.Id("update-number-of-catch-certificates"));
        private IWebElement setUpdateDetails(int index) => _driver.FindElement(By.Id($"update-catch-certificate-details-{index}"));
        private IWebElement lnkSaveAndReturnToManageCertificate => _driver.FindElement(By.Id("save-and-return-manage-catch-certificates"));
        private IWebElement selectSpeciesByName(string name) => _driver.FindElement(By.XPath($"//div[@id='checkbox-{name}-1']//input"));
        private By attachmentCaptionBy => By.XPath("//span[contains(@class, 'govuk-caption-l')]");
        private By numberOfCertificatesLabelBy => By.XPath("//div[@class='govuk-summary-list__key']");
        private By changeLinkBy => By.XPath("//span[contains(@class, 'govuk-details__summary-text')]");
        private By catchCertificateReferenceFieldBy => By.Id("catch-certificate-reference-1");
        private By catchCertificateReferenceLabelBy => By.XPath("//label[@for='catch-certificate-reference-1']");
        private By dateOfIssueLabelBy => By.XPath("//div[contains(@class, 'govuk-label--s')]");
        private By dateOfIssueDayBy => By.Id("date-of-issue-day-1");
        private By dateOfIssueMonthBy => By.Id("date-of-issue-month-1");
        private By dateOfIssueYearBy => By.Id("date-of-issue-year-1");
        private By calendarIconBy => By.XPath("//div[@id='date-of-issue-1']//button[contains(@class, 'date-picker')]");
        private By flagStateLabelBy => By.XPath("//label[@for='flag-state-1']");
        private By flagStateFieldBy => By.Id("flag-state-1");
        private By selectSpeciesHeadingBy => By.XPath("//h2[contains(@class, 'govuk-heading-m')]");
        private By selectAllCheckboxBy => By.XPath("//label[@for='select-all-checkbox-1']");
        private By saveAndContinueButtonBy => By.Id("button-save-and-continue");
        private By saveAndReturnToManageCatchCertificatesLinkBy => By.Id("save-and-return-manage-catch-certificates");
        private By saveAndReturnToHubLinkBy => By.Id("save-and-return");
        private IWebElement lnkAddTheCommodity => _driver.FindElement(By.LinkText("add the commodity"));

        // Error validation
        private IReadOnlyCollection<IWebElement> lblErrorMessages => _driver.FindElements(By.XPath("//div[@class='govuk-error-summary']//ul[@class='govuk-list govuk-error-summary__list']/li"));
        private By errorSummaryBy => By.XPath("//div[@class='govuk-error-summary']");
        private By catchCertificateReferenceErrorBy => By.XPath("//span[@class='govuk-error-message' and contains(text(), 'catch certificate reference')]");
        private By flagStateErrorBy => By.XPath("//span[@class='govuk-error-message' and contains(text(), 'flag state')]");

        // Dropdown options
        private By dropdownOptionsBy => By.XPath("//ul[@id='flag-state-1__listbox']//li[@role='option']");
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

        public void SelectSpecies(int species)
        {
            selectSpecies(species).Click();
        }

        public void SelectSpeciesByName(string species)
        {
            selectSpeciesByName(species).Click();
        }

        public void ClickChangeLink()
        {
            lnkChange.Click();
        }

        public void EnterNumberOfCatchCertificates(string noOfCertificateRef)
        {
            txtNoOfCatchCertificates.Clear();
            txtNoOfCatchCertificates.SendKeys(noOfCertificateRef.ToString());
        }

        public bool VerifyNoOfCatchReferenceSections(int numberOfRefBlocks)
        {
            return lstNoOfCatchCertificatRefereceSections.Displayed;
        }

        public void ClickUpdate()
        {
            btnUpdate.Click();
        }

        public void ClickUpdate(int index)
        {
            setUpdateDetails(index).Click();
        }

        public void ClickSaveAndReturnToManageCertificateLink()
        {
            lnkSaveAndReturnToManageCertificate.Click();
        }

        public void ClickAddTheCommodityLink()
        {
            lnkAddTheCommodity.Click();
        }

        public bool VerifyAttachmentNumberDisplayed(int attachmentNumber, int totalAttachments)
        {
            var caption = _driver.FindElement(attachmentCaptionBy);
            var expectedText = $"Attachment {attachmentNumber} of {totalAttachments}";
            var actualText = NormalizeWhitespace(caption.Text);

            return caption.Displayed && actualText.Equals(expectedText, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyNumberOfCatchCertificatesWithChangeLinkDisplayed()
        {
            var label = _driver.FindElement(numberOfCertificatesLabelBy);
            var changeLink = _driver.FindElement(changeLinkBy);

            var labelText = NormalizeWhitespace(label.Text);
            var linkText = NormalizeWhitespace(changeLink.Text);

            return label.Displayed &&
                   labelText.Equals("Number of catch certificates in this attachment", StringComparison.OrdinalIgnoreCase) &&
                   changeLink.Displayed &&
                   linkText.Equals("Change", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyCatchCertificateReferenceFieldDisplayed()
        {
            var label = _driver.FindElement(catchCertificateReferenceLabelBy);
            var field = _driver.FindElement(catchCertificateReferenceFieldBy);

            var labelText = NormalizeWhitespace(label.Text);

            return label.Displayed &&
                   labelText.Contains("Catch certificate reference", StringComparison.OrdinalIgnoreCase) &&
                   field.Displayed;
        }

        public bool VerifyDateOfIssueFieldsWithCalendarDisplayed()
        {
            var label = _driver.FindElement(dateOfIssueLabelBy);
            var dayField = _driver.FindElement(dateOfIssueDayBy);
            var monthField = _driver.FindElement(dateOfIssueMonthBy);
            var yearField = _driver.FindElement(dateOfIssueYearBy);
            var calendarIcon = _driver.FindElement(calendarIconBy);

            var labelText = NormalizeWhitespace(label.Text);

            return label.Displayed &&
                   labelText.Equals("Date of issue", StringComparison.OrdinalIgnoreCase) &&
                   dayField.Displayed &&
                   monthField.Displayed &&
                   yearField.Displayed &&
                   calendarIcon.Displayed;
        }

        public bool VerifyFlagStateFieldDisplayed()
        {
            var label = _driver.FindElement(flagStateLabelBy);
            var field = _driver.FindElement(flagStateFieldBy);

            var labelText = NormalizeWhitespace(label.Text);

            return label.Displayed &&
                   labelText.Contains("Flag state of catching vessel", StringComparison.OrdinalIgnoreCase) &&
                   field.Displayed;
        }

        public bool VerifySelectSpeciesWithSelectAllDisplayed()
        {
            var heading = _driver.FindElement(selectSpeciesHeadingBy);
            var selectAllCheckbox = _driver.FindElement(selectAllCheckboxBy);

            var headingText = NormalizeWhitespace(heading.Text);
            var checkboxText = NormalizeWhitespace(selectAllCheckbox.Text);

            var expectedHeading = "Select species being imported under this catch certificate";

            return heading.Displayed &&
                   headingText.Equals(expectedHeading, StringComparison.OrdinalIgnoreCase) &&
                   selectAllCheckbox.Displayed &&
                   checkboxText.Equals("Select all", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifySaveAndContinueButtonDisplayed()
        {
            var button = _driver.FindElement(saveAndContinueButtonBy);
            var buttonText = NormalizeWhitespace(button.Text);

            return button.Displayed &&
                   buttonText.Equals("Save and continue", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifySaveAndReturnToManageCatchCertificatesLinkDisplayed()
        {
            var link = _driver.FindElement(saveAndReturnToManageCatchCertificatesLinkBy);
            var linkText = NormalizeWhitespace(link.Text);

            return link.Displayed &&
                   linkText.Equals("Save and return to manage catch certificates", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifySaveAndReturnToHubLinkDisplayed()
        {
            var link = _driver.FindElement(saveAndReturnToHubLinkBy);
            var linkText = NormalizeWhitespace(link.Text);

            return link.Displayed &&
                   linkText.Equals("Save and return to hub", StringComparison.OrdinalIgnoreCase);
        }

        public void StartTypingInFlagState(string partialText, int index = 1)
        {
            var flagStateField = setFlagStateOfCatchingVessel(index);
            flagStateField.Clear();
            flagStateField.SendKeys(partialText);
        }

        public bool VerifyDropdownOptionsInclude(string optionText)
        {
            var options = _driver.FindElements(dropdownOptionsBy);
            return options.Any(option =>
                NormalizeWhitespace(option.Text).Contains(optionText, StringComparison.OrdinalIgnoreCase));
        }

        public void SelectFromDropdown(string optionText)
        {
            var options = _driver.WaitForElements(dropdownOptionsBy);
            var matchingOption = options.FirstOrDefault(option =>
                NormalizeWhitespace(option.Text).Equals(optionText, StringComparison.OrdinalIgnoreCase));

            if (matchingOption != null)
            {
                matchingOption.Click();
            }
        }

        public bool IsFieldHighlighted(string fieldName, int index = 1)
        {
            try
            {
                if (fieldName.Contains("catch certificate reference", StringComparison.OrdinalIgnoreCase))
                {
                    var errorElements = _driver.FindElements(catchCertificateReferenceErrorBy);
                    return errorElements.Count > 0 && errorElements.First().Displayed;
                }
                else if (fieldName.Contains("flag state", StringComparison.OrdinalIgnoreCase))
                {
                    var errorElements = _driver.FindElements(flagStateErrorBy);
                    return errorElements.Count > 0 && errorElements.First().Displayed;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public (bool allErrorsPresent, string errorMessages) VerifySpecificErrorsDisplayed(params string[] expectedErrors)
        {
            try
            {
                // Wait explicitly for error banner to appear (up to 10 seconds)
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                wait.Until(driver => driver.FindElements(errorSummaryBy).Count > 0);
            }
            catch (WebDriverTimeoutException)
            {
                return (false, "No error banner displayed (timed out waiting for error summary)");
            }

            var errorSummaryElements = _driver.FindElements(errorSummaryBy);
            if (errorSummaryElements.Count == 0)
            {
                return (false, "No error banner displayed");
            }

            // Collect all error messages
            var errorMessagesList = new List<string>();
            foreach (var element in lblErrorMessages)
            {
                errorMessagesList.Add(NormalizeWhitespace(element.Text));
            }

            var allErrorMessages = string.Join("; ", errorMessagesList);

            // Check if ALL expected errors are present
            var missingErrors = new List<string>();
            foreach (var expectedError in expectedErrors)
            {
                var found = errorMessagesList.Any(msg =>
                    msg.Contains(expectedError, StringComparison.OrdinalIgnoreCase));

                if (!found)
                {
                    missingErrors.Add(expectedError);
                }
            }

            if (missingErrors.Any())
            {
                var missing = string.Join("; ", missingErrors);
                return (false, $"Missing errors: [{missing}]. All errors displayed: {allErrorMessages}");
            }

            return (true, allErrorMessages);
        }
    }
}