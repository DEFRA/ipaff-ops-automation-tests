using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CommodityRulesStatusForCHEDDSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICommodityRulesStatusForCHEDDPage? commodityRulesStatusForCHEDDPage =>
            _objectContainer.IsRegistered<ICommodityRulesStatusForCHEDDPage>()
                ? _objectContainer.Resolve<ICommodityRulesStatusForCHEDDPage>()
                : null;

        public CommodityRulesStatusForCHEDDSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Commodity rules status for CHED-D page is displayed with the following upload details")]
        public void ThenTheCommodityRulesStatusForCHEDDPageIsDisplayedWithTheFollowingUploadDetails(Table table)
        {
            Assert.True(commodityRulesStatusForCHEDDPage.IsPageLoaded(), "Commodity rules status for CHED-D page is not displayed");
            var actual = commodityRulesStatusForCHEDDPage.GetSummaryDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Commodity rules status for CHED-D page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the Commodity rules status for CHED-D page should show Existing rules equal to {string}")]
        public void ThenTheCommodityRulesStatusForCHEDDPageShouldShowExistingRulesEqualTo(string key)
        {
            var expected = (int)_scenarioContext[key];
            var actual = commodityRulesStatusForCHEDDPage.GetSummaryFieldAsInt("Existing rules");
            Assert.AreEqual(expected, actual,
                $"Expected Existing rules to equal '{key}'={expected} but was {actual}");
        }

        [When("the user clicks the View all CHED-D imports commodity rules link")]
        public void WhenTheUserClicksTheViewAllCHEDDImportsCommodityRulesLink()
        {
            commodityRulesStatusForCHEDDPage.ClickViewAllCHEDDImportsCommodityRulesLink();
        }
    }
}