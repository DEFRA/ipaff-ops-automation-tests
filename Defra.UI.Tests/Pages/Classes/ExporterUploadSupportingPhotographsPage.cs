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
        private By uploadedFileRow(string fileName) =>
            By.XPath($"//table[@id='file-upload-table']//tr[td[contains(normalize-space(),'{fileName}')]]");
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

            // Each photograph must have a unique file name
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(sourceFileName)} {DateTime.Now:ddMMyyyyHHmmss}{Path.GetExtension(sourceFileName)}";
            var uploadPath = Path.Combine(Path.GetTempPath(), uniqueFileName);
            File.Copy(sourcePath, uploadPath, true);

            if (_driver is IAllowsFileDetection allowsDetection)
                allowsDetection.FileDetector = new LocalFileDetector();

            fileInput.SendKeys(uploadPath);
            txtDescription.Clear();
            txtDescription.SendKeys(description);
            btnUpload.Click();

            return uniqueFileName;
        }

        public bool IsPhotographListedWithActions(string fileName, string description)
        {
            var row = _driver.WaitForElement(uploadedFileRow(fileName), true);
            if (row == null)
                return false;

            var rowText = row.Text;
            var hasDownload = row.FindElements(By.XPath(".//*[self::a or self::button or self::input][contains(normalize-space(.),'Download') or @value='Download']")).Count > 0;
            var hasDelete = row.FindElements(By.XPath(".//*[self::a or self::button or self::input][contains(normalize-space(.),'Delete') or @value='Delete']")).Count > 0;

            return rowText.Contains(description) && hasDownload && hasDelete;
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}
