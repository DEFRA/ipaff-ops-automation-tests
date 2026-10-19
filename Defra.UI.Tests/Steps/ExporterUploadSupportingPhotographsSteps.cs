using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterUploadSupportingPhotographsSteps
    {
        private const string PhotographFileName = "IPAFFS Supporting Photograph.jpg";
        private const string PhotographDescription = "Automation supporting photograph";

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterUploadSupportingPhotographsPage? exporterUploadSupportingPhotographsPage =>
            _objectContainer.IsRegistered<IExporterUploadSupportingPhotographsPage>()
                ? _objectContainer.Resolve<IExporterUploadSupportingPhotographsPage>()
                : null;

        public ExporterUploadSupportingPhotographsSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Upload supporting photographs page is displayed")]
        public void ThenTheUploadSupportingPhotographsPageIsDisplayed()
        {
            Assert.True(exporterUploadSupportingPhotographsPage?.IsPageLoaded(), "Upload supporting photographs page is not displayed");
        }

        [When("the user uploads a JPEG supporting photograph with a file description")]
        public void WhenTheUserUploadsAJPEGSupportingPhotographWithAFileDescription()
        {
            var uploadedFileName = exporterUploadSupportingPhotographsPage?.UploadPhotograph(PhotographFileName, PhotographDescription);
            _scenarioContext["SupportingPhotographFileName"] = uploadedFileName;
            _scenarioContext["SupportingPhotographDescription"] = PhotographDescription;
        }

        [Then("the photograph is added to the supporting photographs table with the Download and Delete actions")]
        public void ThenThePhotographIsAddedToTheSupportingPhotographsTableWithTheDownloadAndDeleteActions()
        {
            var fileName = _scenarioContext.Get<string>("SupportingPhotographFileName");
            var description = _scenarioContext.Get<string>("SupportingPhotographDescription");
            Assert.True(exporterUploadSupportingPhotographsPage?.IsPhotographListedWithActions(fileName, description),
                $"Photograph '{fileName}' is not listed in the supporting photographs table with Download and Delete actions");
        }

        [When("the user clicks the Save and continue button on the Upload supporting photographs page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheUploadSupportingPhotographsPage()
        {
            exporterUploadSupportingPhotographsPage?.ClickSaveAndContinueButton();
        }
    }
}
