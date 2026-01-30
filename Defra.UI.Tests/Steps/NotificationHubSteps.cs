using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class NotificationHubSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private INotificationHubPage? notificationHubPage => _objectContainer.IsRegistered<INotificationHubPage>() ? _objectContainer.Resolve<INotificationHubPage>() : null;


        public NotificationHubSteps(IObjectContainer container, ScenarioContext context)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Notification Hub page should be displayed")]
        public void ThenTheNotificationHubPageShouldBeDisplayed()
        {
            Assert.True(notificationHubPage?.IsPageLoaded(), "Notification Hub page not loaded");
        }

        [Then("the Notification Hub page of a new draft notification should be displayed")]
        public void ThenTheNotificationHubPageOfANewDraftNotificationShouldBeDisplayed()
        {
            Assert.True(notificationHubPage?.IsPageLoaded(), "Notification Hub page not loaded");
            Assert.True(notificationHubPage?.GetRefNumber.Contains("DRAFT.GB"), "The Notification page does not display the newly created draft application");
        }

        [When("the user clicks the Commodity hyperlink")]
        public void WhenTheUserClicksTheCommodityHyperlink()
        {
            notificationHubPage?.ClickCommodityLink();
        }

        [When("the user clicks Contact address for consignment link")]
        public void WhenTheUserClicksContactAddressForConsignmentLink()
        {
            notificationHubPage?.ClickContactAddressForConsignmentLink();
        }

        [When("the user clicks the Countries the consignment will travel through hyperlink")]
        public void WhenTheUserClicksTheCountriesTheConsignmentWillTravelThroughHyperlink()
        {
            notificationHubPage?.ClickCountriesTheConsignmentWillTravelThroughLink();
        }

        [When(@"the user clicks on '(.*)' link")]
        public void WhenTheUserClicksOn(string link)
        {
            notificationHubPage?.ClickLink(link);
        }

        [Then("the notification version should be {string}")]
        public void ThenTheNotificationVersionShouldBe(string expectedVersion)
        {
            string actualVersion = notificationHubPage?.GetNotificationVersion() ?? string.Empty;
            Assert.That(actualVersion, Is.EqualTo(expectedVersion),
                $"Expected notification version to be '{expectedVersion}' but was '{actualVersion}'");
        }

        [When("the user records the new CHED Reference number")]
        [Then("the user records the new CHED Reference number")]
        public void WhenTheUserRecordsTheNewCHEDReferenceNumber()
        {
            string refNumber = notificationHubPage?.GetRefNumber ?? string.Empty;
            Assert.False(string.IsNullOrEmpty(refNumber), "Unable to retrieve the CHED Reference number from the Notification Hub page");
            _scenarioContext["NewCHEDReferenceNumber"] = refNumber;
        }

        [Then(@"the '(.*)' task should have the status '(.*)'")]
        public void ThenTheTaskShouldHaveTheStatus(string taskName, string expectedStatus)
        {
            var actualStatus = notificationHubPage?.GetTaskStatus(taskName) ?? string.Empty;
            Assert.That(actualStatus, Is.EqualTo(expectedStatus).IgnoreCase,
                $"Expected task '{taskName}' to have status '{expectedStatus}', but found '{actualStatus}'");
        }
    }
}