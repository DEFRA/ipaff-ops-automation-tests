using Defra.UI.Tests.Pages.Classes;
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
        public void ThenYourControlHasBeenRecordedPageShouldBeDisplayed()
        {
            Assert.True(controlRecordedPage?.IsPageLoaded(), "Your control has been recorded page not loaded");
        }

        [Then("the CHED reference number and Outcome are displayed")]
        public void ThenTheCHEDReferenceNumberAndOutcomeAreDisplayed()
        {
            Assert.False(string.IsNullOrEmpty(controlRecordedPage?.GetCHEDReferenceWithVersion()), "CHED reference with version should not be null or empty");
            Assert.True(controlRecordedPage?.GetOutcome().Contains("Consignment has left the UK"), "Record control outcome doesn't match");
        }

        [When("the user clicks View or print CHED button on Control recorded page")]
        public void WhenTheUserClicksViewOrPrintCHEDButtonOnControlRecordedPage()
        {
            controlRecordedPage?.ClickViewOrPrintCHED();
        }
    }
}