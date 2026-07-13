using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CheckAndSubmitCommodityRuleSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICheckAndSubmitCommodityRulePage? checkAndSubmitCommodityRulePage =>
            _objectContainer.IsRegistered<ICheckAndSubmitCommodityRulePage>()
                ? _objectContainer.Resolve<ICheckAndSubmitCommodityRulePage>()
                : null;

        public CheckAndSubmitCommodityRuleSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Check and submit commodity rule page should be displayed")]
        public void ThenTheCheckAndSubmitCommodityRulePageShouldBeDisplayed()
        {
            Assert.True(checkAndSubmitCommodityRulePage?.IsPageLoaded(), "Check and submit commodity rule page is not displayed");
        }

        [When("the user clicks the Confirm and submit rule button on the Check and submit commodity rule page")]
        public void WhenTheUserClicksTheConfirmAndSubmitRuleButtonOnTheCheckAndSubmitCommodityRulePage()
        {
            checkAndSubmitCommodityRulePage?.ClickConfirmAndSubmitRuleButton();
        }
    }
}