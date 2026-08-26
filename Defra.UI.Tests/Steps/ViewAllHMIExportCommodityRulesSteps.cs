using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ViewAllHMIExportCommodityRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IViewAllHMIExportCommodityRulesPage? viewAllHMIExportCommodityRulesPage =>
            _objectContainer.IsRegistered<IViewAllHMIExportCommodityRulesPage>()
                ? _objectContainer.Resolve<IViewAllHMIExportCommodityRulesPage>()
                : null;

        public ViewAllHMIExportCommodityRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the View all HMI \\(Export\\) Commodity Rules report page should be displayed")]
        public void ThenTheViewAllHMIExportCommodityRulesReportPageShouldBeDisplayed()
        {
            Assert.True(viewAllHMIExportCommodityRulesPage?.IsPageLoaded(), "View all HMI (Export) Commodity Rules report page is not displayed");
        }

        [When("the user scrolls to the bottom of the HMI \\(Export\\) Commodity Rules report page")]
        public void WhenTheUserScrollsToTheBottomOfTheHMIExportCommodityRulesReportPage()
        {
            viewAllHMIExportCommodityRulesPage?.ScrollToBottom();
        }

        [Then("the HMI export commodity rule count is recorded as {string}")]
        public void ThenTheHMIExportCommodityRuleCountIsRecordedAs(string key)
        {
            var count = viewAllHMIExportCommodityRulesPage!.GetTotalRuleCount();
            _scenarioContext[key] = count;
            Assert.Greater(count, 0, $"Recorded HMI export rule count for '{key}' is 0");
        }

        [Then("the HMI export commodity rule count should be {int} less than {string}")]
        public void ThenTheHMIExportCommodityRuleCountShouldBeLessThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllHMIExportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial - delta, actual,
                $"Expected HMI export rule count to be {initial - delta} (initial '{key}'={initial} - {delta}) but was {actual}");
        }

        [When("the user enters {string} in the HMI export commodity rules search field")]
        public void WhenTheUserEntersInTheHMIExportCommodityRulesSearchField(string text)
        {
            viewAllHMIExportCommodityRulesPage?.EnterSearchText(text);
        }

        [When("the user sorts the HMI export commodity rules table by Id descending")]
        public void WhenTheUserSortsTheHMIExportCommodityRulesTableByIdDescending()
        {
            viewAllHMIExportCommodityRulesPage?.SortByIdDescending();
        }

        [Then("the top HMI export commodity rule row should match the following details")]
        public void ThenTheTopHMIExportCommodityRuleRowShouldMatchTheFollowingDetails(Table table)
        {
            var actual = viewAllHMIExportCommodityRulesPage!.GetTopRowDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found in top HMI export rule row");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the user records the Id of the top HMI export commodity rule row as {string}")]
        public void ThenTheUserRecordsTheIdOfTheTopHMIExportCommodityRuleRowAs(string key)
        {
            var id = viewAllHMIExportCommodityRulesPage!.GetTopRowId();
            Assert.IsNotEmpty(id, "Top HMI export rule row Id is empty");
            _scenarioContext[key] = id;
        }

        [When("the user ticks the Select to Delete checkbox for HMI export commodity rule Id recorded as {string}")]
        public void WhenTheUserTicksTheSelectToDeleteCheckboxForHMIExportCommodityRuleIdRecordedAs(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            viewAllHMIExportCommodityRulesPage?.TickSelectToDeleteCheckboxForRuleId(ruleId);
        }

        [Then("the info banner should display {string} on the View all HMI \\(Export\\) Commodity Rules page")]
        public void ThenTheInfoBannerShouldDisplayOnTheViewAllHMIExportCommodityRulesPage(string expectedText)
        {
            var actual = viewAllHMIExportCommodityRulesPage!.GetSelectedRulesInfoText();
            Assert.AreEqual(expectedText, actual,
                $"HMI export selected rules info banner mismatch: expected '{expectedText}' but got '{actual}'");
        }

        [When("the user clicks the Delete Rules button on the View all HMI \\(Export\\) Commodity Rules page")]
        public void WhenTheUserClicksTheDeleteRulesButtonOnTheViewAllHMIExportCommodityRulesPage()
        {
            viewAllHMIExportCommodityRulesPage?.ClickDeleteRulesButton();
        }

        [Then("the Confirm rule deletion dialog should be displayed with {int} rules selected for deletion on the View all HMI \\(Export\\) Commodity Rules page")]
        public void ThenTheConfirmRuleDeletionDialogShouldBeDisplayedWithRulesSelectedForDeletionOnTheViewAllHMIExportCommodityRulesPage(int expectedCount)
        {
            Assert.True(viewAllHMIExportCommodityRulesPage?.IsConfirmDeletionDialogDisplayed(),
                "Confirm rule deletion dialog is not displayed");
            var actual = viewAllHMIExportCommodityRulesPage!.GetConfirmDeletionDialogRuleCount();
            Assert.AreEqual(expectedCount, actual,
                $"Expected {expectedCount} rule(s) in deletion dialog but found {actual}");
        }

        [When("the user clicks the Delete rules button on the confirmation dialog on the View all HMI \\(Export\\) Commodity Rules page")]
        public void WhenTheUserClicksTheDeleteRulesButtonOnTheConfirmationDialogOnTheViewAllHMIExportCommodityRulesPage()
        {
            viewAllHMIExportCommodityRulesPage?.ClickConfirmDeleteButton();
        }

        [Then("the Confirm rule deletion dialog should be closed on the View all HMI \\(Export\\) Commodity Rules page")]
        public void ThenTheConfirmRuleDeletionDialogShouldBeClosedOnTheViewAllHMIExportCommodityRulesPage()
        {
            Assert.True(viewAllHMIExportCommodityRulesPage?.IsConfirmDeletionDialogClosed(),
                "Confirm rule deletion dialog is still displayed");
        }

        [Then("the HMI export commodity rule Id recorded as {string} should no longer be present in the rules table")]
        public void ThenTheHMIExportCommodityRuleIdRecordedAsShouldNoLongerBePresentInTheRulesTable(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            Assert.False(viewAllHMIExportCommodityRulesPage!.IsRuleIdPresent(ruleId),
                $"Rule Id '{ruleId}' (from '{contextKey}') is still present in the rules table after deletion");
        }
    }
}