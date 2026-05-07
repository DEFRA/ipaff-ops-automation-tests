using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class UploadingRuleChangesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IUploadingRuleChangesPage? uploadingRuleChangesPage =>
            _objectContainer.IsRegistered<IUploadingRuleChangesPage>()
                ? _objectContainer.Resolve<IUploadingRuleChangesPage>()
                : null;

        public UploadingRuleChangesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Uploading rule changes to the risk engine page should be displayed")]
        public void ThenUploadingRuleChangesPageShouldBeDisplayed()
        {
            Assert.True(uploadingRuleChangesPage?.IsPageLoaded(), "Uploading rule changes to the risk engine page is not displayed");
        }

        [When("the user clicks the Check file status link")]
        public void WhenTheUserClicksTheCheckFileStatusLink()
        {
            uploadingRuleChangesPage?.ClickCheckFileStatusLink();
        }
    }
}