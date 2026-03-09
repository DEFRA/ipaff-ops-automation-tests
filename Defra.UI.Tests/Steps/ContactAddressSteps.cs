using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
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
            _scenarioContext["ConsignmentContactAddress"] = selectedAddress;
        }

        [Then("the user records the Draft CHED number")]
        public void ThenTheUserRecordsTheDraftCHEDNumber()
        {
            _scenarioContext["DraftCHEDReference"] = contactAddressPage?.GetDraftCHEDRefNumber;
        }

        [Then("the user verifies and enters any missing data on the Contact address for consignment page")]
        public void ThenTheUserVerifiesAndEntersAnyMissingDataOnTheContactAddressForConsignmentPage()
        {
            Assert.True(contactAddressPage?.IsContactAddressForConsignmentPageLoaded(), "Contact address for consignment page not loaded");

            if (contactAddressPage.IsContactAddressRadioButtonSelected())
                ThenTheUserSelectsAContactAddressForTheConsignment();
            else
            {
                contactAddressPage?.SelectContactAddressRadio();
                ThenTheUserSelectsAContactAddressForTheConsignment();
            }
        }

        [Then("the Contact address for consignment should default to the first address on the list")]
        public void ThenTheContactAddressForConsignmentShouldDefaultToTheFirstAddressOnTheList()
        {
            Assert.True(contactAddressPage?.IsFirstContactAddressSelected(),
                "The first contact address radio button is not selected by default.");
        }

        [Then("the Contact address for consignment is prepopulated")]
        public void ThenTheContactAddressForConsignmentIsPrepopulated()
        {
            Assert.True(contactAddressPage?.IsFirstContactAddressSelected(),
                            "The first contact address radio button is not selected by default.");

            var selectedAddress = contactAddressPage?.GetSelectedContactAddress();
            _scenarioContext["ConsignmentContactAddress"] = selectedAddress;
        }

    }
}