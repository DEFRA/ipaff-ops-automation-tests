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


        [When("the user searches for the CHED number")]
        [When("user searches for the import notification after decision submission")]
        [When("the user searches for the newly created notification on the Import notifications page")]
        public void WhenTheUserSearchesForTheNewlyCreatedNotificationOnTheImportNotificationsPage()
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            inspectorImportNotificationsPage?.SearchForChed(chedRef);
        }

        [Then("the user clicks the notificaiton found with status {string}")]
        public void ThenTheNotificationShouldBeFoundWithStatus(string status)
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            inspectorImportNotificationsPage?.SearchForChed(chedRef);
            inspectorImportNotificationsPage?.VerifyNotificationStatus(chedRef, status);
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
    }
}