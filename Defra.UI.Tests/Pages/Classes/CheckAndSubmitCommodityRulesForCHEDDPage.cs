using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CheckAndSubmitCommodityRulesForCHEDDPage : ICheckAndSubmitCommodityRulesForCHEDDPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Check and submit commodity rules for CHED-D']"), true);
        private IWebElement summaryList => _driver.WaitForElement(By.Id("commodity-information"));
        private IWebElement btnConfirmAndSubmit => _driver.WaitForElement(By.XPath("//button[normalize-space()='Confirm and submit rules']"));
        private By summaryRowsBy => By.XPath(".//div[contains(@class,'govuk-summary-list__row')]");
        private By summaryKeyBy => By.XPath("./dt");
        private By summaryValueBy => By.XPath("./dd");
        #endregion

        public CheckAndSubmitCommodityRulesForCHEDDPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;

        public IDictionary<string, string> GetSummaryDetails()
        {
            var rows = summaryList.FindElements(summaryRowsBy);
            return rows.ToDictionary(
                r => r.FindElement(summaryKeyBy).Text.Trim(),
                r => r.FindElement(summaryValueBy).Text.Trim());
        }

        public int GetSummaryFieldAsInt(string field)
        {
            var details = GetSummaryDetails();
            if (!details.TryGetValue(field, out var raw))
                throw new NoSuchElementException($"Summary field '{field}' not found on Check and submit commodity rules for CHED-D page.");

            return int.Parse(raw.Replace(",", string.Empty), CultureInfo.InvariantCulture);
        }

        public void ClickConfirmAndSubmitRulesButton() => btnConfirmAndSubmit.Click();
    }
}