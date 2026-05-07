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

        // The native file picker can't be driven by Selenium; SendKeys to the
        // <input type="file"> handles both 'choose file' and 'select file' steps.
        public void ClickChooseFileButton()
        {
            // No-op: the underlying <input type="file"> is driven via SendKeys in SelectBulkUploadFile.
            // Provided as a separate method so the BDD step reads naturally.
            _ = fileInput;
        }

        public void SelectBulkUploadFile(string fileName)
        {
            var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fullPath = Path.Combine(dirPath, "Data", "Rules", fileName);

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
    }
}