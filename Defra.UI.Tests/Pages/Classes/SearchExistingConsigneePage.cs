using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class SearchExistingConsigneePage : ISearchExistingConsigneePage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement GetSelectButtonForConsignee(string consigneeName) =>
            _driver.FindElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{consigneeName}']/following-sibling::td//button[@name='add-id']"));
        private IWebElement GetConsigneeNameElement(string consigneeName) =>
            _driver.FindElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{consigneeName}']"));
        private IWebElement GetConsigneeAddressElement(string consigneeName) =>
            _driver.FindElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{consigneeName}']/following-sibling::td[contains(@class,'economic-operator-address')]"));
        private IWebElement GetConsigneeCountryElement(string consigneeName) =>
            _driver.FindElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{consigneeName}']/following-sibling::td[2]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SearchExistingConsigneePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Traders")
                && primaryTitle.Text.Contains("Search for an existing consignee");
        }

        public string GetSelectedConsignee(string consigneeName)
        {
            var name = GetConsigneeNameElement(consigneeName).Text.Trim();
            var address = GetConsigneeAddressElement(consigneeName).Text.Trim();
            var country = GetConsigneeCountryElement(consigneeName).Text.Trim();
            var consigneeDetails = name + "\n" + address + "," + country;
            return consigneeDetails;
        }

        public void ClickSelect(string consigneeName)
        {
            GetSelectButtonForConsignee(consigneeName).Click();
        }

        public string GetSelectedConsigneeName(string consigneeName) => GetConsigneeNameElement(consigneeName).Text.Trim();
        public string GetSelectedConsigneeAddress(string consigneeName) => GetConsigneeAddressElement(consigneeName).Text.Trim();
        public string GetSelectedConsigneeCountry(string consigneeName) => GetConsigneeCountryElement(consigneeName).Text.Trim();

    }
}