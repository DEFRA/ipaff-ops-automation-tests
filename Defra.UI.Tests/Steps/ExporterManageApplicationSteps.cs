using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterManageApplicationSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterManageApplicationPage? exporterManageApplicationPage =>
            _objectContainer.IsRegistered<IExporterManageApplicationPage>()
                ? _objectContainer.Resolve<IExporterManageApplicationPage>()
                : null;

        public ExporterManageApplicationSteps(IObjectContainer container)
        {
            _objectContainer = container;
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

        [Then("the Remaining Approved Stock quantity is {string}")]
        public void ThenTheRemainingApprovedStockQuantityIs(string expectedQuantity)
        {
            var actual = exporterManageApplicationPage?.GetRemainingApprovedStockQuantity();
            Assert.AreEqual(expectedQuantity, actual, $"Remaining approved stock quantity is '{actual}', expected '{expectedQuantity}'");
        }
    }
}
