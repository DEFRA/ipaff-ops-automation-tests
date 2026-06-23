using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterHowWillThisConsignmentBeTransportedSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterHowWillThisConsignmentBeTransportedPage? exporterHowWillThisConsignmentBeTransportedPage =>
            _objectContainer.IsRegistered<IExporterHowWillThisConsignmentBeTransportedPage>()
                ? _objectContainer.Resolve<IExporterHowWillThisConsignmentBeTransportedPage>()
                : null;

        public ExporterHowWillThisConsignmentBeTransportedSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the How will this consignment be transported? page is displayed")]
        public void ThenTheHowWillThisConsignmentBeTransportedPageIsDisplayed()
        {
            Assert.True(exporterHowWillThisConsignmentBeTransportedPage?.IsPageLoaded(), "How will this consignment be transported? page is not displayed");
        }

        [When("the user selects a transport method and clicks the Save and continue button")]
        public void WhenTheUserSelectsATransportMethodAndClicksTheSaveAndContinueButton()
        {
            exporterHowWillThisConsignmentBeTransportedPage?.SelectAnyTransportMethod();
            exporterHowWillThisConsignmentBeTransportedPage?.ClickSaveAndContinueButton();
        }
    }
}