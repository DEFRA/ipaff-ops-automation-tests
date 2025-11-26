// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System.Linq;
using Reqnroll;

[Binding]
internal class GridColumnNamesSteps : PowerAppsStepDefiner
{
    // Deprecated for sub-grids - use the method in GridSteps which handles scrolling columns into view to make them accessible
    [Then(@"a grid column is displayed with a name '(.*)' in the '(.*)' grid column field")]
    public void ThenAGridColumnIsDisplayedWithANameInTheGridColumnField(string expectedGridColumnNameValue, string columnId)
    {
        var columnNameValue = this.GetGridColumnNameTextValue(columnId);

        columnNameValue.Should().Be(expectedGridColumnNameValue);
    }

    public string GetGridColumnNameTextValue(string columnTextName)
    {
        Driver.WaitForTransaction();

        if (!Driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Container]), out var grid))
        {
            throw new NotFoundException("Unable to find grid");
        }

        var viewport = grid.FindElement(By.ClassName("ag-center-cols-viewport"));

        IWebElement header = null;
        var difference = 0;
        do
        {
            header = grid
                .FindElements(By.CssSelector("div[class*=headerText] label"))
                .FirstOrDefault(e => e.GetAttribute("title") == columnTextName);

            if (header == null)
            {
                var scrollBefore = System.Convert.ToInt64(Driver.ExecuteScript("return arguments[0].scrollLeft", viewport));
                Driver.ExecuteScript("arguments[0].scrollLeft += arguments[0].offsetWidth", viewport);
                var scrollAfter = System.Convert.ToInt64(Driver.ExecuteScript("return arguments[0].scrollLeft", viewport));
                difference = (int)(scrollAfter - scrollBefore);

                if (difference != 0)
                {
                    // wait for async column load
                    Driver.WaitForTransaction();
                }
            }
        }
        while (header == null && difference != 0);

        if (header == null)
        {
            throw new NotFoundException($"A view header labelled {columnTextName} could not be found.");
        }

        return header.GetAttribute("title");
    }
}
