using AventStack.ExtentReports.Gherkin.Model;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using DocumentFormat.OpenXml.VariantTypes;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.Globalization;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AccompanyingDocumentsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAccompanyingDocumentsPage? accompanyingDocumentsPage => _objectContainer.IsRegistered<IAccompanyingDocumentsPage>() ? _objectContainer.Resolve<IAccompanyingDocumentsPage>() : null;

        public AccompanyingDocumentsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Accompanying documents page should be displayed")]
        public void ThenTheAccompanyingDocumentsPageShouldBeDisplayed()
        {
            Assert.True(accompanyingDocumentsPage?.IsPageLoaded(), "Accompanying documents page is not displayed");
        }

        [Then("the Accompanying documents page should be displayed on the Inspector application")]
        public void WhenTheAccompanyingDocumentsPageShouldBeDisplayedOnTheInspectorApplication()
        {
            Assert.True(accompanyingDocumentsPage?.IsAccompanyingDocPageLoadedOnInspectorApp(), "Accompanying documents page is not displayed on the Inspector application");
        }

        [When("the user selects Document type {string}")]
        public void WhenTheUserSelectsDocumentType(string type)
        {
            accompanyingDocumentsPage?.SelectDocumentType(type);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "DocumentType", type);
        }

        [When("the user selects Document type {string} for creating border notification")]
        public void WhenTheUserSelectsDocumentTypeForCreatingBorderNotification(string type)
        {
            accompanyingDocumentsPage?.SelectDocumentType(type);
            _scenarioContext["AccompanyingDocumentTypeBN"] = type;
        }

        [When("the user selects Document type for the next notification {string}")]
        public void WhenTheUserSelectsDocumentTypeForTheNextNotification(string type)
        {
            accompanyingDocumentsPage?.SelectDocumentType(type);
            UpdateStringToScenarioContextArray(_scenarioContext, "DocumentType", type);
        }

        
        public void UpdateStringToScenarioContextArray(ScenarioContext context, string key, string value)
        {
            if (context.TryGetValue(key, out var existing) && existing is string[] current)
            {
                var updated = new string[current.Length];
                Array.Copy(current,1,updated,0,current.Length - 1);
                updated[current.Length - 1] = value;
                context[key] = updated;
            }
            else
            {
                context[key] = new[] { value };
            }
        }


        [When("the user enters Document reference {string}")]
        public void WhenTheUserEntersDocumentReference(string reference)
        {
            accompanyingDocumentsPage?.EnterDocumentReference(reference);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "DocumentReference", reference);
        }

        [When("the user enters Document reference {string} for creating border notification")]
        public void WhenTheUserEntersDocumentReferenceForCreatingBorderNotification(string reference)
        {
            accompanyingDocumentsPage?.EnterDocumentReference(reference);
            _scenarioContext["AccompanyingDocumentRefBN"] = reference;
        }

        [When("the user enters Document reference for the next notification {string}")]
        public void WhenTheUserEntersDocumentReferenceForTheNextNotification(string reference)
        {
            accompanyingDocumentsPage?.EnterDocumentReference(reference);
            UpdateStringToScenarioContextArray(_scenarioContext, "DocumentReference", reference);
        }

        [When("the user enters date of issue {string}")]
        public void WhenTheUserEntersDateOfIssue(string dateString)
        {
            var date = Utils.ConvertToDate(dateString);
            accompanyingDocumentsPage?.EnterDateOfIssue(date.Day.ToString(), date.Month.ToString(), date.Year.ToString());
            var monthName = date.ToString("MMMM", CultureInfo.InvariantCulture);
            var dateofIssue = date.Day.ToString() + " " + monthName + " " + date.Year.ToString();
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "DateOfIssue", dateofIssue);
        }

        [When("the user enters date of issue for the next notification {string}")]
        public void WhenTheUserEntersDateOfIssueForTheNextNotification(string dateString)
        {
            var (day, month, year) = Utils.GetDayMonthYear(dateString);
            accompanyingDocumentsPage?.EnterDateOfIssue(day, month, year);

            var display = new DateTime(int.Parse(year, CultureInfo.InvariantCulture), int.Parse(month, CultureInfo.InvariantCulture), int.Parse(day, CultureInfo.InvariantCulture)).ToString("d MMMM yyyy", CultureInfo.InvariantCulture);
            UpdateStringToScenarioContextArray(_scenarioContext, "DateOfIssue", display);
        }

        [When("the user selects a future date from the date picker")]
        public void WhenTheUserSelectsAFutureDateFromTheDatePicker()
        {
            Assert.True(accompanyingDocumentsPage?.IsDatePickerIconDisplayed(), "Date picker icon is not displayed on the Accompanying documents page");
            accompanyingDocumentsPage?.SelectDateFromDatePicker();
        }

        [When("the user selects a previous date from the date picker")]
        public void WhenTheUserSelectsAPreviousDateFromTheDatePicker()
        {
            var previousDay = DateTime.Now.AddDays(-1).Day.ToString();

            Assert.True(accompanyingDocumentsPage?.IsDatePickerIconDisplayed(), "Date picker icon is not displayed on the Accompanying documents page");
            accompanyingDocumentsPage?.SelectPreviousDateFromDatePicker(previousDay);
        }

        [When("the user clicks on Add attachment link")]
        public void WhenTheUserClicksOnAddAttachmentLink()
        {
            accompanyingDocumentsPage?.ClickAddAttachmentLink();
        }

        [When("the user uploads the document {string} in the format {string}")]
        [When("the user uploads the document {string} in the format {string} that exceeds size limit")]
        [When("the user uploads a document with a filename longer than {int} characters {string} in the format {string}")]
        [When("the user uploads the document {string} in the format {string} as no document is attached by the copy")]
        public void WhenTheUserUploadsTheDocumentInTheFormat(string name, string format)
        {
            var filename = name + format;
            accompanyingDocumentsPage?.AddAccompanyingDocument(filename);

            if(!filename.Contains("Exceeds size"))
                _scenarioContext["DocumentName"] = filename;
        }

        [When("the user uploads the document {string} in the format {string} for creating border notification")]
        public void WhenTheUserUploadsTheDocumentInTheFormatForCreatingBorderNotification(string name, string format)
        {
            var filename = name + format;
            accompanyingDocumentsPage?.AddAccompanyingDocument(filename);

            _scenarioContext["AccompanyingDocumentFileNameBN"] = filename;
        }

        [Then("the document {string} {string} is uploaded successfully")]
        public void ThenTheDocumentIsUploadedSuccessfully(string name, string format)
        {
            var expectedFileName = name + format;
            var displayedFileName = accompanyingDocumentsPage?.GetFileName;

            // Handle filename truncation - the UI truncates long filenames but keeps the extension
            var isMatch = false;

            if (!string.IsNullOrEmpty(displayedFileName) && !string.IsNullOrEmpty(expectedFileName))
            {
                var displayedExtension = Path.GetExtension(displayedFileName);
                var expectedExtension = Path.GetExtension(expectedFileName);

                var displayedNameWithoutExt = Path.GetFileNameWithoutExtension(displayedFileName);
                var expectedNameWithoutExt = Path.GetFileNameWithoutExtension(expectedFileName);

                // Check if extensions match and displayed name is the start of expected name (handles truncation)
                isMatch = displayedExtension.Equals(expectedExtension, StringComparison.OrdinalIgnoreCase) &&
                          expectedNameWithoutExt.StartsWith(displayedNameWithoutExt, StringComparison.OrdinalIgnoreCase);
            }

            Assert.True(
                isMatch,
                $"The accompanying document upload has failed. Expected '{expectedFileName}', but got '{displayedFileName}'"
            );

            // Verify Download link is present
            Assert.True(
                accompanyingDocumentsPage?.IsDownloadAttachmentLinkPresent(),
                $"Download attachment link is not present for document '{expectedFileName}'"
            );

            // Verify Remove attachment link is present
            Assert.True(
                accompanyingDocumentsPage?.IsRemoveAttachmentLinkPresent(),
                $"Remove attachment link is not present for document '{expectedFileName}'"
            );

            //The date selected from date picker gets populated only after the document is uploaded. Hence, we are adding date to scenario context after attaching the document.
            //Only add if it doesn't already exist (e.g., when date was manually entered instead of using date picker)
            if (!_scenarioContext.ContainsKey("DocumentDateOfIssue"))
            {
                var dateOfIssue = accompanyingDocumentsPage?.GetDocumentIssueDate();
                _scenarioContext["DocumentDateOfIssue"] = dateOfIssue;
            }
        }

        [Then("the file name should contain {int} characters including the file type")]
        public void ThenTheFileNameShouldContainCharactersIncludingTheFileType(int fileLength)
        {
            Assert.True(fileLength.Equals(accompanyingDocumentsPage.GetFileLength), "File name length should be 27 characters after truncation");
        }

        [When("the user enters date of issue {string}{string}{string}")]
        public void WhenTheUserEntersDateOfIssue(string day, string month, string year)
        {
            accompanyingDocumentsPage?.EnterDateOfIssue(day, month, year);
            var dateofIssue = day + " " + month + " " + year;
            _scenarioContext["DocumentDateOfIssue"] = dateofIssue;
        }

        [When("the user enters date of issue from last week")]
        public void WhenTheUserEntersDateOfIssueFromLastWeek()
        {
            var lastWeekDate = DateTime.Now.AddDays(-7);

            // Format day and month with leading zeros to match input format
            var day = lastWeekDate.Day.ToString("00");
            var month = lastWeekDate.Month.ToString("00");
            var year = lastWeekDate.Year.ToString();

            accompanyingDocumentsPage?.EnterDateOfIssue(day, month, year);

            // Store in "dd MM yyyy" format to match HealthCertificateDateOfIssue
            var dateofIssue = $"{day} {month} {year}";  // "01 01 2026"
            _scenarioContext["DocumentDateOfIssue"] = dateofIssue;
        }

        [When(@"the user clicks the Add a document link")]
        public void WhenTheUserClicksTheAddADocumentLink()
        {
            accompanyingDocumentsPage?.ClickAddADocument();
        }

        [Then("the error message for exceeding file size should be displayed")]
        public void ThenTheErrorMessageForExceedingFileSizeShouldBeDisplayed()
        {
            Assert.True(accompanyingDocumentsPage?.ValidateDocUploadErrors(), "Error messages are not displayed when the document size exceeds 10 MB");
        }

        [When("the user clicks on Cancel link")]
        public void WhenTheUserClicksOnCancelLink()
        {
            accompanyingDocumentsPage?.ClickCancelLink();
        }
    }
}