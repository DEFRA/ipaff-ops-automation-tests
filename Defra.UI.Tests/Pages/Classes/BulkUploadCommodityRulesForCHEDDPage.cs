using System.IO;
using System.Reflection;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class BulkUploadCommodityRulesForCHEDDPage : IBulkUploadCommodityRulesForCHEDDPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Bulk upload commodity rules for CHED-D']"), true);
        private IWebElement fileInput => _driver.WaitForElement(By.Id("file-upload-input"));
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Continue']"));
        #endregion

        public BulkUploadCommodityRulesForCHEDDPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Text.Trim().Equals("Bulk upload commodity rules for CHED-D");

        public void ClickChooseFileButton()
        {
            // No explicit "Choose file" button in CHED-D; the input itself triggers the dialog when clicked.
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

        public void ClickContinueButton() => btnContinue.Click();

        private static string ResolveFilePath(string fileName)
        {
            var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(dirPath!, "Data", "Rules", fileName);
        }
    }
}