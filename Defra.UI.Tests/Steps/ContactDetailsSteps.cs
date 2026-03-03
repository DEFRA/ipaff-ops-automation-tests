using Defra.UI.Framework.Driver;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ContactDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        private IContactDetailsPage? contactDetailsPage => _objectContainer.IsRegistered<IContactDetailsPage>() ? _objectContainer.Resolve<IContactDetailsPage>() : null;
        private IWebElement btnSaveAndReview => _driver.FindElement(By.Id("save-and-review-button"));

        public ContactDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Contact details page should be displayed, pre-populated with the user's details")]
        public void ThenTheContactDetailsPageShouldBeDisplayedPre_PopulatedWithTheUsersDetails()
        {
            Assert.True(contactDetailsPage?.IsPageLoaded(), "Contact details page not loaded");
            var contactDetails = contactDetailsPage?.GetContactDetails();

            _scenarioContext["ContactName"] = contactDetails[0];
            _scenarioContext["ContactEmail"] = contactDetails[1];
            _scenarioContext["ContactTelephone"] = contactDetails[2];
        }

        [Then("the user verifies and enters any missing data on the Contact details page")]
        public void ThenTheUserVerifiesAndEntersAnyMissingDataOnTheContactDetailsPage()
        {
            Assert.True(contactDetailsPage?.IsPageLoaded(), "Contact details page not loaded");
            Assert.True(contactDetailsPage?.ValidateIfContactDetailsArePopulated(), "Contact details should be populated by default");
        }

        [When("the user Clicks on Save and review button from Contact details page")]
        public void WhenTheUserClicksOnSaveAndReviewButtonFromContactDetailsPage()
        {
            btnSaveAndReview.Click();
        }

    }
}