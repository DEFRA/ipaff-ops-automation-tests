using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmWhatIsYourFullNameSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmWhatIsYourFullNamePage idmWhatIsYourFullNamePage => _objectContainer.Resolve<IIdmWhatIsYourFullNamePage>();
        public IdmWhatIsYourFullNameSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What is your full name? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmWhatIsYourFullNamePage.IsPageLoaded(), Is.True);

        [When("the user enters their full name for the account and continues")]
        public void WhenNameIsEntered()
        {
            var firstName = Utils.GenerateFirstName();
            var lastName = Utils.GenerateLastName();
            _scenarioContext.Set(firstName, "RegistrationFirstName");
            _scenarioContext.Set(lastName, "RegistrationLastName");
            idmWhatIsYourFullNamePage.EnterNameAndContinue($"{firstName} {lastName}");
        }
    }
}
