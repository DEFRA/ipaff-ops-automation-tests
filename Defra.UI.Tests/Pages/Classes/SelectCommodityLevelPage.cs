using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SelectCommodityLevelPage : ISelectCommodityLevelPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IReadOnlyCollection<IWebElement> lstCommodityLabels => _driver.FindElements(By.XPath("//div[contains(@class,'govuk-radios__item')]/label"));
        private IWebElement RadioForLabel(IWebElement label) => _driver.FindElement(By.Id(label.GetAttribute("for")));
        private IWebElement btnSelect => _driver.FindElement(By.Id("select-button"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SelectCommodityLevelPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Select the commodity level");
        }

        public void SelectCommodityByDescription(string description)
        {
            var label = lstCommodityLabels.FirstOrDefault(l =>
                l.Text.Contains(description, StringComparison.OrdinalIgnoreCase));

            RadioForLabel(label!).Click();
        }

        public void ClickSelect()
        {
            btnSelect.Click();
        }

        #endregion
    }
}