using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class DocumentsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IDocumentsPage? documentsPage => _objectContainer.IsRegistered<IDocumentsPage>() ? _objectContainer.Resolve<IDocumentsPage>() : null;

        public DocumentsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Documents page should be displayed")]
        public void ThenTheDocumentsPageShouldBeDisplayed()
        {
            Assert.True(documentsPage?.IsPageLoaded(), "Documents page is not displayed");
        }

        [Then("the Add another document link is displayed")]
        public void ThenTheAddAnotherDocumentLinkIsDisplayed()
        {
            Assert.True(documentsPage?.IsAddAnotherDocumentLinkDisplayed(), "Add another document link is not displayed");
        }

        [Then("the user verifies there are no documents in the Inspector section")]
        public void ThenTheUserVerifiesThereAreNoDocumentsInTheInspectorSection()
        {
            Assert.True(documentsPage?.VerifyNoDocumentsInInspectorSection(),
                "Expected no documents in Inspector section, but documents were found");
        }

        [Then("the user verifies the entered document information is displayed correctly")]
        public void ThenTheUserVerifiesTheEnteredDocumentInformationIsDisplayedCorrectly()
        {
            // Get the document details from the scenario context
            // DocumentType and DocumentReference are stored as string[] arrays
            var expectedDocumentType = _scenarioContext.Get<string[]>("DocumentType1")[0];
            var expectedDocumentReference = _scenarioContext.Get<string[]>("DocumentReference1")[0];

            // DocumentDateOfIssue is stored as a single string
            var expectedDateOfIssue = _scenarioContext.Get<string>("DocumentDateOfIssue1");

            // Convert the date format from "dd MM yyyy" to "d MMMM yyyy" (e.g., "05 12 2025" to "5 December 2025")
            var actualDateOfIssue = ConvertDateFormat(expectedDateOfIssue);

            // Get the actual document details from the page
            var (actualDocumentType, actualDocumentReference, actualDisplayedDate) = documentsPage?.GetInspectorDocumentDetails(0)
                ?? (null, null, null);

            // Verify document type
            Assert.That(actualDocumentType, Is.EqualTo(expectedDocumentType),
                $"Document type mismatch. Expected: '{expectedDocumentType}', Actual: '{actualDocumentType}'");

            // Verify document reference
            Assert.That(actualDocumentReference, Is.EqualTo(expectedDocumentReference),
                $"Document reference mismatch. Expected: '{expectedDocumentReference}', Actual: '{actualDocumentReference}'");

            // Verify date of issue
            Assert.That(actualDisplayedDate, Is.EqualTo(actualDateOfIssue),
                $"Date of issue mismatch. Expected: '{actualDateOfIssue}', Actual: '{actualDisplayedDate}'");

            Console.WriteLine($"✓ Verified document details - Type: '{actualDocumentType}', Reference: '{actualDocumentReference}', Date: '{actualDisplayedDate}'");
        }

        private string ConvertDateFormat(string dateString)
        {
            try
            {
                // Input format: "dd MM yyyy" (e.g., "05 12 2025")
                // Output format: "d MMMM yyyy" (e.g., "5 December 2025")
                var dateParts = dateString.Split(' ');
                if (dateParts.Length == 3)
                {
                    var day = int.Parse(dateParts[0]);
                    var month = int.Parse(dateParts[1]);
                    var year = int.Parse(dateParts[2]);

                    var date = new DateTime(year, month, day);
                    return date.ToString("d MMMM yyyy");
                }

                return dateString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Date conversion failed: {ex.Message}. Returning original: {dateString}");
                return dateString;
            }
        }

        [When("the user clicks on Download all documents link")]
        public void WhenTheUserClicksOnDownloadAllDocumentsLink()
        {
            documentsPage?.ClickDownloadAllDocumentsLink();
        }

        [When("the user clicks on Download url")]
        public void WhenTheUserClicksOnDownloadUrl()
        {
            documentsPage?.ClickDownloadUrlLink();
        }

        [Then("Catch certificate summary url should be displayed under heading Importer")]
        public void ThenCatchCertificateSummaryUrlIsDisplayedUnderHeadingImporter()
        {
            Assert.True(documentsPage?.IsCatchCertificateSummaryUrlDisplayed(), "Catch certificate summary url didn't displayed");
        }

        [When("the user clicks on Download link for the catch certificate")]
        public void WhenTheUserClicksOnDownloadLinkForTheCatchCert()
        {
            documentsPage?.ClickDownloadLinkInCatchCertificate();
        }
        
        [Then("the user verifies the {string} catch certificate is downloaded")]
        public void WhenTheUserClicksOnDownloadLinkForTheCatchCert(string catchCertificate)
        {
            Assert.True(documentsPage?.IsCatchCertificateDownloaded(catchCertificate), "The catch certificate is not downloaded");
        }
    }
}