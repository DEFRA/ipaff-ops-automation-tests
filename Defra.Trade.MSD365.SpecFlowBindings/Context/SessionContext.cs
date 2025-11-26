// <copyright file="SessionContext.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Context;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Configuration;
using Defra.Trade.Plants.Model;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

/// <summary>
/// Class to manage shared interaction with the bindings.
/// </summary>
public class SessionContext : PowerAppsStepDefiner
{
    private static readonly Random Global = new Random();
    [ThreadStatic]
    private static Random _local;

    /// <summary>
    /// Temporary until an app registration is provided for tests which exists in the Defra - Trade - Plants business unit.
    /// </summary>
    private static Guid? impersonatedUserObjectId;

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionContext"/> class.
    /// </summary>
    public SessionContext()
    {
        this.SessionId = Guid.NewGuid();
        this.Entities = new Dictionary<string, EntityHolder>();
        this.EntityReferences = new Dictionary<string, EntityReference>();
        this.Variables = new Dictionary<string, object>();
    }

    /// <summary>
    /// Gets unique Session Id to help with parallel executiuon.
    /// </summary>
    public Guid SessionId { get; }

    /// <summary>
    /// Gets the entities created in the test being executed.
    /// </summary>
    public Dictionary<string, EntityHolder> Entities { get; private set; }

    /// <summary>
    /// Gets the entity references created in the test being executed.
    /// </summary>
    public Dictionary<string, EntityReference> EntityReferences { get; }

    /// <summary>
    /// Gets the username of the user running the test.
    /// </summary>
    public string UserName { get; internal set; }

    /// <summary>
    /// Gets the password of the user running the test.
    /// </summary>
    public string Password { get; internal set; }

    /// <summary>
    /// Gets the Bearer token used for api calls.
    /// </summary>
    public string SessionToken { get; internal set; }

    /// <summary>
    /// Gets current user.
    /// </summary>
    public Guid UserId => Guid.Parse((string)Driver.ExecuteScript("return Xrm.Utility.getGlobalContext().userSettings.userId;"));

    /// <summary>
    /// Gets or sets the variables used in the executed test.
    /// </summary>
    private Dictionary<string, object> Variables { get; set; }

    /// <summary>
    /// Gets a service client authenticated as the configured admin application user.
    /// </summary>
    /// <param name="requireNewInstance">Specifies whether to reuse an existing connection if recalled while the connection is still active.</param>
    /// <returns>A <see cref="CrmServiceClient"/> authenticated as the configured application user.</returns>
    public static CrmServiceClient GetServiceClient(bool requireNewInstance = false)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var serviceClient = new CrmServiceClient(
            $"Url={TestConfig.GetTestUrl()}; " +
            $"ClientId={TestConfig.ApplicationUser.ClientId}; " +
            $"ClientSecret={TestConfig.ApplicationUser.ClientSecret}; " +
            $"AuthType=ClientSecret; " +
            $"RequireNewInstance={requireNewInstance}");

        // Temporarily impersonate an admin user in the Defra - Trade - Plants business unit.
        // ADO application user will be in the root BU. New app registration to be created in Defra - Trade - Plants business unit.
        if (!impersonatedUserObjectId.HasValue)
        {
            var query = new QueryByAttribute(SystemUser.EntityLogicalName);
            query.AddAttributeValue("internalemailaddress", TestConfig.GetUser("an admin").Username);
            query.ColumnSet = new ColumnSet("azureactivedirectoryobjectid");

            impersonatedUserObjectId = serviceClient.RetrieveMultiple(query).Entities
                .First()
                .ToEntity<SystemUser>()
                .AzureActiveDirectoryObjectId;
        }

        serviceClient.CallerAADObjectId = impersonatedUserObjectId.Value;

        if (serviceClient.LastCrmException != null)
        {
            throw serviceClient.LastCrmException;
        }

        return serviceClient;
    }

    /// <summary>
    /// Gets the test variable by name.
    /// </summary>
    /// <param name="variableName">variableName.</param>
    /// <returns>object.</returns>
    public object GetVariable(string variableName)
    {
        if (this.Variables.TryGetValue(variableName, out object result))
        {
            return result;
        }

        return null;
    }

    /// <summary>
    /// Returns Entity Holder information.
    /// </summary>
    /// <param name="alias">alias.</param>
    /// <returns></returns>
    public EntityHolder GetEntity(string alias)
    {
        return Entities[alias + SessionId];
    }

    /// <summary>
    /// Gets the variable by name.
    /// </summary>
    /// <param name="variableName">variableName.</param>
    /// <param name="val">value returned.</param>
    public void SetVariable(string variableName, object val)
    {
        this.Variables[variableName] = val;
    }

    /// <summary>
    /// Gets the test variable by name.
    /// </summary>
    /// <param name="variableName">variableName.</param>
    /// <returns>Object of type T.</returns>
    /// <typeparam name="T">T Return type.</typeparam>
    public T GetVariable<T>(string variableName)
    {
        if (this.Variables.TryGetValue(variableName, out var result))
        {
            return (T)result;
        }

        return default;
    }

    /// <summary>
    /// Gets the CrmServiceClient which will aid with strong typed api calls.
    /// </summary>
    /// <param name="user">username.</param>
    /// <returns>CrmServiceClient.</returns>
    public static CrmServiceClient GetServiceClient(UserConfiguration user)
    {
        return new CrmServiceClient($"Url={TestConfig.GetTestUrl()}; ClientId={user.Username}; ClientSecret={user.Password}; authtype=ClientSecret; RequireNewInstance=true");
    }

    /// <summary>
    /// Gets the CrmServiceClient which will aid with strong typed api calls.
    /// </summary>
    /// <param name="user">username.</param>
    /// <returns>CrmServiceClient.</returns>
    public static CrmServiceClient GetServiceClient(string user)
    {
        return GetServiceClient(TestConfig.GetUser(user));
    }

    /// <summary>
    /// Gets a service client authenticated as the configured admin application user.
    /// </summary>
    /// <returns>A <see cref="CrmServiceClient"/> authenticated as the configured application user.</returns>
    public static CrmServiceClient GetServiceClient()
    {
        return GetServiceClient(false);
    }

    /// <summary>
    /// Clears the session values after a test.
    /// </summary>
    public void Reset()
    {
        this.Entities.Clear();
        this.Variables.Clear();
        this.SessionToken = string.Empty;
    }

    /// <summary>
    /// Generates a random number.
    /// </summary>
    /// <returns>Random Number.</returns>
    public int RandomNumber()
    {
        if (_local != null)
        {
            return _local.Next();
        }

        lock (Global)
        {
            if (_local != null)
            {
                return _local.Next();
            }

            var seed = Global.Next();
            _local = new Random(seed);
        }

        return _local.Next();
    }

    /// <summary>
    /// Get entity reference of an alias.
    /// </summary>
    /// <param name="alias">alias.</param>
    /// <returns>EntityReference.</returns>
    public EntityReference GetEntityReference(string alias)
    {
        var calcAlias = alias + this.SessionId;
        if (this.EntityReferences.ContainsKey(calcAlias))
        {
            return this.EntityReferences[calcAlias];
        }

        if (!this.Entities.ContainsKey(calcAlias))
        {
            var entityReference = TestDriver.GetTestRecordReference(alias);
            this.EntityReferences.Add(calcAlias, entityReference);
        }
        else
        {
            var entityHolder = this.GetEntity(alias);
            this.EntityReferences.Add(calcAlias, new EntityReference(entityHolder.EntityName, entityHolder.Id));
        }

        return this.EntityReferences[calcAlias];
    }

    /// <summary>
    /// Get entity reference of an alias.
    /// </summary>
    /// <param name="alias">alias.</param>
    /// <param name="entityReference">an EntityReference.</param>
    /// <returns>true if it is found successfully; otherwise, false.</returns>
    public bool TryGetEntityReference(string alias, out EntityReference entityReference)
    {
        entityReference = null;

        try
        {
            entityReference = this.GetEntityReference(alias);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}