using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterYourApplicationSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterYourApplicationPage? exporterYourApplicationPage =>
            _objectContainer.IsRegistered<IExporterYourApplicationPage>()
                ? _objectContainer.Resolve<IExporterYourApplicationPage>()
                : null;

        public ExporterYourApplicationSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Your application page is displayed")]
        public void ThenTheYourApplicationPageIsDisplayed()
        {
            Assert.True(exporterYourApplicationPage?.IsPageLoaded(), "Your application page is not displayed");
        }

        [When("the user clicks the What's in your consignment? link")]
        public void WhenTheUserClicksTheWhatsInYourConsignmentLink()
        {
            exporterYourApplicationPage?.ClickWhatsInYourConsignmentLink();
        }

        [When("the user clicks the What are the inspection details? link")]
        public void WhenTheUserClicksTheWhatAreTheInspectionDetailsLink()
        {
            exporterYourApplicationPage?.ClickWhatAreTheInspectionDetailsLink();
        }

        [When("the user clicks the How will this consignment be transported? link")]
        public void WhenTheUserClicksTheHowWillThisConsignmentBeTransportedLink()
        {
            exporterYourApplicationPage?.ClickHowWillThisConsignmentBeTransportedLink();
        }

        [When("the user clicks the What are the packer details? link")]
        public void WhenTheUserClicksTheWhatAreThePackerDetailsLink()
        {
            exporterYourApplicationPage?.ClickWhatAreThePackerDetailsLink();
        }

        [When("the user clicks the Check your answers and submit your application link")]
        public void WhenTheUserClicksTheCheckYourAnswersAndSubmitYourApplicationLink()
        {
            exporterYourApplicationPage?.ClickCheckYourAnswersAndSubmitYourApplicationLink();
        }
    }
}