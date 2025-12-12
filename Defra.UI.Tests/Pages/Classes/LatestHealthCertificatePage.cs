using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Faker;

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
        private IWebElement addAttachmentLink => _driver.WaitForElement(By.Id("add-attachment-latest-health-cert"));
        private By documentDateBy => By.XPath("//div[contains(@id,'additional-document-date-value')]");
        private IWebElement documentDate => _driver.WaitForElement(documentDateBy);
        private IWebElement fileName => _driver.WaitForElement(By.XPath("//a[contains(@id,'attachment-view-')]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

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

        public void ClickAddAttachmentLink() => addAttachmentLink.Click();

        public string GetFileName => fileName.Text;

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
    }
}