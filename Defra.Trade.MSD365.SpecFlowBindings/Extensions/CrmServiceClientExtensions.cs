// <copyright file="CrmServiceClientExtensions.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Extensions;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Polly;
using System;
using System.Linq;
using System.Threading;

/// <summary>
/// Extensions for <see cref="CrmServiceClient"/>.
/// </summary>
public static class CrmServiceClientExtensions
{
    /// <summary>
    /// Waits for a field to contain a value and returns the value.
    /// </summary>
    /// <typeparam name="TAttributeType">The type of attribute.</typeparam>
    /// <param name="svcClient">The <see cref="CrmServiceClient"/>.</param>
    /// <param name="entityName">The entity logical name.</param>
    /// <param name="id">The entity ID.</param>
    /// <param name="field">The field.</param>
    /// <param name="timeout">The timeout.</param>
    /// <param name="interval">The polling interval.</param>
    /// <returns>The populated field value.</returns>
    /// <exception cref="TimeoutException" />
    public static TAttributeType WaitForFieldValue<TAttributeType>(this CrmServiceClient svcClient, string entityName, Guid id, string field, TimeSpan timeout, int interval = 10000)
    {
        Entity entity = null;
        Policy.Handle<Exception>()
            .WaitAndRetry(SpecflowBindingsConstants.ApiRetryAttempts, retryCount => timeout)
            .Execute(() =>
            {
                entity = svcClient.Retrieve(entityName, id, new ColumnSet(field));
                if (!entity.Contains(field))
                {
                    throw new Exception($"A value for {entityName}({id}).{field} was not found within the specified timeout");
                }
            });

        return (TAttributeType)entity[field];
    }

    /// <summary>
    /// Waits for a query to return records.
    /// </summary>
    /// <typeparam name="TAttributeType">The type of attribute.</typeparam>
    /// <param name="svcClient">The <see cref="CrmServiceClient"/>.</param>
    /// <param name="query">The query.</param>
    /// <param name="timeout">The timeout.</param>
    /// <param name="interval">The polling interval.</param>
    /// <returns>The returned records.</returns>
    /// <exception cref="TimeoutException" />
    public static EntityCollection WaitForRecords(this CrmServiceClient svcClient, QueryBase query, TimeSpan timeout, int interval = 10000, bool continueOnError = false)
    {
        EntityCollection records = null;

        Policy
            .HandleResult<EntityCollection>(r => r.Entities.Count == 0)
            .WaitAndRetry(Enumerable.Repeat(TimeSpan.FromMilliseconds(interval), (int)timeout.TotalMilliseconds / interval))
            .Execute(() =>
            {
                return records = svcClient.RetrieveMultiple(query);
            });

        if (!continueOnError && records.Entities.Count == 0)
        {
            throw new TimeoutException($"No records were found within the specified timeout");
        }

        return records;
    }
}