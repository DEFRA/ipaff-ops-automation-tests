using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using System.Reflection;

namespace Defra.UI.Tests.Pages.Classes
{
    public class UploadCatchCertificatesPage : IUploadCatchCertificatesPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement selectDocumentsHeading => _driver.FindElement(By.XPath("//h2[contains(@class, 'govuk-heading-l') and contains(text(), 'Select documents to upload')]"));
        private IWebElement dropzoneDiv => _driver.FindElement(By.CssSelector("div[data-module='dropzone']"));
        private IWebElement dragDropText => _driver.FindElement(By.XPath("//p[contains(@class, 'govuk-body') and contains(text(), 'Drag and drop files here or')]"));
        private IWebElement chooseFilesLabel => _driver.FindElement(By.XPath("//label[contains(@class, 'govuk-button') and contains(text(), 'Choose files')]"));
        private IWebElement uploadInput => _driver.FindElement(By.CssSelector("input[type='file']"));
        private IWebElement continueButton => _driver.FindElement(By.Id("next-button"));
        private By uploadInputBy => By.CssSelector("input[type='file']");
        private By uploadedFilesListBy => By.CssSelector("div.uploaded-files-list-item");
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public UploadCatchCertificatesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string pageTitle)
        {
            return primaryTitle.Text.Trim().Equals(pageTitle, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifySelectDocumentsHeading()
        {
            return selectDocumentsHeading.Text.Trim().Equals("Select documents to upload", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyDragAndDropFunctionality()
        {
            return dropzoneDiv.Displayed &&
                   dragDropText.Text.Trim().Equals("Drag and drop files here or", StringComparison.OrdinalIgnoreCase) &&
                   chooseFilesLabel.Text.Trim().Equals("Choose files", StringComparison.OrdinalIgnoreCase) &&
                   uploadInput.GetAttribute("type").Equals("file", StringComparison.OrdinalIgnoreCase) &&
                   uploadInput.GetAttribute("multiple") != null;
        }

        public void UploadMultipleCatchCertificates(params string[] fileNames)
        {
            var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var filePaths = fileNames
                .Select(fileName => Path.Combine(dirPath, "Data", "Documents", fileName))
                .ToArray();

            var allPaths = string.Join("\n", filePaths);

            var allowsDetection = (IAllowsFileDetection)_driver;
            allowsDetection.FileDetector = new LocalFileDetector();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            var fileInput = wait.Until(d => d.FindElement(uploadInputBy));

            fileInput.SendKeys(allPaths);
        }

        public bool VerifyFilesAreDisplayed(int expectedFileCount, params string[] expectedFileNames)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var uploadedFiles = wait.Until(d => d.FindElements(uploadedFilesListBy));

            return uploadedFiles.Count == expectedFileCount &&
                   uploadedFiles.All(file => file.Displayed) &&
                   (expectedFileNames == null || expectedFileNames.Length == 0 ||
                    expectedFileNames.All(expectedFileName =>
                        uploadedFiles.Any(file =>
                            file.Text.Trim().Equals(expectedFileName, StringComparison.OrdinalIgnoreCase))));
        }

        public void ClickContinue() => continueButton.Click();
    }
}