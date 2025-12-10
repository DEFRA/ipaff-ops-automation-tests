using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CountriesConsignmentTravelSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ICountriesConsignmentTravelPage? countriesConsignmentTravelPage => _objectContainer.IsRegistered<ICountriesConsignmentTravelPage>() ? _objectContainer.Resolve<ICountriesConsignmentTravelPage>() : null;


        public CountriesConsignmentTravelSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Which countries will the consignment travel through? page should be displayed")]
        public void ThenTheWhichCountriesWillTheConsignmentTravelThroughPageShouldBeDisplayed()
        {
            Assert.True(countriesConsignmentTravelPage?.IsPageLoaded(), "Which countries will the consignment travel through? page not loaded");
        }

        [When("the user selects {string} for Will the consignment travel through any other countries before reaching the UK?")]
        public void WhenTheUserSelectsForWillTheConsignmentTravelThroughAnyOtherCountriesBeforeReachingTheUK(string country)
        {
            countriesConsignmentTravelPage?.SelectCountry(country);
            _scenarioContext.Add("CountriesConsignmentWillTravelThrough", country);
        }
    }
}