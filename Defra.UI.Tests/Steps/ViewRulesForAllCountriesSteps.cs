using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ViewRulesForAllCountriesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IViewRulesForAllCountriesPage? ViewRulesForAllCountriesPage =>
            _objectContainer.IsRegistered<IViewRulesForAllCountriesPage>()
                ? _objectContainer.Resolve<IViewRulesForAllCountriesPage>()
                : null;

        public ViewRulesForAllCountriesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the View rules for all countries page should be displayed")]
        public void ThenTheViewRulesForAllCountriesPageShouldBeDisplayed()
        {
            Assert.True(ViewRulesForAllCountriesPage?.IsPageLoaded(), "View rules for all countries page is not displayed");
        }

        [When("the user scrolls to the bottom of the View rules for all countries page")]
        public void WhenTheUserScrollsToTheBottomOfTheViewRulesForAllCountriesPage()
        {
            ViewRulesForAllCountriesPage?.ScrollToBottom();
        }

        [Then("the HMI country rule count is recorded as {string}")]
        public void ThenTheHMICountryRuleCountIsRecordedAs(string key)
        {
            var count = ViewRulesForAllCountriesPage!.GetTotalRuleCount();
            _scenarioContext[key] = count;
            Assert.Greater(count, 0, $"Recorded HMI country rule count for '{key}' is 0");
        }

        [Then("the HMI country rule count should be {int} less than {string}")]
        public void ThenTheHMICountryRuleCountShouldBeLessThan(int delta, string key)
        {
            var initial = (int)_scenarioContext[key];
            var actual = ViewRulesForAllCountriesPage!.GetTotalRuleCount();
            Assert.AreEqual(initial - delta, actual,
                $"Expected HMI country rule count to be {initial - delta} (initial '{key}'={initial} - {delta}) but was {actual}");
        }

        [When("the user enters {string} in the HMI country rules search field")]
        public void WhenTheUserEntersInTheHMICountryRulesSearchField(string text)
        {
            ViewRulesForAllCountriesPage?.EnterSearchText(text);
        }

        [When("the user sorts the HMI country rules table by Id descending")]
        public void WhenTheUserSortsTheHMICountryRulesTableByIdDescending()
        {
            ViewRulesForAllCountriesPage?.SortByIdDescending();
        }

        [Then("the top HMI country rule row should match the following details")]
        public void ThenTheTopHMICountryRuleRowShouldMatchTheFollowingDetails(Table table)
        {
            var actual = ViewRulesForAllCountriesPage!.GetTopRowDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found in top HMI country rule row");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the top HMI country rule row should have Last Updated date as today's date")]
        public void ThenTheTopHMICountryRuleRowShouldHaveLastUpdatedDateAsTodaysDate()
        {
            var actual = ViewRulesForAllCountriesPage!.GetTopRowDetails();
            Assert.True(actual.ContainsKey("Last Updated"), "Field 'Last Updated' not found in top row");
            var expected = DateTime.Now.ToString("dd/MM/yyyy");
            Assert.AreEqual(expected, actual["Last Updated"],
                $"Field 'Last Updated' mismatch: expected '{expected}' but got '{actual["Last Updated"]}'");
        }

        [Then("the top HMI country rule row should have Created date as today's date")]
        public void ThenTheTopHMICountryRuleRowShouldHaveCreatedDateAsTodaysDate()
        {
            var actual = ViewRulesForAllCountriesPage!.GetTopRowDetails();
            Assert.True(actual.ContainsKey("Created"), "Field 'Created' not found in top row");
            var expected = DateTime.Now.ToString("dd/MM/yyyy");
            Assert.AreEqual(expected, actual["Created"],
                $"Field 'Created' mismatch: expected '{expected}' but got '{actual["Created"]}'");
        }

        [Then("the user records the Id of the top HMI country rule row as {string}")]
        public void ThenTheUserRecordsTheIdOfTheTopHMICountryRuleRowAs(string key)
        {
            var id = ViewRulesForAllCountriesPage!.GetTopRowId();
            Assert.IsNotEmpty(id, "Top HMI country rule row Id is empty");
            _scenarioContext[key] = id;
        }

        [When("the user ticks the Select to Delete checkbox for HMI country rule Id recorded as {string}")]
        public void WhenTheUserTicksTheSelectToDeleteCheckboxForHMICountryRuleIdRecordedAs(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            ViewRulesForAllCountriesPage?.TickSelectToDeleteCheckboxForRuleId(ruleId);
        }

        [Then("the View rules for all countries page should display the {string} info banner")]
        public void ThenTheViewRulesForAllCountriesPageShouldDisplayTheInfoBanner(string expectedText)
        {
            var actual = ViewRulesForAllCountriesPage!.GetSelectedRulesInfoText();
            Assert.AreEqual(expectedText, actual,
                $"Info banner mismatch: expected '{expectedText}' but got '{actual}'");
        }

        [When("the user clicks the Delete Rules button on the View rules for all countries page")]
        public void WhenTheUserClicksTheDeleteRulesButtonOnTheViewRulesForAllCountriesPage()
        {
            ViewRulesForAllCountriesPage?.ClickDeleteRulesButton();
        }

        [Then("the Confirm rule deletion dialog should be displayed with {int} rules selected for deletion on the View rules for all countries page")]
        public void ThenTheConfirmRuleDeletionDialogShouldBeDisplayedWithRulesSelectedForDeletionOnTheViewRulesForAllCountriesPage(int expectedCount)
        {
            Assert.True(ViewRulesForAllCountriesPage?.IsConfirmDeletionDialogDisplayed(),
                "Confirm rule deletion dialog is not displayed");
            var actual = ViewRulesForAllCountriesPage!.GetConfirmDeletionDialogRuleCount();
            Assert.AreEqual(expectedCount, actual,
                $"Expected {expectedCount} rule(s) in deletion dialog but found {actual}");
        }

        [When("the user clicks the Delete rules button on the confirmation dialog on the View rules for all countries page")]
        public void WhenTheUserClicksTheDeleteRulesButtonOnTheConfirmationDialogOnTheViewRulesForAllCountriesPage()
        {
            ViewRulesForAllCountriesPage?.ClickConfirmDeleteButton();
        }

        [Then("the Confirm rule deletion dialog should be closed on the View rules for all countries page")]
        public void ThenTheConfirmRuleDeletionDialogShouldBeClosedOnTheViewRulesForAllCountriesPage()
        {
            Assert.True(ViewRulesForAllCountriesPage?.IsConfirmDeletionDialogClosed(),
                            "Confirm rule deletion dialog is still displayed");
        }

        [Then("the HMI country rule Id recorded as {string} should no longer be present in the rules table")]
        public void ThenTheHMICountryRuleIdRecordedAsShouldNoLongerBePresentInTheRulesTable(string contextKey)
        {
            var ruleId = _scenarioContext.Get<string>(contextKey);
            Assert.False(ViewRulesForAllCountriesPage!.IsRuleIdPresent(ruleId),
                $"HMI Country Rule Id '{ruleId}' (from '{contextKey}') is still present in the rules table after deletion");
        }
    }
}