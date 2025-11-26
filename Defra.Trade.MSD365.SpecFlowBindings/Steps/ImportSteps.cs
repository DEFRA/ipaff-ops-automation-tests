namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.SpecFlowBindings.Tables;
using Defra.Trade.Plants.Specs.Steps;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

[Binding]
public class ImportSteps : PowerAppsStepDefiner
{
    private readonly SessionContext context;

    public ImportSteps(SessionContext context)
    {
        this.context = context;
    }

    [When(@"I enter (\d+(?:(?:st)|(?:nd)|(?:rd)|(?:th))) time recording for '(.*)' as follows and submit")]
    public void WhenIEnterTimeRecordAsFollowsAndSubmit(int index, string serviceTask, Table table)
    {
        var gridName = this.GetGridName(serviceTask);
        new TimeRecordingSteps(this.context).SubmitTimeEntry(gridName, index, true, table);
    }

    [When(@"I enter (.*) time recordings as follows")]
    public void WhenIEnterTimeRecordingAsFollows(string serviceTaskType, Table table)
    {
        var serviceTaskIndex = this.GetLineOrderForServiceTask(serviceTaskType);
        var gridName = this.GetGridName(serviceTaskType);
        this.OpenServiceTaskByIndex(serviceTaskIndex);
        var newRecordIndex = Helpers.GridHelper.GetRows(Driver, gridName).Count;
        new TimeRecordingSteps(this.context).SubmitTimeEntry(gridName, newRecordIndex, false, table);
    }

    [When(@"I submit (.*) time recordings as follows")]
    public void WhenISubmitTimeRecordingAsFollows(string serviceTaskType, Table table)
    {
        this.OpenServiceTaskByType(serviceTaskType);
        this.AddTimeEntryAndSubmit(table, serviceTaskType);
    }

    [Then(@"the following time recordings have been displayed on the current (.*)")]
    public void ThenTheFollowingTimeRecordingsHaveBeenDisplayedForOnTheScreen(string serviceTaskType, Table table)
    {
        this.AssertGridTimeRecord(serviceTaskType, table);
    }

    [Then(@"the following time recordings have been displayed for (.*)")]
    public void ThenTheFollowingTimeRecordingsHaveBeenDisplayedFor(string serviceTaskType, Table table)
    {
        var serviceTaskIndex = this.GetLineOrderForServiceTask(serviceTaskType);
        this.OpenServiceTaskByIndex(serviceTaskIndex);
        this.AssertGridTimeRecord(serviceTaskType, table);
    }

    private void AssertGridTimeRecord(string serviceTaskType, Table table)
    {
        var gridName = this.GetGridName(serviceTaskType);
        PopupSteps.WhenIMaximiseThePopup();
        var rowIndex = 0;
        foreach (var row in table.Rows)
        {
            for (var i = 0; i < table.Header.Count; i++)
            {
                var columnHeader = table.Header.ToList()[i];

                if (row[i].ToLower() == "today")
                {
                    row[columnHeader] = DateTime.UtcNow.Date.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);
                }

                if (columnHeader == "Inspector")
                {
                    row[columnHeader] = new TimeRecordingSteps(this.context).GetInspectorName(row[columnHeader]);
                }

                GridSteps.ThenTheCellForRowInTheGridHasAValueOf(columnHeader, rowIndex, gridName, row[i]);
            }

            rowIndex++;
        }
    }

    [Given(@"'(.*)' has been created, assigned to myself and which has following tasks")]
    public void GivenHasBeenCreatedAssignedToMyselfAndWhichHasFollowingTasks(string dataFileName, Table table)
    {
        LoginSteps.GivenIAmLoggedInToTheAppAs("Defra Trade", "a user");
        this.CreateImportApplicationAndWaitForTasks(dataFileName);
        List<WorkOrderServiceTaskTable> expectedList = table.CreateSet<WorkOrderServiceTaskTable>().ToList();
        expectedList.ForEach(p => p.StatusReason = "Active");
        var serviceTasks = this.GetServiceTaskList(dataFileName);
        foreach (var expectedServiceTask in expectedList)
        {
            serviceTasks.Single(p => p.TaskType == expectedServiceTask.TaskType).Should().BeEquivalentTo(expectedServiceTask);
        }
    }

    [Given(@"An import application has (at least|more than) one commodity which requires Physical Inspection")]
    public void GivenAnImportApplicationHasAtleastOneCommodityWhichRequiresPhysicalInspection(string numberOfPHSICommodities)
    {
        LoginSteps.GivenIAmLoggedInToTheAppAs("Defra Trade", "a user");

        var dataFileName = "an import notification";

        if (numberOfPHSICommodities == "more than")
        {
            var table = new Table("Record", "trd_inspectionrequired", "trd_regulatoryauthoritycode");
            table.AddRow("1", "true", "0");
            table.AddRow("2", "true", "1");
            table.AddRow("3", "true", "2");
            new DataSteps(this.context).GivenHasTheFollowingSetupFor(dataFileName, "trd_plantsimportnotification_trd_plantsimportcommodityline", table);
        }

        this.CreateImportApplicationAndWaitForTasks(dataFileName);
    }

    [When(@"I inspect the commodity and create a lab sample for all commodities")]
    public void WhenIInspectTheCommodityAndCreateALabSampleForTheAllCommodities()
    {
        this.WhenSelectGridCommandForTheFirstCommodity("Create Lab Samples", -1);
    }

    [When(@"I open (\d+(?:(?:st)|(?:nd)|(?:rd)|(?:th))) lab sample")]
    public void WhenIOpenStLabSample(int index)
    {
        GridHelper.WaitForRows(
           Driver,
           "subgrid_lab_samples",
           (rowCount) =>
           {
               XrmApp.Entity.SubGrid.OpenSubGridRecord("subgrid_lab_samples", index);
           },
           $"SubGrid 'subgrid_lab_samples' contains no rows.");

        if (Driver.HasElement(By.Id("modalDialogContentContainer")))
        {
            XrmApp.Dialogs.ConfirmationDialog(true);
        }

        Driver.WaitForTransaction();
        Driver.WaitForPageToLoad();
    }

    [When(@"I select '(.*)' for the (\d+(?:(?:st)|(?:nd)|(?:rd)|(?:th))) commodity")]
    public void WhenSelectGridCommandForTheFirstCommodity(string gridCommand, int index)
    {
        var subGridName = "subgrid_import_commodity_lines";
        var dataSteps = new DataSteps(this.context);
        var flyoutCommandName = "Samples";

        dataSteps.GivenIHaveOpened(SpecFlowBindings.SpecflowBindingsConstants.WorkOrderAlias);
        ApplicationSteps.GivenIOpenATaskForCurrentOpenApplication("3rd", "Identity & Physical Check");
        Driver.WaitForTransaction();
        PopupSteps.OpenPopupTab("samples");

        GridHelper.WaitForRows(
            Driver,
            subGridName,
            (rowCount) =>
            {
                if (index == -1)
                {
                    GridSteps.WhenISelectAllRowsInTheGrid(subGridName);
                }
                else
                {
                    GridSteps.WhenISelectRowInTheGrid(index, subGridName);
                }

                var subGrid = GridHelper.GetGrid(Driver, subGridName);
                CommandHelper.ClickCommand(Driver, subGrid, flyoutCommandName, gridCommand);
            },
            $"SubGrid {subGridName} contains no rows.");
    }

    [Then(@"(.*) lab samples? created for an import workorder")]
    public void WhenLabSampleCreatedForAnImportWorkorder(int numberOfLabSamples)
    {
        EntitySubGridSteps.WhenISelectTheCommandOnTheSubgrid("Refresh", "subgrid_lab_samples");
        XrmApp.Entity.SubGrid.GetSubGridItemsCount("subgrid_lab_samples").Should().Be(numberOfLabSamples);
        var items = XrmApp.Entity.SubGrid.GetSubGridItems("subgrid_lab_samples");
        XrmApp.Entity.OpenEntity("trd_labsample", items.First().Id);
    }

    [When(@"I open '(.*)' service task")]
    public void WhenIOpenServiceTask(string serviceTaskName)
    {
        this.OpenServiceTaskByType(serviceTaskName);
    }

    [When(@"I open '(.*)' service task and assign to myself")]
    public void WhenIOpenServiceTaskAndAssignToSelf(string serviceTaskName)
    {
        this.OpenServiceTaskByType(serviceTaskName);
        new WorkOrderTasksSteps(this.context).AssignServiceTaskToSelf();
    }

    [Given(@"I have opened '(.*)' and which has (.*) service task")]
    public void GivenIHaveOpenedAndWhichHasServiceTask(string alias, int expectedNumberOfServiceTasks)
    {
        new AsyncSteps(this.context).WhenIWaitForTheWorkOrderToBeCreatedWithTasks(expectedNumberOfServiceTasks, alias);
        var dataSteps = new DataSteps(this.context);
        dataSteps.GivenIHaveOpened(alias);
        LookupSteps.WhenISelectARelatedLookupInTheForm("trd_workorderid");
        EntitySteps.ISelectTab("Work Order Tasks");
    }

    [Given(@"'(.*)' has the following commodities")]
    public void GivenHasTheFollowingCommodities(string commodityName, Table table)
    {
        foreach (var tableRow in table.Rows)
        {
            this.GivenIGetCommidityIdAndSaveItAs(tableRow["CommodityName"], commodityName, tableRow["VariableName"]);
        }
    }

    [Given(@"I get (.*) CommodityId in '(.*)' and save it as '(.*)'")]
    [When(@"I get (.*) CommodityId in '(.*)' and save it as '(.*)'")]
    public void GivenIGetCommidityIdAndSaveItAs(string commodityName, string alias, string variableName)
    {
        var id = this.context.Entities[$"{alias}{this.context.SessionId}"].Id;
        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var commodity = plantsContext.trd_plantsimportcommoditylineSet.Single(p => p.trd_CommodityName == commodityName && p.trd_importnotificationid.Id == id);
            this.context.Entities.Add($"{variableName}{this.context.SessionId}", new EntityHolder
            {
                Alias = variableName,
                EntityName = commodity.LogicalName,
                EntityCollectionName = commodity.LogicalName + "s",
                Id = commodity.Id,
            });
        }
    }

    [Given(@"Work order and (.*) service tasks have been created for '(.*)'")]
    public void GivenWorkOrderAndServiceTasksHaveBeenCreatedFor(int expectedNumberOfTasks, string applicationAlias)
    {
        new ApplicationSteps(this.context, null).WaitTillWorkOrderHasBeenCreated(applicationAlias);
        new AsyncSteps(this.context).WhenIWaitForTheWorkOrderToBeCreatedWithTasks(expectedNumberOfTasks, applicationAlias);
    }

    [Then(@"the following workorder service tasks are present in '(.*)'")]
    public void ThenTheFollowingWorkorderServiceTasksArePresentIn(string alias, Table table)
    {
        new AsyncSteps(this.context).WhenIWaitForTheWorkOrderToBeCreatedWithTasks(table.RowCount, alias);
        ImportNotificationSteps.Navigate("WorkOrder");
        this.AssertWorkOrderServiceTasks(table, "workorderservicetasksgrid", alias);
    }

    [When(@"the risk engine outcome as follows")]
    public void WhenTheRiskEngineOutcomeAsFollows(Table table)
    {
        var riskEngineOutcomeList = table.CreateSet<RisEngineOutComeTable>().ToList();

        foreach (var riskOutcome in riskEngineOutcomeList)
        {
            var commodityId = TestDriver.GetTestRecordReference(riskOutcome.Commodity);
            using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
            {
                var commodity = plantsContext.trd_plantsimportcommoditylineSet.Where(st => st.Id == commodityId.Id)
                    .Select(st => new trd_plantsimportcommodityline
                    {
                        Id = st.Id,
                        trd_InspectionClassification = st.trd_InspectionClassification,
                        trd_InspectionRequired = st.trd_InspectionRequired,
                    }).Single();

                commodity.trd_InspectionClassification = riskOutcome.InspectionClassification;
                commodity.trd_InspectionRequired = riskOutcome.InspectionRequired.ToLower() == "yes";
                plantsContext.UpdateObject(commodity);
                plantsContext.SaveChanges();
            }
        }
    }

    [When(@"IPAFFS updates '(.*)' as follows")]
    public void WhenIPAFFSUpdatesAsFollows(string importAlias, Table table)
    {
        var riskEngineOutcomeList = table.CreateSet<ImportNotificationTable>().ToList();
        var application = this.context.GetEntityReference(importAlias);

        foreach (var riskOutcome in riskEngineOutcomeList)
        {
            using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
            {
                var commodity = plantsContext.trd_plantsimportnotificationSet.Where(st => st.Id == application.Id)
                    .Select(st => new trd_plantsimportnotification
                    {
                        Id = st.Id,
                        trd_IpaffsStatus = st.trd_IpaffsStatus,
                        trd_Version = st.trd_Version,
                        statecode = st.statecode,
                        statuscode = st.statuscode,
                    }).Single();

                if (!string.IsNullOrEmpty(riskOutcome.Version))
                {
                    commodity.trd_Version = riskOutcome.Version;
                }

                if (riskOutcome.NotificationStatus.HasValue)
                {
                    commodity.trd_IpaffsStatus = riskOutcome.NotificationStatus;
                }

                commodity.statecode = riskOutcome.Status;
                commodity.statuscode = riskOutcome.StatusReason;
                plantsContext.UpdateObject(commodity);
                plantsContext.SaveChanges();
            }
        }
    }

    public void AddTimeEntryAndSubmit(Table table, string serviceTaskType)
    {
        var gridName = this.GetGridName(serviceTaskType);
        var newRecordIndex = Helpers.GridHelper.GetRows(Driver, gridName).Count;
        new TimeRecordingSteps(this.context).SubmitTimeEntry(gridName, newRecordIndex, true, table);
    }

    public string GetGridName(string serviceTaskType)
    {
        switch (serviceTaskType.ToLower())
        {
            case "document check":
            case "imports phyto certificate audit":
                return "timerecordings_admin_subgrid";
            case "identity & physical check":
            case "hmi check":
                return "timerecordings_subgrid";
            default:
                throw new NotImplementedException("Unexpected service type");
        }
    }

    private void AssertWorkOrderServiceTasks(Table table, string gridName, string alias)
    {
        var expectedList = table.CreateSet<WorkOrderServiceTaskTable>();
        this.WaitTillServiceTasksBeingCreated(gridName, alias, expectedList);
        var actualWorkOrderServiceTasks = ImportNotificationSteps.GetWorkOrderServiceTaskDetails();

        if (table.ContainsColumn("Complete"))
        {
            actualWorkOrderServiceTasks.Should().BeEquivalentTo(expectedList);
        }
        else
        {
            actualWorkOrderServiceTasks.Should().BeEquivalentTo(expectedList, options => options.Excluding(x => x.Complete));
        }
    }

    public List<WorkOrderServiceTaskTable> GetServiceTaskList(string alias)
    {
        var entityReference = this.context.GetEntityReference(alias);
        return entityReference.LogicalName == trd_plantsimportnotification.EntityLogicalName ? this.GetImportServiceTaskList(entityReference) : GetExportServiceTaskList(entityReference);
    }

    private List<WorkOrderServiceTaskTable> GetExportServiceTaskList(EntityReference entityReference)
    {
        // TODO: auth as logged in user
        using (var plantsContext =
            new PlantsContext(
                SessionContext.GetServiceClient()))
        {
            var serviceTaskList = plantsContext.msdyn_workorderservicetaskSet
                .Join(
                    plantsContext.msdyn_workorderSet,
                    serviceTask => serviceTask.msdyn_WorkOrder.Id,
                    workOrder => workOrder.Id,
                    (serviceTask, workOrder) => new { serviceTask, workOrder })
                .Where(item =>
                    item.workOrder.trd_exportapplicationid.Id == entityReference.Id)
                .Select(item => item.serviceTask).ToList();
            var actualList = serviceTaskList.Select(serviceTask => new WorkOrderServiceTaskTable
            {
                StatusReason = serviceTask.statuscode.ToString(),
                TaskType = serviceTask.msdyn_TaskType.Name,
            }).ToList();
            return actualList;
        }
    }

    public List<WorkOrderServiceTaskTable> GetImportServiceTaskList(EntityReference entityReference)
    {
        var serviceTaskList = this.GetServiceTask(entityReference);

        var actualList = serviceTaskList.Select(serviceTask => new WorkOrderServiceTaskTable
        {
            StatusReason = serviceTask.statuscode.ToString(),
            TaskType = serviceTask.msdyn_TaskType.Name,
        }).ToList();

        return actualList;
    }

    public IEnumerable<msdyn_workorderservicetask> GetServiceTask(EntityReference entityReference)
    {
        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var serviceTaskList = plantsContext.msdyn_workorderservicetaskSet
                .Join(plantsContext.msdyn_workorderSet, serviceTask => serviceTask.msdyn_WorkOrder.Id, workOrder => workOrder.Id, (serviceTask, workOrder) => new { serviceTask, workOrder })
                .Where(item => item.workOrder.trd_plantsimportnotification.Id == entityReference.Id)
                .Select(item => item.serviceTask)
                .ToList();

            return serviceTaskList;
        }
    }

    private void WaitTillServiceTasksBeingCreated(string gridName, string alias, IEnumerable<WorkOrderServiceTaskTable> expectedList)
    {
        var entityReference = this.context.GetEntityReference(alias);

        if (entityReference.LogicalName == trd_plantsimportnotification.EntityLogicalName)
        {
            this.WaitForImportServiceTasks(gridName, expectedList, entityReference);
        }
        else
        {
            this.WaitForExportServiceTasks(gridName, expectedList, entityReference);
        }
    }

    private void WaitForExportServiceTasks(string gridName, IEnumerable<WorkOrderServiceTaskTable> expectedList, EntityReference entityReference)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(SpecFlowBindings.SpecflowBindingsConstants.ApiRetryAttempts,
                retryAttempt => SpecFlowBindings.SpecflowBindingsConstants.ApiSleepDuration)
            .Execute(() =>
            {
                // TODO: auth as logged in user
                using (var plantsContext =
                    new PlantsContext(SessionContext.GetServiceClient()))
                {
                    var serviceTaskList = plantsContext.msdyn_workorderservicetaskSet
                        .Join(
                            plantsContext.msdyn_workorderSet,
                            serviceTask => serviceTask.msdyn_WorkOrder.Id,
                            workOrder => workOrder.Id,
                            (serviceTask, workOrder) => new { serviceTask, workOrder })
                        .Where(item => item.workOrder.trd_exportapplicationid.Id == entityReference.Id)
                        .Select(item => item.serviceTask).ToList();
                    var actualList = serviceTaskList.Select(serviceTask => new WorkOrderServiceTaskTable
                    { StatusReason = serviceTask.statuscode.ToString(), TaskType = serviceTask.msdyn_TaskType.Name })
                        .ToList();
                    actualList.Should().BeEquivalentTo(expectedList);
                    EntitySubGridSteps.WhenISelectTheCommandOnTheSubgrid("Refresh", gridName);
                    XrmApp.Grid.GetGridItems().Count.Should().Be(expectedList.Count());
                }
            });
    }

    private void WaitForImportServiceTasks(string gridName, IEnumerable<WorkOrderServiceTaskTable> expectedList, EntityReference entityReference)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(SpecflowBindingsConstants.ApiRetryAttempts, retryAttempt => SpecflowBindingsConstants.ApiSleepDuration)
            .Execute(() =>
            {
                // TODO: auth as logged in user
                using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
                {
                    var serviceTaskList = plantsContext.msdyn_workorderservicetaskSet
                        .Join(
                            plantsContext.msdyn_workorderSet,
                            serviceTask => serviceTask.msdyn_WorkOrder.Id,
                            workOrder => workOrder.Id,
                            (serviceTask, workOrder) => new { serviceTask, workOrder })
                        .Where(item => item.workOrder.trd_plantsimportnotification.Id == entityReference.Id)
                        .Select(item => item.serviceTask).ToList();
                    var actualList = serviceTaskList.Select(serviceTask => new WorkOrderServiceTaskTable
                    { StatusReason = serviceTask.statuscode.ToString(), TaskType = serviceTask.msdyn_TaskType.Name })
                        .ToList();
                    actualList.Should().BeEquivalentTo(expectedList, options => options.Excluding(p => p.Complete));
                    EntitySubGridSteps.WhenISelectTheCommandOnTheSubgrid("Refresh", gridName);
                    XrmApp.Grid.GetGridItems().Count.Should().Be(expectedList.Count());
                }
            });
    }

    private int GetLineOrderForServiceTask(string serviceTaskType)
    {
        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var id = this.context.GetEntity(SpecflowBindingsConstants.WorkOrderAlias).Id;
            var tasks = plantsContext.msdyn_workorderservicetaskSet.Where(x => x.msdyn_WorkOrder.Id == id).ToList();
            return tasks.Single(p => p.msdyn_name == serviceTaskType).msdyn_LineOrder.Value - 1;
        }
    }

    private void CreateImportApplicationAndWaitForTasks(string dataFileName)
    {
        new ImportNotificationSteps(this.context).GivenHasBeenCreated(dataFileName);
        var asynSteps = new AsyncSteps(this.context);
        asynSteps.WhenIWaitForTheWorkOrderToBeCreatedWithTasks(SpecflowBindingsConstants.ImportNotificationTaskCount, dataFileName); asynSteps.WhenIWaitForAllServiceTasksToBeOwned(dataFileName, SpecflowBindingsConstants.DefaultTaskCountOwnedByCITImportsTeam);
        asynSteps.AssignWorkorderToBaseRoleUser();
        asynSteps.WhenIWaitForAllServiceTasksToBeOwned(dataFileName, SpecflowBindingsConstants.DefaultTaskCountOwnedByCITImportsTeam);
    }

    private void OpenServiceTaskByIndex(int serviceTaskIndex)
    {
        var dataSteps = new DataSteps(this.context);
        dataSteps.GivenIHaveOpened(SpecFlowBindings.SpecflowBindingsConstants.WorkOrderAlias);
        WorkOrderTasksSteps.WhenIOpenTheRecord("Work Order Tasks", serviceTaskIndex);
    }

    private void OpenServiceTaskByType(string serviceTaskType)
    {
        var serviceTaskIndex = this.GetLineOrderForServiceTask(serviceTaskType);
        this.OpenServiceTaskByIndex(serviceTaskIndex);
    }
}