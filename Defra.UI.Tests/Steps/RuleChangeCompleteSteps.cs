using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class RuleChangeCompleteSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IRuleChangeCompletePage? ruleChangeCompletePage =>
            _objectContainer.IsRegistered<IRuleChangeCompletePage>()
                ? _objectContainer.Resolve<IRuleChangeCompletePage>()
                : null;

        public RuleChangeCompleteSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Rule change complete page should be displayed")]
        public void ThenTheRuleChangeCompletePageShouldBeDisplayed()
        {
            Assert.True(ruleChangeCompletePage?.IsPageLoaded(), "Rule change complete page is not displayed");
        }
    }
}