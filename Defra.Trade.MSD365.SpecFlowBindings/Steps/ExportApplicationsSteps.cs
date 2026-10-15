// <copyright file="ExportApplicationsSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Linq;

/// <summary>
/// Step bindings relating to the Export Applications grid page.
/// </summary>
[Binding]
public sealed class ExportApplicationsSteps : PowerAppsStepDefiner
{
    private const string ApplicationReferenceColumnId = "trd_applicationreference";

    private readonly ScenarioContext scenarioContext;
    private string searchedAphaReference;

    public ExportApplicationsSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    [Then(@"the '(.*)' view is displayed on the Export Applications page")]
    public void ThenTheViewIsDisplayedOnTheExportApplicationsPage(string expectedViewName)
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

    [When("I search Export Applications for the {string} created in the Exporter Portal")]
    public void WhenISearchExportApplicationsForTheCreatedInTheExporterPortal(string applicationName)
    {
        scenarioContext.TryGetValue($"{applicationName}_APHAReference", out string aphaReference);

        if (string.IsNullOrEmpty(aphaReference))
        {
            scenarioContext.TryGetValue("APHAReference", out aphaReference);
        }

        aphaReference.Should().NotBeNullOrEmpty(
            $"No APHA reference was found in the scenario context under '{applicationName}_APHAReference'.");

        searchedAphaReference = aphaReference;
        XrmApp.Grid.Search(aphaReference);
        Driver.WaitForTransaction();
    }

    [Then("the export application submitted in the Exporter Portal should be returned")]
    public void ThenTheExportApplicationSubmittedInTheExporterPortalShouldBeReturned()
    {
        var expectedAphaReference = searchedAphaReference ?? GetAphaReference();

        // Retry to handle the delay between the Exporter Portal submission and the Export Application
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
            $"Expected exactly 1 Export Application for APHA reference '{expectedAphaReference}' but found {rowCount}.");

        string actualReference = null;

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var referenceCell = Driver.WaitUntilAvailable(
                    By.XPath($"(//div[@role='gridcell' and @col-id='{ApplicationReferenceColumnId}'])[1]"),
                    "Application reference cell could not be found in the Export Applications grid.");

                actualReference = referenceCell.Text.Trim();
            });

        actualReference.Should().Be(expectedAphaReference,
            $"Expected Application reference '{expectedAphaReference}' but found '{actualReference}'.");
    }

    /// <summary>
    /// Reads the grid row count from the footer "Rows: X" span.
    /// Returns null if the footer element cannot be found (grid not yet rendered — keep retrying).
    /// </summary>
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
