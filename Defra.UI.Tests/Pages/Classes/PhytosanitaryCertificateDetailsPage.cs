using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class PhytosanitaryCertificateDetailsPage : IPhytosanitaryCertificateDetailsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        Dictionary<string, string> summaryAndGoodsCache = [];

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IReadOnlyCollection<IWebElement> divH2Titles => _driver.FindElements(By.XPath("//h2[contains(@class,'govuk-heading-m')]"));
        private IReadOnlyCollection<IWebElement> dtConsignmentAndGoodsFields => _driver.FindElements(By.CssSelector(".review-summary-list__row"));
        private IReadOnlyCollection<IWebElement> goodsDetailsTitleList => _driver.FindElements(By.CssSelector("thead tr th"));
        private IReadOnlyCollection<IWebElement> goodsDetailsValueList => _driver.FindElements(By.CssSelector("tbody tr td"));
        private IWebElement tdQuantityTypeTitle => _driver.FindElement(By.XPath("//tr[2]/td"));
        private IWebElement tdQuantityTypeValue => _driver.FindElement(By.XPath("//tr[3]/td"));
        private IWebElement btnClone => _driver.FindElement(By.Id("clone"));
        private IWebElement btnCancel => _driver.FindElement(By.Id("search-to-clone"));
        #endregion

        public PhytosanitaryCertificateDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Phytosanitary certificate details");
        }

        public bool VerifyContentAndTitlesOnPage()
        {
            return Utils.CollectionsEqualIgnoreOrder(
                    divH2Titles.Select(x => x.Text),
                    [
                        "About the consignment",
                        "Description of goods"
                    ]
                );
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

            AddIfMissing(tdQuantityTypeTitle.Text, tdQuantityTypeValue.Text);

            return summaryAndGoodsCache;
        }

        public bool IsCloneAndCancelButtonExists()
        {
            return btnClone.Displayed && btnCancel.Displayed;
        }

        public void ClickCloneButton()
        {
            btnClone.Click();
        }
    }
}
