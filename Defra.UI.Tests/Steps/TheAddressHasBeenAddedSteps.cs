using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class TheAddressHasBeenAddedSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ITheAddressHasBeenAddedPage? theAddressHasBeenAddedPage => _objectContainer.IsRegistered<ITheAddressHasBeenAddedPage>() ? _objectContainer.Resolve<ITheAddressHasBeenAddedPage>() : null;

        public TheAddressHasBeenAddedSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then(@"the address has been added to your address book page should be displayed")]
        public void ThenTheAddressHasBeenAddedToYourAddressBookPageShouldBeDisplayed()
        {
            Assert.True(theAddressHasBeenAddedPage?.IsPageLoaded(), "The address has been added page not loaded");
        }

        [When(@"the user clicks Return to Address Book")]
        public void WhenTheUserClicksReturnToAddressBook()
        {
            theAddressHasBeenAddedPage?.ClickReturnToAddressBook();
        }
    }
}