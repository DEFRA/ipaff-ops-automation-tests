namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using System;
using System.Text.RegularExpressions;

public class HumanReadableIntegerExpression
{
    public HumanReadableIntegerExpression(string value)
    {
        var regEx = new Regex(@"(\d)");

        if (regEx.Match(value).Success)
        {
            this.Value = Convert.ToInt32(regEx.Match(value).Groups[0].Value) - 1;
        }
        else
        {
            throw new Exception("Unexpected format");
        }
    }

    public int Value { get; }
}
