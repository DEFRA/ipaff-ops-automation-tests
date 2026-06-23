using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class HMIImportCommodityRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IHMIImportCommodityRulesPage? hmiImportCommodityRulesPage =>
            _objectContainer.IsRegistered<IHMIImportCommodityRulesPage>()
                ? _objectContainer.Resolve<IHMIImportCommodityRulesPage>()
                : null;

        public HMIImportCommodityRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the HMI \\(Import\\) - Commodity Rules page should be displayed")]
        public void ThenTheHMIImportCommodityRulesPageShouldBeDisplayed()
        {
            Assert.True(hmiImportCommodityRulesPage?.IsPageLoaded(), "HMI (Import) - Commodity Rules page is not displayed");
        }

        [When("the user selects {string} from the country dropdown on the HMI import commodity rules page")]
        public void WhenTheUserSelectsFromTheCountryDropdownOnTheHMIImportCommodityRulesPage(string country)
        {
            hmiImportCommodityRulesPage?.SelectCountry(country);
        }

        [When("the user searches for commodity {string} with name {string} on the HMI import commodity rules page")]
        public void WhenTheUserSearchesForCommodityWithNameOnTheHMIImportCommodityRulesPage(string commodityCode, string commodityName)
        {
            hmiImportCommodityRulesPage?.SearchCommodity(commodityCode, commodityName);
        }

        [When("the user sets the inspection rate to {int} on the HMI import commodity rules page")]
        public void WhenTheUserSetsTheInspectionRateToOnTheHMIImportCommodityRulesPage(int rate)
        {
            hmiImportCommodityRulesPage?.SetInspectionRate(rate);
        }

        [When("the user ensures the Permanent checkbox is checked on the HMI import commodity rules page")]
        public void WhenTheUserEnsuresThePermanentCheckboxIsCheckedOnTheHMIImportCommodityRulesPage()
        {
            hmiImportCommodityRulesPage?.EnsurePermanentIsChecked();
        }

        [When("the user clicks the Confirm and send button on the HMI import commodity rules page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheHMIImportCommodityRulesPage()
        {
            hmiImportCommodityRulesPage?.ClickConfirmAndSendButton();
        }
    }
}