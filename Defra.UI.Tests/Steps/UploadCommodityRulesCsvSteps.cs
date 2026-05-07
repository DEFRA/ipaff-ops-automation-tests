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
    }
}