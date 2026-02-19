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
        private By selectDocumentsHeadingBy => By.XPath("//h2[contains(@class, 'govuk-heading-l') and contains(text(), 'Select documents to upload')]");
        private By dropzoneDivBy => By.CssSelector("div[data-module='dropzone']");
        private By dragDropTextBy => By.XPath("//p[contains(@class, 'govuk-body') and contains(text(), 'Drag and drop files here or')]");
        private By chooseFilesLabelBy => By.XPath("//label[contains(@class, 'govuk-button') and contains(text(), 'Choose files')]");
        private By uploadInputBy => By.CssSelector("input[type='file']");
        private By uploadedFilesListBy => By.CssSelector("div.uploaded-files-list-item");
        private By continueButtonBy => By.Id("next-button");
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
            var heading = _driver.FindElement(selectDocumentsHeadingBy);
            return heading.Text.Trim().Equals("Select documents to upload", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyDragAndDropFunctionality()
        {
            var dropzone = _driver.FindElement(dropzoneDivBy);
            var dragText = _driver.FindElement(dragDropTextBy);
            var chooseLabel = _driver.FindElement(chooseFilesLabelBy);
            var uploadInput = _driver.FindElement(uploadInputBy);

            var dropzoneExists = dropzone.Displayed;
            var dragDropTextExists = dragText.Text.Trim().Equals("Drag and drop files here or", StringComparison.OrdinalIgnoreCase);
            var chooseFilesButtonExists = chooseLabel.Text.Trim().Equals("Choose files", StringComparison.OrdinalIgnoreCase);
            var fileInputExists = uploadInput.GetAttribute("type").Equals("file", StringComparison.OrdinalIgnoreCase);
            var multipleUploadEnabled = uploadInput.GetAttribute("multiple") != null;

            return dropzoneExists && dragDropTextExists && chooseFilesButtonExists && fileInputExists && multipleUploadEnabled;
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

            // Check file count matches
            if (uploadedFiles.Count != expectedFileCount)
            {
                return false;
            }

            // Check all files are displayed
            if (!uploadedFiles.All(file => file.Displayed))
            {
                return false;
            }

            // If expected file names are provided, verify they match
            if (expectedFileNames != null && expectedFileNames.Length > 0)
            {
                var displayedFileNames = uploadedFiles.Select(file => file.Text.Trim()).ToList();

                // Check each expected file name is present in the displayed files
                foreach (var expectedFileName in expectedFileNames)
                {
                    if (!displayedFileNames.Any(displayed => displayed.Equals(expectedFileName, StringComparison.OrdinalIgnoreCase)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void ClickContinue()
        {
            var continueButton = _driver.FindElement(continueButtonBy);
            continueButton.Click();
        }
    }
}