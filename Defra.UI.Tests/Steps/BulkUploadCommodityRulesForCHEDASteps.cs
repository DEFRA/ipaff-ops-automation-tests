using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class BulkUploadCommodityRulesForCHEDASteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IBulkUploadCommodityRulesForCHEDAPage? page =>
            _objectContainer.IsRegistered<IBulkUploadCommodityRulesForCHEDAPage>()
                ? _objectContainer.Resolve<IBulkUploadCommodityRulesForCHEDAPage>()
                : null;

        public BulkUploadCommodityRulesForCHEDASteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Bulk upload commodity rules for CHED-A page should be displayed")]
        public void ThenTheBulkUploadCommodityRulesForCHEDAPageShouldBeDisplayed()
        {
            Assert.True(page?.IsPageLoaded(), "Bulk upload commodity rules for CHED-A page is not displayed");
        }

        [When("the user clicks the Choose file button on the CHED-A bulk upload page")]
        public void WhenTheUserClicksTheChooseFileButtonOnTheCHEDABulkUploadPage()
        {
            page?.ClickChooseFileButton();
        }

        [When("the user navigates to and selects the CHED-A bulk upload file {string}")]
        public void WhenTheUserNavigatesToAndSelectsTheCHEDABulkUploadFile(string fileName)
        {
            page?.SelectBulkUploadFile(fileName);
            _scenarioContext["BulkUploadFileName"] = fileName;
        }

        [Then("the selected file name is displayed within the File added box")]
        public void ThenTheSelectedFileNameIsDisplayedWithinTheFileAddedBox()
        {
            var fileName = (string)_scenarioContext["BulkUploadFileName"];
            Assert.True(page?.IsSelectedFileNameDisplayed(fileName),
                $"Selected file name '{fileName}' was not displayed within the File added box");
        }

        [When("the user clicks the Continue button on the CHED-A bulk upload page")]
        public void WhenTheUserClicksTheContinueButtonOnTheCHEDABulkUploadPage()
        {
            page?.ClickContinueButton();
        }
    }
}