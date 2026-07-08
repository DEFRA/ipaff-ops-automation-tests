using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class HMIExportCommodityRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IHMIExportCommodityRulesPage? hmiExportCommodityRulesPage =>
            _objectContainer.IsRegistered<IHMIExportCommodityRulesPage>()
                ? _objectContainer.Resolve<IHMIExportCommodityRulesPage>()
                : null;

        public HMIExportCommodityRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the HMI \\(Export\\) Commodity Rules page should be displayed")]
        public void ThenTheHMIExportCommodityRulesPageShouldBeDisplayed()
        {
            Assert.True(hmiExportCommodityRulesPage?.IsPageLoaded(), "HMI (Export) Commodity Rules page is not displayed");
        }

        [When("the user selects {string} from the commodity dropdown on the HMI export commodity rules page")]
        public void WhenTheUserSelectsFromTheCommodityDropdownOnTheHMIExportCommodityRulesPage(string commodity)
        {
            hmiExportCommodityRulesPage?.SelectCommodity(commodity);
        }

        [When("the user selects {string} from the variety dropdown on the HMI export commodity rules page")]
        public void WhenTheUserSelectsFromTheVarietyDropdownOnTheHMIExportCommodityRulesPage(string variety)
        {
            hmiExportCommodityRulesPage?.SelectVariety(variety);
        }

        [When("the user sets the inspection rate to {int} on the HMI export commodity rules page")]
        public void WhenTheUserSetsTheInspectionRateToOnTheHMIExportCommodityRulesPage(int rate)
        {
            hmiExportCommodityRulesPage?.SetInspectionRate(rate);
        }

        [When("the user ensures the Permanent checkbox is checked on the HMI export commodity rules page")]
        public void WhenTheUserEnsuresThePermanentCheckboxIsCheckedOnTheHMIExportCommodityRulesPage()
        {
            hmiExportCommodityRulesPage?.EnsurePermanentIsChecked();
        }

        [When("the user clicks the Confirm and send button on the HMI export commodity rules page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheHMIExportCommodityRulesPage()
        {
            hmiExportCommodityRulesPage?.ClickConfirmAndSendButton();
        }
    }
}