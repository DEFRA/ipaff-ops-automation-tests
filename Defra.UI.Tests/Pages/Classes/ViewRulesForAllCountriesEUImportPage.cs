using System.Text.RegularExpressions;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ViewRulesForAllCountriesEUImportPage : IViewRulesForAllCountriesEUImportPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        // Extended timeout for this page — DataTable can take ~60s to render
        private const int PageLoadTimeoutSeconds = 120;

        // Column order in the rendered table — must match the HTML
        private static readonly string[] Columns =
        [
            "Id", "Country", "Rate %", "Previous rate %",
            "Permanent rule", "Start Date", "End Date", "Last Updated", "Created"
        ];

        #region Page Objects
        private By pageTitleBy => By.XPath("//h1[normalize-space()='View rules for all countries']");
        private By searchInputBy => By.XPath("//div[contains(@class,'dataTables_filter')]//input[@type='search']");
        private IWebElement idHeader => _driver.WaitForElement(By.XPath("//table[contains(@class,'dt-instance-required')]//thead//th[normalize-space(text()[1])='Id']"));
        private IWebElement infoLabel => _driver.FindElement(By.XPath("//div[contains(@class,'dataTables_info')]"));
        private IWebElement firstRow => _driver.FindElement(By.XPath("//table[contains(@class,'dt-instance-required')]/tbody/tr[1]"));
        private By firstRowCellsBy => By.XPath("./td");
        private IWebElement RemoveRuleLink(string ruleId) => _driver.WaitForElement(By.XPath($"//a[contains(@href,'ruleId={ruleId}') and normalize-space()='Remove rule']"));
        private By RemoveRuleLinkBy(string ruleId) => By.XPath($"//a[contains(@href,'ruleId={ruleId}') and normalize-space()='Remove rule']");
        #endregion

        public ViewRulesForAllCountriesEUImportPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(PageLoadTimeoutSeconds));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(pageTitleBy));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(searchInputBy));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public int GetTotalRuleCount()
        {
            var match = Regex.Match(infoLabel.Text, @"of\s+([\d,]+)\s+entries");
            return match.Success ? int.Parse(match.Groups[1].Value.Replace(",", "")) : 0;
        }

        public void EnterSearchText(string text)
        {
            var searchInput = _driver.WaitForElement(searchInputBy);
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

        public bool IsRuleIdPresent(string ruleId)
        {
            var elements = _driver.FindElements(RemoveRuleLinkBy(ruleId));
            return elements.Count > 0;
        }

        public void ClickRemoveRuleLinkForRuleId(string ruleId)
        {
            var removeLink = RemoveRuleLink(ruleId);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", removeLink);
            Thread.Sleep(300); // allow scroll to settle before click
            removeLink.Click();
            Thread.Sleep(2000); // allow page to reload after removal
        }
    }
}