using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdentityAndPhysicalChecksSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IIdentityAndPhysicalChecksPage? identityAndPhysicalChecksPage => _objectContainer.IsRegistered<IIdentityAndPhysicalChecksPage>() ? _objectContainer.Resolve<IIdentityAndPhysicalChecksPage>() : null;


        public IdentityAndPhysicalChecksSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Identity and physical checks page should be displayed")]
        public void ThenTheIdentityAndPhysicalChecksPageShouldBeDisplayed()
        {
            Assert.True(identityAndPhysicalChecksPage?.IsPageLoaded());
        }

        [When("the user selects {string} under {string} in identity check")]
        public void WhenTheUserSelectsUnderInIdentityCheck(string decision, string checkType)
        {
            _scenarioContext.Add("IdentityCheckType", checkType);
            _scenarioContext.Add("IdentityCheckDecision", decision);
            identityAndPhysicalChecksPage?.ClickIdentityCheckOption(decision, checkType);
        }

        [When("the user selects {string} for physical check")]
        public void WhenTheUserSelectsForPhysicalCheck(string decision)
        {
            _scenarioContext.Add("PhysicalCheckDecision", decision);
            identityAndPhysicalChecksPage?.ClickPhysicalCheckDecision(decision);
        }

        [When("the user clicks Save and Return")]
        public void WhenTheUserClicksSaveAndReturn()
        {
            identityAndPhysicalChecksPage?.ClickSaveAndReturn();
        }
    }
}