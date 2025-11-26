// <copyright file="PlantInspectionGridSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Reqnroll;

/// <summary>
/// Steps relating to the plants inspection results grid.
/// </summary>
[Binding]
public class PlantInspectionGridSteps : PowerAppsStepDefiner
{
    /// <summary>
    /// Enters inspection results using the UI into the plants inspection grid and submits the results.
    /// </summary>
    /// <param name="positionString">The position on the subgrid to enter the information.</param>
    /// <param name="gridName">The name of the subgrid.</param>
    /// <param name="table">The table of input information.</param>
    [When(@"I enter the (\d[a-z]+) record on the (.*) grid as follows and submit the results")]
    public static void WhenIEnterTheRecordOnTheGridAsFollowsAndSubmitTheResults(string positionString, string gridName, Table table)
    {
        GridSteps.WhenIEnterTheRecordOnTheGridAsFollows(positionString, gridName, table);
        SharedSteps.WaitForScriptProcessing();
        PopupSteps.WhenIClickSave("msdyn_workorderservicetask");
        SharedSteps.WaitForScriptProcessing();
        GridHelper.SelectRow(Driver, gridName, new HumanReadableIntegerExpression(positionString).Value);
        CommandSteps.WhenISelectTheCommand("Submit Results", "trd_inspectionresult");
        SharedSteps.WaitForScriptProcessing();
        PopupSteps.WhenIClickSave("msdyn_workorderservicetask");
        SharedSteps.WaitForScriptProcessing();
    }
}
