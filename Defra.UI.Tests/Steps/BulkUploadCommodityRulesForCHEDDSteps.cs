using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class BulkUploadCommodityRulesForCHEDDSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IBulkUploadCommodityRulesForCHEDDPage? bulkUploadCommodityRulesForCHEDDPage =>
            _objectContainer.IsRegistered<IBulkUploadCommodityRulesForCHEDDPage>()
                ? _objectContainer.Resolve<IBulkUploadCommodityRulesForCHEDDPage>()
                : null;

        public BulkUploadCommodityRulesForCHEDDSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Bulk upload commodity rules for CHED-D page should be displayed")]
        public void ThenTheBulkUploadCommodityRulesForCHEDDPageShouldBeDisplayed()
        {
            Assert.True(bulkUploadCommodityRulesForCHEDDPage.IsPageLoaded(), "Bulk upload commodity rules for CHED-D page is not displayed");
        }

        [When("the user clicks the Choose file button on the CHED-D bulk upload page")]
        public void WhenTheUserClicksTheChooseFileButtonOnTheCHEDDBulkUploadPage()
        {
            bulkUploadCommodityRulesForCHEDDPage.ClickChooseFileButton();
        }

        [When("the user navigates to and selects the CHED-D bulk upload file {string}")]
        public void WhenTheUserNavigatesToAndSelectsTheCHEDDBulkUploadFile(string fileName)
        {
            bulkUploadCommodityRulesForCHEDDPage.SelectBulkUploadFile(fileName);
            _scenarioContext["BulkUploadFileName"] = fileName;
        }

        [Then("the selected file name is displayed within the CHED-D File added box")]
        public void ThenTheSelectedFileNameIsDisplayedWithinTheCHEDDFileAddedBox()
        {
            var fileName = (string)_scenarioContext["BulkUploadFileName"];
            Assert.True(bulkUploadCommodityRulesForCHEDDPage.IsSelectedFileNameDisplayed(fileName),
                $"Selected file name '{fileName}' was not displayed within the File added box");
        }

        [When("the user clicks the Continue button on the CHED-D bulk upload page")]
        public void WhenTheUserClicksTheContinueButtonOnTheCHEDDBulkUploadPage()
        {
            bulkUploadCommodityRulesForCHEDDPage.ClickContinueButton();
        }
    }
}