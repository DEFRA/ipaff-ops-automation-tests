using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CsvFileDetailsAndStatusSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICsvFileDetailsAndStatusPage? csvFileDetailsAndStatusPage =>
            _objectContainer.IsRegistered<ICsvFileDetailsAndStatusPage>()
                ? _objectContainer.Resolve<ICsvFileDetailsAndStatusPage>()
                : null;

        public CsvFileDetailsAndStatusSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the CSV file details and status page should show Total rules {int} more than {string}")]
        [Then("the CSV file details and status page should be displayed with the count of Total rules {int} more than {string}")]
        public void ThenCsvFileDetailsPageShowsTotalRulesIncreased(int delta, string contextKey)
        {
            Assert.True(csvFileDetailsAndStatusPage?.IsPageLoaded(), "CSV file details and status page is not displayed");
            var initial = (int)_scenarioContext[contextKey];
            var actual = csvFileDetailsAndStatusPage!.GetSummaryFieldAsInt("Total rules");
            Assert.AreEqual(initial + delta, actual,
                $"Expected 'Total rules' to be {initial + delta} (initial '{contextKey}'={initial} + {delta}) but was {actual}");
        }

        [Then("the CSV file details and status page should show Existing rules equal to {string}")]
        public void ThenCsvFileDetailsPageShowsExistingRulesEqualTo(string contextKey)
        {
            Assert.True(csvFileDetailsAndStatusPage?.IsPageLoaded(), "CSV file details and status page is not displayed");
            var expected = (int)_scenarioContext[contextKey];
            var actual = csvFileDetailsAndStatusPage!.GetSummaryFieldAsInt("Existing rules");
            Assert.AreEqual(expected, actual,
                $"'Existing rules' mismatch: expected '{expected}' ('{contextKey}') but got '{actual}'");
        }

        [Then("the CSV file details and status page is displayed with the following upload details")]
        public void ThenCsvFileDetailsPageShowsUploadDetails(Table table)
        {
            Assert.True(csvFileDetailsAndStatusPage?.IsPageLoaded(), "CSV file details and status page is not displayed");
            var actual = csvFileDetailsAndStatusPage!.GetSummaryDetails();

            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on CSV file details and status page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [When("the user clicks the PHSI reporting link at the bottom of the summary page")]
        public void WhenTheUserClicksThePhsiReportingLink()
        {
            csvFileDetailsAndStatusPage?.ClickPhsiReportingLink();
        }
    }
}