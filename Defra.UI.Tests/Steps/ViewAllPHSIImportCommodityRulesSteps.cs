using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ViewAllPHSIImportCommodityRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IViewAllPHSIImportCommodityRulesPage? viewAllPHSIImportCommodityRulesPage =>
            _objectContainer.IsRegistered<IViewAllPHSIImportCommodityRulesPage>()
                ? _objectContainer.Resolve<IViewAllPHSIImportCommodityRulesPage>()
                : null;

        public ViewAllPHSIImportCommodityRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the View all PHSI \\(Import\\) Commodity Rules report page should be displayed")]
        [Then("the View all PHSI Import Commodity Rules report page should be displayed")]
        public void ThenTheViewAllPHSIImportCommodityRulesReportPageShouldBeDisplayed()
        {
            Assert.True(viewAllPHSIImportCommodityRulesPage?.IsPageLoaded(), "View all PHSI (Import) Commodity Rules report page is not displayed");
        }

        [Then("the View all PHSI \\(Import\\) Commodity Rules report page should be displayed in a new browser tab")]
        [Then("the View all PHSI Import Commodity Rules report page should be displayed in a new browser tab")]
        public void ThenTheViewAllPHSIImportCommodityRulesReportPageShouldBeDisplayedInANewBrowserTab()
        {
            Assert.True(viewAllPHSIImportCommodityRulesPage?.SwitchToNewlyOpenedTab(), "No new browser tab found");
            Assert.True(viewAllPHSIImportCommodityRulesPage?.IsPageLoaded(), "View all PHSI (Import) Commodity Rules report page is not displayed in new tab");
        }

        [When("the user scrolls to the bottom of the PHSI rules report page")]
        public void WhenTheUserScrollsToTheBottomOfThePHSIRulesReportPage()
        {
            viewAllPHSIImportCommodityRulesPage?.ScrollToBottom();
        }

        [Then("the count of rules is recorded as {string}")]
        public void ThenTheCountOfRulesIsRecordedAs(string key)
        {
            var count = viewAllPHSIImportCommodityRulesPage!.GetTotalRuleCount();
            _scenarioContext[key] = count;
            Assert.Greater(count, 0, $"Recorded rule count for '{key}' is 0");
        }

        [Then("the count of rules should be {int} more than {string}")]
        public void ThenTheCountOfRulesShouldBeMoreThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllPHSIImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial + delta, actual,
                $"Expected rule count to be {initial + delta} (initial '{key}'={initial} + {delta}) but was {actual}");
        }

        [When("the user enters {string} in the PHSI rules search field")]
        public void WhenTheUserEntersInThePHSIRulesSearchField(string text)
        {
            viewAllPHSIImportCommodityRulesPage?.EnterSearchText(text);
        }

        [When("the user sorts the PHSI rules table by Id descending")]
        public void WhenTheUserSortsThePHSIRulesTableByIdDescending()
        {
            viewAllPHSIImportCommodityRulesPage?.SortByIdDescending();
        }

        [Then("the top PHSI rule row should match the following details")]
        public void ThenTheTopPHSIRuleRowShouldMatchTheFollowingDetails(Table table)
        {
            var actual = viewAllPHSIImportCommodityRulesPage!.GetTopRowDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found in top row");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the user records the Id of the top PHSI rule row as {string}")]
        public void ThenTheUserRecordsTheIdOfTheTopPHSIRuleRowAs(string key)
        {
            var id = viewAllPHSIImportCommodityRulesPage!.GetTopRowId();
            Assert.IsNotEmpty(id, "Top row Id is empty");
            _scenarioContext[key] = id;
        }
    }
}