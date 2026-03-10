using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;

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
        private IWebElement useByDateDatePickerIcon => _driver.WaitForElement(By.XPath("//div[@id='sample-use-by-date']//button[@class='date-picker__reveal__icon']"));
        private IWebElement releasedDateDatePickerIcon => _driver.WaitForElement(By.XPath("//div[@id='released-date']//button[@class='date-picker__reveal__icon']"));
        private IWebElement nextMonthButton(string date) => _driver.WaitForElement(By.XPath($"//div[@id='{date}']//button[@class='date-picker__button__next-month']"));
        private IWebElement firstDateOfTheMonth => _driver.WaitForElement(By.XPath("//div[@id='sample-use-by-date']//td/button[text()='1']"));
        private IWebElement secondDateOfTheMonth => _driver.WaitForElement(By.XPath("//div[@id='released-date']//td/button[text()='2']"));
        private IWebElement txtLabTestMethod => _driver.WaitForElement(By.Id("lab-test-method"));
        private IWebElement txtResults => _driver.WaitForElement(By.Name("lab-test-results"));
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

        public bool IsUseByDatePickerIconDisplayed() => useByDateDatePickerIcon.Displayed;

        public bool IsReleasedDatePickerIconDisplayed() => releasedDateDatePickerIcon.Displayed;

        public void SelectSampleUseByDateFromDatePicker()
        {
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(useByDateDatePickerIcon)).Click();
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(nextMonthButton("sample-use-by-date"))).Click();
            firstDateOfTheMonth.Click();
        }

        public void SelectReleasedDateFromDatePicker()
        {
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(releasedDateDatePickerIcon)).Click();
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(nextMonthButton("released-date"))).Click();
            secondDateOfTheMonth.Click();
        }

        public void EnterLabTestMethod(string testMethod) => txtLabTestMethod.SendKeys(testMethod);

        public void EnterResults(string results) => txtResults.SendKeys(results);
    }
}