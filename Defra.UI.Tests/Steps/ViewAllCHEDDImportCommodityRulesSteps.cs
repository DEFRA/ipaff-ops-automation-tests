using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ViewAllCHEDDImportCommodityRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IViewAllCHEDDImportCommodityRulesPage? viewAllCHEDDImportCommodityRulesPage =>
            _objectContainer.IsRegistered<IViewAllCHEDDImportCommodityRulesPage>()
                ? _objectContainer.Resolve<IViewAllCHEDDImportCommodityRulesPage>()
                : null;

        public ViewAllCHEDDImportCommodityRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the View all CHED-D \\(Import\\) Commodity Rules report page should be displayed")]
        [Then("the View all CHED-D Import Commodity Rules report page should be displayed")]
        public void ThenTheViewAllCHEDDImportCommodityRulesReportPageShouldBeDisplayed()
        {
            Assert.True(viewAllCHEDDImportCommodityRulesPage.IsPageLoaded(), "View all CHED-D (Import) Commodity Rules report page is not displayed");
        }

        [Then("the View all CHED-D \\(Import\\) Commodity Rules report page should be displayed in a new browser tab")]
        public void ThenTheViewAllCHEDDImportCommodityRulesReportPageShouldBeDisplayedInANewBrowserTab()
        {
            Assert.True(viewAllCHEDDImportCommodityRulesPage.SwitchToNewlyOpenedTab(), "No new browser tab found");
            Assert.True(viewAllCHEDDImportCommodityRulesPage.IsPageLoaded(), "View all CHED-D (Import) Commodity Rules report page is not displayed in new tab");
        }

        [When("the user scrolls to the bottom of the CHED-D rules report page")]
        public void WhenTheUserScrollsToTheBottomOfTheCHEDDRulesReportPage()
        {
            viewAllCHEDDImportCommodityRulesPage.ScrollToBottom();
        }

        [Then("the count of CHED-D rules is recorded as {string}")]
        public void ThenTheCountOfCHEDDRulesIsRecordedAs(string key)
        {
            var count = viewAllCHEDDImportCommodityRulesPage!.GetTotalRuleCount();
            _scenarioContext[key] = count;
            Assert.Greater(count, 0, $"Recorded rule count for '{key}' is 0");
        }

        [Then("the count of CHED-D rules should be {int} more than {string}")]
        public void ThenTheCountOfCHEDDRulesShouldBeMoreThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllCHEDDImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial + delta, actual,
                $"Expected rule count to be {initial + delta} (initial '{key}'={initial} + {delta}) but was {actual}");
        }

        [Then("the count of CHED-D rules should be {int} less than {string}")]
        public void ThenTheCountOfCHEDDRulesShouldBeLessThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllCHEDDImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial - delta, actual,
                $"Expected rule count to be {initial - delta} (initial '{key}'={initial} - {delta}) but was {actual}");
        }

        [Then("the count of CHED-D rules should equal the recorded {string}")]
        public void ThenTheCountOfCHEDDRulesShouldEqualTheRecorded(string key)
        {
            var expected = (int)_scenarioContext[key];
            var actual = viewAllCHEDDImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(expected, actual,
                $"Expected rule count to equal '{key}'={expected} but was {actual}");
        }

        [When("the user enters {string} in the CHED-D rules search field")]
        public void WhenTheUserEntersInTheCHEDDRulesSearchField(string text)
        {
            viewAllCHEDDImportCommodityRulesPage.EnterSearchText(text);
        }

        [Then("the top CHED-D rule row should match the following details")]
        public void ThenTheTopCHEDDRuleRowShouldMatchTheFollowingDetails(Table table)
        {
            var actual = viewAllCHEDDImportCommodityRulesPage!.GetTopRowDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found in top row");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the top CHED-D rule row should have Start date as today's date")]
        public void ThenTheTopCHEDDRuleRowShouldHaveStartDateAsTodaysDate()
        {
            var actual = viewAllCHEDDImportCommodityRulesPage!.GetTopRowDetails();
            Assert.True(actual.ContainsKey("Start date"), "Field 'Start date' not found in top row");
            var expected = DateTime.Now.ToString("dd/MM/yyyy");
            Assert.AreEqual(expected, actual["Start date"],
                $"Field 'Start date' mismatch: expected '{expected}' but got '{actual["Start date"]}'");
        }

        [Then("the user records the Id of the top CHED-D rule row as {string}")]
        public void ThenTheUserRecordsTheIdOfTheTopCHEDDRuleRowAs(string key)
        {
            var id = viewAllCHEDDImportCommodityRulesPage!.GetTopRowId();
            Assert.IsNotEmpty(id, "Top row Id is empty");
            _scenarioContext[key] = id;
        }

        [When("the user clicks the Remove rule link for CHED-D rule Id recorded as {string}")]
        public void WhenTheUserClicksTheRemoveRuleLinkForCHEDDRuleIdRecordedAs(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            viewAllCHEDDImportCommodityRulesPage?.ClickRemoveRuleLinkForRuleId(ruleId);
        }

        [Then("the CHED-D rule Id recorded as {string} should no longer be present in the rules table")]
        public void ThenTheCHEDDRuleIdRecordedAsShouldNoLongerBePresentInTheRulesTable(string contextKey)
        {
            Assert.True(viewAllCHEDDImportCommodityRulesPage?.IsPageLoaded(),
                "CHED-D rules page has not fully loaded after removal");
            var ruleId = _scenarioContext.Get<string>(contextKey);
            Assert.False(viewAllCHEDDImportCommodityRulesPage!.IsRuleIdPresent(ruleId),
                $"Rule Id '{ruleId}' (from '{contextKey}') is still present in the rules table after removal");
        }
    }
}