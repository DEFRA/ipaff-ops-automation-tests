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
    public class SearchExistingControlledDestinationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ISearchExistingControlledDestinationPage? searchExistingControlledDestinationPage => _objectContainer.IsRegistered<ISearchExistingControlledDestinationPage>() ? _objectContainer.Resolve<ISearchExistingControlledDestinationPage>() : null;

        public SearchExistingControlledDestinationSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Search for an existing controlled destination page should be displayed")]
        public void ThenTheSearchForAnExistingControlledDestinationPageShouldBeDisplayed()
        {
            Assert.True(searchExistingControlledDestinationPage?.IsPageLoaded(), "Search for an existing controlled destination page not loaded");
        }

        [When("the user selects a controlled destination")]
        public void WhenTheUserSelectsAControlledDestination()
        {
            var controlledDestinationName = searchExistingControlledDestinationPage?.GetSelectedControlledDestinationName();
            var controlledDestinationAddress = searchExistingControlledDestinationPage?.GetSelectedControlledDestinationAddress();
            var controlledDestinationType = searchExistingControlledDestinationPage?.GetSelectedControlledDestinationType();
            var controlledDestinationApprovalNumber = searchExistingControlledDestinationPage?.GetSelectedControlledDestinationApprovalNumber();

            _scenarioContext.AddOrUpdate("ControlledDestinationName", controlledDestinationName);
            _scenarioContext.AddOrUpdate("ControlledDestinationAddress", controlledDestinationAddress);
            _scenarioContext.AddOrUpdate("ControlledDestinationType", controlledDestinationType);
            _scenarioContext.AddOrUpdate("ControlledDestinationApprovalNumber", controlledDestinationApprovalNumber);

            searchExistingControlledDestinationPage?.ClickSelect();
        }
    }
}