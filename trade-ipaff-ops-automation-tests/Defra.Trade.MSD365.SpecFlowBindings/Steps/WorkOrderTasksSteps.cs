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
    private readonly ScenarioContext _scenarioContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkOrderTasksSteps"/> class.
    /// </summary>
    /// <param name="ctx">SessionContext.</param>
    public WorkOrderTasksSteps(SessionContext ctx)
    {
        this.ctx = ctx;
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