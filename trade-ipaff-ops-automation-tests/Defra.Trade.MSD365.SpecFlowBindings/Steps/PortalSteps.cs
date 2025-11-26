namespace Defra.Trade.Plants.Specs.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Microsoft.Xrm.Sdk.Messages;
using Polly;
using System;
using System.Linq;
using Reqnroll;

/// <summary>
/// Portal API Steps.
/// </summary>
[Binding]
public class PortalSteps : PowerAppsStepDefiner
{
    private readonly SessionContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PortalSteps"/> class.
    /// </summary>
    /// <param name="context">context.</param>
    public PortalSteps(SessionContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Integration system updates work order status.
    /// </summary>
    /// <param name="workOrderStatus">Work Order status.</param>
    [When(@"Integration system updates workorder status as '(.*)'")]
    public void WhenTraderPortalApplicantRequestedToCancel(string workOrderStatus)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(3, retryAttempt => SpecflowBindingsConstants.ApiSleepDuration)
            .Execute(() =>
            {
                // TODO: replace with S2S user used by portal
                using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
                {
                    var requestToCancelEntityReference = plantsContext.msdyn_workordersubstatusSet.Single(p => p.msdyn_name == workOrderStatus).ToEntityReference();
                    var systemStatus = plantsContext.msdyn_workordersubstatusSet
                        .Where(wos => wos.Id == requestToCancelEntityReference.Id)
                        .Select(wos => wos.msdyn_SystemStatus)
                        .First();

                    var workOrder = new msdyn_workorder
                    {
                        Id = this.context.GetEntity("WorkOrder").Id,
                        msdyn_SystemStatus = systemStatus,
                        msdyn_SubStatus = requestToCancelEntityReference,
                    };
                    plantsContext.Execute(new UpdateRequest { Target = workOrder });
                }
            });
    }
}