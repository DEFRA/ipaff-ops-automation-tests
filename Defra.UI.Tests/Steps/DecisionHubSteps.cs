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
        private readonly ScenarioContext _scenarioContext;

        private IDecisionHubPage? decisionHubPage => _objectContainer.IsRegistered<IDecisionHubPage>() ? _objectContainer.Resolve<IDecisionHubPage>() : null;

        public DecisionHubSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Decision Hub page should be displayed")]
        public void ThenTheDecisionHubPageShouldBeDisplayed()
        {
            Assert.True(decisionHubPage?.IsPageLoaded());
        }

        [Then("the user can see the Decision Hub for the notification created in IPAFFS")]
        public void ThenTheUserCanSeeTheDecisionHubForTheNotificationCreatedInIPAFFS()
        {
            var chedReference = _scenarioContext.ContainsKey("CHEDReference")
                ? _scenarioContext.Get<string>("CHEDReference")
                : null;

            Assert.That(chedReference, Is.Not.Null.And.Not.Empty,
                "CHEDReference was not found in the scenario context — ensure 'When the user records the IPAFFS User details and CHED Reference' ran before this step.");

            Assert.True(
                decisionHubPage?.IsPageLoadedForChedReference(chedReference),
                $"Decision Hub page was not displayed for CHED reference '{chedReference}'.");
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

        [Then("the user can see {int} hypertext links {string} {string}")]
        public void ThenTheUserCanSeeHypertextLinks(int count, string firstLink, string secondLink)
        {
            Assert.True(
                decisionHubPage?.VerifyRecordChecksLinksAreClickable(firstLink, secondLink),
                $"Expected both '{firstLink}' and '{secondLink}' to be displayed as clickable buttons. " +
                $"If '{secondLink}' is rendered as plain text rather than a button, the page is not in the expected state.");
        }

        [When("the user clicks Return to work order")]
        public void WhenTheUserClicksReturnToWorkOrder()
        {
            decisionHubPage?.ClickReturnToWorkOrder();
        }

        [Then("the {string} status is {string}")]
        public void ThenTheRecordChecksStatusIs(string checkName, string expectedStatus)
        {
            Assert.True(
                decisionHubPage?.VerifyRecordChecksStatus(checkName, expectedStatus),
                $"Expected '{checkName}' status to be '{expectedStatus}'.");
        }

        [When("the user clicks on the Record checks link {string}")]
        public void WhenTheUserClicksOnTheRecordChecksLink(string linkName)
        {
            decisionHubPage?.ClickRecordChecksLink(linkName);
        }
    }
}