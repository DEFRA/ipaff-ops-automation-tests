using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CountyParishHoldingPage : ICountyParishHoldingPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
        private IWebElement txtCPHNumber => _driver.WaitForElement(By.Id("cph-number"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CountyParishHoldingPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Addresses")
                && primaryTitle.Text.Contains("Add the County Parish Holding number (CPH)");
        }

        public void EnterCPHNumber(string cphNumber)
        {
            txtCPHNumber.Clear();
            txtCPHNumber.SendKeys(cphNumber);
        }

        public string GetCPHNumber => txtCPHNumber.GetAttribute("value")?.Trim() ?? string.Empty;

    }
}