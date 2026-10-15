using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools.PDFProcessor;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterManageApplicationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterManageApplicationPage? exporterManageApplicationPage =>
            _objectContainer.IsRegistered<IExporterManageApplicationPage>()
                ? _objectContainer.Resolve<IExporterManageApplicationPage>()
                : null;

        public ExporterManageApplicationSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Manage application page should be displayed")]
        public void ThenTheManageApplicationPageShouldBeDisplayed()
        {
            Assert.True(exporterManageApplicationPage?.IsPageLoaded(), "Manage application page is not displayed");
        }

        [When("the user clicks the Sampling report \\(csv) link on the Manage application page")]
        public void WhenTheUserClicksTheSamplingReportCsvLinkOnTheManageApplicationPage()
        {
            exporterManageApplicationPage?.ClickSamplingReportLink();
        }

        [Then("the Sampling report \\(csv) file is downloaded")]
        public void ThenTheSamplingReportCsvFileIsDownloaded()
        {
            Assert.True(exporterManageApplicationPage?.IsSamplingReportDownloaded(), "Sampling report (csv) file was not downloaded");
        }

        [Then("the Remaining Approved Stock quantity in the CSV is {string}")]
        public void ThenTheRemainingApprovedStockQuantityInTheCsvIs(string expectedQuantity)
        {
            var actual = exporterManageApplicationPage?.GetRemainingApprovedStockQuantity();
            Assert.AreEqual(expectedQuantity, actual, $"Remaining approved stock quantity is '{actual}', expected '{expectedQuantity}'");
        }

        [Then("the Phytosanitary Certificate displays the details of the application submitted")]
        public void ThenThePhytosanitaryCertificateDisplaysTheDetailsOfTheApplicationSubmitted()
        {
            var pdfPath = GetPhytoCertificatePdfPath();

            var certificate = new PhytoCertificatePdfConverter().Convert(pdfPath);
            var expected = _scenarioContext.ToDictionary(kvp => kvp.Key, kvp => (object?)kvp.Value);
            var mismatches = PhytoCertificateValidator.Validate(certificate, expected);

            if (mismatches.Any())
            {
                Console.WriteLine("[PHYTO CERTIFICATE VALIDATION] Data mismatches found:");
                foreach (var mismatch in mismatches)
                {
                    Console.WriteLine($"[PHYTO CERTIFICATE VALIDATION] {mismatch}");
                }
            }

            Assert.IsEmpty(mismatches, $"Phytosanitary certificate data validation failed. Mismatches: {string.Join(", ", mismatches)}");
        }

        private string GetPhytoCertificatePdfPath()
        {
            if (_scenarioContext.TryGetValue("PhytoCertificatePdfPath", out string? pdfPath) && !string.IsNullOrWhiteSpace(pdfPath))
                return pdfPath;

            var downloadDirectory = _scenarioContext.Get<string>("PDFDownloadedDirectory");
            var applicationReference = _scenarioContext.Get<string>("Phytosanitary Certificate Application_APHAReference");
            return Path.Combine(downloadDirectory, applicationReference + "-certificate.pdf");
        }
    }
}
