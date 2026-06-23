using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhatAreYouExportingSelectOneCommodityAtATimePage : IExporterWhatAreYouExportingSelectOneCommodityAtATimePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What are you exporting?']"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Continue'] | //input[@value='Continue']"));
        private IReadOnlyCollection<IWebElement> commodityRadioOptions(string commodity) => _driver.FindElements(By.XPath($"//label[contains(normalize-space(),'{commodity}')]"));
        private IWebElement txtCommoditySearch => _driver.WaitForElement(By.XPath("//input[@type='search' or contains(@class,'autocomplete__input')]"), true);
        private IWebElement commodityOption(string commodity) => _driver.WaitForElement(By.XPath($"//li[contains(.,'{commodity}')] | //div[contains(@class,'autocomplete__option') and contains(.,'{commodity}')]"), true);
        #endregion

        public ExporterWhatAreYouExportingSelectOneCommodityAtATimePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("What are you exporting?");

        public void SelectCommodity(string commodity)
        {
            if (commodityRadioOptions(commodity).Count > 0)
            {
                commodityRadioOptions(commodity).First().Click();
                return;
            }

            txtCommoditySearch.Clear();
            txtCommoditySearch.SendKeys(commodity);
            commodityOption(commodity).Click();
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}