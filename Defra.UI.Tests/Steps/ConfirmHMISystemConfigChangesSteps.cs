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

        private IConfirmHMISystemConfigChangesPage? ConfirmHMISystemConfigChangesPage =>
            _objectContainer.IsRegistered<IConfirmHMISystemConfigChangesPage>()
                ? _objectContainer.Resolve<IConfirmHMISystemConfigChangesPage>()
                : null;

        public ConfirmHMISystemConfigChangesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Confirm HMI System Configuration Changes page should be displayed")]
        public void ThenTheConfirmHMISystemConfigurationChangesPageShouldBeDisplayed()
        {
            Assert.True(ConfirmHMISystemConfigChangesPage?.IsPageLoaded(), "Confirm HMI System Configuration Changes is not displayed");
        }

        [Then("the AIS Country Rate should be displayed as {int}")]
        public void ThenTheAISCountryRateShouldBeDisplayedAs(int aisRateExpected)
        {
            var aisRateExpectedNew = "AIS: Country Rate to " + aisRateExpected;
            var aisRateActual = ConfirmHMISystemConfigChangesPage?.GetAISCountryRate();

            StringAssert.Contains($"AIS: Country Rate to {aisRateExpected}%", aisRateActual, $"AIS Country rate should be {aisRateExpected}");
        }

        [Then("the user clicks the Confirm and send button")]
        public void ThenTheUserClicksTheConfirmAndSendButton()
        {
            ConfirmHMISystemConfigChangesPage?.ClickConfirmAndSendButton();
        }

    }
}
