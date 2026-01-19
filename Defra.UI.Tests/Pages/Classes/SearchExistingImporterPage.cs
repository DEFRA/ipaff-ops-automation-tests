using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes

{
    public class SearchExistingImporterPage : ISearchExistingImporterPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;

        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement GetSelectButtonForImporter(string importerName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{importerName}']/following-sibling::td//button[@name='add-id']"));
        private IWebElement GetImporterNameElement(string importerName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{importerName}']"));
        private IWebElement GetImporterAddressElement(string importerName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{importerName}']/following-sibling::td[contains(@class,'economic-operator-address')]"));
        private IWebElement GetImporterCountryElement(string importerName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{importerName}']/following-sibling::td[2]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SearchExistingImporterPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Traders")
                && primaryTitle.Text.Contains("Search for an existing importer");
        }

        public void ClickSelect(string importerName)
        {
            GetSelectButtonForImporter(importerName).Click();
        }

        public string GetSelectedImporter(string importerName)
        {
            var name = GetImporterNameElement(importerName).Text.Trim();
            var address = GetImporterAddressElement(importerName).Text.Trim();
            var country = GetImporterCountryElement(importerName).Text.Trim();
            var importerDetails = name + "\n" + address + "," + country;
            return importerDetails;
        }

        public string GetSelectedImporterName(string importerName) => GetImporterNameElement(importerName).Text.Trim();
        public string GetSelectedImporterAddress(string importerName) => GetImporterAddressElement(importerName).Text.Trim();
        public string GetSelectedImporterCountry(string importerName) => GetImporterCountryElement(importerName).Text.Trim();
    }
}
