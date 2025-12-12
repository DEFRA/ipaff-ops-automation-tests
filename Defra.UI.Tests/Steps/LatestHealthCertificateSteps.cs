using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
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
            _scenarioContext.Add("HealthCertificateReference", reference);
        }

        [When("the user enters Latest Health Certificate date of issue {string}{string}{string}")]    
        public void WhenTheUserEntersLatestHealthCertificateDateOfIssue(string day, string month, string year)
        {
            latestHealthCertificatePage?.EnterDateOfIssue(day, month, year);
            var dateofIssue = day + " " + month + " " + year;
            _scenarioContext.Add("HealthCertificateDateOfIssue", dateofIssue);
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
            _scenarioContext.Add("LatestHealthCertificateDocumentName", filename);
        }

        [Then("the Latest Health Certificate document {string} {string} is uploaded successfully")]
        public void ThenTheLatestHealthCertificateDocumentIsUploadedSuccessfully(string name, string format)
        {
            var filename = name + format;
            Assert.True(latestHealthCertificatePage?.GetFileName.Contains(filename), "The accompanying document upload has failed");

            //The date selected from date picker gets populated only after the document is uploaded. Hence, we are adding date to scenario context after attaching the document.
            var dateOfIssue = latestHealthCertificatePage?.GetDocumentIssueDate();
            _scenarioContext.Add("LatestHealthCertificateDocumentDateOfIssue", dateOfIssue);
        }
    }
}