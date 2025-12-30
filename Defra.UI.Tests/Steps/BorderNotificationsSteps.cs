using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class BorderNotificationsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IBorderNotificationsPage? borderNotificationsPage => _objectContainer.IsRegistered<IBorderNotificationsPage>() ? _objectContainer.Resolve<IBorderNotificationsPage>() : null;

        public BorderNotificationsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("Border notifications dashboard page should be displayed")]
        public void ThenBorderNotificationsDashboardPageShouldBeDisplayed()
        {
            Assert.True(borderNotificationsPage?.IsPageLoaded(), "Border notifications page not loaded");
        }

        [When("the user searches for the newly created border notification")]
        public void WhenTheUserSearchesForTheNewlyCreatedBorderNotification()
        {
            var borderNotification = _scenarioContext.Get<string>("BNNumber");
            borderNotificationsPage?.SearchForNotification(borderNotification);
        }

        [Then("the border notification found with status {string}")]
        public void ThenTheBorderNotificationFoundWithStatus(string status)
        {
            var borderNotification = _scenarioContext.Get<string>("BNNumber");
            Assert.True(borderNotificationsPage?.VerifyNotificationStatus(borderNotification, status));
        }

        [When("the user clicks the view details of the border notification")]
        public void WhenTheUserClicksTheViewDetailsOfTheBorderNotification()
        {
            borderNotificationsPage?.ClickViewDetails();
        }
    }
}