using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;

namespace Defra.UI.Tests.Steps.CP
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


        [Then("the user should be logged into Import notifications page")]
        public void ThenTheUserShouldBeLoggedIntoImportNotificationsPage()
        {
            Assert.True(inspectorImportNotificationsPage?.IsPageLoaded());
        }


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
            inspectorImportNotificationsPage?.VerifyNotificationStatus(chedRef, status);
        }
    }
}