using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class OriginOfProductSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IOriginOfProductPage? originOfProductPage => _objectContainer.IsRegistered<IOriginOfProductPage>() ? _objectContainer.Resolve<IOriginOfProductPage>() : null;
        private IOriginOfImportPage? originOfImportPage => _objectContainer.IsRegistered<IOriginOfImportPage>() ? _objectContainer.Resolve<IOriginOfImportPage>() : null;


        public OriginOfProductSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Origin of the animal or product page should be displayed")]
        [Then("the Origin of the plants plant product or other objects page should be displayed")]
        public void ThenTheOriginOfThePlantsPlantProductOrOtherObjectsPageShouldBeDisplayed()
        {
            Assert.True(originOfProductPage?.IsPageLoaded(), "About the consignment Origin of the animal or product? page not loaded");
        }

        [When("the user changes the Country of origin to {string}")]
        [When("the user chooses {string} from the dropdown for Country of origin")]
        public void WhenTheUserChoosesFromTheDropdownForCountryOfOrigin(string country)
        {
            originOfProductPage?.SelectCountryOfOrigin(country);
            _scenarioContext["CountryOfOrigin"] = country;
            _scenarioContext["ContryFromWhereConsigned"] = country;
        }

        [When("the user chooses {string} from the dropdown for Country of origin and records the country from where consigned")]
        public void WhenTheUserChoosesFromTheDropdownForCountryOfOriginAndRecordsTheCountryFromWhereConsigned(string country)
        {
            originOfProductPage?.SelectCountryOfOrigin(country);
            _scenarioContext["CountryOfOrigin"] = country;
            _scenarioContext["ContryFromWhereConsigned"] = country;
        }
    }
}