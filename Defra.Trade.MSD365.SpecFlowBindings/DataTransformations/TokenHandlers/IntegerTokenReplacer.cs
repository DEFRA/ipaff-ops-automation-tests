// <copyright file="IntegerTokenReplacer.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.TokenHandlers;

using System.Text.RegularExpressions;

/// <summary>
/// Handles string replacement.
/// </summary>
public class IntegerTokenReplacer : BaseTokenHandler
{
    /// <summary>
    /// Gets the regex.
    /// </summary>
    /// <returns>Returns the Regex.</returns>
    public override Regex GetRegex()
    {
        return new Regex(@"#{RandomNumber\[\d+]}#");
    }

    /// <summary>
    /// Generate.
    /// </summary>
    /// <param name="amount">The length of the random.</param>
    /// <returns>Random String.</returns>
    public override bool GeneratesRandom()
    {
        return true;
    }

    /// <summary>
    /// Specifies the allowed character list.
    /// </summary>
    /// <returns>Returns string.</returns>
    public override string AllowedCharacters()
    {
        return "1234567890";
    }
}
