using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.Globalization;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterStockDetailsSteps
    {
        private static readonly string[] DisplayDateFormats = { "d MMM yyyy", "dd MMM yyyy" };

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterStockDetailsPage? exporterStockDetailsPage =>
            _objectContainer.IsRegistered<IExporterStockDetailsPage>()
                ? _objectContainer.Resolve<IExporterStockDetailsPage>()
                : null;

        public ExporterStockDetailsSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Stock details page is displayed")]
        public void ThenTheStockDetailsPageIsDisplayed()
        {
            Assert.True(exporterStockDetailsPage?.IsPageLoaded(), "Stock details page is not displayed");
            _scenarioContext["StockReference"] = exporterStockDetailsPage?.GetStockReference();
        }

        [Then("the stock details show the commodity description, variety, location, inspected on, valid until, quantity inspected and amount drawn down of {int}")]
        public void ThenTheStockDetailsShowTheExpectedDetails(int amountDrawnDown)
        {
            var unit = GetContextValue("UnitOfMeasurement");
            var mismatches = new List<string>();

            Compare("Description", GetContextValue("Commodity"), mismatches);
            Compare("Variety", GetContextValue("Variety"), mismatches);
            Compare("Location", GetContextValue("Location"), mismatches);
            Compare("Quantity inspected", $"{GetContextValue("QuantityInspected")} {unit}".Trim(), mismatches);
            Compare("Amount drawn down", $"{amountDrawnDown} {unit}".Trim(), mismatches);

            var inspectedOn = exporterStockDetailsPage?.GetSummaryValue("Inspected on")?.Trim() ?? string.Empty;
            _scenarioContext["StockInspectedOn"] = inspectedOn;
            if (!TryParseDisplayDate(inspectedOn, out _))
                mismatches.Add($"Inspected on: '{inspectedOn}' is not in the format DD MMM YYYY");

            var validUntil = exporterStockDetailsPage?.GetSummaryValue("Valid until")?.Trim() ?? string.Empty;
            _scenarioContext["StockValidUntil"] = validUntil;
            if (!TryParseDisplayDate(validUntil, out _))
                mismatches.Add($"Valid until: '{validUntil}' is not in the format DD MMM YYYY");

            Assert.IsEmpty(mismatches, $"Stock details validation failed. Mismatches: {string.Join(", ", mismatches)}");
        }

        [When("the user clicks the Draw down button")]
        public void WhenTheUserClicksTheDrawDownButton()
        {
            exporterStockDetailsPage?.ClickDrawDownButton();
        }

        private void Compare(string key, string expected, List<string> mismatches)
        {
            var actual = exporterStockDetailsPage?.GetSummaryValue(key) ?? string.Empty;
            if (!expected.Equals(actual, StringComparison.OrdinalIgnoreCase))
                mismatches.Add($"{key}: Expected '{expected}', Found '{actual}'");
        }

        private string GetContextValue(string key) =>
            _scenarioContext.TryGetValue(key, out object? value) ? value?.ToString()?.Trim() ?? string.Empty : string.Empty;

        private static bool TryParseDisplayDate(string value, out DateTime date) =>
            DateTime.TryParseExact(value.Trim(), DisplayDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }
}
