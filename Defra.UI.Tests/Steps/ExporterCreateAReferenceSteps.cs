using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterCreateAReferenceSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterCreateAReferencePage? exporterCreateAReferencePage =>
            _objectContainer.IsRegistered<IExporterCreateAReferencePage>()
                ? _objectContainer.Resolve<IExporterCreateAReferencePage>()
                : null;

        public ExporterCreateAReferenceSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Create a reference page is displayed")]
        public void ThenTheCreateAReferencePageIsDisplayed()
        {
            Assert.True(exporterCreateAReferencePage?.IsPageLoaded(), "Create a reference page is not displayed");
        }

        [When("the user enters a reference number and clicks the Save and continue button")]
        public void WhenTheUserEntersAReferenceNumberAndClicksTheSaveAndContinueButton()
        {
            var reference = DateTime.Now.ToString("ddMMyyyyHHmmss");
            _scenarioContext["SPS-9510 Reference"] = reference;
            exporterCreateAReferencePage?.EnterReference(reference);
            exporterCreateAReferencePage?.ClickSaveAndContinueButton();
        }

        [When(@"^the user enters reference '([^']*)' and clicks the Save and continue button$")]
        public void WhenTheUserEntersReferenceAndClicksTheSaveAndContinueButton(string reference)
        {
            _scenarioContext["SPS-9510 Reference"] = reference;
            exporterCreateAReferencePage?.EnterReference(reference);
            exporterCreateAReferencePage?.ClickSaveAndContinueButton();
        }
    }
}