using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmWhatIsYourNameSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmWhatIsYourNamePage page => _objectContainer.Resolve<IIdmWhatIsYourNamePage>();
        public IdmWhatIsYourNameSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What is your name? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user enters their first and last names and continues")]
        public void WhenNameIsEntered()
        {
            page.EnterNameAndContinue(_scenarioContext.Get<string>("RegistrationFirstName"), _scenarioContext.Get<string>("RegistrationLastName"));
        }
    }
}
