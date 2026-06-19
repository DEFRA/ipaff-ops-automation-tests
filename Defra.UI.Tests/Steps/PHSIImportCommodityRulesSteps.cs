using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class PHSIImportCommodityRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IPHSIImportCommodityRulesPage? PHSIImportCommodityRulesPage =>
            _objectContainer.IsRegistered<IPHSIImportCommodityRulesPage>()
                ? _objectContainer.Resolve<IPHSIImportCommodityRulesPage>()
                : null;

        public PHSIImportCommodityRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the PHSI \\(Import\\) Commodity Rules page should be displayed")]
        public void ThenThePHSIImportCommodityRulesPageShouldBeDisplayed()
        {
            Assert.True(PHSIImportCommodityRulesPage?.IsPageLoaded(), "PHSI (Import) - Commodity Rules page is not displayed");
        }

        [When("the user selects {string} from the Countries dropdown on the PHSI commodity rules page")]
        public void WhenTheUserSelectsFromTheCountriesDropdownOnThePHSICommodityRulesPage(string country)
        {
            PHSIImportCommodityRulesPage?.SelectCountry(country);
        }

        [When("the user searches for commodity {string} with name {string} on the PHSI commodity rules page")]
        public void WhenTheUserSearchesForCommodityWithNameOnThePHSICommodityRulesPage(string commodityCode, string commodityName)
        {
            PHSIImportCommodityRulesPage?.SearchCommodity(commodityCode, commodityName);
        }

        [When("the user sets the inspection rate to {int} on the PHSI commodity rules page")]
        public void WhenTheUserSetsTheInspectionRateToOnThePHSICommodityRulesPage(int rate)
        {
            PHSIImportCommodityRulesPage?.SetInspectionRate(rate);
        }

        [When("the user ensures the Permanent checkbox is checked on the PHSI commodity rules page")]
        public void WhenTheUserEnsuresThePermanentCheckboxIsCheckedOnThePHSICommodityRulesPage()
        {
            PHSIImportCommodityRulesPage?.EnsurePermanentIsChecked();
        }

        [When("the user ensures the Alignment of inspections checkbox is checked on the PHSI commodity rules page")]
        public void WhenTheUserEnsuresTheAlignmentOfInspectionsCheckboxIsCheckedOnThePHSICommodityRulesPage()
        {
            PHSIImportCommodityRulesPage?.EnsureAlignmentOfInspectionsIsChecked();
        }

        [When("the user clicks the Confirm and send button on the PHSI commodity rules page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnThePHSICommodityRulesPage()
        {
            PHSIImportCommodityRulesPage?.ClickConfirmAndSendButton();
        }
    }
}