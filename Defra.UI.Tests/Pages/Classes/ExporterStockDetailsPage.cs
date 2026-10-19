using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterStockDetailsPage : IExporterStockDetailsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("ssdd-summary-heading"), true);
        private IWebElement btnDrawDown => _driver.WaitForElement(By.Id("Button-DrawDown"));
        private IWebElement summaryValue(string key) =>
            _driver.WaitForElement(By.XPath($"//dt[contains(@class,'govuk-summary-list__key') and normalize-space()='{key}']/following-sibling::dd[contains(@class,'govuk-summary-list__value')][1]"), true);
        #endregion

        public ExporterStockDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().StartsWith("Stock details");

        public string GetStockReference() =>
            pageTitle.Text.Replace("Stock details", string.Empty).Trim();

        public string GetSummaryValue(string key) => summaryValue(key).Text.Trim();

        public void ClickDrawDownButton() => btnDrawDown.Click();
    }
}
