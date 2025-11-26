

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;
using System;
using Reqnroll;

/// <summary>
/// Steps relating to related grids.
/// </summary>
[Binding]
public class RelatedEntityGridSteps : PowerAppsStepDefiner
{
    [Then(@"I can see (exactly|more than|less than) (\d+) records in the related grid")]
    public void ThenICanSeeRecordsInTheRelatedGrid(string compare, int count)
    {
        var actualCount = XrmApp.RelatedGrid.GetRowCount(Driver);

        switch (compare)
        {
            case "exactly":
                actualCount.Should().Be(count);
                break;
            case "more than":
                actualCount.Should().BeGreaterThan(count);
                break;
            case "less than":
                actualCount.Should().BeLessThan(count);
                break;
        }

    }
}
