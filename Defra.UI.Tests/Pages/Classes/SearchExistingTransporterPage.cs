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

namespace Defra.UI.Tests.Pages.Classes
{
    public class SearchExistingTranspoterPage : ISearchExistingTranspoterPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
        private IWebElement txtSearchName => _driver.WaitForElement(By.Id("name"));
        private IWebElement btnSearch => _driver.WaitForElement(By.Id("search"));
        private IWebElement btnSelect => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//button[normalize-space()='Select']"));
        private IWebElement selectedTransporterName => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[1]"));
        private IWebElement selectedTransporterAddress => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[2]"));
        private IWebElement selectedTransporterCountry => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[3]"));
        private IWebElement selectedTransporterApprovalNumber => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[5]"));
        private IWebElement selectedTransporterType => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[6]"));
        private IWebElement GetSelectButtonForTransporter(string transporterName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{transporterName}']/following-sibling::td//button[@name='add-id']"));
        private IWebElement GetTransporterNameElement(string transporterName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{transporterName}']"));
        private IWebElement GetTransporterAddressElement(string transporterName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{transporterName}']/following-sibling::td[contains(@class,'economic-operator-address')]"));
        private IWebElement GetTransporterCountryElement(string transporterName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{transporterName}']/following-sibling::td[2]"));
        private IWebElement GetTransporterApprovalNumberElement(string transporterName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{transporterName}']/following-sibling::td[4]"));
        private IWebElement GetTransporterTypeElement(string transporterName) =>
            _driver.WaitForElement(By.XPath($"//td[contains(@class,'economic-operator-name') and normalize-space()='{transporterName}']/following-sibling::td[5]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SearchExistingTranspoterPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Transport")
                && primaryTitle.Text.Contains("Search for an existing transporter");
        }

        public void ClickSelect() 
        { 
            btnSelect.Click(); 
        }

        public string GetSelectedTransporterName()
        {
            var name = selectedTransporterName.Text.Trim();
            return name;
        }

        public string GetSelectedTransporterAddress()
        {
            var address = selectedTransporterAddress.Text.Trim();
            return address;
        }

        public string GetSelectedTransporterCountry()
        {
            var country = selectedTransporterCountry.Text.Trim();
            return country;
        }

        public string GetSelectedTransporterApprovalNumber()
        {
            var approvalNumber = selectedTransporterApprovalNumber.Text.Trim();
            return approvalNumber;
        }

        public string GetSelectedTransporterType()
        {
            var type = selectedTransporterType.Text.Trim();
            return type;
        }

        public void SearchForTransporter(string transporterName)
        {
            txtSearchName.Clear();
            txtSearchName.SendKeys(transporterName);
            btnSearch.Click();
        }

        public void ClickSelectForTransporter(string transporterName)
        {
            GetSelectButtonForTransporter(transporterName).Click();
        }

        public string GetSelectedTransporterName(string transporterName)
        {
            return GetTransporterNameElement(transporterName).Text.Trim();
        }

        public string GetSelectedTransporterAddress(string transporterName)
        {
            return GetTransporterAddressElement(transporterName).Text.Trim();
        }

        public string GetSelectedTransporterCountry(string transporterName)
        {
            return GetTransporterCountryElement(transporterName).Text.Trim();
        }

        public string GetSelectedTransporterApprovalNumber(string transporterName)
        {
            return GetTransporterApprovalNumberElement(transporterName).Text.Trim();
        }

        public string GetSelectedTransporterType(string transporterName)
        {
            return GetTransporterTypeElement(transporterName).Text.Trim();
        }
    }
}