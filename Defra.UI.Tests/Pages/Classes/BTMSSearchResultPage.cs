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
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@class='govuk-heading-l']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//span[@class='govuk-caption-l']"), true);
        private IWebElement txtItemNumber => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[1]"));
        private IWebElement txtCommodityCode => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[2]"));
        private IWebElement txtComodityDesc => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[3]"));
        private IWebElement txtQuantity => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[4]"));
        private IWebElement txtAuthority => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[5]"));
        private IWebElement txtDecision => _driver.WaitForElement(By.XPath("//*[@class='govuk-details__text']//td[6]"));
       
        private List<IWebElement> commodityCodeList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[2]")).ToList();
        private List<IWebElement> descriptionList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[3]")).ToList();
        private List<IWebElement> quantityList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[4]")).ToList();
        private List<IWebElement> authorityList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[5]")).ToList();
        private List<IWebElement> decisionList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[6]")).ToList();

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public BTMSSearchResultPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string CHEDPREFNum)
        {
            return primaryTitle.Text.Trim().Contains(CHEDPREFNum)
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

        public string GetCommodityCode(string commodityNum)
        {
            return GetCommodityDetails(commodityCodeList, commodityNum);
        }

        public string GetCommodityDesc(string commodityNum)
        {
            return GetCommodityDetails(descriptionList, commodityNum);
        }

        public string GetCommodityQuantity(string commodityNum)
        {
            return GetCommodityDetails(quantityList, commodityNum);
        }

        public string GetCommodityAuthority(string commodityNum)
        {
            return GetCommodityDetails(authorityList, commodityNum);
        }

        public string GetCommodityDecision(string commodityNum)
        {
            return GetCommodityDetails(decisionList, commodityNum);
        }

        private string GetCommodityDetails(List<IWebElement> commDetailList, string commodityNum)
        {
            string commDetailOnPage = String.Empty;
            try
            {
                commDetailOnPage = commDetailList[Int32.Parse(commodityNum)-1].Text.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{commDetailOnPage} failed: {ex}");
            }
            return commDetailOnPage;
        }
    }
}