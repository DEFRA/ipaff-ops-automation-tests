using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SelectTheCHEDDCommodityYouWantToAddARuleForPage : ISelectTheCHEDDCommodityYouWantToAddARuleForPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Select the CHED-D commodity you want to add a rule for']"), true);
        private IWebElement searchCommodityInput => _driver.WaitForElement(By.Id("SearchText-CommodityCode"));
        private IWebElement searchCommodityButton => _driver.WaitForElement(By.XPath("//button[@name='selector-search-button']"));
        private IWebElement selectCommodityLink(string commodityName) =>
            _driver.WaitForElement(By.XPath($"//div[@id='CommodityTreeRouteCommodityCode']//span[@class='commodity-description-tree-item'][.//a[normalize-space()='{commodityName}']]//a[@name='select-commodity-link']"));
        #endregion

        public SelectTheCHEDDCommodityYouWantToAddARuleForPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Select the CHED-D commodity you want to add a rule for");

        public void SearchCommodity(string commodityCode, string commodityName)
        {
            searchCommodityInput.Clear();
            searchCommodityInput.SendKeys(commodityCode);
            searchCommodityButton.Click();
            Thread.Sleep(2000);
            selectCommodityLink(commodityName).Click();
            Thread.Sleep(1000);
        }
    }
}