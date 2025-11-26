// <copyright file="ApplicationSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using System.Linq;
using Reqnroll;

/// <summary>
/// Binding for steps related to application processing.
/// </summary>
[Binding]
public class ApplicationSteps : PowerAppsStepDefiner
{
    private readonly SessionContext context;
    private readonly ScenarioContext scenarioContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationSteps"/> class.
    /// </summary>
    /// <param name="context">Context of the test session.</param>
    /// <param name="scenarioContext">Context of the test scenario.</param>
    public ApplicationSteps(SessionContext context, ScenarioContext scenarioContext)
    {
        this.context = context;
        this.scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Given step for accepting an application from the application record through the UI.
    /// </summary>
    [Given(@"I accept the current application")]
    public static void GivenIAcceptTheCurrentApplication()
    {
        CommandSteps.WhenISelectTheCommand("Accept");
        SharedSteps.WaitForScriptProcessing();
        Capgemini.PowerApps.SpecFlowBindings.Steps.DialogSteps.WhenIConfirmWhenPresentedWithTheConfirmationDialog("confirm");
        SharedSteps.WaitForScriptProcessing();
    }

    /// <summary>
    /// Opens a record at a given position in the work order service tasks grid.
    /// </summary>
    /// <param name="positionString">The position of the record in the subgrid.</param>
    /// <param name="workOrderTaskName">The name of the work order service task type.</param>
    [Given(@"I open a (\d[a-z]+) Task '(.*)' for current open application")]
    [When(@"I open a (\d[a-z]+) Task '(.*)' for current open application")]
    public static void GivenIOpenATaskForCurrentOpenApplication(string positionString, string workOrderTaskName)
    {
        EntitySteps.ISelectTab("Work Order Tasks");
        ImportNotificationSteps.OpenWorkOrderServiceTask(workOrderTaskName);

        Driver.WaitUntilAvailable(By.XPath("//div[contains(@aria-modal, 'true') and contains (@role, 'dialog')]"), (element) =>
        {
            var modalForms = Driver.FindElements(By.XPath("//div[contains(@aria-modal, 'true') and contains (@role, 'dialog')]"));
            var field = ControlHelper.GetControlField(modalForms.Last(), "msdyn_tasktype", "lookup", "field", Driver);
            field.Value.Should().Be(workOrderTaskName);

        }, "No modal form found");
    }

    /// <summary>
    /// Opens a given application alias and assigns to the current user through the UI. This will only work for Export Applications.
    /// </summary>
    /// <param name="applicationName">The alias of the application to assign.</param>
    [Given(@"I open a '(.*)' and assign it to myself")]
    public void WhenIOpenAAndAssignItToMySelf(string applicationName)
    {
        new DataSteps(this.context).GivenIHaveOpened(applicationName);
        LookupSteps.WhenISelectARelatedLookupInTheForm("trd_workorderid");
        CommandSteps.WhenISelectTheCommand("Assign");
        SharedSteps.WaitForScriptProcessing();
        Capgemini.PowerApps.SpecFlowBindings.Steps.DialogSteps.WhenIAssignToMeOnTheAssignDialog();
        SharedSteps.WaitForScriptProcessing();
        CommandSteps.WhenISelectTheCommand("Save");
        SharedSteps.WaitForScriptProcessing();
        EntitySteps.ThenICanSeeAValueOfInTheLookupField(
            "Export Application",
            new LookupItem() { Name = "msdyn_workordertype" },
            "field");
    }

    /// <summary>
    /// Opens the work order associated to an export application asynchronously.
    /// </summary>
    /// <param name="applicationAlias">The alias of the export application.</param>
    [Given(@"I have opened the work order associated to the '(.*)'")]
    [When(@"I have opened the work order associated to the '(.*)'")]
    [Then(@"I have opened the work order associated to the '(.*)'")]
    public void GivenIHaveOpenedTheWorkOrderAssociatedToThe(string applicationAlias)
    {
        var workOrder = WaitTillWorkOrderHasBeenCreated(applicationAlias);
        OpenEntity(workOrder);
    }

    public static void OpenEntity(EntityReference workOrder)
    {
        Driver.ExecuteScript($"Xrm.Navigation.openForm({{ entityId: \"{workOrder.Id}\", entityName: \"{workOrder.LogicalName}\" }})");
        Driver.WaitForTransaction();
    }

    public EntityReference WaitTillWorkOrderHasBeenCreated(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        EntityReference workOrder;
        using (var svc = SessionContext.GetServiceClient())
        {
            workOrder = WorkOrderSteps.GetWorkOrder(svc, application);
        }

        this.AddApplicationToEntityHolder(applicationAlias, application);
        new WorkOrderSteps(this.context).AddWorkOrderToEntityHolder(workOrder);
        return workOrder;
    }

    private void AddApplicationToEntityHolder(string applicationAlias, EntityReference applicationEntityReference)
    {
        if (!this.context.Entities.ContainsKey(applicationAlias + this.context.SessionId))
        {
            this.context.Entities.Add($"{applicationAlias}{this.context.SessionId}",
                new EntityHolder
                {
                    Alias = applicationAlias,
                    EntityName = applicationEntityReference.LogicalName,
                    EntityCollectionName = applicationEntityReference.LogicalName + "s",
                    Id = applicationEntityReference.Id,
                });
        }
    }

    [When(@"the system has assigned yesterday date to application submitted date")]
    public void WhenTheSystemHasAssignedYesterdayDateToApplicationSubmittedDate()
    {
        FormSteps.SetDateTime("trd_datesubmitted", "yesterday", "midnight");
        Driver.WaitForTransaction();
    }

}
