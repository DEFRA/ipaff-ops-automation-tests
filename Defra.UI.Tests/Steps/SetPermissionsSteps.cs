using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SetPermissionsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISetPermissionsPage? setPermissionsPage => _objectContainer.IsRegistered<ISetPermissionsPage>() ? _objectContainer.Resolve<ISetPermissionsPage>() : null;

        public SetPermissionsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Set permissions page should be displayed")]
        public void ThenTheSetPermissionsPageShouldBeDisplayed()
        {
            Assert.True(setPermissionsPage?.IsPageLoaded(), "Set permissions page is not displayed");
        }

        [When("the user toggles all permissions to Yes and clicks Finish")]
        public void WhenTheUserTogglesAllPermissionsToYesAndClicksFinish()
        {
            setPermissionsPage?.ToggleAllPermissionsToYes();
            setPermissionsPage?.ClickFinish();
        }
    }
}