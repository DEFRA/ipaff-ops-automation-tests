using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CheckAndSubmitCommodityRulePage : ICheckAndSubmitCommodityRulePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Check and submit commodity rule']"), true);
        private IWebElement btnConfirmAndSubmitRule => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Confirm and submit rule']"));
        private IWebElement summaryTable => _driver.WaitForElement(By.XPath("//table[@class='govuk-table']"));
        private By summaryRowsBy => By.XPath(".//tbody//tr[@class='govuk-table__row']");
        private By summaryKeyBy => By.XPath("./th");
        private By summaryValueBy => By.XPath("./td[1]");
        #endregion

        public CheckAndSubmitCommodityRulePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Check and submit commodity rule");

        public void ClickConfirmAndSubmitRuleButton() => btnConfirmAndSubmitRule.Click();

        public IDictionary<string, string> GetSummaryDetails()
        {
            var rows = summaryTable.FindElements(summaryRowsBy);
            return rows.ToDictionary(
                r => r.FindElement(summaryKeyBy).Text.Trim(),
                r => r.FindElement(summaryValueBy).Text
                    .Trim()
                    .Replace("\u00C2\u00A0", " ")  // remove UTF-8 mojibake: Â + non-breaking space
                    .Replace("\u00A0", " ")         // remove any remaining non-breaking spaces
                    .Replace("\u00C2", ""));         // remove any remaining Â characters
        }
    }
}