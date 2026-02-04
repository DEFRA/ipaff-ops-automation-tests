using System.Reflection;
using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class LatestHealthCertificatePage : ILatestHealthCertificatePage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement txtDocumentReference => _driver.WaitForElement(By.Name("latest-vet-health-cert-reference"));
        private IWebElement txtDay => _driver.WaitForElement(By.Name("latest-vet-health-cert-issue-date-day"));
        private IWebElement txtMonth => _driver.WaitForElement(By.Name("latest-vet-health-cert-issue-date-month"));
        private IWebElement txtYear => _driver.WaitForElement(By.Name("latest-vet-health-cert-issue-date-year"));
        private IWebElement nextButton => _driver.WaitForElement(By.Id("next-button"));
        private IWebElement addAttachmentLink => _driver.WaitForElement(By.Id("add-attachment-latest-health-cert"));
        private By documentDateBy => By.XPath("//div[contains(@id,'additional-document-date-value')]");
        private IWebElement documentDate => _driver.WaitForElement(documentDateBy);
        private IWebElement fileName => _driver.WaitForElement(By.XPath("//a[contains(@id,'attachment-view-')]"));
        private By HealthCertificateRowsLocator => By.XPath("//div[@class='additional-documents__grid-row additional-document-info']");

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private readonly object @lock = new();

        public LatestHealthCertificatePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Documents")
                && primaryTitle.Text.Contains("Latest Health Certificate");
        }

        public void EnterDocumentReference(string reference)
        {
            txtDocumentReference.Clear();
            txtDocumentReference.SendKeys(reference);
        }

        public void EnterDateOfIssue(string day, string month, string year)
        {
            txtDay.SendKeys(day);
            txtMonth.SendKeys(month);
            txtYear.SendKeys(year);
        }

        public void AddHealthCertificate(string fileName)
        {
            var attachmentsLocator = _driver.FindElement(By.XPath("//input[@id='fileUpload']"));

            if (attachmentsLocator != null)
            {
                lock (@lock)
                {
                    var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var docPath = Path.Combine(dirPath, "Data", "Documents", fileName);

                    var allowsDetection = (IAllowsFileDetection)_driver;
                    allowsDetection.FileDetector = new LocalFileDetector();

                    attachmentsLocator.SendKeys(docPath);
                }
            }
            nextButton.Click();
        }

        public string GetFileName => fileName.Text;

        public void ClickAddAttachmentLink() => addAttachmentLink.Click();

        public string GetDocumentIssueDate()
        {
            _driver.WaitForElementCondition(ExpectedConditions.ElementIsVisible(documentDateBy));
            var text = documentDate.Text.Trim();
            // Convert "1 January 2026" to "01 01 2026" format
            if (DateTime.TryParse(text, out DateTime date))
            {
                return date.ToString("dd MM yyyy");
            }
            return string.Empty;
        }

        public bool AreHealthCertificatesPresent()
        {
            try
            {
                // Check if any document rows are present on the page
                // Using the HealthCertificateRowsLocator locator from page objects region
                var documents = _driver.FindElements(HealthCertificateRowsLocator);
                return documents.Count > 0;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}