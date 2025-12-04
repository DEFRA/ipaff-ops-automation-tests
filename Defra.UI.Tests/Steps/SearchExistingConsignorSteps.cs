using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class SearchExistingConsignorSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
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
        
        [When("the user selects any one of the displayed consignors or exporters")]
        public void WhenTheUserSelectsAnyOneOfTheDisplayedConsignorsOrExporters()
        {
            var consignorName = searchExistingConsignorPage?.GetSelectedConsignorName();
            var consignorAddress = searchExistingConsignorPage?.GetSelectedConsignorAddress();
            var consignorCountry = searchExistingConsignorPage?.GetSelectedConsignorCountry();

            _scenarioContext.Add("ConsignorName", consignorName);
            _scenarioContext.Add("ConsignorAddress", consignorAddress);
            _scenarioContext.Add("ConsignorCountry", consignorCountry);

            searchExistingConsignorPage?.ClickSelect();
        }
    }
}