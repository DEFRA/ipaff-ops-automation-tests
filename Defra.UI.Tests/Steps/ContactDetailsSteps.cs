using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ContactDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IContactDetailsPage? contactDetailsPage => _objectContainer.IsRegistered<IContactDetailsPage>() ? _objectContainer.Resolve<IContactDetailsPage>() : null;


        public ContactDetailsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Contact details page should be displayed, pre-populated with the user's details")]
        public void ThenTheContactDetailsPageShouldBeDisplayedPre_PopulatedWithTheUsersDetails()
        {
            Assert.True(contactDetailsPage?.IsPageLoaded(), "Contact details page not loaded");
        }

        [Then("the user verifies and enters any missing data on the Contact details page")]
        public void ThenTheUserVerifiesAndEntersAnyMissingDataOnTheContactDetailsPage()
        {
            Assert.True(contactDetailsPage?.IsPageLoaded(), "Contact details page not loaded");
            Assert.True(contactDetailsPage?.ValidateIfContactDetailsArePopulated(), "Contact details should be populated by default");
        }
    }
}