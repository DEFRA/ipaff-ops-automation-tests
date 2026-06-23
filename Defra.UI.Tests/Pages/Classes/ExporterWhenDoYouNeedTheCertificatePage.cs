using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhenDoYouNeedTheCertificatePage : IExporterWhenDoYouNeedTheCertificatePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(normalize-space(),'When do you need the certificate')]"), true);
        private IWebElement txtDateDay => _driver.WaitForElement(By.XPath("//input[contains(@id,'date') and contains(@id,'day')]"), true);
        private IWebElement txtDateMonth => _driver.WaitForElement(By.XPath("//input[contains(@id,'date') and contains(@id,'month')]"), true);
        private IWebElement txtDateYear => _driver.WaitForElement(By.XPath("//input[contains(@id,'date') and contains(@id,'year')]"), true);
        private IWebElement txtTimeHour => _driver.WaitForElement(By.XPath("//input[contains(@id,'time') and contains(@id,'hour')]"), true);
        private IWebElement txtTimeMinute => _driver.WaitForElement(By.XPath("//input[contains(@id,'time') and contains(@id,'minute')]"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Continue'] | //input[@value='Continue']"));
        #endregion

        public ExporterWhenDoYouNeedTheCertificatePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Contains("When do you need the certificate");

        public void EnterNeededDateAndTime(DateTime date, string hhmm)
        {
            EnterDate(date);
            EnterTime(hhmm);
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

        private void EnterTime(string hhmm)
        {
            var timeParts = hhmm.Split(':');

            txtTimeHour.Clear();
            txtTimeHour.SendKeys(timeParts[0]);

            txtTimeMinute.Clear();
            txtTimeMinute.SendKeys(timeParts.Length > 1 ? timeParts[1] : "00");
        }
    }
}