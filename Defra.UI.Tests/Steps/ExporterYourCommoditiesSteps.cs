using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterYourCommoditiesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterYourCommoditiesPage? exporterYourCommoditiesPage =>
            _objectContainer.IsRegistered<IExporterYourCommoditiesPage>()
                ? _objectContainer.Resolve<IExporterYourCommoditiesPage>()
                : null;

        public ExporterYourCommoditiesSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Your commodities page is displayed with the commodity line just entered")]
        public void ThenTheYourCommoditiesPageIsDisplayedWithTheCommodityLineJustEntered()
        {
            Assert.True(exporterYourCommoditiesPage?.IsPageLoaded(), "Your commodities page is not displayed");
            var commodity = _scenarioContext.Get<string>("Commodity");
            Assert.True(exporterYourCommoditiesPage?.IsCommodityLineDisplayed(commodity), "Commodity line is not displayed");
        }

        [When("the user selects {string} and clicks the Save and continue button on the Your commodities page")]
        public void WhenTheUserSelectsAndClicksTheSaveAndContinueButtonOnTheYourCommoditiesPage(string option)
        {
            exporterYourCommoditiesPage?.SelectFinishedAddingCommoditiesOption(option);
            exporterYourCommoditiesPage?.ClickSaveAndContinueButton();
        }
    }
}