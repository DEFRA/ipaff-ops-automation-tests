using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class LatestHealthCertificateSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ILatestHealthCertificatePage? latestHealthCertificatePage => _objectContainer.IsRegistered<ILatestHealthCertificatePage>() ? _objectContainer.Resolve<ILatestHealthCertificatePage>() : null;
        private IAccompanyingDocumentsPage? accompanyingDocumentsPage => _objectContainer.IsRegistered<IAccompanyingDocumentsPage>() ? _objectContainer.Resolve<IAccompanyingDocumentsPage>() : null;
        public LatestHealthCertificateSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Latest Health Certificate page should be displayed")]
        public void ThenTheLatestHealthCertificatePageShouldBeDisplayed()
        {
            Assert.True(latestHealthCertificatePage?.IsPageLoaded(), "Latest Health Certificate page not loaded");
        }

        [When("the user enters Latest Health Certificate Document reference {string}")]
        public void WhenTheUserEntersDocumentReference(string reference)
        {
            latestHealthCertificatePage?.EnterDocumentReference(reference);
            _scenarioContext["HealthCertificateReference"] = reference;
        }

        [When("the user enters Latest Health Certificate date of issue {string}{string}{string}")]
        public void WhenTheUserEntersLatestHealthCertificateDateOfIssue(string day, string month, string year)
        {
            latestHealthCertificatePage?.EnterDateOfIssue(day, month, year);
            var dateofIssue = day + " " + month + " " + year;
            _scenarioContext["HealthCertificateDateOfIssue"] = dateofIssue;
        }

        [When("the user enters Latest Health Certificate date of issue from yesterday")]
        public void WhenTheUserEntersLatestHealthCertificateDateOfIssueFromYesterday()
        {
            var yesterday = DateTime.Now.AddDays(-1);
            var day = yesterday.Day.ToString("D2");
            var month = yesterday.Month.ToString("D2");
            var year = yesterday.Year.ToString();

            latestHealthCertificatePage?.EnterDateOfIssue(day, month, year);
            var dateofIssue = day + " " + month + " " + year;
            _scenarioContext["HealthCertificateDateOfIssue"] = dateofIssue;
        }

        [When("the user clicks on Add attachment link on the Latest Health Certificate page")]
        public void WhenTheUserClicksOnAddAttachmentLinkOnTheLatestHealthCertificatePage()
        {
            latestHealthCertificatePage?.ClickAddAttachmentLink();
        }

        [When("the user uploads the Veterinary Health Certificate {string} in the format {string}")]
        public void WhenTheUserUploadsTheVeterinaryHealthCertificateInTheFormat(string name, string format)
        {
            var filename = name + format;
            latestHealthCertificatePage?.AddHealthCertificate(filename);
            _scenarioContext["HealthCertificateFileName"] = filename;
        }

        [Then("the Veterinary Health Certificate {string} {string} is uploaded successfully")]
        public void ThenTheVeterinaryHealthCertificateIsUploadedSuccessfully(string name, string format)
        {
            var expectedFileName = _scenarioContext.Get<string>("HealthCertificateFileName");
            var displayedFileName = latestHealthCertificatePage?.GetFileName;

            // Handle filename truncation - the UI truncates long filenames but keeps the extension
            // Based on observed behaviour: "IPAFFS Test Health Certificate.docx" becomes "IPAFFS Test Health Cer.docx"
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
                $"The Veterinary Health Certificate upload has failed. Expected '{expectedFileName}', but got '{displayedFileName}'"
            );
        }

        [When("the user clicks Latest Health Certificate add attachment link")]
        public void WhenTheUserClicksLatestHealthCertificateAddAttachmentLink()
        {
            latestHealthCertificatePage?.ClickAddAttachmentLink();
        }

        [When("the user uploads the Latest Health Certificate document {string} in the format {string}")]
        public void WhenTheUserUploadsTheLatestHealthCertificateDocumentInTheFormat(string name, string format)
        {
            var filename = name + format;
            accompanyingDocumentsPage?.AddAccompanyingDocument(filename);
            _scenarioContext["LatestHealthCertificateDocumentName"] = filename;
        }

        [Then("the Latest Health Certificate document {string} {string} is uploaded successfully")]
        public void ThenTheLatestHealthCertificateDocumentIsUploadedSuccessfully(string name, string format)
        {
            var filename = name + format;
            Assert.True(latestHealthCertificatePage?.GetFileName.Contains(filename), "The accompanying document upload has failed");

            //The date selected from date picker gets populated only after the document is uploaded. Hence, we are adding date to scenario context after attaching the document.
            var dateOfIssue = latestHealthCertificatePage?.GetDocumentIssueDate();
            _scenarioContext["LatestHealthCertificateDocumentDateOfIssue"] = dateOfIssue;
        }

        [Then("there are no Latest Health Certificate details copied from the original notification")]
        public void ThenThereAreNoLatestHealthCertificateDetailsCopiedFromTheOriginalNotification()
        {
            bool documentsArePresent = latestHealthCertificatePage?.AreHealthCertificatesPresent() ?? false;

            Assert.False(documentsArePresent,
                "Latest Health Certificate should not be copied from the original notification, but documents were found on the page");
        }

    }
}