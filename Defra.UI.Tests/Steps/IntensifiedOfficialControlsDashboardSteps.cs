using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class IntensifiedOfficialControlsDashboardSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IIntensifiedOfficialControlsDashboardPage? intensifiedOfficialControlsDashboardPage =>
            _objectContainer.IsRegistered<IIntensifiedOfficialControlsDashboardPage>()
                ? _objectContainer.Resolve<IIntensifiedOfficialControlsDashboardPage>()
                : null;

        public IntensifiedOfficialControlsDashboardSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Intensified Official Controls dashboard should be displayed")]
        public void ThenTheIntensifiedOfficialControlsDashboardShouldBeDisplayed()
        {
            Assert.True(
                intensifiedOfficialControlsDashboardPage?.IsPageLoaded(),
                "Intensified Official Controls dashboard page is not loaded");
        }

        [When("the user clicks Create new intensified official control button")]
        public void WhenTheUserClicksCreateNewIntensifiedOfficialControlButton()
        {
            intensifiedOfficialControlsDashboardPage?.ClickCreateNewIntensifiedControlCheck();
        }
    }
}