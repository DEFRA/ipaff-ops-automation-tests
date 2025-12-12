using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SignOutSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISignOutPage? signOutPage => _objectContainer.IsRegistered<ISignOutPage>() ? _objectContainer.Resolve<ISignOutPage>() : null;

        public SignOutSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [When("the user logs out of IPAFFS Part {int}")]
        public void WhenTheUserLogsOutOfIPAFFSPart(int partNumber)
        {
            signOutPage?.SignedOut();
        }

        [Then("the user should be logged out successfully")]
        public void ThenTheUserShouldBeLoggedOutSuccessfully()
        {
            Assert.True(signOutPage?.VerifySignedOutPage(), "Signed out page not loaded");
        }
    }
}