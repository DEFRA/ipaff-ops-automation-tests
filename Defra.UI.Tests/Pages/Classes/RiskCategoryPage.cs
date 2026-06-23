using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class RiskCategoryPage : IRiskCategoryPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);

        private IWebElement GetRiskCategoryLabel(string option) =>
            _driver.FindElement(By.XPath($"//label[contains(normalize-space(text()),'{option}')]"));

        private IWebElement GetRiskCategoryInput(string option) =>
            _driver.FindElement(By.XPath($"//label[contains(normalize-space(text()),'{option}')]/preceding-sibling::input[@name='risk-category']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public RiskCategoryPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Select the highest risk category for the commodities in this consignment");
        }

        public void ClickRiskCategory(string option)
        {
            IWebElement label = GetRiskCategoryLabel(option);
            IWebElement input = GetRiskCategoryInput(option);

            label.Click();

            if (!input.Selected)
                label.Click();
        }
    }
}