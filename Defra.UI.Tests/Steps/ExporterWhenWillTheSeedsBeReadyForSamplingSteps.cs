using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhenWillTheSeedsBeReadyForSamplingSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterWhenWillTheSeedsBeReadyForSamplingPage? exporterWhenWillTheSeedsBeReadyForSamplingPage =>
            _objectContainer.IsRegistered<IExporterWhenWillTheSeedsBeReadyForSamplingPage>()
                ? _objectContainer.Resolve<IExporterWhenWillTheSeedsBeReadyForSamplingPage>()
                : null;

        public ExporterWhenWillTheSeedsBeReadyForSamplingSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the When will the seeds be ready for sampling? page is displayed")]
        public void ThenTheWhenWillTheSeedsBeReadyForSamplingPageIsDisplayed()
        {
            Assert.True(exporterWhenWillTheSeedsBeReadyForSamplingPage?.IsPageLoaded(), "When will the seeds be ready for sampling? page is not displayed");
        }

        [When("the user enters a valid date for when the seeds will be ready for sampling at least {int} days in the future")]
        public void WhenTheUserEntersAValidDateForWhenTheSeedsWillBeReadyForSampling(int days)
        {
            exporterWhenWillTheSeedsBeReadyForSamplingPage?.EnterReadyDate(DateTime.Today.AddDays(days));
        }

        [When("the user clicks the Save and continue button on the When will the seeds be ready for sampling? page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheWhenWillTheSeedsBeReadyForSamplingPage()
        {
            exporterWhenWillTheSeedsBeReadyForSamplingPage?.ClickSaveAndContinueButton();
        }
    }
}
