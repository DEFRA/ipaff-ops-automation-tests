using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class HMIImportCommodityRulesPage : IHMIImportCommodityRulesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='HMI (Import) - Commodity Rules']"), true);
        private IWebElement countryDropdown => _driver.WaitForElement(By.Id("country"));
        private IWebElement searchCommodityInput => _driver.WaitForElement(By.Id("search-commodity-input"));
        private IWebElement searchCommodityButton => _driver.WaitForElement(By.Id("search-commodity"));
        private IWebElement rateInput => _driver.WaitForElement(By.Id("rate"));
        private IWebElement permanentCheckbox => _driver.WaitForElementExists(By.Id("permanentRule"));
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Confirm and send']"));
        private IWebElement commodityTreeItem(string commodityName) =>
            _driver.WaitForElement(By.XPath($"//div[@id='commodityTreeContainer']//li[contains(@class,'species-selection-item')]//span[@class='name' and contains(text(),'{commodityName}')]"));
        #endregion

        public HMIImportCommodityRulesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("HMI (Import) - Commodity Rules");

        public void SelectCountry(string country)
        {
            var select = new SelectElement(countryDropdown);
            select.SelectByText(country);
        }

        public void SearchCommodity(string commodityCode, string commodityName)
        {
            searchCommodityInput.Clear();
            searchCommodityInput.SendKeys(commodityCode);
            searchCommodityButton.Click();
            Thread.Sleep(2000); // allow commodity tree to load
            commodityTreeItem(commodityName).Click();
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