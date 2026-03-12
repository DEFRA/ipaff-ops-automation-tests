using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AddAnAgentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAddAnAgentPage? addAnAgentPage => _objectContainer.IsRegistered<IAddAnAgentPage>() ? _objectContainer.Resolve<IAddAnAgentPage>() : null;
        private IUserObject? userObject => _objectContainer.IsRegistered<IUserObject>() ? _objectContainer.Resolve<IUserObject>() : null;

        public AddAnAgentSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Add an agent page should be displayed")]
        public void ThenTheAddAnAgentPageShouldBeDisplayed()
        {
            Assert.True(addAnAgentPage?.IsPageLoaded(), "Add an agent page is not displayed");
        }

        [When("the user enters {string} agent code")]
        public void WhenTheUserEntersAgentCode(string role)
        {
            var agentCode = userObject?.GetUser("IPAFF", role)?.AgentCode;
            Assert.That(agentCode, Is.Not.Null.And.Not.Empty, $"AgentCode not found in Users.json for {role}");
            addAnAgentPage?.EnterAgentCode(agentCode!);
        }

        [When("the user clicks Yes for Is this the agent?")]
        public void WhenTheUserClicksYesForIsThisTheAgent()
        {
            addAnAgentPage?.SelectYesForIsThisTheAgent();
        }

        [When("the user ticks the Confirm delegation checkbox")]
        public void WhenTheUserTicksTheConfirmDelegationCheckbox()
        {
            addAnAgentPage?.TickDelegationCheckbox();
        }
    }
}