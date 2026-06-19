using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CheckAndSubmitCommodityRulesForCHEDDSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICheckAndSubmitCommodityRulesForCHEDDPage? checkAndSubmitCommodityRulesForCHEDDPage =>
            _objectContainer.IsRegistered<ICheckAndSubmitCommodityRulesForCHEDDPage>()
                ? _objectContainer.Resolve<ICheckAndSubmitCommodityRulesForCHEDDPage>()
                : null;

        public CheckAndSubmitCommodityRulesForCHEDDSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Check and submit commodity rules for CHED-D page is displayed with the following upload details")]
        public void ThenTheCheckAndSubmitCommodityRulesForCHEDDPageIsDisplayedWithTheFollowingUploadDetails(Table table)
        {
            Assert.True(checkAndSubmitCommodityRulesForCHEDDPage.IsPageLoaded(), "Check and submit commodity rules for CHED-D page is not displayed");
            var actual = checkAndSubmitCommodityRulesForCHEDDPage!.GetSummaryDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Check and submit page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the Check and submit commodity rules for CHED-D page should show Existing rules equal to {string}")]
        public void ThenTheCheckAndSubmitCommodityRulesForCHEDDPageShouldShowExistingRulesEqualTo(string key)
        {
            var expected = (int)_scenarioContext[key];
            var actual = checkAndSubmitCommodityRulesForCHEDDPage!.GetSummaryFieldAsInt("Existing rules");
            Assert.AreEqual(expected, actual,
                $"Expected Existing rules to equal '{key}'={expected} but was {actual}");
        }

        [When("the user clicks the Confirm and submit rules button on the CHED-D page")]
        public void WhenTheUserClicksTheConfirmAndSubmitRulesButtonOnTheCHEDDPage()
        {
            checkAndSubmitCommodityRulesForCHEDDPage.ClickConfirmAndSubmitRulesButton();
        }
    }
}