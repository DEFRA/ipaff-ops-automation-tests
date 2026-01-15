using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SearchExistingConsignorSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISearchExistingConsignorPage? searchExistingConsignorPage => _objectContainer.IsRegistered<ISearchExistingConsignorPage>() ? _objectContainer.Resolve<ISearchExistingConsignorPage>() : null;

        public SearchExistingConsignorSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Search for an existing consignor or exporter page should be displayed")]
        public void ThenTheSearchForAnExistingConsignorOrExporterPageShouldBeDisplayed()
        {
            Assert.True(searchExistingConsignorPage?.IsPageLoaded(), "Traders Search for an existing consignor or exporter page not loaded");
        }

        [When("the user selects one of the displayed consignors or exporters {string}")]
        public void WhenTheUserSelectsOneOfTheDisplayedConsignorsOrExporters(string consignorName)
        {
            _scenarioContext["ConsignorDetails"] = searchExistingConsignorPage?.GetSelectedConsignor(consignorName);
            searchExistingConsignorPage?.ClickSelect(consignorName);
        }

        [When("the user selects a consignor or exporter {string}")]
        public void WhenTheUserSelectsAConsignorOrExporter(string consignorName)
        {
            var selectedConsignorName = searchExistingConsignorPage?.GetSelectedConsignorName(consignorName);
            var consignorAddress = searchExistingConsignorPage?.GetSelectedConsignorAddress(consignorName);
            var consignorCountry = searchExistingConsignorPage?.GetSelectedConsignorCountry(consignorName);

            _scenarioContext.Add("ConsignorName", selectedConsignorName);
            _scenarioContext.Add("ConsignorAddress", consignorAddress);
            _scenarioContext.Add("ConsignorCountry", consignorCountry);

            searchExistingConsignorPage?.ClickSelect(consignorName);
        }

        [When("the user selects the consignor or exporter from the address book {string}")]
        public void WhenTheUserSelectsTheConsignorOrExporterFromTheAddressBook(string operatorType)
        {
            // Retrieve the operator name that was stored when adding to address book
            var operatorNameKey = $"{operatorType}Name";

            if (!_scenarioContext.ContainsKey(operatorNameKey))
            {
                Assert.Fail($"Operator name for {operatorType} not found in scenario context. Key: {operatorNameKey}");
            }

            var consignorName = _scenarioContext[operatorNameKey]?.ToString();

            // Just click select - don't update context keys
            searchExistingConsignorPage?.ClickSelect(consignorName);

            Console.WriteLine($"[ADDRESS BOOK] Selected {operatorType} as consignor: {consignorName}");
        }
    }
}