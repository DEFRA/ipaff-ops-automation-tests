// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.Specs.Steps;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Reqnroll;

/// <summary>
/// Step bindings relating to the work order functional area.
/// </summary>
[Binding]
public sealed class WorkOrderSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;
    private readonly ScenarioContext scenarioContext;

    public WorkOrderSteps(SessionContext sessionContext, ScenarioContext scenarioContext = null)
    {
        this.sessionContext = sessionContext;
        this.scenarioContext = scenarioContext;
    }

    [Then("I verify the Work Order page is displayed for the notification created in IPAFFS")]
    public void ThenIVerifyTheWorkOrderPageIsDisplayedForTheNotificationCreatedInIPAFFS()
    {
        Driver.WaitForTransaction();

        var expectedChedReference = scenarioContext.Get<string>("CHEDReference");

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span']"),
            "Work Order page header could not be found.");

        var actualPageHeader = pageHeader.Text.Trim();
        actualPageHeader.Should().Be("Work Order",
            $"Expected page header to be 'Work Order' but found '{actualPageHeader}'.");

        var chedReferenceHeader = Driver.WaitUntilAvailable(
            By.XPath("//h1[@data-id='header_title']"),
            $"CHED reference header could not be found on the Work Order page.");

        var actualChedReference = chedReferenceHeader.Text
            .Replace("- Saved", string.Empty)
            .Trim();

        actualChedReference.Should().Be(expectedChedReference,
            $"Expected CHED reference header to be '{expectedChedReference}' but found '{actualChedReference}'.");
    }

    [When("I click the Assign command")]
    public void WhenIClickTheAssignCommand()
    {
        Driver.WaitForTransaction();

        // Determine whether the Assign command should be clicked inside a maximised popup
        // or on the main Work Order page command bar.
        //
        // When called after "I click on the '<task>' task" + "I maximise the popup", the popup
        // container is present in the DOM and its own command bar may not yet have fully rendered.
        // Clicking via CommandSteps.WhenISelectTheCommand in this state hits the Work Order page's
        // overflow button instead, causing ElementClickInterceptedException.
        //
        // When called from the Work Order page (no active popup), fall through to the standard helper.
        var popupContainers = Driver.FindElements(
            By.XPath("//section[contains(@id,'popupContainer')]"));

        if (popupContainers.Count > 0)
        {
            // Wait for the popup's own Assign button to be present and visible before clicking.
            // Scope to the popup container to avoid matching the Work Order command bar.
            var popupAssignButton = Driver.WaitUntilAvailable(
                By.XPath("//section[contains(@id,'popupContainer')]//button[@aria-label='Assign' or @title='Assign']"),
                TimeSpan.FromSeconds(30),
                "Assign button could not be found in the popup command bar within 30 seconds.");

            Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", popupAssignButton);
            Driver.WaitForTransaction();

            popupAssignButton.Click();
        }
        else
        {
            CommandSteps.WhenISelectTheCommand("Assign");
        }

        Driver.WaitForTransaction();
    }

    [Then("I can see the Assign Work Order popup is displayed")]
    public void ThenICanSeeTheAssignWorkOrderPopupIsDisplayed()
    {
        Driver.WaitForTransaction();

        var assignDialog = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='Assign' and @data-uci-dialog='true']"),
            "Assign Work Order dialog could not be found.");

        var dialogHeader = assignDialog.FindElement(By.XPath(".//h1[@data-id='assignheader_id']"));

        dialogHeader.Text.Trim().Should().Be("Assign Work Order",
            $"Expected dialog header to be 'Assign Work Order' but found '{dialogHeader.Text.Trim()}'.");
    }

    [When("I click the Assign button")]
    public void WhenIClickTheAssignButton()
    {
        Driver.WaitForTransaction();
        var assignButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='ok_id' and @title='Assign']"),
            "Assign button could not be found in the Assign Work Order dialog.");

        assignButton.Click();
        Driver.WaitForTransaction();
    }

    [Then("the Substatus of the Work Order should be Assigned")]
    public void ThenTheSubstatusOfTheWorkOrderShouldBeAssigned()
    {
        Driver.WaitForTransaction();

        var substatusLink = Driver.WaitUntilAvailable(
            By.XPath("//a[@aria-label='Assigned']"),
            "Substatus 'Assigned' could not be found in the Work Order header.");

        substatusLink.Text.Trim().Should().Be("Assigned",
            $"Expected Substatus to be 'Assigned' but found '{substatusLink.Text.Trim()}'.");
    }

    [Then("the Owner of the Work Order should be me")]
    public void ThenTheOwnerOfTheWorkOrderShouldBeMe()
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector",useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedOwner = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var ownerLink = Driver.WaitUntilAvailable(
            By.XPath($"//a[@aria-label='{expectedOwner}']"),
            $"Owner '{expectedOwner}' could not be found in the Work Order header.");

        ownerLink.Text.Trim().Should().Be(expectedOwner,
            $"Expected Owner to be '{expectedOwner}' but found '{ownerLink.Text.Trim()}'.");
    }

    [When(@"I check that the Commodity Lines frame shows '(.*)'")]
    public void WhenICheckThatTheCommodityLinesFrameShows(string expectedViewName)
    {
        Driver.WaitForTransaction();

        var viewLabel = Driver.WaitUntilAvailable(
            By.XPath($"//span[contains(@id,'ViewSelector_') and contains(@id,'_text-value') and normalize-space(text())='{expectedViewName}']"),
            $"Commodity Lines frame view '{expectedViewName}' could not be found.");
        
        viewLabel.Text.Trim().Should().Be(expectedViewName,
            $"Expected Commodity Lines frame to show '{expectedViewName}' but found '{viewLabel.Text.Trim()}'.");
    }

    private static readonly string[] ValidRegulatoryAuthorities = ["PHSI", "HMI", "Joint"];
    private const string EppoCodeColumnDataId = "trd_eppocode";
    private const string RegulatoryAuthorityColumnDataId = "trd_regulatoryauthoritycode";

    /// <summary>
    /// Verifies that all Import Commodity Lines on the Work Order match the EPPO codes
    /// provided in the test input, and that each line has a valid Regulatory Authority.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Commodity lines are loaded asynchronously from IPAFFS. The step polls the grid —
    /// refreshing the form between attempts — for up to 10 minutes until all expected rows
    /// are present before collection begins.
    /// </para>
    /// <para>
    /// The grid is sorted by EPPO code ascending before collection so that the collected
    /// order matches the in-memory sorted expected list without any further manipulation.
    /// </para>
    /// <para>
    /// Wijmo virtualises columns outside the viewport, so each page is horizontally scrolled
    /// to force both the EPPO Code (col 5) and Regulatory Authority (col 11) columns into
    /// the DOM before extraction runs. The scroll also causes Wijmo to render a frozen column
    /// panel that duplicates every row. The per-page row count is therefore measured before
    /// scrolling (no frozen panel at that point) and used as a hard limit during extraction
    /// to discard the duplicates.
    /// </para>
    /// <para>
    /// Pagination clicks are issued via JavaScript to avoid
    /// <see cref="OpenQA.Selenium.ElementClickInterceptedException"/> from a Wijmo overlay
    /// div that intercepts native clicks at certain scroll positions.
    /// </para>
    /// <para>
    /// Regulatory Authority is validated as one of the known valid values
    /// (<c>PHSI</c>, <c>HMI</c>, <c>Joint</c>) but is not compared against a specific
    /// expected value — it is set by Dynamics business logic, not sourced from IPAFFS.
    /// </para>
    /// </remarks>
    [Then(@"all the Commodity Lines should be validated with the values given in the input")]
    public void ThenAllTheCommodityLinesShouldBeValidatedWithTheValuesGivenInTheInput()
    {
        Driver.WaitForTransaction();

        var inputTable = scenarioContext["AllCommodityDetails"] as Table;
        inputTable.Should().NotBeNull("AllCommodityDetails was not found in the scenario context.");

        var expectedRowCount = inputTable.Rows.Count;

        // Poll with a page refresh until all commodity lines have loaded from IPAFFS,
        // then return with the grid on page 1 ready for collection.
        WaitForCommodityLinesToLoad(expectedRowCount, timeout: TimeSpan.FromMinutes(10));

        var sortedExpected = inputTable.Rows
            .OrderBy(r => r["EPPO code"])
            .ToList();

        SortCommodityLinesByEppoCodeAscending();
        Driver.WaitForTransaction();

        var collectedRows = new List<(string EppoCode, string RegulatoryAuthority)>();
        var pageNumber = 1;

        // Measured before any horizontal scroll — Wijmo only renders the frozen column panel
        // after scrolling, so the count here reflects the true number of rows per page.
        const string countPageRowsScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) return '0';
            return grid.querySelectorAll('div[role=""row""][aria-label=""Data""]').length.toString();
        ";

        while (true)
        {
            Driver.WaitForTransaction();

            var pageRowCount = int.Parse((Driver.ExecuteScript(countPageRowsScript) as string) ?? "0");

            // Scroll so Wijmo renders both target columns. This also causes a frozen column
            // panel to appear, doubling the DOM row count to 2× pageRowCount.
            ScrollGridUntilBothColumnsVisible();

            // Collect the first pageRowCount rows that have both target cells — this naturally
            // discards the frozen panel duplicates without needing any position-based logic.
            var extractScript = $@"
                var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
                if (!grid) return '';
                var allRows = Array.from(grid.querySelectorAll('div[role=""row""][aria-label=""Data""]'));
                var results = [];
                for (var i = 0; i < allRows.length && results.length < {pageRowCount}; i++) {{
                    var eppo = allRows[i].querySelector('div[role=""gridcell""][aria-colindex=""5""] span[role=""presentation""]');
                    var reg  = allRows[i].querySelector('div[role=""gridcell""][aria-colindex=""11""] span[role=""presentation""]');
                    if (eppo && reg) {{
                        results.push(eppo.textContent.trim() + '|||' + reg.textContent.trim());
                    }}
                }}
                return results.join(';;;');
            ";

            var pageData = Driver.ExecuteScript(extractScript) as string ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(pageData))
            {
                foreach (var entry in pageData.Split(";;;", StringSplitOptions.RemoveEmptyEntries))
                {
                    var parts = entry.Split("|||");
                    if (parts.Length == 2)
                    {
                        collectedRows.Add((parts[0], parts[1]));
                    }
                }
            }

            var nextButton = Driver.WaitUntilAvailable(
                By.XPath("//button[contains(@id,'_nextPage')]"),
                "Pagination next button could not be found.");

            if (nextButton.GetAttribute("disabled") != null)
            {
                break;
            }

            // JS click avoids ElementClickInterceptedException from the Wijmo overlay div.
            Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", nextButton);
            Driver.WaitForTransaction();
            Driver.ExecuteScript("arguments[0].click();", nextButton);
            pageNumber++;
        }

        collectedRows.Count.Should().Be(sortedExpected.Count,
            $"Expected {sortedExpected.Count} commodity lines but found {collectedRows.Count} across all pages.");

        for (var i = 0; i < sortedExpected.Count; i++)
        {
            var expectedEppo = sortedExpected[i]["EPPO code"].Trim();
            var actualEppo = collectedRows[i].EppoCode;

            actualEppo.Should().Be(expectedEppo,
                $"Row {i + 1}: Expected EPPO code '{expectedEppo}' but found '{actualEppo}'.");

            var actualRegulatoryAuthority = collectedRows[i].RegulatoryAuthority;
            ValidRegulatoryAuthorities.Should().Contain(actualRegulatoryAuthority,
                $"Row {i + 1} (EPPO: '{actualEppo}'): Regulatory Authority '{actualRegulatoryAuthority}' is not one of the valid values: {string.Join(", ", ValidRegulatoryAuthorities)}.");
        }
    }

    /// <summary>
    /// Counts the total number of commodity line data rows across all pages
    /// without collecting cell values, purely to check whether loading is complete.
    /// Leaves the grid on the last visited page.
    /// </summary>
    private int CountCommodityLinesAcrossAllPages()
    {
        var total = 0;

        while (true)
        {
            Driver.WaitForTransaction();

            var grid = Driver.WaitUntilAvailable(
                By.XPath("//div[@role='grid'][contains(@aria-label,'Import Commodity Lines')]"),
                "Import Commodity Lines grid could not be found while counting rows.");

            // No horizontal scroll occurs here, so the Wijmo frozen column panel (wj-part='fcells')
            // is not rendered — the plain row query is safe and correct in this context.
            var dataRows = grid.FindElements(
                By.XPath(".//div[@role='row'][@aria-label='Data']"));

            total += dataRows.Count;

            var nextButton = Driver.WaitUntilAvailable(
                By.XPath("//button[contains(@id,'_nextPage')]"),
                "Pagination next button could not be found while counting rows.");

            if (nextButton.GetAttribute("disabled") != null)
            {
                break;
            }

            // Scroll the button into view and click via JavaScript to avoid
            // ElementClickInterceptedException when an overlay div covers the button.
            Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", nextButton);
            Driver.WaitForTransaction();
            Driver.ExecuteScript("arguments[0].click();", nextButton);
        }

        return total;
    }

    /// <summary>
    /// Polls the Import Commodity Lines grid by counting rows across all pages,
    /// refreshing the form between attempts, until the expected count is reached
    /// or the timeout expires. Always refreshes on exit to reset the grid to page 1,
    /// since <see cref="CountCommodityLinesAcrossAllPages"/> leaves the grid on the
    /// last page after paginating through all pages.
    /// </summary>
    /// <param name="expectedRowCount">The total number of commodity lines expected.</param>
    /// <param name="timeout">How long to keep retrying before giving up.</param>
    private void WaitForCommodityLinesToLoad(int expectedRowCount, TimeSpan timeout)
    {
        var deadline = DateTime.UtcNow.Add(timeout);

        while (DateTime.UtcNow < deadline)
        {
            var currentCount = CountCommodityLinesAcrossAllPages();

            // Always refresh after counting — resets the grid to page 1 regardless of
            // whether the count was met, so callers always start from a known position.
            CommandSteps.WhenISelectTheCommand("Refresh");
            Driver.WaitForTransaction();

            if (currentCount >= expectedRowCount)
            {
                return;
            }
        }

        // Final count after timeout — let the assertion produce a clear failure message.
        var finalCount = CountCommodityLinesAcrossAllPages();
        CommandSteps.WhenISelectTheCommand("Refresh");
        Driver.WaitForTransaction();

        finalCount.Should().BeGreaterOrEqualTo(expectedRowCount,
            $"Timed out after {timeout.TotalMinutes} minutes waiting for {expectedRowCount} " +
            $"commodity lines to load. Final count across all pages was {finalCount}.");
    }
    
    private void SortCommodityLinesByEppoCodeAscending()
    {
        var eppoColumnHeaderButton = Driver.WaitUntilAvailable(
            By.XPath($"//div[contains(@id,'_headerButton{EppoCodeColumnDataId}')]"),
            "EPPO Code column header button could not be found.");

        eppoColumnHeaderButton.Click();
        Driver.WaitForTransaction();

        var sortAtoZMenuItem = Driver.WaitUntilAvailable(
            By.XPath("//button[@name='Sort A to Z' and @role='menuitemradio']"),
            "Sort A to Z menu item could not be found.");

        Driver.ExecuteScript("arguments[0].click();", sortAtoZMenuItem);
        Driver.WaitForTransaction();
    }

    private void ScrollGridUntilBothColumnsVisible()
    {
        const string findScrollRootAndCheckScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) return null;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (!scrollRoot) return null;
            var eppoVisible = grid.querySelector('div[role=""gridcell""][aria-colindex=""5""]') !== null;
            var regAuthVisible = grid.querySelector('div[role=""gridcell""][aria-colindex=""11""]') !== null;
            return JSON.stringify({
                scrollLeft: scrollRoot.scrollLeft,
                scrollWidth: scrollRoot.scrollWidth,
                clientWidth: scrollRoot.clientWidth,
                eppoVisible: eppoVisible,
                regAuthVisible: regAuthVisible
            });";

        const string scrollIncrementScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) return;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollLeft += 300; }";

        // Two separate limits:
        //   - gridReadyAttempts: how many times to wait for the grid/scroll-root to appear before giving up.
        //   - maxScrollAttempts: how many incremental scrolls to try once the grid IS present.
        // Separating them gives a clear failure message for each distinct failure mode.
        const int gridReadyAttempts = 20;
        const int maxScrollAttempts = 40;

        // Phase 1: Wait for the grid and its scroll container to be present in the DOM.
        // The Wijmo grid renders asynchronously after the tab activates, so the scroll root
        // may not exist immediately when this method is called.
        string resultJson = null;
        for (var waitAttempt = 0; waitAttempt < gridReadyAttempts; waitAttempt++)
        {
            resultJson = Driver.ExecuteScript(findScrollRootAndCheckScript) as string;

            if (resultJson != null)
            {
                break;
            }

            Driver.WaitForTransaction();
        }

        if (resultJson == null)
        {
            throw new InvalidOperationException(
                $"The Import Commodity Lines grid scroll container could not be found after waiting " +
                $"{gridReadyAttempts} attempts. The grid may not have finished rendering.");
        }

        // Phase 2: Scroll horizontally until both col 5 (EPPO Code) and col 11 (Regulatory Authority)
        // are simultaneously present in the DOM. Wijmo virtualises columns outside the viewport.
        for (var scrollAttempt = 0; scrollAttempt < maxScrollAttempts; scrollAttempt++)
        {
            var eppoVisible = resultJson.Contains(@"""eppoVisible"":true");
            var regAuthVisible = resultJson.Contains(@"""regAuthVisible"":true");

            if (eppoVisible && regAuthVisible)
            {
                return;
            }

            Driver.ExecuteScript(scrollIncrementScript);
            Driver.WaitForTransaction();

            // Re-read after each scroll — the grid may now expose different columns.
            resultJson = Driver.ExecuteScript(findScrollRootAndCheckScript) as string
                ?? throw new InvalidOperationException(
                    "The Import Commodity Lines grid scroll container disappeared during scrolling.");
        }

        throw new InvalidOperationException(
            $"Could not scroll the Commodity Lines grid to show both EPPO Code (col 5) and " +
            $"Regulatory Authority (col 11) simultaneously after {maxScrollAttempts} scroll attempts. " +
            $"Consider reviewing the column layout or increasing the max attempts.");
    }

    [When(@"I sort Commodity Lines by Regulatory Authority")]
    public void WhenISortCommodityLinesByRegulatoryAuthority()
    {
        ScrollGridUntilBothColumnsVisible();
        Driver.WaitForTransaction();

        var regulatoryAuthorityHeaderButton = Driver.WaitUntilAvailable(
            By.XPath($"//div[contains(@id,'_headerButton{RegulatoryAuthorityColumnDataId}')]"),
            "Regulatory Authority column header button could not be found.");

        regulatoryAuthorityHeaderButton.Click();
        Driver.WaitForTransaction();

        var sortAtoZMenuItem = Driver.WaitUntilAvailable(
            By.XPath("//button[@name='Sort A to Z' and @role='menuitemradio']"),
            "Sort A to Z menu item could not be found.");

        sortAtoZMenuItem.Click();
        Driver.WaitForTransaction();
    }

    [When(@"I double click on a Commodity Line with Regulatory Authority set to '(.*)'")]
    public void WhenIDoubleClickOnACommodityLineWithRegulatoryAuthoritySetTo(string regulatoryAuthority)
    {
        ScrollGridUntilBothColumnsVisible();
        Driver.WaitForTransaction();

        // EasyRepro's OpenSubGridRecord uses contains(@data-lp-id,...) which only matches the
        // AgGrid (Import tab). The Wijmo cc-grid (Commodity Lines tab) renders cells without
        // data-lp-id, so OpenSubGridRecord fails there with NoSuchElementException.
        var isAgGrid = Driver.FindElements(
            By.XPath("//div[@id='dataSetRoot_Import_notification_commodity_lines_subgrid']")).Count > 0;

        if (isAgGrid)
        {
            // For AgGrid, find the index of the matching row via Selenium and use EasyRepro.
            var grid = Driver.WaitUntilAvailable(
                By.XPath("//div[@role='grid'][contains(@aria-label,'Import Commodity Lines')]"),
                "Commodity Lines grid could not be found.");

            var dataRows = grid.FindElements(By.XPath(".//div[@role='row'][@aria-label='Data']"));
            var matchingIndex = -1;

            for (var i = 0; i < dataRows.Count; i++)
            {
                var regulatoryCells = dataRows[i].FindElements(
                    By.XPath(".//div[@role='gridcell'][@aria-colindex='11']//span[@role='presentation']"));

                if (regulatoryCells.Count > 0 &&
                    regulatoryCells[0].Text.Trim().Equals(regulatoryAuthority, StringComparison.OrdinalIgnoreCase))
                {
                    matchingIndex = i;
                    break;
                }
            }

            matchingIndex.Should().BeGreaterOrEqualTo(0,
                $"No Commodity Line with Regulatory Authority '{regulatoryAuthority}' could be found on the current page.");

            XrmApp.Entity.SubGrid.OpenSubGridRecord("Import_notification_commodity_lines_subgrid", matchingIndex);
        }
        else
        {
            // For the Wijmo cc-grid, use Selenium's Actions to perform a genuine double-click
            // on the matching regulatory authority cell. This avoids all synthetic event
            // coordinate issues and lets the browser handle the interaction natively.
            var grid = Driver.WaitUntilAvailable(
                By.XPath("//div[@role='grid'][contains(@aria-label,'Import Commodity Lines')]"),
                "Commodity Lines grid could not be found.");

            var matchingCell = grid.FindElements(
                By.XPath(".//div[@role='row'][@aria-label='Data']" +
                         $"//div[@role='gridcell'][@aria-colindex='11']" +
                         $"[.//span[@role='presentation']]"))
                .FirstOrDefault(cell =>
                    cell.FindElement(By.XPath(".//span[@role='presentation']"))
                        .Text.Trim()
                        .Equals(regulatoryAuthority, StringComparison.OrdinalIgnoreCase));

            matchingCell.Should().NotBeNull(
                $"No Commodity Line with Regulatory Authority '{regulatoryAuthority}' could be found on the current page.");

            new Actions(Driver).DoubleClick(matchingCell).Perform();
        }

        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Asserts that the specified Work Order Task in the workorderservicetasksgrid has the expected Status.
    /// </summary>
    /// <param name="taskName">The name of the Work Order Task.</param>
    /// <param name="expectedStatus">The expected Status value (e.g., "Active", "Inactive").</param>
    [Then(@"the Work Order Task '(.*)' Status is '(.*)'")]
    public void ThenTheWorkOrderTaskStatusIs(string taskName, string expectedStatus)
    {
        Driver.WaitForTransaction();

        // Find the grid container for Work Order Service Tasks
        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        // Find all rows in the AG Grid
        var rows = grid.FindElements(By.XPath(".//div[contains(@class,'ag-center-cols-container')]/div[@role='row']"));
        rows.Should().NotBeEmpty("Expected at least one row in the Work Order Service Tasks grid.");

        // Find the row where the Name column (aria-colindex='2') matches the taskName
        IWebElement matchingRow = null;
        foreach (var row in rows)
        {
            var nameCell = row.FindElements(By.XPath(".//div[@role='gridcell'][@aria-colindex='2']//span[@role='presentation']"))
                .FirstOrDefault();
            if (nameCell != null && nameCell.Text.Trim().Equals(taskName, StringComparison.OrdinalIgnoreCase))
            {
                matchingRow = row;
                break;
            }
        }

        matchingRow.Should().NotBeNull($"Could not find a Work Order Task row with Name '{taskName}'.");

        // Status is in aria-colindex='4'
        var statusCell = matchingRow.FindElement(
            By.XPath(".//div[@role='gridcell'][@aria-colindex='4']//label[@aria-label]")
        );

        var actualStatus = statusCell.GetAttribute("aria-label").Trim();

        actualStatus.Should().Be(expectedStatus,
            $"Expected Status for Work Order Task '{taskName}' to be '{expectedStatus}' but found '{actualStatus}'.");
    }

    /// <summary>
    /// Asserts that the specified Work Order Task in the workorderservicetasksgrid has the expected % Complete value.
    /// </summary>
    /// <param name="taskName">The name of the Work Order Task.</param>
    /// <param name="expectedPercentComplete">The expected % Complete value (e.g., "100.00").</param>
    [Then(@"the Work Order Task '(.*)' % Complete is '(.*)'")]
    public void ThenTheWorkOrderTaskPercentCompleteIs(string taskName, string expectedPercentComplete)
    {
        Driver.WaitForTransaction();

        // Find the grid container for Work Order Service Tasks
        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        // Find all rows in the AG Grid
        var rows = grid.FindElements(By.XPath(".//div[contains(@class,'ag-center-cols-container')]/div[@role='row']"));
        rows.Should().NotBeEmpty("Expected at least one row in the Work Order Service Tasks grid.");

        // Find the row where the Name column (aria-colindex='2') matches the taskName
        IWebElement matchingRow = null;
        foreach (var row in rows)
        {
            var nameCell = row.FindElements(By.XPath(".//div[@role='gridcell'][@aria-colindex='2']//span[@role='presentation']"))
                .FirstOrDefault();
            if (nameCell != null && nameCell.Text.Trim().Equals(taskName, StringComparison.OrdinalIgnoreCase))
            {
                matchingRow = row;
                break;
            }
        }

        matchingRow.Should().NotBeNull($"Could not find a Work Order Task row with Name '{taskName}'.");

        // % Complete is in aria-colindex='6'
        var percentCell = matchingRow.FindElement(
            By.XPath(".//div[@role='gridcell'][@aria-colindex='6']//label[@aria-label]")
        );

        var actualPercent = percentCell.GetAttribute("aria-label").Trim();

        actualPercent.Should().Be(expectedPercentComplete,
            $"Expected % Complete for Work Order Task '{taskName}' to be '{expectedPercentComplete}' but found '{actualPercent}'.");
    }

    /// <summary>
    /// Opens the Related tab dropdown and selects the specified item by scrolling it into view
    /// within the flyout before clicking. Required because the flyout container has a fixed
    /// max-height with overflow-y:auto — items below the fold (e.g. 'Charges') are present in
    /// the DOM but not clickable until scrolled into view.
    /// </summary>
    /// <param name="relatedTabName">The aria-label of the Related menu item e.g. 'Charges'.</param>
    [When(@"I select the '(.*)' tab from the Related tab dropdown")]
    public void WhenISelectTheTabFromTheRelatedTabDropdown(string relatedTabName)
    {
        Driver.WaitForTransaction();

        // Locate the Related tab in the tablist. aria-expanded indicates whether the flyout is open.
        var relatedTab = Driver.WaitUntilAvailable(
            By.XPath("//ul[@role='tablist']//li[@role='tab' and @aria-haspopup='true' and @aria-label='Related']"),
            "Related tab could not be found in the tablist.");

        // Open the flyout only if it is not already expanded.
        var isExpanded = relatedTab.GetAttribute("aria-expanded");
        if (!string.Equals(isExpanded, "true", StringComparison.OrdinalIgnoreCase))
        {
            relatedTab.Click();
            Driver.WaitForTransaction();

            // Wait until aria-expanded flips to "true" before proceeding.
            Driver.WaitUntilAvailable(
                By.XPath("//ul[@role='tablist']//li[@role='tab' and @aria-haspopup='true' and @aria-label='Related' and @aria-expanded='true']"),
                "Related tab flyout did not open (aria-expanded did not become 'true').");
        }

        // Wait for the flyout container to be present and visible.
        Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='relatedTabMenuList']"),
            $"Related tab flyout menu could not be found when selecting '{relatedTabName}'.");

        // Find the menu item by its aria-label inside the flyout.
        var menuItem = Driver.WaitUntilAvailable(
            By.XPath($"//div[@data-id='relatedTabMenuList']//div[@role='menuitem' and @aria-label='{relatedTabName}']"),
            $"Related tab menu item '{relatedTabName}' could not be found in the flyout.");

        // Scroll the item into view within the flyout — the container has a fixed max-height
        // with overflow-y:auto so items below the fold are not clickable without scrolling.
        Driver.ExecuteScript("arguments[0].scrollIntoView({block:'nearest'});", menuItem);
        Driver.WaitForTransaction();

        menuItem.Click();
        Driver.WaitForTransaction();
    }

    [Given(@"'(.*)' has updated the status of the work order associated to '(.*)' to '(.*)'")]
    public void GivenHasUpdatedTheStatusOfTheWorkOrderAssociatedToTo(string userAlias, string applicationAlias, string subStatusName)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        var user = TestConfig.GetUser(userAlias);

        using (var svcClient = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svcClient))
        {
            var substatus = context.msdyn_workordersubstatusSet.Where(s => s.msdyn_name == subStatusName).FirstOrDefault();
            if (substatus == null)
            {
                throw new ArgumentException($"Unable to find a work order sub status of {subStatusName}.");
            }

            var userObjectId = context.SystemUserSet
                .Where(u => u.DomainName == user.Username)
                .Select(u => u.AzureActiveDirectoryObjectId)
                .FirstOrDefault();

            if (!userObjectId.HasValue)
            {
                throw new ArgumentException($"Unable to find a system user with a domain name of {user.Username}.");
            }

            var workOrder = GetWorkOrder(svcClient, application);
            WaitForWorkServiceTasks(svcClient, workOrder);
            svcClient.CallerAADObjectId = userObjectId.Value;
            svcClient.Update(new msdyn_workorder { Id = workOrder.Id, msdyn_SubStatus = substatus.ToEntityReference() });
        }
    }

    [Given(@"the work order associated to '(.*)' is assigned to me")]
    public void GivenTheWorkOrderAssociatedToTheIsAssignedToMe(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svcClient = SessionContext.GetServiceClient())
        {
            var workOrder = GetWorkOrder(svcClient, application);
            WaitForWorkServiceTasks(svcClient, workOrder);
            svcClient.AssignEntityToUser(this.sessionContext.UserId, msdyn_workorder.EntityLogicalName, workOrder.Id);
        }

        new AsyncSteps(this.sessionContext).WhenIWaitForAllServiceTasksToBeOwned(applicationAlias, applicationAlias.ToLower().Contains("import") ? SpecflowBindingsConstants.DefaultTaskCountOwnedByCITImportsTeam : 0);
    }

    [When(@"I select the '(.*)' tab on the work order task")]
    public void WhenISelectTheTabOnTheWorkOrderTask(string tabName)
    {
        this.ClickTab(tabName);
    }

    [When(@"I select inspector at position '(.*)' after using search criteria '(.*)'")]
    public void WhenISelectInspectorAtPositionAfterUsingSearchCriteria(int zeroBasedIndex, string searchCriteria)
    {
        this.SelectInspector(zeroBasedIndex, searchCriteria);
    }

    [When(@"I scroll into time recording inspectors")]
    public void WhenIScrollIntoTimeRecordingInspectors()
    {
        this.ScrollIntoElement();
    }

    [When(@"I refresh the time recordings until the '(.*)' grid contains '(.*)' row\(s\)")]
    public void WhenIRefreshTheTimeRecordingsUntilTheGridContainsRows(string gridName, int expectedRowCount)
    {
        if (this.RefreshCommands.Count.Equals(2))
        {
            Wait.Until(
                TimeSpan.FromSeconds(15),
                () => GridHelper.GetRows(Driver, gridName).Count.Equals(expectedRowCount),
                () => this.RefreshCommands[1].Click());
        }
        else
        {
            throw new AutomationException("Expected 2 refresh commands in work order task");
        }
    }

    [When(@"I save the work order task")]
    public void WhenISaveTheWorkOrderTask()
    {
        this.SaveWorkOrder();
    }

    [When(@"I assign workorder to myself")]
    public void WhenIAssignWorkorderToMyself()
    {
        new DataSteps(this.sessionContext).GivenIHaveOpened("WorkOrder");
        this.WhenIAssignTheWorkOrderToMe();
    }

    [Then(@"I can see the workorder status as '(.*)'")]
    public void ThenICanSeeTheWorkorderStatusAs(string status)
    {
        Policy
                .Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(5))
                .Execute(() =>
                {
                    new DataSteps(this.sessionContext).GivenIHaveOpened("WorkOrder");
                    ThenThereIsAValueOfInTheLookupHeaderField(status, "msdyn_substatus");
                });
    }

    [Given(@"I assign the work order to me")]
    [When(@"I assign the work order to me")]
    public void WhenIAssignTheWorkOrderToMe()
    {
        var formContext = Driver.WaitUntilAvailable(By.Id("mainContent"));
        CommandSteps.ClickCommand("Refresh");
        Driver.WaitForPageToLoad();
        CommandHelper.ClickCommand(Driver, formContext, "Assign");
        DialogSteps.WhenIAssignToMeOnTheAssignDialog();
    }

    [When(@"I close the work order task")]
    public void WhenICloseTheWorkOrderTask()
    {
        this.ClosePopupContainer.Click();
    }

    // TODO: Review if the binding in Capgemini.PowerApps.SpecFlowBindings doesn't work. If so, raise an issue on the repository.
    [Then(@"there is a value of '(.*)' in the '(.*)' lookup header field")]
    public void ThenThereIsAValueOfInTheLookupHeaderField(string expectedHeaderValue, string headerName)
    {
        Driver.WaitForTransaction();

        var headerValue = this.GetHeaderValue(headerName);

        headerValue.Should().Be(expectedHeaderValue);
    }

    [Then(@"there is a value in the '(.*)' lookup header field that does not contain (.*)")]
    public void ThenThereIsADifferentValueOfInTheLookupHeaderField(string headerName, string notMatching)
    {
        Driver.WaitForTransaction();

        var headerValue = this.GetHeaderValue(headerName);

        headerValue.Should().NotContain(notMatching);
    }

    [When(@"I click cancel on the assign work order dialog")]
    public void WhenIClickCancelOnTheAssignWorkOrderDialog()
    {
        this.ClickCancel();
    }

    [When(@"I select the '(.*)' tab on the popup dialog")]
    public void WhenITabOnTheWorkOrderPopupDialogTab(string popTabName)
    {
        this.ClickWorkOrderTab(popTabName);
    }

    [When(@"I click on '(.*)' button")]
    public void WhenIClickOnButtonOnTheTable(string buttonName)
    {
        this.ClickButtonOnTable(buttonName);
    }


    [Then(@"a new window with the phyto url field value is opened")]
    public void ThenANewWindowWithThePhytoUrlFieldValueIsOpened()
    {
        var expectedURL = this.GetPhytoURLValue();
        Driver.LastWindow();
        var actualURL = Driver.Url;
        actualURL.Should().Contain(expectedURL);
    }

    [Then(@"I open and view the phyto certificate")]
    public void ThenIOpenAndViewThePhytoCertificate()
    {
        PopupSteps.WhenICloseThePopup();
        CommandSteps.WhenISelectTheCommand("View Phyto");
        ThenANewWindowWithThePhytoUrlFieldValueIsOpened();
    }

    /// <summary>
    /// Gets the cancel button for the assign dialog.
    /// </summary>
    public IWebElement CancelButton => Driver.WaitUntilAvailable(By.XPath("//button[@data-id='cancel_id']"));

    /// <summary>
    /// Clicks the cancel button on the assign dialog.
    /// </summary>
    public void ClickCancel()
    {
        this.CancelButton.Click();
    }

    public IWebElement HeaderFieldsExpand => Driver.WaitUntilAvailable(By.XPath("//button[@data-id='header_overflowButton']"));

    public IWebElement SupportingDocumentURL => Driver.WaitUntilAvailable(By.XPath("//*[contains(@data-id,'trd_documentsviewurl.fieldControl-url-action-icon')]"));

    public IWebElement PhytoURL => Driver.WaitUntilAvailable(By.XPath(".//*[@data-id='trd_phytourl.fieldControl-text-box-text']"));

    public string GetHeaderValue(string headerName)
    {
        this.HeaderFieldsExpand.Click();
        var header = Driver.WaitUntilAvailable(By.XPath($"//ul[@data-id='header_{headerName}.fieldControl-LookupResultsDropdown_{headerName}_SelectedRecordList']"));
        var headerValue = header.Text;
        this.HeaderFieldsExpand.Click();
        return headerValue;
    }

    public void ClickWorkOrderTab(string popTabName)
    {
        var tab = Driver.WaitUntilAvailable(By.XPath($"//li[@data-id='tablist-tab_{popTabName}']"));
        tab.Click();
    }

    public void ClickButtonOnTable(string buttonName)
    {
        Driver.WaitUntilAvailable(By.XPath($"//button[@title='{buttonName}' or @aria-label='{buttonName}']")).Click();
    }

    public string GetPhytoURLValue()
    {
        return this.PhytoURL.GetAttribute("value");
    }

    public IWebElement PopupContainer => Driver.WaitUntilAvailable(By.XPath("//section[contains(@id, 'popupContainer')]"));

    public IWebElement ClosePopupContainer => this.PopupContainer.FindElement(By.XPath(".//button[@data-id='dialogCloseIconButton']"));

    public IWebElement Save => this.PopupContainer.FindElement(By.XPath(".//span[@aria-label='Save']"));

    public IWebElement Search => this.PopupContainer.FindElement(By.XPath(".//div[@class='   css-1wa3eu0-placeholder']"));

    public IWebElement Close => this.PopupContainer.FindElement(By.XPath(".//span[contains (@class, 'ms-Button-label') and contains(string(), 'Close')]"));

    public IList<IWebElement> RefreshCommands => this.PopupContainer.FindElements(By.XPath(".//span[@aria-label='Refresh']"));

    public IList<IWebElement> InspectorGridCells => this.PopupContainer.FindElements(By.XPath(".//div[@data-automationid='DetailsRowCell']"));

    public IWebElement ScrollTimeRecording => this.PopupContainer.FindElement(By.XPath(".//label[contains(string(), 'Time Recording Inspectors')]"));

    public void ClickTab(string tabName)
    {
        Driver.WaitForTransaction();
        var tab = Driver.FindElement(By.XPath($"//li[@title='{tabName}' and contains(@role, 'tab')]"));
        tab.Click();
        Driver.WaitForTransaction();
    }

    public void SelectInspector(int zeroBasedIndex, string searchCriteria)
    {
        this.Search.Click();
        this.Search.InputText(searchCriteria);
        Wait.Until(TimeSpan.FromSeconds(10), () => this.InspectorGridCells.Count > 0);
        this.InspectorGridCells[zeroBasedIndex].Click();
        this.SaveWorkOrder();
    }

    public void SaveWorkOrder()
    {
        this.Save.Click();
    }

    public void ScrollIntoElement()
    {
        Driver.ExecuteScript("arguments[0].scrollIntoView(true);", this.ScrollTimeRecording);
    }

    public static EntityReference GetWorkOrder(CrmServiceClient svcClient, EntityReference application)
    {
        return svcClient.WaitForFieldValue<EntityReference>(application.LogicalName, application.Id, "trd_workorderid", TimeSpan.FromSeconds(5));
    }

    public static EntityCollection WaitForWorkServiceTasks(CrmServiceClient svcClient, EntityReference workOrder)
    {
        return svcClient.WaitForRecords(
            new QueryByAttribute(msdyn_workorderservicetask.EntityLogicalName)
            {
                Attributes = { "msdyn_workorder" },
                Values = { workOrder.Id }
            },
            TimeSpan.FromSeconds(90));
    }

    [Then(@"I can view the following Business process stages")]
    public void ThenICanViewTheFollowingBusinessProcessStages(Table table)
    {
        foreach (var row in table.Rows)
        {
            Driver.FindElement(By.XPath("//*[@title='" + row["Stages"] + "']"));
        }
    }

    /// <summary>
    /// Opens work-order for the current application. Assuming that there will be only one work-order for the given test.
    /// </summary>
    public void OpenWorkOrder()
    {
        ApplicationSteps.OpenEntity(this.sessionContext.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias));
    }

    public void AddWorkOrderToEntityHolder(EntityReference workOrder)
    {
        if (!this.sessionContext.Entities.ContainsKey(SpecflowBindingsConstants.WorkOrderAlias + this.sessionContext.SessionId))
        {
            this.sessionContext.Entities.Add($"{SpecflowBindingsConstants.WorkOrderAlias}{this.sessionContext.SessionId}",
                new EntityHolder
                {
                    Alias = SpecflowBindingsConstants.WorkOrderAlias,
                    EntityName = workOrder.LogicalName,
                    EntityCollectionName = workOrder.LogicalName + "s",
                    Id = workOrder.Id,
                });
        }
    }


    [When(@"I navigate to the service tasks grid on the work order")]
    public void WhenINavigateToTheServiceTasksGridOnTheWorkorder()
    {
        FormSteps.StoreFormValueInVariable("trd_workorderid", "Lookup", "field", "plntworkorderid", this.sessionContext);
        Capgemini.PowerApps.SpecFlowBindings.Steps.LookupSteps.WhenISelectARelatedLookupInTheForm("trd_workorderid");
        Capgemini.PowerApps.SpecFlowBindings.Steps.EntitySteps.ISelectTab("Work Order Tasks");
        Capgemini.PowerApps.SpecFlowBindings.Steps.EntitySubGridSteps.WhenISwitchToTheViewInTheSubgrid("All Work Order Service Tasks", "workorderservicetasksgrid");
    }
}