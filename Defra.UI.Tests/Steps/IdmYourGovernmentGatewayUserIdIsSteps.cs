using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmYourGovernmentGatewayUserIdIsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmYourGovernmentGatewayUserIdIsPage idmYourGovernmentGatewayUserIdIsPage => _objectContainer.Resolve<IIdmYourGovernmentGatewayUserIdIsPage>();
        public IdmYourGovernmentGatewayUserIdIsSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Your Government Gateway user ID is: page is displayed")]
        public void ThenPageIsDisplayed()
        {
            Assert.That(idmYourGovernmentGatewayUserIdIsPage.IsPageLoaded(), Is.True);
            var gatewayId = idmYourGovernmentGatewayUserIdIsPage.GetGatewayId();
            Console.WriteLine($"[REGISTRATION] Government Gateway user ID: {gatewayId}");
            Assert.That(gatewayId, Does.Match(@"^\d{12}$"), "Expected a 12-digit Government Gateway ID");
            _scenarioContext.Set(gatewayId, "RegistrationGatewayId");
        }

        [When("the user continues from the Your Government Gateway user ID is: page")]
        public void WhenContinueIsClicked() => idmYourGovernmentGatewayUserIdIsPage.ClickContinueButton();
    }
}
