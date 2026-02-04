using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using Faker;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ImporterPackerDeliveryAddressConsignorPage : IImporterPackerDeliveryAddressConsignorPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement txtImporterDetails => _driver.FindElement(By.Id("traders-table-importer"));
        private IWebElement lnkAddADeliveryAddr => _driver.FindElement(By.Id("add-place-of-destination"));
        private IWebElement verifyDeliveryAddrName => _driver.FindElement(By.XPath("//td[@headers='place-of-destination-name']"));
        private IWebElement verifyDeliveryAddress => _driver.FindElement(By.XPath("//td[@headers='place-of-destination-address']"));
        private IWebElement verifyDeliveryCountry => _driver.FindElement(By.XPath("//td[@headers='place-of-destination-country']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ImporterPackerDeliveryAddressConsignorPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Traders")
                && primaryTitle.Text.Contains("Importer, Packer, Delivery address and Consignor");
        }

        public bool VerifyImporterName(string importerName)
        {
           return txtImporterDetails.Text.Contains(importerName);
        }

        public void AddADeliveryAddress()
        {
            lnkAddADeliveryAddr.Click();
        }

        public bool VerifySelectedDeliveryAddress(string deliveryAddressName, string deliveryAddress, string deliveryCountry)
        {
            return verifyDeliveryAddrName.Text.Trim().Equals(deliveryAddressName) 
                && verifyDeliveryAddress.Text.Trim().Equals(deliveryAddress) 
                   && verifyDeliveryCountry.Text.Trim().Equals(deliveryCountry);
        }
    }
}