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

            _scenarioContext["ConsignorName"] = selectedConsignorName;
            _scenarioContext["ConsignorAddress"] = consignorAddress;
            _scenarioContext["ConsignorCountry"] = consignorCountry;
            _scenarioContext["ConsignorDetails"] = searchExistingConsignorPage?.GetSelectedConsignor(consignorName);

            searchExistingConsignorPage?.ClickSelect(consignorName);
        }

        [When("the user selects the consignor or exporter from the address book {string}")]
        public void WhenTheUserSelectsTheConsignorOrExporterFromTheAddressBook(string operatorType)
        {
            var operatorNameKey = $"{operatorType}Name";

            var consignorName = _scenarioContext.ContainsKey(operatorNameKey)
                ? _scenarioContext[operatorNameKey]?.ToString()
                : null;

            Assert.That(consignorName, Is.Not.Null.And.Not.Empty,
                $"Operator name for type '{operatorType}' not found in scenario context (expected key: '{operatorNameKey}')");

            searchExistingConsignorPage?.ClickSelect(consignorName);
        }
    }
}