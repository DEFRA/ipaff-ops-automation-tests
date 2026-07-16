using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDPImportCommodityRulesPage : ICHEDPImportCommodityRulesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CHED-P (Import) Commodity Rules']"), true);
        private IWebElement searchCommodityInput => _driver.WaitForElement(By.Id("search-commodity-input"));
        private IWebElement searchCommodityButton => _driver.WaitForElement(By.Id("search-commodity"));
        private IWebElement countriesDropdown => _driver.WaitForElement(By.Id("country"));
        private IWebElement rateInput => _driver.WaitForElement(By.Id("rate"));
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Confirm and send']"));
        private IWebElement commodityTreeItem(string commodityName) =>
            _driver.WaitForElement(By.XPath($"//div[@id='commodityTreeContainer']//li[contains(@class,'species-selection-item')]//span[@class='name' and contains(text(),'{commodityName}')]"));
        private IWebElement speciesSelectionItem(string speciesName) =>
            _driver.WaitForElement(By.XPath($"//div[@id='commodityTreeContainer']//div[contains(@class,'species-selection')]//div[@class='list-card-right' and normalize-space()='{speciesName}']"));
        private IWebElement checkboxByLabel(string label) =>
            _driver.WaitForElementExists(By.XPath($"//label[normalize-space()='{label}']/preceding-sibling::input[@type='checkbox'] | //input[@id=//label[normalize-space()='{label}']/@for]"));
        #endregion

        public CHEDPImportCommodityRulesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("CHED-P (Import) Commodity Rules");

        public void SearchCommodity(string commodityCode, string commodityName)
        {
            searchCommodityInput.Clear();
            searchCommodityInput.SendKeys(commodityCode);
            searchCommodityButton.Click();
            Thread.Sleep(2000); // allow commodity tree to load

            // Stage 1: Click on the initial tree item
            commodityTreeItem(commodityName).Click();
            Thread.Sleep(2000); // allow species list to load

            // Stage 2: Click on the specific species from the loaded list
            speciesSelectionItem(commodityName).Click();
            Thread.Sleep(1000); // allow selection to register
        }

        public void SelectCountry(string country)
        {
            var select = new SelectElement(countriesDropdown);
            select.SelectByText(country);
        }

        public void SetInspectionRate(int rate)
        {
            rateInput.Clear();
            rateInput.SendKeys(rate.ToString());
        }

        public void EnsureCheckboxIsChecked(string checkboxLabel)
        {
            var checkbox = checkboxByLabel(checkboxLabel);
            if (!checkbox.Selected)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", checkbox);
            }
        }

        public void ClickConfirmAndSendButton() => btnConfirmAndSend.Click();
    }
}