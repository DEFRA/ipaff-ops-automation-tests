using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class DefaultRulesForCHEDPPSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IDefaultRulesForCHEDPPPage? defaultRulesForCHEDPPPage =>
            _objectContainer.IsRegistered<IDefaultRulesForCHEDPPPage>()
                ? _objectContainer.Resolve<IDefaultRulesForCHEDPPPage>()
                : null;

        public DefaultRulesForCHEDPPSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Default rules page should be displayed")]
        public void ThenTheDefaultRulesPageShouldBeDisplayed()
        {
            Assert.True(defaultRulesForCHEDPPPage?.IsPageLoaded(), "Default rules page is not displayed");
        }

        [When("the user makes a note of the HMI \\(Import\\) setting value on the Default Rules page")]
        public void ThenTheUserMakesANoteOfTheHMIImportSettingValueOnTheDefaultRulesPage()
        {
            var hmiImportRate = defaultRulesForCHEDPPPage?.GetHmiImportRate();
            _scenarioContext["HmiImportRate"] = hmiImportRate;
        }

        [When("the user sets the HMI \\(Import\\) value to {int} if it is not already {int}")]
        public void ThenTheUserSetsTheHMIImportValueToIfItIsNotAlready(int newHmiImportVal, int currentHmiImportVal)
        {
            defaultRulesForCHEDPPPage?.EnsureHmiImportRateIsZero();
        }

        [When("the user enters {int} for HMI \\(Import\\) value on the Default Rules page")]
        public void WhenTheUserEntersForHMIImportValueOnTheDefaultRulesPage(int hmiImportRate)
        {
            defaultRulesForCHEDPPPage?.SetHmiImportRate(hmiImportRate);
            _scenarioContext["NewHmiImportRate"] = hmiImportRate;
        }

        [When("the user resets the HMI \\(Import\\) rate back to the value noted at the start")]
        public void WhenTheUserResetsTheHMIImportRateBackToTheValueNotedAtTheStart()
        {
            var originalHmiImportRate = (int)_scenarioContext["HmiImportRate"];
            defaultRulesForCHEDPPPage?.SetHmiImportRate(originalHmiImportRate);
        }

        [When("the user clicks the Confirm and send button on the Default Rules page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheDefaultRulesPage()
        {
            defaultRulesForCHEDPPPage?.ClickConfirmAndSendButton();
        }
    }
}
