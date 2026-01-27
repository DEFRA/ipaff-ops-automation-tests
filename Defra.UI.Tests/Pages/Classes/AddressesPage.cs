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
        private IWebElement lnkAddImporter => _driver.WaitForElement(By.LinkText("Add an importer"));
        private IWebElement lnksameAsConsignee => _driver.WaitForElement(By.Id("populate-importer"));
        private IWebElement lnksameAsConsigneePlaceOfDestination => _driver.FindElement(By.Id("populate-place-of-destination"));
        private IWebElement lnkAddDestination => _driver.WaitForElement(By.LinkText("Add a place of destination"));
        private List<IWebElement> consignorRowsList => _driver.FindElements(By.XPath("//table[@id='traders-table-consignor']/tbody/tr")).ToList();
        private List<IWebElement> consigneeRowsList => _driver.FindElements(By.XPath("//table[@id='traders-table-consignee']/tbody/tr")).ToList();
        private List<IWebElement> importerRowsList => _driver.FindElements(By.XPath("//table[@id='traders-table-importer']/tbody/tr")).ToList();
        private List<IWebElement> destinationRowsList => _driver.FindElements(By.XPath("//table[@id='traders-table-place-of-destination']/tbody/tr")).ToList();
        private IWebElement selectedConsignorName => _driver.WaitForElement(By.XPath("//*[@id='traders-table-consignor']//td[1]"));
        private IWebElement selectedConsigneeName => _driver.WaitForElement(By.XPath("//*[@id='traders-table-consignee']//td[1]"));
        private IWebElement selectedDestination => _driver.WaitForElement(By.XPath("//*[@id='traders-table-place-of-destination']//td[1]"));
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
        private IWebElement lnkChange(string section) => _driver.FindElement(By.XPath($"(//h2[normalize-space(text())='{section}']/following::a)[1]"));
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

        public bool VerifySelectedConsignor(string consignor)
        {
            return selectedConsignorName.Text.Trim().Equals(consignor);
        }

        public string GetSelectedConsignor()
        {
            var consignorName = selectedConsignorName.Text.Trim();
            var consignorAddress = verifyConsignorAddress.Text.Trim();
            var consignorCountry = verifyConsignorCountry.Text.Trim();
            var consignorDetails = consignorName + "\n" + consignorAddress + "," + consignorCountry;
            return consignorDetails;
        }

        public string GetSelectedConsignee()
        {
            var consigneeName = selectedConsignorName.Text.Trim();
            var consigneeAddress = verifyConsignorAddress.Text.Trim();
            var consigneeCountry = verifyConsignorCountry.Text.Trim();
            var consigneeDetails = consigneeName + "\n" + consigneeAddress + "," + consigneeCountry;
            return consigneeDetails;
        }

        public void ClickAddConsignee() 
        { 
            lnkAddConsignee.Click(); 
        }

        public bool VerifySelectedConsignee(string consigneeName)
        {
            return selectedConsigneeName.Text.Trim().Equals(consigneeName);
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

        public bool VerifySelectedDestination(string destination)
        {
            return selectedDestination.Text.Trim().Equals(destination);
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

        public void ClickAddImporter()
        {
            lnkAddImporter.Click();
        }

        public bool VerifySelectedImporter(string name, string address, string country)
        {
            return verifyConsigneeName.Text.Trim().Equals(name) &&
                   verifyConsigneeAddress.Text.Trim().Equals(address) &&
                   verifyConsigneeCountry.Text.Trim().Equals(country);
        }

        public void ClickPlaceOfDestinationSameAsConsignee()
        {
            lnksameAsConsigneePlaceOfDestination.Click();
            Thread.Sleep(1000);
        }

        public string GetSelectedPlaceOfDestination()
        {
            var destinationName = verifyDestinationName.Text.Trim();
            var destinationAddress = verifyDestinationAddress.Text.Trim();
            var destinationCountry = verifyDestinationCountry.Text.Trim();
            var destinationDetails = destinationName + "\n" + destinationAddress + "," + destinationCountry;
            return destinationDetails;
        }

        public void ClickChangeInAddressPage(string section)
        {
            lnkChange(section).Click();
        }

        public int GetConsignorRowsCount() => consignorRowsList?.Count ?? 0;
        public int GetConsigneeRowsCount() => consigneeRowsList?.Count ?? 0;
        public int GetImporterRowsCount() => importerRowsList?.Count ?? 0;
        public int GetDestinationRowsCount() => destinationRowsList?.Count ?? 0;
    }
}