using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhenDoYouNeedTheCertificatePage : IExporterWhenDoYouNeedTheCertificatePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(normalize-space(),'When do you need the certificate')]"), true);
        private IWebElement txtDateDay => _driver.WaitForElement(By.Id("date-needed-day"), true);
        private IWebElement txtDateMonth => _driver.WaitForElement(By.Id("date-needed-month"), true);
        private IWebElement txtDateYear => _driver.WaitForElement(By.Id("date-needed-year"), true);
        private IWebElement txtTimeHour => _driver.WaitForElement(By.Id("certificate_needed_time_hour"), true);
        private IWebElement txtTimeMinute => _driver.WaitForElement(By.Id("certificate_needed_time_minute"), true);
        private IWebElement drpTimeMeridiem => _driver.WaitForElement(By.Id("certificate_needed_time_meridiem"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterWhenDoYouNeedTheCertificatePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Contains("When do you need the certificate");

        public void EnterNeededDateAndTime(DateTime date, string hhmm, string meridiem)
        {
            EnterDate(date);
            EnterTime(hhmm, meridiem);
        }

        public void ClickContinueButton() => btnContinue.Click();

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
    }
}