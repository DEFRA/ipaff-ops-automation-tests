using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterAddToYourConsignmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterAddToYourConsignmentPage? exporterAddToYourConsignmentPage =>
            _objectContainer.IsRegistered<IExporterAddToYourConsignmentPage>()
                ? _objectContainer.Resolve<IExporterAddToYourConsignmentPage>()
                : null;

        public ExporterAddToYourConsignmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Add {string} to your consignment page is displayed")]
        public void ThenTheAddToYourConsignmentPageIsDisplayed(string commodity)
        {
            Assert.True(exporterAddToYourConsignmentPage?.IsPageLoaded(commodity), $"Add {commodity} to your consignment page is not displayed");
        }

        [When("the user describes the commodity with botanical name {string}, commodity type {string}, quantity {string}, unit of measurement {string}, number of packages {string}, type of packaging {string}, countries of origin {string}")]
        public void WhenTheUserDescribesTheCommodity(string botanicalName, string commodityType, string quantity, string unitOfMeasurement, string numberOfPackages, string packagingType, string countriesOfOrigin)
        {
            exporterAddToYourConsignmentPage?.EnterBotanicalName(botanicalName);
            _scenarioContext["Commodity"] = exporterAddToYourConsignmentPage?.GetSelectedCommodityDescription() ?? string.Empty;
            exporterAddToYourConsignmentPage?.SelectCommodityType(commodityType);
            exporterAddToYourConsignmentPage?.EnterQuantity(quantity);
            exporterAddToYourConsignmentPage?.SelectUnitOfMeasurement(unitOfMeasurement);
            _scenarioContext["UnitOfMeasurement"] = unitOfMeasurement;
            exporterAddToYourConsignmentPage?.EnterNumberOfPackages(numberOfPackages);
            exporterAddToYourConsignmentPage?.SelectTypeOfPackaging(packagingType);
            exporterAddToYourConsignmentPage?.SelectCountriesOfOrigin(countriesOfOrigin);

            _scenarioContext["PlantCommodityType"] = commodityType;
            _scenarioContext["PlantQuantity"] = quantity;
            _scenarioContext["PlantUnit"] = unitOfMeasurement;
            _scenarioContext["PlantNumberOfPackages"] = numberOfPackages;
            _scenarioContext["PlantPackaging"] = packagingType;
            _scenarioContext["PlantCountriesOfOrigin"] = countriesOfOrigin;
            _scenarioContext.Remove("DrawDownQuantity");
        }

        [When("the user describes the seed commodity with botanical name {string}, variety {string}, lot reference {string}, quantity {string}, unit of measurement {string}, chemically treated {string}, chemicals used {string}, country where grown {string}, destination countries {string}")]
        public void WhenTheUserDescribesTheSeedCommodityForSeedSampling(string botanicalName, string variety, string lotReference, string quantity,
            string unit, string chemicallyTreated, string chemicalsUsed, string countryWhereGrown, string destinationCountries)
        {
            exporterAddToYourConsignmentPage?.EnterBotanicalName(botanicalName);
            _scenarioContext["Commodity"] = exporterAddToYourConsignmentPage?.GetSelectedCommodityDescription();
            exporterAddToYourConsignmentPage?.EnterVariety(variety);
            exporterAddToYourConsignmentPage?.EnterLotReference(lotReference);
            exporterAddToYourConsignmentPage?.EnterQuantity(quantity);
            exporterAddToYourConsignmentPage?.SelectUnitOfMeasurement(unit);
            exporterAddToYourConsignmentPage?.SelectChemicallyTreated(chemicallyTreated);
            exporterAddToYourConsignmentPage?.EnterChemicalsUsed(chemicalsUsed);
            exporterAddToYourConsignmentPage?.SelectCountryWhereGrown(countryWhereGrown);
            exporterAddToYourConsignmentPage?.SelectDestinationCountries(destinationCountries);
        }

        [When("the user describes the seed commodity with botanical name {string}, variety {string}, lot reference {string}, sample reference, quantity {string}, unit of measurement {string}, number of packages {string}, type of packaging {string}, countries of origin {string}")]
        public void WhenTheUserDescribesTheSeedCommodityForPhytosanitaryCertificate(string botanicalName, string variety, string lotReference, string quantity,
            string unit, string numberOfPackages, string typeOfPackaging, string countriesOfOrigin)
        {
            exporterAddToYourConsignmentPage?.EnterBotanicalName(botanicalName);
            _scenarioContext["Commodity"] = exporterAddToYourConsignmentPage?.GetSelectedCommodityDescription();
            _scenarioContext["EppoCode"] = exporterAddToYourConsignmentPage?.GetSelectedCommodityEppoCode();
            exporterAddToYourConsignmentPage?.EnterVariety(variety);
            exporterAddToYourConsignmentPage?.EnterLotReference(lotReference);
            exporterAddToYourConsignmentPage?.EnterSampleReference(_scenarioContext.Get<string>("StockRequestName"));
            exporterAddToYourConsignmentPage?.EnterQuantity(quantity);
            exporterAddToYourConsignmentPage?.SelectUnitOfMeasurement(unit);
            exporterAddToYourConsignmentPage?.EnterNumberOfPackages(numberOfPackages);
            exporterAddToYourConsignmentPage?.SelectTypeOfPackaging(typeOfPackaging);
            exporterAddToYourConsignmentPage?.SelectCountriesOfOrigin(countriesOfOrigin);

            _scenarioContext["SeedVariety"] = variety;
            _scenarioContext["SeedLotReference"] = lotReference;
            _scenarioContext["SeedQuantity"] = quantity;
            _scenarioContext["SeedUnit"] = unit;
            _scenarioContext["SeedNumberOfPackages"] = numberOfPackages;
            _scenarioContext["SeedPackaging"] = typeOfPackaging;
            _scenarioContext["SeedCountriesOfOrigin"] = countriesOfOrigin;
        }

        [When(@"^the user enters commodity details with variety '([^']*)', specific variety '([^']*)', quality class '([^']*)', country of origin '([^']*)', net weight per package '([^']*)', number of packages '([^']*)', type of packaging '([^']*)' and reusable packaging '([^']*)'$")]
        public void WhenTheUserEntersCommodityDetails(
            string varietyType,
            string specificVariety,
            string qualityClass,
            string countryOfOrigin,
            string netWeightPerPackage,
            string numberOfPackages,
            string typeOfPackaging,
            string reusablePackaging)
        {
            exporterAddToYourConsignmentPage?.EnterCommodityDetails(
                varietyType,
                specificVariety,
                qualityClass,
                countryOfOrigin,
                netWeightPerPackage,
                numberOfPackages,
                typeOfPackaging,
                reusablePackaging);

            _scenarioContext["Variety"] = specificVariety;
        }

        [When(@"^the user describes the commodity with common name '([^']*)', botanical name '([^']*)', country of origin '([^']*)', net weight per package '([^']*)', number of packages '([^']*)', type of packaging '([^']*)', reusable packaging '([^']*)'$")]
        public void WhenTheUserDescribesTheCommodityWithCommonName(
            string commonName,
            string botanicalName,
            string countryOfOrigin,
            string netWeightPerPackage,
            string numberOfPackages,
            string typeOfPackaging,
            string reusablePackaging)
        {
            exporterAddToYourConsignmentPage?.DescribeCommodity(
                commonName,
                botanicalName,
                countryOfOrigin,
                netWeightPerPackage,
                numberOfPackages,
                typeOfPackaging,
                reusablePackaging);

            _scenarioContext["Commodity"] = commonName;
        }

        [When("the only options available for Unit of measurement are {string} {string} {string}")]
        public void WhenTheOnlyOptionsAvailableForUnitOfMeasurementAre(string unit1, string unit2, string unit3)
        {
            var expected = new List<string> { "Unit of measurement", unit1, unit2, unit3 };
            var actual = exporterAddToYourConsignmentPage?.GetUnitOfMeasurementOptions();
            Assert.That(actual, Is.EquivalentTo(expected), $"Unit of measurement options were '{string.Join(", ", actual ?? new List<string>())}'");
        }

        [Then("the seed commodity details are displayed with botanical name {string}, variety {string}, lot reference {string}, quantity {string}, unit of measurement {string}, chemically treated {string}, chemicals used {string}, country where grown {string}, destination countries {string}")]
        public void ThenTheSeedCommodityDetailsAreDisplayed(string botanicalName, string variety, string lotReference, string quantity,
            string unit, string chemicallyTreated, string chemicalsUsed, string countryWhereGrown, string destinationCountries)
        {
            Assert.True(exporterAddToYourConsignmentPage?.AreSeedCommodityDetailsDisplayed(botanicalName, variety, lotReference, quantity,
                unit, chemicallyTreated, chemicalsUsed, countryWhereGrown, destinationCountries), "Seed commodity details are not displayed as entered");
        }

        [When("the user clicks the Save and continue button on the Add {string} to your consignment page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheAddToYourConsignmentPage(string commodity)
        {
            exporterAddToYourConsignmentPage?.ClickSaveAndContinueButton();
        }
    }
}
