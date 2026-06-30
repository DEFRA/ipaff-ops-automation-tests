using Defra.UI.Tests.Pages.Interfaces;
using Faker;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class HMIImportCountryRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IHMIImportCountryRulesPage? HMIImportCountryRulesPage =>
            _objectContainer.IsRegistered<IHMIImportCountryRulesPage>()
                ? _objectContainer.Resolve<IHMIImportCountryRulesPage>()
                : null;

        public HMIImportCountryRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the HMI \\(Import) Country Rules page should be displayed")]
        public void ThenTheHMIImportCountryRulesPageShouldBeDisplayed()
        {
            Assert.True(HMIImportCountryRulesPage?.IsPageLoaded(), "HMI (Import) Country Rules page is not displayed");
        }

        [When("the user selects {string} from the country dropdown on the HMI import country rules page")]
        public void WhenTheUserSelectsFromTheCountryDropdownOnTheHMIImportCountryRulesPage(string country)
        {
            HMIImportCountryRulesPage?.SelectCountry(country);
        }

        [When("the user sets the inspection rate to {int} on the HMI import country rules page")]
        public void WhenTheUserSetsTheInspectionRateToOnTheHMIImportCountryRulesPage(int rate)
        {
            HMIImportCountryRulesPage?.SetInspectionRate(rate);
        }

        [When("the user ensures the Approved Inspection Service checkbox is not checked on the HMI import country rules page")]
        public void WhenTheUserEnsuresTheApprovedInspectionServiceCheckboxIsNotCheckedOnTheHMIImportCountryRulesPage()
        {
            HMIImportCountryRulesPage?.EnsureApprovedInspectionServiceIsNotChecked();
        }

        [When("the user ensures the Approved Inspection Service checkbox is checked on the HMI import country rules page")]
        public void WhenTheUserEnsuresTheApprovedInspectionServiceCheckboxIsCheckedOnTheHMIImportCountryRulesPage()
        {
            HMIImportCountryRulesPage?.EnsureApprovedInspectionServiceIsChecked();
        }

        [When("the user ensures the Permanent checkbox is checked on the HMI import country rules page")]
        public void WhenTheUserEnsuresThePermanentCheckboxIsCheckedOnTheHMIImportCountryRulesPage()
        {
            HMIImportCountryRulesPage?.EnsurePermanentIsChecked();
        }

        [When("the user clicks the Confirm and send button on the HMI import country rules page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheHMIImportCountryRulesPage()
        {
            HMIImportCountryRulesPage?.ClickConfirmAndSendButton();
        }
    }
}