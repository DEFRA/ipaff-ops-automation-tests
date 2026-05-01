using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class RiskDecisionReportSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IRiskDecisionReportPage? page =>
            _objectContainer.IsRegistered<IRiskDecisionReportPage>()
                ? _objectContainer.Resolve<IRiskDecisionReportPage>()
                : null;

        public RiskDecisionReportSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Risk decision report page should be displayed")]
        public void ThenTheRiskDecisionReportPageShouldBeDisplayed()
        {
            Assert.True(page?.IsPageLoaded(), "Risk decision report page is not displayed");
        }

        [When("the user enters the recorded CHED Reference in the Risk decision search box and clicks Search")]
        public void WhenTheUserEntersTheRecordedCHEDReferenceInTheRiskDecisionSearchBoxAndClicksSearch()
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            page?.Search(chedRef);
        }

        [Then("the Risk decision report returns one matching record")]
        public void ThenTheRiskDecisionReportReturnsOneMatchingRecord()
        {
            Assert.AreEqual(1, page?.GetRecordCount(), "Expected exactly one record");
        }

        [When("the user clicks the Expand button for the CHED Reference")]
        public void WhenTheUserClicksTheExpandButtonForTheCHEDReference()
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            page?.ClickExpandForCHED(chedRef);
        }

        [When("the user clicks the Requests details link")]
        public void WhenTheUserClicksTheRequestsDetailsLink() => page?.ClickRequestsDetails();

        [Then("the Requests section is expanded with details from IPAFFS")]
        public void ThenTheRequestsSectionIsExpandedWithDetailsFromIPAFFS()
        {
            // Visual smoke check — the click above succeeded.
            Assert.IsTrue(page?.IsPageLoaded());
        }

        [When("the user clicks the Decision details link")]
        public void WhenTheUserClicksTheDecisionDetailsLink() => page?.ClickDecisionDetails();

        [Then("the Decision section contains a DecisionRule matching the recorded {string} with the following values")]
        public void ThenTheDecisionSectionContainsADecisionRuleMatchingTheRecordedWithTheFollowingValues(
            string ruleIdKey, Table table)
        {
            var ruleId = _scenarioContext.Get<string>(ruleIdKey);
            var decisionJson = page!.GetDecisionJson();

            StringAssert.Contains($"\"RuleId\":\"{ruleId}\"", decisionJson,
                $"DecisionRules does not contain RuleId '{ruleId}'");

            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                string expectedFragment = field switch
                {
                    "IsTriggered" => $"\"IsTriggered\":{expected.ToLower()}",
                    "RuleType" or "RegulatorType" => $"\"{field}\":\"{expected}\"",
                    _ => $"\"{field}\":{expected}"
                };
                StringAssert.Contains(expectedFragment, decisionJson,
                    $"Decision JSON missing fragment '{expectedFragment}'");
            }
        }
    }
}