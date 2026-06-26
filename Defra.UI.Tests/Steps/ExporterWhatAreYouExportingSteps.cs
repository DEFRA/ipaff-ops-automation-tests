using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhatAreYouExportingSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterWhatAreYouExportingPage? exporterWhatAreYouExportingPage =>
            _objectContainer.IsRegistered<IExporterWhatAreYouExportingPage>()
                ? _objectContainer.Resolve<IExporterWhatAreYouExportingPage>()
                : null;

        public ExporterWhatAreYouExportingSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What are you exporting? page is displayed")]
        public void ThenTheWhatAreYouExportingPageIsDisplayed()
        {
            Assert.True(exporterWhatAreYouExportingPage?.IsPageLoaded(), "What are you exporting? page is not displayed");
        }

        [When(@"^the user selects '([^']*)' and clicks the Continue button on the What are you exporting\? page$")]
        public void WhenTheUserSelectsAndClicksTheContinueButtonOnTheWhatAreYouExportingPage(string exportType)
        {
            _scenarioContext["Export"] = exportType;
            exporterWhatAreYouExportingPage?.SelectExportType(exportType);
            exporterWhatAreYouExportingPage?.ClickContinueButton();
        }
    }
}