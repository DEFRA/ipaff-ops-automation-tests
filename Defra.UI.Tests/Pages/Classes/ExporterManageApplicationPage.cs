using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterManageApplicationPage : IExporterManageApplicationPage
    {
        private const string ReportFilePattern = "seedSamplingReport_*.csv";
        private const string RemainingQuantityColumn = "Quantity remaining";

        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private static string DownloadDirectory => Path.Combine(Path.GetTempPath(), "automation-downloads");
        private DateTime _downloadStartedUtc;

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("application-summary-heading"), true);
        private IWebElement lnkSamplingReport => _driver.WaitForElement(By.Id("seed-sampling-report-link"));
        #endregion

        public ExporterManageApplicationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().StartsWith("Manage application");

        public void ClickSamplingReportLink()
        {
            Directory.CreateDirectory(DownloadDirectory);
            foreach (var oldFile in Directory.GetFiles(DownloadDirectory, ReportFilePattern))
            {
                File.Delete(oldFile);
            }

            _downloadStartedUtc = DateTime.UtcNow;
            lnkSamplingReport.Click();
        }

        public bool IsSamplingReportDownloaded() => GetDownloadedReportPath() != null;

        public string GetRemainingApprovedStockQuantity()
        {
            var reportPath = GetDownloadedReportPath()
                ?? throw new FileNotFoundException($"No '{ReportFilePattern}' found in {DownloadDirectory}");

            var lines = File.ReadAllLines(reportPath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
            var headers = ParseCsvLine(lines[0]);
            var index = headers.FindIndex(h => h.Equals(RemainingQuantityColumn, StringComparison.OrdinalIgnoreCase));
            if (index < 0)
            {
                throw new InvalidOperationException($"Column '{RemainingQuantityColumn}' not found in {Path.GetFileName(reportPath)}");
            }
            if (lines.Length < 2)
            {
                throw new InvalidOperationException($"No data rows found in {Path.GetFileName(reportPath)}");
            }

            return ParseCsvLine(lines[1])[index].Trim();
        }

        private string? GetDownloadedReportPath()
        {
            var timeout = DateTime.Now.AddSeconds(30);
            while (DateTime.Now < timeout)
            {
                foreach (var name in GetGridReportFileNames())
                {
                    Utils.RetrieveFileFromGrid(_driver, name);
                }

                var file = Directory.Exists(DownloadDirectory)
                    ? new DirectoryInfo(DownloadDirectory).GetFiles(ReportFilePattern)
                        .Where(f => f.LastWriteTimeUtc >= _downloadStartedUtc.AddSeconds(-5) && f.Length > 0)
                        .OrderByDescending(f => f.LastWriteTimeUtc)
                        .FirstOrDefault()
                    : null;

                if (file != null)
                {
                    return file.FullName;
                }
                Thread.Sleep(1000);
            }
            return null;
        }

        // The report name contains a timestamp, so list the Grid node downloads to find it
        private IEnumerable<string> GetGridReportFileNames(string gridUrl = "http://localhost:4444")
        {
            if (_driver is not OpenQA.Selenium.Remote.RemoteWebDriver remoteDriver)
            {
                return Enumerable.Empty<string>();
            }

            try
            {
                using var client = new HttpClient();
                var body = client.GetStringAsync($"{gridUrl}/session/{remoteDriver.SessionId}/se/files").Result;
                using var doc = System.Text.Json.JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("value", out var value)
                    && value.TryGetProperty("names", out var names))
                {
                    return names.EnumerateArray()
                        .Select(n => n.GetString() ?? string.Empty)
                        .Where(n => n.StartsWith("seedSamplingReport_") && n.EndsWith(".csv"))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GRID DOWNLOAD] Unable to list Grid downloads: {ex.Message}");
            }
            return Enumerable.Empty<string>();
        }

        private static List<string> ParseCsvLine(string line)
        {
            var result = new List<string>();
            var current = new System.Text.StringBuilder();
            var inQuotes = false;

            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }
            result.Add(current.ToString());
            return result;
        }
    }
}
