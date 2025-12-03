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
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"));
        private IWebElement rdoMediumRisk => _driver.FindElement(By.XPath("//*[@id='risk-category']/following-sibling::label"));
        private IWebElement rdoLowRisk => _driver.FindElement(By.XPath("//*[@id='risk-category-2']/following-sibling::label")); 
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
            if (rdoMediumRisk.Text.Trim().Contains(option))
                rdoMediumRisk.Click();
            else if (rdoLowRisk.Text.Trim().Contains(option))
                rdoLowRisk.Click();
        }
    }
}