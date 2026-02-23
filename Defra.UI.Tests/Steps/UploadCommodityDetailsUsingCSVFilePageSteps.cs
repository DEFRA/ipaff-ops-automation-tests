using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class UploadCommodityDetailsUsingCSVFilePageSteps
    {

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;


        private IUploadCommodityDetailsUsingCSVFilePage? uploadCommodityDetailsUsingCSVFilePage => _objectContainer.IsRegistered<IUploadCommodityDetailsUsingCSVFilePage>() ? _objectContainer.Resolve<IUploadCommodityDetailsUsingCSVFilePage>() : null;

        public UploadCommodityDetailsUsingCSVFilePageSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }


        [Then("Upload commodity details using a CSV file page should be displayed")]
        public void ThenUploadCommodityDetailsUsingACSVFilePageShouldBeDisplayed()
        {
            Assert.True(uploadCommodityDetailsUsingCSVFilePage?.IsPageLoaded(), "Upload commodity details using a CSV file page is not displayed");
        }

        [When("the user clicks Download the CSV template and read the guide on how to complete this \\(opens in new tab)")]
        public void WhenTheUserClicksDownloadTheCSVTemplateAndReadTheGuideOnHowToCompleteThisOpensInNewTab()
        {
            uploadCommodityDetailsUsingCSVFilePage?.ClickDownloadCSVTemplateLink();
        }

        [Then("the guidance to upload commodity details using a CSV file page will open in new tab")]
        public void ThenTheGuidanceToUploadCommodityDetailsUsingACSVFilePageWillOpenInNewTab()
        {
            Assert.True(uploadCommodityDetailsUsingCSVFilePage?.IsGuidancePageLoadedInNewTab(), "Guidance IPAFFS: upload commodity details using a CSV file page is not displayed");
        }

        [When("user downloads the template by clicking the link {string}")]
        public void WhenUserDownloadsTheTemplateByClickingTheLink(string linkName)
        {
            var oldFile = Path.Combine(Path.GetTempPath(), "automation-downloads", "IPAFFS-commodity-details-CSV-template-xls.xlsx");
            uploadCommodityDetailsUsingCSVFilePage?.ClickCommDetailsCSVTemplateLink(linkName, oldFile);
        }

        [When("the user clicks the downloaded file {string} and open it")]
        public void WhenTheUserClicksTheDownloadedFileAndOpenIt(string fileName)
        {
            uploadCommodityDetailsUsingCSVFilePage?.ClickDownloadedFile(fileName);
        }

        [Then("validates the document {string} should have column headers {string} {string} {string} {string} {string} {string} {string} {string} {string} {string} {string} {string}")]
        public void ThenValidatesTheDocumentShouldHaveColumnHeaders(string fileName, string commCode, string genusAndSpecies, string eppo, string variety, string commClass, string intendedOption, string numOfPackage, string packageType, string quantity, string quantityType, string netWeight, string controlledAtmosContainer)
        {
            List<string> expectedHeaders = new List<string>
            {
                commCode, genusAndSpecies, eppo, variety, commClass,
                intendedOption, numOfPackage, packageType, quantity, 
                quantityType, netWeight, controlledAtmosContainer
            };

            var filePath = Path.Combine(Path.GetTempPath(), "automation-downloads", $"{fileName}");

            uploadCommodityDetailsUsingCSVFilePage?.VerifyExcelFileHeaders(filePath, expectedHeaders);
        }


        [Then(@"validates the document {string} should have column headers ""(.*)""")]
        public void WhenValidatesTheDocumentShouldHaveColumnHeaders(string fileName, string headers)
        {
            List<string> expectedHeaders = headers
                .Split(',')
                .Select(h => h.Trim().Trim('\''))
                .ToList();

            var filePath = Path.Combine(Path.GetTempPath(), "automation-downloads", $"{fileName}");

            uploadCommodityDetailsUsingCSVFilePage?.VerifyExcelFileHeaders(filePath, expectedHeaders);
        }

        [When("the user creates a CSV file from the downloaded template {string} with below data and uploads the CSV file")]
        public void WhenTheUserCreatesACSVFileFromTheDownloadedTemplateWithBelowDataAndUploadsTheCSVFile(string fileName, DataTable dataTable)
        {
            var templateFilePath = Path.Combine(Path.GetTempPath(), "automation-downloads", $"{fileName}");
            var csvFilePath = uploadCommodityDetailsUsingCSVFilePage?.CreateCSVFromExcelTemplate(templateFilePath, dataTable);

            uploadCommodityDetailsUsingCSVFilePage?.SelectCSVFile(csvFilePath);
            uploadCommodityDetailsUsingCSVFilePage?.ClickUploadButton();

            int rowsCount = dataTable.Rows.Count;
            _scenarioContext["NumberOfCommodities"] = rowsCount;
            _scenarioContext["AllCommodityDetails"] = dataTable;
        }
    }
}
