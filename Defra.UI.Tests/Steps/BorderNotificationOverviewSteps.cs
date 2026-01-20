using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class BorderNotificationOverviewSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IBorderNotificationOverviewPage? borderNotificationOverviewPage => _objectContainer.IsRegistered<IBorderNotificationOverviewPage>() ? _objectContainer.Resolve<IBorderNotificationOverviewPage>() : null;

        public BorderNotificationOverviewSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Border notification overview page should be displayed")]
        public void ThenTheBorderNotificationOverviewPageShouldBeDisplayed()
        {
            Assert.True(borderNotificationOverviewPage?.IsPageLoaded(), "Border notification overview page not loaded");
        }

        [When("the user clicks Dashboard link")]
        public void WhenTheUserClicksDashboardLink()
        {
            borderNotificationOverviewPage?.ClickDashboard();
        }
    }
}