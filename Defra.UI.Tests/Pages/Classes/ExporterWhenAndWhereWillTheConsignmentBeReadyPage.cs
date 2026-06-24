using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhenAndWhereWillTheConsignmentBeReadyPage : IExporterWhenAndWhereWillTheConsignmentBeReadyPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='When and where will the consignment be ready?']"), true);
        private IWebElement txtDateDay => _driver.WaitForElement(By.Id("inspection_date_day"), true);
        private IWebElement txtDateMonth => _driver.WaitForElement(By.Id("inspection_date_month"), true);
        private IWebElement txtDateYear => _driver.WaitForElement(By.Id("inspection_date_year"), true);
        private IWebElement txtTimeHour => _driver.WaitForElement(By.Id("inspection_time_hour"), true);
        private IWebElement txtTimeMinute => _driver.WaitForElement(By.Id("inspection_time_minute"), true);
        private IWebElement drpTimeMeridiem => _driver.WaitForElement(By.Id("inspection_time_meridiem"), true);
        private IWebElement txtPlace => _driver.WaitForElement(By.Id("inspection-specific-location"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterWhenAndWhereWillTheConsignmentBeReadyPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("When and where will the consignment be ready?");

        public void EnterReadyDateTimeAndPlace(DateTime date, string hhmm, string meridiem, string place)
        {
            EnterDate(date);
            EnterTime(hhmm, meridiem);
            EnterPlace(place);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

        private void EnterDate(DateTime date)
        {
            txtDateDay.Clear();
            txtDateDay.SendKeys(date.Day.ToString("00"));

            txtDateMonth.Clear();
            txtDateMonth.SendKeys(date.Month.ToString("00"));

            txtDateYear.Clear();
            txtDateYear.SendKeys(date.Year.ToString());
        }

        private void EnterTime(string hhmm, string meridiem)
        {
            var timeParts = hhmm.Split(':');

            txtTimeHour.Clear();
            txtTimeHour.SendKeys(timeParts[0]);

            txtTimeMinute.Clear();
            txtTimeMinute.SendKeys(timeParts.Length > 1 ? timeParts[1] : "00");

            new SelectElement(drpTimeMeridiem).SelectByValue(meridiem.ToLower());
        }

        private void EnterPlace(string place)
        {
            txtPlace.Clear();
            txtPlace.SendKeys(place);
        }
    }
}