using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterYourCommoditiesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterYourCommoditiesPage? exporterYourCommoditiesPage =>
            _objectContainer.IsRegistered<IExporterYourCommoditiesPage>()
                ? _objectContainer.Resolve<IExporterYourCommoditiesPage>()
                : null;

        public ExporterYourCommoditiesSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Your commodities page is displayed with the commodity line just entered")]
        public void ThenTheYourCommoditiesPageIsDisplayedWithTheCommodityLineJustEntered()
        {
            Assert.True(exporterYourCommoditiesPage?.IsPageLoaded(), "Your commodities page is not displayed");
            var commodity = _scenarioContext.Get<string>("Commodity");
            Assert.True(exporterYourCommoditiesPage?.IsCommodityLineDisplayed(commodity), "Commodity line is not displayed");
        }

        [Then("the Your commodities page is displayed")]
        public void ThenTheYourCommoditiesPageIsDisplayed()
        {
            Assert.True(exporterYourCommoditiesPage?.IsPageLoaded(), "Your commodities page is not displayed");
        }

        [Then("the commodity table is displayed for arranging for seed sampling to export seeds for planting")]
        public void ThenTheCommodityTableIsDisplayedForArrangingForSeedSampling()
        {
            AssertCommodityTable(new[] { "Description", "Lot reference", "Destination", "Quantity", "Actions" });
        }

        [Then("the commodity table is displayed for applying for a phytosanitary certificate to export seeds for planting")]
        public void ThenTheCommodityTableIsDisplayedForApplyingForAPhytosanitaryCertificate()
        {
            AssertCommodityTable(new[] { "Description", "Quantity and number of packages", "Packaging, packing material and distinguishing marks", "Actions" });
        }

        [Then("the {string} question is displayed on the Your commodities page")]
        public void ThenTheQuestionIsDisplayedOnTheYourCommoditiesPage(string question)
        {
            Assert.True(exporterYourCommoditiesPage?.IsAddMoreQuestionDisplayed(question), $"'{question}' question is not displayed");
        }

        [Then("there are radio buttons present on the Your commodities page")]
        public void ThenThereAreRadioButtonsPresentOnTheYourCommoditiesPage()
        {
            Assert.True(exporterYourCommoditiesPage?.AreAddMoreRadioButtonsPresent(), "Radio buttons are not present on the Your commodities page");
        }

        [Then("there is no option to Save and continue later on the Your commodities page")]
        public void ThenThereIsNoOptionToSaveAndContinueLaterOnTheYourCommoditiesPage()
        {
            Assert.False(exporterYourCommoditiesPage?.IsSaveAndContinueLaterDisplayed(), "Save and continue later option is displayed on the Your commodities page");
        }

        private void AssertCommodityTable(string[] expectedHeaders)
        {
            var headers = exporterYourCommoditiesPage?.GetCommodityTableHeaders() ?? new List<string>();
            foreach (var expected in expectedHeaders)
            {
                Assert.True(headers.Contains(expected), $"Commodity table header '{expected}' not found. Actual: '{string.Join(", ", headers)}'");
            }
            var commodity = _scenarioContext.Get<string>("Commodity");
            Assert.True(exporterYourCommoditiesPage?.IsCommodityLineDisplayed(commodity), $"Commodity line '{commodity}' is not displayed");
        }

        [When("the user selects {string} and clicks the Save and continue button on the Your commodities page")]
        public void WhenTheUserSelectsAndClicksTheSaveAndContinueButtonOnTheYourCommoditiesPage(string option)
        {
            exporterYourCommoditiesPage?.SelectFinishedAddingCommoditiesOption(option);
            exporterYourCommoditiesPage?.ClickSaveAndContinueButton();
        }
    }
}