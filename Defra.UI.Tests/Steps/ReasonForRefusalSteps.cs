using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using FluentAssertions.Execution;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;


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
        public void WhenTheUserSelectsAsReasonForRefusal(string reason)
        {
            reasonForRefusalPage?.SelectReasonForRefusal(reason);
            _scenarioContext["ReasonForRefusal"] = reason;
        }

        [When("the user provides the reason as {string} in Reason for refusal page")]
        public void WhenTheUserProvidesTheReasonAsInReasonForRefusalPage(string reasonText)
        {
            reasonForRefusalPage?.EnterReasonTextForOther(reasonText);
            _scenarioContext["ReasonForRefusalText"] = reasonText;
        }

        [When("the user selects {string} as another reason for refusal")]
        public void WhenTheUserSelectsAdditionalReasonForRefusal(string reason)
        {
            reasonForRefusalPage?.SelectReasonForRefusal(reason);
            _scenarioContext["AdditionalReasonForRefusal"] = reason;
        }
    }
}