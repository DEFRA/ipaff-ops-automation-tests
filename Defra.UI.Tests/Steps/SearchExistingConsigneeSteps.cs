using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SearchExistingConsigneeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISearchExistingConsigneePage? searchExistingConsigneePage => _objectContainer.IsRegistered<ISearchExistingConsigneePage>() ? _objectContainer.Resolve<ISearchExistingConsigneePage>() : null;

        public SearchExistingConsigneeSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Search for an existing consignee page should be displayed")]
        public void ThenTheSearchForAnExistingConsigneePageShouldBeDisplayed()
        {
            Assert.True(searchExistingConsigneePage?.IsPageLoaded(), "Traders Search for an existing consignee page not loaded");
        }

        [When("the user selects a consignee {string} with a UK country")]
        public void WhenTheUserSelectsAConsigneeWithAUKCountry(string consigneeName)
        {
            _scenarioContext["ConsigneeDetails"] = searchExistingConsigneePage?.GetSelectedConsignee(consigneeName);
            _scenarioContext["ConsigneeName"] = searchExistingConsigneePage?.GetSelectedConsigneeName(consigneeName);
            _scenarioContext["ConsigneeAddress"] = searchExistingConsigneePage?.GetSelectedConsigneeAddress(consigneeName);
            _scenarioContext["ConsigneeCountry"] = searchExistingConsigneePage?.GetSelectedConsigneeCountry(consigneeName);

            searchExistingConsigneePage?.ClickSelect(consigneeName);
        }

        [When("the user selects a consignee {string}")]
        public void WhenTheUserSelectsAConsignee(string consigneeName)
        {
            var selectedConsigneeName = searchExistingConsigneePage?.GetSelectedConsigneeName(consigneeName);
            var consigneeAddress = searchExistingConsigneePage?.GetSelectedConsigneeAddress(consigneeName);
            var consigneeCountry = searchExistingConsigneePage?.GetSelectedConsigneeCountry(consigneeName);

            _scenarioContext["ConsigneeName"] = selectedConsigneeName;
            _scenarioContext["ConsigneeAddress"] = consigneeAddress;
            _scenarioContext["ConsigneeCountry"] = consigneeCountry;
            _scenarioContext["ConsigneeDetails"] = searchExistingConsigneePage?.GetSelectedConsignee(consigneeName);

            searchExistingConsigneePage?.ClickSelect(consigneeName);
        }

        [When("the user selects the consignee from the address book {string}")]
        public void WhenTheUserSelectsTheConsigneeFromTheAddressBook(string operatorType)
        {
            var operatorNameKey = $"{operatorType}Name";

            var consigneeName = _scenarioContext.ContainsKey(operatorNameKey)
                ? _scenarioContext[operatorNameKey]?.ToString()
                : null;

            Assert.That(consigneeName, Is.Not.Null.And.Not.Empty,
                $"Operator name for type '{operatorType}' not found in scenario context (expected key: '{operatorNameKey}')");

            searchExistingConsigneePage?.ClickSelect(consigneeName);
        }
    }
}