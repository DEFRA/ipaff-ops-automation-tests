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

        [When("the user selects a consignee with a UK country")]
        public void WhenTheUserSelectsAConsigneeWithAUKCountry()
        {
            _scenarioContext.Add("ConsigneeDetails", searchExistingConsigneePage.GetSelectedConsignee());
            searchExistingConsigneePage?.ClickSelect();
        }

        [When("the user selects a consignee")]
        public void WhenTheUserSelectsAConsignee()
        {
            var consigneeName = searchExistingConsigneePage?.GetSelectedConsigneeName();
            var consigneeAddress = searchExistingConsigneePage?.GetSelectedConsigneeAddress();
            var consigneeCountry = searchExistingConsigneePage?.GetSelectedConsigneeCountry();

            _scenarioContext.Add("ConsigneeName", consigneeName);
            _scenarioContext.Add("ConsigneeAddress", consigneeAddress);
            _scenarioContext.Add("ConsigneeCountry", consigneeCountry);

            searchExistingConsigneePage?.ClickSelect();
        }
    }
}