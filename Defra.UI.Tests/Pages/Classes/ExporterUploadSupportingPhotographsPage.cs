using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Reqnroll.BoDi;
using System.Reflection;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterUploadSupportingPhotographsPage : IExporterUploadSupportingPhotographsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("supplementary-documents"), true);
        private IWebElement fileInput => _driver.WaitForElementExists(By.Id("file"), true);
        private IWebElement txtDescription => _driver.WaitForElement(By.Id("description"), true);
        private IWebElement btnUpload => _driver.WaitForElement(By.Id("Upload"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));        
        #endregion

        public ExporterUploadSupportingPhotographsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Upload supporting photographs");

        public string UploadPhotograph(string sourceFileName, string description)
        {
            var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var sourcePath = Path.Combine(dirPath!, "Data", "Documents", sourceFileName);

            if (_driver is IAllowsFileDetection allowsDetection)
                allowsDetection.FileDetector = new LocalFileDetector();

            fileInput.SendKeys(sourcePath);
            txtDescription.Clear();
            txtDescription.SendKeys(description);
            btnUpload.Click();

            return sourceFileName;
        }

        public bool IsPhotographListedWithActions(string fileName, string description)
        {
            var fileNameCell = _driver.WaitForElement(By.Id($"file_{fileName}"), true);
            if (fileNameCell == null || !fileNameCell.Text.Trim().Equals(fileName))
                return false;

            var descriptionCell = _driver.FindElements(By.Id($"description_{fileName}"));
            var hasDescription = descriptionCell.Count > 0 && descriptionCell[0].Text.Trim().Equals(description);

            var downloadLink = _driver.FindElements(By.Id($"view_link_{fileName}"));
            var hasDownload = downloadLink.Count > 0 && downloadLink[0].Text.Trim().StartsWith("Download");

            var deleteLink = _driver.FindElements(By.Id($"delete_link_{fileName}"));
            var hasDelete = deleteLink.Count > 0 && deleteLink[0].Text.Trim().StartsWith("Delete");

            return hasDescription && hasDownload && hasDelete;
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}
