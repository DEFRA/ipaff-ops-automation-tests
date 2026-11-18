using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Reqnroll.BoDi;
using System.Reflection;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterSupportingDocumentsPage : IExporterSupportingDocumentsPage
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

        public ExporterSupportingDocumentsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Supporting documents");

        public void UploadDocument(string fileName, string description)
        {
            var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(dirPath!, "Data", "Documents", fileName);

            if (_driver is IAllowsFileDetection allowsDetection)
                allowsDetection.FileDetector = new LocalFileDetector();

            fileInput.SendKeys(filePath);
            txtDescription.Clear();
            txtDescription.SendKeys(description);
            btnUpload.Click();
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}
