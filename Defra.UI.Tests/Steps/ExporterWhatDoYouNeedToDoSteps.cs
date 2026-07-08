using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhatDoYouNeedToDoSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterWhatDoYouNeedToDoPage? exporterWhatDoYouNeedToDoPage =>
            _objectContainer.IsRegistered<IExporterWhatDoYouNeedToDoPage>()
                ? _objectContainer.Resolve<IExporterWhatDoYouNeedToDoPage>()
                : null;

        public ExporterWhatDoYouNeedToDoSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What do you need to do? page is displayed")]
        public void ThenTheWhatDoYouNeedToDoPageIsDisplayed()
        {
            Assert.True(exporterWhatDoYouNeedToDoPage?.IsPageLoaded(), "What do you need to do? page is not displayed");
        }

        [When(@"^the user selects '([^']*)' and clicks the Continue button on the What do you need to do\? page$")]
        public void WhenTheUserSelectsAndClicksTheContinueButtonOnTheWhatDoYouNeedToDoPage(string option)
        {
            _scenarioContext["Journey"] = option;
            exporterWhatDoYouNeedToDoPage?.SelectWhatDoYouNeedToDoOption(option);
            exporterWhatDoYouNeedToDoPage?.ClickContinueButton();
        }
    }
}