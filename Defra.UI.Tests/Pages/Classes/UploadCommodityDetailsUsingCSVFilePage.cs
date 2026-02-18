using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using Reqnroll.BoDi;
using System.Diagnostics;
using System.Text;

namespace Defra.UI.Tests.Pages.Classes
{
    public class UploadCommodityDetailsUsingCSVFilePage : IUploadCommodityDetailsUsingCSVFilePage
    {
        private IObjectContainer _objectContainer;
        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='commodity-details-upload-page']//span"), true);
        private IWebElement lnkDownloadCSVTemplate => _driver.FindElement(By.XPath("//*[@class='govuk-list govuk-list--number']//a"));
        private IWebElement primaryTitleGuidancePage => _driver.WaitForElement(By.XPath("//*[@class='gem-c-heading__text govuk-heading-l']"), true);
        private IWebElement secondaryTitleGuidancePage => _driver.WaitForElement(By.XPath("//*[@class='govuk-caption-xl gem-c-heading__context']"), true);
        private IWebElement lnkCommDetailsCSVTemplate(string linkName) => _driver.FindElement(By.XPath($"//a[normalize-space()='{linkName}']"));
        private IWebElement btnChooseFile => _driver.FindElement(By.Id("fileUpload"));
        private IWebElement btnUpload => _driver.FindElement(By.Id("button-upload"));
        /*private IWebElement txtUploadSuccessBannerContent => _driver.FindElement(By.XPath("//*[@class='govuk-notification-banner__content']/p"));
        private IWebElement txtInfoMessageHeading => _driver.FindElement(By.XPath("//h2[@class='govuk-heading-s']"));
        private IWebElement txtInfoMessageContent => _driver.FindElement(By.XPath("//p[@class='govuk-body']"));*/
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public UploadCommodityDetailsUsingCSVFilePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("Upload commodity details using a CSV file");
        }

        public void ClickDownloadCSVTemplateLink()
        {
            lnkDownloadCSVTemplate.Click();
        }

        public bool IsGuidancePageLoadedInNewTab()
        {
            var windowHandles = _driver.WindowHandles;
            if (windowHandles.Count > 1)
            {
                _driver.SwitchTo().Window(windowHandles.Last());
                return secondaryTitleGuidancePage.Text.Contains("Guidance")
                    && primaryTitleGuidancePage.Text.Contains("IPAFFS: upload commodity details using a CSV file");
            }
            return false;
        }

        public void ClickCommDetailsCSVTemplateLink(string linkName, string oldFile)
        {
            if (File.Exists(oldFile))
            {
                File.Delete(oldFile);
            }
            lnkCommDetailsCSVTemplate(linkName).Click();
        }

        public void ClickDownloadedFile(string fileName)
        {
            var downloadedFile = Path.Combine(Path.GetTempPath(), "automation-downloads", $"{fileName}");

            //Open the file
            Process.Start(new ProcessStartInfo
            {
                FileName = downloadedFile,
                UseShellExecute = true
            });
        }

        public void VerifyExcelFileHeaders(string filePath, List<string> expectedHeaders)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
            {
                var workbookPart = document.WorkbookPart;

                //Get First sheet
                var sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().First();
                var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);

                //Get First row (header row)
                var headerRow = worksheetPart.Worksheet
                                             .GetFirstChild<SheetData>()
                                             .Elements<Row>()
                                             .First();

                var sharedStringTable = workbookPart.SharedStringTablePart?.SharedStringTable;

                //Read header values
                List<string> actualHeaders = headerRow.Elements<Cell>()
                    .Select(cell =>
                    {
                        if (cell.CellValue == null)
                            return string.Empty;

                        string value = cell.CellValue.InnerText;

                        //if text is stored in Sharedstring table
                        if(cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                        {
                            return sharedStringTable
                            .ElementAt(int.Parse(value))
                            .InnerText
                            .Trim();
                        }

                        return value.Trim();
                    }).Take(expectedHeaders.Count).ToList();

                    
                //Compare order + values
                if(!actualHeaders.SequenceEqual(expectedHeaders))
                {
                    throw new Exception(
                        $"Header mismatch!\nExpected: {string.Join(", ", expectedHeaders)}\n" +
                        $"Actual: {string.Join(", ", actualHeaders)}"
                        );
                }
            }
        }


        public string CreateCSVFromExcelTemplate(string templateFilePath, DataTable dataTable)
        {
            string csvPath = Path.Combine(Path.GetDirectoryName(templateFilePath), "CommodityDetails.csv");

            Console.WriteLine("templateFilePath" + templateFilePath);
            Console.WriteLine("csvPath " + csvPath);

            if(File.Exists(csvPath))
                File.Delete(csvPath);

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(templateFilePath, false))
            {
                var workbookPart = document.WorkbookPart;
                var sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().First();
                var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);

                Row headerRow = worksheetPart.Worksheet
                                             .Descendants<Row>()
                                             .First();

                SharedStringTable sharedStrings = workbookPart.SharedStringTablePart?.SharedStringTable;

                var csv = new StringBuilder();

                var headers = headerRow.Elements<Cell>().Select(cell =>
                {
                    string value = cell.CellValue?.InnerText ?? "";

                    if (cell.DataType != null &&
                        cell.DataType?.Value == CellValues.SharedString)
                    {
                        value = sharedStrings.ElementAt(int.Parse(value)).InnerText;
                    }
                    
                    return value.Trim();
                });

                csv.AppendLine(string.Join(",", headers));

                foreach (var row in dataTable.Rows)
                {
                    string commodityCode = row.Values.First().Trim();
                    if(!System.Text.RegularExpressions.Regex.IsMatch(commodityCode, @"^\d{8}$"))
                    {
                        throw new Exception($"Invalid Commodity code '{commodityCode}'. " +
                            "Commodity code must be exactly 8 digits (example: 08105000).");                           
                    }

                    var rowValues = row.Values.Select((v, index) =>
                    {
                        string value = v?.Trim() ?? "";
                        value = value.Replace("\"", "\"\"");
                        return $"\"{value}\"";
                    });

                    csv.AppendLine(string.Join(",", row.Values));
                }

                File.WriteAllText(csvPath, csv.ToString(), new UTF8Encoding(false));
            }

            return csvPath;
        }

        public void SelectCSVFile(string csvFilePath)
        {
            btnChooseFile.SendKeys(csvFilePath);
        }

        public void ClickUploadButton()
        {
            btnUpload.Click();
        }
    }
}
