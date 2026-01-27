using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SearchExistingTranspoterSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ISearchExistingTranspoterPage? searchExistingTranspoterPage => _objectContainer.IsRegistered<ISearchExistingTranspoterPage>() ? _objectContainer.Resolve<ISearchExistingTranspoterPage>() : null;

        public SearchExistingTranspoterSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Search for an existing transporter page should be displayed")]
        public void ThenTheSearchForAnExistingTransporterPageShouldBeDisplayed()
        {
            Assert.True(searchExistingTranspoterPage?.IsPageLoaded(), "Search for an existing transporter page not loaded");
        }

        [When("the user selects any one of the displayed transporters")]
        public void WhenTheUserSelectsAnyOneOfTheDisplayedTransporters()
        {
            var transporterName = searchExistingTranspoterPage?.GetSelectedTransporterName();
            var transporterAddress = searchExistingTranspoterPage?.GetSelectedTransporterAddress();
            var transporterCountry = searchExistingTranspoterPage?.GetSelectedTransporterCountry();
            var transporterApprovalNumber = searchExistingTranspoterPage?.GetSelectedTransporterApprovalNumber();
            var transporterType = searchExistingTranspoterPage?.GetSelectedTransporterType();

            _scenarioContext.AddOrUpdate("TransporterName", transporterName);
            _scenarioContext.AddOrUpdate("TransporterAddress", transporterAddress);
            _scenarioContext.AddOrUpdate("TransporterCountry", transporterCountry);
            _scenarioContext.AddOrUpdate("TransporterApprovalNumber", transporterApprovalNumber);
            _scenarioContext.AddOrUpdate("TransporterType", transporterType);

            searchExistingTranspoterPage?.ClickSelect();
        }

        [When("the user searches for the transporter from the address book {string}")]
        public void WhenTheUserSearchesForTheTransporterFromTheAddressBook(string operatorType)
        {
            var operatorNameKey = $"{operatorType}Name";

            var transporterName = _scenarioContext.GetFromContext<string>(operatorNameKey);

            Assert.That(transporterName, Is.Not.Null.And.Not.Empty,
                $"Operator name for type '{operatorType}' not found in scenario context (expected key: '{operatorNameKey}')");

            searchExistingTranspoterPage?.SearchForTransporter(transporterName);
        }

        [When("the user selects the transporter from the address book {string}")]
        public void WhenTheUserSelectsTheTransporterFromTheAddressBook(string operatorType)
        {
            var operatorNameKey = $"{operatorType}Name";

            var transporterName = _scenarioContext.GetFromContext<string>(operatorNameKey);

            Assert.That(transporterName, Is.Not.Null.And.Not.Empty,
                $"Operator name for type '{operatorType}' not found in scenario context (expected key: '{operatorNameKey}')");

            // Get the transporter details before clicking select
            var selectedApprovalNumber = searchExistingTranspoterPage?.GetSelectedTransporterApprovalNumber(transporterName);
            var selectedType = searchExistingTranspoterPage?.GetSelectedTransporterType(transporterName);

            // Update scenario context with the selected transporter details for validation
            _scenarioContext.AddOrUpdate("TransporterApprovalNumber", selectedApprovalNumber);
            _scenarioContext.AddOrUpdate("TransporterType",selectedType);

            // Click Select button
            searchExistingTranspoterPage?.ClickSelectForTransporter(transporterName);
        }
    }
}