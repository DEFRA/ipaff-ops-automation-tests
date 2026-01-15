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
        public void WhenTheUserSelectsAnImporterWithAUKCountry(string importerName)
        {
            var selectedImporterName = searchExistingImporterPage?.GetSelectedImporterName(importerName);
            var importerAddress = searchExistingImporterPage?.GetSelectedImporterAddress(importerName);
            var importerCountry = searchExistingImporterPage?.GetSelectedImporterCountry(importerName);

            _scenarioContext["ImporterName"] = selectedImporterName;
            _scenarioContext["ImporterAddress"] = importerAddress;
            _scenarioContext["ImporterCountry"] = importerCountry;

            _scenarioContext["ImporterDetails"] = searchExistingImporterPage?.GetSelectedImporter(importerName);

            searchExistingImporterPage?.ClickSelect(importerName);
        }

        [When("the user selects the importer from the address book {string}")]
        public void WhenTheUserSelectsTheImporterFromTheAddressBook(string operatorType)
        {
            // Retrieve the operator name that was stored when adding to address book
            var operatorNameKey = $"{operatorType}Name";

            if (!_scenarioContext.ContainsKey(operatorNameKey))
            {
                Assert.Fail($"Operator name for {operatorType} not found in scenario context. Key: {operatorNameKey}");
            }

            var importerName = _scenarioContext[operatorNameKey]?.ToString();

            // Just click select - don't update context keys
            searchExistingImporterPage?.ClickSelect(importerName);

            Console.WriteLine($"[ADDRESS BOOK] Selected {operatorType} as importer: {importerName}");
        }
    }
}