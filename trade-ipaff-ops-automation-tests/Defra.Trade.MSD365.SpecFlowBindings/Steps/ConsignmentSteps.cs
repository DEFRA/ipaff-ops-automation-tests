// <copyright file="ConsignmentSteps.cs" company="DEFRA">
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
/// Bindings for steps related to consignments and consignment items.
/// </summary>
[Binding]
public class ConsignmentSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;
    private readonly ScenarioContext scenarioContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsignmentSteps"/> class.
    /// </summary>
    /// <param name="sessionContext">SessionContext.</param>
    /// <param name="scenarioContext">ScenarioContext.</param>
    public ConsignmentSteps(SessionContext sessionContext, ScenarioContext scenarioContext)
    {
        this.sessionContext = sessionContext;
        this.scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Opens a commodity line record for a related application.
    /// </summary>
    /// <param name="applicationAlias">the alias of the application related to the commodity line record to open.</param>
    [Given("I have opened a commodity line for '(.*)'")]
    public void GivenIOpenCommodityLine(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = svc.WaitForFieldValue<EntityReference>(application.LogicalName, application.Id, "trd_workorderid", TimeSpan.FromSeconds(30));
            var consignment = svc.WaitForRecords(
            new QueryByAttribute(trd_consignmentitem.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Attributes = { "trd_workorderid" },
                Values = { workOrder.Id },
            },
            SpecflowBindingsConstants.DefaultWaitTime,
            SpecflowBindingsConstants.DefaultRetryInterval,
            false).Entities.Select(x => x.ToEntity<trd_consignmentitem>()).ToList().FirstOrDefault();
            context.ClearChanges();
            XrmApp.Entity.OpenEntity(trd_consignmentitem.EntityLogicalName, consignment.Id);
            SharedSteps.WaitForScriptProcessing();
        }
    }
}
