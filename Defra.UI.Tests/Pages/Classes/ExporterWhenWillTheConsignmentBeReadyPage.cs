using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhenWillTheConsignmentBeReadyPage : IExporterWhenWillTheConsignmentBeReadyPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("inspection-date-time-heading"), true);
        private IWebElement txtDateDay => _driver.WaitForElement(By.Id("inspection_date_day"), true);
        private IWebElement txtDateMonth => _driver.WaitForElement(By.Id("inspection_date_month"), true);
        private IWebElement txtDateYear => _driver.WaitForElement(By.Id("inspection_date_year"), true);
        private IWebElement txtTimeHour => _driver.WaitForElement(By.Id("inspection_time_hour"), true);
        private IWebElement txtTimeMinute => _driver.WaitForElement(By.Id("inspection_time_minute"), true);
        private IWebElement drpTimeMeridiem => _driver.WaitForElement(By.Id("inspection_time_meridiem"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterWhenWillTheConsignmentBeReadyPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("When will the consignment be ready?");

        public void EnterReadyDate(DateTime date)
        {
            txtDateDay.Clear();
            txtDateDay.SendKeys(date.Day.ToString("00"));

            txtDateMonth.Clear();
            txtDateMonth.SendKeys(date.Month.ToString("00"));

            txtDateYear.Clear();
            txtDateYear.SendKeys(date.Year.ToString());
        }

        public void EnterReadyTime(string hour, string minute, string meridiem)
        {
            txtTimeHour.Clear();
            txtTimeHour.SendKeys(hour);

            txtTimeMinute.Clear();
            txtTimeMinute.SendKeys(minute);

            new SelectElement(drpTimeMeridiem).SelectByValue(meridiem);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}
