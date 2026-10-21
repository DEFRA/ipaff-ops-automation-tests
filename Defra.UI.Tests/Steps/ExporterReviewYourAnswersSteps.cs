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
            var allDataMatches = true;
            var mismatches = new List<string>();

            // Application summary
            ValidateIfExists("ExporterDestinationCountry", exporterReviewYourAnswersPage?.GetDestinationCountry(), ref allDataMatches, mismatches);
            ValidateIfExists("Reference", exporterReviewYourAnswersPage?.GetYourReference(), ref allDataMatches, mismatches);

            // Exporter details
            ValidateIfExists("ExporterName", exporterReviewYourAnswersPage?.GetExporterName(), ref allDataMatches, mismatches);
            ValidateExporterAddress(exporterReviewYourAnswersPage?.GetExporterAddress(), ref allDataMatches, mismatches);

            // Your consignment
            ValidateIfExists("Commodity", exporterReviewYourAnswersPage?.GetCommodityDescription(), ref allDataMatches, mismatches);
            if (_scenarioContext.ContainsKey("PlantCommodityType"))
            {
                ValidateIfExists("PlantCommodityType", exporterReviewYourAnswersPage?.GetCommodityType(), ref allDataMatches, mismatches);
                ValidateIfExists("PlantCountriesOfOrigin", exporterReviewYourAnswersPage?.GetCommodityOrigin(), ref allDataMatches, mismatches);
                var quantityKey = _scenarioContext.ContainsKey("DrawDownQuantity") ? "DrawDownQuantity" : "PlantQuantity";
                ValidateQuantity(quantityKey, "PlantUnit", exporterReviewYourAnswersPage?.GetCommodityQuantity(), ref allDataMatches, mismatches);
                ValidateNumberOfPackages("PlantNumberOfPackages", exporterReviewYourAnswersPage?.GetCommodityNumberOfPackages(), ref allDataMatches, mismatches);
                ValidateIfExists("PlantPackaging", exporterReviewYourAnswersPage?.GetCommodityPackaging(), ref allDataMatches, mismatches);
            }
            else
            {
                ValidateIfExists("SeedVariety", exporterReviewYourAnswersPage?.GetCommodityVariety(), ref allDataMatches, mismatches);
                ValidateIfExists("SeedLotReference", exporterReviewYourAnswersPage?.GetCommodityLotReference(), ref allDataMatches, mismatches);
                ValidateIfExists("SeedCountriesOfOrigin", exporterReviewYourAnswersPage?.GetCommodityOrigin(), ref allDataMatches, mismatches);
                ValidateQuantity("SeedQuantity", "SeedUnit", exporterReviewYourAnswersPage?.GetCommodityQuantity(), ref allDataMatches, mismatches);
                ValidateNumberOfPackages("SeedNumberOfPackages", exporterReviewYourAnswersPage?.GetCommodityNumberOfPackages(), ref allDataMatches, mismatches);
                ValidateIfExists("SeedPackaging", exporterReviewYourAnswersPage?.GetCommodityPackaging(), ref allDataMatches, mismatches);
            }

            // Transportation
            ValidateIfExists("ExporterConsignmentValue", exporterReviewYourAnswersPage?.GetConsignmentValue(), ref allDataMatches, mismatches);
            ValidateContains("ExporterPointOfEntry", exporterReviewYourAnswersPage?.GetPointOfEntry(), ref allDataMatches, mismatches);
            ValidateIfExists("ExporterTransportMode", exporterReviewYourAnswersPage?.GetTransportMode(), ref allDataMatches, mismatches);

            // Consignee details
            ValidateIfExists("ExporterConsigneeName", exporterReviewYourAnswersPage?.GetConsigneeName(), ref allDataMatches, mismatches);
            ValidateIfExists("ExporterConsigneeAddress", exporterReviewYourAnswersPage?.GetConsigneeAddress(), ref allDataMatches, mismatches);

            // Supporting documents
            if (_scenarioContext.ContainsKey("SupportingDocumentFileName"))
            {
                ValidateIfExists("SupportingDocumentFileName", exporterReviewYourAnswersPage?.GetSupportingDocumentFileName(), ref allDataMatches, mismatches);
                ValidateIfExists("SupportingDocumentDescription", exporterReviewYourAnswersPage?.GetSupportingDocumentDescription(), ref allDataMatches, mismatches);
            }

            if (!allDataMatches)
            {
                Console.WriteLine("[REVIEW VALIDATION] Data mismatches found:");
                foreach (var mismatch in mismatches)
                {
                    Console.WriteLine($"[REVIEW VALIDATION] {mismatch}");
                }
            }

            Assert.True(allDataMatches, $"Review page data validation failed. Mismatches: {string.Join(", ", mismatches)}");
        }

        private void ValidateIfExists(string contextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (!TryGetExpected(contextKey, out var expectedValue))
                return;

            CompareValues(contextKey, expectedValue, reviewValue?.Trim(), expectedValue.Equals(reviewValue?.Trim(), StringComparison.OrdinalIgnoreCase), ref allDataMatches, mismatches);
        }

        /// <summary>
        /// Point of entry is displayed with its full name, e.g. "NL AMS" displays as "Amsterdam (NL AMS)"
        /// </summary>
        private void ValidateContains(string contextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (!TryGetExpected(contextKey, out var expectedValue))
                return;

            var actualValue = reviewValue?.Trim() ?? string.Empty;
            CompareValues(contextKey, expectedValue, actualValue, actualValue.Contains(expectedValue, StringComparison.OrdinalIgnoreCase), ref allDataMatches, mismatches);
        }

        /// <summary>
        /// The review page shows the exporter address prefixed with the organisation name and without the country,
        /// e.g. 'DEPARTMENT FOR ... (D E F R A), HORNBEAM HOUSE, ELECTRA WAY, CREWE, CHESHIRE, CW1 6GJ'.
        /// </summary>
        private void ValidateExporterAddress(string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (!TryGetExpected("ExporterAddress", out var expectedValue))
                return;

            var lastComma = expectedValue.LastIndexOf(',');
            var expectedWithoutCountry = lastComma >= 0 ? expectedValue[..lastComma] : expectedValue;
            var isMatch = NormaliseAddress(reviewValue).Contains(NormaliseAddress(expectedWithoutCountry));
            CompareValues("ExporterAddress", expectedValue, reviewValue?.Trim(), isMatch, ref allDataMatches, mismatches);
        }

        private static string NormaliseAddress(string? value) =>
            System.Text.RegularExpressions.Regex.Replace(value ?? string.Empty, @"[\s,]+", " ").Trim().ToUpperInvariant();

        private void ValidateQuantity(string quantityKey, string unitKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (!TryGetExpected(quantityKey, out var quantity) || !TryGetExpected(unitKey, out var unit))
                return;

            var expectedValue = $"{quantity} {unit}";
            CompareValues(quantityKey, expectedValue, reviewValue?.Trim(), expectedValue.Equals(reviewValue?.Trim(), StringComparison.OrdinalIgnoreCase), ref allDataMatches, mismatches);
        }

        private void ValidateNumberOfPackages(string packagesKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (!TryGetExpected(packagesKey, out var packages))
                return;

            var expectedValue = packages == "1" ? "1 package" : $"{packages} packages";
            CompareValues(packagesKey, expectedValue, reviewValue?.Trim(), expectedValue.Equals(reviewValue?.Trim(), StringComparison.OrdinalIgnoreCase), ref allDataMatches, mismatches);
        }

        private bool TryGetExpected(string contextKey, out string expectedValue)
        {
            expectedValue = string.Empty;
            if (!_scenarioContext.ContainsKey(contextKey))
            {
                Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return false;
            }

            expectedValue = _scenarioContext[contextKey]?.ToString()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(expectedValue))
            {
                Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return false;
            }
            return true;
        }

        private static void CompareValues(string contextKey, string expectedValue, string? actualValue, bool isMatch, ref bool allDataMatches, List<string> mismatches)
        {
            if (!isMatch)
            {
                allDataMatches = false;
                mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{actualValue}'");
            }
            else
            {
                Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
            }
        }

        [When("the user clicks the Change link next to the {string} field on the Review your answers page")]
        public void WhenTheUserClicksTheChangeLinkNextToTheFieldOnTheReviewYourAnswersPage(string fieldName)
        {
            exporterReviewYourAnswersPage?.ClickChangeLink(fieldName);
        }

        [When("the user clicks the Continue button on the Review your answers page")]
        public void WhenTheUserClicksTheContinueButtonOnTheReviewYourAnswersPage()
        {
            exporterReviewYourAnswersPage?.ClickContinueButton();
        }
    }
}