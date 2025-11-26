

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Reqnroll;

[Binding]
public class EditableGridSteps : PowerAppsStepDefiner
{
    private readonly SessionContext _sessionContext;

    public EditableGridSteps(SessionContext sessionContext)
    {
        this._sessionContext = sessionContext;
    }

    [When(@"I open the record at position '(\d+)' in a modal form from the '(.*)' editablegrid")]
    [When(@"I open the '(.*)' record in a modal form from the '(.*)' editablegrid")]
    public void WhenIOpenTheRecordInAModalFormFromTheEditablegrid(int index, string subGridName)
    {
        var subGrid = GridHelper.GetGrid(Driver, subGridName);
        Driver.ScrollIntoView(subGrid);

        var subGridRows = GridHelper.GetRows(Driver, subGridName);

        if (subGridRows.IsEmpty())
        {
            Policy
            .Handle<NoSuchElementException>()
            .WaitAndRetry(5, retryCount => 5.Seconds())
            .Execute(() =>
            {
                CommandHelper.ClickCommand(Driver, subGrid, "Refresh");
                Driver.WaitForTransaction();

                subGridRows = GridHelper.GetRows(Driver, subGridName);
                if (subGridRows.IsEmpty())
                {
                    throw new NoSuchElementException($"The editable grid '{subGridName}' contains no rows.");
                }
            });
        }

        var subGridRow = subGridRows.ToList()[index];
        if (subGridRow.IsClickable())
        {
            // To avoid duplicate modal forms; always click on the second column
            var subGridCells = subGridRow.FindElements(By.XPath("./div[contains(@role, 'gridcell')]"));
            subGridCells[1].Click();
            Driver.WaitForTransaction();
        }
    }

    [When(@"I click the '(.*)' command on the '(.*)' editablegrid")]
    public void WhenIClickTheCommandOnTheEditableSubgrid(string commandName, string subGridName)
    {
        var subGrid = GridHelper.GetGrid(Driver, subGridName);
        CommandHelper.ClickCommand(Driver, subGrid, commandName);
    }

    [Then(@"I can see the '(.*)' editable grid")]
    public void ThenICanSeeTheEditableGrid(string subGridName)
    {
        var editableSubGrid = Driver.WaitUntilVisible(By.Id($"dataSetRoot_{subGridName}"));
        editableSubGrid.IsVisible().Should().BeTrue(because: "the editable grid is visible");
    }

    [When(@"I click the '([^']+)' flyout on the '([^']+)' editable grid")]
    public static void WhenIClickTheFlyoutOnTheEditableGrid(string flyoutName, string controlName)
    {
        var flyoutCommand = CommandHelper.GetFlyoutCommand(Driver, flyoutName);
        flyoutCommand.Click();
    }

    [When(@"I click '(.*)' command in the '(.*)' editable grid")]
    public void WhenIClickCommandInTheEditableGrid(string commandName, string controlName)
    {
        GridHelper.ClickCommand(Driver, controlName, commandName);
    }

    [When(@"I click the '(.*)' command under the '(.*)' flyout on '(.*)' editable grid")]
    public void WhenIClickTheCommandUnderTheFlyoutOnEditableGrid(string commandName, string flyoutName, string controlName)
    {
        var flyoutCommand = CommandHelper.GetFlyoutCommand(Driver, flyoutName);
        var subCommands = CommandHelper.GetFlyoutCommands(Driver, flyoutCommand);
        subCommands.Should().NotBeEmpty();

        var subCommand = subCommands.Where(d => d.Key == commandName).Select(d => d.Value).Single();
        subCommand.Click();
        Driver.WaitForTransaction();
    }

    [When(@"I click the save button within the '(.*)' editable grid")]
    public void WhenIClickTheSaveButtonWithinTheGrid(string subGridName)
    {
        var editableSubGrid = GridHelper.GetGrid(Driver, subGridName);
        var saveButton = editableSubGrid.FindElement(By.XPath(".//button[@title='Save']"));
        saveButton.Click();
        Driver.WaitForTransaction();
    }

    [Then(@"I should see (exactly|more than|less than) (\d+) records in the '(.*)' editablegrid")]
    public void ThenIShouldSeeRecordsInTheSubgrid(string compare, int count, string subGridName)
    {
        var editableSubGrid = GridHelper.GetGrid(Driver, subGridName);
        var subGridRows = GridHelper.GetRows(editableSubGrid);

        subGridRows.Should().NotBeEmpty();
        subGridRows.ToArray().Length.Should().Be(count);
    }

    [Then(@"the '(.*)' editablegrid contains a record with the '(.*)' field with a value matching the pattern '(.*)'")]
    public void ThenTheEditablegridContainsARecordWithTheFieldWithAValueMatchingThePattern(string subGridName, string fieldName, string pattern)
    {
        var editableGrid = GridHelper.GetGrid(Driver, subGridName);
        editableGrid.Should().NotBeNull();
        Driver.ScrollIntoView(editableGrid);

        var gridItems = GridHelper.GetGridItems(editableGrid);
        gridItems.Should().NotBeEmpty();

        var expression = new Regex($"{pattern}");
        gridItems.ForEach(item => {
            var value = item.GetAttribute<string>(fieldName);
            if (!string.IsNullOrEmpty(value) && value != "---")
            {
                var match = expression.Match(value);
                match.Success.Should().BeTrue();
            }
        });
    }

    [Then(@"I can see the update error '(.*)' within the '(.*)' editable grid")]
    public void ThenICanSeeTheUpdateError(string updateError, string subGridName)
    {
        var editableGrid = GridHelper.GetGrid(Driver, subGridName);
        editableGrid.Should().NotBeNull();
        Driver.ScrollIntoView(editableGrid);

        var alertElement = editableGrid.FindElement(By.XPath(".//div[@role='alert']"));
        alertElement.Text.Should().Be(updateError);
    }
}
