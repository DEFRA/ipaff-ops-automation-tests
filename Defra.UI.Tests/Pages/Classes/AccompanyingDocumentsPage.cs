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

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

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
    }
}