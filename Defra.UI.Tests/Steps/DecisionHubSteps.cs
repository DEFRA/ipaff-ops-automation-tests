using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class DecisionHubSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IDecisionHubPage? decisionHubPage => _objectContainer.IsRegistered<IDecisionHubPage>() ? _objectContainer.Resolve<IDecisionHubPage>() : null;


        public DecisionHubSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }


        [Then("the Decision Hub page should be displayed")]
        public void ThenTheDecisionHubPageShouldBeDisplayed()
        {
            Assert.True(decisionHubPage?.IsPageLoaded());
        }

        [When("the user clicks Save and set as in progress")]
        public void WhenTheUserClicksSaveAndSetAsInProgress()
        {
            decisionHubPage?.ClickSaveAndSetAsInProgress();
        }

        [Then("the notification status should change from {string} to {string}")]
        public void ThenTheNotificationStatusShouldChangeFromTo(string stausNew, string statusInProgress)
        {
            decisionHubPage?.VerifyStatusUpdate(stausNew, statusInProgress);
        }

        [When("the user clicks Local reference number link in Record checks")]
        public void WhenTheUserClicksLocalReferenceNumberLinkInRecordChecks()
        {
            decisionHubPage?.ClickLocalReferenceNumberLink();
        }
    }
}