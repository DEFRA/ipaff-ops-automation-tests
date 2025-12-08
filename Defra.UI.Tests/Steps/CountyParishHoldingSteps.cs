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
    public class CountyParishHoldingSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ICountyParishHoldingPage? countyParishHoldingPage => _objectContainer.IsRegistered<ICountyParishHoldingPage>() ? _objectContainer.Resolve<ICountyParishHoldingPage>() : null;


        public CountyParishHoldingSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Add the County Parish Holding number \\(CPH) page should be displayed")]
        public void ThenTheAddTheCountyParishHoldingNumberCPHPageShouldBeDisplayed()
        {
            Assert.True(countyParishHoldingPage?.IsPageLoaded(), "Add the County Parish Holding number (CPH) page not loaded");
        }

        [When("the user enters the CPH number {string}")]
        public void WhenTheUserEntersTheCPHNumber(string cphNumber)
        {
            countyParishHoldingPage?.EnterCPHNumber(cphNumber);
            _scenarioContext.Add("CPHnumber", cphNumber);
        }
    }
}