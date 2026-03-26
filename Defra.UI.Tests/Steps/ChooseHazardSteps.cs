using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class ChooseHazardSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IChooseHazardPage? chooseHazardPage =>
            _objectContainer.IsRegistered<IChooseHazardPage>()
                ? _objectContainer.Resolve<IChooseHazardPage>()
                : null;

        public ChooseHazardSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Choose a hazard screen should be displayed")]
        public void ThenTheChooseAHazardScreenShouldBeDisplayed()
        {
            Assert.True(
                chooseHazardPage?.IsPageLoaded(),
                "Choose a hazard page is not loaded");
        }

        [When("the user selects the hazard category {string} and subcategory {string} and clicks Search")]
        public void WhenTheUserSelectsTheHazardCategoryAndSubcategoryAndClicksSearch(string category, string subcategory)
        {
            if (!string.IsNullOrWhiteSpace(category))
                chooseHazardPage?.SelectHazardCategory(category);

            if (!string.IsNullOrWhiteSpace(subcategory))
                chooseHazardPage?.SelectHazardSubcategory(subcategory);

            chooseHazardPage?.ClickSearch();
        }

        [Then("the list of laboratory tests are displayed tests for the hazard subcategory {string}")]
        public void ThenTheListOfLaboratoryTestsAreDisplayedTestsForTheHazardSubcategory(string subcategory)
        {
            Assert.True(
                chooseHazardPage?.AreAllResultsForSubcategory(subcategory),
                $"Not all laboratory test results have the subcategory '{subcategory}'");
        }

        [When("the user clicks Select for the Laboratory test {string} from the hazards table")]
        public void WhenTheUserClicksSelectForTheLaboratoryTestFromTheHazardsTable(string labTestName)
        {
            chooseHazardPage?.SelectHazardByLabTestName(labTestName);
            _scenarioContext["LaboratoryTestName"] = labTestName;
            _scenarioContext["HazardName"] = labTestName;
        }
    }
}