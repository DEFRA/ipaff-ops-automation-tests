using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ThereAreRulesInYourCSVFileThatAlreadyExistSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IThereAreRulesInYourCSVFileThatAlreadyExistPage? thereAreRulesInYourCSVFileThatAlreadyExistPage =>
            _objectContainer.IsRegistered<IThereAreRulesInYourCSVFileThatAlreadyExistPage>()
                ? _objectContainer.Resolve<IThereAreRulesInYourCSVFileThatAlreadyExistPage>()
                : null;

        public ThereAreRulesInYourCSVFileThatAlreadyExistSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the There are rules in your CSV file that already exist page should be displayed")]
        public void ThenTheThereAreRulesInYourCSVFileThatAlreadyExistPageShouldBeDisplayed()
        {
            Assert.True(thereAreRulesInYourCSVFileThatAlreadyExistPage?.IsPageLoaded(),
                "There are rules in your CSV file that already exist page is not displayed");
        }

        [When("the user selects Yes to replace existing rules and clicks Continue")]
        public void WhenTheUserSelectsYesToReplaceExistingRulesAndClicksContinue()
        {
            thereAreRulesInYourCSVFileThatAlreadyExistPage?.SelectYesAndClickContinue();
        }
    }
}