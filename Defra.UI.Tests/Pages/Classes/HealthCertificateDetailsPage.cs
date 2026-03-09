using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class HealthCertificateDetailsPage:IHealthCertificateDetailsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        Dictionary<string, string> summaryAndGoodsCache = [];

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IReadOnlyCollection<IWebElement> divSubTitles => _driver.FindElements(By.XPath("//label[@class='govuk-label govuk-label--m']"));
        private IWebElement legendSubTitle => _driver.FindElement(By.XPath("//legend[contains(@class,'govuk-fieldset__legend--m')]"));
        private IWebElement drpContryOfOrigin => _driver.FindElement(By.Id("country-of-origin"));
        private IWebElement txtCerfificateRefNumber => _driver.FindElement(By.Id("certificate-reference"));
        private IWebElement txtCertificateIssueDay => _driver.FindElement(By.Id("certificate-date-of-issue-day"));
        private IWebElement txtCertificateIssueMonth => _driver.FindElement(By.Id("certificate-date-of-issue-month"));
        private IWebElement txtCertificateIssueYear => _driver.FindElement(By.Id("certificate-date-of-issue-year"));
        private IWebElement txtConsignorConsigneeImporterName => _driver.FindElement(By.Id("party-name"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search"));
        private IWebElement healthCertificatePageHeading => _driver.FindElement(By.Id("page-primary-title"));
        private IReadOnlyCollection<IWebElement> dtConsignmentAndGoodsFields => _driver.FindElements(By.CssSelector(".review-summary-list__row"));
        private IReadOnlyCollection<IWebElement> goodsDetailsTitleList => _driver.FindElements(By.CssSelector("thead tr th"));
        private IReadOnlyCollection<IWebElement> goodsDetailsValueList => _driver.FindElements(By.CssSelector("tbody tr td"));
        #endregion

        public HealthCertificateDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Health certificate details");
        }

        public Dictionary<string, string> GetKeyAndValuesOfSummaryAndGoods()
        {
            void AddIfMissing(string key, string value)
            {
                key = key.Trim();
                value = value.Trim();

                if (!summaryAndGoodsCache.ContainsKey(key))
                    summaryAndGoodsCache[key] = value;
            }

            foreach (var keyValue in dtConsignmentAndGoodsFields)
            {
                AddIfMissing(
                    keyValue.FindElement(By.TagName("dt")).Text,
                    keyValue.FindElement(By.TagName("dd")).Text
                );
            }

            for (int i = 0; i < goodsDetailsTitleList.Count; i++)
            {
                AddIfMissing(
                    goodsDetailsTitleList.ElementAt(i).Text,
                    goodsDetailsValueList.ElementAt(i).Text
                );
            }


            return summaryAndGoodsCache;
        }
    }
}
