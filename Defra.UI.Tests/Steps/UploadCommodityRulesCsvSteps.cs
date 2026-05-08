using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class UploadCommodityRulesCsvSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IUploadCommodityRulesCsvPage? uploadCommodityRulesCsvPage =>
            _objectContainer.IsRegistered<IUploadCommodityRulesCsvPage>()
                ? _objectContainer.Resolve<IUploadCommodityRulesCsvPage>()
                : null;

        public UploadCommodityRulesCsvSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Upload multiple commodity rules using a CSV file page should be displayed")]
        public void ThenUploadCsvPageShouldBeDisplayed()
        {
            Assert.True(uploadCommodityRulesCsvPage?.IsPageLoaded(), "Upload multiple commodity rules using a CSV file page is not displayed");
        }

        [When("the user clicks the Choose file button")]
        public void WhenTheUserClicksTheChooseFileButton()
        {
            uploadCommodityRulesCsvPage?.ClickChooseFileButton();
        }

        [When("the user navigates to and selects the bulk upload file {string}")]
        public void WhenTheUserSelectsTheBulkUploadFile(string fileName)
        {
            uploadCommodityRulesCsvPage?.SelectBulkUploadFile(fileName);
            _scenarioContext["BulkUploadFileName"] = fileName;
        }

        [Then("the selected file name is displayed next to the Choose file button")]
        public void ThenTheSelectedFileNameIsDisplayed()
        {
            var fileName = (string)_scenarioContext["BulkUploadFileName"];
            Assert.True(uploadCommodityRulesCsvPage?.IsSelectedFileNameDisplayed(fileName),
                $"Selected file name '{fileName}' was not displayed next to the Choose file button");
        }

        [When("the user clicks the Upload button on the bulk upload page")]
        public void WhenTheUserClicksTheUploadButton()
        {
            uploadCommodityRulesCsvPage?.ClickUploadButton();
        }

        [When("the user updates the bulk update CSV {string} setting the Id for commodity code {string} to the recorded {string}")]
        public void WhenTheUserUpdatesTheBulkUpdateCsvSettingTheIdForCommodityCodeToTheRecorded(
            string fileName, string commodityCode, string contextKey)
        {
            var newId = _scenarioContext.Get<string>(contextKey);
            uploadCommodityRulesCsvPage?.UpdateCsvIdForCommodityCode(fileName, commodityCode, newId);
            _scenarioContext["BulkUpdateFileName"] = fileName;
        }

        [Then("the bulk update CSV row for commodity code {string} should contain the recorded {string}")]
        public void ThenTheBulkUpdateCsvRowForCommodityCodeShouldContainTheRecorded(
            string commodityCode, string contextKey)
        {
            var expectedId = _scenarioContext.Get<string>(contextKey);
            var fileName = (string)_scenarioContext["BulkUpdateFileName"];
            var actualId = uploadCommodityRulesCsvPage!.GetCsvIdForCommodityCode(fileName, commodityCode);
            Assert.AreEqual(expectedId, actualId,
                $"Expected Id '{expectedId}' for commodity code '{commodityCode}' but found '{actualId}' in the bulk update CSV");
        }
    }
}