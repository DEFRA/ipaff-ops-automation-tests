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

        private ICHEDPPHMISystemConfigPage? chedPPHmiSystemConfigPage =>
            _objectContainer.IsRegistered<ICHEDPPHMISystemConfigPage>()
                ? _objectContainer.Resolve<ICHEDPPHMISystemConfigPage>()
                : null;

        public CHEDPPHMISystemConfigSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the HMI System Configuration page should be displayed")]
        public void ThenTheHMISystemConfigurationPageShouldBeDisplayed()
        {
            Assert.True(chedPPHmiSystemConfigPage?.IsPageLoaded(), "HMI System Configuration page is not displayed");
        }

        [When("the user makes note of the current setting for AIS Country Rate")]
        public void WhenTheUserMakesNoteOfTheCurrentSettingForAISCountryRate()
        {
            var countryRate = chedPPHmiSystemConfigPage?.GetCurrentAISRate();
            _scenarioContext["AISCountryRate"] = countryRate;
        }

        [When("the user sets the AIS Country Rate to {int}")]
        public void WhenTheUserSetsTheAISCountryRateTo(int newAISRate)
        {
            chedPPHmiSystemConfigPage?.SetNewAISRate(newAISRate);
            _scenarioContext["NewAISCountryRate"] = newAISRate;
        }

        [When("the user clicks the Save and continue button on the HMI System Configuration page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheHMISystemConfigurationPage()
        {
            chedPPHmiSystemConfigPage.ClickSaveAndContinueButton();
        }

        [When("the user resets the AIS Country Rate back to the value noted at the start")]
        public void WhenTheUserResetsTheAISCountryRateBackToTheValueNotedAtTheStart()
        {
            var originalAISRate = (int)_scenarioContext["AISCountryRate"];
            chedPPHmiSystemConfigPage?.SetNewAISRate(originalAISRate);
        }
    }
}