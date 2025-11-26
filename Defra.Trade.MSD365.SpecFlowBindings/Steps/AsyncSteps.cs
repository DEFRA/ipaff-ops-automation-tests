namespace Defra.Trade.Plants.Specs.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.SpecFlowBindings.Steps;
using FluentAssertions;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Reqnroll;

/// <summary>
/// Time Recording reusable steps.
/// </summary>
[Binding]
public class AsyncSteps : PowerAppsStepDefiner
{
    private readonly SessionContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncSteps"/> class.
    /// </summary>
    /// <param name="context">SessionContext.</param>
    public AsyncSteps(SessionContext context)
    {
        this.ctx = context;
    }

    /// <summary>
    /// Waits for Work Order Creation with Child Tasks.
    /// </summary>
    /// <param name="expectedCount">expected task count.</param>
    /// <param name="applicationAlias">Application alias.</param>
    [Given(@"I wait for the system to generate a work order and (.*) tasks in the background for '(.*)'")]
    [When(@"I wait for the system to generate a work order and (.*) tasks in the background for '(.*)")]
    public void WhenIWaitForTheWorkOrderToBeCreatedWithTasks(int expectedCount, string applicationAlias)
    {
        var entityReference = this.ctx.GetEntityReference(applicationAlias);
        switch (entityReference.LogicalName)
        {
            case trd_exportapplication.EntityLogicalName:
                this.AssertExportServiceTasksCountMatch(entityReference, expectedCount);
                break;
            case trd_plantsimportnotification.EntityLogicalName:
                this.AssertImportServiceTasksCount(entityReference.Id, expectedCount);
                break;
            default:
                throw new Exception($"Unexpected entity");
        }
    }

    /// <summary>
    /// Assert Import service count.
    /// </summary>
    /// <param name="importId">Import Id.</param>
    /// <param name="expectedCount">Expected service task count.</param>
    private void AssertImportServiceTasksCount(Guid importId, int expectedCount)
    {
        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            plantsContext.MergeOption = MergeOption.NoTracking;
            Policy
            .Handle<Exception>()
            .WaitAndRetry(SpecflowBindingsConstants.ApiRetryAttempts, retryAttempt => SpecflowBindingsConstants.ApiSleepDuration)
            .Execute(() =>
            {
                    var workOrderEntity = plantsContext.msdyn_workorderSet.Single(p => p.trd_plantsimportnotification.Id == importId);
                    new WorkOrderSteps(this.ctx).AddWorkOrderToEntityHolder(workOrderEntity.ToEntityReference());

                    var serviceTaskCount = plantsContext.msdyn_workorderservicetaskSet
                        .Join(
                            plantsContext.msdyn_workorderSet,
                            serviceTask => serviceTask.msdyn_WorkOrder.Id,
                            workOrder => workOrder.Id,
                            (serviceTask, workOrder) => new { serviceTask, workOrder })
                        .Where(item => item.workOrder.trd_plantsimportnotification.Id == importId)
                        .Select(item => item.serviceTask).ToList()
                        .Count;
                    serviceTaskCount.Should().Be(expectedCount, $"Expected service tasks count for Work Order {workOrderEntity.msdyn_name} ({workOrderEntity.Id}) should be {expectedCount}");
            });
        }
    }

    /// <summary>
    /// Assign the workorder to base user.
    /// </summary>
    public void AssignWorkorderToBaseRoleUser()
    {
        var testConfig = TestConfig.GetUser(SpecflowBindingsConstants.BaseUser);

        // TODO: auth as logged in user
        var adminServiceClient = SessionContext.GetServiceClient();

        using (var plantsContext = new PlantsContext(adminServiceClient))
        {
            var workOrkder = plantsContext.msdyn_workorderSet.SingleOrDefault(p => p.Id == this.ctx.GetEntity(SpecflowBindingsConstants.WorkOrderAlias).Id);

            AssignRequest assign = new AssignRequest
            {
                Assignee = new EntityReference(SystemUser.EntityLogicalName, plantsContext.SystemUserSet.Where(p => p.DomainName == testConfig.Username).Single().Id),
                Target = workOrkder.ToEntityReference(),
            };
            plantsContext.Execute(assign);
        }
    }

    /// <summary>
    /// Assert all Service tasks are assigned to the Work Order owner.
    /// </summary>
    /// <param name="applicationAlias">Application alias name.</param>
    [Given(@"I wait for the Work Order assignment to be cascaded to the tasks in the background for '(.*)'")]
    [When(@"I wait for the Work Order assignment to be cascaded to the tasks in the background for '(.*)'")]
    public void WhenIWaitForAllServiceTasksToBeOwned(string applicationAlias)
    {
        this.WhenIWaitForAllServiceTasksToBeOwned(applicationAlias, 0);
    }

    /// <summary>
    /// Wait for all service tasks to be owned.
    /// </summary>
    /// <param name="applicationAlias">applicaiton alias.</param>
    /// <param name="count">count.</param>
    public void WhenIWaitForAllServiceTasksToBeOwned(string applicationAlias, int count)
    {
        var entity = this.ctx.GetEntityReference(applicationAlias);
        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            plantsContext.MergeOption = MergeOption.NoTracking;
            Policy
            .Handle<Exception>()
            .WaitAndRetry(SpecflowBindingsConstants.ApiRetryAttempts, retryAttempt => SpecflowBindingsConstants.ApiSleepDuration)
            .Execute(() =>
            {
                var workOrder = plantsContext.msdyn_workorderSet.Single(p => p.trd_plantsimportnotification.Id == entity.Id || p.trd_exportapplicationid.Id == entity.Id);
                var unownedServiceTasks = (from t in plantsContext.msdyn_workorderservicetaskSet where t.msdyn_WorkOrder.Id == workOrder.Id && t.OwnerId != workOrder.OwnerId select t.OwnerId).ToList();
                unownedServiceTasks.Count.Should().Be(count, $"Expected unowned service tasks count should be 0");
            });
        }
    }

    /// <summary>
    /// Asserts the work order service tasks for the work order in the session context have been deactivated.
    /// </summary>
    [Given("all service tasks for the work order related to the export application '(.*)' have been deactivated")]
    public void AssertServiceTasksAreInactive(string applicationAlias)
    {
        var exportApplication = this.ctx.GetEntityReference(applicationAlias);

        if (exportApplication == null)
        {
            throw new Exception("No work order found from the test context.");
        }

        string exceptionMessage = "Not all service tasks are deactivated.";

        using (var context = new PlantsContext(SessionContext.GetServiceClient()))
        {
            context.MergeOption = MergeOption.NoTracking;
            Policy
            .Handle<Exception>(e => e.Message == exceptionMessage)
            .WaitAndRetry(SpecflowBindingsConstants.ApiRetryAttempts, retryAttempt => SpecflowBindingsConstants.ApiSleepDuration.Add(TimeSpan.FromSeconds(retryAttempt)))
            .Execute(() =>
            {
                    List<msdyn_workorderservicetask> serviceTasks = context.msdyn_workorderservicetaskSet.Join(
                            context.msdyn_workorderSet,
                            serviceTask => serviceTask.msdyn_WorkOrder.Id,
                            workOrder => workOrder.Id,
                            (serviceTask, workOrder) => new { serviceTask, workOrder })
                        .Where(item => item.workOrder.trd_exportapplicationid == exportApplication)
                        .Select(item => item.serviceTask).ToList();
                    int allServiceTasksCount = serviceTasks.Count;
                    int inactiveServiceTasksCount = serviceTasks
                        .Where(task => task.statecode == msdyn_workorderservicetaskState.Inactive && task.statuscode == msdyn_workorderservicetask_statuscode.Inactive)
                        .Count();

                    if (allServiceTasksCount != inactiveServiceTasksCount)
                    {
                        throw new Exception(exceptionMessage);
                    }

                    return true;
            });
        }
    }

    /// <summary>
    /// Asserts if the specified work order service tasks for the work order in the session context has been deactivated.
    /// </summary>
    [Then(@"the '(.*)' for the workorder related to the export application '(.*)' has been deactivated")]
    public void ThenTheForTheWorkorderRelatedToTheExportApplicationHasBeenDeactivated(string taskType, string applicationAlias)
    {
        var exportApplication = this.ctx.GetEntityReference(applicationAlias);

        if (exportApplication == null)
        {
            throw new Exception("No work order found from the test context.");
        }

        string exceptionMessage = taskType + " is not deactivated.";
        var serviceTaskCount = 0;

        using (var context = new PlantsContext(SessionContext.GetServiceClient()))
        {
            context.MergeOption = MergeOption.NoTracking;
            var workOrderTaskType = context.msdyn_servicetasktypeSet.Single(p => p.msdyn_name == taskType);
            Policy
                .Handle<Exception>(e => e.Message == exceptionMessage)
                .WaitAndRetry(SpecflowBindingsConstants.ApiRetryAttempts, retryAttempt => SpecflowBindingsConstants.ApiSleepDuration.Add(TimeSpan.FromSeconds(retryAttempt)))
                .Execute(() =>
                {
                    serviceTaskCount = context.msdyn_workorderservicetaskSet.Join(
                           context.msdyn_workorderSet,
                           serviceTask => serviceTask.msdyn_WorkOrder.Id,
                           workOrder => workOrder.Id,
                           (serviceTask, workOrder) => new { serviceTask, workOrder })
                       .Where(item => item.workOrder.trd_exportapplicationid == exportApplication && item.serviceTask.msdyn_TaskType.Id == workOrderTaskType.Id && item.serviceTask.statuscode == msdyn_workorderservicetask_statuscode.Inactive && item.serviceTask.statecode == msdyn_workorderservicetaskState.Inactive)
                       .Select(item => item.serviceTask).ToList().Count;
                    if (serviceTaskCount == 0 || serviceTaskCount > 1)
                    {
                        throw new Exception(exceptionMessage);
                    }

                    return true;
                });
        }
    }

    private void AssertExportServiceTasksCountMatch(EntityReference exportEntityReference, int expectedCount)
    {
        var workOrderId = WorkOrderSteps.GetWorkOrder(SessionContext.GetServiceClient(), exportEntityReference);
        new WorkOrderSteps(this.ctx).AddWorkOrderToEntityHolder(workOrderId);
        var serviceTaskCount = 0;

        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            Policy
                .HandleResult<int>(c => c != expectedCount)
                .WaitAndRetry(SpecflowBindingsConstants.DefaultRetryCount, retryAttempt => TimeSpan.FromSeconds(5))
                .Execute(() =>
                {
                    serviceTaskCount = plantsContext.msdyn_workorderservicetaskSet.Join(
                            plantsContext.msdyn_workorderSet,
                            serviceTask => serviceTask.msdyn_WorkOrder.Id,
                            workOrder => workOrder.Id,
                            (serviceTask, workOrder) => new { serviceTask, workOrder })
                        .Where(item => item.workOrder.trd_exportapplicationid.Id == exportEntityReference.Id)
                        .Select(item => item.serviceTask).ToList().Count;

                    return serviceTaskCount;
                });

            if (serviceTaskCount != expectedCount)
            {
                serviceTaskCount.Should().Be(expectedCount, $"Expected service tasks count should be {expectedCount} for the work order {workOrderId.Id} ");
            }
        }
    }
}