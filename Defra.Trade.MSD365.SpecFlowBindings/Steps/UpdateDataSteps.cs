// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using System;
using System.Linq;
using System.Net.Http;
using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Polly;
using Reqnroll;

[Binding]
public class UpdateDataSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;

    public UpdateDataSteps(SessionContext sessionContext)
    {
        this.sessionContext = sessionContext;
    }

    [When(@"'(.*)' has updated with the following values")]
    public void WhenHasUpdateWithTheFollowingValues(string variableName, Table table)
    {
        string recordId;
        string collectionName;

        if (this.sessionContext.Entities.Keys.Any(p => p.StartsWith(variableName)))
        {
            var dataEntity = this.sessionContext.Entities.Single(p => p.Key.StartsWith(variableName)).Value;
            recordId = dataEntity.Id.ToString();
            collectionName = dataEntity.EntityCollectionName;
        }
        else
        {
            recordId = this.sessionContext.GetVariable<Guid>(variableName).ToString();
            collectionName = this.sessionContext.GetVariable<string>(variableName + "EntityName") + "s";
        }

        Policy
            .Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this.sessionContext.SessionToken);
                    var booking = table.Rows.ToDictionary(r => r["Field"], r => r["Value"]);
                    httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(booking);
                    var requestUri = $"{TestConfig.GetTestUrl()}api/data/v9.0/{collectionName}({recordId})";
                    var response = httpClient.PatchAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ApplicationException($"Error: {response.Content.ReadAsStringAsync().Result} while updating {collectionName}:{recordId} with {json}, URL:{requestUri}");
                    }
                }
            });
    }
}