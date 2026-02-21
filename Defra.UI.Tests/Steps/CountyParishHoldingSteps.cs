using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
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
            _scenarioContext["CPHnumber"] = cphNumber;
        }

        [Then("the CPH number should not be copied from the original notification")]
        public void ThenTheCPHNumberShouldNotBeCopiedFromTheOriginalNotification()
        {
            string actualCPHNumber = countyParishHoldingPage?.GetCPHNumber ?? string.Empty;

            Assert.That(actualCPHNumber, Is.Empty,
                $"CPH Number should not be copied from the original notification, but found '{actualCPHNumber}'");           
        }
    }
}