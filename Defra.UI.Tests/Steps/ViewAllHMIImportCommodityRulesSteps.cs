using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ViewAllHMIImportCommodityRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IViewAllHMIImportCommodityRulesPage? viewAllHMIImportCommodityRulesPage =>
            _objectContainer.IsRegistered<IViewAllHMIImportCommodityRulesPage>()
                ? _objectContainer.Resolve<IViewAllHMIImportCommodityRulesPage>()
                : null;

        public ViewAllHMIImportCommodityRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the View all HMI \\(Import\\) Commodity Rules report page should be displayed")]
        public void ThenTheViewAllHMIImportCommodityRulesReportPageShouldBeDisplayed()
        {
            Assert.True(viewAllHMIImportCommodityRulesPage?.IsPageLoaded(), "View all HMI (Import) Commodity Rules report page is not displayed");
        }

        [When("the user scrolls to the bottom of the HMI rules report page")]
        public void WhenTheUserScrollsToTheBottomOfTheHMIRulesReportPage()
        {
            viewAllHMIImportCommodityRulesPage?.ScrollToBottom();
        }

        [Then("the HMI rule count is recorded as {string}")]
        public void ThenTheHMIRuleCountIsRecordedAs(string key)
        {
            var count = viewAllHMIImportCommodityRulesPage!.GetTotalRuleCount();
            _scenarioContext[key] = count;
            Assert.Greater(count, 0, $"Recorded HMI rule count for '{key}' is 0");
        }

        [Then("the HMI rule count should be {int} less than {string}")]
        public void ThenTheHMIRuleCountShouldBeLessThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllHMIImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial - delta, actual,
                $"Expected HMI rule count to be {initial - delta} (initial '{key}'={initial} - {delta}) but was {actual}");
        }

        [When("the user enters {string} in the HMI rules search field")]
        public void WhenTheUserEntersInTheHMIRulesSearchField(string text)
        {
            viewAllHMIImportCommodityRulesPage?.EnterSearchText(text);
        }

        [When("the user sorts the HMI rules table by Id descending")]
        public void WhenTheUserSortsTheHMIRulesTableByIdDescending()
        {
            viewAllHMIImportCommodityRulesPage?.SortByIdDescending();
        }

        [Then("the top HMI rule row should match the following details")]
        public void ThenTheTopHMIRuleRowShouldMatchTheFollowingDetails(Table table)
        {
            var actual = viewAllHMIImportCommodityRulesPage!.GetTopRowDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found in top HMI rule row");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the user records the Id of the top HMI rule row as {string}")]
        public void ThenTheUserRecordsTheIdOfTheTopHMIRuleRowAs(string key)
        {
            var id = viewAllHMIImportCommodityRulesPage!.GetTopRowId();
            Assert.IsNotEmpty(id, "Top HMI rule row Id is empty");
            _scenarioContext[key] = id;
        }

        [When("the user ticks the Select to Delete checkbox for HMI rule Id recorded as {string}")]
        public void WhenTheUserTicksTheSelectToDeleteCheckboxForHMIRuleIdRecordedAs(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            viewAllHMIImportCommodityRulesPage?.TickSelectToDeleteCheckboxForRuleId(ruleId);
        }

        [Then("the HMI selected rules info banner should display {string}")]
        public void ThenTheHMISelectedRulesInfoBannerShouldDisplay(string expectedText)
        {
            var actual = viewAllHMIImportCommodityRulesPage!.GetSelectedRulesInfoText();
            Assert.AreEqual(expectedText, actual,
                $"HMI selected rules info banner mismatch: expected '{expectedText}' but got '{actual}'");
        }

        [When("the user clicks the Delete Rules button on the HMI rules page")]
        public void WhenTheUserClicksTheDeleteRulesButtonOnTheHMIRulesPage()
        {
            viewAllHMIImportCommodityRulesPage?.ClickDeleteRulesButton();
        }

        [Then("the HMI Confirm rule deletion dialog should be displayed with {int} rules selected for deletion")]
        public void ThenTheHMIConfirmRuleDeletionDialogShouldBeDisplayedWithRulesSelectedForDeletion(int expectedCount)
        {
            Assert.True(viewAllHMIImportCommodityRulesPage?.IsConfirmDeletionDialogDisplayed(),
                "HMI Confirm rule deletion dialog is not displayed");
            var actual = viewAllHMIImportCommodityRulesPage!.GetConfirmDeletionDialogRuleCount();
            Assert.AreEqual(expectedCount, actual,
                $"Expected {expectedCount} rule(s) in HMI deletion dialog but found {actual}");
        }

        [When("the user clicks the Delete rules button on the HMI confirmation dialog")]
        public void WhenTheUserClicksTheDeleteRulesButtonOnTheHMIConfirmationDialog()
        {
            viewAllHMIImportCommodityRulesPage?.ClickConfirmDeleteButton();
        }

        [Then("the HMI Confirm rule deletion dialog should be closed")]
        public void ThenTheHMIConfirmRuleDeletionDialogShouldBeClosed()
        {
            Assert.True(viewAllHMIImportCommodityRulesPage?.IsConfirmDeletionDialogClosed(),
                "HMI Confirm rule deletion dialog is still displayed");
        }

        [Then("the HMI rule Id recorded as {string} should no longer be present in the rules table")]
        public void ThenTheHMIRuleIdRecordedAsShouldNoLongerBePresentInTheRulesTable(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            Assert.False(viewAllHMIImportCommodityRulesPage!.IsRuleIdPresent(ruleId),
                $"HMI Rule Id '{ruleId}' (from '{contextKey}') is still present in the rules table after deletion");
        }
    }
}