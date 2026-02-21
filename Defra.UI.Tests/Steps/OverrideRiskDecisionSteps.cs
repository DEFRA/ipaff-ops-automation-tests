using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class OverrideRiskDecisionSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IOverrideRiskDecisionPage? overrideRiskDecisionPage => _objectContainer.IsRegistered<IOverrideRiskDecisionPage>() ? _objectContainer.Resolve<IOverrideRiskDecisionPage>() : null;

        public OverrideRiskDecisionSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Override risk decision page should be displayed")]
        public void ThenTheOverrideRiskDecisionPageShouldBeDisplayed()
        {
            Assert.True(overrideRiskDecisionPage?.IsPageLoaded(), "Override risk decision page not loaded");
        }

        [When("the user clicks Yes, override risk decision button")]
        public void WhenTheUserClicksYesOverrideRiskDecisionButton()
        {
            overrideRiskDecisionPage?.ClickYesOverrideRiskDecisionButton();
        }

        [Then("the user selects {string} option for override decision")]
        public void ThenTheUserSelectsOption(string option)
        {
            overrideRiskDecisionPage?.ClickOverrideDecisionOption(option);
        }
    }
}