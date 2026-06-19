using System.Linq;
using System.Text.Json;
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

        private IRiskDecisionReportPage? riskDecisionReportPage =>
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
            Assert.True(riskDecisionReportPage?.IsPageLoaded(), "Risk decision report page is not displayed");
        }

        [When("the user enters the recorded CHED Reference for {string} in the Risk decision search box and clicks Search")]
        public void WhenTheUserEntersTheRecordedCHEDReferenceForIterationInTheRiskDecisionSearchBoxAndClicksSearch(string iterationName)
        {
            var chedRef = _scenarioContext.Get<string>($"{iterationName}_CHEDReference");
            riskDecisionReportPage?.Search(chedRef);
        }

        [Then("the Risk decision report returns one matching record")]
        public void ThenTheRiskDecisionReportReturnsOneMatchingRecord()
        {
            Assert.AreEqual(1, riskDecisionReportPage?.GetRecordCount(), "Expected exactly one record");
        }

        [When("the user clicks the Expand button for the CHED Reference of {string}")]
        public void WhenTheUserClicksTheExpandButtonForTheCHEDReferenceOfIteration(string iterationName)
        {
            var chedRef = _scenarioContext.Get<string>($"{iterationName}_CHEDReference");
            riskDecisionReportPage?.ClickExpandForCHED(chedRef);
        }

        [When("the user clicks the Requests details link")]
        public void WhenTheUserClicksTheRequestsDetailsLink() => riskDecisionReportPage?.ClickRequestsDetails();

        [Then("the Requests section is expanded with details from IPAFFS")]
        public void ThenTheRequestsSectionIsExpandedWithDetailsFromIPAFFS()
        {
            var requestsJson = riskDecisionReportPage!.GetRequestsJson();
            Assert.IsNotEmpty(requestsJson, "Requests JSON content is empty");
            _scenarioContext["RiskDecisionRequestsJson"] = requestsJson;
        }

        [When("the user clicks the Decision details link")]
        public void WhenTheUserClicksTheDecisionDetailsLink() => riskDecisionReportPage?.ClickDecisionDetails();

        [Then("the Decision section contains a DecisionRule matching the recorded {string} with the following values")]
        public void ThenTheDecisionSectionContainsADecisionRuleMatchingTheRecordedWithTheFollowingValues(
            string ruleIdKey, Table table)
        {
            var ruleId = _scenarioContext.Get<string>(ruleIdKey);
            var decisionJson = riskDecisionReportPage!.GetDecisionJson();
            _scenarioContext["RiskDecisionJson"] = decisionJson;

            // Parse JSON and locate the specific DecisionRule by RuleId
            using var doc = JsonDocument.Parse(decisionJson);
            var commodities = doc.RootElement.GetProperty("Commodities");

            JsonElement? matchedRule = null;
            foreach (var commodity in commodities.EnumerateArray())
            {
                var rules = commodity.GetProperty("DecisionRules");
                foreach (var rule in rules.EnumerateArray())
                {
                    if (rule.GetProperty("RuleId").GetString() == ruleId)
                    {
                        matchedRule = rule;
                        break;
                    }
                }
                if (matchedRule.HasValue) break;
            }

            Assert.IsNotNull(matchedRule, $"DecisionRules does not contain RuleId '{ruleId}' in the Decision JSON");

            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                var property = matchedRule.Value.GetProperty(field);

                var actual = property.ValueKind switch
                {
                    JsonValueKind.String => property.GetString(),
                    JsonValueKind.True => "true",
                    JsonValueKind.False => "false",
                    _ => property.GetRawText()
                };

                Assert.AreEqual(expected, actual,
                    $"DecisionRule field '{field}' mismatch for RuleId '{ruleId}': expected '{expected}' but got '{actual}'");
            }
        }
    }
}