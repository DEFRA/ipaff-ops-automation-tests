using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class HMIExportCommodityRulesPage : IHMIExportCommodityRulesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='HMI (Export) Commodity Rules']"), true);
        private IWebElement commodityDropdown => _driver.WaitForElement(By.Id("product-selection"));
        private IWebElement varietyDropdown => _driver.WaitForElement(By.Id("variety-selection"));
        private IWebElement rateInput => _driver.WaitForElement(By.Id("rate"));
        private IWebElement permanentCheckbox => _driver.WaitForElementExists(By.Id("permanentRule"));
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Confirm and send']"));
        #endregion

        public HMIExportCommodityRulesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("HMI (Export) Commodity Rules");

        public void SelectCommodity(string commodity)
        {
            var select = new SelectElement(commodityDropdown);
            select.SelectByText(commodity);
            Thread.Sleep(2000); // allow variety dropdown to populate via JavaScript
        }

        public void SelectVariety(string variety)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => varietyDropdown.Enabled);
            var select = new SelectElement(varietyDropdown);
            select.SelectByText(variety);
            Thread.Sleep(1000); // allow selection to register
        }

        public void SetInspectionRate(int rate)
        {
            rateInput.Clear();
            rateInput.SendKeys(rate.ToString());
        }

        public void EnsurePermanentIsChecked()
        {
            if (!permanentCheckbox.Selected)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", permanentCheckbox);
            }
        }

        public void ClickConfirmAndSendButton() => btnConfirmAndSend.Click();
    }
}