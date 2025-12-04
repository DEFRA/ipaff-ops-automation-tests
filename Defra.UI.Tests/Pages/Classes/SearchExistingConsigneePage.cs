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
        private IWebElement btnSelect => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//button[normalize-space()='Select']"));
        private IWebElement selectedConsigneeName => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[1]"));
        private IWebElement selectedConsigneeAddress => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[2]"));
        private IWebElement selectedConsigneeCountry => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[3]"));
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

        public string GetSelectedConsignee()
        {
            var consigneeName = selectedConsigneeName.Text.Trim();
            var consigneeAddress = selectedConsigneeAddress.Text.Trim();
            var consigneeCountry = selectedConsigneeCountry.Text.Trim();
            var consigneeDetails = consigneeName + "\n" + consigneeAddress + "," + consigneeCountry;
            return consigneeDetails;
        }

        public void ClickSelect() 
        { 
            btnSelect.Click(); 
        }

    }
}