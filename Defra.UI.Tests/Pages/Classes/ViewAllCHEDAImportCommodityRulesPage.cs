using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ViewAllCHEDAImportCommodityRulesPage : IViewAllCHEDAImportCommodityRulesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        // Column order in the rendered table — must match the HTML
        private static readonly string[] Columns =
        [
            "Id", "Description", "Commodity code", "Rate %", "Previous rate %",
            "Permanent", "Start date", "End date", "Countries", "Country groups",
            "Country exceptions", "Certified For", "Purpose", "Border Control Post", "Reason"
        ];

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='View all CHED-A (Import) Commodity Rules']"), true);
        private IWebElement searchInput => _driver.WaitForElement(By.XPath("//div[contains(@class,'dataTables_filter')]//input[@type='search']"));
        private IWebElement infoLabel => _driver.FindElement(By.XPath("//div[contains(@class,'dataTables_info')]"));
        private IWebElement firstRow => _driver.FindElement(By.XPath("//table[contains(@class,'dt-instance-required')]/tbody/tr[1]"));
        private By firstRowCellsBy => By.XPath("./td[contains(@class,'govuk-table__cell')]");
        private IWebElement RemoveRuleLink(string ruleId) => _driver.WaitForElement(By.XPath($"//a[contains(@href,'/Rules/Commodity/CHEDA/Remove?ruleId={ruleId}')]"));
        private By RemoveRuleLinkBy(string ruleId) => By.XPath($"//a[contains(@href,'/Rules/Commodity/CHEDA/Remove?ruleId={ruleId}')]");
        #endregion

        public ViewAllCHEDAImportCommodityRulesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Trim().Equals("View all CHED-A (Import) Commodity Rules");
        }

        public void ScrollToBottom() =>
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

        public int GetTotalRuleCount()
        {
            var match = Regex.Match(infoLabel.Text, @"of\s+([\d,]+)\s+entries");
            return match.Success ? int.Parse(match.Groups[1].Value.Replace(",", "")) : 0;
        }

        public void EnterSearchText(string text)
        {
            searchInput.Clear();
            searchInput.SendKeys(text);
            Thread.Sleep(1000); // allow DataTables filter to apply
        }

        public IDictionary<string, string> GetTopRowDetails()
        {
            var cells = firstRow.FindElements(firstRowCellsBy).Select(c => c.Text.Trim()).ToList();
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < Columns.Length && i < cells.Count; i++)
            {
                dict[Columns[i]] = cells[i];
            }
            return dict;
        }

        public string GetTopRowId() => GetTopRowDetails().TryGetValue("Id", out var id) ? id : string.Empty;

        public bool SwitchToNewlyOpenedTab()
        {
            var handles = _driver.WindowHandles;
            if (handles.Count > 1)
            {
                _driver.SwitchTo().Window(handles[handles.Count - 1]);
                return true;
            }
            return false;
        }

        public void ClickRemoveRuleLinkForRuleId(string ruleId)
        {
            var removeLink = RemoveRuleLink(ruleId);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", removeLink);
            Thread.Sleep(300); // allow scroll to settle before click
            removeLink.Click();
            Thread.Sleep(2000); // allow page to reload after removal
        }

        public bool IsRuleIdPresent(string ruleId)
        {
            var elements = _driver.FindElements(RemoveRuleLinkBy(ruleId));
            return elements.Count > 0;
        }
    }
}