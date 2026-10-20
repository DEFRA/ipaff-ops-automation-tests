using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterHowMuchStockAreYouExportingSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterHowMuchStockAreYouExportingPage? exporterHowMuchStockAreYouExportingPage =>
            _objectContainer.IsRegistered<IExporterHowMuchStockAreYouExportingPage>()
                ? _objectContainer.Resolve<IExporterHowMuchStockAreYouExportingPage>()
                : null;

        public ExporterHowMuchStockAreYouExportingSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the How much stock are you exporting? page is displayed")]
        public void ThenTheHowMuchStockAreYouExportingPageIsDisplayed()
        {
            Assert.True(exporterHowMuchStockAreYouExportingPage?.IsPageLoaded(), "How much stock are you exporting? page is not displayed");
        }

        [Then("the Quantity, Inspected on date and Valid until date are displayed on the How much stock are you exporting? page")]
        public void ThenTheQuantityInspectedOnDateAndValidUntilDateAreDisplayedOnTheHowMuchStockAreYouExportingPage()
        {
            var summary = exporterHowMuchStockAreYouExportingPage?.GetStockSummaryText() ?? string.Empty;

            // e.g. "100 Units of stock was inspected on 17 Oct 2026. You have until 16 Nov 2026 to draw down this stock."
            var match = Regex.Match(summary,
                @"^(?<quantity>\d+(\.\d+)?)\s+(?<unit>\S+).*inspected on (?<inspected>\d{1,2} [A-Za-z]{3} \d{4})\..*until (?<valid>\d{1,2} [A-Za-z]{3} \d{4}) to draw down");

            Assert.True(match.Success, $"Quantity, Inspected on date and Valid until date not displayed as expected. Found '{summary}'");

            var expectedQuantity = $"{_scenarioContext.Get<object>("QuantityInspected")} {_scenarioContext.Get<object>("UnitOfMeasurement")}".Trim();
            Assert.True(summary.StartsWith(expectedQuantity, StringComparison.OrdinalIgnoreCase),
                $"Quantity displayed does not match. Expected to start with '{expectedQuantity}', Found '{summary}'");

            var expectedInspectedOn = DateTime.ParseExact(_scenarioContext.Get<string>("VisitDateScheduled"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var inspectedParsed = DateTime.TryParseExact(match.Groups["inspected"].Value, new[] { "d MMM yyyy", "dd MMM yyyy" },
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var actualInspectedOn);
            Assert.True(inspectedParsed && actualInspectedOn.Date == expectedInspectedOn.Date,
                $"Inspected on date does not match the Visit Date Scheduled. Expected '{expectedInspectedOn:d MMM yyyy}', Found '{match.Groups["inspected"].Value}'");
            Assert.AreEqual(_scenarioContext.Get<string>("StockValidUntil"), match.Groups["valid"].Value,
                "Valid until date does not match the Stock details page");
        }

        [When("the user enters the draw down quantity {int} on the How much stock are you exporting? page")]
        public void WhenTheUserEntersTheDrawDownQuantityOnTheHowMuchStockAreYouExportingPage(int quantity)
        {
            _scenarioContext["DrawDownQuantity"] = quantity;
            exporterHowMuchStockAreYouExportingPage?.EnterQuantity(quantity.ToString());
        }

        [When("the user clicks the Save and continue button on the How much stock are you exporting? page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheHowMuchStockAreYouExportingPage()
        {
            exporterHowMuchStockAreYouExportingPage?.ClickSaveAndContinueButton();
        }
    }
}
