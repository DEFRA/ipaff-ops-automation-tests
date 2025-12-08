using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Contracts;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReviewOutcomeDecisionPage : IReviewOutcomeDecisionPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.FindElement(By.XPath("//*[@class=' govuk-heading-xl heading-with-help']"));
        private IWebElement btnSubmitDecision => _driver.WaitForElement(By.Id("submit-decision"));
        private IWebElement inputDay => _driver.WaitForElement(By.Id("date-of-checks-day"));
        private IWebElement inputMonth => _driver.WaitForElement(By.Id("date-of-checks-month"));
        private IWebElement inputYear => _driver.WaitForElement(By.Id("date-of-checks-year"));
        private IWebElement inputHour => _driver.WaitForElement(By.Id("time-of-checks-hour"));
        private IWebElement inputMinutes => _driver.WaitForElement(By.Id("time-of-checks-minute"));
        private IWebElement rdoCertifyingOfficer => _driver.FindElement(By.Id("signed-by-official"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ReviewOutcomeDecisionPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Trim().Contains("Review outcome decision");
        }

        public void ClickSubmitDecision()
        {
            btnSubmitDecision.Click();
        }

        public void EnterCurrentDateAndTime(string day, string month, string year, string hours, string minutes)
        {
            inputDay.Click();
            inputDay.SendKeys(day);
            inputMonth.Click();
            inputMonth.SendKeys(month);
            inputYear.Click();
            inputYear.SendKeys(year);
            inputHour.Click();
            inputHour.SendKeys(hours);
            inputMinutes.Click();
            inputMinutes.SendKeys(minutes);
        }

        public void SelectCertifyingOfficerRadioButton()
        {
            rdoCertifyingOfficer.Click();
        }
    }
}