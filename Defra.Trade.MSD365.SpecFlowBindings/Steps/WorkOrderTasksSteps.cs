// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.BusinessLogic.ReferenceData;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using OpenQA.Selenium;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using Reqnroll;

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

        var gridContainer = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        // Task names are rendered as <a role="link" aria-label="<task name>"> inside the msdyn_name column cells.
        // The aria-label attribute is stable — it is set from the record data, not CSS-in-JS hashing.
        foreach (var taskName in expectedTaskNames)
        {
            var taskLink = gridContainer.FindElements(
                By.XPath($".//div[@col-id='msdyn_name']//a[@role='link' and @aria-label='{taskName}']"));

            taskLink.Count.Should().Be(1,
                $"Expected to find exactly one Work Order Task named '{taskName}' in the grid but found {taskLink.Count}.");
        }
    }

    /// <summary>
    /// Clicks on a Work Order Task link by name in the workorderservicetasksgrid.
    /// </summary>
    /// <param name="taskName">The name of the task to click.</param>
    [When(@"I click on the '(.*)' task")]
    public void WhenIClickOnTheTask(string taskName)
    {
        Driver.WaitForTransaction();

        var gridContainer = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        var taskLink = Driver.WaitUntilAvailable(
            By.XPath($".//div[@col-id='msdyn_name']//a[@role='link' and @aria-label='{taskName}']"),
            $"Work Order Task '{taskName}' could not be found in the grid.");

        taskLink.Click();
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

        var actualTitle = popupTitle.Text;            

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

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='timerecordings_admin_subgrid_container']//div[@role='grid'][contains(@aria-label,'Time Recording')]"),
            "Time Recording grid could not be found.");

        // Inspector is at aria-colindex='3' in the WijMo grid as confirmed in the HTML.
        var inspectorCells = grid.FindElements(
            By.XPath(".//div[@role='row'][@aria-label='Data']//div[@role='gridcell'][@aria-colindex='3']//span[@role='presentation']"));

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

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='timerecordings_admin_subgrid_container']//div[@role='grid'][contains(@aria-label,'Time Recording')]"),
            "Time Recording grid could not be found.");

        var dataRows = grid.FindElements(
            By.XPath(".//div[@role='row'][@aria-label='Data']"));

        dataRows.Should().NotBeEmpty("Expected at least one row in the Time Recording grid.");

        // Find the row where aria-colindex='3' (Inspector) matches the current user,
        // then assert aria-colindex='2' (Entry Status) on that same row.
        IWebElement matchingRow = null;

        foreach (var row in dataRows)
        {
            var inspectorCells = row.FindElements(
                By.XPath(".//div[@role='gridcell'][@aria-colindex='3']//span[@role='presentation']"));

            if (inspectorCells.Count > 0 &&
                inspectorCells[0].Text.Trim().Equals(expectedName, StringComparison.OrdinalIgnoreCase))
            {
                matchingRow = row;
                break;
            }
        }

        matchingRow.Should().NotBeNull(
            $"Could not find a Time Recording row for Inspector '{expectedName}' when verifying entry status.");

        var entryStatusCell = matchingRow.FindElement(
            By.XPath(".//div[@role='gridcell'][@aria-colindex='2']//span[@role='presentation']"));

        entryStatusCell.Text.Trim().Should().Be(expectedEntryStatus,
            $"Expected Entry Status to be '{expectedEntryStatus}' for Inspector '{expectedName}' " +
            $"but found '{entryStatusCell.Text.Trim()}'.");
    }

    /// <summary>
    /// Enters a value into the Admin column cell for the current user's row in the Time Recording subgrid
    /// and stores it in the scenario context for later assertion.
    /// </summary>
    /// <param name="value">The value to enter e.g. '20'.</param>
    [When(@"I enter '(.*)' in the Admin column")]
    public void WhenIEnterInTheAdminColumn(string value)
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='timerecordings_admin_subgrid_container']//div[@role='grid'][contains(@aria-label,'Time Recording')]"),
            "Time Recording grid could not be found.");

        var dataRows = grid.FindElements(By.XPath(".//div[@role='row'][@aria-label='Data']"));

        dataRows.Should().NotBeEmpty("Expected at least one row in the Time Recording grid.");

        IWebElement matchingRow = null;

        foreach (var row in dataRows)
        {
            var inspectorCells = row.FindElements(
                By.XPath(".//div[@role='gridcell'][@aria-colindex='3']//span[@role='presentation']"));

            if (inspectorCells.Count > 0 &&
                inspectorCells[0].Text.Trim().Equals(expectedName, StringComparison.OrdinalIgnoreCase))
            {
                matchingRow = row;
                break;
            }
        }

        matchingRow.Should().NotBeNull(
            $"Could not find a Time Recording row for Inspector '{expectedName}' to enter Admin value.");

        var adminCell = matchingRow.FindElement(
            By.XPath(".//div[@role='gridcell'][@aria-colindex='5']"));

        adminCell.Click();
        Driver.WaitForTransaction();

        var numericInput = adminCell.FindElement(By.XPath(".//input[@wj-part='input' and contains(@class,'wj-numeric')]"));

        numericInput.SendKeys(Keys.Control + "a");
        numericInput.SendKeys(Keys.Delete);
        numericInput.SendKeys(value);
        Driver.WaitForTransaction();

        scenarioContext["AdminTimeValue"] = value;
    }

    /// <summary>
    /// Verifies that the Admin time value for the current user's row has been saved correctly
    /// in the Time Recording subgrid, displayed as '{value} minutes'.
    /// </summary>
    [Then(@"the details are saved")]
    public void ThenTheDetailsAreSaved()
    {
        Driver.WaitForTransaction();

        scenarioContext.TryGetValue("AdminTimeValue", out string enteredValue);
        enteredValue.Should().NotBeNullOrEmpty(
            "AdminTimeValue was not found in the scenario context — ensure 'When I enter ... in the Admin column' ran before this step.");

        var expectedAdminDisplay = $"{enteredValue} minutes";

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='timerecordings_admin_subgrid_container']//div[@role='grid'][contains(@aria-label,'Time Recording')]"),
            "Time Recording grid could not be found.");

        var dataRows = grid.FindElements(By.XPath(".//div[@role='row'][@aria-label='Data']"));

        dataRows.Should().NotBeEmpty("Expected at least one row in the Time Recording grid after saving.");

        IWebElement matchingRow = null;

        foreach (var row in dataRows)
        {
            var inspectorCells = row.FindElements(
                By.XPath(".//div[@role='gridcell'][@aria-colindex='3']//span[@role='presentation']"));

            if (inspectorCells.Count > 0 &&
                inspectorCells[0].Text.Trim().Equals(expectedName, StringComparison.OrdinalIgnoreCase))
            {
                matchingRow = row;
                break;
            }
        }

        matchingRow.Should().NotBeNull(
            $"Could not find the Time Recording row for Inspector '{expectedName}' when verifying save.");

        // After saving, the WijMo cell reverts to read-only display showing '{value} minutes'
        // e.g. entering '20' saves as '20 minutes' as confirmed in the post-save HTML.
        var adminCell = matchingRow.FindElement(
            By.XPath(".//div[@role='gridcell'][@aria-colindex='5']//span[@role='presentation']"));

        adminCell.Text.Trim().Should().Be(expectedAdminDisplay,
            $"Expected Admin value for Inspector '{expectedName}' to display as '{expectedAdminDisplay}' " +
            $"but found '{adminCell.Text.Trim()}'.");
    }

    /// <summary>
    /// Clicks the Save icon in the Time Recording subgrid header to persist inline edits.
    /// </summary>
    [When(@"I click the Save icon")]
    public void WhenIClickTheSaveIcon()
    {
        Driver.WaitForTransaction();

        // The Save button is in the WijMo grid column header area with a stable title="Save" attribute.
        var saveButton = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='timerecordings_admin_subgrid_container']//button[@title='Save' and contains(@class,'cc-ds-header-save-btn')]"),
            "Save button could not be found in the Time Recording subgrid.");

        saveButton.Click();
        Driver.WaitForTransaction();
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

            DataCollection<Entity> queryResults = null;
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