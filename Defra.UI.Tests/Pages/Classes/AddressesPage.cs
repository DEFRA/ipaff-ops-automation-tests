using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Faker;

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
        private IWebElement selectedConsignor => _driver.WaitForElement(By.XPath("//*[@id='traders-table-consignor']//td[1]"));
        private IWebElement selectedConsignee => _driver.WaitForElement(By.XPath("//*[@id='traders-table-consignee']//td[1]"));
        private IWebElement selectedDestination => _driver.WaitForElement(By.XPath("//*[@id='traders-table-place-of-destination']//td[1]"));
        private IWebElement lnksameAsConsignee => _driver.WaitForElement(By.Id("populate-importer"));
        private IWebElement lnkAddDestination => _driver.WaitForElement(By.LinkText("Add a place of destination"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AddressesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Traders")
                && primaryTitle.Text.Contains("Addresses");
        }

        public void ClickAddConsignor() 
        { 
            lnkAddConsignor.Click(); 
        }

        public bool VerifySelectedConsignor()
        {
            return selectedConsignor.Text.Trim().Equals("ABC");
        }

        public void ClickAddConsignee() 
        { 
            lnkAddConsignee.Click(); 
        }

        public bool VerifySelectedConsignee()
        {
            return selectedConsignee.Text.Trim().Equals("XYZ");
        }

        public void ClickImporterSameAsConsignee()
        {
            lnksameAsConsignee.Click();
        }

        public void ClickAddDestination() 
        { 
            lnkAddDestination.Click(); 
        }

        public bool VerifySelectedDestination()
        {
            return selectedDestination.Text.Trim().Equals("XYZ");
        }
    }
}