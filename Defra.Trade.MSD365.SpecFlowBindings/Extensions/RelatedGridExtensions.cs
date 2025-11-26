namespace Defra.Trade.Plants.SpecFlowBindings.Extensions;

using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;

/// <summary>
/// RelatedGrid Extension methods.
/// </summary>
public static class RelatedGridExtensions
{
    /// <summary>
    /// Gets the number of rows within the SubGrid.
    /// </summary>
    /// <param name="relatedGrid">An instance of the XrmApp.RelatedGrid.</param>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <returns>The number of rows.</returns>
    public static int GetRowCount(this RelatedGrid relatedGrid, IWebDriver webDriver)
    {
        webDriver.SwitchTo().DefaultContent();
        var grid = webDriver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));
        var gridCellContainer = grid.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.CellContainer]));
        var rowCount = int.Parse(gridCellContainer.GetAttribute("aria-rowcount"));
        return rowCount - 1; // subtract 1 to exclude header;
    }

    /// <summary>
    /// Gets the number of columns within the SubGrid.
    /// </summary>
    /// <param name="relatedGrid">An instance of the XrmApp.RelatedGrid.</param>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <returns>The number of columns.</returns>
    public static int GetColumnCount(this RelatedGrid relatedGrid, IWebDriver webDriver)
    {
        webDriver.SwitchTo().DefaultContent();
        var grid = webDriver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));
        var gridCellContainer = grid.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.CellContainer]));
        var columnCount = int.Parse(gridCellContainer.GetAttribute("aria-colcount"));
        return columnCount;
    }
}
