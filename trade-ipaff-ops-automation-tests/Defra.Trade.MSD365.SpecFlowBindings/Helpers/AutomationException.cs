namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using System;

[Serializable]
public class AutomationException : Exception
{
    public AutomationException(string message)
        : base(message)
    {
    }

    public AutomationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
