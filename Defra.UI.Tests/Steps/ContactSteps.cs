using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ContactSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IContactPage? contactPage => _objectContainer.IsRegistered<IContactPage>() ? _objectContainer.Resolve<IContactPage>() : null;

        public ContactSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Contacting us by phone or by email page is displayed")]
        public void ThenTheContactingUsByPhoneOrByEmailPageIsDisplayed()
        {
            Assert.True(contactPage?.IsPageLoaded(), "Contact page is not loaded");
        }

        [Then("the Contacting us by phone or by email page displays the text to contact APHA service desk")]
        public void ThenTheContactingUsByPhoneOrByEmailPageDisplaysTheTextToContactAPHAServiceDesk()
        {
            var aphaContactText = "We do not currently have a text relay service for people who are deaf, hearing impaired or have a speech impediment. You can contact APHA service desk on 03300 416 999 or email APHAServiceDesk@apha.gov.uk.";
            Assert.True(contactPage?.GetAphaContactText.Contains(aphaContactText), "Contact APHA service desk text is not displayed on the Contact page");
        }

        [When("the user clicks on the back link")]
        public void WhenTheUserClicksOnTheBackLink()
        {
            contactPage?.ClickBackLink();
        }
    }
}