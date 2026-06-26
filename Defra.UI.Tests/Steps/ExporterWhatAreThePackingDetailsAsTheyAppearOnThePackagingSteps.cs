using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage? exporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage =>
            _objectContainer.IsRegistered<IExporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage>()
                ? _objectContainer.Resolve<IExporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage>()
                : null;

        public ExporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the What are the packing details as they appear on the packaging? page is displayed")]
        public void ThenTheWhatAreThePackingDetailsAsTheyAppearOnThePackagingPageIsDisplayed()
        {
            Assert.True(exporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage?.IsPageLoaded(), "Packing details page is not displayed");
        }

        [When("the user selects the middle packer details option and clicks the Save and continue button")]
        public void WhenTheUserSelectsTheMiddlePackerDetailsOptionAndClicksTheSaveAndContinueButton()
        {
            exporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage?.SelectMiddleOption();
            exporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage?.ClickSaveAndContinueButton();
        }
    }
}