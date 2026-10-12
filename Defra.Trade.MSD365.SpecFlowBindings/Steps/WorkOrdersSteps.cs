// <copyright file="WorkOrdersSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.MSD365.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Linq;

/// <summary>
/// Step bindings relating to the Work Orders grid page.
/// </summary>
[Binding]
public sealed class WorkOrdersSteps : PowerAppsStepDefiner
{
    private const string ApplicationReferenceColumnId = "trd_applicationreference";
    private const string WorkOrderTypeColumnId = "msdyn_workordertype";

    private readonly ScenarioContext scenarioContext;

    public WorkOrdersSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Switches the Work Orders grid to the specified view.
    /// </summary>
    /// <param name="viewName">The name of the view to switch to e.g. 'All Work Orders - Exports'.</param>
    [When(@"I change the Work Orders view to '(.*)'")]
    public void WhenIChangeTheWorkOrdersViewTo(string viewName)
    {
        Driver.WaitForTransaction();

        XrmApp.Grid.SwitchView(viewName);

        Driver.WaitForTransaction();
    }

    [Then(@"the '(.*)' view is displayed on the Work Orders page")]
    public void ThenTheViewIsDisplayedOnTheWorkOrdersPage(string expectedViewName)
    {
        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");

        var viewSelectorButton = Driver.WaitUntilAvailable(
            By.XPath("//button[contains(@data-id,'ViewSelector') and not(contains(@data-id,'ViewSelector_1'))]"),
            "View selector button could not be found.");

        var actualViewName = viewSelectorButton.GetAttribute("aria-label")?.Trim() ?? string.Empty;

        actualViewName.Should().Be(expectedViewName,
            $"Expected view to be '{expectedViewName}' but found '{actualViewName}'");
    }

    [When("I search Work Orders for the application created in the Exporter Portal")]
    public void WhenISearchWorkOrdersForTheApplicationCreatedInTheExporterPortal()
    {
        var aphaReference = GetAphaReference();

        var searchBox = Driver.WaitUntilAvailable(
            By.XPath("//input[@aria-label='Work Order Filter by keyword']"),
            "Work Order 'Filter by keyword' search box could not be found.");

        searchBox.Click();
        searchBox.SendKeys(Keys.Control + "a");
        searchBox.SendKeys(Keys.Delete);
        searchBox.SendKeys(aphaReference);
        searchBox.SendKeys(Keys.Enter);

        Driver.WaitForTransaction();
    }

    [Then("the work order submitted in the Exporter Portal should be returned")]
    public void ThenTheWorkOrderSubmittedInTheExporterPortalShouldBeReturned()
    {
        var expectedAphaReference = GetAphaReference();

        // Retry to handle the delay between the Exporter Portal submission and the Work Order
        // landing in Dynamics — the footer row count indicates the grid has finished refreshing.
        Policy
            .Handle<Exception>()
            .OrResult<int?>(count => count == null || count != 1)
            .WaitAndRetry(
                retryCount: 20,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(30),
                onRetry: (_, _, _, _) =>
                {
                    XrmApp.Grid.ClearSearch();
                    Driver.WaitForTransaction();
                    XrmApp.Grid.Search(expectedAphaReference);
                    Driver.WaitForTransaction();
                })
            .Execute(() =>
            {
                Driver.WaitForTransaction();
                return GetGridRowCountFromFooter();
            });

        Driver.WaitForTransaction();

        var rowCount = GetGridRowCountFromFooter() ?? 0;

        rowCount.Should().Be(1,
            $"Expected exactly 1 Work Order for APHA reference '{expectedAphaReference}' but found {rowCount}.");

        string actualReference = null;

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var referenceCell = Driver.WaitUntilAvailable(
                    By.XPath($"//div[@class='ag-center-cols-container']//div[@role='row'][1]//div[@col-id='{ApplicationReferenceColumnId}']"),
                    "Application reference cell could not be found in the Work Orders grid.");

                actualReference = referenceCell.Text.Trim();
            });

        actualReference.Should().Be(expectedAphaReference,
            $"Expected Application reference '{expectedAphaReference}' but found '{actualReference}'.");
    }

    [Then("the Application Type is set to Pre-Export Application")]
    public void ThenTheApplicationTypeIsSetToPreExportApplication()
    {
        const string expectedApplicationType = "Pre-Export Application";

        Driver.WaitForTransaction();

        ScrollGridUntilColumnVisible(WorkOrderTypeColumnId);

        string actualApplicationType = null;

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var typeCell = Driver.WaitUntilAvailable(
                    By.XPath($"//div[contains(@class,'ag-center-cols-container')]//div[@role='row'][1]//div[@col-id='{WorkOrderTypeColumnId}']"),
                    "Application Type cell could not be found in the Work Orders grid.");

                actualApplicationType = typeCell.Text.Trim();
            });

        actualApplicationType.Should().Be(expectedApplicationType,
            $"Expected Application Type to be '{expectedApplicationType}' but found '{actualApplicationType}'.");
    }

    /// <summary>
    /// Horizontally scrolls the Work Orders AG grid until a cell for the given col-id is rendered.
    /// AG grid virtualises columns, so cells outside the viewport are not present in the DOM.
    /// </summary>
    /// <param name="colId">The col-id of the column that must be rendered.</param>
    private void ScrollGridUntilColumnVisible(string colId)
    {
        var findAndCheckScript = $@"
            var viewport = document.querySelector('.ag-body-horizontal-scroll-viewport')
                || document.querySelector('.ag-center-cols-viewport');
            if (!viewport) return null;
            var visible = document.querySelector('.ag-center-cols-container div[role=""row""] div[col-id=""{colId}""]') !== null;
            return JSON.stringify({{
                scrollLeft: viewport.scrollLeft,
                scrollWidth: viewport.scrollWidth,
                clientWidth: viewport.clientWidth,
                visible: visible
            }});";

        const string scrollIncrementScript = @"
            var viewport = document.querySelector('.ag-body-horizontal-scroll-viewport')
                || document.querySelector('.ag-center-cols-viewport');
            if (viewport) { viewport.scrollLeft += 300; }";

        const int gridReadyAttempts = 20;
        const int maxScrollAttempts = 40;

        string resultJson = null;
        for (var waitAttempt = 0; waitAttempt < gridReadyAttempts; waitAttempt++)
        {
            resultJson = Driver.ExecuteScript(findAndCheckScript) as string;

            if (resultJson != null)
            {
                break;
            }

            Driver.WaitForTransaction();
        }

        if (resultJson == null)
        {
            throw new InvalidOperationException(
                $"The Work Orders grid scroll container could not be found after waiting " +
                $"{gridReadyAttempts} attempts. The grid may not have finished rendering.");
        }

        for (var scrollAttempt = 0; scrollAttempt < maxScrollAttempts; scrollAttempt++)
        {
            if (resultJson.Contains(@"""visible"":true"))
            {
                return;
            }

            Driver.ExecuteScript(scrollIncrementScript);
            Driver.WaitForTransaction();

            resultJson = Driver.ExecuteScript(findAndCheckScript) as string
                ?? throw new InvalidOperationException(
                    "The Work Orders grid scroll container disappeared during scrolling.");
        }

        throw new InvalidOperationException(
            $"Could not scroll the Work Orders grid to show column '{colId}' after {maxScrollAttempts} scroll attempts.");
    }

    private int? GetGridRowCountFromFooter()
    {
        var footerElements = Driver.FindElements(
            By.XPath("//span[contains(@class,'statusTextContainer') and contains(text(),'Rows:')]"));

        if (footerElements.Count == 0)
            return null;

        var rawCount = footerElements[0].Text.Trim().Replace("Rows:", string.Empty).Trim();

        return int.TryParse(rawCount, out var count) ? count : null;
    }

    /// <summary>
    /// Resolves the APHA reference recorded in the Exporter Portal. References are stored as
    /// '{applicationName}_APHAReference'; falls back to 'APHAReference' if no named key exists.
    /// </summary>
    private string GetAphaReference()
    {
        var namedKeys = scenarioContext.Keys
            .Where(k => k.EndsWith("_APHAReference", StringComparison.Ordinal))
            .ToList();

        if (namedKeys.Count == 1)
        {
            return scenarioContext.Get<string>(namedKeys[0]);
        }

        if (namedKeys.Count > 1)
        {
            throw new InvalidOperationException(
                $"Multiple APHA references found in the scenario context [{string.Join(", ", namedKeys)}] — " +
                "unable to determine which application was created in the Exporter Portal.");
        }

        scenarioContext.TryGetValue("APHAReference", out string aphaReference);

        aphaReference.Should().NotBeNullOrEmpty(
            "No APHA reference was found in the scenario context. Ensure the Exporter Portal application reference was recorded.");

        return aphaReference;
    }
}
