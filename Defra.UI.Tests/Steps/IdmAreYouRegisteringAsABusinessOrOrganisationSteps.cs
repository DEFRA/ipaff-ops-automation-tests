using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmAreYouRegisteringAsABusinessOrOrganisationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmAreYouRegisteringAsABusinessOrOrganisationPage page => _objectContainer.Resolve<IIdmAreYouRegisteringAsABusinessOrOrganisationPage>();
        public IdmAreYouRegisteringAsABusinessOrOrganisationSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Are you registering as a business or organisation? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user selects {string} on the Are you registering as a business or organisation? page and continues")]
        public void WhenBusinessIsSelected(string option)
        {
            page.SelectOptionAndContinue(option);
            _scenarioContext.Set(option.Equals("Yes", StringComparison.OrdinalIgnoreCase) ? "Business" : "Individual", "RegistrationAccountType");
        }
    }
}
