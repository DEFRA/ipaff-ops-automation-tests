namespace Defra.Trade.Plants.SpecFlowBindings;

using System.Collections.Generic;

/// <summary>
/// Static class to store constants for data file names.
/// </summary>
public static class DataFileConstants
{
    /// <summary>
    /// Maps entity to the HMI Result Lookup field.
    /// </summary>
    public static Dictionary<string, string> HMIResultRelations = new Dictionary<string, string>()
    {
        { "commodity defects", "trd_RelatedHMIResult@odata.bind" },
        { "test results", "trd_HMIResultId@odata.bind" },
        { "sample results", "trd_HMIResultId@odata.bind" },
    };
}
