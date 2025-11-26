// <copyright file="VisitSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.Specs.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using Reqnroll;

/// <summary>
/// Time Recording reusable steps.
/// </summary>
[Binding]
public class VisitSteps : PowerAppsStepDefiner
{
    private SessionContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="VisitSteps"/> class.
    /// </summary>
    /// <param name="context">SessionContext.</param>
    public VisitSteps(SessionContext context)
    {
        this.ctx = context;
    }

    /// <summary>
    /// Enters submitted time for all work order tasks.
    /// </summary>
    [Given(@"I create a Visit for the Inspection Task")]
    [When(@"I create a Visit for the Inspection Task")]
    public void IcreateaVisitfortheInspectionTask()
    {
        if (XrmApp.Entity.GetEntityName() != "msdyn_workorderservicetask")
        {
            throw new Exception($"You must first navigate to the work order service task");
        }

        var id = XrmApp.Entity.GetObjectId();

        // TODO: auth as logged in user
        using (var context = new PlantsContext(SessionContext.GetServiceClient()))
        {
            EntityReference location;
            using (var svc = SessionContext.GetServiceClient())
            {
                location = svc.WaitForFieldValue<EntityReference>("msdyn_workorderservicetask", id, "trd_inspectionaddressid", TimeSpan.FromSeconds(30));
            }

            var futureDataTime = RandomHelper.GetNextAvailableVisitDate(context);
            var task = context.msdyn_workorderservicetaskSet.Where(x => x.Id == id).Select(st => new msdyn_workorderservicetask { Id = st.Id, trd_InspectionAddressId = st.trd_InspectionAddressId }).FirstOrDefault();
            var visit = new trd_visit { trd_name = "Visit Place", trd_DateScheduled = futureDataTime, trd_InspectionAddress = task.trd_InspectionAddressId };

            context.AddObject(visit);
            context.SaveChanges();

            var serviceTaskToUpdate = new msdyn_workorderservicetask { Id = id, trd_VisitId = visit.ToEntityReference() };

            if (!context.IsAttached(serviceTaskToUpdate))
            {
                context.Attach(serviceTaskToUpdate);
            }

            context.UpdateObject(serviceTaskToUpdate);
            context.SaveChanges();
        }
    }
}