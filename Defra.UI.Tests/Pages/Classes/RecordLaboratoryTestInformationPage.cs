using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.UI.Tests.Pages.Classes
{
    public class RecordLaboratoryTestInformationPage : IRecordLaboratoryTestInformationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1"), true);
        private IWebElement rdoConclusion(string decision) => _driver.FindElement(By.XPath($"//input[@value='{decision}']"));
        private IWebElement txtUseByDay => _driver.WaitForElement(By.Name("sample-use-by-date-day"));
        private IWebElement txtUseByMonth => _driver.WaitForElement(By.Name("sample-use-by-date-month"));
        private IWebElement txtUseByYear => _driver.WaitForElement(By.Name("sample-use-by-date-year"));
        private IWebElement txtReleasedDay => _driver.WaitForElement(By.Name("released-date-day"));
        private IWebElement txtReleasedMonth => _driver.WaitForElement(By.Name("released-date-month"));
        private IWebElement txtReleasedYear => _driver.WaitForElement(By.Name("released-date-year"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public RecordLaboratoryTestInformationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Record laboratory test information");
        }

        public void SelectConclusion(string decision)
        {
            rdoConclusion(decision).Click();
        }

        public void EnterSampleUseByDate(string day, string month, string year)
        {
            txtUseByDay.SendKeys(day);
            txtUseByMonth.SendKeys(month);
            txtUseByYear.SendKeys(year);
        }
        
        public void EnterReleasedDate(string day, string month, string year)
        {
            txtReleasedDay.SendKeys(day);
            txtReleasedMonth.SendKeys(month);
            txtReleasedYear.SendKeys(year);
        }

    }
}
