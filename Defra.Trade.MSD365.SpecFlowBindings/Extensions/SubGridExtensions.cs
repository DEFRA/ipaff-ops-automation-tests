// <copyright file="EntityExtensions.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Extensions;

using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;

/// <summary>
/// <see cref="SubGridExtensions"/> extension methods.
/// </summary>
public static class SubGridExtensions
{
    private const string EditableSubGridList = ".//div[contains(@data-lp-id, \"[NAME]\") and contains(@class, 'editableGrid')]";
    private const string EditableSubGridListCells = ".//div[contains(@wj-part, 'cells') and contains(@class, 'wj-cells') and contains(@role, 'grid')]";
    private const string EditableSubGridListCellRows = ".//div[contains(@class, 'wj-row') and contains(@role, 'row')]";

    /// <summary>
    /// Opens a record from an editable subgrid.
    /// </summary>
    /// <param name="editableSubGrid">The editable subgrid.</param>
    /// <param name="webDriver">The <see cref="IWebDriver"/>.</param>
    /// <param name="subgridName">The subgrid name.</param>
    /// <param name="rowIndex">The row index of the record to be opened.</param>
    /// <returns>BrowserCommandResult.</returns>
    public static BrowserCommandResult<bool> OpenEditableSubGridRecord(this SubGrid editableSubGrid, IWebDriver webDriver, string subgridName, int rowIndex = 0)
    {
        editableSubGrid = editableSubGrid ?? throw new ArgumentNullException(nameof(editableSubGrid));
        webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
        subgridName = subgridName ?? throw new ArgumentNullException(nameof(subgridName));

        webDriver.SwitchTo().DefaultContent();
        var editableGrid = GridHelper.GetGrid(webDriver, subgridName);

        var rows = GridHelper.GetRows(editableGrid);
        var cells = GridHelper.GetCellsForRow(rows, rowIndex);

        new Actions(webDriver).DoubleClick(cells[1]).Perform();
        webDriver.WaitForTransaction();

        return true;
    }
}
