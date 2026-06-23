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

        [When("the user scrolls to the bottom of the View all CHED-D \\(Import) Commodity Rules report page")]
        public void WhenTheUserScrollsToTheBottomOfTheViewAllCHED_DImportCommodityRulesReportPage()
        {
            viewAllCHEDDImportCommodityRulesPage.ScrollToBottom();
        }

        [Then("the count of CHED-D import commodity rules is recorded as {string}")]
        public void ThenTheCountOfCHED_DImportCommodityRulesIsRecordedAs(string key)
        {
            var count = viewAllCHEDDImportCommodityRulesPage!.GetTotalRuleCount();
            _scenarioContext[key] = count;
            Assert.Greater(count, 0, $"Recorded rule count for '{key}' is 0");
        }

        [Then("the count of CHED-D import commodity rules should be {int} more than {string}")]
        public void ThenTheCountOfCHED_DImportCommodityRulesShouldBeMoreThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllCHEDDImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial + delta, actual,
                $"Expected rule count to be {initial + delta} (initial '{key}'={initial} + {delta}) but was {actual}");
        }

        [Then("the count of CHED-D import commodity rules should be {int} less than {string}")]
        public void ThenTheCountOfCHED_DImportCommodityRulesShouldBeLessThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllCHEDDImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial - delta, actual,
                $"Expected rule count to be {initial - delta} (initial '{key}'={initial} - {delta}) but was {actual}");
        }

        [Then("the count of CHED-D import commodity rules should equal the recorded {string}")]
        public void ThenTheCountOfCHED_DImportCommodityRulesShouldEqualTheRecorded(string key)
        {
            var expected = (int)_scenarioContext[key];
            var actual = viewAllCHEDDImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(expected, actual,
                $"Expected rule count to equal '{key}'={expected} but was {actual}");
        }

        [When("the user enters {string} in the CHED-D import commodity rules search field")]
        public void WhenTheUserEntersInTheCHED_DImportCommodityRulesSearchField(string text)
        {
            viewAllCHEDDImportCommodityRulesPage.EnterSearchText(text);
        }

        [Then("the top CHED-D import commodity rule row should match the following details")]
        public void ThenTheTopCHED_DImportCommodityRuleRowShouldMatchTheFollowingDetails(Table table)
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

        [Then("the top CHED-D import commodity rule row should have Start date as today's date")]
        public void ThenTheTopCHED_DImportCommodityRuleRowShouldHaveStartDateAsTodaysDate()
        {
            var actual = viewAllCHEDDImportCommodityRulesPage!.GetTopRowDetails();
            Assert.True(actual.ContainsKey("Start date"), "Field 'Start date' not found in top row");
            var expected = DateTime.Now.ToString("dd/MM/yyyy");
            Assert.AreEqual(expected, actual["Start date"],
                $"Field 'Start date' mismatch: expected '{expected}' but got '{actual["Start date"]}'");
        }

        [Then("the user records the Id of the top CHED-D import commodity rule row as {string}")]
        public void ThenTheUserRecordsTheIdOfTheTopCHED_DImportCommodityRuleRowAs(string key)
        {
            var id = viewAllCHEDDImportCommodityRulesPage!.GetTopRowId();
            Assert.IsNotEmpty(id, "Top row Id is empty");
            _scenarioContext[key] = id;
        }

        [When("the user clicks the Remove rule link for CHED-D import commodity rule Id recorded as {string}")]
        public void WhenTheUserClicksTheRemoveRuleLinkForCHED_DImportCommodityRuleIdRecordedAs(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            viewAllCHEDDImportCommodityRulesPage?.ClickRemoveRuleLinkForRuleId(ruleId);
        }

        [Then("the CHED-D import commodity rule Id recorded as {string} should no longer be present in the rules table")]
        public void ThenTheCHED_DImportCommodityRuleIdRecordedAsShouldNoLongerBePresentInTheRulesTable(string contextKey)
        {
            Assert.True(viewAllCHEDDImportCommodityRulesPage?.IsPageLoaded(),
                "CHED-D rules page has not fully loaded after removal");
            var ruleId = _scenarioContext.Get<string>(contextKey);
            Assert.False(viewAllCHEDDImportCommodityRulesPage!.IsRuleIdPresent(ruleId),
                $"Rule Id '{ruleId}' (from '{contextKey}') is still present in the rules table after removal");
        }
    }
}