using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ThereAreRulesInYourCSVFileThatAlreadyExistCHEDDSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IThereAreRulesInYourCSVFileThatAlreadyExistCHEDDPage? thereAreRulesInYourCSVFileThatAlreadyExistCHEDDPage =>
            _objectContainer.IsRegistered<IThereAreRulesInYourCSVFileThatAlreadyExistCHEDDPage>()
                ? _objectContainer.Resolve<IThereAreRulesInYourCSVFileThatAlreadyExistCHEDDPage>()
                : null;

        public ThereAreRulesInYourCSVFileThatAlreadyExistCHEDDSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the There are rules in your CSV file that already exist page should be displayed for CHED-D")]
        public void ThenTheThereAreRulesInYourCSVFileThatAlreadyExistPageShouldBeDisplayedForCHEDD()
        {
            Assert.True(thereAreRulesInYourCSVFileThatAlreadyExistCHEDDPage?.IsPageLoaded(),
                "There are rules in your CSV file that already exist page is not displayed for CHED-D");
        }

        [When("the user selects Yes to replace existing rules and clicks Continue for CHED-D")]
        public void WhenTheUserSelectsYesToReplaceExistingRulesAndClicksContinueForCHEDD()
        {
            thereAreRulesInYourCSVFileThatAlreadyExistCHEDDPage?.SelectYesAndClickContinue();
        }
    }
}