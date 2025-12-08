using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
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
            Assert.True(decisionPage?.IsPageLoaded(), "Decision page is not displayed");
        }

        [When("the user selects Acceptable for {string} {string}")]
        public void WhenTheUserSelectsAcceptableFor(string acceptableFor, string subOption)
        {
            _scenarioContext.Add("AcceptableFor", acceptableFor);
            _scenarioContext.Add("AcceptableForSubOption", subOption);
            decisionPage?.SelectAcceptableFor(acceptableFor, subOption);
        }
    }
}