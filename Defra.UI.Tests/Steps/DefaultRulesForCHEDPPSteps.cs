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

        [When("the user sets the default inspection rate for HMI \\(Import) to {int} if it is not already {int}")]
        public void WhenTheUserSetsTheDefaultInspectionRateForHMIImportToIfItIsNotAlready(int newHmiImportVal, int currentHmiImportVal)
        {
            defaultRulesForCHEDPPPage?.EnsureHmiImportRateIs(newHmiImportVal);
        }

        [When("the user sets the default inspection rate for HMI \\(Import) to {int} on the Default Rules page")]
        public void WhenTheUserSetsTheDefaultInspectionRateForHMIImportToOnTheDefaultRulesPage(int hmiImportRate)
        {
            defaultRulesForCHEDPPPage?.SetHmiImportRate(hmiImportRate);
            _scenarioContext["NewHmiImportRate"] = hmiImportRate;
        }

        [When("the user resets the HMI \\(Import) rate to {int} on the Default Rules page")]
        public void WhenTheUserResetsTheHMIImportRateToOnTheDefaultRulesPage(int hmiImportRate)
        {
            defaultRulesForCHEDPPPage?.SetHmiImportRate(hmiImportRate);
        }

        [When("the user resets the default inspection rate for HMI \\(Import) to the value noted at the start")]
        public void WhenTheUserResetsTheDefaultInspectionRateForHMIImportToTheValueNotedAtTheStart()
        {
            var originalHmiImportRate = (int)_scenarioContext["HmiImportRate"];
            defaultRulesForCHEDPPPage?.SetHmiImportRate(originalHmiImportRate);
        }

        [When("the user makes a note of the GMS \\(Export) setting value on the Default Rules page")]
        public void WhenTheUserMakesANoteOfTheGMSExportSettingValueOnTheDefaultRulesPage()
        {
            var gmsExportRate = defaultRulesForCHEDPPPage?.GetGmsExportRate();
            _scenarioContext["GmsExportRate"] = gmsExportRate;
        }

        [When("the user sets the default inspection rate for GMS \\(Export) to {int} if it is not already {int}")]
        public void WhenTheUserSetsTheDefaultInspectionRateForGMSExportToIfItIsNotAlready(int newGmsExportVal, int currentGmsExportVal)
        {
            defaultRulesForCHEDPPPage?.EnsureGmsExportRateIs(newGmsExportVal);
        }

        [When("the user sets the default inspection rate for GMS \\(Export) to {int} on the Default Rules page")]
        public void WhenTheUserSetsTheDefaultInspectionRateForGMSExportToOnTheDefaultRulesPage(int gmsExportRate)
        {
            defaultRulesForCHEDPPPage?.SetGmsExportRate(gmsExportRate);
            _scenarioContext["NewGmsExportRate"] = gmsExportRate;
        }

        [When("the user resets the default inspection rate for GMS \\(Export) to the value noted at the start")]
        public void WhenTheUserResetsTheDefaultInspectionRateForGMSExportToTheValueNotedAtTheStart()
        {
            var originalGmsExportRate = (int)_scenarioContext["GmsExportRate"];
            defaultRulesForCHEDPPPage?.SetGmsExportRate(originalGmsExportRate);
        }

        [When("the user clicks the Confirm and send button on the Default Rules page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheDefaultRulesPage()
        {
            defaultRulesForCHEDPPPage?.ClickConfirmAndSendButton();
        }
    }
}