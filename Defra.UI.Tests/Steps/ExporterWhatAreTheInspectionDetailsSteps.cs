using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhatAreTheInspectionDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterWhatAreTheInspectionDetailsPage? exporterWhatAreTheInspectionDetailsPage =>
            _objectContainer.IsRegistered<IExporterWhatAreTheInspectionDetailsPage>()
                ? _objectContainer.Resolve<IExporterWhatAreTheInspectionDetailsPage>()
                : null;

        public ExporterWhatAreTheInspectionDetailsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the What are the inspection details? page is displayed")]
        public void ThenTheWhatAreTheInspectionDetailsPageIsDisplayed()
        {
            Assert.True(exporterWhatAreTheInspectionDetailsPage?.IsPageLoaded(), "What are the inspection details? page is not displayed");
        }

        [When("the user selects an inspection address and continues")]
        public void WhenTheUserSelectsAnInspectionAddressAndContinues()
        {
            exporterWhatAreTheInspectionDetailsPage?.SelectAnInspectionAddressAndContinue();
        }
    }
}