using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
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
            Assert.True(accompanyingDocumentsPage?.IsPageLoaded(), "Accompanying documents");
        }


        [When("the user selects Document type {string}")]
        public void WhenTheUserSelectsDocumentType(string type)
        {
            accompanyingDocumentsPage?.SelectDocumentType(type);
            _scenarioContext.Add("DocumentType", type);
        }

        [When("the user enters Document reference {string}")]
        public void WhenTheUserEntersDocumentReference(string reference)
        {
            accompanyingDocumentsPage?.EnterDocumentReference(reference);
            _scenarioContext.Add("DocumentReference", reference);
        }

        [When("the user enters date of issue {string}")]
        public void WhenTheUserEntersDateOfIssue(string dateString)
        {
            var date = Utils.ConvertToDate(dateString);
            accompanyingDocumentsPage?.EnterDateOfIssue(date.Day.ToString(), date.Month.ToString(), date.Year.ToString());
            var monthName = date.ToString("MMMM", CultureInfo.InvariantCulture);
            var dateofIssue = date.Day.ToString() + " " + monthName + " " + date.Year.ToString();
            _scenarioContext.Add("DateOfIssue", dateofIssue);
        }

        [When("the user selects a future date from the date picker")]
        public void WhenTheUserSelectsAFutureDateFromTheDatePicker()
        {
            Assert.True(accompanyingDocumentsPage?.IsDatePickerIconDisplayed(), "Date picker icon is not displayed on the Accompanying documents page");
            accompanyingDocumentsPage?.SelectDateFromDatePicker();
        }

        [When("the user clicks on Add attachment link")]
        public void WhenTheUserClicksOnAddAttachmentLink()
        {
            accompanyingDocumentsPage?.ClickAddAttachmentLink();
        }

        [When("the user uploads the document {string} in the format {string}")]
        public void WhenTheUserUploadsTheDocumentInTheFormat(string name, string format)
        {
            var filename = name + format;
            accompanyingDocumentsPage?.AddAccompanyingDocument(filename);
            _scenarioContext.Add("DocumentName", filename);
        }

        [Then("the document {string} {string} is uploaded successfully")]
        public void ThenTheDocumentIsUploadedSuccessfully(string name, string format)
        {
            var filename = name + format;
            Assert.True(accompanyingDocumentsPage?.GetFileName.Contains(filename), "The accompanying document upload has failed");

            //The date selected from date picker gets populated only after the document is uploaded. Hence, we are adding date to scenario context after attaching the document.
            var dateOfIssue = accompanyingDocumentsPage?.GetDocumentIssueDate();
            _scenarioContext.Add("DocumentDateOfIssue", dateOfIssue);
        }

        [When("the user enters date of issue {string}{string}{string}")]
        public void WhenTheUserEntersDateOfIssue(string day, string month, string year)
        {
            accompanyingDocumentsPage?.EnterDateOfIssue(day, month, year);
            var dateofIssue = day + " " + month + " " + year;
            _scenarioContext.Add("DocumentDateOfIssue", dateofIssue);
        }
    }
}