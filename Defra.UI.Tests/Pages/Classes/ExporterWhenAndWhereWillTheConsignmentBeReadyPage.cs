using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhenAndWhereWillTheConsignmentBeReadyPage : IExporterWhenAndWhereWillTheConsignmentBeReadyPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='When and where will the consignment be ready?']"), true);
        private IWebElement txtDateDay => _driver.WaitForElement(By.XPath("//input[contains(@id,'date') and contains(@id,'day')]"), true);
        private IWebElement txtDateMonth => _driver.WaitForElement(By.XPath("//input[contains(@id,'date') and contains(@id,'month')]"), true);
        private IWebElement txtDateYear => _driver.WaitForElement(By.XPath("//input[contains(@id,'date') and contains(@id,'year')]"), true);
        private IWebElement txtTimeHour => _driver.WaitForElement(By.XPath("//input[contains(@id,'time') and contains(@id,'hour')]"), true);
        private IWebElement txtTimeMinute => _driver.WaitForElement(By.XPath("//input[contains(@id,'time') and contains(@id,'minute')]"), true);
        private IWebElement txtPlace => _driver.WaitForElement(By.XPath("//label[contains(normalize-space(),'Place')]/following::input[1]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Save and continue'] | //input[@value='Save and continue']"));
        #endregion

        public ExporterWhenAndWhereWillTheConsignmentBeReadyPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("When and where will the consignment be ready?");

        public void EnterReadyDateTimeAndPlace(DateTime date, string hhmm, string place)
        {
            EnterDate(date);
            EnterTime(hhmm);
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

        private void EnterTime(string hhmm)
        {
            var timeParts = hhmm.Split(':');

            txtTimeHour.Clear();
            txtTimeHour.SendKeys(timeParts[0]);

            txtTimeMinute.Clear();
            txtTimeMinute.SendKeys(timeParts.Length > 1 ? timeParts[1] : "00");
        }

        private void EnterPlace(string place)
        {
            txtPlace.Clear();
            txtPlace.SendKeys(place);
        }
    }
}