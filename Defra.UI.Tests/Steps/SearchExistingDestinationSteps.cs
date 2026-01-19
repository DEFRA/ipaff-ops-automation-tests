using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SearchExistingDestinationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISearchExistingDestinationPage? searchExistingDestinationPage => _objectContainer.IsRegistered<ISearchExistingDestinationPage>() ? _objectContainer.Resolve<ISearchExistingDestinationPage>() : null;


        public SearchExistingDestinationSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Search for an existing place of destination page should be displayed")]
        public void ThenTheSearchForAnExistingPlaceOfDestinationPageShouldBeDisplayed()
        {
            Assert.True(searchExistingDestinationPage?.IsPageLoaded(), "Traders Search for an existing place of destination page not loaded");
        }

        [When("the user selects a place of destination {string} with a UK country")]
        public void WhenTheUserSelectsAPlaceOfDestinationWithAUKCountry(string destination)
        {
            _scenarioContext["PlaceOfDestinationDetails"] = searchExistingDestinationPage?.GetSelectedPlaceOfDestination(destination);
            searchExistingDestinationPage?.ClickSelect(destination);
        }

        [When("the user selects a place of destination {string}")]
        public void WhenTheUserSelectsAPlaceOfDestination(string destination)
        {
            var destinationName = searchExistingDestinationPage?.GetSelectedDestinationName(destination);
            var destinationAddress = searchExistingDestinationPage?.GetSelectedDestinationAddress(destination);
            var destinationCountry = searchExistingDestinationPage?.GetSelectedDestinationCountry(destination);

            _scenarioContext.Add("PlaceOfDestinationName", destinationName);
            _scenarioContext.Add("PlaceOfDestinationAddress", destinationAddress);
            _scenarioContext.Add("PlaceOfDestinationCountry", destinationCountry);
            _scenarioContext["PlaceOfDestinationDetails"] = searchExistingDestinationPage?.GetSelectedPlaceOfDestination(destination);

            searchExistingDestinationPage?.ClickSelect(destination);
        }
    }
}