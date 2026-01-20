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

        [When("the user clicks Raise border notification button")]
        public void WhenTheUserClicksRaiseBorderNotificationButton()
        {
            chedOverviewPage?.ClickRaiseBorderNotification();
        }
    }
}