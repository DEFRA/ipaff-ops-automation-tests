using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterHowMuchStockAreYouExportingPage : IExporterHowMuchStockAreYouExportingPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='How much stock are you exporting?']"), true);
        private IWebElement lblStockSummary => _driver.WaitForElement(By.Id("quantity-hint"), true);
        private IWebElement txtQuantity => _driver.WaitForElement(By.Id("quantityOrWeightPerPackage"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterHowMuchStockAreYouExportingPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("How much stock are you exporting?");

        public string GetStockSummaryText() => System.Text.RegularExpressions.Regex.Replace(lblStockSummary.Text, @"\s+", " ").Trim();

        public void EnterQuantity(string quantity)
        {
            txtQuantity.Clear();
            txtQuantity.SendKeys(quantity);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}
