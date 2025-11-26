// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.SpecFlowBindings.Tables;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[Binding]
public class ImportNotificationSteps : PowerAppsStepDefiner
{
    private readonly SessionContext context;
    private readonly ScenarioContext scenarioContext;

    private static string EntityName
    {
        get
        {
            var entityName = string.Empty;
            Policy
                .Handle<WebDriverException>()
                .WaitAndRetry(30, retryAttempt => TimeSpan.FromSeconds(2))
                .Execute(() =>
                {
                    entityName = XrmApp.Entity.GetEntityName();
                });
            return entityName;
        }
    }

    public ImportNotificationSteps(SessionContext context)
    {
        this.context = context;
    }

    public ImportNotificationSteps(SessionContext context, ScenarioContext scenarioContext)
    {
        this.context = context;
        this.scenarioContext = scenarioContext;
    }

    [When(@"an importer amend '(.*)' to inspection address '(.*)' of '(.*)'")]
    public void WhenAnImporterAmendToInspectionAddressOf(string applicationAlias, string address, string bcpName)
    {
        var importNotification = this.context.GetEntityReference(applicationAlias);
        this.SetImportNotificationToAmend(importNotification);
        var inspectionAddress = this.context.GetEntityReference(address);
        this.UpdateInspectionAddress(bcpName, inspectionAddress, importNotification);
    }

    private void UpdateInspectionAddress(string bcpName, EntityReference inspectionAddress, EntityReference importNotification)
    {
        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var importNotificationEntity = plantsContext.trd_plantsimportnotificationSet
                .Where(st => st.Id == importNotification.Id)
                .Select(st => new trd_plantsimportnotification
                {
                    Id = st.Id,
                    trd_BorderControlPostId = st.trd_BorderControlPostId,
                    trd_InspectionLocationId = st.trd_InspectionLocationId,
                    trd_Version = st.trd_Version,
                    statuscode = st.statuscode,
                }).Single();

            var bcp = plantsContext.trd_bordercontrolpostSet.Single(p => p.trd_name == bcpName);

            importNotificationEntity.statuscode = trd_plantsimportnotification_statuscode.Amended;
            importNotificationEntity.trd_Version = "2";
            importNotificationEntity.trd_BorderControlPostId = bcp.ToEntityReference();
            importNotificationEntity.trd_InspectionLocationId = inspectionAddress;
            plantsContext.UpdateObject(importNotificationEntity);
            plantsContext.SaveChanges();
        }
    }

    private void SetImportNotificationToAmend(EntityReference importNotification)
    {
        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var notification = plantsContext.trd_plantsimportnotificationSet.Where(st => st.Id == importNotification.Id)
                .Select(st => new trd_plantsimportnotification
                {
                    Id = st.Id,
                    statuscode = st.statuscode,
                }).Single();


            notification.statuscode = trd_plantsimportnotification_statuscode.Amending;
            plantsContext.UpdateObject(notification);
            plantsContext.SaveChanges();
        }
    }

    [When(@"'(.*)' is completed for '(.*)' with the following details")]
    public void WhenIsCompletedForWithFollowingDetails(string taskName, string applicationAlias, Table table)
    {
        var applicationSteps = new ApplicationSteps(this.context, this.scenarioContext);
        var workOrder = applicationSteps.WaitTillWorkOrderHasBeenCreated(applicationAlias);
        ApplicationSteps.OpenEntity(workOrder);
        EntitySteps.ISelectTab("Work Order Tasks");
        OpenWorkOrderServiceTask(taskName);
        new WorkOrderTasksSteps(this.context).AssignServiceTaskToSelf();

        foreach (var tableRow in table.Rows)
        {
            if (table.ContainsColumn("No. of Phytos Inspected"))
            {
                PopupSteps.WhenIEnterIntoTheTextFieldOnThePopupBox(tableRow["No. of Phytos Inspected"], "trd_noofphytosinspected", "numeric");
            }
        }

        table = this.CreateDefaultTimeEntryTable(taskName);
        new ImportSteps(this.context).AddTimeEntryAndSubmit(table, taskName);
        PopupSteps.MarkCompleteServiceTasks();
        PopupSteps.CloseServiceTask();
    }

    [When(@"an inactive '(.*)' is completed for '(.*)'")]
    public void WhenAnInactiveIsCompletedFor(string taskName, string applicationAlias)
    {
        this.OpenWorkOrder(applicationAlias);

        var subGridName = "workorderservicetasksgrid";
        GridHelper.WaitForRows(
            Driver,
            subGridName,
            (rowCount) =>
            {
                var serviceTasksList = GetWorkOrderServiceTaskDetails().ToList();
                var task = serviceTasksList.SingleOrDefault(p => p.StatusReason == "Inactive" && taskName == p.TaskType);
                this.CompleteTasks(taskName, applicationAlias, false, false);
            },
            $"Subgrid {subGridName} contains no rows.");
    }

    private void OpenWorkOrder(string applicationAlias)
    {
        var applicationSteps = new ApplicationSteps(this.context, this.scenarioContext);
        var workOrder = applicationSteps.WaitTillWorkOrderHasBeenCreated(applicationAlias);
        ApplicationSteps.OpenEntity(workOrder);
        EntitySteps.ISelectTab("Work Order Tasks");
    }

    [When(@"'(.*)' is completed for '(.*)'")]
    public void WhenIsCompletedFor(string taskName, string applicationAlias)
    {
        this.CompleteTasks(taskName, applicationAlias);
    }

    private void CompleteTasks(string taskName, string applicationAlias, bool active = true, bool openWorkOrder = true)
    {
        if (openWorkOrder)
        {
            this.OpenWorkOrder(applicationAlias);
        }

        var serviceTasksList = GetWorkOrderServiceTaskDetails().ToList();
        var index = serviceTasksList.FindIndex(p => p.TaskType == taskName);
        Capgemini.PowerApps.SpecFlowBindings.Steps.GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(index);
        new WorkOrderTasksSteps(this.context).AssignServiceTaskToSelf();
        if (!active)
        {
            PopupSteps.WhenIActivateTheRecordInThePopupBox("msdyn_workorderservicetask");
            DialogSteps.WhenIClickTheButtonOnTheDialog("Activate");
            Driver.WaitForTransaction();
        }

        var table = this.CreateDefaultTimeEntryTable(taskName);
        new ImportSteps(this.context).AddTimeEntryAndSubmit(table, taskName);
        PopupSteps.MarkCompleteServiceTasks();
        PopupSteps.CloseServiceTask();
    }

    private Table CreateDefaultTimeEntryTable(string taskName)
    {
        switch (taskName.ToLower())
        {
            case "document check":
                var table = new Table("Inspector", "Admin");
                table.AddRow("a primary inspector", Faker.RandomNumber.Next(1, 500).ToString());
                return table;
            case "identity & physical check":
                var idAndPhysicalTable = new Table("Inspector", "Travel", "Inspection", "Admin");
                idAndPhysicalTable.AddRow("a primary inspector", Faker.RandomNumber.Next(1, 500).ToString(), Faker.RandomNumber.Next(1, 500).ToString(), Faker.RandomNumber.Next(1, 500).ToString());
                return idAndPhysicalTable;
            default:
                throw new NotImplementedException("TODO");
        }
    }

    [Then(@"I can see the following charges are created")]
    public void ThenICanSeeTheFollowingChargesAreCreated(Table table)
    {
        SessionContext.GetServiceClient().WaitForRecords(
            new QueryByAttribute(trd_charge.EntityLogicalName)
            {
                Attributes = { "trd_workorderid" },
                Values =
                {
                    this.context.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias).Id,
                },
            }, TimeSpan.FromSeconds(90));

        new WorkOrderSteps(this.context).OpenWorkOrder();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        var gridRecords = XrmApp.Grid.GetGridItems();

        var expectedList = table.CreateSet<ChargeTable>();
        var actualList = new List<ChargeTable>();

        foreach (var gridRecord in gridRecords)
        {
            var attributes = (Dictionary<string, object>)(typeof(GridItem).GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(gridRecord));
            var serviceTask = new ChargeTable
            {
                ProductOrService = GetValue(attributes, "trd_productorserviceid"),
                PriceList = GetValue(attributes, "trd_pricelistid"),
                Quantity = GetValue<decimal>(attributes, "trd_qty"),
                Unit = GetValue(attributes, "trd_unitid"),
                UnitChargeAmount = GetValue<decimal>(attributes, "trd_unitchargeamount"),
                WorkOrderTask = GetValue(attributes, "trd_workordertaskid"),
                ChargeExempt = GetValue(attributes, "trd_chargeexempt"),
            };
            actualList.Add(serviceTask);
        }

        actualList.Should().BeEquivalentTo(expectedList);
    }

    [Given(@"'(.*)' has been created")]
    public void GivenHasBeenCreated(string dataFileName)
    {
        if (!dataFileName.Contains("import"))
            throw new NotImplementedException("Need to extend to address other applications");

        var table = new Table("FileName");
        table.AddRow("an aliased contact");
        table.AddRow("an aliased account");
        table.AddRow("an aliased exporter");
        table.AddRow(dataFileName);
        new DataSteps(this.context).GivenAUserHasCreatedSomethingICannot(table);
    }

    [When(@"I set willinspect as (.*) for '(.*)'")]
    public void WhenISetWillinspectAsYesFor(string willInspectValue, string commodityAlias)
    {
        new DataSteps(this.context).GivenIHaveOpened(commodityAlias);
        EntitySteps.WhenIEnterInTheField(willInspectValue, "trd_willinspect", "optionset", "field");
        EntitySteps.WhenISaveTheRecord();
    }

    [When(@"I open related work order for '(.*)'")]
    public void WhenIOpenRelatedWorkOrderFor(string recordAlias)
    {
        new DataSteps(this.context).GivenIHaveOpened(recordAlias);
        LookupSteps.WhenISelectARelatedLookupInTheForm("trd_workorderid");
    }

    [When(@"Commodity '(.*)' is updated as '(.*)' and (Inspection Required|No Inspection Required)")]
    public void WhenCommodityIsUpdatedAsAndInspectionRequired(string variableName, string authority, string inspectionRequiredString)
    {
        var table = new Table("Field", "Value");

        var phsiInspectionRequired = false;
        var hmiInspectionRequired = false;

        if (inspectionRequiredString.ToLower() == "inspection required")
        {
            switch (authority)
            {
                case "PHSI":
                    phsiInspectionRequired = true;
                    break;
                case "JOINT":
                    phsiInspectionRequired = true;
                    hmiInspectionRequired = true;
                    break;
                default:
                    hmiInspectionRequired = true;
                    break;
            }
        }

        table.AddRow("trd_phsiinspectionrequired", phsiInspectionRequired.ToString());
        table.AddRow("trd_hmiinspectionrequired", hmiInspectionRequired.ToString());
        table.AddRow("trd_regulatoryauthoritycode", authority == "PHSI" ? "0" : authority == "HMI" ? "1" : "2");
        new UpdateDataSteps(this.context).WhenHasUpdateWithTheFollowingValues(variableName, table);
    }

    [When(@"'(.*)' has been amending and I can see notification as '(.*)' for (.*) service tasks")]
    public void WhenAnImportNotificationHasBeenAmendingAndICanSeeNotificationAsInServiceTasks(string alias, string message, int serviceTaskCount)
    {
        this.UpdateNotificationStatusAndAssertNotificationMessage(alias, message, serviceTaskCount, "434800007");
    }

    [When(@"'(.*)' has been amended and I can see notification as '(.*)' for (.*) service tasks")]
    public void WhenAnImportNotificationHasBeenAmendedAndICanSeeNotificationAsForServiceTasks(string alias, string message, int serviceTaskCount)
    {
        this.UpdateNotificationStatusAndAssertNotificationMessage(alias, message, serviceTaskCount, "434800008");
    }

    private void UpdateNotificationStatusAndAssertNotificationMessage(string alias, string message, int serviceTaskCount, string status)
    {
        this.UpdateImportStatusAndAssert(status);
        Navigate("WorkOrder");
        for (var i = 0; i < serviceTaskCount; i++)
        {
            OpenGridRecord(i);

            Policy
                .Handle<Exception>()
                .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(2))
                .Execute(() =>
                {
                    var notifications = XrmApp.Entity.GetFormNotifications();
                    notifications.Any(p => p.Message == message).Should().BeTrue($"{message} not found");
                });

            ClosePopup();
        }
    }

    private static void ClosePopup()
    {
        PopupSteps.WhenICloseThePopup();
        SharedSteps.WaitForScriptProcessing();
    }

    [When(@"an import notification has been amended")]
    public void WhenAnImportNotificationHasBeenAmended()
    {
        this.UpdateImportStatusAndAssert("434800007");
        this.UpdateImportStatusAndAssert("434800008");
    }

    [Then(@"no workorder service tasks are present in '(.*)'")]
    public void ThenNoWorkorderServiceTasksArePresentIn(string alias)
    {
        Navigate("WorkOrder");
        var actualWorkOrderServiceTasks = GetWorkOrderServiceTaskDetails();
        actualWorkOrderServiceTasks.Should().BeEquivalentTo(new List<WorkOrderServiceTaskTable>());
    }

    [Then(@"I can see no charges are created")]
    public void ThenICanSeeNoChargesAreCreated()
    {
        new WorkOrderSteps(this.context).OpenWorkOrder();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        var gridRecords = XrmApp.Grid.GetGridItems();
        gridRecords.Count.Should().Be(0, "Expected charges to be 0");
    }

    public static void Navigate(string formName)
    {
        var entityName = EntityName;
        switch (formName)
        {
            case "Commodity":
                NavigateToCommodityForm(entityName);
                return;
            case "Import":
                NavigateToImport(entityName);
                return;
            case "WorkOrder":
                NavigateToWorkOrderForm(entityName);
                return;
            default:
                throw new InvalidOperationException($"Unexpected argument {formName}");
        }
    }

    private static void NavigateToImport(string entityName)
    {
        switch (entityName)
        {
            case "trd_plantsimportnotification":
                return;
            case "msdyn_workorderservicetask":
                PopupSteps.WhenICloseThePopup();
                break;
        }

        CommandSteps.WhenIgoBack();
        IsItImportNotificationFrom().Should().BeTrue();
    }

    private static void NavigateToWorkOrderForm(string entityName)
    {
        switch (entityName)
        {
            case "msdyn_workorder":
                return;
            case "msdyn_workorderservicetask":
                PopupSteps.WhenICloseThePopup();
                return;
            case "trd_plantsimportcommodityline":
                CommandSteps.WhenIgoBack();
                break;
        }
        EntitySteps.ISelectTab("Summary");
        LookupSteps.WhenISelectARelatedLookupInTheForm("trd_workorderid");
        EntitySteps.ISelectTab("Work Order Tasks");
        IsItWorkOrder().Should().BeTrue();
    }

    private static void NavigateToCommodityForm(string entityName)
    {
        switch (entityName)
        {
            case "trd_plantsimportcommodityline":
            case "msdyn_workorder":
                CommandSteps.WhenIgoBack();
                break;
            case "trd_plantsimportnotification":
                break;
        }

        EntitySteps.ISelectTab("Commodity Lines");
    }

    private static bool IsItImportNotificationFrom()
    {
        return EntityName == "trd_plantsimportnotification";
    }

    private static bool IsItWorkOrder()
    {
        return EntityName == "msdyn_workorder";
    }

    public static void OpenWorkOrderServiceTask(string taskName)
    {
        var serviceTasksList = GetWorkOrderServiceTaskDetails().ToList();
        var index = serviceTasksList.FindIndex(p => p.TaskType == taskName);
        XrmApp.Entity.SubGrid.OpenSubGridRecord("workorderservicetasksgrid", index);
        Driver.WaitForTransaction();
    }

    public static IEnumerable<WorkOrderServiceTaskTable> GetWorkOrderServiceTaskDetails()
    {
        return XrmApp.Entity.SubGrid
            .GetSubGridItems("workorderservicetasksgrid")
            .Select(item => new WorkOrderServiceTaskTable
            {
                TaskType = (string)item["msdyn_name"],
                StatusReason = (string)item["statecode"],
                Complete = (string)item["msdyn_percentcomplete"],
            })
            .ToList();
    }

    private void UpdateImportStatusAndAssert(string statusCode)
    {
        var versionNumber = "#{RandomNumber[10]}#".TokeniseText();
        var table = new Table("Field", "Value");
        table.AddRow("trd_version", versionNumber);
        table.AddRow("statuscode", statusCode);
        new UpdateDataSteps(this.context).WhenHasUpdateWithTheFollowingValues("an import notification", table);
        Navigate("Import");
        CommandSteps.WhenISelectTheCommand("Refresh");
        FormSteps.VerifyValue(statusCode, "statuscode");
    }

    private static void OpenGridRecord(int positionValue)
    {
        Capgemini.PowerApps.SpecFlowBindings.Steps.GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(positionValue);
        SharedSteps.WaitForScriptProcessing();
    }

    private static string GetValue(Dictionary<string, object> attributes, string attributeName)
    {
        return GetValue<string>(attributes, attributeName);
    }

    private static T GetValue<T>(Dictionary<string, object> attributes, string attributeName)
    {
        var taskNameKey = attributes.Keys.SingleOrDefault(p => p.EndsWith(attributeName));
        if (taskNameKey == null)
            return default;
        return (T)Convert.ChangeType(attributes[taskNameKey], typeof(T));
    }
}