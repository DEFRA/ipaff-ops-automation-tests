using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
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
        private IViewOperatorPage? viewOperatorPage => _objectContainer.IsRegistered<IViewOperatorPage>() ? _objectContainer.Resolve<IViewOperatorPage>() : null;
        private IDeleteAddressPage? deleteAddressPage => _objectContainer.IsRegistered<IDeleteAddressPage>() ? _objectContainer.Resolve<IDeleteAddressPage>() : null;
        private ITheAddressHasBeenDeletedPage? theAddressHasBeenDeletedPage => _objectContainer.IsRegistered<ITheAddressHasBeenDeletedPage>() ? _objectContainer.Resolve<ITheAddressHasBeenDeletedPage>() : null;

        public AddressBookSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then(@"the Address book page should be displayed")]
        public void ThenTheAddressBookPageShouldBeDisplayed()
        {
            Assert.True(addressBookPage?.IsPageLoaded(), "Address Book page not loaded");
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
        [When("the user clicks on Dashboard above Address book")]
        public void WhenTheUserClicksOnTheDashboardLinkOnTheTopLeft()
        {
            addressBookPage?.ClickDashboardLink();
        }

        [When(@"the user clicks Add an address")]
        public void WhenTheUserClicksAddAnAddress()
        {
            addressBookPage?.ClickAddAnAddress();
        }


        [Then(@"the newly added operator {string} should be displayed in the address book")]
        public void ThenTheNewlyAddedOperatorShouldBeDisplayedInTheAddressBook(string operatorType)
        {
            // Get the operator details stored in scenario context when it was added
            var operatorName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var operatorAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var operatorCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();

            // Verify the operator is displayed in the address book with all details
            var isDisplayed = addressBookPage?.IsOperatorDisplayedInAddressBook(operatorName, operatorType, operatorAddress, operatorCountry);
            Assert.IsTrue(isDisplayed, $"Operator '{operatorName}' of type '{operatorType}' with address '{operatorAddress}' and country '{operatorCountry}' not found in address book");
        }

        [Then(@"the user deletes the newly added operator '(.*)'")]
        public void WhenTheUserDeletesTheNewlyAddedOperator(string operatorType)
        {
            // Get the operator name from scenario context
            var operatorName = _scenarioContext[$"{operatorType}Name"]?.ToString();

            Assert.That(operatorName, Is.Not.Null.And.Not.Empty,
                $"Operator name for type '{operatorType}' not found in scenario context.");

            // Step 1: Click View on the operator row
            addressBookPage?.ClickViewOperator(operatorName);

            // Verify View Operator page loaded
            Assert.IsTrue(viewOperatorPage?.IsPageLoaded(operatorName),
                $"View Operator page did not load for operator '{operatorName}'");

            // Step 2: Click Delete button
            viewOperatorPage?.ClickDelete();

            // Verify Delete Address page loaded
            Assert.IsTrue(deleteAddressPage?.IsPageLoaded(),
                "Delete Address page did not load");

            // Step 3: Click "Yes, delete this address" button
            deleteAddressPage?.ClickYesDeleteThisAddress();

            // Verify The Address Has Been Deleted page loaded
            Assert.IsTrue(theAddressHasBeenDeletedPage?.IsPageLoaded(),
                "The Address Has Been Deleted confirmation page did not load");

            // Step 4: Click Return to Address Book
            theAddressHasBeenDeletedPage?.ClickReturnToAddressBook();

            // Verify we're back on Address Book page
            Assert.IsTrue(addressBookPage?.IsPageLoaded(),
                "Address Book page did not load after deletion");
        }
    }
}