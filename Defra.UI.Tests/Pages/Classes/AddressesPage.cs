using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddressesPage : IAddressesPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement lnkAddConsignor => _driver.WaitForElement(By.LinkText("Add a consignor or exporter"));
        private IWebElement lnkAddConsignee => _driver.WaitForElement(By.LinkText("Add a consignee"));    
        private IWebElement lnksameAsConsignee => _driver.WaitForElement(By.Id("populate-importer"));
        private IWebElement lnkAddDestination => _driver.WaitForElement(By.LinkText("Add a place of destination"));
        private IWebElement selectedImporterName => _driver.WaitForElement(By.XPath("//*[@id='traders-table-importer']//td[1]"));
        private IWebElement selectedImporterAddress => _driver.WaitForElement(By.XPath("//*[@id='traders-table-importer']//td[2]"));
        private IWebElement selectedImporterCountry => _driver.WaitForElement(By.XPath("//*[@id='traders-table-importer']//td[3]"));
        private IWebElement verifyConsignorName => _driver.FindElement(By.XPath("//td[@headers='consignor-name']"));
        private IWebElement verifyConsignorAddress => _driver.FindElement(By.XPath("//td[@headers='consignor-address']"));
        private IWebElement verifyConsignorCountry => _driver.FindElement(By.XPath("//td[@headers='consignor-country']"));
        private IWebElement verifyConsigneeName => _driver.FindElement(By.XPath("//td[@headers='consignee-name']"));
        private IWebElement verifyConsigneeAddress => _driver.FindElement(By.XPath("//td[@headers='consignee-address']"));
        private IWebElement verifyConsigneeCountry => _driver.FindElement(By.XPath("//td[@headers='consignee-country']"));
        private IWebElement verifyDestinationName => _driver.FindElement(By.XPath("//td[@headers='place-of-destination-name']"));
        private IWebElement verifyDestinationAddress => _driver.FindElement(By.XPath("//td[@headers='place-of-destination-address']"));
        private IWebElement verifyDestinationCountry => _driver.FindElement(By.XPath("//td[@headers='place-of-destination-country']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AddressesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return (secondaryTitle.Text.Contains("Traders") && primaryTitle.Text.Contains("Addresses"))
                || IsConsignorExporterConsigneeImporterandPlaceOfDestinationPageLoaded();
        }

        public bool IsConsignorExporterConsigneeImporterandPlaceOfDestinationPageLoaded()
        {
            return secondaryTitle.Text.Contains("Addresses")
                && primaryTitle.Text.Contains("Consignor or Exporter, Consignee, Importer and Place of Destination");
        }

        public void ClickAddConsignor() 
        { 
            lnkAddConsignor.Click(); 
        }        

        public void ClickAddConsignee() 
        { 
            lnkAddConsignee.Click(); 
        }       

        public void ClickImporterSameAsConsignee()
        {
            lnksameAsConsignee.Click();
        }
        public string GetSelectedImporter()
        {
            var importerName = selectedImporterName.Text.Trim();
            var importerAddress = selectedImporterAddress.Text.Trim();
            var importerCountry = selectedImporterCountry.Text.Trim();
            var importerDetails = importerName + "\n" + importerAddress + "," + importerCountry;
            return importerDetails;
        }

        public void ClickAddDestination() 
        { 
            lnkAddDestination.Click(); 
        }        

        public bool VerifySelectedConsignor(string name, string address, string country)
        {
            return verifyConsignorName.Text.Trim().Equals(name) &&
                   verifyConsignorAddress.Text.Trim().Equals(address) &&
                   verifyConsignorCountry.Text.Trim().Equals(country);
        }

        public bool VerifySelectedConsignee(string name, string address, string country)
        {
            return verifyConsigneeName.Text.Trim().Equals(name) &&
                   verifyConsigneeAddress.Text.Trim().Equals(address) &&
                   verifyConsigneeCountry.Text.Trim().Equals(country);
        }

        public bool VerifySelectedDestination(string name, string address, string country)
        {
            return verifyDestinationName.Text.Trim().Equals(name) &&
                   verifyDestinationAddress.Text.Trim().Equals(address) &&
                   verifyDestinationCountry.Text.Trim().Equals(country);
        }
    }
}