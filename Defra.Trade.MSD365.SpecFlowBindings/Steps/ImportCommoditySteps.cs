// <copyright file="ImportCommoditySteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Linq;

/// <summary>
/// Step bindings relating to the Import Commodity Line functional area.
/// </summary>
[Binding]
public sealed class ImportCommoditySteps : PowerAppsStepDefiner
{
    public const string HmiInspectionRequiredKey = "JointCommodityHmiInspectionRequired";

    private static readonly string[] ValidInspectionClassifications =
        ["Mandatory", "Controlled", "Reduced", "Not Notifiable"];

    private readonly ScenarioContext scenarioContext;

    public ImportCommoditySteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    [Then("the Import Commodity Line page is displayed")]
    public void ThenTheImportCommodityLinePageIsDisplayed()
    {
        Driver.WaitForTransaction();

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span'][normalize-space(text())='Import Commodity Line']"),
            "Import Commodity Line page header could not be found — the page may still be showing 'Work Order'.");

        pageHeader.Text.Trim().Should().Be("Import Commodity Line",
            $"Expected page header to be 'Import Commodity Line' but found '{pageHeader.Text.Trim()}'.");
    }

    [Then(@"the settings are displayed as HMI Inspection Required '(.*)', PHSI Inspection Required '(.*)' and Inspection Classification '(.*)'")]
    public void ThenTheSettingsAreDisplayed(
    string expectedHmiInspectionRequired,
    string expectedPhsiInspectionRequired,
    string expectedInspectionClassifications)
    {
        Driver.WaitForTransaction();

        // The header field values are populated asynchronously after navigation and can briefly
        // render with stale data before the form fully resolves. Retry for up to 30 seconds
        // to allow the fields to settle without issuing a full page Refresh (which would
        // navigate away from the record).
        string actualHmi = null;
        string actualPhsi = null;
        string actualClassification = null;

        // Parse '/'-separated accepted values (trimmed). Any of them is considered a valid match.
        // Enables feature lines such as: HMI Inspection Required 'Yes / No'.
        static string[] ParseAccepted(string raw) =>
            (raw ?? string.Empty)
                .Split('/')
                .Select(v => v.Trim())
                .Where(v => v.Length > 0)
                .ToArray();

        var acceptedHmi = ParseAccepted(expectedHmiInspectionRequired);
        var acceptedPhsi = ParseAccepted(expectedPhsiInspectionRequired);
        var acceptedClassifications = ParseAccepted(expectedInspectionClassifications);

        Policy
            .Handle<Exception>()
            .OrResult<bool>(allMatch => !allMatch)
            .WaitAndRetry(6, _ => TimeSpan.FromSeconds(5),
                onRetry: (_, _, attempt, _) =>
                    Console.WriteLine(
                        $"[ThenTheSettingsAreDisplayed] Attempt {attempt}: " +
                        $"HMI='{actualHmi}', PHSI='{actualPhsi}', Classification='{actualClassification}' — retrying..."))
            .Execute(() =>
            {
                actualHmi = GetHeaderFieldValue("HMI Inspection Required");
                actualPhsi = GetHeaderFieldValue("PHSI Inspection Required");
                actualClassification = GetHeaderFieldValue("Inspection Classification");

                var hmiMatch = acceptedHmi.Contains(actualHmi, StringComparer.OrdinalIgnoreCase);
                var phsiMatch = acceptedPhsi.Contains(actualPhsi, StringComparer.OrdinalIgnoreCase);

                var classificationMatch = acceptedClassifications.Length == 0
                    ? actualClassification is "---" or ""
                    : acceptedClassifications.Contains(actualClassification, StringComparer.OrdinalIgnoreCase);

                return hmiMatch && phsiMatch && classificationMatch;
            });

        // Final assertions — at this point all values have stabilised or the retry budget is exhausted.
        actualHmi.Should().BeOneOf(acceptedHmi,
            $"Expected HMI Inspection Required to be one of '{expectedHmiInspectionRequired}' but found '{actualHmi}'.");

        actualPhsi.Should().BeOneOf(acceptedPhsi,
            $"Expected PHSI Inspection Required to be one of '{expectedPhsiInspectionRequired}' but found '{actualPhsi}'.");

        if (acceptedClassifications.Length == 0)
        {
            actualClassification.Should().BeOneOf("---", string.Empty,
                $"Expected Inspection Classification to be blank but found '{actualClassification}'.");
        }
        else
        {
            acceptedClassifications.Should().AllSatisfy(v =>
                ValidInspectionClassifications.Should().Contain(v,
                    $"'{v}' is not a recognised Inspection Classification value."));

            actualClassification.Should().BeOneOf(acceptedClassifications,
                $"Expected Inspection Classification to be one of '{expectedInspectionClassifications}' but found '{actualClassification}'.");
        }

        // Persist the resolved (actual) HMI value so downstream steps can branch strictly on it.
        this.scenarioContext[HmiInspectionRequiredKey] = actualHmi;
    }

    /// <summary>
    /// Reads the value of a named header field by matching the label text in the page header band.
    /// </summary>
    /// <remarks>
    /// Dynamics CSS-in-JS class names are unstable and regenerate on each deployment.
    /// The only stable anchor is the structural pattern within each header column container:
    ///   div[@data-preview_orientation='column']
    ///     div (first child)  — contains the value div as its only child
    ///       div              — the actual value text
    ///     div (last child)   — contains the label text directly
    /// </remarks>
    private string GetHeaderFieldValue(string fieldLabel)
    {
        // Find the column container whose last-child div contains the label text,
        // then return the text of the div nested inside the first-child div (the value).
        var valueXPath =
            $"//div[@data-preview_orientation='column']" +
            $"[div[last()][normalize-space(text())='{fieldLabel}']]" +
            $"/div[1]/div";

        var valueElement = Driver.WaitUntilAvailable(
            By.XPath(valueXPath),
            $"Header field '{fieldLabel}' could not be found on the Import Commodity Line page.");

        return valueElement.Text.Trim();
    }
}