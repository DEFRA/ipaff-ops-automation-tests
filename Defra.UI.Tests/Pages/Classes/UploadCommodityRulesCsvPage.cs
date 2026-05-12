using System.IO;
using System.Reflection;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class UploadCommodityRulesCsvPage : IUploadCommodityRulesCsvPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Upload multiple commodity rules using a CSV file']"), true);
        private IWebElement fileInput => _driver.WaitForElement(By.Id("uploadfile"));
        private IWebElement btnUpload => _driver.WaitForElement(By.XPath("//button[normalize-space()='Upload']"));

        public UploadCommodityRulesCsvPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;

        public void ClickChooseFileButton()
        {
            _ = fileInput;
        }

        public void SelectBulkUploadFile(string fileName)
        {
            var fullPath = ResolveFilePath(fileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Bulk upload file not found at '{fullPath}'", fullPath);

            var allowsDetection = (IAllowsFileDetection)_driver;
            allowsDetection.FileDetector = new LocalFileDetector();

            fileInput.SendKeys(fullPath);
        }

        public bool IsSelectedFileNameDisplayed(string fileName)
        {
            var value = fileInput.GetAttribute("value") ?? string.Empty;
            return value.Contains(fileName, StringComparison.OrdinalIgnoreCase);
        }

        public void ClickUploadButton() => btnUpload.Click();

        /// <summary>
        /// Updates the Id column (first column) for the row whose Commodity code column
        /// matches the given commodity code (with leading apostrophe as stored in the CSV).
        /// Writes the change back to the file on disk so it can be uploaded in a subsequent iteration.
        /// </summary>
        public void UpdateCsvIdForCommodityCode(string fileName, string commodityCode, string newId)
        {
            var fullPath = ResolveFilePath(fileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Bulk update CSV file not found at '{fullPath}'", fullPath);

            var lines = File.ReadAllLines(fullPath).ToList();
            var commodityCodeWithPrefix = $"'{commodityCode}";

            for (int i = 1; i < lines.Count; i++) // skip header row
            {
                var columns = ParseCsvLine(lines[i]);
                // Commodity code is the 3rd column (index 2)
                if (columns.Count > 2 && columns[2].Trim() == commodityCodeWithPrefix)
                {
                    columns[0] = newId;
                    lines[i] = BuildCsvLine(columns);
                    break;
                }
            }

            File.WriteAllLines(fullPath, lines);
        }

        /// <summary>
        /// Reads the Id column value for the row whose Commodity code column matches the given code.
        /// </summary>
        public string GetCsvIdForCommodityCode(string fileName, string commodityCode)
        {
            var fullPath = ResolveFilePath(fileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Bulk update CSV file not found at '{fullPath}'", fullPath);

            var lines = File.ReadAllLines(fullPath);
            var commodityCodeWithPrefix = $"'{commodityCode}";

            for (int i = 1; i < lines.Length; i++)
            {
                var columns = ParseCsvLine(lines[i]);
                if (columns.Count > 2 && columns[2].Trim() == commodityCodeWithPrefix)
                {
                    return columns[0].Trim();
                }
            }

            throw new InvalidOperationException(
                $"No row found in '{fileName}' with Commodity code '{commodityCodeWithPrefix}'");
        }

        /// <summary>
        /// Copies the updated CSV from the build output directory back to the source
        /// Data/Rules directory so it survives a rebuild before the next scenario runs.
        /// </summary>
        public void CopyUpdatedCsvToSourceDirectory(string fileName)
        {
            var outputPath = ResolveFilePath(fileName);

            var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var sourceDir = Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "Data", "Rules"));
            var sourcePath = Path.Combine(sourceDir, fileName);

            File.Copy(outputPath, sourcePath, overwrite: true);
        }

        private static string ResolveFilePath(string fileName)
        {
            var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(dirPath!, "Data", "Rules", fileName);
        }

        /// <summary>
        /// Parses a single CSV line respecting quoted fields that may contain commas.
        /// </summary>
        private static List<string> ParseCsvLine(string line)
        {
            var fields = new List<string>();
            bool inQuotes = false;
            var current = new System.Text.StringBuilder();

            foreach (var ch in line)
            {
                if (ch == '"')
                {
                    inQuotes = !inQuotes;
                    current.Append(ch);
                }
                else if (ch == ',' && !inQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(ch);
                }
            }
            fields.Add(current.ToString());
            return fields;
        }

        /// <summary>
        /// Rebuilds a CSV line from parsed fields, preserving any existing quoting.
        /// </summary>
        private static string BuildCsvLine(List<string> fields) => string.Join(",", fields);
    }
}