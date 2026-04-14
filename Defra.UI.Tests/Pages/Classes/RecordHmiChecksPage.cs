using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class RecordHmiChecksPage : IRecordHmiChecksPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//span[@id='page-primary-title' and text()='Record HMI checks']"));
        private IReadOnlyCollection<IWebElement> hmiStatusSelects =>
            _driver.FindElements(By.XPath("//select[starts-with(@id,'hmi-status-')]"));
        private IReadOnlyCollection<IWebElement> validityPeriodInputs =>
            _driver.FindElements(By.XPath("//input[starts-with(@id,'validity-period-')]"));
        private IWebElement btnSaveAndReturnToWorkOrder => _driver.WaitForElement(By.Id("button-save-and-continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public RecordHmiChecksPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Record HMI checks");
        }

        public bool VerifyCommodityHmiStatus(string expectedStatus)
        {
            var selects = hmiStatusSelects;

            if (selects.Count == 0)
                return false;

            return selects.All(select =>
            {
                var selectedOption = new SelectElement(select).SelectedOption;
                return selectedOption.Text.Trim().Equals(expectedStatus, StringComparison.OrdinalIgnoreCase);
            });
        }

        public void SetAllCommoditiesStatus(string status)
        {
            foreach (var select in hmiStatusSelects)
            {
                new SelectElement(select).SelectByText(status);
            }
        }

        public void SetValidityPeriod(int days)
        {
            foreach (var input in validityPeriodInputs)
            {
                input.Clear();
                input.SendKeys(days.ToString());
            }
        }

        public void ClickSaveAndReturnToWorkOrder()
        {
            btnSaveAndReturnToWorkOrder.Click();
        }
    }
}