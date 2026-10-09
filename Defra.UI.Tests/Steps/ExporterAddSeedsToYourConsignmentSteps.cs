using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterAddSeedsToYourConsignmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterAddSeedsToYourConsignmentPage? exporterAddSeedsToYourConsignmentPage =>
            _objectContainer.IsRegistered<IExporterAddSeedsToYourConsignmentPage>()
                ? _objectContainer.Resolve<IExporterAddSeedsToYourConsignmentPage>()
                : null;

        public ExporterAddSeedsToYourConsignmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Add seeds to your consignment page is displayed")]
        public void ThenTheAddSeedsToYourConsignmentPageIsDisplayed()
        {
            Assert.True(exporterAddSeedsToYourConsignmentPage?.IsPageLoaded(), "Add seeds to your consignment page is not displayed");
        }

        [When("the user describes the seed commodity with botanical name {string}, variety {string}, lot reference {string}, quantity {string}, unit of measurement {string}, chemically treated {string}, chemicals used {string}, country where grown {string}, destination countries {string}")]
        public void WhenTheUserDescribesTheSeedCommodityForSeedSampling(string botanicalName, string variety, string lotReference, string quantity,
            string unit, string chemicallyTreated, string chemicalsUsed, string countryWhereGrown, string destinationCountries)
        {
            exporterAddSeedsToYourConsignmentPage?.EnterBotanicalName(botanicalName);
            _scenarioContext["Commodity"] = exporterAddSeedsToYourConsignmentPage?.GetSelectedCommodityDescription();
            exporterAddSeedsToYourConsignmentPage?.EnterVariety(variety);
            exporterAddSeedsToYourConsignmentPage?.EnterLotReference(lotReference);
            exporterAddSeedsToYourConsignmentPage?.EnterQuantity(quantity);
            exporterAddSeedsToYourConsignmentPage?.SelectUnitOfMeasurement(unit);
            exporterAddSeedsToYourConsignmentPage?.SelectChemicallyTreated(chemicallyTreated);
            exporterAddSeedsToYourConsignmentPage?.EnterChemicalsUsed(chemicalsUsed);
            exporterAddSeedsToYourConsignmentPage?.SelectCountryWhereGrown(countryWhereGrown);
            exporterAddSeedsToYourConsignmentPage?.SelectDestinationCountries(destinationCountries);
        }

        [When("the user describes the seed commodity with botanical name {string}, variety {string}, lot reference {string}, sample reference, quantity {string}, unit of measurement {string}, number of packages {string}, type of packaging {string}, countries of origin {string}")]
        public void WhenTheUserDescribesTheSeedCommodityForPhytosanitaryCertificate(string botanicalName, string variety, string lotReference, string quantity,
            string unit, string numberOfPackages, string typeOfPackaging, string countriesOfOrigin)
        {
            exporterAddSeedsToYourConsignmentPage?.EnterBotanicalName(botanicalName);
            _scenarioContext["Commodity"] = exporterAddSeedsToYourConsignmentPage?.GetSelectedCommodityDescription();
            exporterAddSeedsToYourConsignmentPage?.EnterVariety(variety);
            exporterAddSeedsToYourConsignmentPage?.EnterLotReference(lotReference);
            exporterAddSeedsToYourConsignmentPage?.EnterSampleReference(_scenarioContext.Get<string>("StockRequestName"));
            exporterAddSeedsToYourConsignmentPage?.EnterQuantity(quantity);
            exporterAddSeedsToYourConsignmentPage?.SelectUnitOfMeasurement(unit);
            exporterAddSeedsToYourConsignmentPage?.EnterNumberOfPackages(numberOfPackages);
            exporterAddSeedsToYourConsignmentPage?.SelectTypeOfPackaging(typeOfPackaging);
            exporterAddSeedsToYourConsignmentPage?.SelectCountriesOfOrigin(countriesOfOrigin);

            _scenarioContext["SeedVariety"] = variety;
            _scenarioContext["SeedLotReference"] = lotReference;
            _scenarioContext["SeedQuantity"] = quantity;
            _scenarioContext["SeedUnit"] = unit;
            _scenarioContext["SeedPackaging"] = typeOfPackaging;
            _scenarioContext["SeedCountriesOfOrigin"] = countriesOfOrigin;
        }

        [When("the only options available for Unit of measurement are {string} {string} {string}")]
        public void WhenTheOnlyOptionsAvailableForUnitOfMeasurementAre(string unit1, string unit2, string unit3)
        {
            var expected = new List<string> { "Unit of measurement", unit1, unit2, unit3 };
            var actual = exporterAddSeedsToYourConsignmentPage?.GetUnitOfMeasurementOptions();
            Assert.That(actual, Is.EquivalentTo(expected), $"Unit of measurement options were '{string.Join(", ", actual ?? new List<string>())}'");
        }

        [Then("the seed commodity details are displayed with botanical name {string}, variety {string}, lot reference {string}, quantity {string}, unit of measurement {string}, chemically treated {string}, chemicals used {string}, country where grown {string}, destination countries {string}")]
        public void ThenTheSeedCommodityDetailsAreDisplayed(string botanicalName, string variety, string lotReference, string quantity,
            string unit, string chemicallyTreated, string chemicalsUsed, string countryWhereGrown, string destinationCountries)
        {
            Assert.True(exporterAddSeedsToYourConsignmentPage?.AreSeedCommodityDetailsDisplayed(botanicalName, variety, lotReference, quantity,
                unit, chemicallyTreated, chemicalsUsed, countryWhereGrown, destinationCountries), "Seed commodity details are not displayed as entered");
        }

        [When("the user clicks the Save and continue button on the Add seeds to your consignment page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheAddSeedsToYourConsignmentPage()
        {
            exporterAddSeedsToYourConsignmentPage?.ClickSaveAndContinueButton();
        }
    }
}
