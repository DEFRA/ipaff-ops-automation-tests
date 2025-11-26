namespace Defra.Trade.Plants.Model;

using Microsoft.Xrm.Sdk;

public static class trd_HMIResultExtensions
{
    /// <summary>
    /// Fail the HMI Result due to labeling or other result.
    /// </summary>
    /// <param name="entity">The instance of the HMI Result to fail.</param>
    /// <param name="context">An instance of the PlantsContext.</param>
    public static void FailedToMeetStandard(this trd_HMIResult entity, PlantsContext context)
    {
        entity.trd_labellingotherresult = false;
        context.UpdateObject(entity);
        context.SaveChanges();
    }

    /// <summary>
    /// Fail the HMI Result due to not meeting the class applied.
    /// </summary>
    /// <param name="entity">The instance of the HMI Result to fail.</param>
    /// <param name="defectClass">The Class Attained; this must be lower than Class Applied.</param>
    /// <param name="context">An instance of the PlantsContext.</param>
    public static void FailedToMeetStandard(this trd_HMIResult entity, EntityReference defectClass, PlantsContext context)
    {
        entity.trd_ClassAttained = defectClass;
        context.UpdateObject(entity);
        context.SaveChanges();
    }
}
