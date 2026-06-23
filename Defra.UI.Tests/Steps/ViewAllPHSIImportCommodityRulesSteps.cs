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

        [When("the user scrolls to the bottom of the View all PHSI \\(Import) Commodity Rules report page")]
        public void WhenTheUserScrollsToTheBottomOfTheViewAllPHSIImportCommodityRulesReportPage()
        {
            viewAllPHSIImportCommodityRulesPage?.ScrollToBottom();
        }

        [Then("the count of PHSI import commodity rules is recorded as {string}")]
        public void ThenTheCountOfPHSIImportCommodityRulesIsRecordedAs(string key)
        {
            var count = viewAllPHSIImportCommodityRulesPage!.GetTotalRuleCount();
            _scenarioContext[key] = count;
            Assert.Greater(count, 0, $"Recorded rule count for '{key}' is 0");
        }

        [Then("the count of PHSI import commodity rules should be {int} more than {string}")]
        public void ThenTheCountOfPHSIImportCommodityRulesShouldBeMoreThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllPHSIImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial + delta, actual,
                $"Expected rule count to be {initial + delta} (initial '{key}'={initial} + {delta}) but was {actual}");
        }

        [Then("the count of PHSI import commodity rules should be {int} less than {string}")]
        public void ThenTheCountOfPHSIImportCommodityRulesShouldBeLessThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = viewAllPHSIImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial - delta, actual,
                $"Expected rule count to be {initial - delta} (initial '{key}'={initial} - {delta}) but was {actual}");
        }

        [Then("the count of PHSI import commodity rules should equal the recorded {string}")]
        public void ThenTheCountOfPHSIImportCommodityRulesShouldEqualTheRecorded(string key)
        {
            var expected = (int)_scenarioContext[key];
            var actual = viewAllPHSIImportCommodityRulesPage!.GetTotalRuleCount();
            Assert.AreEqual(expected, actual,
                $"Expected rule count to equal '{key}'={expected} but was {actual}");
        }

        [When("the user enters {string} in the PHSI import commodity rules search field")]
        public void WhenTheUserEntersInThePHSIImportCommodityRulesSearchField(string text)
        {
            viewAllPHSIImportCommodityRulesPage?.EnterSearchText(text);
        }

        [When("the user sorts the PHSI import commodity rules table by Id descending")]
        public void WhenTheUserSortsThePHSIImportCommodityRulesTableByIdDescending()
        {
            viewAllPHSIImportCommodityRulesPage?.SortByIdDescending();
        }

        [Then("the top PHSI import commodity rule row should match the following details")]
        public void ThenTheTopPHSIImportCommodityRuleRowShouldMatchTheFollowingDetails(Table table)
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

        [Then("the user records the Id of the top PHSI import commodity rule row as {string}")]
        public void ThenTheUserRecordsTheIdOfTheTopPHSIImportCommodityRuleRowAs(string key)
        {
            var id = viewAllPHSIImportCommodityRulesPage!.GetTopRowId();
            Assert.IsNotEmpty(id, "Top row Id is empty");
            _scenarioContext[key] = id;
        }

        [When("the user ticks the Select to Delete checkbox for PHSI import commodity rule Id recorded as {string}")]
        public void WhenTheUserTicksTheSelectToDeleteCheckboxForPHSIImportCommodityRuleIdRecordedAs(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            viewAllPHSIImportCommodityRulesPage?.TickSelectToDeleteCheckboxForRuleId(ruleId);
        }

        [Then("the info banner should display {string} on the View all PHSI \\(Import) Commodity Rules report page")]
        public void ThenTheInfoBannerShouldDisplayOnTheViewAllPHSIImportCommodityRulesReportPage(string expectedText)
        {
            var actual = viewAllPHSIImportCommodityRulesPage!.GetSelectedRulesInfoText();
            Assert.AreEqual(expectedText, actual,
                $"Selected rules info banner mismatch: expected '{expectedText}' but got '{actual}'");
        }

        [When("the user clicks the Delete Rules button on the View all PHSI \\(Import) Commodity Rules report page")]
        public void WhenTheUserClicksTheDeleteRulesButtonOnTheViewAllPHSIImportCommodityRulesReportPage()
        {
            viewAllPHSIImportCommodityRulesPage?.ClickDeleteRulesButton();
        }

        [Then("the Confirm rule deletion dialog should be displayed with {int} rules selected for deletion on the View all PHSI \\(Import) Commodity Rules report page")]
        public void ThenTheConfirmRuleDeletionDialogShouldBeDisplayedWithRulesSelectedForDeletionOnTheViewAllPHSIImportCommodityRulesReportPage(int expectedCount)
        {
            Assert.True(viewAllPHSIImportCommodityRulesPage?.IsConfirmDeletionDialogDisplayed(),
                "Confirm rule deletion dialog is not displayed");
            var actual = viewAllPHSIImportCommodityRulesPage!.GetConfirmDeletionDialogRuleCount();
            Assert.AreEqual(expectedCount, actual,
                $"Expected {expectedCount} rule(s) in deletion dialog but found {actual}");
        }

        [When("the user clicks the Delete rules button on the confirmation dialog on the View all PHSI \\(Import) Commodity Rules report page")]
        public void WhenTheUserClicksTheDeleteRulesButtonOnTheConfirmationDialogOnTheViewAllPHSIImportCommodityRulesReportPage()
        {
            viewAllPHSIImportCommodityRulesPage?.ClickConfirmDeleteButton();
        }

        [Then("the Confirm rule deletion dialog should be closed on the View all PHSI \\(Import) Commodity Rules report page")]
        public void ThenTheConfirmRuleDeletionDialogShouldBeClosedOnTheViewAllPHSIImportCommodityRulesReportPage()
        {
            Assert.True(viewAllPHSIImportCommodityRulesPage?.IsConfirmDeletionDialogClosed(),
                "Confirm rule deletion dialog is still displayed");
        }

        [Then("the PHSI import commodity rule Id recorded as {string} should no longer be present in the rules table")]
        public void ThenThePHSIImportCommodityRuleIdRecordedAsShouldNoLongerBePresentInTheRulesTable(string contextKey)
        {
            Assert.True(viewAllPHSIImportCommodityRulesPage?.IsPageLoaded(),
                "PHSI rules page has not fully loaded after deletion");
            var ruleId = _scenarioContext.Get<string>(contextKey);
            Assert.False(viewAllPHSIImportCommodityRulesPage!.IsRuleIdPresent(ruleId),
                $"Rule Id '{ruleId}' (from '{contextKey}') is still present in the rules table after deletion");
        }

        [Then("the PHSI import commodity rules search field should be empty")]
        public void ThenThePHSIImportCommodityRulesSearchFieldShouldBeEmpty()
        {
            var text = viewAllPHSIImportCommodityRulesPage!.GetSearchInputText();
            Assert.IsEmpty(text, $"Expected search field to be empty but found '{text}'");
        }

        [Then("the PHSI import commodity rules table Id column should have no sort applied")]
        public void ThenThePHSIImportCommodityRulesTableIdColumnShouldHaveNoSortApplied()
        {
            Assert.False(viewAllPHSIImportCommodityRulesPage!.IsIdColumnSorted(),
                "Id column still has a sort applied (aria-sort is present) after deletion");
        }
    }
}