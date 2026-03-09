using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class InspectorImportNotificationsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IInspectorImportNotificationsPage? inspectorImportNotificationsPage => _objectContainer.IsRegistered<IInspectorImportNotificationsPage>() ? _objectContainer.Resolve<IInspectorImportNotificationsPage>() : null;


        public InspectorImportNotificationsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then ("the Import notifications dashboard page should be displayed")]
        [Then("the user should be logged into Import notifications page")]
        public void ThenTheUserShouldBeLoggedIntoImportNotificationsPage()
        {
            Assert.True(inspectorImportNotificationsPage?.IsPageLoaded());
        }

        [When("the user searches for the original notification")]
        [When("the user searches for the CHED number")]
        [When("user searches for the import notification after decision submission")]
        [When("the user searches for the newly created notification on the Import notifications page")]
        public void WhenTheUserSearchesForTheNewlyCreatedNotificationOnTheImportNotificationsPage()
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            inspectorImportNotificationsPage?.SearchForChed(chedRef);
        }

        [When("the user clicks the notification found with status {string}")]
        [Then("the user clicks the notification found with status {string}")]
        [Then("the user searches for the CHED D notification that was recently submitted")]
        public void ThenTheNotificationShouldBeFoundWithStatus(string status)
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            inspectorImportNotificationsPage?.VerifyNotificationStatusAndClick(chedRef, status);
        }

        [Then("the notification is displayed on the inspector dashboard")]
        public void ThenTheNotificationIsDisplayedOnTheInspectorDashboard()
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            inspectorImportNotificationsPage?.SearchForChed(chedRef);
            Assert.True(inspectorImportNotificationsPage?.VerifyNotificationIsPresent(chedRef));
        }

        [Then("the notification should be present in the list of part {int} dashboard")]
        public void ThenTheNotificationShouldBePresentInTheListOfPartDashboard(int p0)
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            inspectorImportNotificationsPage?.SearchForChed(chedRef);
            Assert.True(inspectorImportNotificationsPage?.VerifyNotificationIsPresent(chedRef));
        }

        [When("the user clicks View CHED link")]
        public void WhenTheUserClicksViewCHEDLink()
        {
            inspectorImportNotificationsPage?.ClickViewCHED();
        }

        [When("the user clicks Record control in Dashboard page")]
        public void WhenTheUserClicksRecordControlInDashboardPage()
        {
            inspectorImportNotificationsPage?.ClickRecordControl();
        }

        [When("the user clicks Create notification in Dashboard page header")]
        public void WhenTheUserClicksCreateNotificationInDashboardPageHeader()
        {
            inspectorImportNotificationsPage?.ClickCreateNotification();
        }

        [When("the user clicks Record decision from the header")]
        public void WhenTheUserClicksRecordDecisionFromTheHeader()
        {
            inspectorImportNotificationsPage?.ClickRecordDecision();
        }

        [Then("the {string} notification should be displayed with status {string}")]
        public void ThenTheNotificationShouldBeDisplayedWithStatus(string type, string status)
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            var replacementChedReference = _scenarioContext.Get<string>("ReplacementCHEDReference");
            Assert.True(inspectorImportNotificationsPage?.VerifyNotificationIsPresentWithStatus(type, chedRef, replacementChedReference, status));
        }

        [When("the user searches for the replacement notification")]
        public void WhenTheUserSearchesForTheReplacementNotification()
        {
            var chedReference = _scenarioContext.Get<string>("ReplacementCHEDReference");
            inspectorImportNotificationsPage?.SearchForChed(chedReference);
        }

        [Then("the notification should not be present in the inspector workflow")]
        public void ThenTheNotificationShouldNotBePresentInTheInspectorWorkflow()
        {
            Assert.True(inspectorImportNotificationsPage?.VerifyNotificationIsNotPresent(),
                "Expected no notifications to be found, but results were displayed");
        }

        [When("the user clicks into the notification")]
        public void WhenTheUserClicksIntoTheNotification()
        {
            inspectorImportNotificationsPage?.ClickNotification();
        }

        [Then("the notification returned in the search has the status {string} on the Import notifications page")]
        public void ThenTheNotificationReturnedInTheSearchHasTheStatusOnTheImportNotificationsPage(string expectedStatus)
        {
            var actualStatus = inspectorImportNotificationsPage?.GetNotificationStatus();
            Assert.AreEqual(expectedStatus, actualStatus,
                $"Expected notification status '{expectedStatus}' but found '{actualStatus}'");
        }

        [Then("the user verifies {string} link in Dashboard header")]
        public void WhenTheUserVerifiesCreateNotificationInDashboardHeader(string link)
        {
            Assert.IsTrue(inspectorImportNotificationsPage?.VerifyNotificationHeader(link), "The " + link + " is not available");
        }
        
        [Then("the user validates {string} text in Import notifications Page")]
        public void WhenTheUserValidatesTextInImportNotificationPage(string label)
        {
            Assert.IsTrue(inspectorImportNotificationsPage?.VerifyLabel(label), "The " + label + " is not available");
        }

        [When("the user searches for the CHED {string} in Import notifications page")]
        public void WhenTheUserSearchesForTheCHEDInImportNotificationsPage(string chedRef)
        {
            inspectorImportNotificationsPage?.SearchForChed(chedRef);
        }

        [Then("the notification should be found with risk outcome {string}")]
        public void ThenTheNotificationShouldBeFoundWithRiskOutcome(string riskOutcome)
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(inspectorImportNotificationsPage?.VerifyRiskOutcome(chedRef, riskOutcome));
        }
    }
}