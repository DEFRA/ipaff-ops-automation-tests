using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterUploadAFilePage : IExporterUploadAFilePage
    {
        private const string TemplateFileName = "template-seed-sample.csv";
        private const string UnitsColumnName = "Quantity units";

        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private static string TemplateFilePath => Path.Combine(Path.GetTempPath(), "automation-downloads", TemplateFileName);

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Upload a file']"), true);
        private IWebElement lnkTemplate => _driver.WaitForElement(By.Id("template"));
        private IWebElement lnkBack => _driver.WaitForElement(By.Id("link-back"));
        #endregion

        public ExporterUploadAFilePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Upload a file");

        public void DownloadCsvTemplate()
        {
            if (File.Exists(TemplateFilePath))
            {
                File.Delete(TemplateFilePath);
            }

            var originalWindow = _driver.CurrentWindowHandle;
            var windowCount = _driver.WindowHandles.Count;

            lnkTemplate.Click();

            Utils.RetrieveFileFromGrid(_driver, TemplateFileName);

            if (_driver.WindowHandles.Count > windowCount)
            {
                _driver.SwitchTo().Window(_driver.WindowHandles.Last());
                _driver.Close();
            }
            _driver.SwitchTo().Window(originalWindow);
        }

        public bool IsCsvTemplateDownloaded()
        {
            var timeout = DateTime.Now.AddSeconds(30);
            while (DateTime.Now < timeout)
            {
                if (File.Exists(TemplateFilePath) && new FileInfo(TemplateFilePath).Length > 0)
                {
                    return true;
                }
                Thread.Sleep(500);
            }
            return false;
        }

        public List<string> GetUnitsOfMeasurementFromTemplate()
        {
            var lines = File.ReadAllLines(TemplateFilePath);
            var unitsIndex = Array.IndexOf(lines[0].Split(','), UnitsColumnName);
            if (unitsIndex < 0)
            {
                throw new InvalidOperationException($"Column '{UnitsColumnName}' not found in {TemplateFileName}");
            }

            // Skip header and the OPTIONAL/MANDATORY guidance row
            return lines.Skip(2)
                .Select(line => line.Split(','))
                .Where(cells => cells.Length > unitsIndex && !string.IsNullOrWhiteSpace(cells[unitsIndex]))
                .Select(cells => cells[unitsIndex].Trim())
                .ToList();
        }

        public void ClickBackLink() => lnkBack.Click();
    }
}
