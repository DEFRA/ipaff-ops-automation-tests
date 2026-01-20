using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SearchExistingDestinationPage : ISearchExistingDestinationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement GetSelectButtonForDestination(string destinationName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{destinationName}']/following-sibling::td//button[@name='add-id']"));
        private IWebElement GetDestinationNameElement(string destinationName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{destinationName}']"));
        private IWebElement GetDestinationAddressElement(string destinationName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{destinationName}']/following-sibling::td[contains(@class,'economic-operator-address')]"));
        private IWebElement GetDestinationCountryElement(string destinationName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{destinationName}']/following-sibling::td[2]"));
        private IWebElement btnSelect => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//button[normalize-space()='Select']"));
        private IWebElement selectedPOEName => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[1]"));
        private IWebElement selectedPOEAddress => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[2]"));
        private IWebElement selectedPOECountry => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[3]"));
        private IWebElement selectedDestinationName => _driver.FindElement(By.XPath("//td[@class='govuk-table__cell economic-operator-name']"));
        private IWebElement selectedDestinationAddress => _driver.FindElement(By.XPath("//td[@class='govuk-table__cell economic-operator-address']"));
        private IWebElement selectedDestinationCountry => _driver.FindElement(By.XPath("//td[@class='govuk-table__cell'][not(@headers)]"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SearchExistingDestinationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Traders")
                && primaryTitle.Text.Contains("Search for an existing place of destination");
        }

        public string GetSelectedPlaceOfDestination(string destinationName)
        {
            var name = GetDestinationNameElement(destinationName).Text.Trim();
            var address = GetDestinationAddressElement(destinationName).Text.Trim();
            var country = GetDestinationCountryElement(destinationName).Text.Trim();
            var placeOfDestinationDetails = name + "\n" + address + "," + country;
            return placeOfDestinationDetails;
        }

        public string GetSelectedDestinationName(string destinationName) => GetDestinationNameElement(destinationName).Text.Trim();
        public string GetSelectedDestinationAddress(string destinationName) => GetDestinationAddressElement(destinationName).Text.Trim();
        public string GetSelectedDestinationCountry(string destinationName) => GetDestinationCountryElement(destinationName).Text.Trim();

        public void ClickSelect(string destinationName)
        {
            GetSelectButtonForDestination(destinationName).Click();
        }
    }
}