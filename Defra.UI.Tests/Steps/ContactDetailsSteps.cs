using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ContactDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
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
    }
}