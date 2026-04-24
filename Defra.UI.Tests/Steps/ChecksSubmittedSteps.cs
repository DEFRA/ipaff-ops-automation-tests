using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ChecksSubmittedSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IChecksSubmittedPage? checksSubmittedPage => _objectContainer.IsRegistered<IChecksSubmittedPage>() ? _objectContainer.Resolve<IChecksSubmittedPage>() : null;

        public ChecksSubmittedSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Your checks have been submitted page should be displayed")]
        public void ThenTheYourChecksHaveBeenSubmittedPageShouldBeDisplayed()
        {
            Assert.True(checksSubmittedPage?.IsPageLoaded(), "Yours checks have been submitted page not loaded");
            _scenarioContext["CHEDReferenceWithVersion"] = checksSubmittedPage.GetCHEDReferenceWithVersion();
            _scenarioContext["Outcome"] = checksSubmittedPage.GetOutcome();
            Assert.True(checksSubmittedPage?.IsViewOrPrintCHEDButtonDisplayed(), "View or print CHED button is not displayed");
        }

        [Then("the Your checks have been submitted page is displayed")]
        public void ThenTheYourChecksHaveBeenSubmittedPageIsDisplayed()
        {
            Assert.True(checksSubmittedPage?.IsPageLoaded(), "Yours checks have been submitted page not loaded");
        }

        [When("the user clicks View or print CHED button")]
        public void WhenTheUserClicksViewOrPrintCHEDButton()
        {
            checksSubmittedPage?.ClickViewOrPrintCHED();
        }

        [When("the user clicks return to your dashboard link in decision submitted page")]
        public void WhenTheUserClicksReturnToYourDashboardLinkInDecisionSubmittedPage()
        {
            checksSubmittedPage?.ClickReturnToYourDashboard();
        }

        [When("the user clicks View or print CHED")]
        public void WhenTheUserClicksViewOrPrintCHED()
        {
            checksSubmittedPage?.ClickViewOrPrintCHED();
        }

        [Then(@"the user should see an error message {string} under title {string} in checks submitted page")]
        [Then("a border notification banner displaying the reason for refusal {string} under the title {string} is displayed")]

        public void ThenIShouldSeeAnErrorMessageUnderTitleInChecksSubmittedPage(string errorMessage, string title)
        {
            Assert.True(checksSubmittedPage?.VerifyErrorMessageTitle(title));

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Assert.True(checksSubmittedPage?.IsError(errorMessage), $"There is no error message found with - {errorMessage}");
            }
        }

        [Then("the message {string} should be displayed under Next steps")]
        public void ThenTheMessageShouldBeDisplayedUnderNextSteps(string message)
        {
            Assert.True(checksSubmittedPage?.VerifyNextStepsMessage(message));
        }

        [When("the user clicks on Create border notification button")]
        public void WhenTheUserClicksOnCreateBorderNotificationButton()
        {
            checksSubmittedPage?.ClickCreateBorderNotiButton();
        }
        
        [Then("the user verfies the decision outcome as {string}")]
        public void WhenTheUserVerifiesDecisionOutcome(String outcome)
        {
            Assert.IsTrue(checksSubmittedPage?.VerifyOutcome(outcome), "The Decision outcome is not " + outcome);
        }

        [When("the user clicks Return to Decision hub")]
        public void WhenTheUserClicksReturnToDecisionHub()
        {
            checksSubmittedPage?.ClickReturnToDecisionHub();
        }
    }
}