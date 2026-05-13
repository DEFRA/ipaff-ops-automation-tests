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
        private By firstRowCellsBy => By.XPath("./td");
        private IWebElement selectedCountText => _driver.WaitForElement(By.Id("selected-count-text"));
        private IWebElement deleteRulesButton => _driver.WaitForElement(By.Id("remove-selected-rules"));
        private IWebElement confirmDeleteButton => _driver.WaitForElement(By.Id("confirm-delete"));
        private By confirmationModalBy => By.Id("confirmation-modal");
        private IWebElement ruleCountSpan => _driver.WaitForElement(By.Id("rule-count"));
        #endregion

        public ViewAllPHSIImportCommodityRulesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            try
            {
                return pageTitle.Text.Trim().Equals("View all PHSI (Import) Commodity Rules")
                    && searchInput.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
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

        public void TickSelectToDeleteCheckboxForRuleId(string ruleId)
        {
            var checkbox = _driver.FindElement(By.Id($"rule-{ruleId}"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", checkbox);
            Thread.Sleep(300); // allow scroll to settle before click
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", checkbox);
        }

        public string GetSelectedRulesInfoText() => selectedCountText.Text.Trim();

        public void ClickDeleteRulesButton() => deleteRulesButton.Click();

        public bool IsConfirmDeletionDialogDisplayed()
        {
            var modals = _driver.FindElements(confirmationModalBy);
            return modals.Count > 0 && modals[0].Displayed;
        }

        public int GetConfirmDeletionDialogRuleCount() => int.Parse(ruleCountSpan.Text.Trim());

        public void ClickConfirmDeleteButton() => confirmDeleteButton.Click();

        public bool IsConfirmDeletionDialogClosed()
        {
            var modals = _driver.FindElements(confirmationModalBy);
            return modals.Count == 0 || !modals[0].Displayed;
        }

        public bool IsRuleIdPresent(string ruleId)
        {
            var elements = _driver.FindElements(By.Id($"rule-{ruleId}"));
            return elements.Count > 0;
        }

        public string GetSearchInputText() => searchInput.GetAttribute("value") ?? string.Empty;

        public bool IsIdColumnSorted()
        {
            var ariaSort = idHeader.GetAttribute("aria-sort");
            return !string.IsNullOrEmpty(ariaSort);
        }
    }
}