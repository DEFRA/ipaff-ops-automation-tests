// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Extensions;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.BusinessLogic.ReferenceData;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Steps for api interacting with test data creation.
/// </summary>
[Binding]
public class WorkOrderTasksSteps : PowerAppsStepDefiner
{
    private SessionContext ctx;
    private readonly ScenarioContext scenarioContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkOrderTasksSteps"/> class.
    /// </summary>
    /// <param name="ctx">SessionContext.</param>
    public WorkOrderTasksSteps(SessionContext ctx, ScenarioContext scenarioContext = null)
    {
        this.ctx = ctx;
        this.scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Verifies that all specified Work Order Task names are present in the workorderservicetasksgrid.
    /// Refreshes the subgrid and retries for up to 3 minutes to allow tasks to finish rendering
    /// after navigation or popup dismissal — consistent with the commodity lines load pattern.
    /// </summary>
    [Then(@"I can see following Work Order Tasks (.*)")]
    public void ThenICanSeeFollowingWorkOrderTasks(string taskNamesRaw)
    {
        Driver.WaitForTransaction();

        // Parse the space-separated quoted task names from the step argument.
        // e.g. 'HMI Check' 'Document Check' 'Imports Phyto Certificate Audit' 'Identity & Physical Check'
        var expectedTaskNames = System.Text.RegularExpressions.Regex
            .Matches(taskNamesRaw, @"'([^']+)'")
            .Select(m => m.Groups[1].Value.Trim())
            .ToList();

        expectedTaskNames.Should().NotBeEmpty("at least one task name must be specified in the step.");

        WaitForWorkOrderTasksToLoad(expectedTaskNames, timeout: TimeSpan.FromMinutes(5));

        var gridContainer = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        // Final assertions after the grid has stabilised.
        foreach (var taskName in expectedTaskNames)
        {
            var taskLinks = gridContainer.FindElements(
                By.XPath($".//div[@col-id='msdyn_name']//a[@role='link' and @aria-label='{taskName}']"));

            taskLinks.Count.Should().Be(1,
                $"Expected to find exactly one Work Order Task named '{taskName}' in the grid " +
                $"but found {taskLinks.Count} after waiting.");
        }
    }

    /// <summary>
    /// Polls the Work Order Service Tasks subgrid by checking for all expected task name links,
    /// refreshing the subgrid between attempts, until all tasks are present or the timeout expires.
    /// Uses XrmApp.Entity.SubGrid.ClickCommand to target the subgrid Refresh command specifically,
    /// avoiding a full page refresh that would reset the Work Order form state.
    /// </summary>
    /// <param name="expectedTaskNames">The task names that must all be present in the grid.</param>
    /// <param name="timeout">How long to keep retrying before giving up.</param>
    private void WaitForWorkOrderTasksToLoad(List<string> expectedTaskNames, TimeSpan timeout)
    {
        const string subGridName = "workorderservicetasksgrid";
        var deadline = DateTime.UtcNow.Add(timeout);
        var attempt = 0;

        while (DateTime.UtcNow < deadline)
        {
            attempt++;

            var gridContainer = Driver.WaitUntilAvailable(
                By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
                "Work Order Service Tasks grid could not be found.");

            var missingTasks = expectedTaskNames
                .Where(taskName => gridContainer
                    .FindElements(By.XPath($".//div[@col-id='msdyn_name']//a[@role='link' and @aria-label='{taskName}']"))
                    .Count != 1)
                .ToList();

            if (missingTasks.Count == 0)
            {
                return;
            }

            XrmApp.Entity.SubGrid.ClickCommand(subGridName, "Refresh");
            Driver.WaitForTransaction();
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        // Timed out — collect final state for a useful failure message.
        var gridFinal = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        var stillMissing = expectedTaskNames
            .Where(taskName => gridFinal
                .FindElements(By.XPath($".//div[@col-id='msdyn_name']//a[@role='link' and @aria-label='{taskName}']"))
                .Count != 1)
            .ToList();

        stillMissing.Should().BeEmpty(
            $"Timed out after {timeout.TotalMinutes} minute(s) and {attempt} subgrid refresh attempt(s) " +
            $"waiting for all Work Order Tasks to appear. " +
            $"Still missing after timeout: [{string.Join(", ", stillMissing.Select(t => $"'{t}'"))}].");
    }

    /// <summary>
    /// Clicks on a Work Order Task link by name in the workorderservicetasksgrid.
    /// Retries once if an ElementClickInterceptedException is thrown — indicating a tooltip
    /// overlay (ms-Callout) is blocking the click. The retry refreshes the subgrid to
    /// collapse all rows and dismiss any active tooltip before attempting the click again.
    /// </summary>
    /// <param name="taskName">The name of the task to click.</param>
    [When(@"I click on the '(.*)' task")]
    public void WhenIClickOnTheTask(string taskName)
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<ElementClickInterceptedException>()
            .WaitAndRetry(2, _ => TimeSpan.FromSeconds(5),
                onRetry: (_, _, attempt, _) =>
                {
                    // The ms-Callout tooltip from a previously hovered row is intercepting the click.
                    // Refresh the subgrid to collapse all rows and clear the tooltip overlay.
                    XrmApp.Entity.SubGrid.ClickCommand("workorderservicetasksgrid", "Refresh");
                    Driver.WaitForTransaction();
                })
            .Execute(() =>
            {
                var taskLink = Driver.WaitUntilAvailable(
                    By.XPath($"//div[@data-id='dataSetRoot_workorderservicetasksgrid']//div[@col-id='msdyn_name']//a[@role='link' and @aria-label='{taskName}']"),
                    $"Work Order Task '{taskName}' could not be found in the grid.");

                taskLink.Click();
            });

        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Verifies that the Work Order Task popup is displayed with the expected title.
    /// </summary>
    /// <param name="expectedTitle">The expected popup title.</param>
    [Then(@"I verify the '(.*)' popup is displayed")]
    public void ThenIVerifyThePopupIsDisplayed(string expectedTitle)
    {
        Driver.WaitForTransaction();

        var popupTitle = Driver.WaitUntilAvailable(
            By.XPath("//h2[@data-id='header_title']"),
            $"Popup title header could not be found when verifying '{expectedTitle}' popup.");

        var actualTitle = popupTitle.Text
            .Replace("- Saved", string.Empty)
            .Trim();

        actualTitle.Should().Be(expectedTitle,
            $"Expected popup title to be '{expectedTitle}' but found '{actualTitle}'.");
    }

    [Then("I can see the Assign Work Order Task popup is displayed")]
    public void ThenICanSeeTheAssignWorkOrderTaskPopupIsDisplayed()
    {
        Driver.WaitForTransaction();

        var assignDialog = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='Assign' and @data-uci-dialog='true']"),
            "Assign Work Order dialog could not be found.");

        var dialogHeader = assignDialog.FindElement(By.XPath(".//h1[@data-id='assignheader_id']"));

        dialogHeader.Text.Trim().Should().Be("Assign Work Order Task",
            $"Expected dialog header to be 'Assign Work Order' but found '{dialogHeader.Text.Trim()}'.");
    }

    /// <summary>
    /// Clicks the 'Add my time' command button in the Time Recording subgrid.
    /// </summary>
    [When(@"I click Add my time within the Time Recording section")]
    public void WhenIClickAddMyTimeWithinTheTimeRecordingSection()
    {
        Driver.WaitForTransaction();

        var addMyTimeButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='trd_timerecording|NoRelationship|SubGridStandard|trd.trd_timerecording.AddMyTime.Button']"),
            "Add my time button could not be found in the Time Recording section.");

        addMyTimeButton.Click();
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Locates the Time Recording WijMo grid element, searching across both known subgrid container
    /// data-id values used by different task types:
    /// - 'timerecordings_admin_subgrid_container' (Document Check, Imports Phyto Certificate Audit)
    /// - 'timerecordings_subgrid_container' (Identity &amp; Physical Check)
    /// </summary>
    private IWebElement GetTimeRecordingGrid()
    {
        // Use an XPath union to match either known container data-id in a single search.
        return Driver.WaitUntilAvailable(
            By.XPath(
                "//div[@data-id='timerecordings_admin_subgrid_container' or @data-id='timerecordings_subgrid_container']" +
                "//div[@role='grid'][contains(@aria-label,'Time Recording')]"),
            "Time Recording grid could not be found in either 'timerecordings_admin_subgrid_container' or 'timerecordings_subgrid_container'.");
    }

    /// <summary>
    /// Locates the Time Recording Save button, searching across both known subgrid container
    /// data-id values used by different task types.
    /// </summary>
    private IWebElement GetTimeRecordingSaveButton()
    {
        return Driver.WaitUntilAvailable(
            By.XPath(
                "(//div[@data-id='timerecordings_admin_subgrid_container'] | //div[@data-id='timerecordings_subgrid_container'])" +
                "//button[@title='Save' and contains(@class,'cc-ds-header-save-btn')]"),
            "Save button could not be found in the Time Recording subgrid.");
    }

    /// <summary>
    /// Resolves the aria-colindex of a named column in the Time Recording grid by matching the column header title.
    /// This is resilient to column position differences across task types.
    /// </summary>
    /// <param name="grid">The Time Recording WijMo grid element.</param>
    /// <param name="columnTitle">The column header title e.g. 'Inspector', 'Admin', 'Travel'.</param>
    /// <returns>The aria-colindex string value for the matching column header.</returns>
    private static string GetTimeRecordingColumnIndex(IWebElement grid, string columnTitle)
    {
        var columnHeaders = grid.FindElements(
            By.XPath($".//div[@role='columnheader' and @title='{columnTitle}']"));

        columnHeaders.Should().NotBeEmpty(
            $"Could not find a column header with title '{columnTitle}' in the Time Recording grid.");

        var ariaColIndex = columnHeaders[0].GetAttribute("aria-colindex");

        ariaColIndex.Should().NotBeNullOrEmpty(
            $"Column header '{columnTitle}' did not have an aria-colindex attribute.");

        return ariaColIndex;
    }

    /// <summary>
    /// Finds the Time Recording row in the grid belonging to the current user by dynamically
    /// resolving the Inspector column index from the column header title.
    /// Retries for up to 10 seconds to allow the WijMo grid to fully render after transactions
    /// such as dialog dismissal or save operations.
    /// </summary>
    /// <param name="grid">The Time Recording WijMo grid element.</param>
    /// <param name="expectedName">The expected inspector display name e.g. 'Harbir Sandhu'.</param>
    /// <returns>The matching row element.</returns>
    private static IWebElement GetTimeRecordingRowForCurrentUser(IWebElement grid, string expectedName)
    {
        IWebElement matchingRow = null;

        Policy
            .Handle<Exception>()
            .OrResult<IWebElement>(row => row == null)
            .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                // Re-resolve the inspector column index on each attempt in case the grid
                // re-renders and the column headers are replaced in the DOM.
                var inspectorColIndex = GetTimeRecordingColumnIndex(grid, "Inspector");

                var dataRows = grid.FindElements(By.XPath(".//div[@role='row'][@aria-label='Data']"));

                if (dataRows.Count == 0)
                {
                    return matchingRow = null;
                }

                foreach (var row in dataRows)
                {
                    var inspectorCells = row.FindElements(
                        By.XPath($".//div[@role='gridcell'][@aria-colindex='{inspectorColIndex}']//span[@role='presentation']"));

                    if (inspectorCells.Count > 0 &&
                        inspectorCells[0].Text.Trim().Equals(expectedName, StringComparison.OrdinalIgnoreCase))
                    {
                        return matchingRow = row;
                    }
                }

                return matchingRow = null;
            });

        return matchingRow;
    }

    /// <summary>
    /// Verifies that a new row appears in the Time Recording subgrid containing the current user's name in the Inspector column.
    /// </summary>
    [Then(@"a new row appears in the grid containing my name")]
    public void ThenANewRowAppearsInTheGridContainingMyName()
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var grid = GetTimeRecordingGrid();
        var inspectorColIndex = GetTimeRecordingColumnIndex(grid, "Inspector");

        var inspectorCells = grid.FindElements(
            By.XPath($".//div[@role='row'][@aria-label='Data']//div[@role='gridcell'][@aria-colindex='{inspectorColIndex}']//span[@role='presentation']"));

        inspectorCells.Should().NotBeEmpty(
            "Expected at least one row in the Time Recording grid after clicking Add my time.");

        var matchingCell = inspectorCells.FirstOrDefault(
            cell => cell.Text.Trim().Equals(expectedName, StringComparison.OrdinalIgnoreCase));

        matchingCell.Should().NotBeNull(
            $"Expected to find a row with Inspector '{expectedName}' in the Time Recording grid but no matching row was found. " +
            $"Found inspectors: [{string.Join(", ", inspectorCells.Select(c => $"'{c.Text.Trim()}'"))}].");
    }

    /// <summary>
    /// Verifies that the entry status for the current user's row in the Time Recording subgrid matches the expected value.
    /// </summary>
    /// <param name="expectedEntryStatus">The expected entry status value e.g. 'Draft'.</param>
    [Then(@"the entry status is '(.*)'")]
    public void ThenTheEntryStatusIs(string expectedEntryStatus)
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var grid = GetTimeRecordingGrid();
        var entryStatusColIndex = GetTimeRecordingColumnIndex(grid, "Entry Status");

        var matchingRow = GetTimeRecordingRowForCurrentUser(grid, expectedName);

        matchingRow.Should().NotBeNull(
            $"Could not find a Time Recording row for Inspector '{expectedName}' when verifying entry status.");

        var entryStatusCell = matchingRow.FindElement(
            By.XPath($".//div[@role='gridcell'][@aria-colindex='{entryStatusColIndex}']//span[@role='presentation']"));

        entryStatusCell.Text.Trim().Should().Be(expectedEntryStatus,
            $"Expected Entry Status to be '{expectedEntryStatus}' for Inspector '{expectedName}' " +
            $"but found '{entryStatusCell.Text.Trim()}'.");
    }

    /// <summary>
    /// Enters a value into a named column cell for the current user's row in the Time Recording subgrid
    /// and stores the expected display value in the scenario context for later assertion.
    /// The column index is resolved dynamically from the column header title, making this resilient
    /// to grids where column positions differ across task types.
    /// Values >= 60 are stored as hours (to 2 decimal places if not whole), otherwise as minutes.
    /// e.g. 59 => "59 minutes", 300 => "5 hours", 599 => "9.98 hours"
    /// </summary>
    /// <param name="value">The numeric value to enter e.g. '20'.</param>
    /// <param name="columnName">The column header name e.g. 'Admin', 'Travel', 'Inspection'.</param>
    [When(@"I enter '(.*)' in the (Admin|Travel|Inspection) column")]
    public void WhenIEnterInTheColumn(string value, string columnName)
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var grid = GetTimeRecordingGrid();
        var ariaColIndex = GetTimeRecordingColumnIndex(grid, columnName);

        var matchingRow = GetTimeRecordingRowForCurrentUser(grid, expectedName);

        matchingRow.Should().NotBeNull(
            $"Could not find a Time Recording row for Inspector '{expectedName}' to enter {columnName} value.");

        var targetCell = matchingRow.FindElement(
            By.XPath($".//div[@role='gridcell'][@aria-colindex='{ariaColIndex}']"));

        targetCell.Click();
        Driver.WaitForTransaction();

        var numericInput = targetCell.FindElement(
            By.XPath(".//input[@wj-part='input' and contains(@class,'wj-numeric')]"));

        numericInput.SendKeys(Keys.Control + "a");
        numericInput.SendKeys(Keys.Delete);
        numericInput.SendKeys(value);
        numericInput.SendKeys(Keys.Tab);
        Driver.WaitForTransaction();

        var minutes = int.Parse(value);
        string expectedDisplay;

        if (minutes >= 60)
        {
            var hours = minutes / 60.0;

            if (hours % 1 == 0)
            {
                var wholeHours = (int)hours;
                expectedDisplay = wholeHours == 1 ? "1 hour" : $"{wholeHours} hours";
            }
            else
            {
                expectedDisplay = $"{hours:F2} hours";
            }
        }
        else
        {
            expectedDisplay = minutes == 1 ? "1 minute" : $"{minutes} minutes";
        }

        scenarioContext[$"{columnName}TimeValue"] = expectedDisplay;
    }

    /// <summary>
    /// Verifies that all time values entered during the scenario have been saved correctly
    /// in the Time Recording subgrid for the current user's row.
    /// Validates each column (Admin, Travel, Inspection) that was stored in the scenario context,
    /// resolving column positions dynamically from the column header title.
    /// </summary>
    [Then(@"the details are saved")]
    public void ThenTheDetailsAreSaved()
    {
        Driver.WaitForTransaction();

        var columnNamesToVerify = new[] { "Admin", "Travel", "Inspection" }
            .Where(col => scenarioContext.ContainsKey($"{col}TimeValue"))
            .ToList();

        columnNamesToVerify.Should().NotBeEmpty(
            "No time values were found in the scenario context — ensure at least one 'When I enter ... in the ... column' step ran before this step.");

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var grid = GetTimeRecordingGrid();

        var matchingRow = GetTimeRecordingRowForCurrentUser(grid, expectedName);

        matchingRow.Should().NotBeNull(
            $"Could not find the Time Recording row for Inspector '{expectedName}' when verifying save.");

        foreach (var columnName in columnNamesToVerify)
        {
            scenarioContext.TryGetValue($"{columnName}TimeValue", out string expectedDisplay);

            var ariaColIndex = GetTimeRecordingColumnIndex(grid, columnName);

            var cell = matchingRow.FindElement(
                By.XPath($".//div[@role='gridcell'][@aria-colindex='{ariaColIndex}']//span[@role='presentation']"));

            cell.Text.Trim().Should().Be(expectedDisplay,
                $"Expected {columnName} value for Inspector '{expectedName}' to display as '{expectedDisplay}' " +
                $"but found '{cell.Text.Trim()}'.");
        }
    }

    /// <summary>
    /// Selects the Time Recording row belonging to the current user in the subgrid,
    /// but only if it is not already selected.
    /// </summary>
    [When(@"I select the new row in the grid")]
    public void WhenISelectTheNewRowInTheGrid()
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var grid = GetTimeRecordingGrid();

        var matchingRow = GetTimeRecordingRowForCurrentUser(grid, expectedName);

        matchingRow.Should().NotBeNull(
            $"Could not find a Time Recording row for Inspector '{expectedName}' to select.");

        var isSelected = matchingRow.GetAttribute("aria-selected") == "true";

        if (!isSelected)
        {
            // The row selection checkbox is always rendered at aria-colindex='1' in the WijMo grid
            // and does not have a stable column header title to resolve dynamically.
            var selectionCell = matchingRow.FindElement(
                By.XPath(".//div[@role='gridcell'][@aria-colindex='1']"));

            selectionCell.Click();
            Driver.WaitForTransaction();
        }
    }

    /// <summary>
    /// Clicks the Save icon in the Time Recording subgrid header to persist inline edits.
    /// </summary>
    [When(@"I click the Save icon")]
    public void WhenIClickTheSaveIcon()
    {
        Driver.WaitForTransaction();

        GetTimeRecordingSaveButton().Click();
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Clicks the 'Submit Time' command button in the Time Recording subgrid.
    /// </summary>
    [When(@"I click Submit Time")]
    public void WhenIClickSubmitTime()
    {
        Driver.WaitForTransaction();

        var submitTimeButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='trd_timerecording|NoRelationship|SubGridStandard|trd.Mscrm.SubGrid.trd_timerecording.SubmitTimeButton']"),
            "Submit Time button could not be found in the Time Recording subgrid.");

        submitTimeButton.Click();
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Verifies the Confirm Time Entries confirmation dialog is displayed.
    /// </summary>
    [Then(@"I can see the Confirm Time Entries popup is displayed")]
    public void ThenICanSeeTheConfirmTimeEntriesPopupIsDisplayed()
    {
        Driver.WaitForTransaction();

        var dialog = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='confirmdialog'][@data-uci-dialog='true']"),
            "Confirm Time Entries dialog could not be found.");

        var titleElement = dialog.FindElement(By.XPath(".//h1[@data-id='dialogTitleText']"));

        titleElement.Text.Trim().Should().Be("Confirm Time Entries",
            $"Expected dialog title to be 'Confirm Time Entries' but found '{titleElement.Text.Trim()}'.");
    }

    /// <summary>
    /// Clicks the OK button on the currently displayed confirmation dialog.
    /// </summary>
    [When(@"I click the OK button")]
    public void WhenIClickTheOkButton()
    {
        Driver.WaitForTransaction();

        var okButton = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='confirmdialog']//button[@data-id='confirmButton']"),
            "OK button could not be found on the confirmation dialog.");

        okButton.Click();
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Clicks the 'Mark Complete' command in the current context.
    /// </summary>
    [When(@"I click Mark Complete")]
    public void WhenIClickMarkComplete()
    {
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Mark Complete");
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Verifies that a grey read-only banner is displayed with the specified text.
    /// </summary>
    /// <param name="bannerText">The expected banner text.</param>
    [Then(@"a grey banner is displayed '(.*)'")]
    public void ThenAGreyBannerIsDisplayed(string bannerText)
    {
        Driver.WaitForTransaction();

        // The read-only notification banner text is inside a span with data-id="warningNotification"
        var banner = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='warningNotification']"),
            "Read-only banner was not found.");

        banner.Text.Trim().Should().Be(bannerText,
            $"Expected read-only banner text to be '{bannerText}' but found '{banner.Text.Trim()}'.");
    }

    /// <summary>
    /// Updates the Audit Status field in the first row of the Accompanying Documents subgrid.
    /// </summary>
    /// <param name="auditStatus">The audit status value to select e.g. 'Pass'.</param>
    [When(@"I update the Audit status to '(.*)'")]
    public void WhenIUpdateTheAuditStatusTo(string auditStatus)
    {
        Driver.WaitForTransaction();

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='AccompanyingDocuments_container']//div[@role='grid'][contains(@aria-label,'All Phyto Audits')]"),
            "Accompanying Documents grid could not be found.");

        var auditStatusCell = grid.FindElement(
            By.XPath(".//div[@role='row'][@aria-label='Data']//div[@role='gridcell'][@aria-colindex='5']"));

        auditStatusCell.Click();
        Driver.WaitForTransaction();

        // The active cell expands into a WijMo combobox — click the dropdown header to open it.
        var activeCell = Driver.WaitUntilAvailable(
            By.XPath("//div[@role='gridcell'][@aria-colindex='5'][@aria-selected='true']//div[@wj-part='header']"),
            "Audit Status dropdown header could not be found.");

        activeCell.Click();
        Driver.WaitForTransaction();

        // Audit Status dropdown uses role="menuitem" and class="wj-listbox-item".
        var option = Driver.WaitUntilAvailable(
            By.XPath($"//div[contains(@class,'wj-listbox')]//div[contains(@class,'wj-listbox-item') and @role='menuitem' and normalize-space(text())='{auditStatus}']"),
            $"Audit Status option '{auditStatus}' could not be found in the dropdown.");

        option.Click();
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Updates the Date documents received field in the first row of the Accompanying Documents subgrid to today's date.
    /// </summary>
    [When(@"I update the Date documents received to today's date")]
    public void WhenIUpdateTheDateDocumentsReceivedToTodaysDate()
    {
        Driver.WaitForTransaction();

        var todayFormatted = DateTime.Today.ToString("dd/MM/yyyy");

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='AccompanyingDocuments_container']//div[@role='grid'][contains(@aria-label,'All Phyto Audits')]"),
            "Accompanying Documents grid could not be found.");

        var dateCell = grid.FindElement(
            By.XPath(".//div[@role='row'][@aria-label='Data']//div[@role='gridcell'][@aria-colindex='6']"));

        dateCell.Click();
        Driver.WaitForTransaction();

        // The active cell expands into a WijMo date input — type into wj-part="input".
        var dateInput = Driver.WaitUntilAvailable(
            By.XPath("//div[@role='gridcell'][@aria-colindex='6'][@aria-selected='true']//input[@wj-part='input']"),
            "Date documents received input could not be found.");

        dateInput.SendKeys(Keys.Control + "a");
        dateInput.SendKeys(Keys.Delete);
        dateInput.SendKeys(todayFormatted);
        dateInput.SendKeys(Keys.Tab);
        Driver.WaitForTransaction();

        scenarioContext["DateDocumentsReceived"] = todayFormatted;
    }

    /// <summary>
    /// Updates the Documents match electronic copy? field in the first row of the Accompanying Documents subgrid.
    /// </summary>
    /// <param name="value">The value to select e.g. 'Yes'.</param>
    [When(@"I update the Documents match electronic copy\? to '(.*)'")]
    public void WhenIUpdateTheDocumentsMatchElectronicCopyTo(string value)
    {
        Driver.WaitForTransaction();

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='AccompanyingDocuments_container']//div[@role='grid'][contains(@aria-label,'All Phyto Audits')]"),
            "Accompanying Documents grid could not be found.");

        var documentsMatchCell = grid.FindElement(
            By.XPath(".//div[@role='row'][@aria-label='Data']//div[@role='gridcell'][@aria-colindex='7']"));

        documentsMatchCell.Click();
        Driver.WaitForTransaction();

        // The active cell expands into a WijMo grid editor input — type the value to filter the dropdown.
        var editorInput = Driver.WaitUntilAvailable(
            By.XPath("//div[@role='gridcell'][@aria-colindex='7'][@aria-selected='true']//input[contains(@class,'wj-grid-editor')]"),
            "Documents match electronic copy? input could not be found.");

        editorInput.SendKeys(Keys.Control + "a");
        editorInput.SendKeys(Keys.Delete);
        editorInput.SendKeys(value);
        editorInput.SendKeys(Keys.Tab);
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Verifies the Audit Status field displays the expected value in the Accompanying Documents subgrid.
    /// </summary>
    /// <param name="expectedStatus">The expected audit status value e.g. 'Pass'.</param>
    [Then(@"the Audit status field is '(.*)'")]
    public void ThenTheAuditStatusFieldIs(string expectedStatus)
    {
        Driver.WaitForTransaction();

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='AccompanyingDocuments_container']//div[@role='grid'][contains(@aria-label,'All Phyto Audits')]"),
            "Accompanying Documents grid could not be found.");

        var auditStatusCell = grid.FindElement(
            By.XPath(".//div[@role='row'][@aria-label='Data']//div[@role='gridcell'][@aria-colindex='5']//span[@role='presentation']"));

        auditStatusCell.Text.Trim().Should().Be(expectedStatus,
            $"Expected Audit Status to be '{expectedStatus}' but found '{auditStatusCell.Text.Trim()}'.");
    }

    /// <summary>
    /// Verifies the Date documents received field displays today's date in the Accompanying Documents subgrid.
    /// </summary>
    [Then(@"the Date documents received field is today's date")]
    public void ThenTheDateDocumentsReceivedFieldIsTodaysDate()
    {
        Driver.WaitForTransaction();

        scenarioContext.TryGetValue("DateDocumentsReceived", out string expectedDate);
        expectedDate.Should().NotBeNullOrEmpty(
            "DateDocumentsReceived was not found in the scenario context — ensure 'When I update the Date documents received to today's date' ran before this step.");

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='AccompanyingDocuments_container']//div[@role='grid'][contains(@aria-label,'All Phyto Audits')]"),
            "Accompanying Documents grid could not be found.");

        var dateCell = grid.FindElement(
            By.XPath(".//div[@role='row'][@aria-label='Data']//div[@role='gridcell'][@aria-colindex='6']//span[@role='presentation']"));

        dateCell.Text.Trim().Should().Be(expectedDate,
            $"Expected Date documents received to be '{expectedDate}' but found '{dateCell.Text.Trim()}'.");
    }

    /// <summary>
    /// Verifies the Documents match electronic copy? field displays the expected value in the Accompanying Documents subgrid.
    /// </summary>
    /// <param name="expectedValue">The expected value e.g. 'Yes'.</param>
    [Then(@"the Documents match electronic copy\? field is '(.*)'")]
    public void ThenTheDocumentsMatchElectronicCopyFieldIs(string expectedValue)
    {
        Driver.WaitForTransaction();

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='AccompanyingDocuments_container']//div[@role='grid'][contains(@aria-label,'All Phyto Audits')]"),
            "Accompanying Documents grid could not be found.");

        var documentsMatchCell = grid.FindElement(
            By.XPath(".//div[@role='row'][@aria-label='Data']//div[@role='gridcell'][@aria-colindex='7']//span[@role='presentation']"));

        documentsMatchCell.Text.Trim().Should().Be(expectedValue,
            $"Expected Documents match electronic copy? to be '{expectedValue}' but found '{documentsMatchCell.Text.Trim()}'.");
    }

    /// <summary>
    /// Clicks the Save icon in the Accompanying Documents subgrid header to persist inline edits.
    /// </summary>
    [When(@"I click the Save icon above the Accompanying Documents grid")]
    public void WhenIClickTheSaveIconAboveTheAccompanyingDocumentsGrid()
    {
        Driver.WaitForTransaction();

        var saveButton = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='AccompanyingDocuments_container']//button[@title='Save' and contains(@class,'cc-ds-header-save-btn')]"),
            "Save button could not be found in the Accompanying Documents subgrid.");

        saveButton.Click();
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Verifies that the Accompanying Documents subgrid details have been saved by confirming the Save button is disabled.
    /// </summary>
    [Then(@"the Accompanying Documents grid details are saved")]
    public void ThenTheAccompanyingDocumentsGridDetailsAreSaved()
    {
        Driver.WaitForTransaction();

        // After saving, the Save button becomes disabled — confirming no pending changes remain.
        var saveButton = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='AccompanyingDocuments_container']//button[@title='Save' and contains(@class,'cc-ds-header-save-btn')]"),
            "Save button could not be found in the Accompanying Documents subgrid.");

        var isDisabled = saveButton.GetAttribute("disabled");

        isDisabled.Should().NotBeNull(
            "Expected the Save button to be disabled after saving the Accompanying Documents grid, indicating all changes were persisted.");
    }

    /// <summary>
    /// Verifies that the specified tab is displayed and selected on the current Work Order Task form.
    /// </summary>
    /// <param name="tabName">The name of the tab to verify e.g. 'Summary', 'Samples', 'Notes'.</param>
    [Then(@"the '(.*)' tab is displayed and selected")]
    public void ThenTheTabIsDisplayedAndSelected(string tabName)
    {
        Driver.WaitForTransaction();

        // Tabs are identified by aria-label matching the tab name and aria-selected="true" when active.
        var tab = Driver.WaitUntilAvailable(
            By.XPath($"//ul[@role='tablist']//li[@role='tab' and @aria-label='{tabName}']"),
            $"'{tabName}' tab could not be found on the Work Order Task form.");

        var isSelected = tab.GetAttribute("aria-selected");

        isSelected.Should().Be("true",
            $"Expected the '{tabName}' tab to be selected but it was not (aria-selected='{isSelected}').");
    }

    /// <summary>
    /// Verifies that the Import Commodity Lines grid is populated with between 480 and 500 rows,
    /// using the AG Grid status bar row count which reflects the total dataset regardless of virtualisation.
    /// </summary>
    [Then(@"the Import Commodity Lines grid is populated with the commodity lines from the notification created in IPAFFS")]
    public void ThenTheImportCommodityLinesGridIsPopulatedWithTheCommodityLinesFromTheNotificationCreatedInIPAFFS()
    {
        Driver.WaitForTransaction();

        var gridContainer = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_subgrid_import_commodity_lines']"),
            "Import Commodity Lines grid container could not be found.");

        // The AG Grid status bar displays the total row count regardless of virtualisation,
        // e.g. <span class="statusTextContainer-182">Rows: 499</span>
        var statusText = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_subgrid_import_commodity_lines']//span[contains(@class,'statusTextContainer') and starts-with(normalize-space(text()),'Rows:')]"),
            "Import Commodity Lines grid status bar row count could not be found.");

        var rawText = statusText.Text.Trim();

        // Parse the integer from e.g. "Rows: 499"
        var rowCountText = rawText.Replace("Rows:", string.Empty).Trim();

        int.TryParse(rowCountText, out var rowCount).Should().BeTrue(
            $"Expected the status bar to contain a parseable row count but found '{rawText}'.");

        rowCount.Should().BeGreaterThan(480,
            $"Expected more than 480 Import Commodity Lines rows but the status bar reported {rowCount}.");

        rowCount.Should().BeLessOrEqualTo(500,
            $"Expected no more than 500 Import Commodity Lines rows but the status bar reported {rowCount}.");
    }

    /// <summary>
    /// Verifies that the Lab Samples grid is empty, showing no data rows.
    /// </summary>
    [Then(@"the Lab Samples grid is empty")]
    public void ThenTheLabSamplesGridIsEmpty()
    {
        Driver.WaitForTransaction();

        var gridContainer = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_subgrid_lab_samples']"),
            "Lab Samples grid container could not be found.");

        // When empty the AG Grid renders a no-rows overlay — confirmed in the HTML.
        var noRowsOverlay = gridContainer.FindElements(
            By.XPath(".//div[contains(@class,'ag-overlay-no-rows-wrapper')]//div[contains(@class,'label')]"));

        noRowsOverlay.Should().NotBeEmpty(
            "Expected the Lab Samples grid to display the empty state overlay but it was not found.");

        noRowsOverlay[0].Text.Trim().Should().Be("We didn't find anything to show here",
            $"Expected empty Lab Samples grid message but found '{noRowsOverlay[0].Text.Trim()}'.");

        // Additionally confirm no data rows are present.
        var dataRows = gridContainer.FindElements(
            By.XPath(".//div[@role='row'][@aria-label='Press SPACE to select this row.']"));

        dataRows.Should().BeEmpty(
            $"Expected no rows in the Lab Samples grid but found {dataRows.Count}.");
    }

    /// <summary>
    /// Updates an application's work order to have submitted or unsubmitted time associated with the service tasks for a given bookable resource.
    /// </summary>
    /// <param name="applicationAlias">The alias of the application.</param>
    [Given(@"'(.*)' has (unsubmitted|submitted) time associated with the work order tasks for '(.*)'")]
    public void GivenHasUnsubmittedTimeAssociatedWithTheWorkOrderTasks(string bookableResourceAlias, string submissionStatus, string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        var bookableResource = TestDriver.GetTestRecordReference(bookableResourceAlias);

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = WorkOrderSteps.GetWorkOrder(svc, application);
            var tasks = context.msdyn_workorderservicetaskSet.Where(x => x.msdyn_WorkOrder.Id == workOrder.Id).ToList();

            foreach (var task in tasks)
            {
                var timeRecording = new trd_timerecording
                {
                    Id = Guid.NewGuid(),
                    trd_TrainingProvidedtoTrade = 60,
                    trd_TrainingProvidedtoInspectors = 30,
                    trd_TrainingReceived = 45,
                    trd_Date = DateTime.UtcNow,
                    trd_Travel = 60,
                    trd_Admin = 20,
                    trd_Inspection = 10,
                    trd_msdyn_workorderservicetask_trd_timerecording_WorkOrderServiceTask = task,
                    trd_BookableResource = bookableResource,
                    trd_WorkerOrder = workOrder
                };

                if (submissionStatus == "submitted")
                {
                    timeRecording.statecode = trd_timerecordingState.Inactive;
                    timeRecording.statuscode = trd_timerecording_statuscode.Submitted;
                }
                else
                {
                    timeRecording.statecode = trd_timerecordingState.Active;
                    timeRecording.statuscode = trd_timerecording_statuscode.Draft;
                }

                context.AddObject(timeRecording);
            }
            context.SaveChanges();
        }
    }

    [Given(@"I have opened the '(.*)' service task for '(.*)'")]
    [When(@"I open the '(.*)' service task for '(.*)'")]
    public void GivenIOpenServiceTask(string serviceTask, string applicationAlias)
    {
        // Attempt to obtain the reference via the SessionContext first, if there is no output value
        // then attempt to locate via the TestDriver.
        Driver.WaitForTransaction();
        EntityReference application = null;
        if (!this.ctx.TryGetEntityReference(applicationAlias, out application))
        {
            application = TestDriver.GetTestRecordReference(applicationAlias);
        }

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = WorkOrderSteps.GetWorkOrder(svc, application);
            var woTasks = context.msdyn_workorderservicetaskSet.Where(x => x.msdyn_WorkOrder.Id == workOrder.Id).ToList();
            var task = woTasks.Where(x => serviceTask == x.msdyn_name).FirstOrDefault();

            Driver.ExecuteScript($"Xrm.Navigation.openForm({{ entityId: \"{task.Id}\", entityName: \"{msdyn_workorderservicetask.EntityLogicalName}\" }})");
        }

        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Updates an application's work order service tasks to complete, adding time recordings for the given bookable resource.
    /// </summary>
    /// <param name="applicationAlias">The alias of the application.</param>
    [When(@"The following work order service tasks have been completed by '(.*)' for '(.*)'")]
    [Given(@"The following work order service tasks have been completed by '(.*)' for '(.*)'")]
    public void GivenWorkOrderServiceTasksAreComplete(string bookableResourceAlias, string applicationAlias, Table taskNames)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        var bookableResource = TestDriver.GetTestRecordReference(bookableResourceAlias);
        var tasksToComplete = taskNames.Rows.Select(x => x[0]).ToList();
        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = WorkOrderSteps.GetWorkOrder(svc, application);
            var woTasks = context.msdyn_workorderservicetaskSet.Where(x => x.msdyn_WorkOrder.Id == workOrder.Id).ToList();
            var tasks = woTasks.Where(x => tasksToComplete.Contains(x.msdyn_name)).OrderBy(y => y.msdyn_LineOrder).ToList();
            List<trd_inspectionresult> results = null;

            for (int i = 0; i < tasksToComplete.Count; i++)
            {
                var visit = new trd_visit
                {
                    trd_name = "Visit Place",
                    trd_DateScheduled = Helpers.RandomHelper.GetNextAvailableVisitDate(context),
                    trd_InspectionAddress = tasks[i].trd_InspectionAddressId,
                };

                context.AddObject(visit);
                context.SaveChanges();

                var taskWithVisit = new msdyn_workorderservicetask
                {
                    Id = tasks[i].Id,
                    trd_VisitId = visit.ToEntityReference(),
                };

                if (!context.IsAttached(taskWithVisit))
                {
                    context.Attach(taskWithVisit);
                }

                context.UpdateObject(taskWithVisit);
                context.SaveChanges();

                var timeRecording = new trd_timerecording
                {
                    Id = Guid.NewGuid(),
                    trd_TrainingProvidedtoTrade = 60,
                    trd_TrainingProvidedtoInspectors = 30,
                    trd_TrainingReceived = 45,
                    trd_Date = DateTime.UtcNow,
                    trd_Travel = 60,
                    trd_Admin = 20,
                    trd_Inspection = 10,
                    trd_WorkOrderServiceTask = tasks[i].ToEntityReference(),
                    trd_BookableResource = bookableResource,
                    trd_WorkerOrder = workOrder,
                };

                timeRecording.statecode = trd_timerecordingState.Inactive;
                timeRecording.statuscode = trd_timerecording_statuscode.Submitted;
                context.ClearChanges();
                context.AddObject(timeRecording);
                context.SaveChanges();

                if (results == null)
                {
                    
                    results = svc.WaitForRecords(
                        new QueryByAttribute(trd_inspectionresult.EntityLogicalName)
                        {
                            ColumnSet = new ColumnSet(false),
                            Attributes = { "trd_workorder", "trd_fminspectionresult" },
                            Values = { workOrder.Id, null },
                        },
                        TimeSpan.FromSeconds(15),
                        continueOnError: true).Entities.Select(x => x.ToEntity<trd_inspectionresult>()).ToList();
                    
                    context.ClearChanges();
                    CompleteCommodities(context, results, results.Count, "Pass");
                }

                var completedTask = new msdyn_workorderservicetask
                {
                    Id = tasks[i].Id,
                    msdyn_PercentComplete = 100,
                    statecode = msdyn_workorderservicetaskState.Inactive,
                    statuscode = msdyn_workorderservicetask_statuscode.Inactive,
                };

                if (!context.IsAttached(completedTask))
                {
                    context.Attach(completedTask);
                }

                context.UpdateObject(completedTask);
                context.SaveChanges();
            }
        }
    }

    [Given("(.*) commodities for '(.*)' have a result of '(.*)'")]
    public void GivenCommodityResultsAreRecorded(int commoditiesToComplete, string applicationAlias, string result)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = WorkOrderSteps.GetWorkOrder(svc, application);
            var query = new QueryByAttribute(trd_inspectionresult.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Attributes = { "trd_workorder", "trd_fminspectionresult" },
                Values = { workOrder.Id, null },
            };

            Microsoft.Xrm.Sdk.DataCollection<Microsoft.Xrm.Sdk.Entity> queryResults = null;
            Wait.Until(TimeSpan.FromSeconds(180), () => (queryResults = svc.RetrieveMultiple(query).Entities).Count >= commoditiesToComplete);
            context.ClearChanges();

            CompleteCommodities(context, queryResults.Select(r => r.ToEntity<trd_inspectionresult>()).ToList(), commoditiesToComplete, result);
        }
    }

    public static void CompleteCommodities(PlantsContext context, List<trd_inspectionresult> results, int commoditiesToComplete, string result)
    {
        var osvResult = msdyn_inspectionresult.Pass;
        var qtyInspected = 0;
        var qtyPassed = 0;

        switch (result)
        {
            case "Partial Pass":
                osvResult = msdyn_inspectionresult.PartialPass;
                qtyInspected = 100;
                qtyPassed = 50;
                break;

            case "Not Inspected":
                osvResult = msdyn_inspectionresult.NotInspected;
                qtyInspected = 0;
                break;

            case "Fail":
                osvResult = msdyn_inspectionresult.Fail;
                qtyInspected = 100;
                qtyPassed = 0;
                break;
        }

        for (int i = 0; i < commoditiesToComplete; i++)
        {
            var inspectionResult = new trd_inspectionresult
            {
                Id = results[i].Id,
                trd_QuantityAppliedFor = 100,
                trd_QuantityInspected = qtyInspected,
                trd_QuantityPassed = qtyPassed,
                trd_ReasonforFailure = result == "Fail" ? new EntityReference("trd_reason", ReasonForFailure.PresenceOfPestOnCommodity) : null,
                trd_FMInspectionResult = osvResult,
                statuscode = trd_inspectionresult_statuscode.Submitted,
                statecode = trd_inspectionresultState.Inactive,
            };

            if (!context.IsAttached(inspectionResult))
            {
                context.Attach(inspectionResult);
            }

            context.UpdateObject(inspectionResult);
        }

        context.SaveChanges();
    }

    /// <summary>
    /// Opens the record in the subgrid.
    /// </summary>
    /// <param name="tabName">recordIndex</param>
    /// <param name="recordIndex">tabName.</param>
    /// <param name="popupAreaName">popupAreaName.</param>
    [When(@"I select the '(.*)' tab and open record '(\d+)'")]
    public static void WhenIOpenTheRecord(string tabName, int recordIndex)
    {
        SharedSteps.WaitForScriptProcessing();
        EntitySteps.ISelectTab(tabName);
        Driver.WaitForTransaction();
        var entityName = XrmApp.Entity.GetEntityName();
        Policy
            .Handle<Exception>()
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5))
            .Execute(() =>
            {
                Capgemini.PowerApps.SpecFlowBindings.Steps.GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(recordIndex);
                XrmApp.Entity.GetEntityName().Should().NotBe(entityName);
            });
    }

    /// <summary>
    /// Opens the record in the subgrid and assigns to the current user.
    /// </summary>
    /// <param name="tabName">recordIndex</param>
    /// <param name="recordIndex">tabName.</param>
    /// <param name="popupAreaName">popupAreaName.</param>
    [When(@"I select the '(.*)' tab and open record '(\d+)' and assign to me using the '(.*)' dialog")]
    [Given(@"I select the '(.*)' tab and open record '(\d+)' and assign to me using the '(.*)' dialog")]
    public void WhenIOpenTheRecordAndAssignToMe(string tabName, int recordIndex, string popupAreaName)
    {
        WhenIOpenTheRecord(tabName, recordIndex);
        this.AssignServiceTaskToSelf();
    }

    [Given(@"I have opened a work order service task associated to '(.*)' and assigned it to myself")]
    public void WhenIOpenAServiceTask(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = WorkOrderSteps.GetWorkOrder(svc, application);
            var tasks = WorkOrderSteps.WaitForWorkServiceTasks(svc, workOrder);
            var task = tasks[new Random().Next(0, tasks.Entities.Count - 1)];

            svc.AssignEntityToUser(this.ctx.UserId, task.LogicalName, task.Id);

            Driver.ExecuteScript($"Xrm.Navigation.openForm({{ entityId: \"{task.Id}\", entityName: \"{task.LogicalName}\" }})");
            Driver.WaitForTransaction();
        }
    }

    [When(@"I completed the inspection task without inspection results")]
    public void WhenICompletedTheInspectionTaskWithoutInspectionResults()
    {
        WhenIOpenTheRecordAndAssignToMe("Work Order Tasks", 1, "msdyn_workorderservicetask");
        PopupSteps.WhenIMarkCompleteTheRecordInThePopupBox("msdyn_workorderservicetask");
    }

    public void AssignServiceTaskToSelf()
    {
        PopupSteps.WhenIAssignTheRecordInThePopupBox("msdyn_workorderservicetask");
        Driver.WaitForTransaction();
        DialogSteps.WhenIAssignToMeOnTheAssignDialog();
        Driver.WaitForTransaction();
    }

    [Then(@"I should see the value of '(.*)' in the '(.*)' optionset field of all associated HMI Results for '(.*)'")]
    public void ThenIShouldSeeTheValueOfInTheOptionsetFieldOfAllAssociatedHMIResultsFor(string attributeValue, string attributeName, string applicationAlias)
    {
        var application = this.ctx.EntityReferences
            .AsQueryable().FirstOrDefault(e => e.Key.Contains(applicationAlias)).Value;
        application.Should().NotBeNull();

        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                var workOrder = WorkOrderSteps.GetWorkOrder(serviceClient, application).ToEntity<msdyn_workorder>();
                var serviceTasks = workOrder.GetServiceTasks(context);
                serviceTasks.Should().NotBeNullOrEmpty();

                var serviceTaskType = msdyn_servicetasktype.FindTaskTypeBy("Exports HMI Check", context);
                serviceTaskType.Should().NotBeNull();

                var serviceTask = serviceTasks.FirstOrDefault(t => t.msdyn_TaskType.Id == serviceTaskType.Id);
                serviceTask.Should().NotBeNull();

                var hmiResults = serviceTask.GetHMIResults(context);
                hmiResults.Should().NotBeNullOrEmpty();
                hmiResults.ToList().ForEach(item =>
                {
                    item.trd_InspectionRequired.Should().NotBeNull();
                    item.trd_InspectionRequired.Value.Should().Be(trd_hmiinspectionrequired.Inspectionrequired);
                });
            }
        }
    }

    [Then(@"I should see the inspection type for the commodity line for '(.*)'")]
    public void ThenIShouldSeeTheInspectionTypeForTheCommodityLineFor(string applicationAlias, Table table)
    {
        var application = this.ctx.EntityReferences
            .AsQueryable().FirstOrDefault(e => e.Key.Contains(applicationAlias)).Value;
        application.Should().NotBeNull();

        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                var workOrder = WorkOrderSteps.GetWorkOrder(serviceClient, application).ToEntity<msdyn_workorder>();
                var serviceTasks = workOrder.GetServiceTasks(context);
                serviceTasks.Should().NotBeNullOrEmpty();

                var serviceTaskType = msdyn_servicetasktype.FindTaskTypeBy("Exports HMI Check", context);
                serviceTaskType.Should().NotBeNull();

                var serviceTask = serviceTasks.FirstOrDefault(t => t.msdyn_TaskType.Id == serviceTaskType.Id);
                serviceTask.Should().NotBeNull();

                var hmiResults = serviceTask.GetHMIResults(context);
                hmiResults.Should().NotBeNullOrEmpty();
                hmiResults.Should().HaveCount(table.Rows.Count);

                var itemsToCheck = hmiResults
                   .Select(i => new { Commodity = i.trd_Commodity.Name, Inspection = i.trd_InspectionRequired.ToInt() })
                   .ToList();

                // Get options from metadata
                var attributeMetadata = serviceClient.GetEntityAttributeMetadataForAttribute("trd_hmiresult", "trd_inspectionrequired");
                var options = ((PicklistAttributeMetadata)attributeMetadata).OptionSet.Options.Select(o => new { Label = o.Label.LocalizedLabels.First().Label, Value = o.Value });

                foreach (var row in table.Rows)
                {
                    var item = itemsToCheck.Single(i => i.Commodity == row["Commodity"]);
                    var option = options.Single(o => o.Value == item.Inspection);
                    option.Should().NotBeNull();
                }
            }
        }
    }

    [When(@"I activate the inactive issue phyto task")]
    public void WhenIActivateTheInactiveIssuePhytoTask()
    {
        GridSteps.WhenISelectRowInTheGrid(2, "workorderservicetasksgrid");
        GridSteps.WhenIClickCommandInTheSubgrid("Activate", "workorderservicetasksgrid");
        DialogSteps.WhenIClickTheButtonOnTheDialog("OK");
    }
}