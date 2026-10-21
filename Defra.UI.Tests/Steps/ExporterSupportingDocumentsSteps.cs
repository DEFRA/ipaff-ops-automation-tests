using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterSupportingDocumentsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterSupportingDocumentsPage? exporterSupportingDocumentsPage =>
            _objectContainer.IsRegistered<IExporterSupportingDocumentsPage>()
                ? _objectContainer.Resolve<IExporterSupportingDocumentsPage>()
                : null;

        public ExporterSupportingDocumentsSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Supporting documents page is displayed")]
        public void ThenTheSupportingDocumentsPageIsDisplayed()
        {
            Assert.True(exporterSupportingDocumentsPage?.IsPageLoaded(), "Supporting documents page is not displayed");
        }

        [When("the user uploads a supporting document and clicks the Save and continue button")]
        public void WhenTheUserUploadsASupportingDocumentAndClicksTheSaveAndContinueButton()
        {
            const string fileName = "IPAFFS Test Document 1.docx";
            const string fileDescription = "Automation supporting document";

            exporterSupportingDocumentsPage?.UploadDocument(fileName, fileDescription);
            _scenarioContext["SupportingDocumentFileName"] = fileName;
            _scenarioContext["SupportingDocumentDescription"] = fileDescription;
            exporterSupportingDocumentsPage?.ClickSaveAndContinueButton();
        }
    }
}
