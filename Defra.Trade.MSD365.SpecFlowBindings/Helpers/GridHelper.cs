namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using Defra.Trade.Plants.SpecFlowBindings.Steps;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Helper class for interacting with Dynamics views, grids and subgrids.
/// </summary>
public class GridHelper
{
    public static IWebDriver driver;
    private const string GridHeaderClassPcf = "pcf-grid-header";
    private const string GridHeaderClassWj = "wj-header";

    /// <summary>
    /// Gets a list of headers for a given grid.
    /// </summary>
    /// <param name="driver">The IWebDriver object for interacting with the test window.</param>
    /// <param name="gridName">The name of the grid to retrieve headers for.</param>
    /// <returns>Returns the headers of the grid as a list of IWebElements.</returns>
    public static IList<IWebElement> GetHeaders(IWebDriver driver, string gridName)
    {

        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("document.body.style.zoom='50%'");
        var grid = GetGrid(driver, gridName);
        grid.Should().NotBeNull($"Unable to find the grid {gridName}");
        var headers = grid.FindElements(By.XPath($".//div[contains (@class, '{GridHeaderClassWj}') or contains (@class, '{GridHeaderClassPcf}')]"));
        return headers;
    }

    /// <summary>
    /// Validates that a view displays the expected column headers.
    /// </summary>
    /// <param name="driver">The IWebDriver object for interacting with the test window.</param>
    /// <param name="gridName">The name of the grid to retrieve headers for.</param>
    /// <param name="expectedHeaders">The list of headers expected to be on view.</param>
    /// <param name="inOrder">Do the Headers need to be in the order listed.</param>
    public static void ValidateHeaders(IWebDriver driver, string gridName, string[] expectedHeaders, bool inOrder)
    {
        driver.WaitForTransaction();

        if (!driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Control]), out var grid))
        {
            throw new NotFoundException("Unable to find grid");
        }

        var actualHeaders = grid.FindElements(By.CssSelector("[class*='headerText-']")).Select(e => e.Text);

        if (!actualHeaders.Any())
        {
            throw new NotFoundException("Unable to find any headers on the grid");
        }

        if (inOrder)
        {
            actualHeaders.Should().Equal(expectedHeaders);
        }
        else
        {
            actualHeaders.Should().BeEquivalentTo(expectedHeaders);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridContext"></param>
    /// <returns></returns>
    public static IQueryable<IWebElement> GetColumns(IWebElement gridContext)
    {
        var columnGroup = gridContext.FindElement(By.ClassName("wj-colheaders"));
        var headerRow = columnGroup.FindElement(By.ClassName("wj-row"));

        return headerRow.FindElements(By.XPath(".//div"))
            .Where(c => !string.IsNullOrEmpty(c.GetAttribute("title")) && !string.IsNullOrEmpty(c.GetAttribute("id")))
            .AsQueryable();
    }

    public static List<GridItem> GetGridItems(IWebElement gridContext)
    {
        var gridItems = new List<GridItem>();
        var subGridRows = GetRows(gridContext);
        var subGridColumns = GetColumns(gridContext);

        foreach (var subGridRow in subGridRows)
        {
            var rowCells = subGridRow.FindElements(By.ClassName("wj-cell"));
            var item = new GridItem() { EntityName = "" };
            var currentIndex = 0;

            foreach (var subGridColumn in subGridColumns)
            {
                var id = subGridColumn.GetAttribute("data-id");
                if (!id.Contains("btnheaderselectcolumn"))
                {
                    var data = rowCells[currentIndex + 1].GetAttribute("title");
                    item[id] = data;
                    currentIndex++;
                }
            }

            gridItems.Add(item);
        }

        return gridItems;
    }

    /// <summary>
    /// Finds a specific header on the page.
    /// </summary>
    /// <param name="driver">The IWebDriver object for interacting with the test window.</param>
    /// <param name="headerName">The name of the header to find.</param>
    /// <returns>Returns the header as an IWebElement object.</returns>
    public static IWebElement FindHeader(IWebDriver driver, string headerName)
    {
        var headerObj = driver.FindElement(By.XPath($".//div[contains (@class, 'headerText') and contains(string(), '{headerName}')]"));
        return headerObj;
    }

    public static IList<IWebElement> GetRows(IWebDriver driver, string gridName)
    {
        driver.WaitForTransaction();

        var formContext = FormHelper.GetFormContext(driver);
        var grid = GetGrid(driver, formContext, gridName);
        try
        {
            return grid
                .FindElement(By.ClassName("ag-center-cols-container"))
                .FindElements(By.XPath(".//div[contains (@class, 'ag-row') and contains(@role, 'row')]"));
        }
        catch (Exception)
        {
            return grid.FindElements(By.XPath(".//div[contains (@class, 'wj-row') and contains(@aria-label, 'Data')]"));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridContext"></param>
    /// <returns></returns>
    public static IList<IWebElement> GetRows(IWebElement gridContext)
    {
        var gridRows = gridContext.FindElements(By.XPath(".//div[contains (@class, 'wj-row') and contains(@aria-label, 'Data')]"));
        return gridRows;
    }

    public static string GetValueOfCell(IWebDriver driver, string gridName, string headerName, int rowIndex)
    {
        var headerIndex = GetHeaderIndex(driver, gridName, headerName);
        var cells = GetCells(driver, gridName, rowIndex);
        var outerCell = cells[headerIndex];
        return outerCell.Text.Replace("", string.Empty);
    }

    public static void SelectAllRows(IWebDriver driver, string gridName = "")
    {
        var grid = GetGrid(driver, gridName);

        Policy
            .Timeout(5.Seconds())
            .Execute(() =>
            {
                var selected = false;
                do
                {
                    // 2 different types of grid
                    //var possibleSelectAllElements = new[] { ".//button[@title='Select All']", ".//span[@role='checkbox' and @aria-label='Toggle selection of all rows']" };
                    var possibleSelectAllElements = new[] { ".//button[@title='Select All']", "//*[contains(@id, 'quickview_import_commodity_lines.subgrid_import_commodity_lines-pcf_grid_control_container')]/div/div[1]/div/div/div/div/div[2]/div[2]/div[1]/div[2]/div/div/div[1]" };

                    foreach (var el in possibleSelectAllElements)
                    {
                        if (grid.FindElements(By.XPath(el)).Count > 0)
                        {
                            grid.ClickWhenAvailable(By.XPath(el));
                            break;
                        }
                    }

                    selected = driver.FindElements(By.CssSelector("div.wj-row[aria-selected=true]")).Count > 0
                        || driver.FindElements(By.XPath(".//div[@role='row' and @aria-selected='true']")).Count > 0;
                }
                while (!selected);
            });
    }

    public static bool IsRowSelected(IWebDriver driver, string gridName, int rowIndex)
    {
        var rows = GetRows(driver, gridName);
        return rows[rowIndex].GetAttribute("aria-selected") == "true";
    }

    public static void SelectRow(IWebDriver driver, string gridName, int rowIndex)
    {
        driver.WaitForTransaction();

        var rows = GetRows(driver, gridName);
        var row = rows[rowIndex];
        if (row.HasAttribute("aria-selected") && row.GetAttribute<bool>("aria-selected"))
        {
            return;
        }

        GetCellsForRow(rows, rowIndex)[0].Click();

        driver.WaitForTransaction();
    }

    public static void UnselectRow(IWebDriver driver, string gridName, int rowIndex)
    {
        driver.WaitForTransaction();

        var rows = GetRows(driver, gridName);
        var row = rows[rowIndex];
        if (row.HasAttribute("aria-selected") && row.GetAttribute<bool>("aria-selected"))
        {
            GetCellsForRow(rows, rowIndex)[0].Click();
            driver.WaitForTransaction();
        }
    }

    public static void OpenTheGridRow(IWebDriver driver, string gridName, string headerName, int Index)
    {
        driver.WaitForTransaction();
        GridHelper.SelectRow(driver, gridName, Index);
        SharedSteps.WaitForScriptProcessing();

        var headerIndex = GridHelper.GetHeaderIndex(driver, gridName, headerName);
        var cells = GridHelper.GetCells(driver, gridName, Index);
        var outerCell = cells[headerIndex];
        outerCell.Click();
        SharedSteps.WaitForScriptProcessing();
        driver.DoubleClick(outerCell);
        driver.WaitForTransaction();
    }

    public static void SelectCell(IWebDriver driver, string gridName, string headerName, int rowIndex)
    {
        var headerIndex = GetHeaderIndex(driver, gridName, headerName);
        var cells = GetCells(driver, gridName, rowIndex);
        cells[headerIndex].Click();
    }

    public static void ShowCellDropdown(IWebDriver driver, string gridName, string headerName, int rowIndex)
    {
        var headerIndex = GetHeaderIndex(driver, gridName, headerName);
        var cells = GetCells(driver, gridName, rowIndex);
        var outerCell = cells[headerIndex];
        outerCell.Click();
        var dropDownButton = outerCell.WaitUntilAvailable(By.XPath(".//button[@aria-label='Toggle Dropdown']"));
        if (!ElementHelper.IsElementPresent(driver, By.XPath(".//div[@role='listbox' and contains(@class, 'wj-dropdown-panel')]"))
            && !ElementHelper.IsElementPresent(driver, By.XPath(".//div[@wj-part='dropdown']")))
        {
            dropDownButton.Click();
        }
    }

    public static void SelectCellAndClickTheLookupNavigationButton(IWebDriver driver, string gridName, string headerName, int rowIndex)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(1))
            .Execute(() =>
            {
                if (!IsRowSelected(driver, gridName, rowIndex))
                {
                    SelectRow(driver, gridName, rowIndex);
                    SharedSteps.WaitForScriptProcessing();
                }

                // wait for nested input/s to appear
                IReadOnlyCollection<IWebElement> inputElements = null;
                IWebElement innerCell = null;
                var headerIndex = GetHeaderIndex(driver, gridName, headerName);
                var cells = GetCells(driver, gridName, rowIndex);
                var outerCell = cells[headerIndex];
                outerCell.Click();
                SharedSteps.WaitForScriptProcessing();
                Wait.Until(TimeSpan.FromSeconds(5), () => (inputElements = outerCell.FindElements(By.XPath(".//button"))).Any());

                if (inputElements.Count > 1)
                {
                    innerCell = outerCell.FindElement(By.XPath(".//button[@class='wj-btn-default wj-btn cc-gridcell-navigable']"));
                    innerCell.Click();
                }
                else
                {
                    throw new Exception("Editable grid lookup button not found");
                }
            });
    }        

    public static string GetGridErrorMessage(IWebDriver driver, string gridName, string headerName, int rowIndex)
    {
        return Policy
        .Handle<Exception>()
        .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(1))
        .Execute(() =>
        {
            var headerIndex = GetHeaderIndex(driver, gridName, headerName);
            var cells = GetCells(driver, gridName, rowIndex);
            var outerCell = cells[headerIndex];
            var elements = GetGrid(driver, gridName).FindElements(By.XPath(".//div[contains(@id,'_genericError') or contains(@id,'_updateError')]/div"));

            if (!elements.Any())
            {
                return string.Empty;
            }

            var errorText = elements
                            .Select(e => e.GetAttribute("textContent"))
                            .Where(et => et.StartsWith($"Error at row {rowIndex + 1},") && et.Contains($"{headerName}:"))
                            .FirstOrDefault();

            if (errorText == null)
            {
                return string.Empty;
            }

            return errorText.Replace($"Error at row {rowIndex + 1}, {headerName}: ", string.Empty);
        });
    }

    public static void InputValueIntoCell(IWebDriver driver, string gridName, string headerName, int rowIndex, string value, bool clearExistingValue = false, bool clickAway = true)
    {
        Policy
         .Handle<Exception>()
         .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(1))
         .Execute(() =>
         {
             if (!IsRowSelected(driver, gridName, rowIndex))
             {
                 SelectRow(driver, gridName, rowIndex);
                 SharedSteps.WaitForScriptProcessing();
             }

             // wait for nested input/s to appear
             IReadOnlyCollection<IWebElement> inputElements = null;
             IWebElement innerCell = null;
             var headerIndex = GetHeaderIndex(driver, gridName, headerName);
             var cells = GetCells(driver, gridName, rowIndex);
             var outerCell = cells[headerIndex];
             bool done = false;
             outerCell.Click();
             SharedSteps.WaitForScriptProcessing();
             Wait.Until(TimeSpan.FromSeconds(5), () => (inputElements = outerCell.FindElements(By.XPath(".//input"))).Any());

             if (inputElements.Count > 1)
             {
                 // Choose based on wj-part as per previous code
                 innerCell = outerCell.FindElement(By.XPath(".//input[@wj-part='input']"));
             }
             else
             {
                 // Maybe a text area?
                 var textArea = outerCell.FindElements(By.XPath(".//textarea")).FirstOrDefault();

                 // Otherwise there's only one input, so we'll use that
                 innerCell = textArea ?? inputElements.First();
             }

             try
             {
                 Wait.Until(TimeSpan.FromSeconds(5), () => innerCell.Displayed);
             }
             catch (TimeoutException)
             {
                 // for boolean fields this is okay as the underlying control cannpt be focused
             }

             if (clearExistingValue)
             {
                 innerCell.Click();
                 innerCell.InputText(Keys.Control + "a", false);
                 innerCell.InputText(Keys.Delete, false);
                 SharedSteps.WaitForScriptProcessing();
             }

             // is there a dropdown?
             var dropDownButton = outerCell.FindElements(By.XPath(".//button[@aria-label='Toggle Dropdown']"));

             if (dropDownButton.Count > 0)
             {
                 // dropdown mode
                 if (!dropDownButton[0].Displayed)
                 {
                     // there seems to be a bug whereby first time around the dropdown button may not appear
                     // if we click enter in the cell, then re-select the row this sorts it out
                     innerCell.InputText(Keys.Tab, false);
                     SharedSteps.WaitForScriptProcessing();

                     // double select all clears selection
                     SelectAllRows(driver, gridName);
                     SelectAllRows(driver, gridName);

                     // now throw an Exception to re-execute the Policy from scratch
                     throw new ApplicationException("Retry");
                 }
                 else
                 {
                     // loop twice, pressing the dropdown button if we fail the first time
                     for (int loop = 0; loop < 2; loop++)
                     {
                         // try and find dropdown item...
                         var item = driver.FindElements(By.XPath($"//div[@class='cc-grid-lookupitem-field-0' and .='{value}']"));

                         if (!item.Any())
                         {
                             item = driver.FindElements(By.XPath($"//div[@class='wj-listbox-item' and .='{value}']"));
                         }

                         if (!item.Any())
                         {
                             dropDownButton[0].Click();
                         }
                         else
                         {
                             item.First().Click();
                             done = true;
                         }
                     }
                 }
             }

             if (!done)
             {
                 innerCell.InputText(value, false);
                 innerCell.InputText(Keys.Tab, false);
             }

             if (clickAway)
             {
                 driver.FindElement(By.TagName("body")).Click();
             }

             driver.WaitForTransaction();
         });
    }
    public static void InputValueIntoCellOfSubgrid(IWebDriver driver, string gridName, string headerName, int rowIndex, string value, bool clearExistingValue = false, bool clickAway = true)
    {
        Policy
         .Handle<Exception>()
         .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(1))
         .Execute(() =>
         {
             if (!IsRowSelected(driver, gridName, rowIndex))
             {
                 SelectRow(driver, gridName, rowIndex);
                 SharedSteps.WaitForScriptProcessing();
             }

             // wait for nested input/s to appear
             IReadOnlyCollection<IWebElement> inputElements = null;
             IWebElement innerCell = null;
             var headerIndex = GetHeaderIndex(driver, gridName, headerName);
             var cells = GetCells(driver, gridName, rowIndex);
             var outerCell = cells[headerIndex];
             bool done = false;
             outerCell.Click();
             SharedSteps.WaitForScriptProcessing();
             driver.DoubleClick(outerCell);
             driver.WaitForTransaction();
             SharedSteps.WaitForScriptProcessing();
             Wait.Until(TimeSpan.FromSeconds(5), () => (inputElements = outerCell.FindElements(By.XPath(".//input"))).Any());

             if (inputElements.Count > 1)
             {
                 // Choose based on wj-part as per previous code
                 innerCell = outerCell.FindElement(By.XPath(".//input[@wj-part='input']"));
             }
             else
             {
                 // Maybe a text area?
                 var textArea = outerCell.FindElements(By.XPath(".//textarea")).FirstOrDefault();

                 // Otherwise there's only one input, so we'll use that
                 innerCell = textArea ?? inputElements.First();
             }

             try
             {
                 Wait.Until(TimeSpan.FromSeconds(5), () => innerCell.Displayed);
             }
             catch (TimeoutException)
             {
                 // for boolean fields this is okay as the underlying control cannpt be focused
             }

             if (clearExistingValue)
             {
                 innerCell.Click();
                 innerCell.InputText(Keys.Control + "a", false);
                 innerCell.InputText(Keys.Delete, false);
                 SharedSteps.WaitForScriptProcessing();
             }

             // is there a dropdown?
             var dropDownButton = outerCell.FindElements(By.XPath(".//button[@aria-label='Toggle Dropdown']"));

             if (dropDownButton.Count > 0)
             {
                 // dropdown mode
                 if (!dropDownButton[0].Displayed)
                 {
                     // there seems to be a bug whereby first time around the dropdown button may not appear
                     // if we click enter in the cell, then re-select the row this sorts it out
                     innerCell.InputText(Keys.Tab, false);
                     SharedSteps.WaitForScriptProcessing();

                     // double select all clears selection
                     SelectAllRows(driver, gridName);
                     SelectAllRows(driver, gridName);

                     // now throw an Exception to re-execute the Policy from scratch
                     throw new ApplicationException("Retry");
                 }
                 else
                 {
                     // loop twice, pressing the dropdown button if we fail the first time
                     for (int loop = 0; loop < 2; loop++)
                     {
                         // try and find dropdown item...
                         var item = driver.FindElements(By.XPath($"//div[@class='cc-grid-lookupitem-field-0' and .='{value}']"));

                         if (!item.Any())
                         {
                             item = driver.FindElements(By.XPath($"//div[@class='wj-listbox-item' and .='{value}']"));
                         }

                         if (!item.Any())
                         {
                             dropDownButton[0].Click();
                         }
                         else
                         {
                             item.First().Click();
                             done = true;
                         }
                     }
                 }
             }

             if (!done)
             {
                 innerCell.InputText(value, false);
                 innerCell.InputText(Keys.Tab, false);
             }

             if (clickAway)
             {
                 driver.FindElement(By.TagName("body")).Click();
             }

             driver.WaitForTransaction();
         });
    }

    public static void InputOptionSetValueIntoCell(IWebDriver driver, string gridName, string headerName, int rowIndex, string value)
    {
        var headerIndex = GetHeaderIndex(driver, gridName, headerName);
        var cells = GetCells(driver, gridName, rowIndex);
        var outerCell = cells[headerIndex];
        outerCell.Click();
        Wait.Until(TimeSpan.FromSeconds(5), () => outerCell.FindElement(By.XPath(".//input[contains(@class,'wj-grid-editor')]")).Displayed);
        var innerCell = outerCell.FindElement(By.XPath(".//input[contains(@class,'wj-grid-editor')]"));
        innerCell.InputText(value, false);
        innerCell.InputText(Keys.Enter, false);
    }

    public static void ClickCommand(IWebDriver driver, string gridName, string commandName)
    {
        var grid = GetGrid(driver, gridName);
        CommandHelper.ClickCommand(driver, grid, commandName);
    }

    public static IWebElement GetContextualGridButton(IWebDriver driver, string buttonName)
    {
        var button = driver.FindElement(By.Name(buttonName));
        return button;
    }

    public static IWebElement GetGrid(IWebDriver driver, string gridName = "")
    {
        driver.WaitForTransaction();

        if (string.IsNullOrEmpty(gridName))
        {
            return driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));
        }

        return driver.WaitUntilAvailable(
            By.XPath(AppElements.Xpath[AppReference.Entity.SubGridContents].Replace("[NAME]", gridName)));
    }
    
    /// <summary>
    /// Find the specified element 
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="formContext">The element on which to search.</param>
    /// <param name="subGridName">The unique name of the subgrid.</param>
    /// <returns>The first child element found within the context.</returns>
    public static IWebElement GetGrid(IWebDriver webDriver, IWebElement formContext, string subGridName)
    {
        IWebElement subGrid = null;

        try
        {
            subGrid = formContext.FindElement(By.XPath($".//div[@data-id = '{subGridName}']"));
        }
        catch (NoSuchElementException)
        {
            subGrid = formContext.FindElement(By.Id($"dataSetRoot_{subGridName}"));
        }

        return subGrid;
    }

    public static bool IsCellReadOnly(IWebDriver driver, string gridName, string headerName, int rowIndex)
    {
        var headerIndex = GetHeaderIndex(driver, gridName, headerName);
        var cells = GetCells(driver, gridName, rowIndex);
        var outerCell = cells[headerIndex];
        outerCell.Click();
        return outerCell.GetAttribute("aria-readonly") == "true";
    }

    public static int GetHeaderIndex(IWebDriver driver, string gridName, string headerName)
    {
        int result = -1;

        for (int loop = 0; loop < 4 && result < 0; loop++)
        {
            Policy
             .Handle<StaleElementReferenceException>()
             .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(1))
             .Execute(() =>
             {
                 var headers = GetHeaders(driver, gridName);
                 for (int i = 0; i < headers.Count; i++)
                 {
                     var x = headers[i].Text;
                     if (CheckHeaderMatched(headers[i], headerName))
                     {
                         result = i;
                         break;
                     }
                 }

                 if (result < 0)
                 {
                     // OK - maybe need to scroll the column into view (columns are created on demand)
                     var grid = GetGrid(driver, gridName);
                     var element = grid.FindElements(By.XPath(".//div[@wj-part='root']"));

                     if (element.Count == 1)
                     {
                         // scroll in units of 1000
                         ((IJavaScriptExecutor)driver).ExecuteScript($"arguments[0].scrollLeft = {loop * 1000}", element[0]);
                         System.Threading.Thread.Sleep(1000);
                     }
                 }
             });
        }

        if (result < 0)
        {
            throw new AutomationException($"Cannot find header with name '{headerName}' in grid '{gridName}'");
        }
        else
        {
            return result;
        }
    }

    /// <summary>
    /// Gets a list of cells for a row.
    /// </summary>
    /// <param name="rows">List of rows.</param>
    /// <param name="rowIndex">The index of the row to return cells for.</param>
    /// <returns>A list of cells.</returns>
    public static IList<IWebElement> GetCellsForRow(IList<IWebElement> rows, int rowIndex)
    {
        var row = rows[rowIndex];

        if (row.HasElement(By.ClassName("wj-cell")))
        {
            return rows[rowIndex].FindElements(By.ClassName("wj-cell"));
        }
        else if (row.HasElement(By.ClassName("ag-cell")))
        {
            return row.FindElements(By.ClassName("ag-cell"));
        }

        throw new NotFoundException("Unable to find cells for row");
    }

    public static void ValidateHeaderInCommodityLineForPotato()
    {
        var PotatoType = driver.FindElement(By.XPath("//*[@title='Potato Type']")).Displayed;
        if (driver.FindElement(By.XPath("//*[contains(@id,'dataSetRoot_Commoditylines')]")).Enabled)
        {
           //var PotatoType = driver.FindElement(By.XPath("//*[contains(@title,'Potato Type')]")).Displayed;
            
        }
    }

    /// <summary>
    /// Waits for rows to be present within subGrid.
    /// </summary>
    /// <param name="webDriver">WebDriver instance to interact.</param>
    /// <param name="subGridName">Unique name of the subGrid.</param>
    /// <param name="successCallback">Callback handler.</param>
    /// <param name="exceptionMessage">Friendly error message.</param>
    public static void WaitForRows(
        IWebDriver webDriver,
        string subGridName,
        Action<int> successCallback,
        string exceptionMessage)
    {
        webDriver.WaitForTransaction();
        var rows = GetRows(webDriver, subGridName);
        if (rows.IsEmpty())
        {
            Policy.Handle<Exception>()
                .WaitAndRetry(3, retryAttempt =>
                {
                    Console.WriteLine($"Refresh sub grid {subGridName} retry attempt {retryAttempt}");
                    return TimeSpan.FromSeconds(5);
                })
                .Execute(() =>
                {
                    GridHelper.ClickCommand(webDriver, subGridName, "Refresh");
                    rows = GetRows(webDriver, subGridName);
                    if (rows.IsEmpty())
                    {
                        throw new Exception(exceptionMessage);
                    }
                });
        }

        successCallback(rows.Count);
    }

    public static IList<IWebElement> GetCells(IWebDriver driver, string gridName, int rowIndex)
    {
        var rows = GetRows(driver, gridName);
        var cells = GetCellsForRow(rows, rowIndex);
        return cells;
    }

    private static bool CheckHeaderMatched(IWebElement header, string headerName)
    {
        var headerClasses = header.GetAttribute("class").Split(' ');

        if (headerClasses.Contains(GridHeaderClassPcf))
        {
            if (header.GetAttribute("col-id").Contains("__row_status"))
            {
                return false;
            }

            var rowContent = header.FindElement(By.ClassName("ms-Label"));
            if (rowContent != null && rowContent.Text != string.Empty && rowContent.Text.Equals(headerName))
            {
                return true;
            }
        }
        else if (headerClasses.Contains(GridHeaderClassWj))
        {
            if (header.GetAttribute("id").Contains("btnheaderselectcolumn"))
            {
                return false;
            }

            var headerTitle = header.GetAttribute("title");
            if (!string.IsNullOrWhiteSpace(headerTitle) && headerTitle.Equals(headerName))
            {
                return true;
            }
        }

        return false;
    }
}
