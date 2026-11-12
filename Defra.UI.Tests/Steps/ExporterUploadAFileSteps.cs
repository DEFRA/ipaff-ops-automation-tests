using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterUploadAFileSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterUploadAFilePage? exporterUploadAFilePage =>
            _objectContainer.IsRegistered<IExporterUploadAFilePage>()
                ? _objectContainer.Resolve<IExporterUploadAFilePage>()
                : null;

        public ExporterUploadAFileSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Upload a file page is displayed")]
        public void ThenTheUploadAFilePageIsDisplayed()
        {
            Assert.True(exporterUploadAFilePage?.IsPageLoaded(), "Upload a file page is not displayed");
        }

        [When("the user downloads the CSV template")]
        public void WhenTheUserDownloadsTheCSVTemplate()
        {
            exporterUploadAFilePage?.DownloadCsvTemplate();
        }

        [Then("the CSV template is downloaded")]
        public void ThenTheCSVTemplateIsDownloaded()
        {
            Assert.True(exporterUploadAFilePage?.IsCsvTemplateDownloaded(), "CSV template was not downloaded");
        }

        [Then("the only units of measurements listed in the template are {string} {string} {string}")]
        public void ThenTheOnlyUnitsOfMeasurementsListedInTheTemplateAre(string unit1, string unit2, string unit3)
        {
            var expected = new[] { unit1, unit2, unit3 }.Select(u => u.ToLowerInvariant()).OrderBy(u => u).ToList();
            var actual = exporterUploadAFilePage?.GetUnitsOfMeasurementFromTemplate().Select(u => u.ToLowerInvariant()).OrderBy(u => u).ToList();
            Assert.That(actual, Is.EqualTo(expected), $"Units in template were '{string.Join(", ", actual ?? new List<string>())}'");
        }

        [When("the user clicks the Back link on the Upload a file page")]
        public void WhenTheUserClicksTheBackLinkOnTheUploadAFilePage()
        {
            exporterUploadAFilePage?.ClickBackLink();
        }
    }
}
