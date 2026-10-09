using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterYourApplicationsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterYourApplicationsPage? exporterYourApplicationsPage =>
            _objectContainer.IsRegistered<IExporterYourApplicationsPage>()
                ? _objectContainer.Resolve<IExporterYourApplicationsPage>()
                : null;

        public ExporterYourApplicationsSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Exporter Portal homepage should be displayed")]
        [Then("the Your applications page is displayed")]
        public void ThenTheYourApplicationsPageIsDisplayed()
        {
            Assert.True(exporterYourApplicationsPage?.IsPageLoaded(), "Your applications page is not displayed");
        }

        [When("the user clicks the Start a new application button")]
        public void WhenTheUserClicksTheStartANewApplicationButton()
        {
            exporterYourApplicationsPage?.ClickStartNewApplicationButton();
        }

        [When("the user enters the recorded APHA reference for {string} in the search box")]
        public void WhenTheUserEntersTheRecordedAPHAReferenceForInTheSearchBox(string applicationName)
        {
            exporterYourApplicationsPage?.SearchForApplication(GetRecordedReference(applicationName));
        }

        [Then("the Export Application portal search returns one matching record")]
        public void ThenTheExportApplicationPortalSearchReturnsOneMatchingRecord()
        {
            Assert.AreEqual(1, exporterYourApplicationsPage?.GetApplicationRowCount(), "Search did not return exactly one matching record");
        }

        [When("the user clicks the APHA reference link for {string}")]
        public void WhenTheUserClicksTheAPHAReferenceLinkFor(string applicationName)
        {
            exporterYourApplicationsPage?.ClickAphaReferenceLink(GetRecordedReference(applicationName));
        }

        private string GetRecordedReference(string applicationName)
        {
            var key = $"{applicationName}_APHAReference";
            if (!_scenarioContext.TryGetValue(key, out string reference) || string.IsNullOrWhiteSpace(reference))
            {
                throw new InvalidOperationException($"No APHA reference recorded in ScenarioContext under '{key}'");
            }
            return reference;
        }
    }
}