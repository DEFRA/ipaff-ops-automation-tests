using System.Reflection;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
<<<<<<< HEAD
using SeleniumExtras.WaitHelpers;
=======
>>>>>>> b9a7fde06c7a3a2654595566fc1feb42e06a309a

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
        private IWebElement DatePickerIcon => _driver.WaitForElement(By.ClassName("date-picker__reveal__icon"));
        private IWebElement NextMonthButton => _driver.WaitForElement(By.ClassName("date-picker__button__next-month"));
        private IWebElement FirstDateOfTheMonth => _driver.WaitForElement(By.XPath("//td/button[text()='1']"));
        private IWebElement AddAttachmentLink => _driver.WaitForElement(By.Id("add-attachment"));
        private IWebElement NextButton => _driver.WaitForElement(By.Id("next-button"));
        private IWebElement FileName => _driver.WaitForElement(By.XPath("//a[contains(@id,'attachment-view-')]"));
        private List<IWebElement> DatePickerDateList => _driver.WaitForElements(By.XPath("//table[@class='date-picker__date-table']//tr/td")).ToList();

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
            return DatePickerIcon.Displayed;
        }

        public void SelectDateFromDatePicker()
        {
            try
            {
                _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(DatePickerIcon)).Click();
                _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(NextMonthButton)).Click();
                FirstDateOfTheMonth.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Date element is not clicked: " + ex.Message);
            }
        }

        public void ClickAddAttachmentLink() => AddAttachmentLink.Click();

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
            NextButton.Click();
        }

        public string GetFileName => FileName.Text;
    }
}