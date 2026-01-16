using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AddressBookSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAddressBookPage? addressBookPage => _objectContainer.IsRegistered<IAddressBookPage>() ? _objectContainer.Resolve<IAddressBookPage>() : null;

        public AddressBookSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Address book page should be displayed")]
        public void ThenTheAddressBookPageShouldBeDisplayed()
        {
            Assert.True(addressBookPage?.IsPageLoaded(), "Address book page is not loaded");
        }

        [When("the user searches by selecting {string} in the Type dropdown")]
        public void WhenTheUserSearchesBySelectingInTheTypeDropdown(string type)
        {
            addressBookPage?.SelectType(type);
            addressBookPage?.ClickSearchButton();
        }

        [Then("the type of every address listed is {string}")]
        public void ThenTheTypeOfEveryAddressListedIs(string type)
        {
            addressBookPage?.ValidateTypeInSearchResults(type);
        }

        [When("the user clicks on the Dashboard link on the top left")]
        public void WhenTheUserClicksOnTheDashboardLinkOnTheTopLeft()
        {
            addressBookPage?.ClickDashboardLink();
        }
    }
}