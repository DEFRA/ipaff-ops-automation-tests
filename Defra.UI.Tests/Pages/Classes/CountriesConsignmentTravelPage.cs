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
    public class CountriesConsignmentTravelPage : ICountriesConsignmentTravelPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
        private IWebElement ddlTransitedState => _driver.WaitForElement(By.Id("transited-state"));
        private IWebElement btnAddAnotherCountry => _driver.WaitForElement(By.Id("add-another-country"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CountriesConsignmentTravelPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Trim().Contains("Which countries will the consignment travel through?") &&
                   secondaryTitle.Text.Trim().Contains("Transport");
        }      

        public void SelectCountry(string country)
        {
            new SelectElement(ddlTransitedState).SelectByText(country);
        }

        public void ClickAddAnotherCountry()
        {
            btnAddAnotherCountry.Click();
        }
    }
}