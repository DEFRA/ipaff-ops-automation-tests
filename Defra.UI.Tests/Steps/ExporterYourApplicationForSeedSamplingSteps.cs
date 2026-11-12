using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterYourApplicationForSeedSamplingSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterYourApplicationForSeedSamplingPage? exporterYourApplicationForSeedSamplingPage =>
            _objectContainer.IsRegistered<IExporterYourApplicationForSeedSamplingPage>()
                ? _objectContainer.Resolve<IExporterYourApplicationForSeedSamplingPage>()
                : null;

        public ExporterYourApplicationForSeedSamplingSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Your application for seed sampling page is displayed")]
        public void ThenTheYourApplicationForSeedSamplingPageIsDisplayed()
        {
            Assert.True(exporterYourApplicationForSeedSamplingPage?.IsPageLoaded(), "Your application for seed sampling page is not displayed");
        }

        [When("the user clicks the Seeds to be sampled link")]
        public void WhenTheUserClicksTheSeedsToBeSampledLink()
        {
            exporterYourApplicationForSeedSamplingPage?.ClickSeedsToBeSampledLink();
        }

        [When("the user clicks the Inspection details link")]
        public void WhenTheUserClicksTheInspectionDetailsLink()
        {
            exporterYourApplicationForSeedSamplingPage?.ClickInspectionDetailsLink();
        }
    }
}
