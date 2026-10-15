using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhenWillTheSeedsBeReadyForSamplingPage : IExporterWhenWillTheSeedsBeReadyForSamplingPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("inspection-date-time-heading"), true);
        private IWebElement txtDay => _driver.WaitForElement(By.Id("inspection_date_day"));
        private IWebElement txtMonth => _driver.WaitForElement(By.Id("inspection_date_month"));
        private IWebElement txtYear => _driver.WaitForElement(By.Id("inspection_date_year"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterWhenWillTheSeedsBeReadyForSamplingPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("When will the seeds be ready for sampling?");

        public void EnterReadyDate(DateTime date)
        {
            txtDay.Clear();
            txtDay.SendKeys(date.Day.ToString());
            txtMonth.Clear();
            txtMonth.SendKeys(date.Month.ToString());
            txtYear.Clear();
            txtYear.SendKeys(date.Year.ToString());
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}
