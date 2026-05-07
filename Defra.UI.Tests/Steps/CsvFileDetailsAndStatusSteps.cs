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

        [Then("the CSV file details and status page should be displayed with {int} incoming rules added")]
        public void ThenCsvFileDetailsPageShowsIncomingRulesAdded(int expected)
        {
            Assert.True(csvFileDetailsAndStatusPage?.IsPageLoaded(), "CSV file details and status page is not displayed");
            var actual = csvFileDetailsAndStatusPage!.GetSummaryFieldAsInt("Incoming rules to be added");
            Assert.AreEqual(expected, actual,
                $"Expected 'Incoming rules to be added' to be {expected} but was {actual}");
        }

        [Then("the CSV file details and status page should be displayed with the count of Total rules {int} more than {string}")]
        public void ThenCsvFileDetailsPageShowsTotalRulesIncreased(int delta, string contextKey)
        {
            Assert.True(csvFileDetailsAndStatusPage?.IsPageLoaded(), "CSV file details and status page is not displayed");
            var initial = (int)_scenarioContext[contextKey];
            var actual = csvFileDetailsAndStatusPage!.GetSummaryFieldAsInt("Total rules");
            Assert.AreEqual(initial + delta, actual,
                $"Expected 'Total rules' to be {initial + delta} (initial '{contextKey}'={initial} + {delta}) but was {actual}");
        }

        [When("the user clicks the PHSI reporting link at the bottom of the summary page")]
        public void WhenTheUserClicksThePhsiReportingLink()
        {
            csvFileDetailsAndStatusPage?.ClickPhsiReportingLink();
        }
    }
}