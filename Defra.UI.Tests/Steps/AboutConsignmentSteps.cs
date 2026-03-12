using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AboutConsignmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAboutConsignmentPage? aboutConsignmentPage => _objectContainer.IsRegistered<IAboutConsignmentPage>() ? _objectContainer.Resolve<IAboutConsignmentPage>() : null;
        private IUserObject? UserObject => _objectContainer.IsRegistered<IUserObject>() ? _objectContainer.Resolve<IUserObject>() : null;

        public AboutConsignmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the About the consignment\\/What are you importing? page should be displayed with radio buttons")]
        public void ThenTheAboutTheConsignmentWhatAreYouImportingPageShouldBeDisplayedWithRadioButtons()
        {
            Assert.True(aboutConsignmentPage?.IsPageLoaded(), "About the consignment What are you importing? page not loaded");
            Assert.True(aboutConsignmentPage?.AreImportOptionsPresent(), "Expected import options are not present on the page.");
        }

        [When("the user chooses {string} option")]
        public void WhenTheUserChoosesOption(string option)
        {
            aboutConsignmentPage?.ClickImportingProduct(option);
            _scenarioContext["ImportType"] = option;
        }

        [Then("the user should be able to click Save and continue")]
        [When("the user clicks Save and continue")]
        [Then("the user clicks Save and continue")]
        public void WhenTheUserClicksSaveAndContinue()
        {
            aboutConsignmentPage?.ClickSaveAndContinue();
        }

        [Then("About the consignment - Who are you creating this notification for? page should be displayed")]
        public void ThenAboutTheConsignment_WhoAreYouCreatingThisNotificationForPageShouldBeDisplayed()
        {
            Assert.True(aboutConsignmentPage?.IsWhoAreYouCreatingThisNotificationForPageLoaded(), "About the consignment - Who are you creating this notification for? page not loaded");
        }

        [When("the user selects {string} option in about the consignment page")]
        public void WhenTheUserSelectsOptionInAboutTheConsignmentPage(string option)
        {
            aboutConsignmentPage?.SelectToWhomNotificationCreatedFor(option);
        }

        [Then("About the consignment - Which company is this notification for page should be displayed")]
        public void ThenAboutTheConsignment_WhichCompanyIsThisNotificationForPageShouldBeDisplayed()
        {
            Assert.True(aboutConsignmentPage?.IsWhichCompanyIsThisNotificationForPageLoaded(), "About the consignment - Which company is this notification for page not loaded");
        }

        [When("the user selects company name as {string}")]
        public void WhenTheUserSelectsCompanyNameAs(string option)
        {
            aboutConsignmentPage?.SelectCompany(option);
            _scenarioContext["CompanyName"] = option;
        }

        [When("the user waits upto 10 minutes to select the {string} radio button option")]
        public void WhenTheUserWaitsUpToTenMinutesToSelectTheRadioButtonOption(string role)
        {
            var businessName = UserObject?.GetUser("IPAFF", role)?.BusinessName;
            Assert.That(businessName, Is.Not.Null.And.Not.Empty, $"BusinessName not found in Users.json for role '{role}'");

            aboutConsignmentPage?.WaitAndSelectCompanyRadioButton(
                businessName!,
                maxWait: TimeSpan.FromMinutes(10),
                retryInterval: TimeSpan.FromSeconds(30));

            _scenarioContext["CompanyName"] = businessName;
        }
    }
}