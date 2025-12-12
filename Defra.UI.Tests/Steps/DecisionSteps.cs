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
            Assert.True(decisionPage?.IsPageLoaded());
        }

        [Then("the main radio option {string} and the sub radio option {string} are selected by default")]
        public void ThenTheMainRadioOptionAndTheSubRadioOptionAreSelectedByDefault(string mainRadio, string subRadio)
        {
            Assert.Multiple(() =>
            {
                Assert.True(decisionPage?.IsAcceptableForRadioSelected(mainRadio), $"The main radio option {mainRadio} is not selected by default on the Decision page");
                Assert.True(decisionPage?.IsInternalMarketSubRadioSelected(subRadio), $"The sub radio option {subRadio} is not selected by default on the Decision page");
            }
            );
        }

    }
}