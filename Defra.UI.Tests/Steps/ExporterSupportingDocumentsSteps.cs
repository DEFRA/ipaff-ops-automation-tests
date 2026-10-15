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

        private IExporterSupportingDocumentsPage? exporterSupportingDocumentsPage =>
            _objectContainer.IsRegistered<IExporterSupportingDocumentsPage>()
                ? _objectContainer.Resolve<IExporterSupportingDocumentsPage>()
                : null;

        public ExporterSupportingDocumentsSteps(IObjectContainer container)
        {
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
            exporterSupportingDocumentsPage?.UploadDocument("IPAFFS Test Document 1.docx", "Automation supporting document");
            exporterSupportingDocumentsPage?.ClickSaveAndContinueButton();
        }
    }
}
