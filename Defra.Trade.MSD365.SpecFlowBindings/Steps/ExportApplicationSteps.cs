// <copyright file="ExportApplicationSteps.cs" company="DEFRA">
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
/// Step bindings relating to the Export Application record.
/// </summary>
[Binding]
public sealed class ExportApplicationSteps : PowerAppsStepDefiner
{
    private const string StockSubgridXPath =
        "//div[contains(@data-lp-id,'|trd_stock|trd_trd_exportapplication_trd_stock_exportapplication')]";

    private readonly ScenarioContext scenarioContext;

    public ExportApplicationSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    [Then("the Export Application page is displayed for the application created in the Exporter Portal")]
    public void ThenTheExportApplicationPageIsDisplayedForTheApplicationCreatedInTheExporterPortal()
    {
        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");

        var expectedAphaReference = GetAphaReference();

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span']"),
            "Export Application page header could not be found.");

        var actualPageHeader = pageHeader.Text.Trim();
        actualPageHeader.Should().Be("Export Application",
            $"Expected page header to be 'Export Application' but found '{actualPageHeader}'.");

        var referenceHeader = Driver.WaitUntilAvailable(
            By.XPath("//h1[@data-id='header_title']"),
            "Application reference header could not be found on the Export Application page.");

        var actualReference = (referenceHeader.GetAttribute("title") ?? referenceHeader.Text)
            .Replace("- Saved", string.Empty)
            .Trim();

        actualReference.Should().Be(expectedAphaReference,
            $"Expected Export Application header to be '{expectedAphaReference}' but found '{actualReference}'.");
    }

    [Then(@"the Status of the Export Application should be '(.*)'")]
    public void ThenTheStatusOfTheExportApplicationShouldBe(string expectedStatus)
    {
        Driver.WaitForTransaction();

        var headerHelper = new DynamicsHeaderHelper(Driver);
        var statusText = headerHelper.GetHeaderFieldValueByLabel("Status");

        statusText.Should().NotBeNull(
            $"Status '{expectedStatus}' could not be found in the Export Application header, even after expanding " +
            "the 'More Header Editable Fields' overflow flyout.");

        statusText.Should().Be(expectedStatus,
            $"Expected Export Application Status to be '{expectedStatus}' but found '{statusText}'.");
    }

    [Then(@"the Status Reason of the Export Application should be '(.*)'")]
    public void ThenTheStatusReasonOfTheExportApplicationShouldBe(string expectedStatusReason)
    {
        Driver.WaitForTransaction();

        var headerHelper = new DynamicsHeaderHelper(Driver);
        var statusReasonText = headerHelper.GetHeaderFieldValueByLabel("Status Reason");

        statusReasonText.Should().NotBeNull(
            $"Status Reason '{expectedStatusReason}' could not be found in the Export Application header, even after expanding " +
            "the 'More Header Editable Fields' overflow flyout.");

        statusReasonText.Should().Be(expectedStatusReason,
            $"Expected Export Application Status Reason to be '{expectedStatusReason}' but found '{statusReasonText}'.");
    }

    /// <summary>
    /// Verifies the number of records in the Stock subgrid on the Export Application Related tab.
    /// Uses the AG grid footer row count, retrying while the subgrid finishes refreshing.
    /// </summary>
    [Then(@"I can see (\d+) records? displayed within the Stock subgrid")]
    public void ThenICanSeeRecordDisplayedWithinTheStockSubgrid(int expectedRecordCount)
    {
        Driver.WaitForTransaction();

        var actualCount = Policy
            .Handle<Exception>()
            .OrResult<int?>(count => count != expectedRecordCount)
            .WaitAndRetry(6, _ => TimeSpan.FromSeconds(5))
            .Execute(() =>
            {
                Driver.WaitForTransaction();
                return GetStockSubgridRowCount();
            });

        actualCount.Should().Be(expectedRecordCount,
            $"Expected {expectedRecordCount} record(s) in the Stock subgrid but found {actualCount}.");
    }

    /// <summary>
    /// Reads the Name of the first record in the Stock subgrid and stores it in the
    /// scenario context under the key 'StockRequestName'.
    /// </summary>
    [Then("I make a note of the stock name")]
    public void ThenIMakeANoteOfTheStockName()
    {
        Driver.WaitForTransaction();

        string stockName = null;

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .OrResult<string>(string.IsNullOrWhiteSpace)
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var subgrid = Driver.WaitUntilAvailable(
                    By.XPath(StockSubgridXPath),
                    "Stock subgrid could not be found on the Export Application page.");

                var nameCell = subgrid.FindElement(
                    By.XPath(".//div[contains(@class,'ag-center-cols-container')]//div[@role='row'][1]//div[@col-id='trd_name']"));

                return stockName = nameCell.Text.Trim();
            });

        stockName.Should().NotBeNullOrWhiteSpace("The stock name could not be read from the Stock subgrid.");

        scenarioContext["StockRequestName"] = stockName;
    }

    private int? GetStockSubgridRowCount()
    {
        var subgrid = Driver.WaitUntilAvailable(
            By.XPath(StockSubgridXPath),
            "Stock subgrid could not be found on the Export Application page.");

        var footerElements = subgrid.FindElements(
            By.XPath(".//span[contains(@class,'statusTextContainer') and contains(text(),'Rows:')]"));

        if (footerElements.Count > 0)
        {
            var rawCount = footerElements[0].Text.Trim().Replace("Rows:", string.Empty).Trim();

            if (int.TryParse(rawCount, out var count))
            {
                return count;
            }
        }

        return subgrid.FindElements(
            By.XPath(".//div[contains(@class,'ag-center-cols-container')]//div[@role='row']")).Count;
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
