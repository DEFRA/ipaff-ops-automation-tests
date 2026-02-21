using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Pages.Classes;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SearchExistingDeliveryAddressSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISearchExistingDeliveryAddressPage? searchExistingDeliveryAddressPage => _objectContainer.IsRegistered<ISearchExistingDeliveryAddressPage>() ? _objectContainer.Resolve<ISearchExistingDeliveryAddressPage>() : null;

        public SearchExistingDeliveryAddressSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }


        [Then("Search for an existing delivery address page should be displayed")]
        public void ThenSearchForAnExistingDeliveryAddressPageShouldBeDisplayed()
        {
            Assert.True(searchExistingDeliveryAddressPage?.IsPageLoaded(), "Traders Search for an existing place of destination page not loaded");
        }

        [When("the user selects one of the displayed delivery address {string}")]
        public void WhenTheUserSelectsOneOfTheDisplayedDeliveryAddress(string deliveryAddress)
        {
            var deliveryAddressName = searchExistingDeliveryAddressPage?.GetSelectedDeliveryAddressName(deliveryAddress);
            var fulldeliveryAddress = searchExistingDeliveryAddressPage?.GetSelectedDeliveryAddress(deliveryAddress);
            var deliveryCountry = searchExistingDeliveryAddressPage?.GetSelectedDeliveryCountry(deliveryAddress);

            _scenarioContext["DeliveryAddressName"] = deliveryAddressName;
            _scenarioContext["DeliveryAddress"] = fulldeliveryAddress;
            _scenarioContext["DeliveryCountry"] = deliveryCountry;
            _scenarioContext["DeliveryAddressDetails"] = searchExistingDeliveryAddressPage?.GetSelectedDeliveryAddressDetails(deliveryAddress);

            searchExistingDeliveryAddressPage?.ClickSelect(deliveryAddress);
        }       
    }
}