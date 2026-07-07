using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ConfirmationOfDefaultRateChangePage : IConfirmationOfDefaultRateChangePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Confirmation of default rate change']"), true);
        private IReadOnlyCollection<IWebElement> hmiImportRateChangePanes => _driver.FindElements(By.XPath("//p[contains(text(),'HMI (Import)')]/following-sibling::div[1]//div[contains(concat(' ',normalize-space(@class),' '),' app-pane ')]"));
        private IReadOnlyCollection<IWebElement> gmsExportRateChangePanes => _driver.FindElements(By.XPath("//p[contains(text(),'GMS (Export)')]/following-sibling::div[1]//div[contains(concat(' ',normalize-space(@class),' '),' app-pane ')]"));
        private By ratePaneKeyBy => By.XPath(".//div[contains(@class,'app-pane-header')]");
        private By ratePaneValueBy => By.XPath(".//div[contains(@class,'app-pane-body')]");
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(By.XPath("//button[normalize-space()='Confirm and send']"));
        #endregion

        public ConfirmationOfDefaultRateChangePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Confirmation of default rate change");

        public IDictionary<string, string> GetHmiImportRateConfirmationDetails()
            => ExtractPaneDetails(hmiImportRateChangePanes);

        public IDictionary<string, string> GetGmsExportRateConfirmationDetails()
            => ExtractPaneDetails(gmsExportRateChangePanes);

        public void ClickConfirmAndSendButton() => btnConfirmAndSend.Click();

        private IDictionary<string, string> ExtractPaneDetails(IReadOnlyCollection<IWebElement> panes)
        {
            var details = new Dictionary<string, string>();

            // Extract "From" and "To" rate change pane values
            foreach (var pane in panes)
            {
                var key = pane.FindElement(ratePaneKeyBy).Text.Trim();
                var value = pane.FindElement(ratePaneValueBy).Text.Trim();
                details[key] = value;
            }

            return details;
        }
    }
}