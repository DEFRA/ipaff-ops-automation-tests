using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
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
            decisionHubPage?.ClickLocalRefNumLink();
        }

        [When("the user clicks Seal numbers link")]
        public void WhenTheUserClicksSealNumbersLink()
        {
            decisionHubPage?.ClickSealNumbersLink();
        }

        [When("the user clicks Laboratory tests link")]
        public void WhenTheUserClicksLaboratoryTestsLink()
        {
            decisionHubPage?.ClickLaboratoryTestsLink();
        }

        [When("the user clicks Decision link")]
        public void WhenTheUserClicksDecisionLink()
        {
            decisionHubPage?.ClickDecisionLink();
        }
        
        [When("the user clicks Review And Submit link")]
        public void WhenTheUserClicksReviewAndSubmitLink()
        {
            decisionHubPage?.ClickReviewAndSubmitLink();
        }

        [When("the user clicks override the risk decision")]
        public void WhenTheUserClicksOverrideTheRiskDecision()
        {
            decisionHubPage?.ClickOverrideRiskDecisionLink();
        }

        [Then("the user verifies {string} box appears in the page")]
        public void ThenBoxAppearsUnderTheDecisionHubScreenTitle(string msgboxTitle)
        {
            Assert.True(decisionHubPage?.VerifyInspectionRequiredBox(msgboxTitle));
        }

        [Then("the text {string} is displayed")]
        public void ThenTheTextIsDisplayed(string message)
        {
            Assert.True(decisionHubPage?.VerifyInspectionRequiredMessage(message));
        }

        [When("the user clicks Checks link in Record checks")]
        public void WhenTheUserClicksChecksLinkInRecordChecks()
        {
            decisionHubPage?.ClickChecksLink();
        }

        [When("the user clicks View notification of consignment")]
        public void WhenTheUserClicksViewNotificationOfConsignment()
        {
            decisionHubPage?.ClickViewNotificationOfConsignment();
        }

        [When("the user clicks on Attachments button on Decision Hub page")]
        public void WhenTheUserClicksOnAttachmentsButtonOnDecisionHubPage()
        {
            decisionHubPage?.ClickAttachmentsButton();
        }

    }
}