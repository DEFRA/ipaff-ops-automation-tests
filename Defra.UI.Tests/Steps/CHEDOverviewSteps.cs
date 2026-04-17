using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDOverviewSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICHEDOverviewPage? chedOverviewPage => _objectContainer.IsRegistered<ICHEDOverviewPage>() ? _objectContainer.Resolve<ICHEDOverviewPage>() : null;

        // Required to snapshot window handles before the PDF tab opens, so that
        // VerifyCertificateInNewTab has an accurate baseline to detect the new handle.
        private IYourImportNotificationsPage? importNotificationsPage => _objectContainer.IsRegistered<IYourImportNotificationsPage>() ? _objectContainer.Resolve<IYourImportNotificationsPage>() : null;

        public CHEDOverviewSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the CHED overview page should be displayed")]
        public void ThenTheCHEDOverviewPageShouldBeDisplayed()
        {
            Assert.True(chedOverviewPage?.IsPageLoaded(), "CHED overview page not loaded");
        }

        [Then("the CHED overview page should be displayed for the {string} notification")]
        public void ThenTheCHEDOverviewPageShouldBeDisplayedForTheNotification(string type)
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            var replacementChedReference = _scenarioContext.Get<string>("ReplacementCHEDReference");
            Assert.True(chedOverviewPage?.VerifyCHEDReference(type, chedReference, replacementChedReference));
        }

        [When("the user clicks Raise border notification button")]
        public void WhenTheUserClicksRaiseBorderNotificationButton()
        {
            chedOverviewPage?.ClickRaiseBorderNotification();
        }

        [When("the user clicks Copy as replacement button")]
        public void WhenTheUserClicksCopyAsReplacementButton()
        {
            chedOverviewPage?.ClickCopyAsReplacement();
        }

        [Then("link should be displayed as Replaced by along with {string} notification number")]
        public void ThenLinkShouldBeDisplayedAsReplacedByAlongWithNotificationNumber(string type)
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            var replacementChedReference = _scenarioContext.Get<string>("ReplacementCHEDReference");
            Assert.True(chedOverviewPage?.VerifyReplacedByLink(type, chedReference, replacementChedReference));
        }

        [When("the user clicks Replaced by link")]
        public void WhenTheUserClicksReplacedByLink()
        {
            chedOverviewPage?.ClickReplacedByLink();
        }

        [When("the user clicks Clear all in consinments requiring control page")]
        public void WhenTheUserClicksClearAll()
        {
            chedOverviewPage?.ClickClearAll();
        }

        [Then("Show CHED button is displayed")]
        public void TheShowChedButtonIsDisplayed()
        {
            Assert.IsTrue(chedOverviewPage?.VerifyShowChedButton(), "The Show CHED button is not present");
        }

        /// <summary>
        /// Snapshots window handles before opening the PDF so that VerifyCertificateInNewTab
        /// can correctly identify the new tab even when multiple tabs are already open
        /// (e.g. the Dynamics hand-off flow where Browser 2 already has 2 tabs).
        /// window.open in ClickShowChed is synchronous — the tab exists in WindowHandles
        /// before VerifyCertificateInNewTab runs — so the snapshot must be taken here,
        /// before the call to ClickShowChed.
        /// </summary>
        [When("the user clicks on the Show CHED button")]
        public void WhenTheUserClicksOnTheShowChedButton()
        {
            importNotificationsPage?.RecordHandlesBeforePdfOpen();
            chedOverviewPage?.ClickShowChed();
        }

        [Then("the user verifies {string} tab in CHED Overview page")]
        public void ThenTheUserVerifiesTabInChedOverviewPage(string tabName)
        {
            Assert.IsTrue(chedOverviewPage?.VerifyTab(tabName), "The " + tabName + " is not present");
        }

        [Then("the user verifies the value is present for {string}")]
        public void ThenTheUserVerifiesValueIsPresent(string fieldName)
        {
            Assert.IsTrue(chedOverviewPage?.IsFieldValuePresent(fieldName), "The field value for " + fieldName + " is not present");
        }

        [Then("the user verifies the value is present for {string} under {string}")]
        public void ThenTheUserVerifiesValueIsPresent(string fieldName, string sectionName)
        {
            Assert.IsTrue(chedOverviewPage?.IsFieldValuePresent(fieldName, sectionName), "The field value for " + fieldName + " is not present");
        }

        [Then("the user verifies the value is present for {string} in {string} column")]
        public void ThenTheUserVerifiesValueIsPresentInColumn(string fieldName, string column)
        {
            Assert.IsTrue(chedOverviewPage?.IsFieldValuePresentInTable(fieldName, column), "The field value for " + fieldName + " is not present in " + column);
        }

        [When("the user switches to {string} tab in CHED Overview page")]
        public void WhenTheUserSwitchesToTabInChedOverviewPage(string tabName)
        {
            chedOverviewPage?.SwitchTab(tabName);
        }

        [When("the user clicks on Record control button")]
        public void WhenTheUserClicksOnRecordControlButton()
        {
            chedOverviewPage?.ClickRecordControlButton();
        }

        [Then("verifies Risk decision PHSI is set to {string}")]
        public void ThenVerifiesRiskDecisionPHSIIsSetTo(string decision)
        {
            Assert.True(chedOverviewPage?.VerifyRiskDecisionPHSI(decision));
        }

        [Then("verifies Document check is set to {string}")]
        public void ThenVerifiesDocumentCheckIsSetTo(string status)
        {
            Assert.True(chedOverviewPage?.VerifyDocumentCheck(status));
        }

        [Then("verifies Risk decision HMI is set to {string}")]
        public void ThenVerifiesRiskDecisionHMIIsSetTo(string decision)
        {
            Assert.True(chedOverviewPage?.VerifyRiskDecisionHMI(decision));
        }

        [Then("verifies {string} field is set to {string}")]
        public void ThenVerifiesFieldIsSetTo(string fieldName, string status)
        {
            Assert.True(chedOverviewPage?.VerifyDecisionRecordedBy(fieldName, status));
        }

        [When("the user clicks Record control button")]
        public void WhenTheUserClicksRecordControlButton()
        {
            chedOverviewPage?.ClickRecordControl();
        }

        [Then("the notification status is {string} for the notification created in IPAFFS")]
        public void ThenTheNotificationStatusIsForTheNotificationCreatedInIPAFFS(string expectedStatus)
        {
            var chedReference = _scenarioContext.ContainsKey("CHEDReference")
                ? _scenarioContext.Get<string>("CHEDReference")
                : null;

            Assert.That(chedReference, Is.Not.Null.And.Not.Empty,
                "CHEDReference was not found in scenario context — ensure the CHED reference was recorded earlier in the scenario.");

            Assert.True(
                chedOverviewPage?.VerifyNotificationStatus(expectedStatus, chedReference),
                $"Expected CHED Overview status to be '{expectedStatus}' for reference '{chedReference}'.");
        }

        [Then("all the checks are {string} or {string} showing {int} of {int}")]
        public void ThenAllTheChecksAreOrShowingCount(string expectedDecision, string alternateDecision, int shown, int total)
        {
            Assert.True(
                chedOverviewPage?.VerifyChecksCount(shown, total),
                $"Expected the checks count to show '{shown} of {total}' but a different count was displayed.");

            var result = chedOverviewPage?.VerifyAllCheckDecisions(expectedDecision, alternateDecision);

            Assert.True(
                result?.AllMatch,
                $"Expected all {result?.Total} check decision tags to be '{expectedDecision}' or '{alternateDecision}' " +
                $"but {result?.NonMatchingValues.Count} were not. " +
                $"Non-matching values: [{string.Join(", ", result?.NonMatchingValues.Select(v => $"'{v}'") ?? [])}].");
        }
    }
}