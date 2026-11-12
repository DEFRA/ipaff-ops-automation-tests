using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmWhatKindOfBusinessOrOrganisationAreYouSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmWhatKindOfBusinessOrOrganisationAreYouPage idmWhatKindOfBusinessOrOrganisationAreYouPage => _objectContainer.Resolve<IIdmWhatKindOfBusinessOrOrganisationAreYouPage>();
        public IdmWhatKindOfBusinessOrOrganisationAreYouSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What kind of business or organisation are you? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmWhatKindOfBusinessOrOrganisationAreYouPage.IsPageLoaded(), Is.True);

        [When("the user selects {string} on the What kind of business or organisation are you? page and continues")]
        public void WhenOrganisationTypeIsSelected(string option)
        {
            idmWhatKindOfBusinessOrOrganisationAreYouPage.SelectOptionAndContinue(option);
            _scenarioContext.Set(option, "RegistrationOrganisationType");
        }
    }
}
