using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ConsignmentsRequiringSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IConsignmentsRequiringControlPage? consignmentsRequiringControlPage => _objectContainer.IsRegistered<IConsignmentsRequiringControlPage>() ? _objectContainer.Resolve<IConsignmentsRequiringControlPage>() : null;

        public ConsignmentsRequiringSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Consignments requiring control page should be displayed")]
        public void ThenTheConsignmentsRequiringControlPageShouldBeDisplayed()
        {
            Assert.True(consignmentsRequiringControlPage?.IsPageLoaded(), "Consignments requiring control page not loaded");
        }


        [Then("the notification should be found with the status {string}")]
        public void ThenTheNotificationShouldBeFoundWithTheStatus(string status)
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(consignmentsRequiringControlPage?.VerifyNotificationStatus(chedRef, status));
        }
    }
}