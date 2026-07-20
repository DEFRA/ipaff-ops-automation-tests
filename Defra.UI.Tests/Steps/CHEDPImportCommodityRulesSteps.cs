using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDPImportCommodityRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICHEDPImportCommodityRulesPage? chedpImportCommodityRulesPage =>
            _objectContainer.IsRegistered<ICHEDPImportCommodityRulesPage>()
                ? _objectContainer.Resolve<ICHEDPImportCommodityRulesPage>()
                : null;

        public CHEDPImportCommodityRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the CHED-P \\(Import) Commodity Rules page should be displayed")]
        public void ThenTheCHED_PImportCommodityRulesPageShouldBeDisplayed()
        {
            Assert.True(chedpImportCommodityRulesPage?.IsPageLoaded(), "CHED-P (Import) Commodity Rules page is not displayed");
        }

        [When("the user searches for commodity {string} with name {string} on the CHED-P \\(Import) Commodity Rules page")]
        public void WhenTheUserSearchesForCommodityWithNameOnTheCHED_PImportCommodityRulesPage(string commodityCode, string commodityName)
        {
            chedpImportCommodityRulesPage?.SearchCommodity(commodityCode, commodityName);
        }

        [When("the user selects {string} in the Countries field on the CHED-P \\(Import) Commodity Rules page")]
        public void WhenTheUserSelectsInTheCountriesFieldOnTheCHED_PImportCommodityRulesPage(string country)
        {
            chedpImportCommodityRulesPage?.SelectCountry(country);
        }

        [When("the user sets the inspection rate to {int} on the CHED-P \\(Import) Commodity Rules page")]
        public void WhenTheUserSetsTheInspectionRateToOnTheCHED_PImportCommodityRulesPage(int rate)
        {
            chedpImportCommodityRulesPage?.SetInspectionRate(rate);
        }

        [When("the user ensures the {string} checkbox is checked on the CHED-P \\(Import) Commodity Rules page")]
        public void WhenTheUserEnsuresTheCheckboxIsCheckedOnTheCHED_PImportCommodityRulesPage(string checkboxLabel)
        {
            chedpImportCommodityRulesPage?.EnsureCheckboxIsChecked(checkboxLabel);
        }

        [When("the user clicks the Confirm and send button on the CHED-P \\(Import) Commodity Rules page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheCHED_PImportCommodityRulesPage()
        {
            chedpImportCommodityRulesPage?.ClickConfirmAndSendButton();
        }
    }
}