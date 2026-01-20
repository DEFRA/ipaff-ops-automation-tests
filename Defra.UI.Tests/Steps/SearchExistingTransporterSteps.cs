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

            _scenarioContext.Add("TransporterName", transporterName);
            _scenarioContext.Add("TransporterAddress", transporterAddress);
            _scenarioContext.Add("TransporterCountry", transporterCountry);
            _scenarioContext.Add("TransporterApprovalNumber", transporterApprovalNumber);
            _scenarioContext.Add("TransporterType", transporterType);

            searchExistingTranspoterPage?.ClickSelect();
        }

        [When("the user searches for the transporter from the address book {string}")]
        public void WhenTheUserSearchesForTheTransporterFromTheAddressBook(string operatorType)
        {
            // Retrieve the operator name that was stored when adding to address book
            var operatorNameKey = $"{operatorType}Name";

            if (!_scenarioContext.ContainsKey(operatorNameKey))
            {
                Assert.Fail($"Operator name for {operatorType} not found in scenario context. Key: {operatorNameKey}");
            }

            var transporterName = _scenarioContext[operatorNameKey]?.ToString();

            // Search for the transporter by name
            searchExistingTranspoterPage?.SearchForTransporter(transporterName);
        }

        [When("the user selects the transporter from the address book {string}")]
        public void WhenTheUserSelectsTheTransporterFromTheAddressBook(string operatorType)
        {
            // Retrieve the operator name that was stored when adding to address book
            var operatorNameKey = $"{operatorType}Name";

            if (!_scenarioContext.ContainsKey(operatorNameKey))
            {
                Assert.Fail($"Operator name for {operatorType} not found in scenario context. Key: {operatorNameKey}");
            }

            var transporterName = _scenarioContext[operatorNameKey]?.ToString();

            // Get the transporter details before clicking select
            var selectedApprovalNumber = searchExistingTranspoterPage?.GetSelectedTransporterApprovalNumber(transporterName);
            var selectedType = searchExistingTranspoterPage?.GetSelectedTransporterType(transporterName);

            // Update scenario context with the selected transporter details for validation
            _scenarioContext["TransporterApprovalNumber"] = selectedApprovalNumber;
            _scenarioContext["TransporterType"] = selectedType;

            // Click Select button
            searchExistingTranspoterPage?.ClickSelectForTransporter(transporterName);
        }
    }
}