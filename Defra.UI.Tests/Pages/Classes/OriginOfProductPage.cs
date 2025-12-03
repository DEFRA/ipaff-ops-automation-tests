using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class OriginOfProductPage : IOriginOfProductPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"));
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"));
        private IWebElement drpdownCountry => _driver.WaitForElement(By.Id("origin-country"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public OriginOfProductPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("About the consignment")
                && primaryTitle.Text.Contains("Origin of the animal or product");
        }

        public void SelectCountryOfOrigin(string country)
        {
            var Select = new SelectElement(drpdownCountry);
            Select.SelectByText(country);
        }
    }
}