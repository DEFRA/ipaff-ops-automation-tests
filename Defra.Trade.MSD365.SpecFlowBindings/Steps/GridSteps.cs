// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Extensions;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Polly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Reqnroll;

[Binding]
public class GridSteps : PowerAppsStepDefiner
{
    private const string GridReadonlyTableColumnHeaderName = "Fields";
    private readonly SessionContext ctx;

    public GridSteps(SessionContext ctx)
    {
        this.ctx = ctx;
    }

    [When(@"I enter '(.*)' into the '(.*)' cell for row '(.*)' in the '(.*)' grid( after clearing the existing value| without clicking away|)")]
    public static void WhenIEnterIntoTheCellForRowInTheGrid(string value, string headerName, int rowIndex, string gridName, string additionalParameters)
    {
        GridHelper.InputValueIntoCell(Driver, gridName, headerName, rowIndex, value, additionalParameters?.IndexOf("after clearing") > 0, additionalParameters?.IndexOf("without clicking away") < 0);
    }
    [When(@"I enter '(.*)' into the '(.*)' cell for row '(.*)' in the '(.*)' grid( after clearing the existing value| without clicking away|)")]
    public static void WhenIEnterIntoTheCellForRowInTheSubGrid(string value, string headerName, int rowIndex, string gridName, string additionalParameters)
    {
        GridHelper.InputValueIntoCellOfSubgrid(Driver, gridName, headerName, rowIndex, value, additionalParameters?.IndexOf("after clearing") > 0, additionalParameters?.IndexOf("without clicking away") < 0);
    }

    [When(@"I enter the (\d[a-z]+) record on the (.*) grid as follows")]
    public static void WhenIEnterTheRecordOnTheGridAsFollows(string positionString, string gridName, Table table)
    {
        var position = new HumanReadableIntegerExpression(positionString);
        table.Rows.Count.Should().Be(1, "Expected only one record");
        for (var i = 0; i < table.Header.Count; i++)
        {
            WhenIEnterIntoTheCellForRowInTheGrid(table.Rows[0][i], table.Header.ToList()[i], position.Value, gridName, string.Empty);
        }

        SharedSteps.WaitForScriptProcessing();
    }

    [When(@"I enter today's date '(.*)' into the '(.*)' cell for row '(.*)' in the '(.*)' grid")]
    public static void WhenIEnterTodaysDateIntoTheCellForRowInTheGrid(int addDays, string headerName, int rowIndex, string gridName)
    {
        var date = GetDateToday(addDays);
        WhenIEnterIntoTheCellForRowInTheGrid(date, headerName, rowIndex, gridName, null);
    }

    [When(@"I enter a future date into the '(.*)' cell for row '(.*)' in the '(.*)' grid")]
    public void WhenIEnterAFutureDateIntoInTheGridCell(string headerName, int rowIndex, string gridName)
    {
        WhenIEnterTodaysDateIntoTheCellForRowInTheGrid(5, headerName, rowIndex, gridName);
    }

    [When(@"I enter a future date into the '(.*)' cell for row '(.*)' in the '(.*)' grid and accept the alert showing the message '(.*)'")]
    public void WhenIEnterAFutureDateIntoTheInThe(string headerName, int rowIndex, string gridName, string expectedMessageValue)
    {
        WhenIEnterTodaysDateIntoTheCellForRowInTheGrid(1, headerName, rowIndex, gridName);
        DialogSteps.ThenADialogIsDisplayedWithMessageInTheDialogField(expectedMessageValue, "dialogMessageText");
        DialogSteps.WhenIClickOKOnTheRejectDialog();
    }

    [Then(@"the '(.*)' cell for row '(.*)' in the '(.*)' grid has a value of '(.*)'")]
    public static void ThenTheCellForRowInTheGridHasAValueOf(string headerName, int rowIndex, string gridName, string expectedCellValue)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(30, retryAttempt => TimeSpan.FromSeconds(1))
            .Execute(() =>
            {
                var cellValue = GridHelper.GetValueOfCell(Driver, gridName, headerName, rowIndex);
                cellValue.Should().Be(expectedCellValue);
            });
    }

    [Then(@"'(.*)' cell for row '(.*)' in the '(.*)' grid has a value of '(.*)'")]
    public void ThenCellForRowInTheGridHasAValueOf(string qtyInspected, int rowIndex, string gridName, string qtyAppliedFor)
    {
        Policy
             .Handle<Exception>()
             .WaitAndRetry(30, retryAttempt => TimeSpan.FromSeconds(1))
             .Execute(() =>
             {
                 var qtyInspectedCellValue = GridHelper.GetValueOfCell(Driver, gridName, qtyInspected, rowIndex);
                 var qtyAppliedForCellValue = GridHelper.GetValueOfCell(Driver, gridName, qtyAppliedFor, rowIndex);

                 qtyInspectedCellValue.Should().Be(qtyAppliedForCellValue);
             });
    }

    [When(@"I '(.*)' the record to get the '(.*)' cell for row '(.*)' in the '(.*)' grid has a value of '(.*)'")]
    public static void WhenITheRecordToGetTheCellForRowInTheGridHasAValueOf(string command, string headerName, int rowIndex, string gridName, string expectedCellValue)
    {
        Policy
              .Handle<Exception>()
              .WaitAndRetry(30, retryAttempt => TimeSpan.FromSeconds(1))
              .Execute(() =>
              {
                  GridHelper.ClickCommand(Driver, gridName, command);
                  var cellValue = GridHelper.GetValueOfCell(Driver, gridName, headerName, rowIndex);
                  cellValue.Should().Be(expectedCellValue);
              });
    }

    [Then(@"the '(.*)' cell for row '(.*)' in the '(.*)' grid (contains|does not contain) data")]
    public static void ThenTheCellForRowInTheGridContainsData(string headerName, int rowIndex, string gridName, string expectedCellValue)
    {
        var cellValue = "";
        Policy
            .Handle<Exception>()
            .WaitAndRetry(SpecflowBindingsConstants.DefaultRetryCount,
             retryAttempt => TimeSpan.FromSeconds(SpecflowBindingsConstants.GridRetryInterval))
            .Execute(() =>
            {
                cellValue = GridHelper.GetValueOfCell(Driver, gridName, headerName, rowIndex);
            });
        if (expectedCellValue == "contains")
        {
            cellValue.Should().NotBeNull();
        }
        else
        {
            cellValue.Should().BeNull();
        }
    }

    [Then(@"I can see the '(.*)' view")]
    public static void ThenISeeView(string viewName)
    {
        var gridName = Driver.WaitUntilVisible(By.XPath("//span[contains(@id, 'ViewSelector') and contains(@id, 'text-value')]"));
        gridName.Text.Should().Be(viewName);
    }

    private static List<string> GetViewList()
    {
        Driver.ClickWhenAvailable(
            By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelector].Replace("//button", "//*")),
            2.Seconds(),
            "Unable to find the view selector.");

        Driver.WaitForTransaction();

        if (!Driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Grid.ViewContainer].Replace("//ul", string.Empty)), out var viewContainer))
        {
            throw new NotFoundException("Unable to find the view container.");
        }

        var gridViewsXPath = By.XPath(".//button[@data-value]//span[contains(@class, 'ms-ContextualMenu-itemText')]");
        var subgridViewsXpath = By.XPath("//li[@data-value and contains(@data-id, 'ViewSelector')]");

        if (viewContainer.HasElement(gridViewsXPath))
        {
            // Grid
            return viewContainer
                .FindElements(gridViewsXPath)
                .Select(e => e.Text)
                .ToList();
        }
        else if (viewContainer.HasElement(subgridViewsXpath))
        {
            // Related grid or subgrid
            return viewContainer
                .FindElements(subgridViewsXpath)
                .Select(e => e.Text)
                .ToList();
        }

        throw new NotFoundException("Unable to find views.");
    }

    [Then(@"I can see (|only )the following views in the View Selector")]
    public static void ThenISeeFollowingViews(string only, Table views)
    {
        GetViewSelectorLabels(
            (actualViews) =>
            {
                var viewNames = views.Rows.Select((row) => row.Values.First()).ToList();
                viewNames.ForEach(x => actualViews.Should().Contain(x));
                if (only == "only ")
                {
                    actualViews.Count.Should().Be(viewNames.Count);
                }
            },
            "View items cannot contain an empty string.");
    }

    [Then(@"I can see (|only )the following views in the view selector with the following headers(| in any order)")]
    public static void ThenISeeFollowingViewsWithFollowingColumns(string only, string inOrder, Table views)
    {
        var headersByView = views.Rows.ToDictionary(r => r.Values.First(), r => r.Values.Last().Split(','));
        var checkHeaderOrder = !string.IsNullOrEmpty(inOrder);
        var failOnAdditionalHeaders = !string.IsNullOrEmpty(only);

        GetViewSelectorLabels(
            (actualViews) =>
            {
                if (failOnAdditionalHeaders)
                {
                    actualViews.Count.Should().Be(headersByView.Count);
                }

                foreach (var expectedView in headersByView.Keys)
                {
                    actualViews.Should().Contain(expectedView);

                    if (!Driver.HasElement(By.XPath($".//h1[@title='{expectedView}']")))
                    {
                        XrmApp.Grid.SwitchView(expectedView);
                    }

                    GridHelper.ValidateHeaders(Driver, expectedView, headersByView[expectedView], checkHeaderOrder);
                }
            },
            "View items cannot contain an empty string.");
    }

    [Then(@"I can see a section title of '(.*)' for the (.*) subgrid")]
    public static void ThenISeeGridSectionTitle(string title, string subgrid)
    {
        var sections = Driver.FindElements(By.XPath(".//div[contains(@data-id,'dataSetRoot_" + subgrid + "')]/ancestor::section"));
        var actualTitle = sections.Where(x => x.GetAttribute("ariaLabel") != null).FirstOrDefault().GetAttribute("ariaLabel");
        actualTitle.Should().Be(title);
    }

    [Then(@"I can see a label of '(.*)' for the (.*) subgrid")]
    public static void ThenISeeGridLabel(string title, string subgrid)
    {
        var actualTitle = Driver.WaitUntilAvailable(By.XPath(".//div[contains(@data-id,'dataSetRoot_" + subgrid + "')]//h3")).Text;
        actualTitle.Should().Be(title);
    }

    [When(@"I select the record at position '(\d+)' in the '(.*)' subgrid")]
    public static void WhenISelectTheRecordAtPositionInTheSubgrid(int index, string subGridName)
    {
        XrmApp.Entity.SubGrid.HighlightRecord(subGridName, Driver, index);
    }

    [Then(@"the following errors have been displayed in the (\d[a-z]+) record on the (.*) grid")]
    public static void ThenTheFollowingErrorsHaveBeenDisplayedInTheRecordOnThePlantsInspectionResultsGrid(string positionString, string gridName, Table table)
    {
        var position = new HumanReadableIntegerExpression(positionString);
        AssertGridErrorMessages(table, gridName, position.Value);
    }

    [Then(@"the following values have been displayed in the (\d[a-z]+) record for the (.*) grid")]
    public static void ThenFollowingValuesHaveBeenDisplayedInTheFirstInspectionResults(string positionString, string gridName, Table table)
    {
        var position = new HumanReadableIntegerExpression(positionString);
        table.Rows.Count.Should().Be(1, "Expected only one record");
        for (var i = 1; i < table.Header.Count; i++)
        {
            ThenTheCellForRowInTheGridHasAValueOf(table.Header.ToList()[i], position.Value, gridName, table.Rows[0][i]);
        }
    }

    [Then(@"I can see '(.*)' has the following state")]
    public void ThenICanSeeHasTheFollowingState(string gridName, Table table)
    {
        var errors = new StringBuilder();
        foreach (var tableRow in table.Rows)
        {
            string expectedState = tableRow["Editable"].ToLower() == "false" ? "is" : "is not";
            try
            {
                CellIsOrNotReadOnly(0, tableRow[GridReadonlyTableColumnHeaderName], gridName, expectedState);
            }
            catch (AssertionFailedException ex)
            {
                errors.AppendLine(ex.Message);
            }
        }

        errors.ToString().Should().BeEmpty();
    }

    [Then(@"the following fields are locked in the (\d[a-z]+) record for the (.*) grid")]
    public static void ThenTheFollowingValuesAreLockedInTheStRecordForTheInspectionResultGrid(string positionString, string gridName, Table table)
    {
        var position = new HumanReadableIntegerExpression(positionString);
        table.Header.Contains(GridReadonlyTableColumnHeaderName).Should().BeTrue($"Expected header:{GridReadonlyTableColumnHeaderName} is missing ");
        table.Header.Count.Should().Be(1, "Expected only one header");
        foreach (var tableRow in table.Rows)
        {
            CellIsOrNotReadOnly(position.Value, tableRow[GridReadonlyTableColumnHeaderName], gridName, "is");
        }
    }

    [Then(@"the '(.*)' grid contains '(.*)' row\(s\)")]
    public static void ThenTheGridContainsRows(string gridName, int expectedRowCount)
    {
        Wait.Until(TimeSpan.FromSeconds(15), () => GridHelper.GetRows(Driver, gridName).Count.Equals(expectedRowCount));
        var rows = GridHelper.GetRows(Driver, gridName);

        rows.Count.Should().Be(expectedRowCount);
    }

    [When(@"I sort the '(.*)' column by '(.*)'")]
    public static void WhenISortGridColumnBy(string column, string sorter)
    {
        var header = GridHelper.FindHeader(Driver, column);
        header.Click();
        var sortButton = GridHelper.GetContextualGridButton(Driver, "Sort " + sorter);
        sortButton.Click();
    }

    [Then(@"the '(.*)' grid has a '(.*)' column")]
    public static void ThenTheGridContainsColumns(string gridName, string column)
    {
        Wait.Until(TimeSpan.FromSeconds(15), () => GridHelper.GetHeaders(Driver, gridName).Any(x => x.Text == column));
    }

    [Then(@"I can see the grid headers as follows (.*)")]
    public static void ThenICanSeeTheGridHeadersAsFollows(string[] expectedColumns)
    {
        // Replace spurious special characters before comparison
        var columns = (from col in Client.GetGridHeaders().Value select col.Replace("", string.Empty)).ToList();
        columns.Should().BeEquivalentTo(expectedColumns, options => options.WithStrictOrdering());
    }

    // Iterate and improve to test multiple views
    [Then(@"the current view has the following columns")]
    public static void ThenTheGridFollowingColumns(Table table)
    {
        var expectedColumns = table.Rows.Select(tableRow => tableRow["Header"]).ToList();
        Client.GetGridHeaders(string.Empty).Value.Should().BeEquivalentTo(expectedColumns, options => options.WithStrictOrdering());
    }

    [Then(@"the '(.*)' grid has following columns")]
    public static void ThenTheGridFollowingColumns(string gridName, Table table)
    {
        Driver.ScrollIntoView(GridHelper.GetGrid(Driver, gridName));
        var expectedColumns = table.Rows.Select(tableRow => tableRow["Header"]).ToList();
        Client.GetGridHeaders(gridName).Value.Should().BeEquivalentTo(expectedColumns, options => options.WithStrictOrdering());
    }

    [Then(@"row '(.*)' of the '(.*)' grid contains '(.*)'")]
    public static void ThenRowOfTheGridContains(int rowIndex, string gridName, string expectedValue)
    {
        Wait.Until(TimeSpan.FromSeconds(5), () => GridHelper.GetRows(Driver, gridName).Count > rowIndex);
        var rows = GridHelper.GetRows(Driver, gridName);
        var rowText = rows[rowIndex].Text;

        rowText.Contains(expectedValue);
    }

    [Then(@"row '(.*)' of the '(.*)' grid contains today's date '(.*)'")]
    public static void ThenRowOfTheGridContainsTodaysDate(int rowIndex, string gridName, int addDays)
    {
        var date = GetDateToday(addDays);
        ThenRowOfTheGridContains(rowIndex, gridName, date);
    }

    [When(@"I click the cell on row '(.*)' column '(.*)' of the '(.*)' subgrid")]
    public static void WhenIClickCell(int row, string column, string subgridName)
    {
        GridHelper.SelectCell(Driver, subgridName, column, row);
    }

    [When(@"I expand the dropdown for the cell in row '(.*)' column '(.*)' of the '(.*)' subgrid")]
    public static void WhenIExpandCellDropdown(int row, string column, string subgridName)
    {
        GridHelper.ShowCellDropdown(Driver, subgridName, column, row);
    }

    [Then(@"The displayed dropdown list (has|does not have) the following menuitem")]
    public static void ThenTheDisplayedDropdownListHasMenuitem(string shouldHave, Table menuitem)
    {
        List<string> menuList = menuitem.Rows.Select((row) => row.Values.First()).ToList();

        // if box is a menu and menuitem
        IWebElement dropDown;
        if (ElementHelper.IsElementPresent(Driver, By.XPath(".//div[@role='menu' and contains(@class, 'wj-dropdown-panel')]")))
        {
            dropDown = Driver.FindElement(By.XPath(".//div[@role='menu' and contains(@class, 'wj-dropdown-panel')]"));
        }
        else
        {
            dropDown = Driver.FindElement(By.XPath(".//div[contains(@class, 'wj-dropdown')]"));
        }

        var actualMenu = dropDown.FindElements(By.XPath(".//div[@class='wj-listbox-item']"));
        var actual = false;
        bool expected = shouldHave == "has";
        for (int i = 0; i < menuList.Count; i++)
        {
            actual = actualMenu.Where(x => x.Text == menuList[i]).Count() == 1;
            actual.Should().Be(expected);
        }
    }

    [Then(@"The displayed dropdown list (has|does not have) the following options")]
    public static void DisplayedDropdownHasOptions(string hasOrDoesNotHave, Table options)
    {
        var expectedOptions = options.Rows.Select((row) => row.Values.First()).ToArray();

        Policy
            .Handle<Exception>()
            .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var isListBox = ElementHelper.IsElementPresent(Driver, By.XPath(".//div[@role='listbox' and contains(@class, 'wj-dropdown-panel')]"));
                var xPath = isListBox ? By.XPath(".//div[@role='listbox' and contains(@class, 'wj-dropdown-panel')]") : By.XPath(".//div[contains(@class, 'wj-dropdown')]");
                var dropDown = Driver.FindElement(xPath);
                var actualOptions = dropDown.FindElements(By.XPath(".//div[@class='wj-listbox-item']/div/div")).Select(o => o.Text).ToArray();

                if (!actualOptions.Any())
                {
                    actualOptions = dropDown.FindElements(By.XPath(".//div[@class='wj-listbox-item']")).Select(o => o.Text).ToArray();
                }

                var expectedToHave = hasOrDoesNotHave == "has";
                if (expectedToHave)
                {
                    actualOptions.Should().Contain(expectedOptions);
                }
                else
                {
                    actualOptions.Should().NotContain(expectedOptions);
                }
            });
    }

    [When(@"I enter '(.*)' into the '(.*)' cell for all rows in the '(.*)' grid")]
    public static void WhenIEnterIntoTheCellForAllRowsInTheGrid(string value, string headerName, string gridName)
    {
        for (int i = 0, ic = GridHelper.GetRows(Driver, gridName).Count; i < ic; i++)
        {
            GridHelper.InputValueIntoCell(Driver, gridName, headerName, i, value);
        }
    }

    [When(@"I enter '(.*)' into the '(.*)' option set for row '(.*)' in the '(.*)' grid")]
    public static void WhenIEnterIntoTheOptionSetForRowInTheGrid(string value, string headerName, int rowIndex, string gridName)
    {
        GridHelper.InputOptionSetValueIntoCell(Driver, gridName, headerName, rowIndex, value);
    }

    [Then(@"the '(.*)' grid is displayed on the form")]
    public static void ThenSubgridIsDisplayed(string gridName)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var grid = GridHelper.GetGrid(Driver, gridName);
                grid.Should().NotBeNull();
            });
    }

    [Then(@"the '(.*)' cell for row '(.*)' in the '(.*)' grid does NOT have a value of today's date '(.*)'")]
    public static void ThenTheCellForRowInTheGridDoesNotHaveAValueOfTodaysDate(string headerName, int rowIndex, string gridName, int addDays)
    {
        var date = GetDateToday(addDays);
        var cellValue = GridHelper.GetValueOfCell(Driver, gridName, headerName, rowIndex);
        cellValue.Should().NotBe(date);
    }

    [When(@"I select all rows in the '(.*)' grid")]
    public static void WhenISelectAllRowsInTheGrid(string gridName)
    {
        Driver.WaitForTransaction();

        if (!Driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridContents].Replace("[NAME]", gridName)), out var grid))
        {
            throw new NotFoundException($"Unable to find the {gridName} subgrid.");
        }

        if (grid.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridHeadersEditable])))
        {
            grid.ClickWhenAvailable(
                By.XPath(".//div[@data-id='btnheaderselectcolumn']//button"),
                "Unable to click 'Select all' button.");
        }
        else
        {
            GridHelper.SelectAllRows(Driver, gridName);
        }
    }

    [When("I select all rows in the grid")]
    public static void WhenISelectAllRowsInTheGridPage()
    {
        GridHelper.SelectAllRows(Driver);
    }

    [When(@"I select '(a single commodity|all commodities)' and system changes the value when toggle Include on Phyto")]
    public void WhenISelectTheSingleCommodityAndSystemChangesTheValueWhenToggleIncludeOnPhyto(string option)
    {
        const string gridName = "subgrid_selected_commodities";
        SelectCommodityGrid(option, gridName);
        EntitySubGridSteps.ThenICanSeeTheCommandOnTheSubgrid("Excl/Incl on Phyto", gridName);
        EntitySubGridSteps.WhenISelectTheCommandOnTheSubgrid("Excl/Incl on Phyto", gridName);
        ThenTheCellForRowInTheGridHasAValueOf("Include on Phyto", 0, gridName, "No");
        SelectCommodityGrid(option, gridName);
        EntitySubGridSteps.ThenICanSeeTheCommandOnTheSubgrid("Excl/Incl on Phyto", gridName);
        EntitySubGridSteps.WhenISelectTheCommandOnTheSubgrid("Excl/Incl on Phyto", gridName);
        ThenTheCellForRowInTheGridHasAValueOf("Include on Phyto", 0, gridName, "Yes");
    }

    [When(@"I select row '(.*)' in the '(.*)' grid")]
    public static void WhenISelectRowInTheGrid(int rowIndex, string gridName)
    {
        GridHelper.WaitForRows(
            Driver,
            gridName,
            (rowCount) =>
            {
                GridHelper.SelectRow(Driver, gridName, rowIndex);
            },
            $"SubGrid {gridName} contains no rows.");
    }

    [When(@"I select row '(.*)' and click '(.*)' in the '(.*)' grid")]
    public static void WhenISelectRowInTheGridAndClickCommand(int rowIndex, string commandName, string gridName)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                try
                {
                    if (rowIndex >= 0)
                    {
                        GridHelper.SelectRow(Driver, gridName, rowIndex);
                    }
                    else
                    {
                        GridHelper.SelectAllRows(Driver, gridName);
                    }

                    Driver.WaitForTransaction();
                    EntitySubGridSteps.WhenISelectTheCommandOnTheSubgrid(commandName, gridName);
                    Driver.WaitForTransaction();
                }
                catch (Exception)
                {
                    CommandSteps.WhenISelectTheCommand("Refresh");
                    throw;
                }
            });
    }

    [When(@"I deselect row '(.*)' in the '(.*)' grid")]
    public static void WhenIDeSelectRowInTheGrid(int rowIndex, string gridName)
    {
        GridHelper.SelectRow(Driver, gridName, rowIndex);
    }

    [When(@"I click command '(.*)' in the '(.*)' subgrid")]
    public static void WhenIClickCommandInTheSubgrid(string commandName, string gridName)
    {
        var subGrid = GridHelper.GetGrid(Driver, gridName);
        CommandHelper.ClickCommand(Driver, subGrid, commandName);
    }
    
    [When(@"I click can|cannot see the command '(.*)' in the '(.*)' subgrid")]
    public static void WhenICanSeeCommandInTheSubgrid(string canOrCannot, string commandName, string gridName)
    {
        var subGrid = GridHelper.GetGrid(Driver, gridName);
        var commandButtons = CommandHelper.GetCommands(Driver, subGrid);
        var commandButton = commandButtons.FirstOrDefault(d => d.Key == commandName).Value;
        if (canOrCannot.ToUpper().Equals("CAN"))
        {
            commandButton.IsVisible().Should().BeTrue();
        }
        else
        {
            commandButton.IsVisible().Should().BeFalse();
        }
    }

    [Then(@"I can see the following headers on the grid view(| in any order)")]
    public static void ThenICanSeeTheFollowingHeadersInTheGridView(string inOrder, Table expectedHeadersTable)
    {
        Driver.WaitForTransaction();

        if (!Driver.TryFindElement(By.CssSelector("div[data-type=grid]"), out var grid))
        {
            throw new NotFoundException("Unable to find the grid");
        }

        var viewport = grid.FindElement(By.ClassName("ag-center-cols-viewport"));
        var difference = 0;
        do
        {
            var scrollBefore = (long)Driver.ExecuteScript("return arguments[0].scrollLeft", viewport);
            Driver.ExecuteScript("arguments[0].scrollLeft += arguments[0].offsetWidth", viewport);
            var scrollAfter = (long)Driver.ExecuteScript("return arguments[0].scrollLeft", viewport);

            difference = (int)(scrollAfter - scrollBefore);
        } while (difference != 0);

        var actualHeaders = grid.FindElements(By.CssSelector("div[class*=headerText] label")).Select(e => e.GetAttribute("title"));
        if (!actualHeaders.Any())
        {
            throw new NotFoundException("Unable to find any headers on the grid");
        }

        var expectedHeaders = expectedHeadersTable.Rows.Select(r => r[0]);
        if (inOrder == "in any order")
        {
            actualHeaders.Should().BeEquivalentTo(expectedHeaders);
        }
        else
        {
            actualHeaders.Should().Equal(expectedHeaders);
        }
    }

    [Then(@"the number of rows in the '(.*)' grid is (\d+)")]
    public static void CheckGridRowCount(string gridName, int expected)
    {
        if (expected > 0)
        {
            GridHelper.WaitForRows(
            Driver,
            gridName,
            (rowCount) =>
            {
                rowCount.Should().Be(expected);
            },
            $"SubGrid {gridName} contains no rwos.");
        }
        else
        {
            var rowCount = GridHelper.GetRows(Driver, gridName).Count;
            rowCount.Should().Be(expected);
        }
    }

    [Then(@"the number of rows in the grid is (\d+)")]
    public static void CheckGridRowCount(int expected)
    {
        var noOfRows = XrmApp.Grid.GetGridItems().Count;
        noOfRows.Should().Be(expected);
    }

    [Then(@"I can see a column titled '(.*)' in the '(.*)' subgrid")]
    public static void CheckColumnExists(string colName, string gridName)
    {
        var headerIndex = GridHelper.GetHeaderIndex(Driver, gridName, colName);

        headerIndex.Should().BeGreaterOrEqualTo(0);
    }

    [Then(@"I can not see a column titled '(.*)' in the '(.*)' subgrid")]
    public static void CheckColumnDoesNotExist(string colName, string gridName)
    {
        Action getHeaderIndex = () => GridHelper.GetHeaderIndex(Driver, gridName, colName);

        getHeaderIndex.Should().Throw<AutomationException>();
    }

    [Then(@"I can see refresh button on displayed on the grid")]
    public static void ThenICanSeeRefreshButtonOnDisplayedOnTheGrid()
    {
        var button = Driver.FindElement(By.XPath("//*[contains(@id,'msdyn_workorder|NoRelationship|Form|Mscrm.Modern.refreshCommand72')]/button"));

        button.IsVisible().Should().BeTrue();
    }

    [When(@"I select '(.*)' cell for row '(.*)' in the '(.*)' grid and click the lookup button")]
    public static void WhenISelectTheCellForRowInTheGridAndClickTheLookupButton(string headerName, int rowIndex, string gridName)
    {
        GridHelper.SelectCellAndClickTheLookupNavigationButton(Driver, gridName, headerName, rowIndex);
    }

    [When(@"I search a value '(.*)' in the '(.*)' subgrid")]
    public static void WhenISearchAValue(string searchValue, string gridName)
    {
        XrmApp.Entity.SubGrid.Search(gridName, searchValue);
    }

    [When("I wait up to (.*) seconds for the '(.*)' subgrid to be populated, scrolling to the '(.*)' section, refreshing periodically")]
    public static void WaitForGridRecords(int seconds, string subGridName, string section)
    {
        Wait.Until(TimeSpan.FromSeconds(seconds), () =>
        {
            FormSteps.WhenIScrollToLabel(section);

            if (GridHelper.GetRows(Driver, subGridName).Count > 0)
            {
                return true;
            }

            CommandSteps.WhenISelectTheCommand("Refresh");

            return false;
        });
    }

    [Then(@"The cell at row '(.*)' column '(.*)' in the '(.*)' subgrid (is|is not) read only")]
    public static void CellIsOrNotReadOnly(int row, string column, string gridName, string readOnlyStr)
    {
        var expected = readOnlyStr == "is";
        var isReadOnly = GridHelper.IsCellReadOnly(Driver, gridName, column, row);
        isReadOnly.Should().Be(expected, $"Grid: {gridName} Field: {column}");
    }

    [Then(@"row '(.*)' column '(.*)' of the '(.*)' grid contains the value of variable '(.*)'")]
    public void ThenRowOfTheSubGridContainsVariable(int rowIndex, string columnName, string gridName, string expectedValue)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(4, retryAttempt => TimeSpan.FromSeconds(10))
            .Execute(() =>
            {
                var cellValue = GridHelper.GetValueOfCell(Driver, gridName, columnName, rowIndex);
                try
                {
                    cellValue.Should().Be(this.ctx.GetVariable<string>(expectedValue), $"GridRowIndex: {rowIndex}, HeaderName: {columnName}, GridName {gridName}, Expected Value: {expectedValue}");
                }
                catch
                {
                    EntitySubGridSteps.WhenISelectTheCommandOnTheSubgrid("Refresh", gridName);
                    throw;
                }
            });
    }

    [When(@"I store the value in row '(.*)' column '(.*)' of the '(.*)' subgrid in variable '(.*)'")]
    public void WhenIStoreGridValue(int row, string column, string subgridName, string variableName)
    {
        var cellValue = GridHelper.GetValueOfCell(Driver, subgridName, column, row);
        this.ctx.SetVariable(variableName, cellValue);
    }

    [When(@"I search a value from variable '(.*)' in the grid")]
    public void WhenISearchAVariableValue(string variableName)
    {
        XrmApp.Grid.Search(this.ctx.GetVariable(variableName)?.ToString() ?? string.Empty);
    }

    private static string GetDateToday(int addDays)
    {
        return DateTime.UtcNow.Date.AddDays(addDays).ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);
    }

    private static void AssertGridErrorMessages(Table table, string gridName, int recordIndex)
    {
        var actualErrors = table.Rows.Select(r => GridHelper.GetGridErrorMessage(Driver, gridName, r["Field"], recordIndex)).ToList();
        var expectedErrors = table.Rows.Select(r => r["ErrorMessage"]).ToList();

        actualErrors.Should().BeEquivalentTo(expectedErrors);
    }

    public static void SelectCommodityGrid(string option, string gridName)
    {
        switch (option.ToLower())
        {
            case "all commodities":
                WhenISelectAllRowsInTheGrid(gridName);
                break;
            case "a single commodity":
                WhenISelectRowInTheGrid(0, gridName);
                break;
            default:
                throw new NotImplementedException("Unexpected option");
        }
    }

    [Scope(Tag = "Pheats")]
    [When(@"I open the record at position '(\d+)' in the grid")]
    [When(@"I open the (\d+(?:(?:st)|(?:nd)|(?:rd)|(?:th))) record in the grid")]
    public static void WhenIOpenTheRecordAtPositionInTheGrid(int index)
    {
        XrmApp.Grid.OpenRecord(index);

        Driver.WaitForTransaction();
    }

    [Scope(Tag = "Trade")]
    [When(@"I open the record at position '(\d+)' in the '(.*)' subgrid")]
    public static void ClickTheGridBeforeSelection(int index, string gridName)
    {
        GridHelper.WaitForRows(
            Driver,
            gridName,
            (rowCount) =>
            {
                XrmApp.Entity.SubGrid.HighlightRecord(gridName, Driver, index);
                XrmApp.Entity.SubGrid.OpenSubGridRecord(gridName, index);
            },
            $"SubGrid {gridName} contains no rows.");
    }

    [Scope(Tag = "Trade")]
    [When(@"I open the record at position '(\d+)' in the '(.*)' editablesubgrid")]
    public static void OpenRecordFromSubGrid(int index, string gridName)
    {
        GridHelper.WaitForRows(
            Driver,
            gridName,
            (rowCount) =>
            {
                XrmApp.Entity.SubGrid.HighlightRecord(gridName, Driver, index);
                XrmApp.Entity.SubGrid.OpenEditableSubGridRecord(Driver, gridName, index);
            },
            $"SubGrid {gridName} contains no rows.");
    }

    [Scope(Tag = "Trade")]
    [When(@"I click the '([^']+)' command under the '([^']+)' flyout on the '([^']+)' subgrid")]
    public static void WhenIClickTheCommandUnderTheFlyoutOnTheSubgrid(string commandName, string flyoutName, string subGridName)
    {
        var grid = GridHelper.GetGrid(Driver, subGridName);
        CommandHelper.ClickCommand(Driver, grid, flyoutName, commandName);
    }

    /// <summary>
    /// Selects a row within the subgrid on a model form.
    /// </summary>
    /// <param name="rowIndex">The index of the row to select.</param>
    /// <param name="subGridName">The unique name of the subgrid.</param>
    /// <param name="formDisplayName">The display name of the modal form.</param>
    [Given(@"I select the '(.*)' record in the '(.*)' subgrid on the '(.*)' modal form")]
    [When(@"I select the '(.*)' record in the '(.*)' subgrid on the '(.*)' modal form")]
    public void GivenISelectTheRecordInTheSubgridOnTheModalForm(int rowIndex, string subGridName, string formDisplayName)
    {
        GridHelper.WaitForRows(
            Driver,
            subGridName,
            (rowCount) =>
            {
                GridHelper.SelectRow(Driver, subGridName, rowIndex);
            },
            $"SubGrid {subGridName} contains no rows.");
    }

    /// <summary>
    /// Clicks the specified command within a subgrid.
    /// </summary>
    /// <param name="commandName">The display name of the command.</param>
    /// <param name="subGridName">The unique name of the subgrid.</param>
    /// <param name="formDisplayName">The display name of the modal form.</param>
    [When(@"I click the '(.*)' command on the '(.*)' subgrid on the '(.*)' modal form")]
    public void WhenIClickTheCommandOnTheSubgridOnTheModalForm(string commandName, string subGridName, string formDisplayName)
    {
        var subGrid = GridHelper.GetGrid(Driver, subGridName);
        CommandHelper.ClickCommand(Driver, subGrid, commandName);
    }

    /// <summary>
    /// Switches view in a grid.
    /// </summary>
    /// <param name="viewName">The name of the view.</param>
    [Scope(Tag = "Trade")]
    [When(@"I switch to the '(.*)' view in the grid")]
    public static void WhenISwitchToTheViewInTheGrid(string viewName)
    {
        SwitchView(viewName);
    }

    [Scope(Tag = "Trade")]
    [Then(@"I can see (exactly|more than|less than) (\d+) records in the '(.*)' subgrid")]
    public static void ThenICanSeeRecordsInTheSubgrid(string compare, int count, string subGridName)
    {
        var subGridRows = GridHelper.GetRows(Driver, subGridName);

        switch (compare)
        {
            case "exactly":
                subGridRows.Count.Should().Be(count);
                break;
            case "more than":
                subGridRows.Count.Should().BeGreaterThan(count);
                break;
            case "less than":
                subGridRows.Count.Should().BeLessThan(count);
                break;
        }
    }

    [Then(@"the grid column '(.*)' is sorted by (ascending|descending)")]
    public void ThenTheGridColumnIsSortedBy(string column, string expectedSortOrder)
    {
        Driver.WaitForTransaction();

        if (!Driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Container]), out var grid))
        {
            throw new NotFoundException("Unable to find the grid.");
        }

        if (!grid.TryFindElement(By.XPath($"//div[@role='columnheader' and .//label[@title='{column}']]"), out var header))
        {
            throw new NotFoundException($"Unable to find {column} header.");
        }

        header.GetAttribute("aria-sort").Should().Be(expectedSortOrder);
    }

    private static void GetViewSelectorLabels(Action<List<string>> successCallback, string exceptionMessage)
    {
        List<string> items = new List<string>();
        var wait = new WebDriverWait(Driver, new TimeSpan(0, 0, 10));

        wait.Until(d =>
        {
            items = GetViewList();
            if (items.Contains(string.Empty))
            {
                // Collapse the View Selector flyout
                d.ClickIfVisible(By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelector].Replace("//button", "//*")));
                return false;
            }

            return true;
        });

        successCallback(items);
    }

    private static void SwitchView(string viewName)
    {
        Driver.WaitForTransaction();

        Driver.ClickWhenAvailable(
            By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelector].Replace("//button", "//*")),
            2.Seconds(),
            "Unable to find the view selector.");
        Driver.WaitForTransaction();

        if (!Driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Grid.ViewContainer].Replace("//ul", string.Empty)), out var viewContainer))
        {
            throw new NotFoundException("Unable to find the view container.");
        }

        if (!viewContainer.TryFindElement(By.XPath($"//li[contains(@class, 'ms-ContextualMenu-item')]//span[text()='{viewName}']"), out var view))
        {
            throw new NotFoundException($"Unable to find the view '{viewName}'.");
        }

        view.Click();
        Driver.WaitForTransaction();
    }
}
