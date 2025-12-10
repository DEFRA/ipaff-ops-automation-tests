using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class DecisionSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IDecisionPage? decisionPage => _objectContainer.IsRegistered<IDecisionPage>() ? _objectContainer.Resolve<IDecisionPage>() : null;


        public DecisionSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Decision page should be displayed")]
        public void ThenTheDecisionPageShouldBeDisplayed()
        {
            Assert.True(decisionPage?.IsPageLoaded());
        }

        [Then("the user verifies the Transit radio button option is pre populated")]
        public void ThenTheUserVerifiesTheTransitRadioButtonOptionIsPrePopulated()
        {
            Assert.True(decisionPage?.VerifyTransitRadioButtonPrePopulated());
        }

        [Then("verifies exit BCP Transited country and Destination country are pre populated from part {int}")]
        public void ThenVerifiesExitBCPTransitedCountryAndDestinationCountryArePrePopulatedFromPart(int p0)
        {
            var exitBCP = _scenarioContext.Get<string>("ExitBorderControlPost");
            var transitedCountry = _scenarioContext.Get<string>("TransitedCountry");
            var destinationCountry = _scenarioContext.Get<string>("DestinationCountry");

            Assert.True(decisionPage?.VerifyPrepopulatedTransitDetails(exitBCP, transitedCountry, destinationCountry));
        }
    }
}