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
    public class ApprovedEstablishmentPage : IApprovedEstablishmentPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
        private IWebElement lnkSearchForApproved => _driver.WaitForElement(By.Name("add-establishment"));
        private IWebElement countryDropdown => _driver.WaitForElement(By.Id("establishment-country-code"));
        private IWebElement lnkSelectEstablishment => _driver.WaitForElement(By.Id("select-establishment-1"));
        private IWebElement selectedEstablishment => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[1]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ApprovedEstablishmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Documents")
                && primaryTitle.Text.Contains("Approved establishment of origin (where required)");
        }
       
        public void ClickSearchForApproved() 
        { 
            lnkSearchForApproved.Click(); 
        }

        public bool VerifySelectedCountryOfOrigin(string country)
        {
            var select = new SelectElement(countryDropdown);
            string actual = select.SelectedOption.Text.Trim();
            return actual.Equals(country);
        }

        public void ClickSelectEstablishment() 
        { 
            lnkSelectEstablishment.Click(); 
        }

        public bool VerifySelectedEstablismentName()
        {
            return selectedEstablishment.Text.Trim().Equals("CHARRADE MARCEL ETS");
        }
    }
}