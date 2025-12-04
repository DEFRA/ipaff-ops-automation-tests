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
    public class SearchExistingDestinationPage : ISearchExistingDestinationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
        private IWebElement btnSelect => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//button[normalize-space()='Select']"));
        private IWebElement selectedDestinationName => _driver.FindElement(By.XPath("//td[@class='govuk-table__cell economic-operator-name']"));
        private IWebElement selectedDestinationAddress => _driver.FindElement(By.XPath("//td[@class='govuk-table__cell economic-operator-address']"));
        private IWebElement selectedDestinationCountry => _driver.FindElement(By.XPath("//td[@class='govuk-table__cell'][not(@headers)]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SearchExistingDestinationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Traders")
                && primaryTitle.Text.Contains("Search for an existing place of destination");
        }

        public void ClickSelect() 
        { 
            btnSelect.Click(); 
        }

        public string GetSelectedDestinationName() => selectedDestinationName.Text.Trim();
        public string GetSelectedDestinationAddress() => selectedDestinationAddress.Text.Trim();
        public string GetSelectedDestinationCountry() => selectedDestinationCountry.Text.Trim();
    }
}