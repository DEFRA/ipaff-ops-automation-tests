using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ChangeOrganisationSettingsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IChangeOrganisationSettingsPage? changeOrganisationSettingsPage => _objectContainer.IsRegistered<IChangeOrganisationSettingsPage>() ? _objectContainer.Resolve<IChangeOrganisationSettingsPage>() : null;

        public ChangeOrganisationSettingsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Change organisation settings page should be displayed")]
        public void ThenTheChangeOrganisationSettingsPageShouldBeDisplayed()
        {
            Assert.True(changeOrganisationSettingsPage?.IsPageLoaded(), "Change organisation settings page is not displayed");
        }

        [Then("the Change organisation settings page should be displayed with {string}")]
        public void ThenTheChangeOrganisationSettingsPageShouldBeDisplayedWith(string expectedMessage)
        {
            Assert.True(changeOrganisationSettingsPage?.IsConfirmationPageLoaded(expectedMessage),
                $"Change organisation settings confirmation page is not displayed with message '{expectedMessage}'");
        }

        [Then("the instruction text should be displayed on the Change organisation settings page")]
        public void ThenTheInstructionTextShouldBeDisplayed()
        {
            Assert.True(changeOrganisationSettingsPage?.IsInstructionTextDisplayed(),
                "'Below are the available options you may change for this organisation. Select the relevant options and click save.' is not displayed");
        }

        [Then("the Select all that apply hint should be displayed")]
        public void ThenTheSelectAllThatApplyHintShouldBeDisplayed()
        {
            Assert.True(changeOrganisationSettingsPage?.IsSelectAllThatApplyHintDisplayed(),
                "'Select all that apply.' hint is not displayed");
        }

        [Then("the 'I want to authorise an agent to act for my business' checkbox should be displayed")]
        public void ThenTheAuthoriseAgentCheckboxShouldBeDisplayed()
        {
            Assert.True(changeOrganisationSettingsPage?.IsAuthoriseAgentCheckboxDisplayed(),
                "'I want to authorise an agent to act for my business' checkbox is not displayed");
        }

        [Then("the 'I am an agent who wants authority to act on behalf of other businesses' checkbox should be displayed")]
        public void ThenTheActAsAgentCheckboxShouldBeDisplayed()
        {
            Assert.True(changeOrganisationSettingsPage?.IsActAsAgentCheckboxDisplayed(),
                "'I am an agent who wants authority to act on behalf of other businesses' checkbox is not displayed");
        }

        [Then("the agent declaration text should be displayed")]
        public void ThenTheAgentDeclarationTextShouldBeDisplayed()
        {
            Assert.True(changeOrganisationSettingsPage?.IsAgentDeclarationTextDisplayed(),
                "The agent declaration text is not fully displayed");
        }

        [Then("the 'I confirm that I have read and accepted the above statement\\/s.' checkbox should be displayed")]
        public void ThenTheConfirmationCheckboxShouldBeDisplayed()
        {
            Assert.True(changeOrganisationSettingsPage?.IsConfirmationCheckboxDisplayed(),
                "'I confirm that I have read and accept the above statement/s.' checkbox is not displayed");
        }

        [Then("the warning message should be displayed on the Change organisation settings page")]
        public void ThenTheWarningMessageShouldBeDisplayed()
        {
            Assert.True(changeOrganisationSettingsPage?.IsWarningMessageDisplayed(),
                "'If you are turning an option off, any delegations associated with that option will be removed and permissions revoked.' warning is not displayed");
        }

        [Then("the Save button should be displayed on the Change organisation settings page")]
        public void ThenTheSaveButtonShouldBeDisplayed()
        {
            Assert.True(changeOrganisationSettingsPage?.IsSaveButtonDisplayed(),
                "Save button is not displayed on the Change organisation settings page");
        }

        [When("the user ticks 'I want to authorise an agent to act for my business' checkbox")]
        public void WhenTheUserTicksAuthoriseAgentCheckbox()
        {
            changeOrganisationSettingsPage?.TickAuthoriseAgentCheckbox();
        }

        [When("the user unticks 'I am an agent who wants authority to act on behalf of other businesses' checkbox")]
        public void WhenTheUserUnticksActAsAgentCheckbox()
        {
            changeOrganisationSettingsPage?.UntickActAsAgentCheckbox();
        }

        [When("the user ticks 'I confirm that I have read and accepted the above statement\\/s.' checkbox")]
        public void WhenTheUserTicksConfirmationCheckbox()
        {
            changeOrganisationSettingsPage?.TickConfirmationCheckbox();
        }

        [When("the user clicks Save on the Change organisation settings page")]
        public void WhenTheUserClicksSaveOnTheChangeOrganisationSettingsPage()
        {
            changeOrganisationSettingsPage?.ClickSave();
        }

        [When("the user clicks Continue on the Change organisation settings page")]
        public void WhenTheUserClicksContinueOnTheChangeOrganisationSettingsPage()
        {
            changeOrganisationSettingsPage?.ClickContinue();
        }

        [When("the user unticks 'I want to authorise an agent to act for my business' checkbox")]
        public void WhenTheUserUnticksAuthoriseAgentCheckbox()
        {
            changeOrganisationSettingsPage?.UntickAuthoriseAgentCheckbox();
        }

        [When("the user ticks 'I am an agent who wants authority to act on behalf of other businesses' checkbox")]
        public void WhenTheUserTicksActAsAgentCheckbox()
        {
            changeOrganisationSettingsPage?.TickActAsAgentCheckbox();
        }
    }
}