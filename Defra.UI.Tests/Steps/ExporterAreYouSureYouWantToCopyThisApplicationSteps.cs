using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterAreYouSureYouWantToCopyThisApplicationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterAreYouSureYouWantToCopyThisApplicationPage? exporterAreYouSureYouWantToCopyThisApplicationPage =>
            _objectContainer.IsRegistered<IExporterAreYouSureYouWantToCopyThisApplicationPage>()
                ? _objectContainer.Resolve<IExporterAreYouSureYouWantToCopyThisApplicationPage>()
                : null;

        public ExporterAreYouSureYouWantToCopyThisApplicationSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Are you sure you want to copy this application? page is displayed")]
        public void ThenTheAreYouSureYouWantToCopyThisApplicationPageIsDisplayed()
        {
            Assert.True(exporterAreYouSureYouWantToCopyThisApplicationPage?.IsPageLoaded(), "Are you sure you want to copy this application? page is not displayed");
        }

        [When("the user selects {string} and clicks the Continue button on the Are you sure you want to copy this application? page")]
        public void WhenTheUserSelectsAndClicksTheContinueButtonOnTheAreYouSureYouWantToCopyThisApplicationPage(string option)
        {
            exporterAreYouSureYouWantToCopyThisApplicationPage?.SelectOption(option);
            exporterAreYouSureYouWantToCopyThisApplicationPage?.ClickContinueButton();
        }
    }
}
