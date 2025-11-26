// <copyright file="EntityHolder.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Context;

using System;

/// <summary>
/// Holds information on a Dynamics entity.
/// </summary>
public class EntityHolder
{
    /// <summary>
    /// gets or sets the Guid of the record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// gets or sets the alias of the record.
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// gets or sets the entity name of the record.
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    /// gets the entity collection name of the record.
    /// </summary>
    public string EntityCollectionName { get; set; }
}
