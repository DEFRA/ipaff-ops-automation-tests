// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.MSD365.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.Specs.Steps;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Polly;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

/// <summary>
/// Step bindings relating to the work order functional area.
/// </summary>
[Binding]
public sealed class WorkOrderSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;
    private readonly ScenarioContext scenarioContext;

    public WorkOrderSteps(SessionContext sessionContext, ScenarioContext scenarioContext = null)
    {
        this.sessionContext = sessionContext;
        this.scenarioContext = scenarioContext;
    }

    [Then("I verify the Work Order page is displayed for the notification created in IPAFFS")]
    public void ThenIVerifyTheWorkOrderPageIsDisplayedForTheNotificationCreatedInIPAFFS()
    {
        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");

        var expectedChedReference = scenarioContext.Get<string>("CHEDReference");

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span']"),
            "Work Order page header could not be found.");

        var actualPageHeader = pageHeader.Text.Trim();
        actualPageHeader.Should().Be("Work Order",
            $"Expected page header to be 'Work Order' but found '{actualPageHeader}'.");

        var chedReferenceHeader = Driver.WaitUntilAvailable(
            By.XPath("//h1[@data-id='header_title']"),
            $"CHED reference header could not be found on the Work Order page.");

        var actualChedReference = chedReferenceHeader.Text
            .Replace("- Saved", string.Empty)
            .Trim();

        actualChedReference.Should().Be(expectedChedReference,
            $"Expected CHED reference header to be '{expectedChedReference}' but found '{actualChedReference}'.");
    }

    [When("I click the Assign command")]
    public void WhenIClickTheAssignCommand()
    {
        Driver.WaitForTransaction();

        // Determine whether the Assign command should be clicked inside a maximised popup
        // or on the main Work Order page command bar.
        //
        // When called after "I click on the '<task>' task" + "I maximise the popup", the popup
        // container is present in the DOM and its own command bar may not yet have fully rendered.
        // Clicking via CommandSteps.WhenISelectTheCommand in this state hits the Work Order page's
        // overflow button instead, causing ElementClickInterceptedException.
        //
        // When called from the Work Order page (no active popup), fall through to the standard helper.
        var popupContainers = Driver.FindElements(
            By.XPath("//section[contains(@id,'popupContainer')]"));

        if (popupContainers.Count > 0)
        {
            // The popup command bar renders asynchronously after the popup opens.
            // Poll with FindElements until the Assign button appears — a single
            // WaitUntilAvailable call times out if the bar is still loading when it starts.
            IWebElement popupAssignButton = null;
            var deadline = DateTime.UtcNow.AddSeconds(60);

            while (DateTime.UtcNow < deadline)
            {
                Driver.WaitForTransaction();

                var candidates = Driver.FindElements(
                    By.XPath("//section[contains(@id,'popupContainer')]//button[contains(@data-id,'Assign') or contains(@aria-label,'Assign')]"));

                if (candidates.Count > 0)
                {
                    popupAssignButton = candidates[0];
                    break;
                }

                Thread.Sleep(2000);
            }

            if (popupAssignButton == null)
            {
                throw new InvalidOperationException(
                    "Assign button could not be found in the popup command bar within 60 seconds.");
            }

            Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", popupAssignButton);
            Driver.WaitForTransaction();

            popupAssignButton.Click();
        }
        else
        {
            CommandSteps.WhenISelectTheCommand("Assign");
        }

        Driver.WaitForTransaction();
    }

    [Then("I can see the Assign Work Order popup is displayed")]
    public void ThenICanSeeTheAssignWorkOrderPopupIsDisplayed()
    {
        Driver.WaitForTransaction();

        var assignDialog = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='Assign' and @data-uci-dialog='true']"),
            "Assign Work Order dialog could not be found.");

        var dialogHeader = assignDialog.FindElement(By.XPath(".//h1[@data-id='assignheader_id']"));

        dialogHeader.Text.Trim().Should().Be("Assign Work Order",
            $"Expected dialog header to be 'Assign Work Order' but found '{dialogHeader.Text.Trim()}'.");
    }

    [When("I click the Assign button")]
    public void WhenIClickTheAssignButton()
    {
        Driver.WaitForTransaction();
        var assignButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='ok_id' and @title='Assign']"),
            "Assign button could not be found in the Assign Work Order dialog.");

        assignButton.Click();
        Driver.WaitForTransaction();
    }

    [Then("the Substatus of the Work Order should be Assigned")]
    public void ThenTheSubstatusOfTheWorkOrderShouldBeAssigned()
    {
        Driver.WaitForTransaction();

        var headerHelper = new DynamicsHeaderHelper(Driver);
        var substatusText = headerHelper.GetHeaderFieldValueText("header_msdyn_substatus", "msdyn_substatus");

        substatusText.Should().NotBeNull(
            "Substatus 'Assigned' could not be found in the Work Order header, even after expanding " +
            "the 'More Header Editable Fields' overflow flyout.");

        substatusText.Should().Be("Assigned",
            $"Expected Substatus to be 'Assigned' but found '{substatusText}'.");
    }

    [Then(@"the Substatus of the Work Order should be '(.*)'")]
    public void ThenTheSubstatusOfTheWorkOrderShouldBe(string expectedSubstatus)
    {
        Driver.WaitForTransaction();

        var headerHelper = new DynamicsHeaderHelper(Driver);
        var substatusText = headerHelper.GetHeaderFieldValueText("header_msdyn_substatus", "msdyn_substatus");

        substatusText.Should().NotBeNull(
            $"Substatus '{expectedSubstatus}' could not be found in the Work Order header, even after expanding " +
            "the 'More Header Editable Fields' overflow flyout.");

        substatusText.Should().Be(expectedSubstatus,
            $"Expected Substatus to be '{expectedSubstatus}' but found '{substatusText}'.");
    }

    /// <summary>
    /// Verifies that the specified Work Order Task is no longer present in the
    /// Active Work Order Service Tasks - Exports subgrid. Refreshes the subgrid between
    /// attempts as task deactivation is processed asynchronously.
    /// </summary>
    /// <param name="taskName">The name of the task e.g. 'Seed Sampling Inspection Task'.</param>
    [Then(@"the '(.*)' is no longer visible in the Active Work Order Service Tasks - Exports subgrid")]
    public void ThenTheTaskIsNoLongerVisibleInTheActiveWorkOrderServiceTasksExportsSubgrid(string taskName)
    {
        Driver.WaitForTransaction();

        var taskLinkXPath = $"//div[@data-id='dataSetRoot_workorderservicetasksgrid']//div[@col-id='msdyn_name']//a[@role='link' and @aria-label='{taskName}']";

        var remaining = Policy
            .Handle<Exception>()
            .OrResult<int>(count => count > 0)
            .WaitAndRetry(6, _ =>
            {
                XrmApp.Entity.SubGrid.ClickCommand("workorderservicetasksgrid", "Refresh");
                Driver.WaitForTransaction();
                return TimeSpan.FromSeconds(5);
            })
            .Execute(() =>
            {
                Driver.WaitForTransaction();

                Driver.WaitUntilAvailable(
                    By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
                    "Work Order Service Tasks grid could not be found.");

                return Driver.FindElements(By.XPath(taskLinkXPath)).Count;
            });

        remaining.Should().Be(0,
            $"Expected Work Order Task '{taskName}' to no longer be visible in the Active Work Order Service Tasks - Exports subgrid " +
            $"but it was still present after retrying.");
    }

    /// <summary>
    /// Clicks the Export Application lookup link in the Application &amp; Inspection Address section
    /// of the Work Order. Scrolls the field into view first as the lookup tag is lazy-rendered.
    /// </summary>
    [When("I click the Export Application link in the Application & Inspection Address section")]
    public void WhenIClickTheExportApplicationLinkInTheApplicationInspectionAddressSection()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .Or<ElementClickInterceptedException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(3))
            .Execute(() =>
            {
                var fieldContainers = Driver.FindElements(By.XPath("//div[@data-id='trd_exportapplicationid']"));

                if (fieldContainers.Count > 0)
                {
                    Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", fieldContainers[0]);
                    Driver.WaitForTransaction();
                }

                var exportApplicationLink = Driver.WaitUntilAvailable(
                    By.XPath("//div[@data-id='trd_exportapplicationid.fieldControl-LookupResultsDropdown_trd_exportapplicationid_selected_tag_text']"),
                    "Export Application link could not be found in the Application & Inspection Address section.");

                exportApplicationLink.Click();
            });

        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");
    }

    [Then("the Owner of the Work Order should be me")]
    public void ThenTheOwnerOfTheWorkOrderShouldBeMe()
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedOwner = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var headerHelper = new DynamicsHeaderHelper(Driver);
        var ownerText = headerHelper.GetHeaderFieldValueText("header_ownerid", "ownerid");

        ownerText.Should().NotBeNull(
            $"Owner '{expectedOwner}' could not be found in the Work Order header, even after expanding " +
            "the 'More Header Editable Fields' overflow flyout.");

        ownerText.Should().Contain(expectedOwner,
            $"Expected Owner to contain '{expectedOwner}' but found '{ownerText}'.");
    }

    [Then(@"the Owner of the Work Order should be '(.*)'")]
    public void ThenTheOwnerOfTheWorkOrderShouldBe(string expectedOwner)
    {
        Driver.WaitForTransaction();

        var headerHelper = new DynamicsHeaderHelper(Driver);
        var ownerText = headerHelper.GetHeaderFieldValueText("header_ownerid", "ownerid");

        ownerText.Should().NotBeNull(
            $"Owner '{expectedOwner}' could not be found in the Work Order header, even after expanding " +
            "the 'More Header Editable Fields' overflow flyout.");

        ownerText.Should().Be(expectedOwner,
            $"Expected Owner to be '{expectedOwner}' but found '{ownerText}'.");
    }

    /// <summary>
    /// Verifies the Work Order page is displayed for the named application, using the APHA
    /// reference stored in the scenario context as '{applicationName}_APHAReference'.
    /// </summary>
    /// <param name="applicationName">The application name e.g. 'Phytosanitary Certificate Application'.</param>
    [Then(@"the Work Order page is displayed for the '(.*)' created in the Exporter Portal")]
    public void ThenTheWorkOrderPageIsDisplayedForTheApplicationCreatedInTheExporterPortal(string applicationName)
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

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span']"),
            "Work Order page header could not be found.");

        var actualPageHeader = pageHeader.Text.Trim();
        actualPageHeader.Should().Be("Work Order",
            $"Expected page header to be 'Work Order' but found '{actualPageHeader}'.");

        var referenceHeader = Driver.WaitUntilAvailable(
            By.XPath("//h1[@data-id='header_title']"),
            "Application reference header could not be found on the Work Order page.");

        var actualReference = (referenceHeader.GetAttribute("title") ?? referenceHeader.Text)
            .Replace("- Saved", string.Empty)
            .Trim();

        actualReference.Should().Be(expectedAphaReference,
            $"Expected Work Order header to be '{expectedAphaReference}' but found '{actualReference}'.");
    }

    [Then(@"the Application Type should be '(.*)' in the General section")]
    public void ThenTheApplicationTypeShouldBeInTheGeneralSection(string expectedApplicationType)
    {
        var actual = GetLookupSelectedTagText(
            "msdyn_workordertype",
            "msdyn_workordertype.fieldControl-LookupResultsDropdown_msdyn_workordertype_selected_tag_text");

        actual.Should().Be(expectedApplicationType,
            $"Expected Application Type to be '{expectedApplicationType}' but found '{actual}'.");
    }

    [Then(@"the Commodity Group should be '(.*)' in the General section")]
    public void ThenTheCommodityGroupShouldBeInTheGeneralSection(string expectedCommodityGroup)
    {
        var actual = GetLookupSelectedTagText(
            "msdyn_primaryincidenttype",
            "msdyn_primaryincidenttype.fieldControl-LookupResultsDropdown_msdyn_primaryincidenttype_selected_tag_text");

        actual.Should().Be(expectedCommodityGroup,
            $"Expected Commodity Group to be '{expectedCommodityGroup}' but found '{actual}'.");
    }

    /// <summary>
    /// Clicks the Work Order lookup link in the Application Details section of the Export Application.
    /// The Work Order is linked asynchronously, so the record is refreshed between attempts.
    /// </summary>
    [When("I click the Work Order link in the Application Details section")]
    public void WhenIClickTheWorkOrderLinkInTheApplicationDetailsSection()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .Or<ElementClickInterceptedException>()
            .Or<InvalidOperationException>()
            .WaitAndRetry(
                retryCount: 10,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(10),
                onRetry: (_, _, _, _) =>
                {
                    CommandSteps.WhenISelectTheCommand("Refresh");
                    Driver.WaitForTransaction();
                    SignInPromptHelper.DismissSignInPrompts(Driver, "post-refresh");
                })
            .Execute(() =>
            {
                var fieldContainers = Driver.FindElements(By.XPath("//div[@data-id='trd_workorderid']"));

                if (fieldContainers.Count > 0)
                {
                    Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", fieldContainers[0]);
                    Driver.WaitForTransaction();
                }

                var workOrderLink = Driver.WaitUntilAvailable(
                    By.XPath("//div[@data-id='trd_workorderid.fieldControl-LookupResultsDropdown_trd_workorderid_selected_tag_text']"),
                    TimeSpan.FromSeconds(10),
                    "Work Order link could not be found in the Application Details section.");

                workOrderLink.Click();
            });

        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");
    }

    private const string StockRequestsSubGridName = "stockrequests_subgrid";
    private const string StockRequestsSubgridXPath = "//div[@data-id='stockrequests_subgrid-powerapps_onegrid_control_container']";
    private const string CommodityLinesSubgridXPath = "//div[@data-control-name='subgrid_selected_commodities' or starts-with(@data-id,'PhytoCommodities-subgrid_selected_commodities')]";
    private const string WorkOrderCommodityLinesSubgridXPath = "//div[@data-id='dataSetRoot_Commoditylines']";
    private const string StockRequestsStockColumnId = "trd_stock";
    private const string StockRequestsStatusReasonColumnId = "statuscode";
    private const string StockRequestsRowStatusColumnId = "__row_status";

    private static readonly By PotatoCommodityGrid = By.CssSelector("[data-id='dataSetRoot_Commoditylines']");
    private static readonly Dictionary<string, string> PotatoCommodityColumnIds = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Potato Type"] = "trd_potatotype",
        ["Variety"] = "trd_variety",
        ["Quantity Exported"] = "trd_finalweight",
        ["Lot Reference"] = "trd_lotreference",
        ["Soil Sampling/Stock Number"] = "trd_soilsampleapplicationnumber",
        ["GSI Number"] = "trd_gsinumber",
        ["Stock Number"] = "trd_stocknumber",
        ["Chemicals Used"] = "trd_chemicalused",
    };

    /// <summary>
    /// Maps each column name to its Dynamics schema col-id, scrolls the Commodity Lines grid horizontally until that header renders,
    /// then asserts the header label text exactly matches the expected column name.
    /// </summary>
    [Then("I can see the following columns in the Commodity Lines subgrid {string} {string} {string} {string} {string} {string} {string} {string}")]
    public void ThenICanSeeTheFollowingColumnsInTheCommodityLinesSubgrid(string column1, string column2, string column3, string column4, string column5, string column6, string column7, string column8)
    {
        foreach (var column in new[] { column1, column2, column3, column4, column5, column6, column7, column8 })
        {
            var text = Policy.Handle<StaleElementReferenceException>().Or<NoSuchElementException>()
                .WaitAndRetry(4, _ => TimeSpan.FromSeconds(1))
                .Execute(() => ScrollPotatoCommodityGridUntilColumnVisible(PotatoCommodityColumnIds[column])
                    .FindElement(By.CssSelector("[data-testid='columnHeader'] label")).Text.Trim());
            text.Should().Be(column, $"Commodity Lines should contain column '{column}'");
        }
    }

    /// <summary>
    /// Scrolls the Commodity Lines grid to the Potato Type column, then performs a real mouse double-click (Selenium Actions)
    /// on the first row's Potato Type cell to open the Commodity Line record.
    /// </summary>
    [When("I double click on a commodity line in the Commodity Lines subgrid")]
    public void WhenIDoubleClickOnACommodityLineInTheCommodityLinesSubgrid()
    {
        Policy.Handle<StaleElementReferenceException>().Or<NoSuchElementException>().Or<ElementClickInterceptedException>()
            .WaitAndRetry(4, _ => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                ScrollPotatoCommodityGridUntilColumnVisible("trd_potatotype");
                var cell = Driver.FindElement(PotatoCommodityGrid)
                    .FindElement(By.CssSelector("[role='row'][row-index='0'] [role='gridcell'][col-id='trd_potatotype']"));
                new Actions(Driver).MoveToElement(cell).DoubleClick().Perform();
            });
        Driver.WaitForTransaction();
    }

    private IWebElement ScrollPotatoCommodityGridUntilColumnVisible(string columnId)
    {
        for (var attempt = 0; attempt < 40; attempt++)
        {
            Driver.WaitForTransaction();
            var grid = Driver.WaitUntilAvailable(PotatoCommodityGrid, "Commodity Lines grid was not found.");
            if (grid.FindElements(By.CssSelector("[aria-busy='true'], .ag-overlay-loading-center" )).Any(e => e.Displayed))
            {
                Thread.Sleep(250);
                continue;
            }
            var header = grid.FindElements(By.CssSelector($"[role='columnheader'][col-id='{columnId}']"))
                .FirstOrDefault(e => e.Displayed);
            if (header != null)
            {
                Driver.ExecuteScript("arguments[0].scrollIntoView({block:'nearest', inline:'center'});", header);
                return Driver.FindElement(PotatoCommodityGrid).FindElement(By.CssSelector($"[role='columnheader'][col-id='{columnId}']"));
            }
            Driver.ExecuteScript(@"
                var root = arguments[0].querySelector('.ag-body-horizontal-scroll-viewport');
                if (root) { root.scrollLeft = arguments[1] === 0 ? 0 : root.scrollLeft + 250; }", grid, attempt);
            Thread.Sleep(250);
        }
        throw new NoSuchElementException($"Commodity Lines column '{columnId}' was not rendered after scrolling.");
    }

    private static readonly Dictionary<string, string> StockRequestsColumnIds = new()
    {
        ["Stock"] = StockRequestsStockColumnId,
        ["Status Reason"] = StockRequestsStatusReasonColumnId,
    };

    /// <summary>
    /// Verifies the number of rows in the Stock Requests subgrid, refreshing the subgrid between
    /// attempts to allow asynchronously created stock requests to render.
    /// </summary>
    [Then(@"I can see (\d+) rows? in the Stock Requests subgrid")]
    public void ThenICanSeeRowInTheStockRequestsSubgrid(int expectedRowCount)
    {
        var actualRowCount = GetStockRequestsRowCountWithRefresh(expectedRowCount);

        actualRowCount.Should().Be(expectedRowCount,
            $"Expected {expectedRowCount} row(s) in the Stock Requests subgrid but found {actualRowCount}.");
    }

    [Then("I cannot see any row in the Stock Requests subgrid")]
    public void ThenICannotSeeAnyRowInTheStockRequestsSubgrid()
    {
        var actualRowCount = GetStockRequestsRowCountWithRefresh(0);

        actualRowCount.Should().Be(0,
            $"Expected no rows in the Stock Requests subgrid but found {actualRowCount}.");
    }

    [Then(@"I can see (\d+) rows? in the Commodity Lines subgrid")]
    public void ThenICanSeeRowInTheCommodityLinesSubgrid(int expectedRowCount)
    {
        Driver.WaitForTransaction();

        var actualRowCount = Policy
            .Handle<Exception>()
            .OrResult<int?>(count => count != expectedRowCount)
            .WaitAndRetry(6, _ => TimeSpan.FromSeconds(5))
            .Execute(() =>
            {
                Driver.WaitForTransaction();
                var containerXPath = Driver.FindElements(By.XPath(CommodityLinesSubgridXPath)).Any(e => e.Displayed)
                    ? CommodityLinesSubgridXPath
                    : WorkOrderCommodityLinesSubgridXPath;
                return GetSubgridRowCount(containerXPath);
            });

        actualRowCount.Should().Be(expectedRowCount,
            $"Expected {expectedRowCount} row(s) in the Commodity Lines subgrid but found {actualRowCount}.");
    }

    /// <summary>
    /// Verifies the Status Reason column of the Stock Requests subgrid row for the stock reference
    /// created in the Exporter Portal.
    /// </summary>
    [Then("the Status Reason of the Stock Request in the Stock Requests subgrid should be {string}")]
    public void ThenTheStatusReasonOfTheStockRequestInTheStockRequestsSubgridShouldBe(string expectedStatusReason)
    {
        Driver.WaitForTransaction();

        string actual = null;

        Policy
            .Handle<Exception>()
            .OrResult<string>(value => value != expectedStatusReason)
            .WaitAndRetry(6, _ =>
            {
                XrmApp.Entity.SubGrid.ClickCommand(StockRequestsSubGridName, "Refresh");
                Driver.WaitForTransaction();
                return TimeSpan.FromSeconds(5);
            })
            .Execute(() =>
            {
                Driver.WaitForTransaction();
                return actual = GetStockRequestRowCell(StockRequestsStatusReasonColumnId).Text.Trim();
            });

        actual.Should().Be(expectedStatusReason,
            $"Expected Stock Request Status Reason to be '{expectedStatusReason}' but found '{actual}' after retrying.");
    }

    [When("I select the record for the stock reference created in the Exporter Portal")]
    public void WhenISelectTheRecordForTheStockReferenceCreatedInTheExporterPortal()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .Or<ElementClickInterceptedException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var row = GetStockRequestRow();

                if (row.GetAttribute("aria-selected") != "true")
                {
                    GetStockRequestRowCell(StockRequestsRowStatusColumnId).Click();
                }
            });

        Driver.WaitForTransaction();
    }

    [Then("the Accept Request button should be displayed")]
    public void ThenTheAcceptRequestButtonShouldBeDisplayed()
    {
        Driver.WaitForTransaction();

        var acceptRequestButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='trd_stockrequest|NoRelationship|SubGridStandard|trd.trd_stockrequest.AcceptRequest.Button']"),
            "Accept Request button could not be found above the Stock Requests subgrid.");

        acceptRequestButton.Displayed.Should().BeTrue("Expected the Accept Request button to be displayed.");
    }

    [When("I click the Accept Request button")]
    public void WhenIClickTheAcceptRequestButton()
    {
        Driver.WaitForTransaction();

        var acceptRequestButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='trd_stockrequest|NoRelationship|SubGridStandard|trd.trd_stockrequest.AcceptRequest.Button']"),
            "Accept Request button could not be found above the Stock Requests subgrid.");

        Driver.ExecuteScript("arguments[0].click();", acceptRequestButton);
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Clicks the link in the given column of the Stock Requests subgrid row for the stock
    /// reference created in the Exporter Portal.
    /// </summary>
    /// <param name="columnName">The column display name e.g. 'Stock'.</param>
    [When(@"I click the link in the '(.*)' column")]
    public void WhenIClickTheLinkInTheColumn(string columnName)
    {
        StockRequestsColumnIds.TryGetValue(columnName, out var colId).Should().BeTrue(
            $"No col-id mapping is defined for Stock Requests column '{columnName}'.");

        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .Or<ElementClickInterceptedException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var link = GetStockRequestRowCell(colId).FindElement(By.XPath(".//a"));
                link.Click();
            });

        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");
    }

    /// <summary>
    /// Switches the Stock Requests subgrid to the specified view.
    /// </summary>
    /// <param name="viewName">The name of the view e.g. 'All Stock Requests'.</param>
    [When(@"I change the view to '(.*)' in the Stock Requests subgrid")]
    public void WhenIChangeTheViewToInTheStockRequestsSubgrid(string viewName)
    {
        Driver.WaitForTransaction();

        XrmApp.Entity.SubGrid.SwitchView(StockRequestsSubGridName, viewName);

        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Switches the Work Order Tasks subgrid to the specified view.
    /// </summary>
    /// <param name="viewName">The name of the view e.g. 'All Work Order Service Tasks'.</param>
    [When(@"I change the view to '(.*)' in the Work Order Tasks subgrid")]
    public void WhenIChangeTheViewToInTheWorkOrderTasksSubgrid(string viewName)
    {
        Driver.WaitForTransaction();

        XrmApp.Entity.SubGrid.SwitchView("workorderservicetasksgrid", viewName);

        Driver.WaitForTransaction();
    }

    [When("I select a row in the Commodity Lines subgrid")]
    public void WhenISelectARowInTheCommodityLinesSubgrid()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .Or<ElementClickInterceptedException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var rowCheckMark = Driver.WaitUntilAvailable(
                    By.XPath("(//div[@data-id='Commoditylines-powerapps_onegrid_control_container']//div[@role='row'][.//div[@role='gridcell']]//i[@data-icon-name='CheckMark'])[1]"),
                    "Row selector checkbox could not be found in the Commodity Lines subgrid.");

                var row = rowCheckMark.FindElement(By.XPath("./ancestor::div[@role='row'][1]"));
                if (row.GetAttribute("aria-selected") != "true")
                {
                    var checkbox = rowCheckMark.FindElement(By.XPath("./ancestor::*[contains(@class,'ms-Checkbox')][1]"));
                    checkbox.Click();
                }
            });

        Driver.WaitForTransaction();
    }

    [When("I click the Create Stock button")]
    public void WhenIClickTheCreateStockButton()
    {
        Driver.WaitForTransaction();

        var createStockButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='trd_consignmentitem|NoRelationship|SubGridStandard|trd.trd_consignmentitem.CreateStock.Button']"),
            "Create Stock button could not be found above the Commodity Lines subgrid.");

        Driver.ExecuteScript("arguments[0].click();", createStockButton);
        Driver.WaitForTransaction();
    }

    [Then(@"the '(.*)' view is displayed in the Stock Requests subgrid")]
    public void ThenTheViewIsDisplayedInTheStockRequestsSubgrid(string expectedViewName)
    {
        Driver.WaitForTransaction();

        string actualViewName = null;

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .OrResult<string>(value => value != expectedViewName)
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var viewLabel = Driver.WaitUntilAvailable(
                    By.XPath($"{StockRequestsSubgridXPath}/ancestor::div[.//div[starts-with(@data-id,'ViewSelector_')]][1]//span[contains(@id,'ViewSelector_') and contains(@id,'_text-value')]"),
                    "Stock Requests subgrid view selector could not be found.");

                return actualViewName = viewLabel.Text.Trim();
            });

        actualViewName.Should().Be(expectedViewName,
            $"Expected Stock Requests subgrid view to be '{expectedViewName}' but found '{actualViewName}'.");
    }

    [When("I open the record for the stock reference created in the Exporter Portal")]
    public void WhenIOpenTheRecordForTheStockReferenceCreatedInTheExporterPortal()
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
            .Or<ElementClickInterceptedException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                var cell = GetStockRequestRowCell(StockRequestsStatusReasonColumnId);
                new Actions(Driver).DoubleClick(cell).Perform();
            });

        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");
    }

    private int? GetStockRequestsRowCountWithRefresh(int expectedRowCount)
    {
        Driver.WaitForTransaction();

        return Policy
            .Handle<Exception>()
            .OrResult<int?>(count => count != expectedRowCount)
            .WaitAndRetry(6, _ =>
            {
                XrmApp.Entity.SubGrid.ClickCommand(StockRequestsSubGridName, "Refresh");
                Driver.WaitForTransaction();
                return TimeSpan.FromSeconds(5);
            })
            .Execute(() =>
            {
                Driver.WaitForTransaction();
                return GetSubgridRowCount(StockRequestsSubgridXPath);
            });
    }

    /// <summary>
    /// Returns the row count of a subgrid, preferring the "Rows: X" footer, falling back to
    /// the rendered data rows. Returns null if the subgrid grid has not rendered yet.
    /// </summary>
    private int? GetSubgridRowCount(string containerXPath)
    {
        var containers = Driver.FindElements(By.XPath(containerXPath)).Where(e => e.Displayed).ToList();

        if (containers.Count == 0 || containers[^1].FindElements(By.XPath(".//div[@role='grid' or @role='treegrid']")).Count == 0)
        {
            return null;
        }

        var container = containers[^1];

        var footerElements = container.FindElements(
            By.XPath(".//span[contains(@class,'statusTextContainer') and contains(text(),'Rows:')]"));

        if (footerElements.Count > 0 &&
            int.TryParse(footerElements[0].Text.Replace("Rows:", string.Empty).Trim(), out var footerCount))
        {
            return footerCount;
        }

        if (container.FindElements(By.XPath(".//div[contains(@class,'ag-center-cols-container')]")).Count > 0)
        {
            return container.FindElements(
                By.XPath(".//div[contains(@class,'ag-center-cols-container')]//div[@role='row']")).Count;
        }

        return container.FindElements(By.XPath(".//div[@role='row'][.//div[@role='gridcell']]")).Count;
    }

    /// <summary>
    /// Locates the Stock Requests subgrid row whose Stock column matches the stock reference
    /// stored in the scenario context as 'StockRequestName'.
    /// </summary>
    private IWebElement GetStockRequestRow()
    {
        var stockName = scenarioContext.Get<string>("StockRequestName");

        return Driver.WaitUntilAvailable(
            By.XPath($"{StockRequestsSubgridXPath}//div[@col-id='{StockRequestsStockColumnId}' and " +
                     $"(normalize-space(.)='{stockName}' or .//a[@aria-label='{stockName}'])]/ancestor::div[@role='row'][1]"),
            $"Stock Requests subgrid row for stock reference '{stockName}' could not be found.");
    }

    /// <summary>
    /// Returns the cell for the given col-id in the Stock Requests subgrid row for the stock reference.
    /// AG grid renders pinned and centre columns in separate row elements sharing the same row-index,
    /// so the cell is resolved by row-index rather than within the matched row element.
    /// </summary>
    private IWebElement GetStockRequestRowCell(string colId)
    {
        ScrollStockRequestsGridUntilColumnVisible(colId);

        var row = GetStockRequestRow();
        var rowIndex = row.GetAttribute("row-index");

        if (string.IsNullOrEmpty(rowIndex))
        {
            return row.FindElement(By.XPath($".//div[@col-id='{colId}']"));
        }

        return Driver.WaitUntilAvailable(
            By.XPath($"{StockRequestsSubgridXPath}//div[@role='row' and @row-index='{rowIndex}']//div[@col-id='{colId}']"),
            $"Cell '{colId}' could not be found in the Stock Requests subgrid row {rowIndex}.");
    }

    /// <summary>
    /// Horizontally scrolls the Stock Requests AG grid until a cell for the given col-id is rendered.
    /// </summary>
    private void ScrollStockRequestsGridUntilColumnVisible(string colId)
    {
        var findAndCheckScript = $@"
            var container = document.querySelector('div[data-control-name=""{StockRequestsSubGridName}""]');
            if (!container) return null;
            var viewport = container.querySelector('.ag-body-horizontal-scroll-viewport')
                || container.querySelector('.ag-center-cols-viewport');
            if (!viewport) return null;
            return container.querySelector('div[role=""row""] div[col-id=""{colId}""]') !== null ? 'visible' : 'hidden';";

        var scrollIncrementScript = $@"
            var container = document.querySelector('div[data-control-name=""{StockRequestsSubGridName}""]');
            if (!container) return;
            var viewport = container.querySelector('.ag-body-horizontal-scroll-viewport')
                || container.querySelector('.ag-center-cols-viewport');
            if (viewport) {{ viewport.scrollLeft += 300; }}";

        const int gridReadyAttempts = 20;
        const int maxScrollAttempts = 40;

        string result = null;
        for (var waitAttempt = 0; waitAttempt < gridReadyAttempts && result == null; waitAttempt++)
        {
            result = Driver.ExecuteScript(findAndCheckScript) as string;

            if (result == null)
            {
                Driver.WaitForTransaction();
            }
        }

        if (result == null)
        {
            throw new InvalidOperationException(
                $"The Stock Requests subgrid scroll container could not be found after waiting " +
                $"{gridReadyAttempts} attempts. The grid may not have finished rendering.");
        }

        for (var scrollAttempt = 0; scrollAttempt < maxScrollAttempts; scrollAttempt++)
        {
            if (result == "visible")
            {
                return;
            }

            Driver.ExecuteScript(scrollIncrementScript);
            Driver.WaitForTransaction();

            result = Driver.ExecuteScript(findAndCheckScript) as string
                ?? throw new InvalidOperationException(
                    "The Stock Requests subgrid scroll container disappeared during scrolling.");
        }

        throw new InvalidOperationException(
            $"Could not scroll the Stock Requests subgrid to show column '{colId}' after {maxScrollAttempts} scroll attempts.");
    }

    /// <summary>
    /// Reads the selected tag text of a lookup field.
    /// as the lookup tag is lazy-rendered, retrying on stale elements.
    /// </summary>
    private string GetLookupSelectedTagText(string fieldSchemaName, string selectedTagTextDataId)
    {
        Driver.WaitForTransaction();

        string text = null;

        Policy
            .Handle<StaleElementReferenceException>()
            .Or<NoSuchElementException>()
            .Or<WebDriverTimeoutException>()
            .Or<InvalidOperationException>()
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

                var tag = Driver.WaitUntilAvailable(
                    By.XPath($"//div[@data-id='{selectedTagTextDataId}']"),
                    $"Lookup value '{selectedTagTextDataId}' could not be found.");

                return text = tag.Text.Trim();
            });

        return text;
    }

    [When(@"I check that the Commodity Lines frame shows '(.*)'")]
    public void WhenICheckThatTheCommodityLinesFrameShows(string expectedViewName)
    {
        Driver.WaitForTransaction();

        var viewLabel = Driver.WaitUntilAvailable(
            By.XPath($"//span[contains(@id,'ViewSelector_') and contains(@id,'_text-value') and normalize-space(text())='{expectedViewName}']"),
            $"Commodity Lines frame view '{expectedViewName}' could not be found.");
        
        viewLabel.Text.Trim().Should().Be(expectedViewName,
            $"Expected Commodity Lines frame to show '{expectedViewName}' but found '{viewLabel.Text.Trim()}'.");
    }

    private static readonly string[] ValidRegulatoryAuthorities = ["PHSI", "HMI", "Joint"];
    private const string EppoCodeColumnDataId = "trd_eppocode";
    private const string RegulatoryAuthorityColumnDataId = "trd_regulatoryauthoritycode";

    /// <summary>
    /// Verifies that all Import Commodity Lines on the Work Order match the EPPO codes
    /// provided in the test input, and that each line has a valid Regulatory Authority.
    /// </summary>
    [Then(@"all the Commodity Lines should be validated with the values given in the input")]
    public void ThenAllTheCommodityLinesShouldBeValidatedWithTheValuesGivenInTheInput()
    {
        Driver.WaitForTransaction();

        var inputTable = scenarioContext["AllCommodityDetails"] as Table;
        inputTable.Should().NotBeNull("AllCommodityDetails was not found in the scenario context.");

        var expectedRowCount = inputTable.Rows.Count;

        // Step 1: Poll until all commodity lines have loaded, refreshing between attempts.
        WaitForCommodityLinesToLoad(expectedRowCount, timeout: TimeSpan.FromMinutes(10));

        // Step 2: Sort the grid by EPPO code ascending so page order matches our in-memory sort.
        SortCommodityLinesByEppoCodeAscending();
        Driver.WaitForTransaction();
        Thread.Sleep(3000);

        // Step 3: Collect all rows across every page, retrying the entire collection if the
        // total falls short. Wijmo virtualises both columns AND rows — a single page may return
        // fewer rows than expected even after ExtractRowsFromCurrentPage retries, because the
        // DOM row count itself can be reduced by vertical virtualisation when the grid is
        // scrolled horizontally. Retrying the full collection from page 1 is the only reliable
        // way to recover from this, since each retry starts fresh after a sort reset.
        List<(string EppoCode, string RegulatoryAuthority)> collectedRows = null;
        var pageNumber = 1;
        const int maxCollectionAttempts = 3;

        for (var collectionAttempt = 1; collectionAttempt <= maxCollectionAttempts; collectionAttempt++)
        {
            collectedRows = new List<(string EppoCode, string RegulatoryAuthority)>();
            pageNumber = 1;

            while (true)
            {
                Driver.WaitForTransaction();
                Thread.Sleep(1500);

                // Ensure both EPPO Code (col 5) and Regulatory Authority (col 11) are in the DOM.
                ScrollGridUntilBothColumnsVisible();

                var pageRows = ExtractRowsFromCurrentPage();
                collectedRows.AddRange(pageRows);

                var nextButtons = Driver.FindElements(By.XPath("//button[contains(@id,'_nextPage')]"));

                if (!nextButtons.Any() || nextButtons[0].GetAttribute("disabled") != null)
                {
                    break;
                }

                Driver.ExecuteScript("arguments[0].click();", nextButtons[0]);
                pageNumber++;
            }

            if (collectedRows.Count >= expectedRowCount)
            {
                break;
            }

            Console.WriteLine(
                $"[COLLECT ROWS] Attempt {collectionAttempt}/{maxCollectionAttempts}: " +
                $"collected {collectedRows.Count} of {expectedRowCount} rows across {pageNumber} page(s). " +
                $"Refreshing and retrying from page 1.");

            // Refresh to reset pagination back to page 1, then re-sort before the next attempt.
            CommandSteps.WhenISelectTheCommand("Refresh");
            Driver.WaitForTransaction();
            SortCommodityLinesByEppoCodeAscending();
            Driver.WaitForTransaction();
            Thread.Sleep(3000);
        }

        // Step 4: Sort expected rows the same way as the grid.
        var sortedExpected = inputTable.Rows
            .OrderBy(r => r["EPPO code"])
            .ToList();

        // Step 5: Assert total count.
        collectedRows.Count.Should().Be(sortedExpected.Count,
            $"Expected {sortedExpected.Count} commodity lines but collected {collectedRows.Count} " +
            $"across {pageNumber} page(s) after {maxCollectionAttempts} collection attempt(s).");

        // Step 6: Validate each row's EPPO code and Regulatory Authority.
        for (var i = 0; i < sortedExpected.Count; i++)
        {
            var expectedEppo = sortedExpected[i]["EPPO code"].Trim();
            var actualEppo = collectedRows[i].EppoCode;

            actualEppo.Should().Be(expectedEppo,
                $"Row {i + 1}: Expected EPPO code '{expectedEppo}' but found '{actualEppo}'.");

            collectedRows[i].RegulatoryAuthority.Should().BeOneOf(ValidRegulatoryAuthorities,
                $"Row {i + 1} (EPPO: '{actualEppo}'): Regulatory Authority should be one of the valid values.");
        }
    }

    /// <summary>
    /// Extracts EPPO code and Regulatory Authority from every data row on the currently visible
    /// grid page. Uses the pagination label as the expected row count where available, falling
    /// back to the DOM row count. Resets vertical scroll to 0 on each retry to counteract
    /// Wijmo row virtualisation caused by a non-zero scrollTop.
    /// </summary>
    private List<(string EppoCode, string RegulatoryAuthority)> ExtractRowsFromCurrentPage()
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            try
            {
                var grid = Driver.WaitUntilAvailable(
                    By.XPath("//div[@role='grid'][contains(@aria-label,'Import Commodity Lines')]"),
                    "Commodity Lines grid could not be found while extracting rows.");

                var rows = grid.FindElements(By.XPath(".//div[@role='row'][@aria-label='Data']"));
                var domRowCount = rows.Count;

                if (domRowCount == 0)
                {
                    Thread.Sleep(1000);
                    Driver.WaitForTransaction();
                    continue;
                }

                // Prefer the pagination label count as ground truth over the DOM row count.
                // The DOM count can be reduced by vertical virtualisation; the label always
                // reflects the true page size regardless of what Wijmo has rendered.
                var expectedOnPage = TryReadExpectedRowsOnCurrentPage() ?? domRowCount;

                var results = new List<(string EppoCode, string RegulatoryAuthority)>();

                foreach (var row in rows)
                {
                    var eppoSpan = row.FindElements(
                        By.XPath(".//div[@role='gridcell'][@aria-colindex='5']//span[@role='presentation']"))
                        .FirstOrDefault();

                    var regAuthSpan = row.FindElements(
                        By.XPath(".//div[@role='gridcell'][@aria-colindex='11']//span[@role='presentation']"))
                        .FirstOrDefault();

                    if (eppoSpan != null && regAuthSpan != null)
                    {
                        results.Add((eppoSpan.Text.Trim(), regAuthSpan.Text.Trim()));
                    }
                }

                if (results.Count < expectedOnPage)
                {
                    Console.WriteLine(
                        $"[EXTRACT ROWS] Attempt {attempt + 1}: collected {results.Count} of " +
                        $"{expectedOnPage} expected rows (DOM count: {domRowCount}). " +
                        $"Re-scrolling and resetting vertical scroll before retrying.");

                    ScrollGridUntilBothColumnsVisible();
                    Driver.WaitForTransaction();
                    Thread.Sleep(500);
                    continue;
                }

                return results;
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(1000);
                Driver.WaitForTransaction();
            }
        }

        return new List<(string EppoCode, string RegulatoryAuthority)>();
    }

    /// <summary>
    /// Counts the total number of commodity line data rows across all pages
    /// without collecting cell values, purely to check whether loading is complete.
    /// Leaves the grid on the last visited page.
    /// </summary>
    private int CountCommodityLinesAcrossAllPages()
    {
        var total = 0;

        while (true)
        {
            Driver.WaitForTransaction();

            var grid = Driver.WaitUntilAvailable(
                By.XPath("//div[@role='grid'][contains(@aria-label,'Import Commodity Lines')]"),
                "Import Commodity Lines grid could not be found while counting rows.");

            // No horizontal scroll occurs here, so the Wijmo frozen column panel (wj-part='fcells')
            // is not rendered — the plain row query is safe and correct in this context.
            var dataRows = grid.FindElements(
                By.XPath(".//div[@role='row'][@aria-label='Data']"));

            total += dataRows.Count;

            var nextButton = Driver.WaitUntilAvailable(
                By.XPath("//button[contains(@id,'_nextPage')]"),
                "Pagination next button could not be found while counting rows.");

            if (nextButton.GetAttribute("disabled") != null)
            {
                break;
            }

            // Scroll the button into view and click via JavaScript to avoid
            // ElementClickInterceptedException when an overlay div covers the button.
            Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", nextButton);
            Driver.WaitForTransaction();
            Driver.ExecuteScript("arguments[0].click();", nextButton);
        }

        return total;
    }

    /// <summary>
    /// Polls the Import Commodity Lines grid by counting rows across all pages,
    /// refreshing the form between attempts, until the expected count is reached
    /// or the timeout expires. Always refreshes on exit to reset the grid to page 1,
    /// since <see cref="CountCommodityLinesAcrossAllPages"/> leaves the grid on the
    /// last page after paginating through all pages.
    /// </summary>
    /// <param name="expectedRowCount">The total number of commodity lines expected.</param>
    /// <param name="timeout">How long to keep retrying before giving up.</param>
    private void WaitForCommodityLinesToLoad(int expectedRowCount, TimeSpan timeout)
    {
        var deadline = DateTime.UtcNow.Add(timeout);

        while (DateTime.UtcNow < deadline)
        {
            var currentCount = CountCommodityLinesAcrossAllPages();

            // Always refresh after counting — resets the grid to page 1 regardless of
            // whether the count was met, so callers always start from a known position.
            CommandSteps.WhenISelectTheCommand("Refresh");
            Driver.WaitForTransaction();

            if (currentCount >= expectedRowCount)
            {
                return;
            }
        }

        // Final count after timeout — let the assertion produce a clear failure message.
        var finalCount = CountCommodityLinesAcrossAllPages();
        CommandSteps.WhenISelectTheCommand("Refresh");
        Driver.WaitForTransaction();

        finalCount.Should().BeGreaterOrEqualTo(expectedRowCount,
            $"Timed out after {timeout.TotalMinutes} minutes waiting for {expectedRowCount} " +
            $"commodity lines to load. Final count across all pages was {finalCount}.");
    }
    
    private void SortCommodityLinesByEppoCodeAscending()
    {
        var eppoColumnHeaderButton = Driver.WaitUntilAvailable(
            By.XPath($"//div[contains(@id,'_headerButton{EppoCodeColumnDataId}')]"),
            "EPPO Code column header button could not be found.");

        eppoColumnHeaderButton.Click();
        Driver.WaitForTransaction();

        var sortAtoZMenuItem = Driver.WaitUntilAvailable(
            By.XPath("//button[@name='Sort A to Z' and @role='menuitemradio']"),
            "Sort A to Z menu item could not be found.");

        Driver.ExecuteScript("arguments[0].click();", sortAtoZMenuItem);
        Driver.WaitForTransaction();
    }

    private void ScrollGridUntilBothColumnsVisible()
    {
        const string findScrollRootAndCheckScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) return null;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (!scrollRoot) return null;
            var eppoVisible = grid.querySelector('div[role=""gridcell""][aria-colindex=""5""]') !== null;
            var regAuthVisible = grid.querySelector('div[role=""gridcell""][aria-colindex=""11""]') !== null;
            return JSON.stringify({
                scrollLeft: scrollRoot.scrollLeft,
                scrollWidth: scrollRoot.scrollWidth,
                clientWidth: scrollRoot.clientWidth,
                eppoVisible: eppoVisible,
                regAuthVisible: regAuthVisible
            });";

        const string scrollIncrementScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) return;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollLeft += 300; }";

        // After achieving horizontal visibility, reset vertical scroll to 0 so Wijmo renders
        // rows from the top of the page. Without this reset, a non-zero scrollTop (caused by
        // prior horizontal scrolling or page navigation) clips the bottom row out of the
        // viewport — Wijmo de-virtualises it, reducing DOM row count from 50 to 49 on every
        // attempt, including outer collection retries.
        const string resetVerticalScrollScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) return;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollTop = 0; }";

        const int gridReadyAttempts = 20;
        const int maxScrollAttempts = 40;

        string resultJson = null;
        for (var waitAttempt = 0; waitAttempt < gridReadyAttempts; waitAttempt++)
        {
            resultJson = Driver.ExecuteScript(findScrollRootAndCheckScript) as string;

            if (resultJson != null)
            {
                break;
            }

            Driver.WaitForTransaction();
        }

        if (resultJson == null)
        {
            throw new InvalidOperationException(
                $"The Import Commodity Lines grid scroll container could not be found after waiting " +
                $"{gridReadyAttempts} attempts. The grid may not have finished rendering.");
        }

        for (var scrollAttempt = 0; scrollAttempt < maxScrollAttempts; scrollAttempt++)
        {
            var eppoVisible = resultJson.Contains(@"""eppoVisible"":true");
            var regAuthVisible = resultJson.Contains(@"""regAuthVisible"":true");

            if (eppoVisible && regAuthVisible)
            {
                // Both columns are in the DOM. Reset vertical scroll to 0 so Wijmo renders
                // rows starting from the top of the page — prevents the bottom row being
                // clipped by a non-zero scrollTop accumulated during horizontal scrolling.
                Driver.ExecuteScript(resetVerticalScrollScript);
                Driver.WaitForTransaction();
                return;
            }

            Driver.ExecuteScript(scrollIncrementScript);
            Driver.WaitForTransaction();

            resultJson = Driver.ExecuteScript(findScrollRootAndCheckScript) as string
                ?? throw new InvalidOperationException(
                    "The Import Commodity Lines grid scroll container disappeared during scrolling.");
        }

        throw new InvalidOperationException(
            $"Could not scroll the Commodity Lines grid to show both EPPO Code (col 5) and " +
            $"Regulatory Authority (col 11) simultaneously after {maxScrollAttempts} scroll attempts. " +
            $"Consider reviewing the column layout or increasing the max attempts.");
    }

    /// <summary>
    /// Reads the expected number of data rows on the current grid page from the pagination
    /// label (e.g. "1 - 50 of 500"). Returns null if the label cannot be parsed, in which
    /// case callers should fall back to the DOM row count.
    /// </summary>
    private int? TryReadExpectedRowsOnCurrentPage()
    {
        // Dynamics renders the pagination range as a label like "1 - 50 of 500".
        // The containing element varies by grid type; search broadly by text pattern.
        var paginationLabels = Driver.FindElements(
            By.XPath("//*[contains(@aria-label,'of') and contains(@aria-label,' - ')]"));

        foreach (var label in paginationLabels)
        {
            var text = (label.GetAttribute("aria-label") ?? label.Text ?? string.Empty).Trim();

            // Expected format: "X - Y of Z"
            var match = System.Text.RegularExpressions.Regex.Match(
                text, @"(\d+)\s*-\s*(\d+)\s+of\s+(\d+)");

            if (match.Success &&
                int.TryParse(match.Groups[1].Value, out var from) &&
                int.TryParse(match.Groups[2].Value, out var to))
            {
                return to - from + 1;
            }
        }

        return null;
    }

    [When(@"I sort Commodity Lines by Regulatory Authority")]
    public void WhenISortCommodityLinesByRegulatoryAuthority()
    {
        ScrollGridUntilBothColumnsVisible();
        Driver.WaitForTransaction();

        var regulatoryAuthorityHeaderButton = Driver.WaitUntilAvailable(
            By.XPath($"//div[contains(@id,'_headerButton{RegulatoryAuthorityColumnDataId}')]"),
            "Regulatory Authority column header button could not be found.");

        regulatoryAuthorityHeaderButton.Click();
        Driver.WaitForTransaction();

        var sortAtoZMenuItem = Driver.WaitUntilAvailable(
            By.XPath("//button[@name='Sort A to Z' and @role='menuitemradio']"),
            "Sort A to Z menu item could not be found.");

        sortAtoZMenuItem.Click();
        Driver.WaitForTransaction();

        Thread.Sleep(3000);
    }

    [When(@"I double click on a Commodity Line with Regulatory Authority set to '(.*)'")]
    public void WhenIDoubleClickOnACommodityLineWithRegulatoryAuthoritySetTo(string regulatoryAuthority)
    {
        // The same Gherkin step targets two physically different Wijmo grids:
        //   1. Work Order page  -> aria-label "Import Commodity Lines"          (12 cols)
        //   2. Commodity Lines tab on the Import Notification -> aria-label
        //      "Active Import Commodity Lines"                                  (18 cols)
        //
        // Both virtualise columns AND rows, so neither row index nor a fixed
        // aria-colindex can be assumed. The Regulatory Authority column is
        // identified by its header data-id ("trd_regulatoryauthoritycode"),
        // and the corresponding data cell is located by matching style.left
        // (Wijmo positions cells to align with their header).
        //
        // Row activation is performed with three layered strategies because
        // Selenium Actions.DoubleClick is unreliable on remote Selenium Grid
        // (Docker-based CI agents) — synthesised MouseEvents are dispatched
        // as a fallback, with Enter-key activation as a last resort.
        const int maxNavigationAttempts = 3;

        for (var navigationAttempt = 1; navigationAttempt <= maxNavigationAttempts; navigationAttempt++)
        {
            Driver.WaitForTransaction();

            // Ensure the Regulatory Authority column header is rendered into the DOM.
            ScrollCommodityLinesGridUntilColumnVisible(RegulatoryAuthorityColumnDataId);

            if (TryActivateCommodityLineByColumnValue(
                    columnDataId: RegulatoryAuthorityColumnDataId,
                    expectedValue: regulatoryAuthority))
            {
                return;
            }

            Console.WriteLine(
                $"[DOUBLE CLICK] Navigation attempt {navigationAttempt}/{maxNavigationAttempts}: " +
                $"Import Commodity Line page header not found after activating row for " +
                $"Regulatory Authority '{regulatoryAuthority}'. Retrying...");

            Driver.WaitForTransaction();
            Thread.Sleep(1500);
        }

        // Let downstream assertions (e.g. the page header check) produce the failure message.
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Scrolls the Commodity Lines Wijmo grid horizontally until the header with the given
    /// <paramref name="columnDataId"/> is rendered into the DOM, then resets vertical scroll
    /// to 0 so row virtualisation does not clip the bottom row.
    /// Works for both the Work Order subgrid and the Commodity Lines tab grid.
    /// </summary>
    private void ScrollCommodityLinesGridUntilColumnVisible(string columnDataId)
    {
        const string findGridScript = @"
            return document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]') !== null;";

        const string isHeaderRenderedScript = @"
            var dataId = arguments[0];
            return document.querySelector(
                '[wj-part=""ch""] [wj-part=""chcells""] .wj-cell.wj-header[data-id=""' + dataId + '""]') !== null;";

        const string scrollRightScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) { return; }
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollLeft += 300; }";

        const string resetVerticalScrollScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) { return; }
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollTop = 0; }";

        const int gridReadyAttempts = 20;
        const int maxScrollAttempts = 60;

        for (var i = 0; i < gridReadyAttempts; i++)
        {
            if ((bool)Driver.ExecuteScript(findGridScript))
            {
                break;
            }

            Driver.WaitForTransaction();
        }

        for (var attempt = 0; attempt < maxScrollAttempts; attempt++)
        {
            if ((bool)Driver.ExecuteScript(isHeaderRenderedScript, columnDataId))
            {
                Driver.ExecuteScript(resetVerticalScrollScript);
                Driver.WaitForTransaction();
                return;
            }

            Driver.ExecuteScript(scrollRightScript);
            Driver.WaitForTransaction();
        }

        throw new InvalidOperationException(
            $"Could not scroll the Commodity Lines grid to render the column with data-id " +
            $"'{columnDataId}' after {maxScrollAttempts} scroll attempts.");
    }

    /// <summary>
    /// Finds the data row whose cell under the column identified by <paramref name="columnDataId"/>
    /// equals <paramref name="expectedValue"/>, then attempts to activate (open) that row.
    /// Returns true once navigation to the Import Commodity Line page is confirmed.
    /// </summary>
    private bool TryActivateCommodityLineByColumnValue(string columnDataId, string expectedValue)
    {
        const int maxLocateAttempts = 5;

        for (var attempt = 0; attempt < maxLocateAttempts; attempt++)
        {
            try
            {
                var matchingCell = Driver.ExecuteScript(
                    @"
                    var dataId = arguments[0];
                    var target = (arguments[1] || '').toString().trim().toLowerCase();

                    var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
                    if (!grid) { return null; }

                    var header = document.querySelector(
                        '[wj-part=""ch""] [wj-part=""chcells""] .wj-cell.wj-header[data-id=""' + dataId + '""]');
                    if (!header) { return null; }

                    // Wijmo aligns each data cell to its header by style.left.
                    var headerLeft = header.style.left;

                    var rows = grid.querySelectorAll('div[role=""row""][aria-label=""Data""]');
                    for (var i = 0; i < rows.length; i++) {
                        var cells = rows[i].querySelectorAll('div[role=""gridcell""]');
                        for (var j = 0; j < cells.length; j++) {
                            if (cells[j].style.left !== headerLeft) { continue; }

                            // Title is set by Wijmo even when the inner span is virtualised away.
                            var title = (cells[j].getAttribute('title') || '').trim();
                            var span = cells[j].querySelector('span[role=""presentation""]');
                            var spanText = span ? span.textContent.trim() : '';
                            var value = (title.length > 0 ? title : spanText).toLowerCase();

                            if (value === target) {
                                return cells[j];
                            }
                            break;
                        }
                    }
                    return null;",
                    columnDataId,
                    expectedValue) as IWebElement;

                if (matchingCell == null)
                {
                    Driver.WaitForTransaction();
                    Thread.Sleep(500);
                    continue;
                }

                Driver.ExecuteScript(
                    "arguments[0].scrollIntoView({block:'center', inline:'center'});",
                    matchingCell);
                Driver.WaitForTransaction();

                // Strategy 1: native OS-level double-click via Selenium Actions.
                try
                {
                    new Actions(Driver).MoveToElement(matchingCell).DoubleClick().Perform();
                    Driver.WaitForTransaction();
                }
                catch (Exception)
                {
                    // Suppressed — fallbacks below will compensate.
                }

                if (IsImportCommodityLinePageDisplayed())
                {
                    return true;
                }

                // Strategy 2: synthesised MouseEvents. Reliable on remote Selenium Grid
                // (Docker) where Actions.DoubleClick is sometimes silently dropped.
                Driver.ExecuteScript(
                    @"
                    var cell = arguments[0];
                    if (!cell || !cell.getBoundingClientRect) { return; }
                    var rect = cell.getBoundingClientRect();
                    var cx = rect.left + rect.width / 2;
                    var cy = rect.top + rect.height / 2;
                    function fire(type, detail) {
                        cell.dispatchEvent(new MouseEvent(type, {
                            bubbles: true, cancelable: true, view: window,
                            clientX: cx, clientY: cy, button: 0, detail: detail
                        }));
                    }
                    fire('mousedown', 1); fire('mouseup', 1); fire('click', 1);
                    fire('mousedown', 2); fire('mouseup', 2); fire('click', 2);
                    fire('dblclick', 2);",
                    matchingCell);
                Driver.WaitForTransaction();

                if (IsImportCommodityLinePageDisplayed())
                {
                    return true;
                }

                // Strategy 3: keyboard activation — focus the row and press Enter.
                try
                {
                    Driver.ExecuteScript(
                        @"var row = arguments[0].closest('div[role=""row""]');
                          if (row) { row.setAttribute('tabindex','-1'); row.focus(); }",
                        matchingCell);
                    new Actions(Driver).MoveToElement(matchingCell).Click().SendKeys(Keys.Enter).Perform();
                    Driver.WaitForTransaction();
                }
                catch (Exception)
                {
                    // Suppressed — caller will retry the whole flow.
                }

                return IsImportCommodityLinePageDisplayed();
            }
            catch (StaleElementReferenceException)
            {
                Driver.WaitForTransaction();
                Thread.Sleep(500);
            }
        }

        return false;
    }

    private bool IsImportCommodityLinePageDisplayed()
    {
        return Driver
            .FindElements(By.XPath(
                "//span[@data-id='entity_name_span'][normalize-space(text())='Import Commodity Line']"))
            .Count > 0;
    }

    /// <summary>
    /// Asserts that the specified Work Order Task in the workorderservicetasksgrid has the expected Status.
    /// </summary>
    /// <param name="taskName">The name of the Work Order Task.</param>
    /// <param name="expectedStatus">The expected Status value (e.g., "Active", "Inactive").</param>
    [Then(@"the Work Order Task '(.*)' Status is '(.*)'")]
    public void ThenTheWorkOrderTaskStatusIs(string taskName, string expectedStatus)
    {
        Driver.WaitForTransaction();

        // Find the grid container for Work Order Service Tasks
        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        // Find all rows in the AG Grid
        var rows = grid.FindElements(By.XPath(".//div[contains(@class,'ag-center-cols-container')]/div[@role='row']"));
        rows.Should().NotBeEmpty("Expected at least one row in the Work Order Service Tasks grid.");

        // Find the row where the Name column (aria-colindex='2') matches the taskName
        IWebElement matchingRow = null;
        foreach (var row in rows)
        {
            var nameCell = row.FindElements(By.XPath(".//div[@role='gridcell'][@aria-colindex='2']//span[@role='presentation']"))
                .FirstOrDefault();
            if (nameCell != null && nameCell.Text.Trim().Equals(taskName, StringComparison.OrdinalIgnoreCase))
            {
                matchingRow = row;
                break;
            }
        }

        matchingRow.Should().NotBeNull($"Could not find a Work Order Task row with Name '{taskName}'.");

        // Status is in aria-colindex='4'
        var statusCell = matchingRow.FindElement(
            By.XPath(".//div[@role='gridcell'][@aria-colindex='4']//label[@aria-label]")
        );

        var actualStatus = statusCell.GetAttribute("aria-label").Trim();

        actualStatus.Should().Be(expectedStatus,
            $"Expected Status for Work Order Task '{taskName}' to be '{expectedStatus}' but found '{actualStatus}'.");
    }

    /// <summary>
    /// Asserts that the specified Work Order Task in the workorderservicetasksgrid has the expected % Complete value.
    /// </summary>
    /// <param name="taskName">The name of the Work Order Task.</param>
    /// <param name="expectedPercentComplete">The expected % Complete value (e.g., "100.00").</param>
    [Then(@"the Work Order Task '(.*)' % Complete is '(.*)'")]
    public void ThenTheWorkOrderTaskPercentCompleteIs(string taskName, string expectedPercentComplete)
    {
        Driver.WaitForTransaction();

        // Find the grid container for Work Order Service Tasks
        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        // Find all rows in the AG Grid
        var rows = grid.FindElements(By.XPath(".//div[contains(@class,'ag-center-cols-container')]/div[@role='row']"));
        rows.Should().NotBeEmpty("Expected at least one row in the Work Order Service Tasks grid.");

        // Find the row where the Name column (aria-colindex='2') matches the taskName
        IWebElement matchingRow = null;
        foreach (var row in rows)
        {
            var nameCell = row.FindElements(By.XPath(".//div[@role='gridcell'][@aria-colindex='2']//span[@role='presentation']"))
                .FirstOrDefault();
            if (nameCell != null && nameCell.Text.Trim().Equals(taskName, StringComparison.OrdinalIgnoreCase))
            {
                matchingRow = row;
                break;
            }
        }

        matchingRow.Should().NotBeNull($"Could not find a Work Order Task row with Name '{taskName}'.");

        // % Complete is in aria-colindex='6'
        var percentCell = matchingRow.FindElement(
            By.XPath(".//div[@role='gridcell'][@aria-colindex='6']//label[@aria-label]")
        );

        var actualPercent = percentCell.GetAttribute("aria-label").Trim();

        actualPercent.Should().Be(expectedPercentComplete,
            $"Expected % Complete for Work Order Task '{taskName}' to be '{expectedPercentComplete}' but found '{actualPercent}'.");
    }

    /// <summary>
    /// Opens the Related tab dropdown and selects the specified item by scrolling it into view
    /// within the flyout before clicking. Required because the flyout container has a fixed
    /// max-height with overflow-y:auto — items below the fold (e.g. 'Charges') are present in
    /// the DOM but not clickable until scrolled into view.
    /// </summary>
    /// <param name="relatedTabName">The aria-label of the Related menu item e.g. 'Charges'.</param>
    [When(@"I select the '(.*)' tab from the Related tab dropdown")]
    public void WhenISelectTheTabFromTheRelatedTabDropdown(string relatedTabName)
    {
        Driver.WaitForTransaction();

        // Locate the Related tab in the tablist. aria-expanded indicates whether the flyout is open.
        var relatedTab = Driver.WaitUntilAvailable(
            By.XPath("//ul[@role='tablist']//li[@role='tab' and @aria-haspopup='true' and @aria-label='Related']"),
            "Related tab could not be found in the tablist.");

        // Open the flyout only if it is not already expanded.
        var isExpanded = relatedTab.GetAttribute("aria-expanded");
        if (!string.Equals(isExpanded, "true", StringComparison.OrdinalIgnoreCase))
        {
            relatedTab.Click();
            Driver.WaitForTransaction();

            // Wait until aria-expanded flips to "true" before proceeding.
            Driver.WaitUntilAvailable(
                By.XPath("//ul[@role='tablist']//li[@role='tab' and @aria-haspopup='true' and @aria-label='Related' and @aria-expanded='true']"),
                "Related tab flyout did not open (aria-expanded did not become 'true').");
        }

        // Wait for the flyout container to be present and visible.
        Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='relatedTabMenuList']"),
            $"Related tab flyout menu could not be found when selecting '{relatedTabName}'.");

        // Find the menu item by its aria-label inside the flyout.
        var menuItem = Driver.WaitUntilAvailable(
            By.XPath($"//div[@data-id='relatedTabMenuList']//div[@role='menuitem' and @aria-label='{relatedTabName}']"),
            $"Related tab menu item '{relatedTabName}' could not be found in the flyout.");

        // Scroll the item into view within the flyout — the container has a fixed max-height
        // with overflow-y:auto so items below the fold are not clickable without scrolling.
        Driver.ExecuteScript("arguments[0].scrollIntoView({block:'nearest'});", menuItem);
        Driver.WaitForTransaction();

        menuItem.Click();
        Driver.WaitForTransaction();
    }

    [Given(@"'(.*)' has updated the status of the work order associated to '(.*)' to '(.*)'")]
    public void GivenHasUpdatedTheStatusOfTheWorkOrderAssociatedToTo(string userAlias, string applicationAlias, string subStatusName)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        var user = TestConfig.GetUser(userAlias);

        using (var svcClient = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svcClient))
        {
            var substatus = context.msdyn_workordersubstatusSet.Where(s => s.msdyn_name == subStatusName).FirstOrDefault();
            if (substatus == null)
            {
                throw new ArgumentException($"Unable to find a work order sub status of {subStatusName}.");
            }

            var userObjectId = context.SystemUserSet
                .Where(u => u.DomainName == user.Username)
                .Select(u => u.AzureActiveDirectoryObjectId)
                .FirstOrDefault();

            if (!userObjectId.HasValue)
            {
                throw new ArgumentException($"Unable to find a system user with a domain name of {user.Username}.");
            }

            var workOrder = GetWorkOrder(svcClient, application);
            WaitForWorkServiceTasks(svcClient, workOrder);
            svcClient.CallerAADObjectId = userObjectId.Value;
            svcClient.Update(new msdyn_workorder { Id = workOrder.Id, msdyn_SubStatus = substatus.ToEntityReference() });
        }
    }

    [Given(@"the work order associated to '(.*)' is assigned to me")]
    public void GivenTheWorkOrderAssociatedToTheIsAssignedToMe(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svcClient = SessionContext.GetServiceClient())
        {
            var workOrder = GetWorkOrder(svcClient, application);
            WaitForWorkServiceTasks(svcClient, workOrder);
            svcClient.AssignEntityToUser(this.sessionContext.UserId, msdyn_workorder.EntityLogicalName, workOrder.Id);
        }

        new AsyncSteps(this.sessionContext).WhenIWaitForAllServiceTasksToBeOwned(applicationAlias, applicationAlias.ToLower().Contains("import") ? SpecflowBindingsConstants.DefaultTaskCountOwnedByCITImportsTeam : 0);
    }

    [When(@"I select the '(.*)' tab on the work order task")]
    public void WhenISelectTheTabOnTheWorkOrderTask(string tabName)
    {
        this.ClickTab(tabName);
    }

    [When(@"I select inspector at position '(.*)' after using search criteria '(.*)'")]
    public void WhenISelectInspectorAtPositionAfterUsingSearchCriteria(int zeroBasedIndex, string searchCriteria)
    {
        this.SelectInspector(zeroBasedIndex, searchCriteria);
    }

    [When(@"I scroll into time recording inspectors")]
    public void WhenIScrollIntoTimeRecordingInspectors()
    {
        this.ScrollIntoElement();
    }

    [When(@"I refresh the time recordings until the '(.*)' grid contains '(.*)' row\(s\)")]
    public void WhenIRefreshTheTimeRecordingsUntilTheGridContainsRows(string gridName, int expectedRowCount)
    {
        if (this.RefreshCommands.Count.Equals(2))
        {
            Wait.Until(
                TimeSpan.FromSeconds(15),
                () => GridHelper.GetRows(Driver, gridName).Count.Equals(expectedRowCount),
                () => this.RefreshCommands[1].Click());
        }
        else
        {
            throw new AutomationException("Expected 2 refresh commands in work order task");
        }
    }

    [When(@"I save the work order task")]
    public void WhenISaveTheWorkOrderTask()
    {
        this.SaveWorkOrder();
    }

    [When(@"I assign workorder to myself")]
    public void WhenIAssignWorkorderToMyself()
    {
        new DataSteps(this.sessionContext).GivenIHaveOpened("WorkOrder");
        this.WhenIAssignTheWorkOrderToMe();
    }

    [Then(@"I can see the workorder status as '(.*)'")]
    public void ThenICanSeeTheWorkorderStatusAs(string status)
    {
        Policy
                .Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(5))
                .Execute(() =>
                {
                    new DataSteps(this.sessionContext).GivenIHaveOpened("WorkOrder");
                    ThenThereIsAValueOfInTheLookupHeaderField(status, "msdyn_substatus");
                });
    }

    [Given(@"I assign the work order to me")]
    [When(@"I assign the work order to me")]
    public void WhenIAssignTheWorkOrderToMe()
    {
        var formContext = Driver.WaitUntilAvailable(By.Id("mainContent"));
        CommandSteps.ClickCommand("Refresh");
        Driver.WaitForPageToLoad();
        CommandHelper.ClickCommand(Driver, formContext, "Assign");
        DialogSteps.WhenIAssignToMeOnTheAssignDialog();
    }

    [When(@"I close the work order task")]
    public void WhenICloseTheWorkOrderTask()
    {
        this.ClosePopupContainer.Click();
    }

    // TODO: Review if the binding in Capgemini.PowerApps.SpecFlowBindings doesn't work. If so, raise an issue on the repository.
    [Then(@"there is a value of '(.*)' in the '(.*)' lookup header field")]
    public void ThenThereIsAValueOfInTheLookupHeaderField(string expectedHeaderValue, string headerName)
    {
        Driver.WaitForTransaction();

        var headerValue = this.GetHeaderValue(headerName);

        headerValue.Should().Be(expectedHeaderValue);
    }

    [Then(@"there is a value in the '(.*)' lookup header field that does not contain (.*)")]
    public void ThenThereIsADifferentValueOfInTheLookupHeaderField(string headerName, string notMatching)
    {
        Driver.WaitForTransaction();

        var headerValue = this.GetHeaderValue(headerName);

        headerValue.Should().NotContain(notMatching);
    }

    [When(@"I click cancel on the assign work order dialog")]
    public void WhenIClickCancelOnTheAssignWorkOrderDialog()
    {
        this.ClickCancel();
    }

    [When(@"I select the '(.*)' tab on the popup dialog")]
    public void WhenITabOnTheWorkOrderPopupDialogTab(string popTabName)
    {
        this.ClickWorkOrderTab(popTabName);
    }

    [When(@"I click on '(.*)' button")]
    public void WhenIClickOnButtonOnTheTable(string buttonName)
    {
        this.ClickButtonOnTable(buttonName);
    }


    [Then(@"a new window with the phyto url field value is opened")]
    public void ThenANewWindowWithThePhytoUrlFieldValueIsOpened()
    {
        var expectedURL = this.GetPhytoURLValue();
        Driver.LastWindow();
        var actualURL = Driver.Url;
        actualURL.Should().Contain(expectedURL);
    }

    /// <summary>
    /// Clicks View Phyto in the Work Order ribbon and switches the Dynamics driver to the new
    /// tab containing the Phytosanitary certificate PDF. The PDF stays in Browser 2, so
    /// IsDynamicsActive remains true and the Dynamics hooks keep ownership of screenshots.
    /// </summary>
    [When("I click View Phyto command from the header ribbon")]
    public void WhenIClickViewPhytoCommandFromTheHeaderRibbon()
    {
        Driver.WaitForTransaction();
        var dynamicsHandle = Driver.CurrentWindowHandle;
        var handlesBefore = Driver.WindowHandles.ToList();

        const int maxAttempts = 2;
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            SignInPromptHelper.DismissSignInPrompts(Driver, "pre-view-phyto-click");

            CommandSteps.WhenISelectTheCommand("View Phyto");
            Driver.WaitForTransaction();

            try
            {
                new WebDriverWait(Driver, TimeSpan.FromSeconds(30))
                    .Until(d => d.WindowHandles.Count > handlesBefore.Count);
                break;
            }
            catch (WebDriverTimeoutException) when (attempt < maxAttempts)
            {
                Console.WriteLine("[ViewPhyto] Phyto certificate tab did not open within 30s, retrying ribbon click.");
            }
        }

        var phytoHandle = Driver.WindowHandles.Except(handlesBefore).Single();
        Driver.SwitchTo().Window(phytoHandle);

        scenarioContext["DynamicsWindowHandle"] = dynamicsHandle;
        scenarioContext["PhytoCertificateWindowHandle"] = phytoHandle;
    }

    [Then("the Phytosanitary certificate for {string} is displayed in a new window")]
    public void ThenThePhytosanitaryCertificateForIsDisplayedInANewWindow(string applicationName)
    {
        var phytoHandle = scenarioContext.Get<string>("PhytoCertificateWindowHandle");
        Driver.CurrentWindowHandle.Should().Be(phytoHandle, "the driver should be focused on the Phytosanitary certificate tab");

        var reference = scenarioContext.Get<string>($"{applicationName}_APHAReference");
        var expectedUrlPattern = new System.Text.RegularExpressions.Regex(
            $@"/protected/application/{System.Text.RegularExpressions.Regex.Escape(reference)}/r-numbers/\d+/certificate/?(\?.*)?$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        try
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(60)).Until(d =>
                expectedUrlPattern.IsMatch(d.Url)
                && ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete");
        }
        catch (WebDriverTimeoutException)
        {
            throw new InvalidOperationException(
                $"The Phytosanitary certificate window did not load the certificate URL for application '{reference}'. Actual URL: {Driver.Url}");
        }

        // Chromium (Chrome / Edge) renders a PDF as a viewer document wrapping an <embed type='application/pdf'>,
        // so check both the document content type and the embedded viewer.
        var isPdf = (bool)((IJavaScriptExecutor)Driver).ExecuteScript(
            "return document.contentType === 'application/pdf' || !!document.querySelector(\"embed[type='application/pdf']\");");
        isPdf.Should().BeTrue($"the new window should display the Phytosanitary certificate PDF. URL: {Driver.Url}");

        scenarioContext["PhytoCertificateUrl"] = Driver.Url;
    }

    /// <summary>
    /// Downloads the certificate PDF via PdfDownloadHelper, which re-uses the driver session cookies
    /// from the test host. Works locally and against the Selenium Grid (no browser download needed).
    /// The file path is stored under PhytoCertificatePdfPath for the PDF validation step in Defra.UI.Tests.
    /// </summary>
    [When("I download the Phytosanitary certificate PDF for {string}")]
    public void WhenIDownloadThePhytosanitaryCertificatePdfFor(string applicationName)
    {
        var pdfUrl = scenarioContext.TryGetValue("PhytoCertificateUrl", out string url) ? url : Driver.Url;
        var reference = scenarioContext.Get<string>($"{applicationName}_APHAReference");

        var pdfPath = PdfDownloadHelper.DownloadPdfUsingSession(Driver, pdfUrl, $"{reference}-certificate");

        scenarioContext["PDFDownloadedDirectory"] = Path.GetDirectoryName(pdfPath);
        scenarioContext["PhytoCertificatePdfPath"] = pdfPath;
    }

    [When("I close the PDF window")]
    public void WhenIClosePdfWindow()
    {
        var phytoHandle = scenarioContext.Get<string>("PhytoCertificateWindowHandle");
        var dynamicsHandle = scenarioContext.Get<string>("DynamicsWindowHandle");

        if (Driver.WindowHandles.Contains(phytoHandle))
        {
            Driver.SwitchTo().Window(phytoHandle);
            Driver.Close();
        }

        Driver.SwitchTo().Window(dynamicsHandle);
        Driver.WaitForTransaction();
        SignInPromptHelper.DismissSignInPrompts(Driver, "post-close-phyto-window");
    }

    [Then("the window is closed")]
    public void ThenTheWindowIsClosed()
    {
        var phytoHandle = scenarioContext.Get<string>("PhytoCertificateWindowHandle");
        var dynamicsHandle = scenarioContext.Get<string>("DynamicsWindowHandle");

        Driver.WindowHandles.Should().NotContain(phytoHandle, "the Phytosanitary certificate window should be closed");
        Driver.CurrentWindowHandle.Should().Be(dynamicsHandle, "focus should return to the Dynamics window");
    }

    [Then(@"I open and view the phyto certificate")]
    public void ThenIOpenAndViewThePhytoCertificate()
    {
        PopupSteps.WhenICloseThePopup();
        CommandSteps.WhenISelectTheCommand("View Phyto");
        ThenANewWindowWithThePhytoUrlFieldValueIsOpened();
    }

    /// <summary>
    /// Gets the cancel button for the assign dialog.
    /// </summary>
    public IWebElement CancelButton => Driver.WaitUntilAvailable(By.XPath("//button[@data-id='cancel_id']"));

    /// <summary>
    /// Clicks the cancel button on the assign dialog.
    /// </summary>
    public void ClickCancel()
    {
        this.CancelButton.Click();
    }

    public IWebElement HeaderFieldsExpand => Driver.WaitUntilAvailable(By.XPath("//button[@data-id='header_overflowButton']"));

    public IWebElement SupportingDocumentURL => Driver.WaitUntilAvailable(By.XPath("//*[contains(@data-id,'trd_documentsviewurl.fieldControl-url-action-icon')]"));

    public IWebElement PhytoURL => Driver.WaitUntilAvailable(By.XPath(".//*[@data-id='trd_phytourl.fieldControl-text-box-text']"));

    public string GetHeaderValue(string headerName)
    {
        this.HeaderFieldsExpand.Click();
        var header = Driver.WaitUntilAvailable(By.XPath($"//ul[@data-id='header_{headerName}.fieldControl-LookupResultsDropdown_{headerName}_SelectedRecordList']"));
        var headerValue = header.Text;
        this.HeaderFieldsExpand.Click();
        return headerValue;
    }

    public void ClickWorkOrderTab(string popTabName)
    {
        var tab = Driver.WaitUntilAvailable(By.XPath($"//li[@data-id='tablist-tab_{popTabName}']"));
        tab.Click();
    }

    public void ClickButtonOnTable(string buttonName)
    {
        Driver.WaitUntilAvailable(By.XPath($"//button[@title='{buttonName}' or @aria-label='{buttonName}']")).Click();
    }

    public string GetPhytoURLValue()
    {
        return this.PhytoURL.GetAttribute("value");
    }

    public IWebElement PopupContainer => Driver.WaitUntilAvailable(By.XPath("//section[contains(@id, 'popupContainer')]"));

    public IWebElement ClosePopupContainer => this.PopupContainer.FindElement(By.XPath(".//button[@data-id='dialogCloseIconButton']"));

    public IWebElement Save => this.PopupContainer.FindElement(By.XPath(".//span[@aria-label='Save']"));

    public IWebElement Search => this.PopupContainer.FindElement(By.XPath(".//div[@class='   css-1wa3eu0-placeholder']"));

    public IWebElement Close => this.PopupContainer.FindElement(By.XPath(".//span[contains (@class, 'ms-Button-label') and contains(string(), 'Close')]"));

    public IList<IWebElement> RefreshCommands => this.PopupContainer.FindElements(By.XPath(".//span[@aria-label='Refresh']"));

    public IList<IWebElement> InspectorGridCells => this.PopupContainer.FindElements(By.XPath(".//div[@data-automationid='DetailsRowCell']"));

    public IWebElement ScrollTimeRecording => this.PopupContainer.FindElement(By.XPath(".//label[contains(string(), 'Time Recording Inspectors')]"));

    public void ClickTab(string tabName)
    {
        Driver.WaitForTransaction();
        var tab = Driver.FindElement(By.XPath($"//li[@title='{tabName}' and contains(@role, 'tab')]"));
        tab.Click();
        Driver.WaitForTransaction();
    }

    public void SelectInspector(int zeroBasedIndex, string searchCriteria)
    {
        this.Search.Click();
        this.Search.InputText(searchCriteria);
        Wait.Until(TimeSpan.FromSeconds(10), () => this.InspectorGridCells.Count > 0);
        this.InspectorGridCells[zeroBasedIndex].Click();
        this.SaveWorkOrder();
    }

    public void SaveWorkOrder()
    {
        this.Save.Click();
    }

    public void ScrollIntoElement()
    {
        Driver.ExecuteScript("arguments[0].scrollIntoView(true);", this.ScrollTimeRecording);
    }

    public static EntityReference GetWorkOrder(CrmServiceClient svcClient, EntityReference application)
    {
        return svcClient.WaitForFieldValue<EntityReference>(application.LogicalName, application.Id, "trd_workorderid", TimeSpan.FromSeconds(5));
    }

    public static EntityCollection WaitForWorkServiceTasks(CrmServiceClient svcClient, EntityReference workOrder)
    {
        return svcClient.WaitForRecords(
            new QueryByAttribute(msdyn_workorderservicetask.EntityLogicalName)
            {
                Attributes = { "msdyn_workorder" },
                Values = { workOrder.Id }
            },
            TimeSpan.FromSeconds(90));
    }

    [Then(@"I can view the following Business process stages")]
    public void ThenICanViewTheFollowingBusinessProcessStages(Table table)
    {
        foreach (var row in table.Rows)
        {
            Driver.FindElement(By.XPath("//*[@title='" + row["Stages"] + "']"));
        }
    }

    /// <summary>
    /// Opens work-order for the current application. Assuming that there will be only one work-order for the given test.
    /// </summary>
    public void OpenWorkOrder()
    {
        ApplicationSteps.OpenEntity(this.sessionContext.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias));
    }

    public void AddWorkOrderToEntityHolder(EntityReference workOrder)
    {
        if (!this.sessionContext.Entities.ContainsKey(SpecflowBindingsConstants.WorkOrderAlias + this.sessionContext.SessionId))
        {
            this.sessionContext.Entities.Add($"{SpecflowBindingsConstants.WorkOrderAlias}{this.sessionContext.SessionId}",
                new EntityHolder
                {
                    Alias = SpecflowBindingsConstants.WorkOrderAlias,
                    EntityName = workOrder.LogicalName,
                    EntityCollectionName = workOrder.LogicalName + "s",
                    Id = workOrder.Id,
                });
        }
    }


    [When(@"I navigate to the service tasks grid on the work order")]
    public void WhenINavigateToTheServiceTasksGridOnTheWorkorder()
    {
        FormSteps.StoreFormValueInVariable("trd_workorderid", "Lookup", "field", "plntworkorderid", this.sessionContext);
        Capgemini.PowerApps.SpecFlowBindings.Steps.LookupSteps.WhenISelectARelatedLookupInTheForm("trd_workorderid");
        Capgemini.PowerApps.SpecFlowBindings.Steps.EntitySteps.ISelectTab("Work Order Tasks");
        Capgemini.PowerApps.SpecFlowBindings.Steps.EntitySubGridSteps.WhenISwitchToTheViewInTheSubgrid("All Work Order Service Tasks", "workorderservicetasksgrid");
    }
}
