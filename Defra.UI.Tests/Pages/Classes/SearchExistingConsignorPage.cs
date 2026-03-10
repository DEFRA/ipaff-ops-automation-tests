using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SearchExistingConsignorPage : ISearchExistingConsignorPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects

        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement GetSelectButtonForConsignor(string consignorName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{consignorName}']/following-sibling::td//button[@name='add-id']"));
        private IWebElement GetConsignorNameElement(string consignorName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{consignorName}']"));
        private IWebElement GetConsignorAddressElement(string consignorName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{consignorName}']/following-sibling::td[contains(@class,'economic-operator-address')]"));
        private IWebElement GetConsignorCountryElement(string consignorName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{consignorName}']/following-sibling::td[2]"));
        private IWebElement lnkCreateConginor => _driver.WaitForElement(By.Id("add-economic-operator"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SearchExistingConsignorPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Traders")
                    && primaryTitle.Text.Contains("Search for an existing consignor or exporter");
        }

        public string GetSelectedConsignor(string consignorName)
        {
            var name = GetConsignorNameElement(consignorName).Text.Trim();
            var address = GetConsignorAddressElement(consignorName).Text.Trim();
            var country = GetConsignorCountryElement(consignorName).Text.Trim();
            var consignorDetails = $"{name}\n{address},{country}";
            return consignorDetails;
        }

        public void ClickSelect(string consignorName)
        {
            GetSelectButtonForConsignor(consignorName).Click();
        }

        public string GetSelectedConsignorName(string consignorName) => GetConsignorNameElement(consignorName).Text.Trim();
        public string GetSelectedConsignorAddress(string consignorName) => GetConsignorAddressElement(consignorName).Text.Trim();
        public string GetSelectedConsignorCountry(string consignorName) => GetConsignorCountryElement(consignorName).Text.Trim();

        public void ClickCreateANewConsignorLink()
        {
            lnkCreateConginor.Click();
        }
    }
}