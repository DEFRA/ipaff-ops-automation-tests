using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CookiesPage : ICookiesPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement lnkImportOfProducts => _driver.FindElement(By.ClassName("govuk-service-navigation__link")); 
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CookiesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Cookies on the Imports of Products Animals Food and Feed Service (IPAFFS)");
        }

        public void ClickImportOfProduct()
        {
            lnkImportOfProducts.Click();
        }
    }
}