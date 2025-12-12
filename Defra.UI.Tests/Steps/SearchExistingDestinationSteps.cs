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

        [When("the user selects a place of destination with a UK country")]
        public void WhenTheUserSelectsAPlaceOfDestinationWithAUKCountry()
        {
            _scenarioContext.Add("PlaceOfDestinationDetails", searchExistingDestinationPage.GetSelectedPlaceOfDestination());
            searchExistingDestinationPage?.ClickSelect();
        }

        [When("the user selects a place of destination")]
        public void WhenTheUserSelectsAPlaceOfDestination()
        {
            var destinationName = searchExistingDestinationPage?.GetSelectedDestinationName();
            var destinationAddress = searchExistingDestinationPage?.GetSelectedDestinationAddress();
            var destinationCountry = searchExistingDestinationPage?.GetSelectedDestinationCountry();

            _scenarioContext.Add("DestinationName", destinationName);
            _scenarioContext.Add("DestinationAddress", destinationAddress);
            _scenarioContext.Add("DestinationCountry", destinationCountry);

            searchExistingDestinationPage?.ClickSelect();
        }
    }
}