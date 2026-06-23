using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class CHEDPPHMISystemConfigSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICHEDPPHMISystemConfigPage? ChedPPHmiSystemConfigPage =>
            _objectContainer.IsRegistered<ICHEDPPHMISystemConfigPage>()
                ? _objectContainer.Resolve<ICHEDPPHMISystemConfigPage>()
                : null;

        public CHEDPPHMISystemConfigSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the HMI System Configuration page should be displayed")]
        public void ThenTheHMISystemConfigurationPageShouldBeDisplayed()
        {
            Assert.True(ChedPPHmiSystemConfigPage?.IsPageLoaded(), "HMI System Configuration page is not displayed");
        }

        [When("the user makes note of the current setting for AIS Country Rate")]
        public void WhenTheUserMakesNoteOfTheCurrentSettingForAISCountryRate()
        {
            var countryRate = ChedPPHmiSystemConfigPage?.GetCurrentAISRate();
            _scenarioContext["AISCountryRate"] = countryRate;
        }

        [When("the user sets the AIS Country Rate to {int}")]
        public void WhenTheUserSetsTheAISCountryRateTo(int newAISRate)
        {
            ChedPPHmiSystemConfigPage?.SetNewAISRate(newAISRate);
            _scenarioContext["NewAISCountryRate"] = newAISRate;
        }

        [When("the user clicks the Save and continue button")]
        public void WhenTheUserClicksTheSaveAndContinueButton()
        {
            ChedPPHmiSystemConfigPage.ClickSaveAndContinueButton();
        }

    }
}