using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ConfirmationOfRateChangePage : IConfirmationOfRateChangePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Confirmation of rate change']"), true);
        private IWebElement defaultRateChangepageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Confirmation of default rate change']"), true);

        private IReadOnlyCollection<IWebElement> rateChangePanes => _driver.FindElements(By.XPath("//div[contains(concat(' ',normalize-space(@class),' '),' app-pane ')]"));
        private IReadOnlyCollection<IWebElement> summaryListRows => _driver.FindElements(By.XPath("//dl[@id='commodity-information']//div[contains(@class,'govuk-summary-list__row')]"));
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(By.XPath("//button[normalize-space()='Confirm and send']"));
        private By rateChangePaneKeyBy => By.XPath(".//div[contains(@class,'app-pane-header')]");
        private By rateChangePaneValueBy => By.XPath(".//div[contains(@class,'app-pane-body')]//span[contains(@class,'govuk-body-l')]");

        private IReadOnlyCollection<IWebElement> hmiImportRateChangePanes => _driver.FindElements(By.XPath("//p[contains(text(),'HMI (Import)')]/following-sibling::div[1]//div[contains(concat(' ',normalize-space(@class),' '),' app-pane ')]"));
        private By hmiImportRateChangePaneKeyBy => By.XPath(".//div[contains(@class,'app-pane-header')]");
        private By hmiImportRateChangePaneValueBy => By.XPath(".//div[contains(@class,'app-pane-body')]");
        #endregion

        public ConfirmationOfRateChangePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Confirmation of rate change");
        public bool IsDefaultRateChangePageLoaded() => defaultRateChangepageTitle.Text.Trim().Equals("Confirmation of default rate change");

        public IDictionary<string, string> GetHmiImportRateConfirmationDetails()
        {
            var details = new Dictionary<string, string>();

            // Extract "From" and "To" rate change pane values
            foreach (var pane in hmiImportRateChangePanes)
            {
                var key = pane.FindElement(hmiImportRateChangePaneKeyBy).Text.Trim();
                var value = pane.FindElement(hmiImportRateChangePaneValueBy).Text.Trim();
                details[key] = value;
            }

            return details;
        }

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

            // Extract commodity summary list rows (Name, Class name, Description, etc.)
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