using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ManageYourAuthorisationsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IManageYourAuthorisationsPage? manageYourAuthorisationsPage => _objectContainer.IsRegistered<IManageYourAuthorisationsPage>() ? _objectContainer.Resolve<IManageYourAuthorisationsPage>() : null;
        private IUserObject? userObject => _objectContainer.IsRegistered<IUserObject>() ? _objectContainer.Resolve<IUserObject>() : null;

        public ManageYourAuthorisationsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Manage your authorisations page should be displayed")]
        public void ThenTheManageYourAuthorisationsPageShouldBeDisplayed()
        {
            Assert.True(manageYourAuthorisationsPage?.IsPageLoaded(), "Manage your authorisations page is not displayed");
        }

        [Then("the business name should be displayed as the page header for {string}")]
        public void ThenTheBusinessNameShouldBeDisplayedAsThePageHeader(string role)
        {
            var businessName = userObject?.GetUser("IPAFF", role)?.BusinessName;
            Assert.That(businessName, Is.Not.Null.And.Not.Empty, $"BusinessName not found in Users.json for {role}");
            Assert.True(manageYourAuthorisationsPage?.IsBusinessNameDisplayedAsHeader(businessName!),
                $"Business name '{businessName}' is not displayed as the page header");
        }

        [Then("the change settings link should be displayed")]
        public void ThenTheChangeSettingsLinkShouldBeDisplayed()
        {
            Assert.True(manageYourAuthorisationsPage?.IsChangeSettingsLinkDisplayed(),
                "The 'To change settings related to this organisation, click here' link is not displayed");
        }

        [When("the user clicks the change settings link")]
        public void WhenTheUserClicksTheChangeSettingsLink()
        {
            manageYourAuthorisationsPage?.ClickChangeSettingsLink();
        }

        [Then("the Businesses you are authorised to represent header should be displayed")]
        public void ThenTheBusinessesYouAreAuthorisedToRepresentHeaderShouldBeDisplayed()
        {
            Assert.True(manageYourAuthorisationsPage?.IsBusinessesYouAreAuthorisedToRepresentHeaderDisplayed(),
                "The 'Businesses you are authorised to represent' header is not displayed");
        }

        [Then("the agent code and helper text should be displayed for {string}")]
        public void ThenTheAgentCodeAndHelperTextShouldBeDisplayedFor(string role)
        {
            var agentCode = userObject?.GetUser("IPAFF", role)?.AgentCode;
            Assert.That(agentCode, Is.Not.Null.And.Not.Empty, $"AgentCode not found in Users.json for {role}");
            Assert.True(manageYourAuthorisationsPage?.IsAgentCodeDisplayed(agentCode!),
                $"Agent code '{agentCode}' with expected helper text is not displayed on the page");
        }

        [Then("the Automatically accept delegation requests from Importers\\/Exporters should be toggled to Yes")]
        public void ThenTheAutomaticallyAcceptDelegationToggleShouldBeSetToYes()
        {
            Assert.True(manageYourAuthorisationsPage?.IsAutomaticallyAcceptDelegationToggledYes(),
                "The 'Automatically accept delegation requests from Importers/Exporters' toggle is not set to Yes");
        }

        [Then("the Companies section should be displayed with no permissions message")]
        public void ThenTheCompaniesSectionShouldBeDisplayedWithNoPermissionsMessage()
        {
            Assert.True(manageYourAuthorisationsPage?.IsCompaniesWithNoPermissionsDisplayed(),
                "The Companies section with 'You have not been assigned any permissions by importers/exporters.' is not displayed");
        }

        [Then("{string} and {string} should be listed as companies")]
        public void ThenRolesShouldBeListedAsCompanies(string role1, string role2)
        {
            var trader1BusinessName = userObject?.GetUser("IPAFF", role1)?.BusinessName;
            var trader2BusinessName = userObject?.GetUser("IPAFF", role2)?.BusinessName;

            Assert.That(trader1BusinessName, Is.Not.Null.And.Not.Empty, $"BusinessName not found in Users.json for {role1}");
            Assert.That(trader2BusinessName, Is.Not.Null.And.Not.Empty, $"BusinessName not found in Users.json for {role2}");

            Assert.True(manageYourAuthorisationsPage?.AreCompaniesListed(trader1BusinessName!, trader2BusinessName!),
                $"Expected both '{trader1BusinessName}' and '{trader2BusinessName}' to be listed as companies on the Manage your authorisations page");
        }

        [Then("the Agents acting on your behalf header should be displayed")]
        public void ThenTheAgentsActingOnYourBehalfHeaderShouldBeDisplayed()
        {
            Assert.True(manageYourAuthorisationsPage?.IsAgentsActingOnYourBehalfHeaderDisplayed(),
                "The 'Agents acting on your behalf' header is not displayed");
        }

        [Then("the no agents authorised message should be displayed")]
        public void ThenTheNoAgentsAuthorisedMessageShouldBeDisplayed()
        {
            Assert.True(manageYourAuthorisationsPage?.IsNoAgentsAuthorisedMessageDisplayed(),
                "'You have not yet authorised any agents to act on behalf of your business.' message is not displayed");
        }

        [Then("the Add an agent button should be displayed")]
        public void ThenTheAddAnAgentButtonShouldBeDisplayed()
        {
            Assert.True(manageYourAuthorisationsPage?.IsAddAnAgentButtonDisplayed(),
                "'Add an agent' button is not displayed");
        }

        [When("the user clicks Add an agent")]
        public void WhenTheUserClicksAddAnAgent()
        {
            manageYourAuthorisationsPage?.ClickAddAnAgent();
        }

        [Then("the {string} name should be listed under Agents acting on your behalf")]
        public void ThenTheAgentNameShouldBeListedUnderAgentsActingOnYourBehalf(string role)
        {
            var businessName = userObject?.GetUser("IPAFF", role)?.BusinessName;
            Assert.That(businessName, Is.Not.Null.And.Not.Empty, $"BusinessName not found in Users.json for {role}");
            Assert.True(manageYourAuthorisationsPage?.IsAgentListedWithConfirmedDelegation(businessName!),
                $"Agent '{businessName}' is not listed under 'Agents acting on your behalf' with status 'Agent confirmed delegation'");
        }

        [Then("the Companies section should be displayed with permissions message")]
        public void ThenTheCompaniesSectionShouldBeDisplayedWithPermissionsMessage()
        {
            Assert.True(manageYourAuthorisationsPage?.IsCompaniesWithPermissionsDisplayed(),
                "The Companies section with permissions message and company list is not displayed");
        }

        [When("the user clicks on the Back link above the Manage your authorisations header")]
        public void WhenTheUserClicksOnTheBackLinkAboveTheManageYourAuthorisationsHeader()
        {
            manageYourAuthorisationsPage?.ClickBackLink();
        }
    }
}