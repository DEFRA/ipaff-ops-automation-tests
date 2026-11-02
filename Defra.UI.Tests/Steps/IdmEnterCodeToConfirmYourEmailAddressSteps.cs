using Defra.UI.Tests.HelperMethods;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmEnterCodeToConfirmYourEmailAddressSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private string _confirmationCode = "";
        private IIdmEnterCodeToConfirmYourEmailAddressPage page => _objectContainer.Resolve<IIdmEnterCodeToConfirmYourEmailAddressPage>();

        public IdmEnterCodeToConfirmYourEmailAddressSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Enter code to confirm your email address page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [Then("a confirmation email is received for the new account")]
        public async Task ThenConfirmationEmailIsReceived()
        {
            var inbox = _scenarioContext.Get<string>("RegistrationInbox");
            _confirmationCode = await _objectContainer.Resolve<IFetchCodeFromEmail>().GetCodeFromEmail(inbox);
            Assert.That(_confirmationCode, Is.Not.Empty, $"No email confirmation code received for {inbox}");
        }

        [When("the user confirms the email address using the emailed code")]
        public void WhenEmailIsConfirmed()
        {
            Assert.That(_confirmationCode, Is.Not.Empty, "The confirmation email must be received first");
            page.EnterCodeAndConfirm(_confirmationCode);
        }
    }
}
