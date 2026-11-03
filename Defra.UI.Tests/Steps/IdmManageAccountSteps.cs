using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmManageAccountSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmManageAccountPage idmManageAccountPage => _objectContainer.Resolve<IIdmManageAccountPage>();
        public IdmManageAccountSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Manage account page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmManageAccountPage.IsPageLoaded(), Is.True);

        [When("the user accesses their Defra account in a new window")]
        public void WhenDefraAccountIsOpened() => _scenarioContext.Set(idmManageAccountPage.OpenDefraAccountInNewWindow(), "IdmExporterWindow");

        [When("the user clicks Back on the Manage account page")]
        public void WhenBackIsClicked() => idmManageAccountPage.ClickBackLink();
    }
}
