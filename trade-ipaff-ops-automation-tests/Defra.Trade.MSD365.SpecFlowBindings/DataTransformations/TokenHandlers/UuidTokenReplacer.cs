// <copyright file="UuidTokenReplacer.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.TokenHandlers;

using System;
using System.Text.RegularExpressions;

/// <summary>
/// Handles UUID replacement.
/// </summary>
public class UuidTokenReplacer : BaseTokenHandler
{
    /// <summary>
    /// Specifies the allowed character list.
    /// </summary>
    /// <returns>Returns string.</returns>
    public override string AllowedCharacters()
    {
        return string.Empty;
    }

    /// <summary>
    /// Determines whether class supports random.
    /// </summary>
    /// <returns>Boolean.</returns>
    public override bool GeneratesRandom()
    {
        return false;
    }

    /// <inheritdoc/>
    public override string Generate(int amount)
    {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Gets the regex.
    /// </summary>
    /// <returns>Returns the Regex.</returns>
    public override Regex GetRegex()
    {
        return new Regex(@"{{random.uuid}}");
    }
}
