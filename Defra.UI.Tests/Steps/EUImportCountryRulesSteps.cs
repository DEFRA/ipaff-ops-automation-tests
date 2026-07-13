using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class EUImportCountryRulesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IEUImportCountryRulesPage? euImportCountryRulesPage =>
            _objectContainer.IsRegistered<IEUImportCountryRulesPage>()
                ? _objectContainer.Resolve<IEUImportCountryRulesPage>()
                : null;

        public EUImportCountryRulesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the EU \\(Import) Country Rules page should be displayed")]
        public void ThenTheEUImportCountryRulesPageShouldBeDisplayed()
        {
            Assert.True(euImportCountryRulesPage?.IsPageLoaded(),
                "EU (Import) Country Rules page is not displayed");
        }

        [When("the user selects {string} from the country dropdown on the EU import country rules page")]
        public void WhenTheUserSelectsFromTheCountryDropdownOnTheEUImportCountryRulesPage(string country)
        {
            euImportCountryRulesPage?.SelectCountry(country);
        }

        [When("the user sets the inspection rate to {int} on the EU import country rules page")]
        public void WhenTheUserSetsTheInspectionRateToOnTheEUImportCountryRulesPage(int rate)
        {
            euImportCountryRulesPage?.SetInspectionRate(rate);
        }

        [When("the user ensures the Permanent checkbox is checked on the EU import country rules page")]
        public void WhenTheUserEnsuresThePermanentCheckboxIsCheckedOnTheEUImportCountryRulesPage()
        {
            euImportCountryRulesPage?.EnsurePermanentCheckboxIsChecked();
        }

        [When("the user clicks the Confirm and send button on the EU import country rules page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheEUImportCountryRulesPage()
        {
            euImportCountryRulesPage?.ClickConfirmAndSendButton();
        }
    }
}