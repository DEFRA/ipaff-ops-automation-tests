using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterDoYouWantToApplyUnderThePheatsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterDoYouWantToApplyUnderThePheatsPage? exporterDoYouWantToApplyUnderThePheatsPage =>
            _objectContainer.IsRegistered<IExporterDoYouWantToApplyUnderThePheatsPage>()
                ? _objectContainer.Resolve<IExporterDoYouWantToApplyUnderThePheatsPage>()
                : null;

        public ExporterDoYouWantToApplyUnderThePheatsSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Do you want to apply under the plant health exports audited trader scheme \\(PHEATS)? page is displayed")]
        public void ThenTheDoYouWantToApplyUnderThePheatsPageIsDisplayed()
        {
            Assert.True(exporterDoYouWantToApplyUnderThePheatsPage?.IsPageLoaded(), "Do you want to apply under the plant health exports audited trader scheme (PHEATS)? page is not displayed");
        }

        [When("the user selects {string} and clicks the Save and continue button on the Do you want to apply under the plant health exports audited trader scheme \\(PHEATS)? page")]
        public void WhenTheUserSelectsAndClicksTheSaveAndContinueButtonOnTheDoYouWantToApplyUnderThePheatsPage(string option)
        {
            exporterDoYouWantToApplyUnderThePheatsPage?.SelectOption(option);
            exporterDoYouWantToApplyUnderThePheatsPage?.ClickSaveAndContinueButton();
        }
    }
}
