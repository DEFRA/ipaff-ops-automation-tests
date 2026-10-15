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

    /// <summary>
    /// Verifies the Export Application page is displayed for the named application
    /// reference stored in the scenario context as '{applicationName}_APHAReference'.
    /// </summary>
    /// <param name="applicationName">The application name e.g. 'Phytosanitary Certificate Application'.</param>
    [Then(@"the Export Application page is displayed for the '(.*)' created in the Exporter Portal")]
    public void ThenTheExportApplicationPageIsDisplayedForTheNamedApplicationCreatedInTheExporterPortal(string applicationName)
    {
        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");

        scenarioContext.TryGetValue($"{applicationName}_APHAReference", out string expectedAphaReference);

        if (string.IsNullOrEmpty(expectedAphaReference))
        {
            scenarioContext.TryGetValue("APHAReference", out expectedAphaReference);
        }

        expectedAphaReference.Should().NotBeNullOrEmpty(
            $"No APHA reference was found in the scenario context under '{applicationName}_APHAReference'.");

        AssertExportApplicationPageIsDisplayed(expectedAphaReference);
    }

    [Then(@"the Commodity Group should be '(.*)'")]
    public void ThenTheCommodityGroupShouldBe(string expectedCommodityGroup)
    {
        var actual = GetLookupFieldText("trd_commoditygroupid", "trd_commoditygroupid.fieldControl-Lookup_trd_commoditygroupid");

        actual.Should().Be(expectedCommodityGroup,
            $"Expected Commodity Group to be '{expectedCommodityGroup}' but found '{actual}'.");
    }

    [Then(@"the Application Method should be '(.*)'")]
    public void ThenTheApplicationMethodShouldBe(string expectedApplicationMethod)
    {
        Driver.WaitForTransaction();

        string actual = null;

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .OrResult<string>(string.IsNullOrWhiteSpace)
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var control = Driver.WaitUntilAvailable(
                    By.XPath("//*[(self::select or self::input or self::button) and @aria-label='Application Method']"),
                    "Application Method field could not be found on the Export Application page.");

                Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", control);

                actual = control.TagName.Equals("select", StringComparison.OrdinalIgnoreCase)
                    ? new OpenQA.Selenium.Support.UI.SelectElement(control).SelectedOption.Text.Trim()
                    : control.TagName.Equals("input", StringComparison.OrdinalIgnoreCase)
                        ? control.GetAttribute("value")?.Trim()
                        : (control.GetAttribute("title") ?? control.Text)?.Trim();

                return actual;
            });

        actual.Should().Be(expectedApplicationMethod,
            $"Expected Application Method to be '{expectedApplicationMethod}' but found '{actual}'.");
    }

    [Then(@"the Application Type should be '(.*)'")]
    public void ThenTheApplicationTypeShouldBe(string expectedApplicationType)
    {
        var actual = GetLookupFieldText("trd_applicationtypeid", "trd_applicationtypeid.fieldControl-LookupResultsDropdown_trd_applicationtypeid_SelectedRecordList");

        actual.Should().Be(expectedApplicationType,
            $"Expected Application Type to be '{expectedApplicationType}' but found '{actual}'.");
    }

    /// <summary>
    /// Verifies that the specified tab is displayed on the current form (selection is not checked).
    /// </summary>
    /// <param name="tabName">The name of the tab e.g. 'Stock Requests'.</param>
    [Then(@"the '(.*)' tab is displayed")]
    public void ThenTheTabIsDisplayed(string tabName)
    {
        Driver.WaitForTransaction();

        // Retry every 30 seconds for up to 5 minutes, refreshing the page between attempts.
        // Only used on full form pages (Export Application / Work Order), not on Work Order Task pop-ups,
        // as a refresh would downsize the pop-up.
        const int maxRetries = 10;
        var isDisplayed = false;

        for (var attempt = 0; attempt <= maxRetries && !isDisplayed; attempt++)
        {
            if (attempt > 0)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
                Driver.Navigate().Refresh();
                Driver.WaitForTransaction();
            }

            try
            {
                var tab = Driver.WaitUntilAvailable(
                    By.XPath($"//ul[@role='tablist']//li[@role='tab' and @aria-label='{tabName}']"),
                    TimeSpan.FromSeconds(5),
                    $"'{tabName}' tab could not be found on the form.");

                isDisplayed = tab.Displayed;
            }
            catch (Exception)
            {
                isDisplayed = false;
            }
        }

        isDisplayed.Should().BeTrue($"Expected the '{tabName}' tab to be displayed but it was not after retrying for 5 minutes.");
    }

    private void AssertExportApplicationPageIsDisplayed(string expectedAphaReference)
    {
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

    /// <summary>
    /// Reads the display text of a lookup field. Scrolls the field container into view first as
    /// lookup tags are lazy-rendered, retrying on stale elements.
    /// </summary>
    private string GetLookupFieldText(string fieldSchemaName, string valueDataId)
    {
        Driver.WaitForTransaction();

        string text = null;

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .OrResult<string>(string.IsNullOrWhiteSpace)
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(3))
            .Execute(() =>
            {
                var fieldContainers = Driver.FindElements(By.XPath($"//div[@data-id='{fieldSchemaName}']"));

                if (fieldContainers.Count > 0)
                {
                    Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", fieldContainers[0]);
                    Driver.WaitForTransaction();
                }

                var valueElement = Driver.WaitUntilAvailable(
                    By.XPath($"//*[@data-id='{valueDataId}']"),
                    $"Lookup value element '{valueDataId}' could not be found.");

                var tagTexts = valueElement.FindElements(By.XPath(".//div[contains(@data-id,'_selected_tag_text')]"));

                return text = (tagTexts.Count > 0 ? tagTexts[0].Text : valueElement.Text)?.Trim();
            });

        return text;
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
}
