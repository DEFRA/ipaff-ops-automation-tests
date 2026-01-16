using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class OriginOfImportSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IOriginOfImportPage? originOfImportPage => _objectContainer.IsRegistered<IOriginOfImportPage>() ? _objectContainer.Resolve<IOriginOfImportPage>() : null;


        public OriginOfImportSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Origin of the import page should be displayed")]
        public void ThenTheOriginOfTheImportPageShouldBeDisplayed()
        {
            Assert.True(originOfImportPage?.IsPageLoaded(), "About the consignment Origin of the import page not loaded");
        }

        [Then("the Origin of the import page should be displayed, showing {string} as the Country of origin and Country from where consigned")]
        public void ThenTheOriginOfTheImportPageShouldBeDisplayedShowingAsTheCountryOfOriginAndCountryFromWhereConsigned(string country)
        {
            Assert.True(originOfImportPage?.IsPageLoaded(), "About the consignment Origin of the import page not loaded");
            _scenarioContext["ContryFromWhereConsigned"] = country;
        }

        [When("the user chooses {string} for Does your consignment require a region code?")]
        public void WhenTheUserChoosesForDoesYourConsignmentRequireARegionCode(string option)
        {
            originOfImportPage?.IsRegionOfOriginCodeNeeded(option);
            _scenarioContext["IsRegionOfOriginCodeRequired"] = option;
        }

        [When("the user chooses {string} for Does this consignment conform to regulatory regulations?")]
        public void WhenTheUserChoosesForDoesThisConsignmentConformToRegulatoryRegulations(string option)
        {
            originOfImportPage?.IsConformToRegulatoryRequirements(option);
            _scenarioContext["ConsignmentConformToRegulatoryRequirements"] = option;
        }

        [When("the user chooses {string} for Will the consignment change vehicles or means of transport after the Border Control Post \\(BCP)?")]
        public void WhenTheUserChoosesForWillTheConsignmentChangeVehiclesOrMeansOfTransportAfterTheBorderControlPostBCP(string option)
        {
            originOfImportPage?.IsItAfterBCP(option);
        }

        [When("the user enters a reference number {string} in the Add a reference number for this consignment \\(optional) field")]
        public void WhenTheUserEntersAReferenceNumberInTheAddAReferenceNumberForThisConsignmentOptionalField(string refNum)
        {
            originOfImportPage?.EnterConsignmentRefNum(refNum);
            _scenarioContext["ConsignmentReferenceNumber"] = refNum;
        }

        [When("I click the forward button in the browser")]
        public void WhenIClickTheForwardButtonInTheBrowser()
        {
            originOfImportPage?.ClickBrowserForwardButton();
        }

        [When("the user clicks on Save and return to hub")]
        public void WhenTheUserClicksOnSaveAndReturnToHub()
        {
            originOfImportPage?.ClickSaveAndReturnToHub();
        }

        [When("country of origin and Country from where consigned fields are pre-populated with previously selected country")]
        public void ThenCountryOfOriginAndCountryFromWhereConsignedFieldsArePre_PopulatedWithPreviouslySelectedCountry()
        {
            var countryOfOrigin = _scenarioContext.Get<string>("CountryOfOrigin");
            Assert.Multiple(() =>
            {
                Assert.True(countryOfOrigin.Equals(originOfImportPage?.GetOriginCountryText), "Country of origin field does not contain previously selected country");
                Assert.True(countryOfOrigin.Equals(originOfImportPage?.GetOriginCountryText), "Country from where consigned field does not contain previously selected country");
            });
        }

        [When("{string} is pre-selected for the Does your consignment require a region code? radio option")]
        public void ThenIsPre_SelectedForTheDoesYourConsignmentRequireARegionCodeRadioOption(string ragionCodeRadio)
        {
            Assert.True(originOfImportPage?.IsRegionCodeRadioSelected(ragionCodeRadio), $"Region code radio is not pre-selected with {ragionCodeRadio} option");
        }

        [When("the user changes the consigned country to {string}")]
        public void WhenTheUserChangesTheConsignedCountryTo(string consignedCountry)
        {
            originOfImportPage?.SelectConsignedCountry(consignedCountry);
            _scenarioContext["ContryFromWhereConsigned"] = consignedCountry;
        }
    }
}