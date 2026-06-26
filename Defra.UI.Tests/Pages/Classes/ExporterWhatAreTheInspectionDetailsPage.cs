using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhatAreTheInspectionDetailsPage : IExporterWhatAreTheInspectionDetailsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What are the inspection details?']"), true);
        private IWebElement lnkSelectFirstAddress => _driver.WaitForElement(By.XPath("//table[@id='addresses-table']//a[contains(normalize-space(),'Select')][1]"), true);
        #endregion

        public ExporterWhatAreTheInspectionDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("What are the inspection details?");

        public void SelectAnInspectionAddressAndContinue()
        {
            lnkSelectFirstAddress.Click();
        }
    }
}