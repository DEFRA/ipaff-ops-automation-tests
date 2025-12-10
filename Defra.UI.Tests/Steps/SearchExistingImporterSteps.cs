using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SearchExistingImporterSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISearchExistingImporterPage? searchExistingImporterPage => _objectContainer.IsRegistered<ISearchExistingImporterPage>() ? _objectContainer.Resolve<ISearchExistingImporterPage>() : null;

        public SearchExistingImporterSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Search for an existing importer page should be displayed")]
        public void ThenTheSearchForAnExistingImporterPageShouldBeDisplayed()
        {
            Assert.True(searchExistingImporterPage?.IsPageLoaded(), "Traders Search for an existing importer page not loaded");
        }

        [When("the user selects an importer {string} with a UK country")]
        public void WhenTheUserSelectsAnImporterWithAUKCountry(string importer)
        {
            var consigneeName = searchExistingImporterPage?.GetSelectedImporterName();
            var consigneeAddress = searchExistingImporterPage?.GetSelectedImporterAddress();
            var consigneeCountry = searchExistingImporterPage?.GetSelectedImporterCountry();

            _scenarioContext.Add("ImporterName", consigneeName);
            _scenarioContext.Add("ImporterAddress", consigneeAddress);
            _scenarioContext.Add("ImporterCountry", consigneeCountry);

            _scenarioContext.Add("ImporterDetails", searchExistingImporterPage?.GetSelectedImporter());

            searchExistingImporterPage?.ClickSelect(importer);
        }
    }
}