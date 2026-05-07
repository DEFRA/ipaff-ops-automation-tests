using System;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SubmitCommodityRulesCsvPage : ISubmitCommodityRulesCsvPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Submit multiple commodity rules using a CSV file']"), true);
        private IWebElement firstRowStatus => _driver.FindElement(By.XPath("(//table//tbody/tr[1]/td[5]//span[contains(@class,'govuk-tag')])[1]"));
        private IWebElement firstRowActionLink => _driver.FindElement(By.XPath("(//table//tbody/tr[1]/td[6]/a)[1]"));

        public SubmitCommodityRulesCsvPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;

        /// <summary>
        /// Polls the status cell of the top row, refreshing if the expected status has not yet appeared.
        /// </summary>
        public bool WaitForFirstRecordStatus(string expectedStatus, int timeoutSeconds = 60)
        {
            var endTime = DateTime.UtcNow.AddSeconds(timeoutSeconds);
            while (DateTime.UtcNow < endTime)
            {
                try
                {
                    if (string.Equals(firstRowStatus.Text.Trim(), expectedStatus, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                catch (StaleElementReferenceException) { /* refreshed page; retry */ }

                _driver.Navigate().Refresh();
                _driver.Wait(2);
            }
            return string.Equals(firstRowStatus.Text.Trim(), expectedStatus, StringComparison.OrdinalIgnoreCase);
        }

        public void ClickFirstRecordActionLink() => firstRowActionLink.Click();
    }
}