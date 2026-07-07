using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DefaultRulesForCHEDPPPage : IDefaultRulesForCHEDPPPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Default rules']"), true);
        private IWebElement hmiImportRate => _driver.WaitForElement(By.Id("defaultrules_0__rate"));
        private IWebElement gmsExportRate => _driver.WaitForElement(By.Id("defaultrules_1__rate"));
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Confirm and send']"));
        #endregion

        public DefaultRulesForCHEDPPPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Default rules");

        public int GetHmiImportRate()
        {
            var value = hmiImportRate.GetAttribute("value")?.Trim();
            return int.TryParse(value, out var result) ? result : 0;
        }

        public void EnsureHmiImportRateIs(int targetRate)
        {
            var currentValue = hmiImportRate.GetAttribute("value")?.Trim() ?? string.Empty;
            var targetValue = targetRate.ToString();

            // Set HMI (Import) rate to the target value only if it is not already that value
            if (currentValue != targetValue)
            {
                hmiImportRate.Clear();
                hmiImportRate.SendKeys(targetValue);
                hmiImportRate.SendKeys(Keys.Tab);
            }
        }

        public void SetHmiImportRate(int rate)
        {
            hmiImportRate.Clear();
            hmiImportRate.SendKeys(rate.ToString());
            hmiImportRate.SendKeys(Keys.Tab);
        }

        public int GetGmsExportRate()
        {
            var value = gmsExportRate.GetAttribute("value")?.Trim();
            return int.TryParse(value, out var result) ? result : 0;
        }

        public void EnsureGmsExportRateIs(int targetRate)
        {
            var currentValue = gmsExportRate.GetAttribute("value")?.Trim() ?? string.Empty;
            var targetValue = targetRate.ToString();

            // Set GMS (Export) rate to the target value only if it is not already that value
            if (currentValue != targetValue)
            {
                gmsExportRate.Clear();
                gmsExportRate.SendKeys(targetValue);
                gmsExportRate.SendKeys(Keys.Tab);
            }
        }

        public void SetGmsExportRate(int rate)
        {
            gmsExportRate.Clear();
            gmsExportRate.SendKeys(rate.ToString());
            gmsExportRate.SendKeys(Keys.Tab);
        }

        public void ClickConfirmAndSendButton() => btnConfirmAndSend.Click();
    }
}