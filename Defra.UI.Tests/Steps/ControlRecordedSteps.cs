using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class ControlRecordedSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IControlRecordedPage? controlRecordedPage => _objectContainer.IsRegistered<IControlRecordedPage>() ? _objectContainer.Resolve<IControlRecordedPage>() : null;

        public ControlRecordedSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("Your control has been recorded page should be displayed")]
        public void ThenTheYourChecksHaveBeenSubmittedPageShouldBeDisplayed()
        {
            Assert.True(controlRecordedPage?.IsPageLoaded(), "Your control has been recorded Page is not loaded");
            _scenarioContext["CHEDReferenceWithVersion"] = controlRecordedPage?.GetCHEDReferenceWithVersion();
            _scenarioContext["Outcome"] = controlRecordedPage?.GetOutcome();
            Assert.True(controlRecordedPage?.IsViewOrPrintCHEDButtonDisplayed(), "View or print CHED button is not displayed");
        }

        [Then("the outcome is recorded as {string}")]
        public void ThenTheOutcomeIsRecordedAs(string outcome)
        {
            Assert.True(controlRecordedPage?.GetOutcome().Equals(outcome), "The outcome is not recorded as "+outcome);
        }

        [When("the user clicks View or print CHED button in Record control recorded page")]
        public void WhenTheUserClicksViewOrPrintChedButtonInRecordControlRecordedPage()
        {
            controlRecordedPage?.ClickViewOrPrintCHED();
        }
        
        [When("the user clicks Return to Your Dashboard link in Record control recorded page")]
        public void WhenTheUserClicksReturnToYourDashboardLinkInRecordControlRecordedPage()
        {
            controlRecordedPage?.ClickReturnToYourDashboardLink();
        }
    }
}
