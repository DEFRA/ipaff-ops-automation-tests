using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ViewAllCHEDPImportCommodityRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IViewAllCHEDPImportCommodityRulesPage? viewAllCHEDPImportCommodityRulesPage =>
            _objectContainer.IsRegistered<IViewAllCHEDPImportCommodityRulesPage>()
                ? _objectContainer.Resolve<IViewAllCHEDPImportCommodityRulesPage>()
                : null;

        public ViewAllCHEDPImportCommodityRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the View all CHED-P \\(Import\\) Commodity Rules report page should be displayed")]
        public void ThenTheViewAllCHEDPImportCommodityRulesReportPageShouldBeDisplayed()
        {
            Assert.True(viewAllCHEDPImportCommodityRulesPage?.IsPageLoaded(), "View all CHED-P (Import) Commodity Rules report page is not displayed");
        }

        [Then("the View all CHED-P \\(Import\\) Commodity Rules report page should be displayed in a new browser tab")]
        public void ThenTheViewAllCHEDPImportCommodityRulesReportPageShouldBeDisplayedInANewBrowserTab()
        {
            Assert.True(viewAllCHEDPImportCommodityRulesPage?.SwitchToNewlyOpenedTab(), "No new browser tab found");
            Assert.True(viewAllCHEDPImportCommodityRulesPage?.IsPageLoaded(), "View all CHED-P (Import) Commodity Rules report page is not displayed in new tab");
        }

        [When("the user scrolls to the bottom of the CHED-P rules report page")]
        public void WhenTheUserScrollsToTheBottomOfTheCHEDPRulesReportPage()
        {
            viewAllCHEDPImportCommodityRulesPage?.ScrollToBottom();
        }

        [Then("the count of CHED-P rules is recorded as {string}")]
        public void ThenTheCountOfCHEDPRulesIsRecordedAs(string key)
        {
            var count = viewAllCHEDPImportCommodityRulesPage!.GetTotalRuleCount();
            _scenarioContext[key] = count;
            Assert.Greater(count, 0, $"Recorded rule count for '{key}' is 0");
        }

        [Then("the count of CHED-P rules should be {int} more than {string}")]
        public void ThenTheCountOfCHEDPRulesShouldBeMoreThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllCHEDPImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial + delta, actual,
                $"Expected rule count to be {initial + delta} (initial '{key}'={initial} + {delta}) but was {actual}");
        }

        [Then("the count of CHED-P rules should be {int} less than {string}")]
        public void ThenTheCountOfCHEDPRulesShouldBeLessThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllCHEDPImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial - delta, actual,
                $"Expected rule count to be {initial - delta} (initial '{key}'={initial} - {delta}) but was {actual}");
        }

        [Then("the count of CHED-P rules should equal the recorded {string}")]
        public void ThenTheCountOfCHEDPRulesShouldEqualTheRecorded(string key)
        {
            var expected = (int)_scenarioContext[key];
            var actual = viewAllCHEDPImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(expected, actual,
                $"Expected rule count to equal '{key}'={expected} but was {actual}");
        }

        [When("the user enters {string} in the CHED-P rules search field")]
        public void WhenTheUserEntersInTheCHEDPRulesSearchField(string text)
        {
            viewAllCHEDPImportCommodityRulesPage?.EnterSearchText(text);
        }

        [Then("the top CHED-P rule row should match the following details")]
        public void ThenTheTopCHEDPRuleRowShouldMatchTheFollowingDetails(Table table)
        {
            var actual = viewAllCHEDPImportCommodityRulesPage!.GetTopRowDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found in top row");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the top CHED-P rule row should have Start date as today's date")]
        public void ThenTheTopCHEDPRuleRowShouldHaveStartDateAsTodaysDate()
        {
            var actual = viewAllCHEDPImportCommodityRulesPage!.GetTopRowDetails();
            Assert.True(actual.ContainsKey("Start date"), "Field 'Start date' not found in top row");
            var expected = DateTime.Now.ToString("dd/MM/yyyy");
            Assert.AreEqual(expected, actual["Start date"],
                $"Field 'Start date' mismatch: expected '{expected}' but got '{actual["Start date"]}'");
        }

        [Then("the user records the Id of the top CHED-P rule row as {string}")]
        public void ThenTheUserRecordsTheIdOfTheTopCHEDPRuleRowAs(string key)
        {
            var id = viewAllCHEDPImportCommodityRulesPage!.GetTopRowId();
            Assert.IsNotEmpty(id, "Top row Id is empty");
            _scenarioContext[key] = id;
        }

        [When("the user clicks the Remove rule link for CHED-P rule Id recorded as {string}")]
        public void WhenTheUserClicksTheRemoveRuleLinkForCHEDPRuleIdRecordedAs(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            viewAllCHEDPImportCommodityRulesPage?.ClickRemoveRuleLinkForRuleId(ruleId);
        }

        [Then("the CHED-P rule Id recorded as {string} should no longer be present in the rules table")]
        public void ThenTheCHEDPRuleIdRecordedAsShouldNoLongerBePresentInTheRulesTable(string contextKey)
        {
            Assert.True(viewAllCHEDPImportCommodityRulesPage?.IsPageLoaded(),
                "CHED-P rules page has not fully loaded after removal");
            var ruleId = _scenarioContext.Get<string>(contextKey);
            Assert.False(viewAllCHEDPImportCommodityRulesPage!.IsRuleIdPresent(ruleId),
                $"Rule Id '{ruleId}' (from '{contextKey}') is still present in the rules table after removal");
        }
    }
}