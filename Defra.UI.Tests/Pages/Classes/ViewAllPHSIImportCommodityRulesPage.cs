using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ViewAllPHSIImportCommodityRulesPage : IViewAllPHSIImportCommodityRulesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        // Column order in the rendered table — must match the HTML
        private static readonly string[] Columns =
        [
            "Id", "Description", "Commodity code", "Name", "Eppo", "Intended Use",
            "Type", "Woody/Non-woody?", "Indoor use/Outdoor use?", "Rate %",
            "Previous rate %", "Permanent", "Start date", "End date", "Countries",
            "Country groups", "Country exceptions", "Document check aligned", "Reason"
        ];

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='View all PHSI (Import) Commodity Rules']"), true);
        private IWebElement searchInput => _driver.WaitForElement(By.XPath("//input[@type='search' or contains(@aria-controls,'commodity-rules-table')]"));
        private IWebElement idHeader => _driver.WaitForElement(By.XPath("//table[@id='commodity-rules-table']//thead//th[normalize-space()='Id']"));
        private IWebElement infoLabel => _driver.FindElement(By.XPath("//div[contains(@id,'commodity-rules-table_info') or contains(@class,'dataTables_info')]"));
        private IWebElement firstRow => _driver.FindElement(By.XPath("//table[@id='commodity-rules-table']/tbody/tr[1]"));
        private By firstRowCells => By.XPath("./td");
        #endregion

        public ViewAllPHSIImportCommodityRulesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("View all PHSI (Import) Commodity Rules");

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

        public void SortByIdDescending()
        {
            // Two clicks => descending
            idHeader.Click();
            Thread.Sleep(500);
            idHeader.Click();
            Thread.Sleep(500);
        }

        public IDictionary<string, string> GetTopRowDetails()
        {
            var cells = firstRow.FindElements(firstRowCells).Select(c => c.Text.Trim()).ToList();
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
    }
}