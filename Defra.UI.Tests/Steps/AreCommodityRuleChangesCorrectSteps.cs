using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AreCommodityRuleChangesCorrectSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAreCommodityRuleChangesCorrectPage? areCommodityRuleChangesCorrectPage =>
            _objectContainer.IsRegistered<IAreCommodityRuleChangesCorrectPage>()
                ? _objectContainer.Resolve<IAreCommodityRuleChangesCorrectPage>()
                : null;

        public AreCommodityRuleChangesCorrectSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the 'Are the commodity rule changes correct?' page is displayed with the following upload details")]
        public void ThenConfirmChangesPageShowsDetails(Table table)
        {
            Assert.True(areCommodityRuleChangesCorrectPage?.IsPageLoaded(), "Are the commodity rule changes correct? page is not displayed");
            var actual = areCommodityRuleChangesCorrectPage!.GetSummaryDetails();

            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Are the commodity rule changes correct? page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the 'Are the commodity rule changes correct?' page should show Existing rules equal to {string}")]
        public void ThenConfirmChangesPageShowsExistingRulesEqualTo(string contextKey)
        {
            var actual = areCommodityRuleChangesCorrectPage!.GetSummaryDetails();
            var expected = (int)_scenarioContext[contextKey];
            var existingRules = int.Parse(actual["Existing rules"]);
            Assert.AreEqual(expected, existingRules,
                $"'Existing rules' mismatch: expected '{expected}' ('{contextKey}') but got '{existingRules}'");
        }

        [Then("the 'Are the commodity rule changes correct?' page should show Total rules {int} more than {string}")]
        public void ThenConfirmChangesPageShowsTotalRulesMoreThan(int delta, string contextKey)
        {
            var actual = areCommodityRuleChangesCorrectPage!.GetSummaryDetails();
            var initial = (int)_scenarioContext[contextKey];
            var totalRules = int.Parse(actual["Total rules"]);
            Assert.AreEqual(initial + delta, totalRules,
                $"'Total rules' mismatch: expected '{initial + delta}' ('{contextKey}'={initial} + {delta}) but got '{totalRules}'");
        }

        [When("the user selects the {string} radio option")]
        public void WhenTheUserSelectsTheRadioOption(string optionLabel)
        {
            areCommodityRuleChangesCorrectPage?.SelectConfirmChangesOption(optionLabel);
        }

        [When("the user clicks the Submit button on the rule changes page")]
        public void WhenTheUserClicksTheSubmitButtonOnTheRuleChangesPage()
        {
            areCommodityRuleChangesCorrectPage?.ClickSubmitButton();
        }
    }
}