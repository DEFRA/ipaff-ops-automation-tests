using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.CP
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
            Assert.True(confirmationPage?.VerifyInitialAssessmentPage(),"Initial rist assessment page not loaded");
        }

        [When("the user records the IPAFFS User details and CHED Reference")]
        [Then("the user records the IPAFFS User details and CHED Reference")]
        public void WhenTheUserRecordsTheIPAFFSUserDetailsAndCHEDReference()
        {
            _scenarioContext.Add("CHEDReference", confirmationPage.GetCHEDReference());
            _scenarioContext.Add("CustomsDeclarationReference", confirmationPage.GetCustomsDeclarationReference());
            _scenarioContext.Add("CustomsDocumentCode", confirmationPage.GetCustomsDocumentCode());
        }

        [When("the user logs out of IPAFFS Part {int}")]
        public void WhenTheUserLogsOutOfIPAFFSPart(int partNumber)
        {
            confirmationPage?.SignedOut();
        }

        [Then("the user should be logged out successfully")]
        public void ThenTheUserShouldBeLoggedOutSuccessfully()
        {
            Assert.True(confirmationPage?.VerifySignedOutPage(), "Signed out page not loaded");
        }

        [When("the user clicks Return to your dashboard")]
        public void WhenTheUserClicksReturnToYourDashboard()
        {
            confirmationPage?.ClickReturnToDashboard();
        }
    }
}