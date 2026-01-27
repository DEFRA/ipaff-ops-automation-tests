using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ConfirmationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IConfirmationPage? confirmationPage => _objectContainer.IsRegistered<IConfirmationPage>() ? _objectContainer.Resolve<IConfirmationPage>() : null;


        public ConfirmationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Confirmation page should be displayed with the initial risk assessment")]
        public void ThenTheConfirmationPageShouldBeDisplayedWithTheInitialRiskAssessment()
        {
            Assert.True(confirmationPage?.VerifyInitialAssessmentPage(), "Initial rist assessment page not loaded");
        }

        [When("the user records the IPAFFS User details and CHED Reference")]
        [Then("the user records the IPAFFS User details and CHED Reference")]
        public void WhenTheUserRecordsTheIPAFFSUserDetailsAndCHEDReference()
        {
            _scenarioContext["CHEDReference"] = confirmationPage.GetCHEDReference();
            _scenarioContext["CustomsDeclarationReference"] = confirmationPage.GetCustomsDeclarationReference();
            _scenarioContext["CustomsDocumentCode"] = confirmationPage.GetCustomsDocumentCode();
        }

        [When("the user clicks Return to your dashboard")]
        [When("the user clicks return to your dashboard link")]
        public void WhenTheUserClicksReturnToYourDashboard()
        {
            confirmationPage?.ClickReturnToDashboard();
        }

        [Then("the details should be recorded")]
        public void ThenTheDetailsShouldBeRecorded()
        {
            //No Implementation
        }

        [Then("the user verified the banner message {string}")]
        public void ThenTheUserVerifiedTheBannerMessage(string message)
        {
            Assert.True(confirmationPage?.VerifyBannerMessage(message), $"Banner doesn't contain a message '{message}'");
        }

        [When("the user clicks Return to your dashboard link")]
        public void WhenTheUserClicksReturnToYourDashboardLink()
        {
            confirmationPage?.ClickReturnToDashboardLink();
        }
    }
}