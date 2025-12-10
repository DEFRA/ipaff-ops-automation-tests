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
        private IWebElement btnSelect => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//button[normalize-space()='Select']"));
        private IWebElement importerName => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[1]"));
        private IWebElement importerAddress => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[2]"));
        private IWebElement importerCountry => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[3]"));
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

        public void ClickSelect(string importer) 
        { 
            if(importerName.Text.Trim().Equals(importer))
                btnSelect.Click();
        }

        public string GetSelectedImporter()
        {
            var importerDetails = importerName.Text.Trim() + "\n" 
                + importerAddress.Text.Trim() + "," + importerCountry.Text.Trim();
            return importerDetails;
        }

        public string GetSelectedImporterName()
        {
            return importerName.Text.Trim();
        }

        public string GetSelectedImporterAddress()
        {
            return importerAddress.Text.Trim();
        }

        public string GetSelectedImporterCountry()
        {
            return importerCountry.Text.Trim();
        }
    }
}