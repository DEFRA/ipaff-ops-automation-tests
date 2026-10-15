using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterAddBulbsToYourConsignmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterAddBulbsToYourConsignmentPage? exporterAddBulbsToYourConsignmentPage =>
            _objectContainer.IsRegistered<IExporterAddBulbsToYourConsignmentPage>()
                ? _objectContainer.Resolve<IExporterAddBulbsToYourConsignmentPage>()
                : null;

        public ExporterAddBulbsToYourConsignmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Add bulbs to your consignment page is displayed")]
        public void ThenTheAddBulbsToYourConsignmentPageIsDisplayed()
        {
            Assert.True(exporterAddBulbsToYourConsignmentPage?.IsPageLoaded(), "Add bulbs to your consignment page is not displayed");
        }

        [When("the user describes the commodity with botanical name {string}, commodity type {string}, quantity {string}, unit of measurement {string}, number of packages {string}, type of packaging {string}, countries of origin {string}")]
        public void WhenTheUserDescribesTheCommodity(string botanicalName, string commodityType, string quantity, string unitOfMeasurement, string numberOfPackages, string packagingType, string countriesOfOrigin)
        {
            exporterAddBulbsToYourConsignmentPage?.EnterBotanicalName(botanicalName);
            _scenarioContext["Commodity"] = exporterAddBulbsToYourConsignmentPage?.GetSelectedCommodityDescription() ?? string.Empty;
            exporterAddBulbsToYourConsignmentPage?.SelectCommodityType(commodityType);
            exporterAddBulbsToYourConsignmentPage?.EnterQuantity(quantity);
            exporterAddBulbsToYourConsignmentPage?.SelectUnitOfMeasurement(unitOfMeasurement);
            exporterAddBulbsToYourConsignmentPage?.EnterNumberOfPackages(numberOfPackages);
            exporterAddBulbsToYourConsignmentPage?.SelectTypeOfPackaging(packagingType);
            exporterAddBulbsToYourConsignmentPage?.SelectCountriesOfOrigin(countriesOfOrigin);
        }

        [When("the user clicks the Save and continue button on the Add bulbs to your consignment page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheAddBulbsToYourConsignmentPage()
        {
            exporterAddBulbsToYourConsignmentPage?.ClickSaveAndContinueButton();
        }
    }
}
