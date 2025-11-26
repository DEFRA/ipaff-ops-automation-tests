// <copyright file="CountryCheckSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
using Reqnroll;

/// <summary>
/// Bindings related to country check record processing.
/// </summary>
[Binding]
public class CountryCheckSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCheckSteps"/> class.
    /// </summary>
    /// <param name="sessionCtx">The test session context object.</param>
    public CountryCheckSteps(SessionContext sessionCtx)
    {
        this.sessionContext = sessionCtx;
    }

    /// <summary>
    /// Opens a related country check for an application.
    /// </summary>
    /// <param name="applicationAlias">The alias of the application related to the country check.</param>
    [Given(@"I have opened a country check for '(.*)'")]
    public void GivenIOpenRelatedCountryCheck(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var result = svc.WaitForRecords(
            new QueryByAttribute(trd_countrycheck.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(),
                Attributes = { "trd_exportapplication" },
                Values = { application.Id },
            },
            SpecflowBindingsConstants.DefaultWaitTime,
            SpecflowBindingsConstants.DefaultRetryInterval,
            false).Entities.FirstOrDefault();
            context.ClearChanges();

            XrmApp.Entity.OpenEntity("trd_countrycheck", result.Id);
        }
    }
}