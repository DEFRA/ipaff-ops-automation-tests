// <copyright file="InspectionResultSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using Reqnroll;

/// <summary>
/// Steps relating the processing of inspection results.
/// </summary>
[Binding]
public class InspectionResultSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;
    private readonly ScenarioContext scenarioContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="InspectionResultSteps"/> class.
    /// </summary>
    /// <param name="sessionContext">SessionContext.</param>
    /// <param name="scenarioContext">ScenarioContext.</param>
    public InspectionResultSteps(SessionContext sessionContext, ScenarioContext scenarioContext)
    {
        this.sessionContext = sessionContext;
        this.scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Opens an inspection result record for a related application.
    /// </summary>
    /// <param name="applicationAlias">The application related to the inspection result.</param>
    [Given("I have opened an inspection result for '(.*)'")]
    public void GivenIOpenInspectionResult(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = WorkOrderSteps.GetWorkOrder(svc, application);
            var inspectionresult = svc.WaitForRecords(
            new QueryByAttribute(trd_inspectionresult.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Attributes = { "trd_workorder" },
                Values = { workOrder.Id },
            },
            SpecflowBindingsConstants.DefaultWaitTime,
            SpecflowBindingsConstants.DefaultRetryInterval,
            false).Entities.Select(x => x.ToEntity<trd_inspectionresult>()).ToList().FirstOrDefault();
            context.ClearChanges();
            XrmApp.Entity.OpenEntity(trd_inspectionresult.EntityLogicalName, inspectionresult.Id);
            SharedSteps.WaitForScriptProcessing();
        }
    }
}
