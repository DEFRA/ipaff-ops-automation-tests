using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ConfirmationOfCommodityRateChangePage : IConfirmationOfCommodityRateChangePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Confirmation of commodity rate change']"), true);
        private IReadOnlyCollection<IWebElement> rateChangePanes => _driver.FindElements(By.XPath("//div[contains(concat(' ',normalize-space(@class),' '),' app-pane ')]"));
        private IReadOnlyCollection<IWebElement> summaryListRows => _driver.FindElements(By.XPath("//dl[@id='commodity-information']//div[contains(@class,'govuk-summary-list__row')]"));
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(By.XPath("//button[normalize-space()='Confirm and send']"));
        private By rateChangePaneKeyBy => By.XPath(".//div[contains(@class,'app-pane-header')]");
        private By rateChangePaneValueBy => By.XPath(".//div[contains(@class,'app-pane-body')]//span[contains(@class,'govuk-body-l')]");
        #endregion

        public ConfirmationOfCommodityRateChangePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Confirmation of commodity rate change");

        public IDictionary<string, string> GetConfirmationDetails()
        {
            var details = new Dictionary<string, string>();

            // Extract "From" and "To" rate change pane values
            foreach (var pane in rateChangePanes)
            {
                var key = pane.FindElement(rateChangePaneKeyBy).Text.Trim();
                var value = pane.FindElement(rateChangePaneValueBy).Text.Trim();
                details[key] = value;
            }

            // Extract commodity summary list rows (Commodity Group, Commodity, Variety, etc.)
            foreach (var row in summaryListRows)
            {
                var key = row.FindElement(By.XPath(".//dt")).Text.Trim();
                var value = row.FindElement(By.XPath(".//dd")).Text.Trim();
                details[key] = value;
            }

            return details;
        }

        public void ClickConfirmAndSendButton() => btnConfirmAndSend.Click();
    }
}