using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmWhatIsYourPhoneNumberSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmWhatIsYourPhoneNumberPage idmWhatIsYourPhoneNumberPage => _objectContainer.Resolve<IIdmWhatIsYourPhoneNumberPage>();
        public IdmWhatIsYourPhoneNumberSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What is your phone number? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmWhatIsYourPhoneNumberPage.IsPageLoaded(), Is.True);

        [When("the user enters their phone number and continues")]
        public void WhenPhoneIsEntered() => idmWhatIsYourPhoneNumberPage.EnterPhoneAndContinue(_scenarioContext.Get<string>("RegistrationPhone"));
    }
}
