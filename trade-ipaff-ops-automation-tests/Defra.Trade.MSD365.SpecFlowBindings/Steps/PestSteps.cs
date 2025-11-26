// <copyright file="PestSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using System.Linq;
using Reqnroll;

/// <summary>
/// Steps relating to pest processing.
/// </summary>
[Binding]
public class PestSteps : PowerAppsStepDefiner
{
    private readonly SessionContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="PestSteps"/> class.
    /// </summary>
    /// <param name="ctx">The test session context object.</param>
    public PestSteps(SessionContext ctx)
    {
        this.ctx = ctx;
    }

    /// <summary>
    /// Verifies that a given subgrid contains a certain number of pest records sorted by priority and usage count.
    /// </summary>
    /// <param name="gridName">The name of the subgrid to assert.</param>
    /// <param name="topCount">The expected number of pests in the subgrid.</param>
    [Then(@"the '(.*)' subgrid contains the top (.*) pests sorted by priority and then usage count")]
    public void VerifyPestsGrid(string gridName, int topCount)
    {
        var columnHeading = "Pest Name";

        if (topCount > 0)
        {
            GridHelper.WaitForRows(
                Driver,
                gridName,
                (rowCount) =>
                {
                    rowCount.Should().Be(topCount);
                },
                $"SubGrid {gridName} contains no rows.");

            using (var svcClient = SessionContext.GetServiceClient())
            {
                using (var context = new PlantsContext(svcClient))
                {
                    var pests = context.trd_pestSet
                        .OrderByDescending(x => x.trd_IsPestaPriority)
                        .ThenByDescending(x => x.trd_PestCount)
                        .Select(x => x.trd_name)
                        .Take(topCount)
                        .ToList();

                    for (int rowIndex = 0; rowIndex < pests.Count; rowIndex++)
                    {
                        pests[rowIndex].Should().Be(GridHelper.GetValueOfCell(Driver, gridName, columnHeading, rowIndex));
                    }
                }
            }
        }
        else
        {
            var rowCount = GridHelper.GetRows(Driver, gridName).Count;
            rowCount.Should().Be(topCount);
        }
    }
}
