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

        [Then("the count of rules should be {int} less than {string}")]
        public void ThenTheCountOfRulesShouldBeLessThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllPHSIImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial - delta, actual,
                $"Expected rule count to be {initial - delta} (initial '{key}'={initial} - {delta}) but was {actual}");
        }

        [Then("the count of rules should equal the recorded {string}")]
        public void ThenTheCountOfRulesShouldEqualTheRecorded(string key)
        {
            var expected = (int)_scenarioContext[key];
            var actual = viewAllPHSIImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(expected, actual,
                $"Expected rule count to equal '{key}'={expected} but was {actual}");
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

        [When("the user ticks the Select to Delete checkbox for rule Id recorded as {string}")]
        public void WhenTheUserTicksTheSelectToDeleteCheckboxForRuleIdRecordedAs(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            viewAllPHSIImportCommodityRulesPage?.TickSelectToDeleteCheckboxForRuleId(ruleId);
        }

        [Then("the selected rules info banner should display {string}")]
        public void ThenTheSelectedRulesInfoBannerShouldDisplay(string expectedText)
        {
            var actual = viewAllPHSIImportCommodityRulesPage!.GetSelectedRulesInfoText();
            Assert.AreEqual(expectedText, actual,
                $"Selected rules info banner mismatch: expected '{expectedText}' but got '{actual}'");
        }

        [When("the user clicks the Delete Rules button")]
        public void WhenTheUserClicksTheDeleteRulesButton()
        {
            viewAllPHSIImportCommodityRulesPage?.ClickDeleteRulesButton();
        }

        [Then("the Confirm rule deletion dialog should be displayed with {int} rules selected for deletion")]
        public void ThenTheConfirmRuleDeletionDialogShouldBeDisplayedWithRulesSelectedForDeletion(int expectedCount)
        {
            Assert.True(viewAllPHSIImportCommodityRulesPage?.IsConfirmDeletionDialogDisplayed(),
                "Confirm rule deletion dialog is not displayed");
            var actual = viewAllPHSIImportCommodityRulesPage!.GetConfirmDeletionDialogRuleCount();
            Assert.AreEqual(expectedCount, actual,
                $"Expected {expectedCount} rule(s) in deletion dialog but found {actual}");
        }

        [When("the user clicks the Delete rules button on the confirmation dialog")]
        public void WhenTheUserClicksTheDeleteRulesButtonOnTheConfirmationDialog()
        {
            viewAllPHSIImportCommodityRulesPage?.ClickConfirmDeleteButton();
        }

        [Then("the Confirm rule deletion dialog should be closed")]
        public void ThenTheConfirmRuleDeletionDialogShouldBeClosed()
        {
            Assert.True(viewAllPHSIImportCommodityRulesPage?.IsConfirmDeletionDialogClosed(),
                "Confirm rule deletion dialog is still displayed");
        }

        [Then("the rule Id recorded as {string} should no longer be present in the rules table")]
        public void ThenTheRuleIdRecordedAsShouldNoLongerBePresentInTheRulesTable(string contextKey)
        {
            Assert.True(viewAllPHSIImportCommodityRulesPage?.IsPageLoaded(),
                "PHSI rules page has not fully loaded after deletion");
            var ruleId = _scenarioContext.Get<string>(contextKey);
            Assert.False(viewAllPHSIImportCommodityRulesPage!.IsRuleIdPresent(ruleId),
                $"Rule Id '{ruleId}' (from '{contextKey}') is still present in the rules table after deletion");
        }

        [Then("the PHSI rules search field should be empty")]
        public void ThenThePHSIRulesSearchFieldShouldBeEmpty()
        {
            var text = viewAllPHSIImportCommodityRulesPage!.GetSearchInputText();
            Assert.IsEmpty(text, $"Expected search field to be empty but found '{text}'");
        }

        [Then("the PHSI rules table Id column should have no sort applied")]
        public void ThenThePHSIRulesTableIdColumnShouldHaveNoSortApplied()
        {
            Assert.False(viewAllPHSIImportCommodityRulesPage!.IsIdColumnSorted(),
                "Id column still has a sort applied (aria-sort is present) after deletion");
        }
    }
}