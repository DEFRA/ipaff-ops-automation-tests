using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterTermsAndConditionsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IExporterTermsAndConditionsPage exporterTermsAndConditionsPage => _objectContainer.Resolve<IExporterTermsAndConditionsPage>();
        public ExporterTermsAndConditionsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Terms and conditions page is displayed on the Exporter Portal")]
        public void ThenPageIsDisplayed() => Assert.That(exporterTermsAndConditionsPage.IsPageLoaded(), Is.True);

        [When("the user accepts the exporter registration terms and continues")]
        public void WhenTermsAreAccepted() => exporterTermsAndConditionsPage.AcceptAndContinue();
    }
}
