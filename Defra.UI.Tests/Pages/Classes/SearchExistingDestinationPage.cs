using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SearchExistingDestinationPage : ISearchExistingDestinationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement btnSelect => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//button[normalize-space()='Select']"));
        private IWebElement selectedPOEName => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[1]"));
        private IWebElement selectedPOEAddress => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[2]"));
        private IWebElement selectedPOECountry => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//td[3]"));
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

        public string GetSelectedPlaceOfDestination()
        {
            var placeOfDestinationName = selectedPOEName.Text.Trim();
            var placeOfDestinationAddress = selectedPOEAddress.Text.Trim();
            var placeOfDestinationCountry = selectedPOECountry.Text.Trim();
            var placeOfDestinationDetails = placeOfDestinationName + "\n" + placeOfDestinationAddress + "," + placeOfDestinationCountry;
            return placeOfDestinationDetails;
        }

        public void ClickSelect() 
        { 
            btnSelect.Click(); 
        }
    }
}