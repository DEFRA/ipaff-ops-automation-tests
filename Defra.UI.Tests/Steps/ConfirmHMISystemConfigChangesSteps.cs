using Defra.UI.Tests.Pages.Classes;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class ConfirmHMISystemConfigChangesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IConfirmHMISystemConfigChangesPage? confirmHMISystemConfigChangesPage =>
            _objectContainer.IsRegistered<IConfirmHMISystemConfigChangesPage>()
                ? _objectContainer.Resolve<IConfirmHMISystemConfigChangesPage>()
                : null;

        public ConfirmHMISystemConfigChangesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Confirm HMI System Configuration Changes page should be displayed")]
        public void ThenTheConfirmHMISystemConfigurationChangesPageShouldBeDisplayed()
        {
            Assert.True(confirmHMISystemConfigChangesPage?.IsPageLoaded(), "Confirm HMI System Configuration Changes is not displayed");
        }

        [Then("the AIS Country Rate should be displayed as {int}")]
        public void ThenTheAISCountryRateShouldBeDisplayedAs(int aisRateExpected)
        {
            var aisRateExpectedNew = "AIS: Country Rate to " + aisRateExpected;
            var aisRateActual = confirmHMISystemConfigChangesPage?.GetAISCountryRate();

            StringAssert.Contains($"AIS: Country Rate to {aisRateExpected}%", aisRateActual, $"AIS Country rate should be {aisRateExpected}");
        }

        [Then("the user clicks the Confirm and send button")]
        public void ThenTheUserClicksTheConfirmAndSendButton()
        {
            confirmHMISystemConfigChangesPage?.ClickConfirmAndSendButton();
        }
    }
}