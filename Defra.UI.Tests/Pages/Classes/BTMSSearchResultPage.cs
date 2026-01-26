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
        private List<IWebElement> searchResultRows => _driver.FindElements(By.XPath("//*[@class='govuk-table govuk-table--small-text-until-tablet btms-notification']//tbody/tr")).ToList();
        private List<IWebElement> commodityCodeList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[2]")).ToList();
        private List<IWebElement> descriptionList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[3]")).ToList();
        private List<IWebElement> quantityList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[4]")).ToList();
        private List<IWebElement> authorityList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[5]")).ToList();
        private List<IWebElement> decisionList => _driver.WaitForElements(By.XPath("//*[@class='govuk-details__text']//td[6]")).ToList();
        private IWebElement txtCHEDStatus => _driver.FindElement(By.XPath("//*[normalize-space()='CHED Status']/following-sibling::dd"));
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
            return searchResultRows.Any(row =>
            {
                var cells = row.FindElements(By.TagName("td")).ToList();
                return cells[1].Text.Trim().Equals(commodityCode)
                    && cells[2].Text.Trim().Equals(commodityDescription)
                    && cells[3].Text.Trim().Equals(commodityQuantity)
                    && cells[4].Text.Trim().Replace("\r\n", " ").Equals(authority)
                    && cells[5].Text.Trim().Replace("\r\n", " ").Equals(decision);
            });            
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

        public bool VerifyStatus(string status)
        {
            return txtCHEDStatus.Text.Trim().Equals(status);
        }

        public bool IsPageLoadedForReplacementCHED(string replacementCHEDPREFNum)
        {
            return primaryTitle.Text.Trim().Contains(replacementCHEDPREFNum)
                && secondaryTitle.Text.Trim().Contains("Showing result for");
        }
    }
}