using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterReviewYourAnswersSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterReviewYourAnswersPage? exporterReviewYourAnswersPage =>
            _objectContainer.IsRegistered<IExporterReviewYourAnswersPage>()
                ? _objectContainer.Resolve<IExporterReviewYourAnswersPage>()
                : null;

        public ExporterReviewYourAnswersSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Review your answers page is displayed")]
        public void ThenTheReviewYourAnswersPageIsDisplayed()
        {
            Assert.True(exporterReviewYourAnswersPage?.IsPageLoaded(), "Review your answers page is not displayed");
        }

        [Then("the data presented for review matches the data entered into the Exporter Portal for a phytosanitary certificate application")]
        public void ThenTheDataPresentedForReviewMatchesTheDataEnteredForAPhytosanitaryCertificateApplication()
        {
            var commodityText = exporterReviewYourAnswersPage?.GetCommodityTableText() ?? string.Empty;
            var expectedCommodityValues = new[]
            {
                _scenarioContext.Get<string>("Commodity"),
                _scenarioContext.Get<string>("SeedVariety"),
                $"Lot reference: {_scenarioContext.Get<string>("SeedLotReference")}",
                _scenarioContext.Get<string>("SeedCountriesOfOrigin"),
                $"{_scenarioContext.Get<string>("SeedQuantity")} {_scenarioContext.Get<string>("SeedUnit")}",
                _scenarioContext.Get<string>("SeedPackaging")
            };
            foreach (var expected in expectedCommodityValues)
            {
                Assert.True(commodityText.Contains(expected, StringComparison.OrdinalIgnoreCase), $"Review commodity table does not contain '{expected}'. Actual: '{commodityText}'");
            }

            Assert.AreEqual("1200.00", exporterReviewYourAnswersPage?.GetConsignmentValue(), "Consignment value does not match");
            StringAssert.Contains("NL AMS", exporterReviewYourAnswersPage?.GetPointOfEntry(), "Point of entry does not match");
            Assert.AreEqual("Road or ferry", exporterReviewYourAnswersPage?.GetTransportMode(), "Transport mode does not match");
        }

        [When("the user clicks the Continue button on the Review your answers page")]
        public void WhenTheUserClicksTheContinueButtonOnTheReviewYourAnswersPage()
        {
            exporterReviewYourAnswersPage?.ClickContinueButton();
        }
    }
}