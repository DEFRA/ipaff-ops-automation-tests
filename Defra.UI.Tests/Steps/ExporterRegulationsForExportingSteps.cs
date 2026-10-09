using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterRegulationsForExportingSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterRegulationsForExportingPage? exporterRegulationsForExportingPage =>
            _objectContainer.IsRegistered<IExporterRegulationsForExportingPage>()
                ? _objectContainer.Resolve<IExporterRegulationsForExportingPage>()
                : null;

        public ExporterRegulationsForExportingSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Regulations for exporting {string} to {string} page is displayed")]
        public void ThenTheRegulationsForExportingToPageIsDisplayed(string commodity, string country)
        {
            Assert.True(exporterRegulationsForExportingPage?.IsPageLoaded(commodity, country), $"Regulations for exporting {commodity} to {country} page is not displayed");
        }

        [Then("the guidance needs you to have a phytosanitary certificate to export seeds to {string}")]
        public void ThenTheGuidanceNeedsYouToHaveAPhytosanitaryCertificateToExportSeedsTo(string country)
        {
            var guidance = $"needs you to have a phytosanitary certificate to export seeds to {country}";
            Assert.True(exporterRegulationsForExportingPage?.IsGuidanceDisplayed(guidance), $"Guidance '{guidance}' is not displayed");
        }

        [Then("the guidance needs you to describe your consignment using the botanical name \\(genus and species)")]
        public void ThenTheGuidanceNeedsYouToDescribeYourConsignmentUsingTheBotanicalName()
        {
            var guidance = "needs you to describe your consignment using the botanical name (genus and species)";
            Assert.True(exporterRegulationsForExportingPage?.IsGuidanceDisplayed(guidance), $"Guidance '{guidance}' is not displayed");
        }

        [When("the user ticks the exporting regulations checkbox")]
        public void WhenTheUserTicksTheExportingRegulationsCheckbox()
        {
            exporterRegulationsForExportingPage?.TickConfirmRegulationsCheckbox();
        }

        [When("the user clicks the Continue button on the Regulations for exporting page")]
        public void WhenTheUserClicksTheContinueButtonOnTheRegulationsForExportingPage()
        {
            exporterRegulationsForExportingPage?.ClickContinueButton();
        }
    }
}
