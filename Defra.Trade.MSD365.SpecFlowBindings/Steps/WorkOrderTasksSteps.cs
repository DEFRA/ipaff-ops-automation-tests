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
    /// Refreshes the subgrid and retries for up to 20 minutes to allow tasks to finish rendering
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

        WaitForWorkOrderTasksToLoad(expectedTaskNames, timeout: TimeSpan.FromMinutes(20));

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
    /// Uses JavaScript click to bypass any ms-Callout tooltip overlay that may be
    /// positioned over the link at the browser's hit-test coordinates.
    /// </summary>
    /// <param name="taskName">The name of the task to click.</param>
    [When(@"I click on the '(.*)' task")]
    public void WhenIClickOnTheTask(string taskName)
    {
        Driver.WaitForTransaction();

        var taskLink = Driver.WaitUntilAvailable(
            By.XPath($"//div[@data-id='dataSetRoot_workorderservicetasksgrid']//div[@col-id='msdyn_name']//a[@role='link' and @aria-label='{taskName}']"),
            $"Work Order Task '{taskName}' could not be found in the grid.");

        // Use JavaScript click to bypass the ms-Callout tooltip overlay that intercepts
        // the WebDriver click when a previously hovered row's tooltip is still visible.
        // IJavaScriptExecutor.click() does not go through the browser's hit-test pipeline
        // so the tooltip position is irrelevant.
        Driver.ExecuteScript("arguments[0].click();", taskLink);

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

        Driver.ExecuteScript("arguments[0].click();", addMyTimeButton);
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Locates the Time Recording WijMo grid element and resolves the subgrid name from the container
    /// data-id, searching across both known subgrid container data-id values used by different task types:
    /// - 'timerecordings_admin_subgrid_container' (Document Check, Imports Phyto Certificate Audit)
    /// - 'timerecordings_subgrid_container' (Identity &amp; Physical Check)
    /// </summary>
    /// <returns>
    /// A tuple of the WijMo grid element and the subgrid name
    /// e.g. ("timerecordings_admin_subgrid") derived from the matched container data-id.
    /// </returns>
    private (IWebElement Grid, string SubGridName) GetTimeRecordingGridWithName()
    {
        var container = Driver.WaitUntilAvailable(
            By.XPath(
                "//div[@data-id='timerecordings_admin_subgrid_container' or @data-id='timerecordings_subgrid_container']"),
            "Time Recording subgrid container could not be found in either 'timerecordings_admin_subgrid_container' or 'timerecordings_subgrid_container'.");

        // Derive the subgrid name by stripping '_container' from the data-id attribute.
        var containerId = container.GetAttribute("data-id");
        var subGridName = containerId.Replace("_container", string.Empty);

        var grid = container.FindElement(
            By.XPath(".//div[@role='grid'][contains(@aria-label,'Time Recording')]"));

        grid.Should().NotBeNull(
            $"Time Recording grid element could not be found inside container '{containerId}'.");

        return (grid, subGridName);
    }

    /// <summary>
    /// Locates the Time Recording WijMo grid element, searching across both known subgrid container
    /// data-id values used by different task types:
    /// - 'timerecordings_admin_subgrid_container' (Document Check, Imports Phyto Certificate Audit)
    /// - 'timerecordings_subgrid_container' (Identity &amp; Physical Check)
    /// </summary>
    private IWebElement GetTimeRecordingGrid()
    {
        return GetTimeRecordingGridWithName().Grid;
    }

    /// <summary>
    /// Verifies that the entry status for the current user's row in the Time Recording subgrid matches the expected value.
    /// Clicks the Time Recording subgrid Refresh command between retries to force the grid to reload
    /// from the server — required when the status update (e.g. Draft -> Submitted) is asynchronous
    /// and the WijMo grid does not automatically re-render after the confirmation dialog is dismissed.
    /// Retries up to 6 times with a 5-second gap (30 seconds total) before failing.
    /// </summary>
    /// <param name="expectedEntryStatus">The expected entry status value e.g. 'Draft', 'Submitted'.</param>
    [Then(@"the entry status is '(.*)'")]
    public void ThenTheEntryStatusIs(string expectedEntryStatus)
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        string actualStatus = null;

        Policy
            .Handle<Exception>()
            .OrResult<string>(status => !string.Equals(status, expectedEntryStatus, StringComparison.OrdinalIgnoreCase))
            .WaitAndRetry(6, retryAttempt =>
            {
                // Resolve the subgrid name dynamically and click its Refresh command so the grid
                // reloads the latest server-side status rather than reading a stale cached value.
                var (_, subGridName) = GetTimeRecordingGridWithName();
                XrmApp.Entity.SubGrid.ClickCommand(subGridName, "Refresh");
                Driver.WaitForTransaction();

                return TimeSpan.FromSeconds(5);
            })
            .Execute(() =>
            {
                Driver.WaitForTransaction();

                var grid = GetTimeRecordingGrid();
                var entryStatusColIndex = GetTimeRecordingColumnIndex(grid, "Entry Status");
                var matchingRow = GetTimeRecordingRowForCurrentUser(grid, expectedName);

                matchingRow.Should().NotBeNull(
                    $"Could not find a Time Recording row for Inspector '{expectedName}' when verifying entry status.");

                var entryStatusCell = matchingRow.FindElement(
                    By.XPath($".//div[@role='gridcell'][@aria-colindex='{entryStatusColIndex}']//span[@role='presentation']"));

                return actualStatus = entryStatusCell.Text.Trim();
            });

        actualStatus.Should().Be(expectedEntryStatus,
            $"Expected Entry Status to be '{expectedEntryStatus}' for Inspector '{expectedName}' " +
            $"but found '{actualStatus}' after retrying.");
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
                        By.XPath($".//div[@role='gridcell'][@aria-colindex='{inspectorColIndex}']"));

                    if (inspectorCells.Count > 0 &&
                        (inspectorCells[0].Text.Trim().Equals(expectedName, StringComparison.OrdinalIgnoreCase) ||
                         (inspectorCells[0].GetAttribute("title") ?? string.Empty).Trim().Equals(expectedName, StringComparison.OrdinalIgnoreCase)))
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
    /// Refreshes the subgrid and retries for up to 30 seconds to allow the WijMo grid to render
    /// the new row after 'Add my time' — the row creation is asynchronous and may not be immediately visible.
    /// </summary>
    [Then(@"a new row appears in the grid containing my name")]
    public void ThenANewRowAppearsInTheGridContainingMyName()
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        IWebElement matchingCell = null;

        Policy
            .Handle<Exception>()
            .OrResult<IWebElement>(cell => cell == null)
            .WaitAndRetry(6, retryAttempt =>
            {
                // Refresh the Time Recording subgrid between attempts to force the grid
                // to reload from the server — the new row may not yet be rendered after
                // 'Add my time' triggers an async record creation.
                var (_, subGridName) = GetTimeRecordingGridWithName();
                XrmApp.Entity.SubGrid.ClickCommand(subGridName, "Refresh");
                Driver.WaitForTransaction();

                return TimeSpan.FromSeconds(5);
            })
            .Execute(() =>
            {
                var grid = GetTimeRecordingGrid();
                var inspectorColIndex = GetTimeRecordingColumnIndex(grid, "Inspector");

                var inspectorCells = grid.FindElements(
                    By.XPath($".//div[@role='row'][@aria-label='Data']//div[@role='gridcell'][@aria-colindex='{inspectorColIndex}']//span[@role='presentation']"));

                return matchingCell = inspectorCells.FirstOrDefault(
                    cell => cell.Text.Trim().Equals(expectedName, StringComparison.OrdinalIgnoreCase));
            });

        matchingCell.Should().NotBeNull(
            $"Expected to find a row with Inspector '{expectedName}' in the Time Recording grid after retrying " +
            $"but no matching row was found.");
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
    /// Polls aria-selected to handle the async delay between the tab click and DOM update.
    /// </summary>
    /// <param name="tabName">The name of the tab to verify e.g. 'Summary', 'Samples', 'Notes'.</param>
    [Then(@"the '(.*)' tab is displayed and selected")]
    public void ThenTheTabIsDisplayedAndSelected(string tabName)
    {
        Driver.WaitForTransaction();

        // aria-selected is updated asynchronously after SelectTab fires the click event.
        // Poll until the attribute flips to "true" or the retry budget is exhausted.
        string actualAriaSelected = null;

        Policy
            .Handle<Exception>()
            .OrResult<bool>(selected => !selected)
            .WaitAndRetry(6, _ => TimeSpan.FromSeconds(5),
                onRetry: (_, _, attempt, _) =>
                    Console.WriteLine(
                        $"[ThenTheTabIsDisplayedAndSelected] Attempt {attempt}: " +
                        $"tab='{tabName}', aria-selected='{actualAriaSelected}' — retrying..."))
            .Execute(() =>
            {
                Driver.WaitUntilAvailable(
                    By.XPath($"//ul[@role='tablist']//li[@role='tab' and @aria-label='{tabName}']"),
                    $"'{tabName}' tab could not be found on the form.");

                // A parent form (e.g. the Work Order behind a Work Order Task popup) can have a tab with
                // the same name. The active form is the last displayed match in the DOM.
                var tab = Driver.FindElements(
                        By.XPath($"//ul[@role='tablist']//li[@role='tab' and @aria-label='{tabName}']"))
                    .Where(e => e.Displayed)
                    .LastOrDefault()
                    ?? throw new NoSuchElementException($"'{tabName}' tab is not displayed on the form.");

                actualAriaSelected = tab.GetAttribute("aria-selected");
                return actualAriaSelected == "true";
            });

        actualAriaSelected.Should().Be("true",
            $"Expected the '{tabName}' tab to be selected but it was not (aria-selected='{actualAriaSelected}') after retrying.");
    }

    private const string InspectionResultsDefaultSubGridName = "InspectionResults";
    private const string InspectionResultsQuantityBasedSubGridName = "InspectionResultQuantityBased";

    private static string InspectionResultsSubGridName =>
        Driver.FindElements(By.XPath($"//div[@data-id='dataSetRoot_{InspectionResultsQuantityBasedSubGridName}']")).Count > 0
            ? InspectionResultsQuantityBasedSubGridName
            : InspectionResultsDefaultSubGridName;
    private const string InspectionResultsContainerXPath = "//div[@data-id='dataSetRoot_InspectionResults' or @data-id='dataSetRoot_InspectionResultQuantityBased']";
    private const string InspectionResultColumnDataId = "trd_fminspectionresult";
    private const string StatusReasonColumnDataId = "statuscode";

    private static readonly Dictionary<string, string> InspectionResultsColumnDataIds = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Status Reason"] = "statuscode",
        ["Sample ID (Consignment Item)"] = "a_f4855dca5a0deb11a813000d3a2c32cb.trd_samplereference",
        ["Lot reference (Consignment Item)"] = "a_f4855dca5a0deb11a813000d3a2c32cb.trd_lotreference",
        ["Genus (Consignment Item)"] = "a_f4855dca5a0deb11a813000d3a2c32cb.trd_botanicalgenus",
        ["Species (Consignment Item)"] = "a_f4855dca5a0deb11a813000d3a2c32cb.trd_botanicalspecies",
        ["Variety (Consignment Item)"] = "a_f4855dca5a0deb11a813000d3a2c32cb.trd_variety",
        ["Description (Consignment Item)"] = "a_f4855dca5a0deb11a813000d3a2c32cb.trd_description",
        ["Total Quantity (Consignment Item)"] = "a_f4855dca5a0deb11a813000d3a2c32cb.trd_quantity",
        ["Unit of Measurement (Consignment Item)"] = "a_f4855dca5a0deb11a813000d3a2c32cb.trd_unitofmeasurement",
        ["Inspection Result"] = "trd_fminspectionresult",
        ["Reason"] = "trd_reasonforfailure",
        ["Task Type"] = "trd_tasktype",
    };

    private static readonly Dictionary<string, string> NestedRowColumnDataIds = new()
    {
        ["Country"] = "trd_countryid",
        ["Commodity Line"] = "trd_consignmentitemid",
        ["Passed Destination Countries"] = "trd_passeddestinationcountries",
        ["Reason for Country Not Valid"] = "trd_reasonforcountrynotvalid",
    };

    private static readonly Dictionary<string, string> TimeRecordingColumnDataIds = new()
    {
        ["Travel"] = "trd_travel",
        ["Inspection"] = "trd_inspection",
        ["Admin"] = "trd_admin",
        ["Training given (Client)"] = "trd_trainingprovidedtotrade",
        ["Training given (Inspector)"] = "trd_trainingprovidedtoinspectors",
        ["Training received"] = "trd_trainingreceived",
    };

    /// <summary>
    /// Verifies that each of the specified tabs is displayed on the Work Order Task form.
    /// Only visibility is checked — not selection.
    /// </summary>
    [Then(@"the '(.*)' '(.*)' '(.*)' '(.*)' tabs are displayed for the Work Order Task")]
    public void ThenTheTabsAreDisplayedForTheWorkOrderTask(string tab1, string tab2, string tab3, string tab4)
    {
        Driver.WaitForTransaction();

        foreach (var tabName in new[] { tab1, tab2, tab3, tab4 })
        {
            var tab = Driver.WaitUntilAvailable(
                By.XPath($"//ul[@role='tablist' and @aria-label='Work Order Task Form']//li[@role='tab' and @aria-label='{tabName}']"),
                $"'{tabName}' tab could not be found on the Work Order Task form.");

            tab.Displayed.Should().BeTrue($"Expected the '{tabName}' tab to be displayed on the Work Order Task form.");
        }
    }

    [When(@"I select the Visit field on the Work Order Task page")]
    public void WhenISelectTheVisitFieldOnTheWorkOrderTaskPage()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<ElementClickInterceptedException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .WaitAndRetry(3, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var visitLookup = Driver.WaitUntilAvailable(
                    By.XPath("//*[@data-id='trd_visitid.fieldControl-Lookup_trd_visitid']"),
                    "Visit lookup field could not be found on the Work Order Task page.");

                Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", visitLookup);
                visitLookup.Click();
            });

        Driver.WaitForTransaction();
    }

    [When(@"I select the New button on the Visits dropdown")]
    public void WhenISelectTheNewButtonOnTheVisitsDropdown()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<ElementClickInterceptedException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .WaitAndRetry(3, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var newButton = Driver.WaitUntilAvailable(
                    By.XPath("//*[@data-id='trd_visitid.fieldControl-LookupResultsPopup_trd_visitid_addNewBtn_0' or @data-id='trd_visitid.fieldControl-LookupResultsDropdown_trd_visitid_addNewBtn_0']"),
                    "New button could not be found on the Visits lookup dropdown.");

                newButton.Click();
            });

        Driver.WaitForTransaction();
    }

    [When(@"I scroll down to the Inspection Results grid on the Work Order Task page")]
    public void WhenIScrollDownToTheInspectionResultsGridOnTheWorkOrderTaskPage()
    {
        Driver.WaitForTransaction();

        var container = Driver.WaitUntilAvailable(
            By.XPath(InspectionResultsContainerXPath),
            "Inspection Results grid could not be found on the Work Order Task page.");

        Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", container);
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Verifies the number of data rows in the Inspection Results grid. Refreshes the subgrid
    /// between attempts to allow the asynchronously generated inspection results to render.
    /// </summary>
    [Then(@"I can see (\d+) rows? in the Inspection Results grid")]
    public void ThenICanSeeRowInTheInspectionResultsGrid(int expectedRowCount)
    {
        Driver.WaitForTransaction();

        var actualRowCount = Policy
            .Handle<Exception>()
            .OrResult<int>(count => count != expectedRowCount)
            .WaitAndRetry(6, _ =>
            {
                XrmApp.Entity.SubGrid.ClickCommand(InspectionResultsSubGridName, "Refresh");
                Driver.WaitForTransaction();
                return TimeSpan.FromSeconds(5);
            })
            .Execute(() =>
            {
                Driver.WaitForTransaction();
                return GetInspectionResultsDataRows(GetInspectionResultsGrid()).Count;
            });

        actualRowCount.Should().Be(expectedRowCount,
            $"Expected {expectedRowCount} row(s) in the Inspection Results grid but found {actualRowCount}.");
    }

    [Then(@"^I can see the following columns are present in the Inspection Results grid ((?:'[^']+'\s*)+)$")]
    public void ThenICanSeeTheFollowingColumnsArePresentInTheInspectionResultsGrid(
        string columns)
    {
        Driver.WaitForTransaction();

        foreach (var columnName in System.Text.RegularExpressions.Regex.Matches(columns, "'([^']+)'").Select(m => m.Groups[1].Value))
        {
            var headerTitle = ReadPotatoGrid(() =>
            {
                var header = InspectionResultsColumnDataIds.TryGetValue(columnName, out var dataId)
                    ? ScrollInspectionResultsGridUntilColumnVisible(dataId)
                    : FindTaskGridHeader(InspectionResultsContainerXPath, columnName);

                if (header == null)
                    throw new NoSuchElementException($"Column '{columnName}' could not be found in the Inspection Results grid.");

                return ReadTaskGridHeaderText(header);
            });

            headerTitle
                .Should().BeEquivalentTo(columnName, $"Inspection Results should contain '{columnName}'");
        }

        ResetInspectionResultsGridHorizontalScroll();
    }

    /// <summary>
    /// Selects the first row in the Inspection Results grid if it is not already selected.
    /// </summary>
    [When(@"I select the row in the Inspection Results grid")]
    public void WhenISelectTheRowInTheInspectionResultsGrid()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<ElementClickInterceptedException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                ResetInspectionResultsGridHorizontalScroll();

                var row = GetInspectionResultsDataRows(GetInspectionResultsGrid()).FirstOrDefault();

                row.Should().NotBeNull("Could not find a row in the Inspection Results grid to select.");

                if (row.GetAttribute("aria-selected") != "true")
                {
                    // The row selection checkbox is always rendered at aria-colindex='1' in the WijMo grid.
                    var selectionCell = row.FindElement(By.XPath(".//div[@role='gridcell'][@aria-colindex='1']"));
                    selectionCell.Click();
                    Driver.WaitForTransaction();
                }
            });
    }

    /// <summary>
    /// Verifies that the only editable column in the first row of the Inspection Results grid
    /// is the specified column, using the cell aria-readonly attribute.
    /// </summary>
    [Then(@"I can only edit the '(.*)' column in the Inspection Results grid")]
    public void ThenICanOnlyEditTheColumnInTheInspectionResultsGrid(string expectedEditableColumn)
    {
        Driver.WaitForTransaction();

        var editableColumns = new HashSet<string>();
        var checkedColumns = new HashSet<string>();

        foreach (var (columnName, dataId) in InspectionResultsColumnDataIds)
        {
            Policy
                .Handle<StaleElementReferenceException>()
                .WaitAndRetry(3, _ => TimeSpan.FromSeconds(2))
                .Execute(() =>
                {
                    var header = ScrollInspectionResultsGridUntilColumnVisible(dataId);

                    if (header == null)
                    {
                        return;
                    }

                    var row = GetInspectionResultsDataRows(GetInspectionResultsGrid()).FirstOrDefault();

                    row.Should().NotBeNull("Could not find a row in the Inspection Results grid.");

                    var cell = GetInspectionResultsCellForHeader(row, header);

                    if (cell == null)
                    {
                        return;
                    }

                    checkedColumns.Add(columnName);

                    if (cell.GetAttribute("aria-readonly") != "true")
                    {
                        editableColumns.Add(columnName);
                    }
                });
        }

        ResetInspectionResultsGridHorizontalScroll();

        checkedColumns.Should().Contain(expectedEditableColumn,
            $"The '{expectedEditableColumn}' column could not be checked in the Inspection Results grid.");

        editableColumns.Should().BeEquivalentTo(new[] { expectedEditableColumn },
            $"Expected only '{expectedEditableColumn}' to be editable in the Inspection Results grid but editable columns were " +
            $"[{string.Join(", ", editableColumns)}].");
    }

    [When(@"I expand the row in the Inspection Results grid")]
    public void WhenIExpandTheRowInTheInspectionResultsGrid()
    {
        Driver.WaitForTransaction();

        ClickInspectionResultsNestedExpander(".//*[contains(@class,'cc-grid-nested-expander-expand')]");
    }

    [Then(@"a nested row is displayed under the Inspection Results grid")]
    public void ThenANestedRowIsDisplayedUnderTheInspectionResultsGrid()
    {
        Driver.WaitForTransaction();

        var nestedGrid = Policy
            .Handle<Exception>()
            .OrResult<IWebElement>(grid => grid == null)
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(GetInspectionResultsNestedGrid);

        nestedGrid.Should().NotBeNull("Expected a nested row to be displayed under the Inspection Results grid.");
        nestedGrid.Displayed.Should().BeTrue("Expected the nested row under the Inspection Results grid to be visible.");
    }

    [Then(@"I can see the following columns are present in the nested row '(.*)' '(.*)' '(.*)' '(.*)'")]
    public void ThenICanSeeTheFollowingColumnsArePresentInTheNestedRow(string c1, string c2, string c3, string c4)
    {
        Driver.WaitForTransaction();

        var nestedGrid = GetInspectionResultsNestedGrid();

        nestedGrid.Should().NotBeNull("The nested row could not be found under the Inspection Results grid.");

        foreach (var columnName in new[] { c1, c2, c3, c4 })
        {
            NestedRowColumnDataIds.TryGetValue(columnName, out var dataId).Should().BeTrue(
                $"No data-id mapping is defined for nested row column '{columnName}'.");

            var headers = nestedGrid.FindElements(
                By.XPath($".//div[contains(@class,'wj-header')][@data-id='{dataId}']"));

            headers.Should().NotBeEmpty(
                $"Column '{columnName}' (data-id '{dataId}') could not be found in the nested row.");

            var headerTitle = (headers[0].GetAttribute("title") ?? headers[0].Text ?? string.Empty).Trim();

            headerTitle.Should().Be(columnName,
                $"Expected the nested row column with data-id '{dataId}' to be titled '{columnName}' but found '{headerTitle}'.");
        }
    }

    [When(@"I contract row in the Inspection Results grid")]
    public void WhenIContractRowInTheInspectionResultsGrid()
    {
        Driver.WaitForTransaction();

        ClickInspectionResultsNestedExpander(".//*[contains(@class,'cc-grid-nested-expander') and not(contains(@class,'cc-grid-nested-expander-expand'))]");
    }

    [Then(@"the nested row is hidden under the Inspection Results grid")]
    public void ThenTheNestedRowIsHiddenUnderTheInspectionResultsGrid()
    {
        Driver.WaitForTransaction();

        var nestedGrid = Policy
            .Handle<Exception>()
            .OrResult<IWebElement>(grid => grid != null)
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(GetInspectionResultsNestedGrid);

        nestedGrid.Should().BeNull(
            "Expected the nested row under the Inspection Results grid to be hidden.");
    }

    [When(@"I click the Pass Selected button above the Inspection Results grid")]
    public void WhenIClickThePassSelectedButtonAboveTheInspectionResultsGrid()
    {
        Driver.WaitForTransaction();

        var passSelectedButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='trd_inspectionresult|NoRelationship|SubGridStandard|trd.trd_inspectionresult.PassSelected']"),
            "Pass Selected button could not be found above the Inspection Results grid.");

        passSelectedButton.Click();
        Driver.WaitForTransaction();

        XrmApp.Entity.Save();
        Driver.WaitForTransaction();
    }

    [Then(@"the Inspection Result is '(.*)'")]
    public void ThenTheInspectionResultIs(string expectedInspectionResult)
    {
        var actual = GetInspectionResultsFirstRowCellValueWithRefresh(InspectionResultColumnDataId, expectedInspectionResult);

        actual.Should().Be(expectedInspectionResult,
            $"Expected Inspection Result to be '{expectedInspectionResult}' but found '{actual}' after retrying.");
    }

    [When(@"I click the Submit Results button above the Inspection Results grid")]
    public void WhenIClickTheSubmitResultsButtonAboveTheInspectionResultsGrid()
    {
        Driver.WaitForTransaction();

        var submitResultsButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='trd_inspectionresult|NoRelationship|SubGridStandard|trd.trd_inspectionresult.SubmitResults']"),
            "Submit Results button could not be found above the Inspection Results grid.");

        submitResultsButton.Click();
        Driver.WaitForTransaction();

        XrmApp.Entity.Save();
        Driver.WaitForTransaction();
    }

    [Then(@"the Status Reason is '(.*)'")]
    public void ThenTheStatusReasonIs(string expectedStatusReason)
    {
        var actual = GetInspectionResultsFirstRowCellValueWithRefresh(StatusReasonColumnDataId, expectedStatusReason);

        actual.Should().Be(expectedStatusReason,
            $"Expected Status Reason to be '{expectedStatusReason}' but found '{actual}' after retrying.");
    }

    [When(@"I select the Time Recording Inspectors field on the Work Order Task page")]
    public void WhenISelectTheTimeRecordingInspectorsFieldOnTheWorkOrderTaskPage()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<ElementClickInterceptedException>()
            .Or<NoSuchElementException>()
            .WaitAndRetry(3, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var container = Driver.WaitUntilAvailable(
                    By.XPath("//div[@data-id='trd_timerecordinginspectorids.fieldControl_container']"),
                    "Time Recording Inspectors field could not be found on the Work Order Task page.");

                Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", container);
                Driver.WaitForTransaction();

                container.FindElement(By.XPath(".//div[contains(@class,'-control')]")).Click();
            });

        Driver.WaitForTransaction();
    }

    [When(@"I search for my name in the Time Recording Inspectors field")]
    public void WhenISearchForMyNameInTheTimeRecordingInspectorsField()
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<ElementClickInterceptedException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .WaitAndRetry(3, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var searchInput = Driver.WaitUntilAvailable(
                    By.XPath("//div[@data-id='trd_timerecordinginspectorids.fieldControl_container']//input"),
                    "Time Recording Inspectors search input could not be found.");

                searchInput.SendKeys(Keys.Control + "a");
                searchInput.SendKeys(Keys.Backspace);
                searchInput.SendKeys(expectedName);
                Driver.WaitForTransaction();

                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(Driver, TimeSpan.FromSeconds(20))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(250),
                };
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

                // Wait until the input holds the full typed text.
                wait.Until(_ => string.Equals(searchInput.GetAttribute("value"), expectedName, StringComparison.OrdinalIgnoreCase));

                // Wait until the filtered drop down has settled: every visible option contains the name
                // and the first (auto focused) option is the match. Tab then locks in that option.
                wait.Until(_ =>
                {
                    var options = Driver.FindElements(By.XPath("//div[contains(@class,'-menu')]//div[contains(@class,'-option')]"));
                    return options.Count > 0
                        && options.All(o => o.Text.Contains(expectedName, StringComparison.OrdinalIgnoreCase));
                });

                Thread.Sleep(500);

                // Re-check after a short pause in case a late refresh changed the list.
                var firstOption = Driver.FindElements(By.XPath("//div[contains(@class,'-menu')]//div[contains(@class,'-option')]")).FirstOrDefault();
                if (firstOption == null || !firstOption.Text.Contains(expectedName, StringComparison.OrdinalIgnoreCase))
                {
                    throw new WebDriverTimeoutException($"Drop down did not settle on '{expectedName}'.");
                }

                searchInput.SendKeys(Keys.Tab);
            });

        Driver.WaitForTransaction();
    }

    [Then(@"a new row appears in the Time Recording grid containing my name")]
    public void ThenANewRowAppearsInTheTimeRecordingGridContainingMyName()
    {
        ThenANewRowAppearsInTheGridContainingMyName();
    }

    /// <summary>
    /// Enters a numeric value into the named column for the current user's row in the Time Recording subgrid.
    /// The column is resolved by its data-id first, falling back to the column header title.
    /// </summary>
    /// <param name="value">The numeric value to enter e.g. 5.</param>
    /// <param name="columnName">The column name e.g. 'Travel', 'Training given (Client)'.</param>
    [When(@"I enter (\d+) in the '(.*)' column")]
    public void WhenIEnterValueInTheNamedColumn(int value, string columnName)
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .WaitAndRetry(3, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                ScrollTimeRecordingGridToStart();

                var grid = GetTimeRecordingGrid();

                var matchingRow = GetTimeRecordingRowForCurrentUser(grid, expectedName);

                matchingRow.Should().NotBeNull(
                    $"Could not find a Time Recording row for Inspector '{expectedName}' to enter {columnName} value.");

                var rowIndex = matchingRow.GetAttribute("aria-rowindex");

                var ariaColIndex = ScrollTimeRecordingGridUntilColumnVisible(columnName);

                var cellXPath = $".//div[@role='row'][@aria-label='Data'][@aria-rowindex='{rowIndex}']//div[@role='gridcell'][@aria-colindex='{ariaColIndex}']";

                // Wijmo re-renders cells after a scroll/click, so always re-locate rather than reuse references.
                // Bring the cell fully into view first so the click does not trigger a further scroll/re-render.
                Driver.ExecuteScript(
                    "arguments[0].scrollIntoView({block:'nearest',inline:'nearest'});",
                    GetTimeRecordingGrid().FindElement(By.XPath(cellXPath)));
                Thread.Sleep(500);

                GetTimeRecordingGrid().FindElement(By.XPath(cellXPath)).Click();
                Thread.Sleep(500);
                Driver.WaitForTransaction();

                var inputXPath = cellXPath + "//input[@wj-part='input' and contains(@class,'wj-numeric')]";

                // The editor can be re-rendered between keystrokes, so re-locate it for each action.
                void SendToEditor(string keys) =>
                    Policy
                        .Handle<StaleElementReferenceException>()
                        .WaitAndRetry(3, _ => TimeSpan.FromMilliseconds(300))
                        .Execute(() => GetTimeRecordingGrid().FindElement(By.XPath(inputXPath)).SendKeys(keys));

                SendToEditor(Keys.Control + "a");
                SendToEditor(Keys.Delete);
                SendToEditor(value.ToString());
                SendToEditor(Keys.Tab);
                Driver.WaitForTransaction();

                // Leave the grid at the far left so later steps relying on the Inspector column still work.
                ScrollTimeRecordingGridToStart();
            });
    }

    /// <summary>
    /// Scrolls the Time Recording grid back to the far left so the Inspector column is rendered and the
    /// current user's row can be matched by name. Required because Wijmo removes the Inspector column
    /// from the DOM once the grid is scrolled to the far-right Training columns.
    /// </summary>
    private static void ScrollTimeRecordingGridToStart()
    {
        const string scrollToStartScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Time Recording""]');
            if (!grid) return;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.querySelector('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollLeft = 0; }";

        Driver.ExecuteScript(scrollToStartScript);
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Wijmo virtualises columns
    /// scrolled horizontally. Scrolls the grid's wj-part="root" container in steps until a header whose
    /// visible text matches the column name is rendered, and returns its aria-colindex.
    /// </summary>
    private static string ScrollTimeRecordingGridUntilColumnVisible(string columnName)
    {
        const string findColumnScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Time Recording""]');
            if (!grid) return null;
            var headers = grid.querySelectorAll('div[role=""columnheader""]');
            for (var i = 0; i < headers.length; i++) {
                if (headers[i].textContent.trim() === arguments[0]) return headers[i].getAttribute('aria-colindex');
            }
            return null;";

        const string scrollIncrementScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Time Recording""]');
            if (!grid) return;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.querySelector('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollLeft += 300; }";

        const int maxScrollAttempts = 20;

        for (var attempt = 0; attempt < maxScrollAttempts; attempt++)
        {
            if (Driver.ExecuteScript(findColumnScript, columnName) is string ariaColIndex)
            {
                return ariaColIndex;
            }

            Driver.ExecuteScript(scrollIncrementScript);
            Driver.WaitForTransaction();
        }

        throw new InvalidOperationException(
            $"The '{columnName}' column could not be found in the Time Recording grid after {maxScrollAttempts} scroll attempts.");
    }

    private static IWebElement GetInspectionResultsGrid()
    {
        var container = Driver.WaitUntilAvailable(
            By.XPath(InspectionResultsContainerXPath),
            "Inspection Results grid container could not be found.");

        return container;
    }

    private static IWebElement GetInspectionResultsCellForHeader(IWebElement row, IWebElement header)
    {
        var left = header.GetCssValue("left");

        return row
            .FindElements(By.XPath(".//div[@role='gridcell']"))
            .FirstOrDefault(c => c.GetCssValue("left") == left);
    }

    private static IReadOnlyCollection<IWebElement> GetInspectionResultsDataRows(IWebElement grid)
    {
        return grid.FindElements(By.XPath(".//div[@role='row'][@aria-label='Data']"));
    }

    private static IWebElement GetInspectionResultsNestedGrid()
    {
        var container = Driver.WaitUntilAvailable(
            By.XPath(InspectionResultsContainerXPath),
            "Inspection Results grid container could not be found.");

        return container
            .FindElements(By.XPath(".//div[contains(@class,'wj-header')][@data-id='trd_countryid']"))
            .Any(h => h.Displayed)
            ? container
            : null;
    }

    private static void ClickInspectionResultsNestedExpander(string expanderXPath)
    {
        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<ElementClickInterceptedException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                ResetInspectionResultsGridHorizontalScroll();

                var row = GetInspectionResultsDataRows(GetInspectionResultsGrid()).FirstOrDefault();

                row.Should().NotBeNull("Could not find a row in the Inspection Results grid.");

                var expander = row.FindElement(By.XPath(expanderXPath));
                expander.Click();
            });

        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Reads the first-row cell value for a given column in the Inspection Results grid, refreshing the
    /// subgrid between retries until it matches the expected value — status changes are asynchronous.
    /// </summary>
    private static string GetInspectionResultsFirstRowCellValueWithRefresh(string columnDataId, string expectedValue)
    {
        Driver.WaitForTransaction();

        string actualValue = null;

        Policy
            .Handle<Exception>()
            .OrResult<string>(value => !string.Equals(value, expectedValue, StringComparison.OrdinalIgnoreCase))
            .WaitAndRetry(6, _ =>
            {
                XrmApp.Entity.SubGrid.ClickCommand(InspectionResultsSubGridName, "Refresh");
                Driver.WaitForTransaction();
                return TimeSpan.FromSeconds(5);
            })
            .Execute(() =>
            {
                Driver.WaitForTransaction();

                var header = ScrollInspectionResultsGridUntilColumnVisible(columnDataId);

                header.Should().NotBeNull($"Column with data-id '{columnDataId}' could not be found in the Inspection Results grid.");

                var row = GetInspectionResultsDataRows(GetInspectionResultsGrid()).FirstOrDefault();

                row.Should().NotBeNull("Could not find a row in the Inspection Results grid.");

                var cell = GetInspectionResultsCellForHeader(row, header);

                cell.Should().NotBeNull($"No cell found under column '{columnDataId}' in the Inspection Results grid.");

                return actualValue = cell.Text.Trim();
            });

        ResetInspectionResultsGridHorizontalScroll();

        return actualValue;
    }

    /// <summary>
    /// Horizontally scrolls the Inspection Results WijMo grid until the column header with the given
    /// data-id is rendered. WijMo virtualises columns, so headers outside the viewport are not in the DOM.
    /// </summary>
    /// <returns>The column header element, or null if it could not be found.</returns>
    private static IWebElement ScrollInspectionResultsGridUntilColumnVisible(string dataId)
    {
        const string scrollIncrementScript = @"
            var container = Array.from(document.querySelectorAll('div[data-id=""dataSetRoot_InspectionResults""], div[data-id=""dataSetRoot_InspectionResultQuantityBased""]')).find(e => e.getClientRects().length);
            if (!container) return false;
            var grid = container.querySelector('div[role=""grid""]');
            if (!grid) return false;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.querySelector('[wj-part=""root""]') || grid.parentElement;
            if (!scrollRoot) return false;
            var before = scrollRoot.scrollLeft;
            scrollRoot.scrollLeft += 300;
            return scrollRoot.scrollLeft !== before;";

        const int maxScrollAttempts = 40;
        var headerXPath = $".//div[contains(@class,'wj-header')][@data-id='{dataId}']";

        ResetInspectionResultsGridHorizontalScroll();

        for (var attempt = 0; attempt < maxScrollAttempts; attempt++)
        {
            var headers = GetInspectionResultsGrid().FindElements(By.XPath(headerXPath));

            if (headers.Count > 0)
            {
                return headers[0];
            }

            var scrolled = Driver.ExecuteScript(scrollIncrementScript) as bool? ?? false;
            Driver.WaitForTransaction();

            if (!scrolled)
            {
                headers = GetInspectionResultsGrid().FindElements(By.XPath(headerXPath));
                return headers.FirstOrDefault();
            }
        }

        return null;
    }

    private static void ResetInspectionResultsGridHorizontalScroll()
    {
        Driver.ExecuteScript(@"
            var container = Array.from(document.querySelectorAll('div[data-id=""dataSetRoot_InspectionResults""], div[data-id=""dataSetRoot_InspectionResultQuantityBased""]')).find(e => e.getClientRects().length);
            if (!container) return;
            var grid = container.querySelector('div[role=""grid""]');
            if (!grid) return;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.querySelector('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollLeft = 0; }");
        Driver.WaitForTransaction();
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

    [When(@"I click the Save button on the Work Order Task page")]
    public void WhenIClickTheSaveButtonOnTheWorkOrderTaskPage()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<ElementClickInterceptedException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .WaitAndRetry(3, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var saveButton = Driver.WaitUntilAvailable(
                    By.XPath("//button[@data-id='msdyn_workorderservicetask|NoRelationship|Form|Mscrm.Form.msdyn_workorderservicetask.Save']"),
                    "Save button could not be found on the Work Order Task page.");

                saveButton.Click();
            });

        Driver.WaitForTransaction();
    }

    #region Potato inspection page objects
    private const string SelectedCommoditiesXPath = "//*[@data-id='dataSetRoot_subgrid_selected_commodities']";
    private const string DeclarationsXPath = "//*[@data-id='dataSetRoot_declarations_subgrid']";
    private const string SelectedDeclarationsXPath = "//*[@data-id='dataSetRoot_SelectedDeclarations_grid']";
    private const string TimeRecordingXPath = "//*[@data-id='timerecordings_admin_subgrid_container' or @data-id='timerecordings_subgrid_container']";
    private static readonly By AddDeclarationsButton = By.CssSelector("[data-id='trd_declaration|NoRelationship|SubGridStandard|trd.trd_declaration.Button.AddDeclarations']");
    private static readonly Dictionary<string, string> SummaryFieldIds = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Task Type"] = "msdyn_tasktype.fieldControl-Lookup_msdyn_tasktype",
        ["Work Order Status"] = "QV_Work_Order_Sub_Status.msdyn_substatus.fieldControl-Lookup_msdyn_substatus",
        ["Date Assigned"] = "trd_dateassigned.fieldControl._datecontrol-date-container",
        ["% Complete"] = "msdyn_percentcomplete.fieldControl-pcf-container-id",
        ["Charge"] = "trd_chargeid.fieldControl-Lookup_trd_chargeid",
        ["Status Reason"] = "statuscode.fieldControl-pcf-container-id",
    };
    private static readonly Dictionary<string, string> EditableLookupIds = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Visit"] = "trd_visitid",
        ["Inspector Assigned"] = "trd_inspectorassignedid",
    };
    #endregion

    [Then("the following fields are displayed and read only on the Work Order Task page")]
    public void ThenTheFollowingFieldsAreDisplayedAndReadOnlyOnTheWorkOrderTaskPage(Table table)
    {
        foreach (var row in table.Rows)
        {
            var field = row["Field"];
            var actual = ReadPotatoGrid(() =>
            {
                var control = GetTaskControl(SummaryFieldIds[field]);
                return (Displayed: control.Displayed, ReadOnly: IsTaskControlReadOnly(control), Value: ReadTaskControlValue(control));
            });
            actual.Displayed.Should().BeTrue($"'{field}' should be displayed");
            actual.ReadOnly.Should().BeTrue($"'{field}' should be read only");
            var expected = row["Expected value"];
            if (field == "Date Assigned")
            {
                DateTime.ParseExact(actual.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).Date
                    .Should().Be(DateTime.Today, "Date Assigned should be today's date");
            }
            else if (field == "% Complete")
            {
                ParsePotatoQuantity(actual.Value).Should().Be(ParsePotatoQuantity(expected));
            }
            else
            {
                actual.Value.Should().Be(expected == "Blank" ? string.Empty : expected, $"'{field}' should match the expected value");
            }
        }
    }

    [Then("the {string} and {string} fields are displayed and editable on the Work Order Task page")]
    public void ThenTheFieldsAreDisplayedAndEditableOnTheWorkOrderTaskPage(string field1, string field2)
    {
        foreach (var field in new[] { field1, field2 })
        {
            var editable = ReadPotatoGrid(() =>
            {
                var id = EditableLookupIds[field];
                var lookup = GetTaskControl($"{id}.fieldControl-Lookup_{id}");
                var search = GetTaskControl($"{id}.fieldControl-Lookup_{id}_microsoftIcon_searchButton");
                return lookup.Displayed && !IsTaskControlReadOnly(lookup) && search.Displayed && search.Enabled
                    && search.GetAttribute("aria-disabled") != "true";
            });
            editable.Should().BeTrue($"'{field}' should be displayed and editable");
        }
    }

    [Then("the Potato Type displayed in the Inspection Results grid matches the Exporter Portal {string}")]
    public void ThenThePotatoTypeDisplayedInTheInspectionResultsGridMatchesTheExporterPortal(string application)
    {
        scenarioContext.ContainsKey($"{application}_APHAReference").Should().BeTrue();
        ReadInspectionValue("Potato Type (Consignment Item)").Should().Be(scenarioContext.Get<string>("PotatoType"));
    }

    [Then("the Quantity Applied For in the Inspection Results grid matches the Exporter Portal {string}")]
    public void ThenTheQuantityAppliedForInTheInspectionResultsGridMatchesTheExporterPortal(string application)
    {
        scenarioContext.ContainsKey($"{application}_APHAReference").Should().BeTrue();
        Math.Round(ParsePotatoQuantity(ReadInspectionValue("Quantity Applied For")), 2)
            .Should().Be(Math.Round(ParsePotatoQuantity(scenarioContext.Get<string>("PotatoQuantity")), 2));
    }

    [Then("the {string} {string} {string} {string} fields are blank in the Inspection Results grid")]
    public void ThenTheFieldsAreBlankInTheInspectionResultsGrid(string field1, string field2, string field3, string field4)
    {
        foreach (var field in new[] { field1, field2, field3, field4 })
        {
            ReadInspectionValue(field).Should().BeNullOrWhiteSpace($"'{field}' should be blank, not zero or a selected option");
        }
    }

    [When("I enter the Quantity Applied For into the Quantity Inspected field in the Inspection Results grid")]
    public void WhenIEnterTheQuantityAppliedForIntoTheQuantityInspectedFieldInTheInspectionResultsGrid()
    {
        EnterInspectionQuantity("Quantity Inspected");
    }

    [When("I enter the Quantity Applied For into the Quantity Passed field in the Inspection Results grid")]
    public void WhenIEnterTheQuantityAppliedForIntoTheQuantityPassedFieldInTheInspectionResultsGrid()
    {
        EnterInspectionQuantity("Quantity Passed");
    }

    [Then("the Quantity Inspected matches the Quantity Applied For")]
    public void ThenTheQuantityInspectedMatchesTheQuantityAppliedFor() => AssertInspectionQuantities("Quantity Inspected", "Quantity Applied For");

    [Then("the Quantity Passed matches the Quantity Applied For")]
    public void ThenTheQuantityPassedMatchesTheQuantityAppliedFor() => AssertInspectionQuantities("Quantity Passed", "Quantity Applied For");

    [Then("the Quantity Exported matches the Quantity Passed")]
    public void ThenTheQuantityExportedMatchesTheQuantityPassed() => AssertInspectionQuantities("Quantity Exported", "Quantity Passed");

    [Then("the following columns are displayed and read only in the Selected Commodity Lines subgrid {string} {string} {string} {string} {string} {string}")]
    public void ThenTheFollowingColumnsAreDisplayedAndReadOnlyInTheSelectedCommodityLinesSubgrid(string column1, string column2, string column3, string column4, string column5, string column6)
    {
        foreach (var column in new[] { column1, column2, column3, column4, column5, column6 })
        {
            var readOnly = ReadPotatoGrid(() =>
            {
                var header = FindTaskGridHeader(SelectedCommoditiesXPath, column);
                var rows = GetTaskGridRows(GetTaskGrid(SelectedCommoditiesXPath));
                if (rows.Count == 0) throw new NoSuchElementException("Selected Commodity Lines has no data rows.");
                return rows.All(row => IsTaskControlReadOnly(GetTaskGridCell(row, header)));
            });
            readOnly.Should().BeTrue($"'{column}' should be displayed and read only in Selected Commodity Lines");
        }
    }

    [Then("the following columns are displayed in the Declarations subgrid {string} {string} {string} {string} {string} {string}")]
    public void ThenTheFollowingColumnsAreDisplayedInTheDeclarationsSubgrid(string column1, string column2, string column3, string column4, string column5, string column6)
        => AssertTaskGridColumns(DeclarationsXPath, column1, column2, column3, column4, column5, column6);

    [Then("the following columns are displayed in the Selected Declarations subgrid {string} {string} {string} {string} {string} {string}")]
    public void ThenTheFollowingColumnsAreDisplayedInTheSelectedDeclarationsSubgrid(string column1, string column2, string column3, string column4, string column5, string column6)
        => AssertTaskGridColumns(SelectedDeclarationsXPath, column1, column2, column3, column4, column5, column6);

    [Then("the following columns are displayed in the Time Recording grid {string} {string} {string} {string} {string} {string}")]
    public void ThenTheFollowingColumnsAreDisplayedInTheTimeRecordingGrid(string column1, string column2, string column3, string column4, string column5, string column6)
        => AssertTaskGridColumns(TimeRecordingXPath, column1, column2, column3, column4, column5, column6);

    [When("I randomly select a declaration from the Declarations subgrid")]
    public void WhenIRandomlySelectADeclarationFromTheDeclarationsSubgrid()
    {
        var rows = ReadPotatoGrid(() =>
        {
            FindTaskGridHeader(DeclarationsXPath, "Declaration Short Name");
            return GetTaskGridRows(GetTaskGrid(DeclarationsXPath)).Select(row => row.GetAttribute("row-id")).ToArray();
        });
        if (rows.Length == 0) throw new NoSuchElementException("No declarations are available to select.");
        var rowId = rows[Random.Shared.Next(rows.Length)];
        scenarioContext["SelectedDeclarationRowId"] = rowId;
        scenarioContext["SelectedDeclarationShortName"] = ReadDeclarationCell(rowId, "Declaration Short Name");
        scenarioContext["SelectedDeclarationDescription"] = ReadDeclarationCell(rowId, "Declaration Description");
        ReadPotatoGrid(() =>
        {
            var row = GetDeclarationRow(rowId);
            if (row.GetAttribute("aria-selected") != "true")
                row.FindElement(By.CssSelector("[role='gridcell'][col-id='__row_status']")).Click();
            return true;
        });
        Driver.WaitForTransaction();
    }

    [When("I record the selected Declaration Description for {string}")]
    public void WhenIRecordTheSelectedDeclarationDescriptionFor(string application)
    {
        var id = scenarioContext.Get<string>("SelectedDeclarationRowId");
        scenarioContext[$"{application}_DeclarationDescription"] = ReadDeclarationCell(id, "Declaration Description");
        scenarioContext[$"{application}_DeclarationShortName"] = ReadDeclarationCell(id, "Declaration Short Name");
    }

    [When("I click the Add Declarations button")]
    public void WhenIClickTheAddDeclarationsButton()
    {
        ReadPotatoGrid(() =>
        {
            Driver.FindElements(AddDeclarationsButton).Last(e => e.Displayed).Click();
            return true;
        });
        Driver.WaitForTransaction();
    }

    [Then("the selected declaration is displayed in the Selected Declarations subgrid")]
    public void ThenTheSelectedDeclarationIsDisplayedInTheSelectedDeclarationsSubgrid()
    {
        var name = scenarioContext.Get<string>("SelectedDeclarationShortName");
        var description = scenarioContext.Get<string>("SelectedDeclarationDescription");
        var found = Policy.Handle<StaleElementReferenceException>().Or<NoSuchElementException>()
            .OrResult<bool>(value => !value).WaitAndRetry(10, _ => TimeSpan.FromSeconds(2)).Execute(() =>
            {
                var nameHeader = FindTaskGridHeader(SelectedDeclarationsXPath, "Declaration Short Name");
                var nameLeft = nameHeader.GetCssValue("left");
                var descriptionHeader = FindTaskGridHeader(SelectedDeclarationsXPath, "Declaration Description");
                var descriptionLeft = descriptionHeader.GetCssValue("left");
                return GetTaskGridRows(GetTaskGrid(SelectedDeclarationsXPath)).Any(row =>
                {
                    var cells = row.FindElements(By.CssSelector("[role='gridcell']"));
                    var nameCell = cells.FirstOrDefault(c => c.GetCssValue("left") == nameLeft);
                    var descriptionCell = cells.FirstOrDefault(c => c.GetCssValue("left") == descriptionLeft);
                    return nameCell != null && descriptionCell != null
                        && ReadTaskGridCell(nameCell) == name && ReadTaskGridCell(descriptionCell) == description;
                });
            });
        found.Should().BeTrue($"selected declaration '{name}' and its full description should appear in the same row");
    }

    [Then("the date is today's date")]
    public void ThenTheDateIsTodaysDate()
    {
        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var expectedName = string.Join(" ", currentUser.Username.Split('@')[0].Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));
        var date = ReadPotatoGrid(() =>
        {
            var inspectorHeader = FindTaskGridHeader(TimeRecordingXPath, "Inspector");
            var inspectorLeft = inspectorHeader.GetCssValue("left");
            var dateHeader = FindTaskGridHeader(TimeRecordingXPath, "Date");
            var row = GetTaskGridRows(GetTaskGrid(TimeRecordingXPath)).FirstOrDefault(r => r
                .FindElements(By.CssSelector("[role='gridcell']"))
                .Any(c => c.GetCssValue("left") == inspectorLeft && ReadTaskGridCell(c).Equals(expectedName, StringComparison.OrdinalIgnoreCase)))
                ?? throw new NoSuchElementException($"Time Recording row for '{expectedName}' was not found.");
            return ReadTaskGridCell(GetTaskGridCell(row, dateHeader));
        });
        DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).Date.Should().Be(DateTime.Today);
    }

    [Then("the Declarations subgrid is filtered by the Country Name {string}")]
    public void ThenTheDeclarationsSubgridIsFilteredByTheCountryName(string country)
    {
        ReadPotatoGrid(() => Driver.ExecuteScript("var root = arguments[0].querySelector('.ag-body-viewport'); if (root) root.scrollTop = 0;", GetTaskGrid(DeclarationsXPath)));
        Thread.Sleep(300);
        var countries = new Dictionary<string, string>();
        var complete = false;
        for (var page = 0; page < 200; page++)
        {
            var result = ReadPotatoGrid(() =>
            {
                var header = FindTaskGridHeader(DeclarationsXPath, "Country Name");
                var grid = GetTaskGrid(DeclarationsXPath);
                foreach (var row in GetTaskGridRows(grid))
                    countries[row.GetAttribute("row-id")] = ReadTaskGridCell(GetTaskGridCell(row, header));
                return (bool)Driver.ExecuteScript(@"
                    var root = arguments[0].querySelector('.ag-body-viewport');
                    if (!root) return false;
                    var end = root.scrollTop + root.clientHeight >= root.scrollHeight - 1;
                    if (!end) root.scrollTop += Math.max(1, root.clientHeight - 42);
                    return end;", grid);
            });
            if (result) { complete = true; break; }
            Thread.Sleep(300);
        }
        complete.Should().BeTrue("all declaration rows should be checked within the scroll limit");
        countries.Should().NotBeEmpty("the country filter should return declarations");
        countries.Values.Should().OnlyContain(value => value == country, $"all declarations should be for '{country}'");
    }

    private static IWebElement GetTaskControl(string dataId) => Driver.FindElements(By.CssSelector($"[data-id='{dataId}']"))
        .LastOrDefault(e => e.Displayed) ?? throw new NoSuchElementException($"Task control '{dataId}' was not displayed.");

    private static bool IsTaskControlReadOnly(IWebElement control)
    {
        if (!control.Enabled || control.GetAttribute("readonly") != null || control.GetAttribute("disabled") != null
            || control.GetAttribute("aria-readonly") == "true" || control.GetAttribute("aria-disabled") == "true") return true;
        // Dynamics read-only lookups remain navigable links and expose their state through accessibility metadata.
        // Nested lookup containers share a data-id; the empty input can be a sibling of the state description.
        var dataId = control.GetAttribute("data-id");
        var lookupContainer = !string.IsNullOrEmpty(dataId) && dataId.Contains(".fieldControl-Lookup_", StringComparison.Ordinal)
            ? control.FindElement(By.XPath($"ancestor-or-self::*[@data-id='{dataId}'][last()]"))
            : control;
        var lookupMetadata = lookupContainer.FindElements(By.CssSelector("[data-id$='_a11y_description'], [data-id$='_selected_tag']"));
        if (lookupMetadata.Any(element =>
        {
            var description = element.GetAttribute("data-id").EndsWith("_a11y_description", StringComparison.Ordinal)
                ? element.GetAttribute("textContent")
                : element.GetAttribute("aria-label");
            return (description ?? string.Empty).Split(',')
                .Any(part => part.Trim().Equals("Readonly", StringComparison.OrdinalIgnoreCase));
        })) return true;

        var inputs = lookupContainer.FindElements(By.CssSelector("input:not([type='hidden']), select, textarea"));
        if (inputs.Count > 0) return inputs.All(e => !e.Enabled || e.GetAttribute("readonly") != null
            || e.GetAttribute("aria-readonly") == "true" || e.GetAttribute("aria-disabled") == "true");

        return control.FindElements(By.XPath("ancestor-or-self::*[@aria-readonly='true' or @aria-disabled='true' or @disabled] ")).Count > 0;
    }

    private static string ReadTaskControlValue(IWebElement control)
    {
        var selected = control.FindElements(By.CssSelector("[data-id$='_selected_tag_text'], [data-id$='_SelectedRecordList']"))
            .FirstOrDefault(e => !string.IsNullOrWhiteSpace(e.Text));
        if (selected != null) return selected.Text.Trim();
        var input = control.FindElements(By.CssSelector("input:not([type='hidden']), textarea, select")).FirstOrDefault();
        if (input != null)
        {
            if (input.TagName == "select") return new OpenQA.Selenium.Support.UI.SelectElement(input).SelectedOption.Text.Trim();
            return (input.GetAttribute("value") ?? string.Empty).Trim();
        }
        return control.Text.Trim();
    }

    private static decimal ParsePotatoQuantity(string value) => decimal.Parse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);

    private static T ReadPotatoGrid<T>(Func<T> action) => Policy.Handle<StaleElementReferenceException>()
        .Or<NoSuchElementException>().Or<ElementClickInterceptedException>()
        .WaitAndRetry(5, _ => TimeSpan.FromSeconds(1)).Execute(() =>
        {
            Driver.WaitForTransaction();
            return action();
        });

    private static IWebElement GetTaskGrid(string xpath)
    {
        for (var attempt = 0; attempt < 40; attempt++)
        {
            Driver.WaitForTransaction();
            var grid = Driver.FindElements(By.XPath(xpath)).LastOrDefault(e => e.Displayed);
            if (grid != null && grid.GetAttribute("aria-busy") != "true"
                && !grid.FindElements(By.CssSelector("[aria-busy='true'], .ag-overlay-loading-center, [role='progressbar']")).Any(e => e.Displayed))
                return grid;
            Thread.Sleep(250);
        }
        throw new NoSuchElementException($"Grid '{xpath}' did not finish loading.");
    }

    private static string ReadTaskGridHeaderText(IWebElement header)
    {
        if (!string.IsNullOrEmpty(header.GetAttribute("col-id")))
            return header.FindElements(By.CssSelector("[data-testid='columnHeader'] label"))
                .FirstOrDefault(e => e.Displayed)?.Text.Trim() ?? string.Empty;

        return (header.GetAttribute("title") ?? header.Text ?? string.Empty).Trim();
    }

    // Both AG Grid and Wijmo virtualise columns. Re-find after each scroll rather than retaining cell references.
    private static IWebElement FindTaskGridHeader(string xpath, string column)
    {
        for (var attempt = 0; attempt < 40; attempt++)
        {
            var grid = GetTaskGrid(xpath);
            var header = grid.FindElements(By.CssSelector("[role='columnheader'], .wj-header[data-id]"))
                .FirstOrDefault(e => e.Displayed
                    && ReadTaskGridHeaderText(e).Equals(column, StringComparison.OrdinalIgnoreCase));
            if (header != null) return header;
            Driver.ExecuteScript(@"
                var container = arguments[0];
                var root = container.querySelector('.ag-body-horizontal-scroll-viewport');
                if (!root) {
                    var grid = container.querySelector('[role=""grid""]');
                    root = grid && (grid.closest('[wj-part=""root""]') || grid.querySelector('[wj-part=""root""]') || grid.parentElement);
                }
                if (root) root.scrollLeft = arguments[1] === 0 ? 0 : root.scrollLeft + 250;", grid, attempt);
            Thread.Sleep(200);
        }
        throw new NoSuchElementException($"Column '{column}' was not rendered in '{xpath}'.");
    }

    private static IReadOnlyCollection<IWebElement> GetTaskGridRows(IWebElement grid) => grid.FindElements(
        By.CssSelector("[role='row'][row-index], [role='row'][aria-label='Data']"));

    private static IWebElement GetTaskGridCell(IWebElement row, IWebElement header)
    {
        var columnId = header.GetAttribute("col-id");
        if (!string.IsNullOrEmpty(columnId)) return row.FindElement(By.CssSelector($"[role='gridcell'][col-id='{columnId}']"));
        var index = header.GetAttribute("aria-colindex");
        if (!string.IsNullOrEmpty(index)) return row.FindElement(By.CssSelector($"[role='gridcell'][aria-colindex='{index}']"));
        var left = header.GetCssValue("left");
        return row.FindElements(By.CssSelector("[role='gridcell']")).FirstOrDefault(c => c.GetCssValue("left") == left)
            ?? throw new NoSuchElementException("No cell was found under the grid header.");
    }

    private static string ReadTaskGridCell(IWebElement cell)
    {
        var select = cell.FindElements(By.TagName("select")).FirstOrDefault();
        if (select != null) return new OpenQA.Selenium.Support.UI.SelectElement(select).SelectedOption.Text.Trim();
        var input = cell.FindElements(By.CssSelector("input:not([type='checkbox']):not([type='hidden']), textarea")).FirstOrDefault();
        if (input != null) return (input.GetAttribute("value") ?? string.Empty).Trim();
        var title = cell.GetAttribute("title");
        var label = cell.FindElements(By.CssSelector("label[aria-label], a[aria-label]")).FirstOrDefault();
        return (string.IsNullOrWhiteSpace(title) ? label?.GetAttribute("aria-label") ?? cell.Text : title).Trim();
    }

    private static void AssertTaskGridColumns(string xpath, params string[] columns)
    {
        foreach (var column in columns)
            ReadPotatoGrid(() => FindTaskGridHeader(xpath, column).Displayed).Should().BeTrue($"'{column}' should be displayed");
    }

    private static string InspectionColumnName(string column) => column == "Quantity Exported" ? "Quantity Exported (Consignment Item)" : column;

    private static IWebElement GetPotatoInspectionCell(string column)
    {
        var header = FindTaskGridHeader(InspectionResultsContainerXPath, InspectionColumnName(column));
        var row = GetTaskGridRows(GetTaskGrid(InspectionResultsContainerXPath)).FirstOrDefault()
            ?? throw new NoSuchElementException("Inspection Results has no data row.");
        return GetTaskGridCell(row, header);
    }

    private static string ReadInspectionValue(string column) => ReadPotatoGrid(() => ReadTaskGridCell(GetPotatoInspectionCell(column)));

    private void EnterInspectionQuantity(string column)
    {
        var quantity = ReadInspectionValue("Quantity Applied For");
        ReadPotatoGrid(() =>
        {
            GetPotatoInspectionCell(column).Click();
            Driver.WaitForTransaction();
            var input = GetPotatoInspectionCell(column).FindElements(By.CssSelector("input:not([type='hidden'])"))
                .FirstOrDefault(e => e.Displayed && e.Enabled)
                ?? throw new NoSuchElementException($"Numeric editor for '{column}' was not found.");
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Backspace);
            input.SendKeys(quantity);
            input.SendKeys(Keys.Tab);
            return true;
        });
        XrmApp.Entity.Save();
        Driver.WaitForTransaction();
    }

    private static void AssertInspectionQuantities(string actualColumn, string expectedColumn)
    {
        var expected = ParsePotatoQuantity(ReadInspectionValue(expectedColumn));
        var actual = Policy.Handle<StaleElementReferenceException>().Or<NoSuchElementException>()
            .OrResult<string>(value => !decimal.TryParse(value, System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture, out var number) || Math.Round(number, 2) != Math.Round(expected, 2))
            .WaitAndRetry(10, _ => TimeSpan.FromSeconds(2)).Execute(() => ReadInspectionValue(actualColumn));
        Math.Round(ParsePotatoQuantity(actual), 2).Should().Be(Math.Round(expected, 2), $"'{actualColumn}' should match '{expectedColumn}'");
    }

    private static IWebElement GetDeclarationRow(string rowId) => GetTaskGrid(DeclarationsXPath)
        .FindElement(By.CssSelector($"[role='row'][row-id='{rowId}']"));

    private static string ReadDeclarationCell(string rowId, string column) => ReadPotatoGrid(() =>
    {
        var header = FindTaskGridHeader(DeclarationsXPath, column);
        return ReadTaskGridCell(GetTaskGridCell(GetDeclarationRow(rowId), header));
    });

    [Then(@"the Status of the Work Order should be '(.*)'")]
    public void ThenTheStatusOfTheWorkOrderShouldBe(string expectedStatus)
    {
        Driver.WaitForTransaction();

        string actualStatus = null;

        Policy
            .Handle<Exception>()
            .OrResult<string>(value => value != expectedStatus)
            .WaitAndRetry(6, _ => TimeSpan.FromSeconds(5))
            .Execute(() =>
            {
                Driver.WaitForTransaction();
                var statusControl = Driver.WaitUntilAvailable(
                    By.XPath("//*[@data-id='QV_Work_Order_Sub_Status.msdyn_substatus.fieldControl-Lookup_msdyn_substatus']"),
                    "Work Order Status field could not be found on the Work Order Task page.");

                var value = statusControl.Text;
                if (string.IsNullOrWhiteSpace(value))
                {
                    var titled = statusControl.FindElements(By.XPath(".//*[@title]"));
                    value = titled.Count > 0 ? titled[0].GetAttribute("title") : statusControl.GetAttribute("title");
                }

                return actualStatus = value?.Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim();
            });

        actualStatus.Should().Be(expectedStatus,
            $"Expected Status to be '{expectedStatus}' but found '{actualStatus}'.");
    }
}