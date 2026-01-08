using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ReasonForRefusalSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IReasonForRefusalPage? reasonForRefusalPage => _objectContainer.IsRegistered<IReasonForRefusalPage>() ? _objectContainer.Resolve<IReasonForRefusalPage>() : null;

        public ReasonForRefusalSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Reason for Refusal page should be displayed")]
        public void ThenTheReasonForRefusalPageShouldBeDisplayed()
        {
            Assert.True(reasonForRefusalPage?.IsPageLoaded(), "Reason for refusal page not loaded");
        }

        [When("the user selects {string} as reason for refusal")]
        [When("the user selects {string} as another reason for refusal")]
        public void WhenTheUserSelectsAsReasonForRefusal(string reason)
        {
            reasonForRefusalPage?.SelectReasonForRefusal(reason);
            _scenarioContext.Add("ReasonForRefusal", reason);
        }
    }
}