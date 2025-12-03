using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class BTMSSearchResultPage : IBTMSSearchResultPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects   
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@class='govuk-heading-l']"));
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//span[@class='govuk-caption-l']"));
        private IWebElement txtItemNumber => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[1]"));
        private IWebElement txtCommodityCode => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[2]"));
        private IWebElement txtComodityDesc => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[3]"));
        private IWebElement txtQuantity => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[4]"));
        private IWebElement txtAuthority => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[5]"));
        private IWebElement txtDecision => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[6]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public BTMSSearchResultPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Trim().Contains("CHEDP.GB.2025.1055883")
                && secondaryTitle.Text.Trim().Contains("Showing result for");
        }

        public bool ValidateBTMSSearchResult(string commodityCode, string commodityDescription, string commodityQuantity, string authority, string decision)
        {
            return txtCommodityCode.Text.Trim().Contains(commodityCode)
                && txtComodityDesc.Text.Trim().Contains(commodityDescription)
                && txtQuantity.Text.Trim().Contains(commodityQuantity)
                && txtAuthority.Text.Trim().Contains(authority)
                && txtDecision.Text.Trim().Contains(decision);
        }
    }
}