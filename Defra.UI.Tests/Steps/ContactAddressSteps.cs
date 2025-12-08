using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ContactAddressSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IContactAddressPage? contactAddressPage => _objectContainer.IsRegistered<IContactAddressPage>() ? _objectContainer.Resolve<IContactAddressPage>() : null;


        public ContactAddressSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Contact address for consignment page should be displayed")]
        public void ThenTheContactAddressForConsignmentPageShouldBeDisplayed()
        {
            Assert.True(contactAddressPage?.IsPageLoaded(), "Complete notification Contact address for consignment page not loaded");
        }

        [Then("the Contacts - Contact address for consignment page should be displayed")]
        public void ThenTheContacts_ContactAddressForConsignmentPageShouldBeDisplayed()
        {
            Assert.True(contactAddressPage?.IsContactAddressForConsignmentPageLoaded(), "Complete notification Contact address for consignment page not loaded");
        }

        [Then("the Contact address for consignment page should be displayed without the secondary title")]
        public void ThenTheContactAddressForConsignmentPageShouldBeDisplayedWithoutTheSecondaryTitle()
        {
            Assert.True(contactAddressPage?.IsPageLoadedWithoutSecondaryTitle(), "Complete notification Contact address for consignment page not loaded");
        }
       
        [Then("the user selects a contact address for the consignment")]
        public void ThenTheUserSelectsAContactAddressForTheConsignment()
        {
            var selectedAddress = contactAddressPage?.GetSelectedContactAddress();
            _scenarioContext.Add("ConsignmentContactAddress", selectedAddress);
        }
    }
}