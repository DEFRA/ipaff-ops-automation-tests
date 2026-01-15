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

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IAddressBookPage? addressBookPage => _objectContainer.IsRegistered<IAddressBookPage>() ? _objectContainer.Resolve<IAddressBookPage>() : null;

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

        [When(@"the user clicks Add an address")]
        public void WhenTheUserClicksAddAnAddress()
        {
            addressBookPage?.ClickAddAnAddress();
        }

        [When(@"the user clicks Dashboard link")]
        public void WhenTheUserClicksDashboardLink()
        {
            addressBookPage?.ClickDashboard();
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
    }
}