using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class RecordControlPage : IRecordControlPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement rdoConsignmentLeave(string decision) => _driver.FindElement(By.XPath($"//input[@value='{decision}']"));
        private IWebElement drpMeansOfTransport => _driver.FindElement(By.Id("transport-type"));
        private IWebElement txtIdentification => _driver.FindElement(By.Id("identification"));
        private IWebElement txtDocumentation => _driver.FindElement(By.Id("documentation"));
        private IWebElement datePickerIcon => _driver.WaitForElement(By.XPath("//button[@class='date-picker__reveal__icon']"));
        private IWebElement nextMonthButton => _driver.WaitForElement(By.XPath("//button[@class='date-picker__button__next-month']"));
        private IWebElement firstDateOfTheMonth => _driver.WaitForElement(By.XPath("//td/button[text()='1']"));
        private IWebElement drpExitBCP => _driver.FindElement(By.Id("exitBIP"));
        private IWebElement txtLeaveDateDay => _driver.FindElement(By.Id("leave-date-day"));
        private IWebElement txtLeaveDateMonth => _driver.FindElement(By.Id("leave-date-month"));
        private IWebElement txtLeaveDateYear => _driver.FindElement(By.Id("leave-date-year"));
        private IWebElement drpDestinationCountry => _driver.FindElement(By.Id("redispatching-country"));
        private IWebElement btnSubmitControl => _driver.FindElement(By.Id("button-save-and-continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public RecordControlPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Record control");
        }

        public void SelectConsignmentLeaveRadio(string consignmentLeaveDecision)
        {
            rdoConsignmentLeave(consignmentLeaveDecision).Click();
        }

        public void SelectMeansOfTransport(string transport)
        {
            new SelectElement(drpMeansOfTransport).SelectByText(transport);
        }

        public void EnterIdentification(string identification)
        {
            txtIdentification.SendKeys(identification);
        }

        public void EnterDocumentation(string identification)
        {
            txtIdentification.Clear();
            txtIdentification.SendKeys(identification);
        }

        public void SelectDateFromDatePicker()
        {
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(datePickerIcon)).Click();
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(nextMonthButton)).Click();
            firstDateOfTheMonth.Click();
        }

        public void SelectExitBCP(string exitBCP)
        {
            new SelectElement(drpExitBCP).SelectByText(exitBCP);
        }

        public void SelectDestinationCountry(string destination)
        {
            new SelectElement(drpDestinationCountry).SelectByText(destination);
        }

        public void ClickSubmitControlButton() => btnSubmitControl.Click();

        public void EnterDate(string day, string month, string year)
        {
            txtLeaveDateDay.SendKeys(day);
            txtLeaveDateMonth.SendKeys(month);
            txtLeaveDateYear.SendKeys(year);
        }
        #endregion
    }
}