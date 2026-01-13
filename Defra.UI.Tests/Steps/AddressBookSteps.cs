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
        private IChooseAddressTypePage? chooseAddressTypePage => _objectContainer.IsRegistered<IChooseAddressTypePage>() ? _objectContainer.Resolve<IChooseAddressTypePage>() : null;
        private IChooseOperatorTypePage? chooseOperatorTypePage => _objectContainer.IsRegistered<IChooseOperatorTypePage>() ? _objectContainer.Resolve<IChooseOperatorTypePage>() : null;
        private IAddOperatorDetailsPage? addOperatorDetailsPage => _objectContainer.IsRegistered<IAddOperatorDetailsPage>() ? _objectContainer.Resolve<IAddOperatorDetailsPage>() : null;
        private ITheAddressHasBeenAddedPage? theAddressHasBeenAddedPage => _objectContainer.IsRegistered<ITheAddressHasBeenAddedPage>() ? _objectContainer.Resolve<ITheAddressHasBeenAddedPage>() : null;

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
                
        [Then(@"the newly added address '([^']*)' should be displayed in the address book")]
        public void ThenTheNewlyAddedAddressShouldBeDisplayedInTheAddressBook(string addressName)
        {
            Assert.True(addressBookPage?.IsAddressDisplayedInAddressBook(addressName), 
                $"Address '{addressName}' not found in address book");
        }

        [When(@"the user clicks Dashboard link")]
        public void WhenTheUserClicksDashboardLink()
        {
            addressBookPage?.ClickDashboard();
        }       
    }
}