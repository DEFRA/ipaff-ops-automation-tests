// <copyright file="StringExtensions.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Extensions;

using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Tokenisation;

/// <summary>
/// Extensions to <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Replaces all tokens in the json file with the available functions.
    /// </summary>
    /// <param name="input">String.</param>
    /// <returns>String with the tokenised output.</returns>
    public static string TokeniseText(this string input)
    {
        TokenFactory.GetHandlers().ForEach(handler =>
        {
            input = handler.ReplaceTokens(input);
        });

        return input;
    }

    /// <summary>
    /// Transforms a given alias into a guid.
    /// </summary>
    /// <param name="input">The alias to be transformed.</param>
    /// <param name="ctx">The session context.</param>
    /// <returns>Returns a guid as a string.</returns>
    public static string TransformAliasToGuid(this string input, SessionContext ctx)
    {
        input = input.Replace("alias.bind", "odata.bind");

        foreach (var item in ctx.Entities)
        {
            var holder = item.Value;
            input = input.Replace(holder.Alias.ToString(), $"/{holder.EntityCollectionName}({holder.Id})");
        }

        return input;
    }
}
