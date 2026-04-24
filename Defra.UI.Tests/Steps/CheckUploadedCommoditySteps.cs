using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CheckUploadedCommoditySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICheckUploadedCommodityPage? checkUploadedCommodityPage => _objectContainer.IsRegistered<ICheckUploadedCommodityPage>() ? _objectContainer.Resolve<ICheckUploadedCommodityPage>() : null;

        public CheckUploadedCommoditySteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("Check uploaded commodity details page should be displayed")]
        public void ThenCheckUploadedCommodityDetailsPageShouldBeDisplayed()
        {
            Assert.True(checkUploadedCommodityPage?.IsPageLoaded(), "Check uploaded commodity details page not loaded");
        }


        [Then("CSV file should be uploaded successfully with {string} message and validate the count of commodity")]
        public void ThenCSVFileShouldBeUploadedSuccessfullyAndValidateTheCountOfCommodity(string successMsg)
        {
            var expectedCommodityCount = _scenarioContext.Get<int>("NumberOfCommodities");

            Assert.True(checkUploadedCommodityPage?.WaitForUploadToCompleteAndVerifySuccessMessage(successMsg), "CSV upload failed");
            Assert.True(checkUploadedCommodityPage?.IsCountOfCommodityMatchesWithInput(expectedCommodityCount), "Commodity count not matches with the number of commodities given in the input");
        }

        [Then("all the displayed commodity data in all tables should be validated with the values given in the input")]
        public void ThenAllTheDisplayedCommodityDataInAllTablesShouldBeValidatedWithTheValuesGivenInTheInput()
        {
            var inputAllCommodityData = _scenarioContext["AllCommodityDetails"] as Table;

            bool allDataMatches = true;
            var mismatches = new List<string>();

            checkUploadedCommodityPage?.ValidateAllCommodityDetails(inputAllCommodityData, ref allDataMatches, mismatches);

            Assert.True(allDataMatches, "Validation failed: " + string.Join(", ", mismatches));
        }


        [Then("checks the information message heading {string} and content {string}")]
        public void ThenChecksTheInformationMessageHeadingAndContent(string msgHeading, string msgContent)
        {
            Assert.True(checkUploadedCommodityPage?.VerifyInfoMessage(msgHeading, msgContent));
        }

        [When("the user clicks Confirm and continue button")]
        public void WhenTheUserClicksConfirmAndContinueButton()
        {
            checkUploadedCommodityPage?.ClickConfirmAndContinueButton();
        }

        [Then("there is no banner stating Upload successful")]
        public void ThenThereIsNoBannerStatingUploadSuccessful()
        {
            Assert.True(checkUploadedCommodityPage?.IsUploadSuccessBannerAbsent(), "Upload successful banner should not be displayed but was found");
        }
    }
}