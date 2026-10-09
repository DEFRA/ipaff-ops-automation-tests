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

        [Then("the guidance is This is the address saved to your Defra account.")]
        public void ThenTheGuidanceIsThisIsTheAddressSavedToYourDefraAccount()
        {
            const string guidance = "This is the address saved to your Defra account.";
            Assert.True(exporterIsThisTheExporterAddressPage?.IsGuidanceDisplayed(guidance), $"Guidance '{guidance}' is not displayed");
        }

        [Then("the {string} and {string} radio buttons are displayed and unselected by default")]
        public void ThenTheAndRadioButtonsAreDisplayedAndUnselectedByDefault(string option1, string option2)
        {
            Assert.True(exporterIsThisTheExporterAddressPage?.IsRadioButtonPresentAndUnselected(option1), $"'{option1}' radio button is not displayed or is selected");
            Assert.True(exporterIsThisTheExporterAddressPage?.IsRadioButtonPresentAndUnselected(option2), $"'{option2}' radio button is not displayed or is selected");
        }

        [When("the user selects {string} and clicks the Save and continue button on the Is this the exporter address? page")]
        public void WhenTheUserSelectsAndClicksTheSaveAndContinueButtonOnTheIsThisTheExporterAddressPage(string option)
        {
            exporterIsThisTheExporterAddressPage?.SelectOption(option);
            exporterIsThisTheExporterAddressPage?.ClickSaveAndContinueButton();
        }
    }
}
