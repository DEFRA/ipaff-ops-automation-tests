using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterIsThisTheExporterAddressSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterIsThisTheExporterAddressPage? exporterIsThisTheExporterAddressPage =>
            _objectContainer.IsRegistered<IExporterIsThisTheExporterAddressPage>()
                ? _objectContainer.Resolve<IExporterIsThisTheExporterAddressPage>()
                : null;

        public ExporterIsThisTheExporterAddressSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Is this the exporter address? page is displayed")]
        public void ThenTheIsThisTheExporterAddressPageIsDisplayed()
        {
            Assert.True(exporterIsThisTheExporterAddressPage?.IsPageLoaded(), "Is this the exporter address? page is not displayed");
        }

        [When("the user selects {string} and clicks the Save and continue button on the Is this the exporter address? page")]
        public void WhenTheUserSelectsAndClicksTheSaveAndContinueButtonOnTheIsThisTheExporterAddressPage(string option)
        {
            exporterIsThisTheExporterAddressPage?.SelectOption(option);
            exporterIsThisTheExporterAddressPage?.ClickSaveAndContinueButton();
        }
    }
}
