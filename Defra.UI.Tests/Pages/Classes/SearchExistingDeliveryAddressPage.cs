using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SearchExistingDeliveryAddressPage : ISearchExistingDeliveryAddressPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement GetSelectButtonForDeliveryAddress(string deliveryAddress) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{deliveryAddress}']/following-sibling::td//button[@name='add-id']"));
        private IWebElement GetDeliveryNameElement(string deliveryAddress) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{deliveryAddress}']"));
        private IWebElement GetDeliveryAddressElement(string deliveryAddress) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{deliveryAddress}']/following-sibling::td[contains(@class,'economic-operator-address')]"));
        private IWebElement GetDeliveryCountryElement(string deliveryAddress) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{deliveryAddress}']/following-sibling::td[2]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SearchExistingDeliveryAddressPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Traders")
                && primaryTitle.Text.Contains("Search for an existing delivery address");
        }

        public string GetSelectedDeliveryAddressDetails(string deliveryAddress)
        {
            var name = GetDeliveryNameElement(deliveryAddress).Text.Trim();
            var address = GetDeliveryAddressElement(deliveryAddress).Text.Trim();
            var country = GetDeliveryCountryElement(deliveryAddress).Text.Trim();
            var deliveryAddressDetails = name + "\n" + address + "," + country;
            return deliveryAddressDetails;
        }

        public void ClickSelect(string deliveryAddress) 
        {
            GetSelectButtonForDeliveryAddress(deliveryAddress).Click();
        }

        public string GetSelectedDeliveryAddressName(string deliveryAddress) => GetDeliveryNameElement(deliveryAddress).Text.Trim();
        public string GetSelectedDeliveryAddress(string deliveryAddress) => GetDeliveryAddressElement(deliveryAddress).Text.Trim();
        public string GetSelectedDeliveryCountry(string deliveryAddress) => GetDeliveryCountryElement(deliveryAddress).Text.Trim();
    }
}