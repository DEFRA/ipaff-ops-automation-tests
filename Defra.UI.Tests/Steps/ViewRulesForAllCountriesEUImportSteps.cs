using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ViewRulesForAllCountriesEUImportSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IViewRulesForAllCountriesEUImportPage? viewRulesForAllCountriesEUImportPage =>
            _objectContainer.IsRegistered<IViewRulesForAllCountriesEUImportPage>()
                ? _objectContainer.Resolve<IViewRulesForAllCountriesEUImportPage>()
                : null;

        public ViewRulesForAllCountriesEUImportSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the EU import country rule count is recorded as {string}")]
        public void ThenTheEUImportCountryRuleCountIsRecordedAs(string countryRuleCount)
        {
            var count = viewRulesForAllCountriesEUImportPage!.GetTotalRuleCount();
            _scenarioContext[countryRuleCount] = count;
            Assert.Greater(count, 0, $"Recorded EU import country rule count for '{countryRuleCount}' is 0");
        }

        [When("the user enters {string} in the EU import country rules search field")]
        public void WhenTheUserEntersInTheEUImportCountryRulesSearchField(string searchText)
        {
            viewRulesForAllCountriesEUImportPage?.EnterSearchText(searchText);
        }

        [When("the user sorts the EU import country rules table by Id descending")]
        public void WhenTheUserSortsTheEUImportCountryRulesTableByIdDescending()
        {
            viewRulesForAllCountriesEUImportPage?.SortByIdDescending();
        }

        [Then("the top EU import country rule row should match the following details")]
        public void ThenTheTopEUImportCountryRuleRowShouldMatchTheFollowingDetails(Table dataTable)
        {
            var actual = viewRulesForAllCountriesEUImportPage!.GetTopRowDetails();
            foreach (var row in dataTable.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found in top EU import country rule row");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the top EU import country rule row should have Last Updated date as today's date")]
        public void ThenTheTopEUImportCountryRuleRowShouldHaveLastUpdatedDateAsTodaysDate()
        {
            var actual = viewRulesForAllCountriesEUImportPage!.GetTopRowDetails();
            Assert.True(actual.ContainsKey("Last Updated"), "Field 'Last Updated' not found in top EU import country rule row");
            var expected = DateTime.Now.ToString("dd/MM/yyyy");
            Assert.AreEqual(expected, actual["Last Updated"],
                $"Field 'Last Updated' mismatch: expected '{expected}' but got '{actual["Last Updated"]}'");
        }

        [Then("the top EU import country rule row should have Created date as today's date")]
        public void ThenTheTopEUImportCountryRuleRowShouldHaveCreatedDateAsTodaysDate()
        {
            var actual = viewRulesForAllCountriesEUImportPage!.GetTopRowDetails();
            Assert.True(actual.ContainsKey("Created"), "Field 'Created' not found in top EU import country rule row");
            var expected = DateTime.Now.ToString("dd/MM/yyyy");
            Assert.AreEqual(expected, actual["Created"],
                $"Field 'Created' mismatch: expected '{expected}' but got '{actual["Created"]}'");
        }

        [Then("the user records the Id of the top EU import country rule row as {string}")]
        public void ThenTheUserRecordsTheIdOfTheTopEUImportCountryRuleRowAs(string newCountryRuleId)
        {
            var id = viewRulesForAllCountriesEUImportPage!.GetTopRowId();
            Assert.IsNotEmpty(id, "Top EU import country rule row Id is empty");
            _scenarioContext[newCountryRuleId] = id;
        }

        [When("the user clicks the Remove rule link for EU import country rule Id recorded as {string}")]
        public void WhenTheUserClicksTheRemoveRuleLinkForEUImportCountryRuleIdRecordedAs(string newCountryRuleId)
        {
            var ruleId = _scenarioContext.Get<string>(newCountryRuleId);
            viewRulesForAllCountriesEUImportPage?.ClickRemoveRuleLinkForRuleId(ruleId);
        }

        [Then("the EU import country rule Id recorded as {string} should no longer be present in the rules table")]
        public void ThenTheEUImportCountryRuleIdRecordedAsShouldNoLongerBePresentInTheRulesTable(string newCountryRuleId)
        {
            var ruleId = _scenarioContext.Get<string>(newCountryRuleId);
            Assert.False(viewRulesForAllCountriesEUImportPage!.IsRuleIdPresent(ruleId),
                $"EU Import Country Rule Id '{ruleId}' (from '{newCountryRuleId}') is still present in the rules table after removal");
        }

        [Then("the EU import country rule count should be {int} less than {string}")]
        public void ThenTheEUImportCountryRuleCountShouldBeLessThan(int count, string countryRuleCount)
        {
            var initial = (int)_scenarioContext[countryRuleCount];
            var actual = viewRulesForAllCountriesEUImportPage!.GetTotalRuleCount();
            Assert.AreEqual(initial - count, actual,
                $"Expected EU import country rule count to be {initial - count} (initial '{countryRuleCount}'={initial} - {count}) but was {actual}");
        }
    }
}