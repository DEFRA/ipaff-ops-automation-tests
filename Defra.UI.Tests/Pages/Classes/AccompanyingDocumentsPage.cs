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
    public class AccompanyingDocumentsPage : IAccompanyingDocumentsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement ddlDocumentType => _driver.WaitForElement(By.Name("document-type"));
        private IWebElement txtDocumentReference => _driver.WaitForElement(By.Name("document-reference"));
        private IWebElement txtDay => _driver.WaitForElement(By.Name("document-issue-date-day"));
        private IWebElement txtMonth => _driver.WaitForElement(By.Name("document-issue-date-month"));
        private IWebElement txtYear => _driver.WaitForElement(By.Name("document-issue-date-year"));
        private IWebElement datePickerIcon => _driver.WaitForElement(By.ClassName("date-picker__reveal__icon"));
        private IWebElement nextMonthButton => _driver.WaitForElement(By.ClassName("date-picker__button__next-month"));
        private IWebElement firstDateOfTheMonth => _driver.WaitForElement(By.XPath("//td/button[text()='1']"));
        private By documentDateBy => By.XPath("//div[contains(@id,'additional-document-date-value')]");
        private IWebElement documentDate => _driver.WaitForElement(documentDateBy);
        private IWebElement addAttachmentLink => _driver.WaitForElement(By.Id("add-attachment"));
        private IWebElement nextButton => _driver.WaitForElement(By.Id("next-button"));
        private IWebElement fileName => _driver.WaitForElement(By.XPath("//a[contains(@id,'attachment-view-')]"));
        private List<IWebElement> datePickerDateList => _driver.WaitForElements(By.XPath("//table[@class='date-picker__date-table']//tr/td")).ToList();

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private readonly object @lock = new();

        public AccompanyingDocumentsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Documents")
                && primaryTitle.Text.Contains("Accompanying documents");
        }
        
        public void SelectDocumentType(string type)
        { 
            new SelectElement(ddlDocumentType).SelectByText(type); 
        }
      
        public void EnterDocumentReference(string reference)
        { 
            txtDocumentReference.Click(); 
            txtDocumentReference.Clear(); 
            txtDocumentReference.SendKeys(reference); 
        }

        public void EnterDateOfIssue(string day, string month, string year)
        { 
            txtDay.Click(); 
            txtDay.SendKeys(day);
            txtMonth.Click();
            txtMonth.SendKeys(month);
            txtYear.Click();
            txtYear.SendKeys(year);
        }

        public bool IsDatePickerIconDisplayed()
        {
            return datePickerIcon.Displayed;
        }

        public void SelectDateFromDatePicker()
        {
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(datePickerIcon)).Click();
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(nextMonthButton)).Click();
            firstDateOfTheMonth.Click();
        }

        public string? GetDocumentIssueDate()
        {
            try
            {
                _driver.WaitForElementCondition(ExpectedConditions.ElementIsVisible(documentDateBy));
                var text = documentDate.Text.Trim();
                // Convert "1 January 2026" to "01 01 2026" format
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MM yyyy");
                }
                return text;
            }
            catch { return null; }
        }

        public void ClickAddAttachmentLink() => addAttachmentLink.Click();

        public void AddAccompanyingDocument(string fileName)
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
    }
}