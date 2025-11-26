// <copyright file="TokenFactory.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Tokenisation;

using Defra.Trade.Plants.SpecFlowBindings.TokenHandlers;
using System.Collections.Generic;

/// <summary>
/// Factory to return all the token handlers.
/// </summary>
public static class TokenFactory
{
    /// <summary>
    /// Return all handlers for Tokenisation.
    /// </summary>
    /// <returns>All the registered Handlers.</returns>
    public static List<BaseTokenHandler> GetHandlers()
    {
        return new List<BaseTokenHandler>
        {
            new StringTokenReplacer(),
            new IntegerTokenReplacer(),
            new DateTimeTokenReplacer(),
            new UuidTokenReplacer(),
        };
    }
}
