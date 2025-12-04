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
        private IWebElement btnSelect => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//button[normalize-space()='Select']"));
        private IWebElement selectedTransporterName => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[1]"));
        private IWebElement selectedTransporterAddress => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[2]"));
        private IWebElement selectedTransporterCountry => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[3]"));
        private IWebElement selectedTransporterApprovalNumber => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[5]"));
        private IWebElement selectedTransporterType => _driver.FindElement(By.XPath("//table[@id='Table-SearchResults']//tbody//tr[1]//td[6]"));
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
            Console.WriteLine($"[SEARCH PAGE] Transporter Name: '{name}'");
            return name;
        }

        public string GetSelectedTransporterAddress()
        {
            var address = selectedTransporterAddress.Text.Trim();
            Console.WriteLine($"[SEARCH PAGE] Transporter Address: '{address}'");
            return address;
        }

        public string GetSelectedTransporterCountry()
        {
            var country = selectedTransporterCountry.Text.Trim();
            Console.WriteLine($"[SEARCH PAGE] Transporter Country: '{country}'");
            return country;
        }

        public string GetSelectedTransporterApprovalNumber()
        {
            var approvalNumber = selectedTransporterApprovalNumber.Text.Trim();
            Console.WriteLine($"[SEARCH PAGE] Transporter Approval Number: '{approvalNumber}'");
            return approvalNumber;
        }

        public string GetSelectedTransporterType()
        {
            var type = selectedTransporterType.Text.Trim();
            Console.WriteLine($"[SEARCH PAGE] Transporter Type: '{type}'");
            return type;
        }
    }
}