using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterAddCommoditiesToYourConsignmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterAddCommoditiesToYourConsignmentPage? exporterAddCommoditiesToYourConsignmentPage =>
            _objectContainer.IsRegistered<IExporterAddCommoditiesToYourConsignmentPage>()
                ? _objectContainer.Resolve<IExporterAddCommoditiesToYourConsignmentPage>()
                : null;

        public ExporterAddCommoditiesToYourConsignmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Add commodities to your consignment page is displayed")]
        public void ThenTheAddCommoditiesToYourConsignmentPageIsDisplayed()
        {
            Assert.True(exporterAddCommoditiesToYourConsignmentPage?.IsPageLoaded(), "Add commodities to your consignment page is not displayed");
        }

        [When(@"^the user enters commodity details with variety '([^']*)', specific variety '([^']*)', quality class '([^']*)', country of origin '([^']*)', net weight per package '([^']*)', number of packages '([^']*)', type of packaging '([^']*)' and reusable packaging '([^']*)'$")]
        public void WhenTheUserEntersCommodityDetailsWithVarietySpecificVarietyQualityClassCountryOfOriginNetWeightPerPackageNumberOfPackagesTypeOfPackagingAndReusablePackaging(
            string varietyType,
            string specificVariety,
            string qualityClass,
            string countryOfOrigin,
            string netWeightPerPackage,
            string numberOfPackages,
            string typeOfPackaging,
            string reusablePackaging)
        {
            exporterAddCommoditiesToYourConsignmentPage?.EnterCommodityDetails(
                varietyType,
                specificVariety,
                qualityClass,
                countryOfOrigin,
                netWeightPerPackage,
                numberOfPackages,
                typeOfPackaging,
                reusablePackaging);

            _scenarioContext["Variety"] = varietyType;
            _scenarioContext["SpecificVariety"] = specificVariety;
        }

        [When("the user clicks the Save and continue button on the Add commodities to your consignment page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheAddCommoditiesToYourConsignmentPage()
        {
            exporterAddCommoditiesToYourConsignmentPage?.ClickSaveAndContinueButton();
        }
    }
}