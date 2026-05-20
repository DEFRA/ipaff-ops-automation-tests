using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CommodityRulesStatusForCHEDASteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICommodityRulesStatusForCHEDAPage? commodityRulesStatusForCHEDAPage =>
            _objectContainer.IsRegistered<ICommodityRulesStatusForCHEDAPage>()
                ? _objectContainer.Resolve<ICommodityRulesStatusForCHEDAPage>()
                : null;

        public CommodityRulesStatusForCHEDASteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Commodity rules status for CHED-A page is displayed with the following upload details")]
        public void ThenTheCommodityRulesStatusForCHEDAPageIsDisplayedWithTheFollowingUploadDetails(Table table)
        {
            Assert.True(commodityRulesStatusForCHEDAPage.IsPageLoaded(), "Commodity rules status for CHED-A page is not displayed");
            var actual = commodityRulesStatusForCHEDAPage.GetSummaryDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Commodity rules status for CHED-A page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the Commodity rules status for CHED-A page should show Existing rules equal to {string}")]
        public void ThenTheCommodityRulesStatusForCHEDAPageShouldShowExistingRulesEqualTo(string key)
        {
            var expected = (int)_scenarioContext[key];
            var actual = commodityRulesStatusForCHEDAPage.GetSummaryFieldAsInt("Existing rules");
            Assert.AreEqual(expected, actual,
                $"Expected Existing rules to equal '{key}'={expected} but was {actual}");
        }

        [When("the user clicks the View all CHED-A imports commodity rules link")]
        public void WhenTheUserClicksTheViewAllCHEDAImportsCommodityRulesLink()
        {
            commodityRulesStatusForCHEDAPage.ClickViewAllCHEDAImportsCommodityRulesLink();
        }
    }
}