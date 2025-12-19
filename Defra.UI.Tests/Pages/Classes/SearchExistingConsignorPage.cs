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
        private IWebElement btnSelect => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//button[normalize-space()='Select']"));
        private IWebElement selectedConsignorName => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[1]"));
        private IWebElement selectedConsignorAddress => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[2]"));
        private IWebElement selectedConsignorCountry => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[3]"));
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

        public string GetSelectedConsignor()
        {
            var consignorName = selectedConsignorName.Text.Trim();
            var consignorAddress = selectedConsignorAddress.Text.Trim();
            var consignorCountry = selectedConsignorCountry.Text.Trim();
            var consignorDetails = consignorName + "\n" + consignorAddress + "," + consignorCountry;
            return consignorDetails;
        }

        public void ClickSelect(string consignor) 
        {
            if (selectedConsignorName.Text.Trim().Equals(consignor))
                btnSelect.Click();
        }

        public string GetSelectedConsignorName() => selectedConsignorName.Text.Trim();
        public string GetSelectedConsignorAddress() => selectedConsignorAddress.Text.Trim();
        public string GetSelectedConsignorCountry() => selectedConsignorCountry.Text.Trim();

    }
}