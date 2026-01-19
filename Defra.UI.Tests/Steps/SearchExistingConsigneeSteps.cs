using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


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
            searchExistingConsigneePage?.ClickSelect(consigneeName);
        }

        [When("the user selects a consignee {string}")]
        public void WhenTheUserSelectsAConsignee(string consigneeName)
        {
            var selectedConsigneeName = searchExistingConsigneePage?.GetSelectedConsigneeName(consigneeName);
            var consigneeAddress = searchExistingConsigneePage?.GetSelectedConsigneeAddress(consigneeName);
            var consigneeCountry = searchExistingConsigneePage?.GetSelectedConsigneeCountry(consigneeName);

            _scenarioContext.Add("ConsigneeName", selectedConsigneeName);
            _scenarioContext.Add("ConsigneeAddress", consigneeAddress);
            _scenarioContext.Add("ConsigneeCountry", consigneeCountry);

            searchExistingConsigneePage?.ClickSelect(consigneeName);
        }

        [When("the user selects the consignee from the address book {string}")]
        public void WhenTheUserSelectsTheConsigneeFromTheAddressBook(string operatorType)
        {
            // Retrieve the operator name that was stored when adding to address book
            var operatorNameKey = $"{operatorType}Name";

            if (!_scenarioContext.ContainsKey(operatorNameKey))
            {
                Assert.Fail($"Operator name for {operatorType} not found in scenario context. Key: {operatorNameKey}");
            }

            var consigneeName = _scenarioContext[operatorNameKey]?.ToString();

            // Just click select - don't update context keys
            searchExistingConsigneePage?.ClickSelect(consigneeName);
        }
    }
}