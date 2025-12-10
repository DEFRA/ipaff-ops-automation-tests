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
            _scenarioContext.Add("CHEDReference", confirmationPage.GetCHEDReference());
            _scenarioContext.Add("CustomsDeclarationReference", confirmationPage.GetCustomsDeclarationReference());
            _scenarioContext.Add("CustomsDocumentCode", confirmationPage.GetCustomsDocumentCode());
        }
       
        [When("the user clicks Return to your dashboard")]
        public void WhenTheUserClicksReturnToYourDashboard()
        {
            confirmationPage?.ClickReturnToDashboard();
        }

        [Then("the details should be recorded")]
        public void ThenTheDetailsShouldBeRecorded()
        {
            //No Implementation
        }
    }
}