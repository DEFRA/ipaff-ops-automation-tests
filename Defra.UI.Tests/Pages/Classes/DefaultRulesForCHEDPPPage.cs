using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Pages.Interfaces;
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

        public void EnsureHmiImportRateIsZero()
        {
            var currentValue = hmiImportRate.GetAttribute("value")?.Trim() ?? string.Empty;

            // Set Hmi Import Rate to "0" only if it is not already "0"
            if (currentValue != "0")
            {
                hmiImportRate.Clear();
                hmiImportRate.SendKeys("0");

                hmiImportRate.SendKeys(Keys.Tab);
            }           
        }

        public void SetHmiImportRate(int rate)
        {
            hmiImportRate.Clear();
            hmiImportRate.SendKeys(rate.ToString());
            hmiImportRate.SendKeys(Keys.Tab);
        }

        public void ClickConfirmAndSendButton() => btnConfirmAndSend.Click();
    }
}