using Defra.UI.Tests.Pages.Interfaces;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhatAreYouExportingSelectOneCommodityAtATimeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterWhatAreYouExportingSelectOneCommodityAtATimePage? exporterWhatAreYouExportingSelectOneCommodityAtATimePage =>
            _objectContainer.IsRegistered<IExporterWhatAreYouExportingSelectOneCommodityAtATimePage>()
                ? _objectContainer.Resolve<IExporterWhatAreYouExportingSelectOneCommodityAtATimePage>()
                : null;

        public ExporterWhatAreYouExportingSelectOneCommodityAtATimeSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [When(@"^the user selects commodity '([^']*)' and clicks the Continue button$")]
        public void WhenTheUserSelectsCommodityAndClicksTheContinueButton(string commodity)
        {
            _scenarioContext["Commodity"] = commodity;
            exporterWhatAreYouExportingSelectOneCommodityAtATimePage?.SelectCommodity(commodity);
            exporterWhatAreYouExportingSelectOneCommodityAtATimePage?.ClickContinueButton();
        }
    }
}