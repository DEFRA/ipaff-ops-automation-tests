using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class RecordPhsiChecksPage : IRecordPhsiChecksPage
    {
        private readonly IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle =>
            _driver.WaitForElement(By.Id("page-primary-title"));

        private IWebElement chooseCommoditiesSubtitle =>
            _driver.WaitForElement(By.XPath("//h2[normalize-space()='Choose commodities to record decisions against']"));

        private IReadOnlyCollection<IWebElement> rowCheckboxes =>
            _driver.FindElements(By.CssSelector("input.govuk-checkboxes__input[id^='checkbox-']:not([id='select-all-checkbox'])"));

        private IReadOnlyCollection<IWebElement> documentaryStatusSelects =>
            _driver.FindElements(By.CssSelector("select[id^='document-status-']"));

        private IReadOnlyCollection<IWebElement> identityStatusSelects =>
            _driver.FindElements(By.CssSelector("select[id^='identity-status-']"));

        private IReadOnlyCollection<IWebElement> physicalStatusSelects =>
            _driver.FindElements(By.CssSelector("select[id^='physical-status-']"));

        private IReadOnlyCollection<IWebElement> documentaryCheckResultCells =>
            _driver.FindElements(By.CssSelector("td[id^='documentary-check-row-']"));

        private IReadOnlyCollection<IWebElement> identityCheckResultCells =>
            _driver.FindElements(By.CssSelector("td[id^='identity-check-row-']"));

        private IReadOnlyCollection<IWebElement> physicalCheckResultCells =>
            _driver.FindElements(By.CssSelector("td[id^='physical-check-row-']"));

        private By firstDocumentaryRowBy =>
            By.Id("documentary-check-row-0");

        private By selectAllCheckboxBy =>
            By.Id("select-all-checkbox");

        private By btnRecordAndSelectMoreBy =>
            By.Id("button-submit-checks");

        private By btnRecordAndFinishBy =>
            By.CssSelector("button[name='record-and-finish']");

        private By successBannerBy =>
            By.Id("success-banner");

        private By successBannerMessageBy =>
            By.Id("number-of-bulk-values-applied");
        
        private By nextPageBy =>
            By.Id("next-page");

        private bool hasNextPage =>
            _driver.FindElements(nextPageBy).Count > 0;
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public RecordPhsiChecksPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Trim()
                       .Equals("Record PHSI checks", StringComparison.OrdinalIgnoreCase)
                   && chooseCommoditiesSubtitle.Displayed;
        }

        public void RecordAllCompliantDecisionsAcrossAllPages(string decision)
        {
            var isLastPage = false;

            while (!isLastPage)
            {
                // Wait for the first documentary check row to confirm the commodity table
                // is fully loaded — this is a visible element unlike the js-hidden select-all checkbox
                _driver.WaitForElement(firstDocumentaryRowBy);

                // Determine whether this is the final page before interacting
                isLastPage = !hasNextPage;

                // 1. Select all commodity lines on the current page
                TickSelectAll();
                ValidateAllRowsAreSelected();

                // 2. Set all status dropdowns to the requested decision
                SetAllStatusDropdowns(decision);
                ValidateAllStatusDropdownsSet(decision);

                // 3. Submit the page
                if (isLastPage)
                {
                    ClickRecordAndFinish();
                }
                else
                {
                    ClickRecordAndSelectMore();

                    // Wait for the success banner — confirms POST completed and page reloaded
                    ValidateSuccessBanner();

                    // Validate all decision cells on the current page before moving on
                    ValidateAllDecisionCellsOnPage(decision);

                    // Navigate to the next page to process remaining commodity lines
                    _driver.FindElement(nextPageBy).Click();
                }
            }
        }

        // ── Private helpers ─────────────────────────────────────────────────────

        private void TickSelectAll()
        {
            // select-all-checkbox has class js-hidden so it is never visible —
            // use FindElement rather than WaitForElement to avoid the visibility timeout
            var checkbox = _driver.FindElement(selectAllCheckboxBy);
            if (!checkbox.Selected)
                checkbox.Click();
        }

        private void ValidateAllRowsAreSelected()
        {
            var uncheckedRows = rowCheckboxes.Where(c => !c.Selected).ToList();
            Assert.That(uncheckedRows, Is.Empty,
                $"Expected all commodity rows to be selected but {uncheckedRows.Count} were unchecked.");
        }

        private void SetAllStatusDropdowns(string decision)
        {
            SetSelectElements(documentaryStatusSelects, decision);
            SetSelectElements(identityStatusSelects, decision);
            SetSelectElements(physicalStatusSelects, decision);
        }

        private static void SetSelectElements(IReadOnlyCollection<IWebElement> selects, string value)
        {
            foreach (var select in selects)
            {
                var dropdown = new SelectElement(select);
                dropdown.SelectByText(value);
            }
        }

        private void ValidateAllStatusDropdownsSet(string decision)
        {
            ValidateSelectsSetTo(documentaryStatusSelects, decision, "Documentary");
            ValidateSelectsSetTo(identityStatusSelects, decision, "Identity");
            ValidateSelectsSetTo(physicalStatusSelects, decision, "Physical");
        }

        private static void ValidateSelectsSetTo(
            IReadOnlyCollection<IWebElement> selects, string expected, string checkType)
        {
            var incorrectCount = selects
                .Count(s => new SelectElement(s).SelectedOption.Text.Trim()
                    .Equals(expected, StringComparison.OrdinalIgnoreCase) == false);

            Assert.That(incorrectCount, Is.Zero,
                $"Expected all {checkType} check dropdowns to be set to '{expected}' " +
                $"but {incorrectCount} were not.");
        }

        private void ClickRecordAndSelectMore()
        {
            _driver.WaitForElement(btnRecordAndSelectMoreBy).Click();
        }

        private void ClickRecordAndFinish()
        {
            _driver.FindElement(btnRecordAndFinishBy).Click();
        }

        private void ValidateSuccessBanner()
        {
            // Re-resolve the banner via WaitForElement — ensures we wait for the reloaded
            // page's DOM rather than using a cached stale reference
            var banner = _driver.WaitForElement(successBannerBy);
            Assert.That(banner.Displayed, Is.True,
                "Expected success banner to be displayed after clicking 'Record and select more'.");

            var bannerText = _driver.WaitForElement(successBannerMessageBy).Text.Trim();
            Assert.That(bannerText, Does.Contain("decisions have been recorded"),
                $"Unexpected success banner message: '{bannerText}'.");
        }

        private void ValidateAllDecisionCellsOnPage(string expectedDecision)
        {
            ValidateCells(documentaryCheckResultCells, expectedDecision, "Documentary check decision");
            ValidateCells(identityCheckResultCells, expectedDecision, "Identity check decision");
            ValidateCells(physicalCheckResultCells, expectedDecision, "Physical check decision");
        }

        private static void ValidateCells(
            IReadOnlyCollection<IWebElement> cells, string expected, string columnName)
        {
            var incorrectCells = cells
                .Where(c => !c.Text.Trim().Equals(expected, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Assert.That(incorrectCells, Is.Empty,
                $"Expected all '{columnName}' cells to show '{expected}' " +
                $"but {incorrectCells.Count} did not.");
        }
    }
}