using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterDestinationCountrySteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterDestinationCountryPage? exporterDestinationCountryPage =>
            _objectContainer.IsRegistered<IExporterDestinationCountryPage>()
                ? _objectContainer.Resolve<IExporterDestinationCountryPage>()
                : null;

        public ExporterDestinationCountrySteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Destination country page is displayed")]
        public void ThenTheDestinationCountryPageIsDisplayed()
        {
            Assert.True(exporterDestinationCountryPage?.IsPageLoaded(), "Destination country page is not displayed");
        }

        [When(@"^the user searches for destination country '([^']*)' and clicks the Continue button$")]
        public void WhenTheUserSearchesForDestinationCountryAndClicksTheContinueButton(string country)
        {
            exporterDestinationCountryPage?.SearchAndSelectCountry(country);
            exporterDestinationCountryPage?.ClickContinueButton();
        }
    }
}