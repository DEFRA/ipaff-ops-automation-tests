using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class SearchExistingConsigneeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISearchExistingConsigneePage? searchExistingConsigneePage => _objectContainer.IsRegistered<ISearchExistingConsigneePage>() ? _objectContainer.Resolve<ISearchExistingConsigneePage>() : null;


        public SearchExistingConsigneeSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
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
    }
}