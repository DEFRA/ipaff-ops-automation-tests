using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class TransporterSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ITransporterPage? transporterPage => _objectContainer.IsRegistered<ITransporterPage>() ? _objectContainer.Resolve<ITransporterPage>() : null;


        public TransporterSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Transporter page should be displayed")]
        public void ThenTheTransporterPageShouldBeDisplayed()
        {
            Assert.True(transporterPage?.IsPageLoaded(), "Transporter page not loaded");
        }

        [When("the user clicks Add a transporter")]
        public void WhenTheUserClicksAddATransporter()
        {
            transporterPage?.ClickAddTransporter();
        }

        [Then("the Transporter should be copied from the original notification")]
        [Then("the chosen transporter should be displayed on the Transporter page")]
        public void ThenTheChosenTransporterShouldBeDisplayedOnTheTransporterPage()
        {
            var transporterName = _scenarioContext.Get<string>("TransporterName");
            var transporterAddress = _scenarioContext.Get<string>("TransporterAddress");
            var transporterCountry = _scenarioContext.Get<string>("TransporterCountry");
            var transporterApprovalNumber = _scenarioContext.Get<string>("TransporterApprovalNumber");
            var transporterType = _scenarioContext.Get<string>("TransporterType");

            Assert.True(transporterPage?.VerifySelectedTransporter(transporterName, transporterAddress, transporterCountry, transporterApprovalNumber, transporterType),
                        "Transporter details do not match");
        }

        [When("the user clicks Save and continue in Transporter page")]
        public void WhenTheUserClicksSaveAndContinueInTransporterPage()
        {
            transporterPage?.ClickSaveAndContinue();
        }

        [When("the user clicks Save and return to hub in Transporter page")]
        public void WhenTheUserClicksSaveAndReturnToHubInTransporterPage()
        {
            transporterPage?.ClickSaveAndReturnToHub();
        }

        [When("the user clicks on Change link next to Transporter")]
        public void WhenTheUserClicksOnChangeLinkNextToTransporter()
        {
            transporterPage?.ClickChangeTransporter();
        }        
    
        [Then("the chosen transporter from the address book should be displayed on the Transporter page {string}")]
        public void ThenTheChosenTransporterFromTheAddressBookShouldBeDisplayedOnTheTransporterPage(string operatorType)
        {
            // Get the ORIGINAL operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();
            var expectedApprovalNumber = _scenarioContext["TransporterApprovalNumber"]?.ToString();
            var expectedType = _scenarioContext["TransporterType"]?.ToString();

            // Verify using existing page method
            var isDisplayed = transporterPage?.VerifySelectedTransporter(expectedName, expectedAddress, expectedCountry, expectedApprovalNumber, expectedType);

            Assert.IsTrue(isDisplayed,
                $"Transporter from address book ({operatorType}) not displayed correctly. Expected: {expectedName}, {expectedAddress}, {expectedCountry}, {expectedApprovalNumber}, {expectedType}");
        }
    }
}