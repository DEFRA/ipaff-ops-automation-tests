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
    private static readonly string[] ValidInspectionClassifications =
        ["Mandatory", "Controlled", "Reduced", "Not Notifiable"];

    [Then("the Import Commodity Line page is displayed")]
    public void ThenTheImportCommodityLinePageIsDisplayed()
    {
        Driver.WaitForTransaction();

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span']"),
            "Import Commodity Line page header could not be found.");

        pageHeader.Text.Trim().Should().Be("Import Commodity Line",
            $"Expected page header to be 'Import Commodity Line' but found '{pageHeader.Text.Trim()}'.");

        // The form loads the page header before field controls are fully hydrated.
        // Waiting for the Work Order lookup value to be present ensures the form has
        // fully rendered before any subsequent tab selection or field assertion steps run.
        Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='trd_msdyn_workorder.fieldControl-LookupResultsDropdown_trd_msdyn_workorder_selected_tag']"),
            "Work Order lookup value did not appear on the Import Commodity Line page — form may not have fully loaded.");
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

                var hmiMatch = actualHmi == expectedHmiInspectionRequired;
                var phsiMatch = actualPhsi == expectedPhsiInspectionRequired;

                var classificationMatch = string.IsNullOrWhiteSpace(expectedInspectionClassifications)
                    ? actualClassification is "---" or ""
                    : expectedInspectionClassifications
                        .Split('/')
                        .Select(v => v.Trim())
                        .Contains(actualClassification, StringComparer.OrdinalIgnoreCase);

                return hmiMatch && phsiMatch && classificationMatch;
            });

        // Final assertions — at this point all values have stabilised or the retry budget is exhausted.
        actualHmi.Should().Be(expectedHmiInspectionRequired,
            $"Expected HMI Inspection Required to be '{expectedHmiInspectionRequired}' but found '{actualHmi}'.");

        actualPhsi.Should().Be(expectedPhsiInspectionRequired,
            $"Expected PHSI Inspection Required to be '{expectedPhsiInspectionRequired}' but found '{actualPhsi}'.");

        if (string.IsNullOrWhiteSpace(expectedInspectionClassifications))
        {
            actualClassification.Should().BeOneOf("---", string.Empty,
                $"Expected Inspection Classification to be blank but found '{actualClassification}'.");
        }
        else
        {
            var acceptedValues = expectedInspectionClassifications
                .Split('/')
                .Select(v => v.Trim())
                .ToArray();

            acceptedValues.Should().AllSatisfy(v =>
                ValidInspectionClassifications.Should().Contain(v,
                    $"'{v}' is not a recognised Inspection Classification value."));

            actualClassification.Should().BeOneOf(acceptedValues,
                $"Expected Inspection Classification to be one of '{expectedInspectionClassifications}' but found '{actualClassification}'.");
        }
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