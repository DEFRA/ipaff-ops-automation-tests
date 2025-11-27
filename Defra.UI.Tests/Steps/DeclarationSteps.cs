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
    public class DeclarationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IDeclarationPage? declarationPage => _objectContainer.IsRegistered<IDeclarationPage>() ? _objectContainer.Resolve<IDeclarationPage>() : null;


        public DeclarationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Declaration page should be displayed")]
        public void ThenTheDeclarationPageShouldBeDisplayed()
        {
            Assert.True(declarationPage?.IsPageLoaded(), "Declaration page not loaded");
        }

        [When("the user clicks Submit notification")]
        public void WhenTheUserClicksSubmitNotification()
        {
            declarationPage?.ClickSubmitNotification();
        }

        [Then("the Confirmation page should be displayed with the initial risk assessment")]
        public void ThenTheConfirmationPageShouldBeDisplayedWithTheInitialRiskAssessment()
        {
            Assert.True(declarationPage?.VerifyInitialAssessmentPage(),"Initial rist assessment page not loaded");
        }

        [When("the user records the IPAFFS User details and CHED Reference")]
        public void WhenTheUserRecordsTheIPAFFSUserDetailsAndCHEDReference()
        {
            _scenarioContext.Add("CHEDReference", declarationPage.GetCHEDReference());
            _scenarioContext.Add("CustomsDeclarationReference", declarationPage.GetCustomsDeclarationReference());
            _scenarioContext.Add("CustomsDocumentCode", declarationPage.GetCustomsDocumentCode());
        }

        [Then("the details should be recorded")]
        public void ThenTheDetailsShouldBeRecorded()
        {
            //No Implementation
        }

        [When("the user logs out of IPAFFS Part {int}")]
        public void WhenTheUserLogsOutOfIPAFFSPart(int partNumber)
        {
            declarationPage?.SignedOut();
        }

        [Then("the user should be logged out successfully")]
        public void ThenTheUserShouldBeLoggedOutSuccessfully()
        {
            Assert.True(declarationPage?.VerifySignedOutPage(), "Signed out page not loaded");
        }
    }
}