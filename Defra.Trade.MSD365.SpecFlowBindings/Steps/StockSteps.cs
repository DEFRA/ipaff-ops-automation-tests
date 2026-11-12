// <copyright file="StockSteps.cs" company="DEFRA">
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
using System.Text.RegularExpressions;

/// <summary>
/// Step bindings relating to the Stock record.
/// </summary>
[Binding]
public sealed class StockSteps : PowerAppsStepDefiner
{
    private const string TotalDrawDownFieldId = "trd_totaldrawdown";
    private const string TotalDrawDownIncPendingFieldId = "trd_totaldrawdownincpending";
    private const string RemainingApprovedStockFieldId = "trd_remainingapprovedstock";
    private const string RemainingApprovedStockIncPendingFieldId = "trd_remainingapprovedstockincpending";

    private static readonly string[] StockDrawDownQuantityFieldIds =
    [
        TotalDrawDownFieldId,
        TotalDrawDownIncPendingFieldId,
        RemainingApprovedStockFieldId,
        RemainingApprovedStockIncPendingFieldId,
    ];

    private static readonly Regex ThreeDecimalPlacesRegex = new(@"^-?\d{1,3}(,\d{3})*\.\d{3}$|^-?\d+\.\d{3}$");

    private readonly ScenarioContext scenarioContext;

    public StockSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    [Then("the Stock page is displayed for the stock reference created in the Exporter Portal")]
    public void ThenTheStockPageIsDisplayedForTheStockReferenceCreatedInTheExporterPortal()
    {
        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");

        var expectedStockName = scenarioContext.Get<string>("StockRequestName");

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span']"),
            "Stock page header could not be found.");

        var actualPageHeader = pageHeader.Text.Trim();
        actualPageHeader.Should().Be("Stock",
            $"Expected page header to be 'Stock' but found '{actualPageHeader}'.");

        var referenceHeader = Driver.WaitUntilAvailable(
            By.XPath("//h1[@data-id='header_title']"),
            "Stock reference header could not be found on the Stock page.");

        var actualStockName = (referenceHeader.GetAttribute("title") ?? referenceHeader.Text)
            .Replace("- Saved", string.Empty)
            .Trim();

        actualStockName.Should().Be(expectedStockName,
            $"Expected Stock header to be '{expectedStockName}' but found '{actualStockName}'.");
    }

    [Then(@"the (\d+) Stock Draw Down Quantities fields are displayed to three decimal places")]
    public void ThenTheStockDrawDownQuantitiesFieldsAreDisplayedToThreeDecimalPlaces(int expectedFieldCount)
    {
        StockDrawDownQuantityFieldIds.Length.Should().Be(expectedFieldCount,
            $"Expected {expectedFieldCount} Stock Draw Down Quantities fields but {StockDrawDownQuantityFieldIds.Length} are defined.");

        foreach (var fieldId in StockDrawDownQuantityFieldIds)
        {
            var value = GetDecimalFieldValue(fieldId);

            ThreeDecimalPlacesRegex.IsMatch(value).Should().BeTrue(
                $"Expected field '{fieldId}' to be displayed to three decimal places but found '{value}'.");
        }
    }

    [Then(@"the Total Draw Down quantity is '(.*)'")]
    public void ThenTheTotalDrawDownQuantityIs(string expectedQuantity)
    {
        string actual = null;

        Policy
            .Handle<Exception>()
            .OrResult<string>(value => !string.Equals(value, expectedQuantity, StringComparison.Ordinal))
            .WaitAndRetry(6, _ =>
            {
                Driver.WaitUntilClickable(
                    By.XPath("//li[contains(@id,'|NoRelationship|Form|Mscrm.Modern.refreshCommand')]//button | //button[contains(@data-id,'|NoRelationship|Form|Mscrm.Modern.refreshCommand')]"),
                    TimeSpan.FromSeconds(10),
                    "Refresh button could not be found in the command bar.").Click();
                Driver.WaitForTransaction();
                return TimeSpan.FromSeconds(5);
            })
            .Execute(() => actual = GetDecimalFieldValue(TotalDrawDownFieldId));

        actual.Should().Be(expectedQuantity,
            $"Expected Total Draw Down quantity to be '{expectedQuantity}' but found '{actual}' after retrying.");
    }

    [Then(@"the Total Draw Down Including Pending quantity is '(.*)'")]
    public void ThenTheTotalDrawDownIncludingPendingQuantityIs(string expectedQuantity)
    {
        var actual = GetDecimalFieldValue(TotalDrawDownIncPendingFieldId);

        actual.Should().Be(expectedQuantity,
            $"Expected Total Draw Down Including Pending quantity to be '{expectedQuantity}' but found '{actual}'.");
    }

    [Then(@"the Remaining Approved Stock quantity is '(.*)'")]
    public void ThenTheRemainingApprovedStockQuantityIs(string expectedQuantity)
    {
        var actual = GetDecimalFieldValue(RemainingApprovedStockFieldId);

        actual.Should().Be(expectedQuantity,
            $"Expected Remaining Approved Stock quantity to be '{expectedQuantity}' but found '{actual}'.");
    }

    [Then(@"the Remaining Approved Stock Including Pending quantity is '(.*)'")]
    public void ThenTheRemainingApprovedStockIncludingPendingQuantityIs(string expectedQuantity)
    {
        var actual = GetDecimalFieldValue(RemainingApprovedStockIncPendingFieldId);

        actual.Should().Be(expectedQuantity,
            $"Expected Remaining Approved Stock Including Pending quantity to be '{expectedQuantity}' but found '{actual}'.");
    }

    /// <summary>
    /// Reads the displayed value of a decimal number field, scrolling it into view first and
    /// retrying on stale elements. The '---' placeholder is treated as an empty value.
    /// </summary>
    private string GetDecimalFieldValue(string fieldId)
    {
        Driver.WaitForTransaction();

        string value = null;

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .OrResult<string>(string.IsNullOrWhiteSpace)
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var input = Driver.WaitUntilAvailable(
                    By.XPath($"//input[@data-id='{fieldId}.fieldControl-decimal-number-text-input']"),
                    $"Decimal field '{fieldId}' could not be found on the Stock page.");

                Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

                return value = (input.GetAttribute("value") ?? input.GetAttribute("title"))?.Trim();
            });

        return value;
    }
}
