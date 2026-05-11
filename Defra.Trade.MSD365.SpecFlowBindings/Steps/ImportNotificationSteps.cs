// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.SpecFlowBindings.Tables;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[Binding]
public class ImportNotificationSteps : PowerAppsStepDefiner
{
    private readonly SessionContext context;
    private readonly ScenarioContext scenarioContext;

    private static string EntityName
    {
        get
        {
            var entityName = string.Empty;
            Policy
                .Handle<WebDriverException>()
                .WaitAndRetry(30, retryAttempt => TimeSpan.FromSeconds(2))
                .Execute(() =>
                {
                    entityName = XrmApp.Entity.GetEntityName();
                });
            return entityName;
        }
    }

    public ImportNotificationSteps(SessionContext context, ScenarioContext scenarioContext)
    {
        this.context = context;
        this.scenarioContext = scenarioContext;
    }

    [Then(@"the '(.*)' view is displayed")]
    public void ThenTheViewIsDisplayed(string expectedViewName)
    {
        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");

        var viewSelectorButton = Driver.WaitUntilAvailable(
            By.XPath("//button[contains(@data-id,'ViewSelector') and not(contains(@data-id,'ViewSelector_1'))]"),
            $"View selector button could not be found.");

        var actualViewName = viewSelectorButton.GetAttribute("aria-label")?.Trim() ?? string.Empty;

        actualViewName.Should().Be(expectedViewName,
            $"Expected view to be '{expectedViewName}' but found '{actualViewName}'");
    }

    /// <summary>
    /// Switches the Import Notifications grid to the specified view.
    /// Uses XrmApp.Grid.SwitchView which clicks the ViewSelector button and selects
    /// the matching view by name from the dropdown — consistent with GridSteps.WhenISwitchToTheViewInTheGrid.
    /// </summary>
    /// <param name="viewName">The name of the view to switch to e.g. 'Inactive Import Notifications'.</param>
    [When(@"I change the Import Notifications view to '(.*)'")]
    public void WhenIChangeTheImportNotificationsViewTo(string viewName)
    {
        Driver.WaitForTransaction();

        XrmApp.Grid.SwitchView(viewName);

        Driver.WaitForTransaction();
    }

    [When("I search Import Notifications for the notification created in IPAFFS")]
    [When("I search Importer Notifications for the notification created in IPAFFS")]
    public void WhenISearchImporterNotificationsForTheNotificationCreatedInIPAFFS()
    {              
        var chedReference = scenarioContext.Get<string>("CHEDReference");
        XrmApp.Grid.Search(chedReference);
        Driver.WaitForTransaction();
    }

    [Then("the notification created in IPAFFS should be returned")]
    public void ThenTheNotificationCreatedInIPAFFSShouldBeReturned()
    {
        var expectedChedReference = scenarioContext.Get<string>("CHEDReference");

        // Retry to handle transient UI delays where the grid has not yet filtered
        // down to the expected result — the footer row count is the most reliable
        // indicator that the search has completed and the grid has refreshed.
        Policy
            .Handle<Exception>()
            .OrResult<int?>(count => count == null || count != 1)
            .WaitAndRetry(
                retryCount: 10,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(3))
            .Execute(() =>
            {
                Driver.WaitForTransaction();
                return GetGridRowCountFromFooter();
            });

        // Final assertion after retries — produces a clear failure message if still not exactly 1 row.
        Driver.WaitForTransaction();

        var rowCount = GetGridRowCountFromFooter() ?? 0;

        rowCount.Should().Be(1,
            $"Expected exactly 1 result for CHED reference '{expectedChedReference}' but found {rowCount}.");

        var chedReferenceCell = Driver.WaitUntilAvailable(
            By.XPath("//div[@class='ag-center-cols-container']//div[@role='row'][1]//div[@col-id='trd_chedppreference']//a"),
            $"CHED reference cell could not be found in the grid.");

        var actualChedReference = chedReferenceCell.Text.Trim();

        actualChedReference.Should().Be(expectedChedReference,
            $"Expected CHED reference '{expectedChedReference}' but found '{actualChedReference}'.");
    }

    /// <summary>
    /// Verifies that the notification created in IPAFFS is no longer returned in the search results.
    /// If the record is still present, repeats the search every 30 seconds for up to 30 minutes —
    /// there is a processing delay in Dynamics after IPAFFS checks are completed before the record
    /// is removed from the Active Import Notifications view.
    /// Row count is read from the grid footer "Rows: X" span, which is the most reliable indicator
    /// of actual grid row count — the ag-grid row divs may not be present even when data is shown.
    /// </summary>
    [Then("the notification created in IPAFFS should not be returned")]
    public void ThenTheNotificationCreatedInIPAFFSShouldNotBeReturned()
    {
        var expectedChedReference = scenarioContext.Get<string>("CHEDReference");

        Policy
            .Handle<Exception>()
            .OrResult<int?>(count => count == null || count > 0)
            .WaitAndRetry(
                retryCount: 60,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(30),
                onRetry: (_, _, _, _) =>
                {
                    // Wait has elapsed — clear and re-search before the next Execute check.
                    XrmApp.Grid.ClearSearch();
                    Driver.WaitForTransaction();
                    XrmApp.Grid.Search(expectedChedReference);
                    Driver.WaitForTransaction();
                })
            .Execute(() =>
            {
                // Check the current search results (first run uses the prior step's search).
                Driver.WaitForTransaction();
                return GetGridRowCountFromFooter();
            });

        // Final assertion after retries are exhausted — produces a clear failure message if still present.
        Driver.WaitForTransaction();

        var remainingRowCount = GetGridRowCountFromFooter() ?? 0;

        remainingRowCount.Should().Be(0,
            $"Expected CHED reference '{expectedChedReference}' to no longer appear in the Active Import Notifications " +
            $"view after waiting up to 30 minutes, but {remainingRowCount} row(s) were still returned.");
    }

    /// <summary>
    /// Reads the grid row count from the footer "Rows: X" span.
    /// Returns null if the footer element cannot be found (grid not yet rendered — keep retrying).
    /// Returns 0 only when the footer explicitly shows "Rows: 0" (record is confirmed gone).
    /// Returns a positive count when results are still present.
    /// </summary>
    private int? GetGridRowCountFromFooter()
    {
        var footerElements = Driver.FindElements(
            By.XPath("//span[contains(@class,'statusTextContainer') and contains(text(),'Rows:')]"));

        if (footerElements.Count == 0)
            return null; // footer not present yet — grid may still be loading

        var footerText = footerElements[0].Text.Trim(); // e.g. "Rows: 3"
        var rawCount = footerText.Replace("Rows:", string.Empty).Trim();

        return int.TryParse(rawCount, out var count) ? count : null;
    }

    [Then("I verify the Import Notification page is displayed for the notification created in IPAFFS")]
    public void ThenIVerifyTheImportNotificationPageIsDisplayedForTheNotificationCreatedInIPAFFS()
    {
        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");

        var expectedChedReference = scenarioContext.Get<string>("CHEDReference");

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span']"),
            "Import Notification page header could not be found.");

        var actualPageHeader = pageHeader.Text.Trim();
        actualPageHeader.Should().Be("Import Notification",
            $"Expected page header to be 'Import Notification' but found '{actualPageHeader}'.");

        var chedReferenceHeader = Driver.WaitUntilAvailable(
            By.XPath("//h1[@data-id='header_title']"),
            $"CHED reference header could not be found on the Import Notification page.");

        var actualChedReference = chedReferenceHeader.Text
            .Replace("- Saved", string.Empty)
            .Trim();

        actualChedReference.Should().Be(expectedChedReference,
            $"Expected CHED reference header to be '{expectedChedReference}' but found '{actualChedReference}'.");
    }

    /// <summary>
    /// Verifies that the Import Notification Status header field displays the expected value.
    /// Uses data-preview_orientation='column' as the stable anchor — CSS class names (pa-*) are
    /// dynamically generated and must not be used as locators.
    /// Structure: div[@data-preview_orientation='column'] → div[value] + div[label text only]
    /// </summary>
    /// <param name="expectedStatus">The expected status value e.g. 'Inactive'.</param>
    [Then(@"the Import Notification Status is '(.*)'")]
    public void ThenTheImportNotificationStatusIs(string expectedStatus)
    {
        Driver.WaitForTransaction();

        // The label div is a direct child leaf div with text 'Status' (no child elements).
        // The value div is the first sibling div and contains the actual status text as a leaf descendant.
        var statusValue = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-preview_orientation='column']" +
                     "[child::div[not(*) and normalize-space(text())='Status']]" +
                     "/div[1]/descendant::div[not(*) and normalize-space(.)!=''][1]"),
            "Import Notification Status value could not be found in the page header.");

        statusValue.Text.Trim().Should().Be(expectedStatus,
            $"Expected Import Notification Status to be '{expectedStatus}' but found '{statusValue.Text.Trim()}'.");
    }

    /// <summary>
    /// Verifies that the Import Notification Status Reason header field displays the expected value.
    /// Uses data-preview_orientation='column' as the stable anchor — CSS class names (pa-*) are
    /// dynamically generated and must not be used as locators.
    /// </summary>
    /// <param name="expectedStatusReason">The expected status reason value e.g. 'Completed'.</param>
    [Then(@"the Import Notification Status Reason is '(.*)'")]
    public void ThenTheImportNotificationStatusReasonIs(string expectedStatusReason)
    {
        Driver.WaitForTransaction();

        var statusReasonValue = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-preview_orientation='column']" +
                     "[child::div[not(*) and normalize-space(text())='Status Reason']]" +
                     "/div[1]/descendant::div[not(*) and normalize-space(.)!=''][1]"),
            "Import Notification Status Reason value could not be found in the page header.");

        statusReasonValue.Text.Trim().Should().Be(expectedStatusReason,
            $"Expected Import Notification Status Reason to be '{expectedStatusReason}' but found '{statusReasonValue.Text.Trim()}'.");
    }

    /// <summary>
    /// Clicks the Work Order reference link in the <c>trd_workorderid</c> lookup field on the Summary tab.
    /// Handles two compounding issues: (1) the Dynamics plugin/flow that creates and links the Work Order
    /// runs asynchronously after the Import Notification is received from IPAFFS — on a cold first run the
    /// field may be genuinely empty; (2) the field's inner anchor is only rendered into the DOM once its
    /// container is scrolled into the viewport (lazy-load). The outer retry loop refreshes the record via
    /// the Dynamics command bar between attempts to pull the latest server state, while the inner scroll
    /// loop incrementally scrolls the Summary tab panel to trigger the lazy-load render.
    /// Waits up to 5 minutes total for the Work Order to be linked before failing.
    /// </summary>
    [When("I click the reference number in the Work Order field for the notification created in IPAFFS")]
    public void WhenIClickTheReferenceNumberInTheWorkOrderFieldForTheNotificationCreatedInIPAFFS()
    {
        Driver.WaitForTransaction();

        var expectedChedReference = scenarioContext.Get<string>("CHEDReference");

        const int maxScrollAttempts = 20;
        const int scrollIncrementPx = 300;
        const int maxOuterRetries = 10;
        const int outerRetryWaitSeconds = 30;

        IWebElement workOrderLink = null;

        for (var outerAttempt = 0; outerAttempt < maxOuterRetries && workOrderLink == null; outerAttempt++)
        {
            if (outerAttempt > 0)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(outerRetryWaitSeconds));

                CommandSteps.WhenISelectTheCommand("Refresh");
                Driver.WaitForTransaction();

                SignInPromptHelper.DismissSignInPrompts(Driver, "post-refresh");
            }

            for (var scrollAttempt = 0; scrollAttempt < maxScrollAttempts; scrollAttempt++)
            {
                Driver.ExecuteScript(
                    @"var panel = document.querySelector('[role=""tabpanel""][aria-label=""Summary""]')
                               || document.querySelector('[role=""tabpanel""]');
                      if (panel) { panel.scrollTop += arguments[0]; }",
                    scrollIncrementPx);

                Driver.WaitForTransaction();

                var candidates = Driver.FindElements(
                    By.XPath($"//div[@data-id='trd_workorderid.fieldControl-LookupResultsDropdown_trd_workorderid_selected_tag'" +
                             $" and @aria-label='{expectedChedReference}']"));

                if (candidates.Count > 0)
                {
                    workOrderLink = candidates[0];
                    break;
                }
            }
        }

        var totalWaitMinutes = (maxOuterRetries - 1) * outerRetryWaitSeconds / 60;

        workOrderLink.Should().NotBeNull(
            $"Work Order field link for CHED reference '{expectedChedReference}' could not be found " +
            $"after {maxOuterRetries} attempts over {totalWaitMinutes} minutes. " +
            $"The Dynamics plugin/flow that creates and links the Work Order to this Import Notification " +
            $"may not have completed. Verify that the Work Order creation flow is active and that " +
            $"the trd_workorderid lookup is populated on the record in Dynamics.");

        workOrderLink.Click();

        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");
    }

    [Then("I verify the Importer Notification Details reflect the information from the EU Import Notification")]
    public void ThenIVerifyTheImporterNotificationDetailsReflectTheInformationFromTheEUImportNotification()
    {
        var expectedChedReference = scenarioContext.Get<string>("CHEDReference");

        Driver.WaitForTransaction();

        var referenceNumberInput = Driver.WaitUntilAvailable(
            By.XPath("//input[@data-id='header_defraimp_name.fieldControl-text-box-text']"),
            "Reference Number field could not be found on the Importer Notification Details page.");

        var actualReferenceNumber = referenceNumberInput.GetAttribute("value")?.Trim() ?? string.Empty;

        actualReferenceNumber.Should().Be(expectedChedReference,
            $"Expected Reference Number to be '{expectedChedReference}' but found '{actualReferenceNumber}'");
    }

    [When(@"an importer amend '(.*)' to inspection address '(.*)' of '(.*)'")]
    public void WhenAnImporterAmendToInspectionAddressOf(string applicationAlias, string address, string bcpName)
    {
        var importNotification = this.context.GetEntityReference(applicationAlias);
        this.SetImportNotificationToAmend(importNotification);
        var inspectionAddress = this.context.GetEntityReference(address);
        this.UpdateInspectionAddress(bcpName, inspectionAddress, importNotification);
    }

    private void UpdateInspectionAddress(string bcpName, EntityReference inspectionAddress, EntityReference importNotification)
    {
        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var importNotificationEntity = plantsContext.trd_plantsimportnotificationSet
                .Where(st => st.Id == importNotification.Id)
                .Select(st => new trd_plantsimportnotification
                {
                    Id = st.Id,
                    trd_BorderControlPostId = st.trd_BorderControlPostId,
                    trd_InspectionLocationId = st.trd_InspectionLocationId,
                    trd_Version = st.trd_Version,
                    statuscode = st.statuscode,
                }).Single();

            var bcp = plantsContext.trd_bordercontrolpostSet.Single(p => p.trd_name == bcpName);

            importNotificationEntity.statuscode = trd_plantsimportnotification_statuscode.Amended;
            importNotificationEntity.trd_Version = "2";
            importNotificationEntity.trd_BorderControlPostId = bcp.ToEntityReference();
            importNotificationEntity.trd_InspectionLocationId = inspectionAddress;
            plantsContext.UpdateObject(importNotificationEntity);
            plantsContext.SaveChanges();
        }
    }

    private void SetImportNotificationToAmend(EntityReference importNotification)
    {
        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var notification = plantsContext.trd_plantsimportnotificationSet.Where(st => st.Id == importNotification.Id)
                .Select(st => new trd_plantsimportnotification
                {
                    Id = st.Id,
                    statuscode = st.statuscode,
                }).Single();


            notification.statuscode = trd_plantsimportnotification_statuscode.Amending;
            plantsContext.UpdateObject(notification);
            plantsContext.SaveChanges();
        }
    }

    [When(@"'(.*)' is completed for '(.*)' with the following details")]
    public void WhenIsCompletedForWithFollowingDetails(string taskName, string applicationAlias, Table table)
    {
        var applicationSteps = new ApplicationSteps(this.context, this.scenarioContext);
        var workOrder = applicationSteps.WaitTillWorkOrderHasBeenCreated(applicationAlias);
        ApplicationSteps.OpenEntity(workOrder);
        EntitySteps.ISelectTab("Work Order Tasks");
        OpenWorkOrderServiceTask(taskName);
        new WorkOrderTasksSteps(this.context).AssignServiceTaskToSelf();

        foreach (var tableRow in table.Rows)
        {
            if (table.ContainsColumn("No. of Phytos Inspected"))
            {
                PopupSteps.WhenIEnterIntoTheTextFieldOnThePopupBox(tableRow["No. of Phytos Inspected"], "trd_noofphytosinspected", "numeric");
            }
        }

        table = this.CreateDefaultTimeEntryTable(taskName);
        new ImportSteps(this.context).AddTimeEntryAndSubmit(table, taskName);
        PopupSteps.MarkCompleteServiceTasks();
        PopupSteps.CloseServiceTask();
    }

    [When(@"an inactive '(.*)' is completed for '(.*)'")]
    public void WhenAnInactiveIsCompletedFor(string taskName, string applicationAlias)
    {
        this.OpenWorkOrder(applicationAlias);

        var subGridName = "workorderservicetasksgrid";
        GridHelper.WaitForRows(
            Driver,
            subGridName,
            (rowCount) =>
            {
                var serviceTasksList = GetWorkOrderServiceTaskDetails().ToList();
                var task = serviceTasksList.SingleOrDefault(p => p.StatusReason == "Inactive" && taskName == p.TaskType);
                this.CompleteTasks(taskName, applicationAlias, false, false);
            },
            $"Subgrid {subGridName} contains no rows.");
    }

    private void OpenWorkOrder(string applicationAlias)
    {
        var applicationSteps = new ApplicationSteps(this.context, this.scenarioContext);
        var workOrder = applicationSteps.WaitTillWorkOrderHasBeenCreated(applicationAlias);
        ApplicationSteps.OpenEntity(workOrder);
        EntitySteps.ISelectTab("Work Order Tasks");
    }

    [When(@"'(.*)' is completed for '(.*)'")]
    public void WhenIsCompletedFor(string taskName, string applicationAlias)
    {
        this.CompleteTasks(taskName, applicationAlias);
    }

    private void CompleteTasks(string taskName, string applicationAlias, bool active = true, bool openWorkOrder = true)
    {
        if (openWorkOrder)
        {
            this.OpenWorkOrder(applicationAlias);
        }

        var serviceTasksList = GetWorkOrderServiceTaskDetails().ToList();
        var index = serviceTasksList.FindIndex(p => p.TaskType == taskName);
        Capgemini.PowerApps.SpecFlowBindings.Steps.GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(index);
        new WorkOrderTasksSteps(this.context).AssignServiceTaskToSelf();
        if (!active)
        {
            PopupSteps.WhenIActivateTheRecordInThePopupBox("msdyn_workorderservicetask");
            DialogSteps.WhenIClickTheButtonOnTheDialog("Activate");
            Driver.WaitForTransaction();
        }

        var table = this.CreateDefaultTimeEntryTable(taskName);
        new ImportSteps(this.context).AddTimeEntryAndSubmit(table, taskName);
        PopupSteps.MarkCompleteServiceTasks();
        PopupSteps.CloseServiceTask();
    }

    private Table CreateDefaultTimeEntryTable(string taskName)
    {
        switch (taskName.ToLower())
        {
            case "document check":
                var table = new Table("Inspector", "Admin");
                table.AddRow("a primary inspector", Faker.RandomNumber.Next(1, 500).ToString());
                return table;
            case "identity & physical check":
                var idAndPhysicalTable = new Table("Inspector", "Travel", "Inspection", "Admin");
                idAndPhysicalTable.AddRow("a primary inspector", Faker.RandomNumber.Next(1, 500).ToString(), Faker.RandomNumber.Next(1, 500).ToString(), Faker.RandomNumber.Next(1, 500).ToString());
                return idAndPhysicalTable;
            default:
                throw new NotImplementedException("TODO");
        }
    }

    [Then(@"I can see the following charges are created")]
    public void ThenICanSeeTheFollowingChargesAreCreated(Table table)
    {
        SessionContext.GetServiceClient().WaitForRecords(
            new QueryByAttribute(trd_charge.EntityLogicalName)
            {
                Attributes = { "trd_workorderid" },
                Values =
                {
                    this.context.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias).Id,
                },
            }, TimeSpan.FromSeconds(90));

        new WorkOrderSteps(this.context).OpenWorkOrder();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        var gridRecords = XrmApp.Grid.GetGridItems();

        var expectedList = table.CreateSet<ChargeTable>();
        var actualList = new List<ChargeTable>();

        foreach (var gridRecord in gridRecords)
        {
            var attributes = (Dictionary<string, object>)(typeof(GridItem).GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(gridRecord));
            var serviceTask = new ChargeTable
            {
                ProductOrService = GetValue(attributes, "trd_productorserviceid"),
                PriceList = GetValue(attributes, "trd_pricelistid"),
                Quantity = GetValue<decimal>(attributes, "trd_qty"),
                Unit = GetValue(attributes, "trd_unitid"),
                UnitChargeAmount = GetValue<decimal>(attributes, "trd_unitchargeamount"),
                WorkOrderTask = GetValue(attributes, "trd_workordertaskid"),
                ChargeExempt = GetValue(attributes, "trd_chargeexempt"),
            };
            actualList.Add(serviceTask);
        }

        actualList.Should().BeEquivalentTo(expectedList);
    }

    [Given(@"'(.*)' has been created")]
    public void GivenHasBeenCreated(string dataFileName)
    {
        if (!dataFileName.Contains("import"))
            throw new NotImplementedException("Need to extend to address other applications");

        var table = new Table("FileName");
        table.AddRow("an aliased contact");
        table.AddRow("an aliased account");
        table.AddRow("an aliased exporter");
        table.AddRow(dataFileName);
        new DataSteps(this.context).GivenAUserHasCreatedSomethingICannot(table);
    }

    [When(@"I set willinspect as (.*) for '(.*)'")]
    public void WhenISetWillinspectAsYesFor(string willInspectValue, string commodityAlias)
    {
        new DataSteps(this.context).GivenIHaveOpened(commodityAlias);
        EntitySteps.WhenIEnterInTheField(willInspectValue, "trd_willinspect", "optionset", "field");
        EntitySteps.WhenISaveTheRecord();
    }

    [When(@"I open related work order for '(.*)'")]
    public void WhenIOpenRelatedWorkOrderFor(string recordAlias)
    {
        new DataSteps(this.context).GivenIHaveOpened(recordAlias);
        LookupSteps.WhenISelectARelatedLookupInTheForm("trd_workorderid");
    }

    [When(@"Commodity '(.*)' is updated as '(.*)' and (Inspection Required|No Inspection Required)")]
    public void WhenCommodityIsUpdatedAsAndInspectionRequired(string variableName, string authority, string inspectionRequiredString)
    {
        var table = new Table("Field", "Value");

        var phsiInspectionRequired = false;
        var hmiInspectionRequired = false;

        if (inspectionRequiredString.ToLower() == "inspection required")
        {
            switch (authority)
            {
                case "PHSI":
                    phsiInspectionRequired = true;
                    break;
                case "JOINT":
                    phsiInspectionRequired = true;
                    hmiInspectionRequired = true;
                    break;
                default:
                    hmiInspectionRequired = true;
                    break;
            }
        }

        table.AddRow("trd_phsiinspectionrequired", phsiInspectionRequired.ToString());
        table.AddRow("trd_hmiinspectionrequired", hmiInspectionRequired.ToString());
        table.AddRow("trd_regulatoryauthoritycode", authority == "PHSI" ? "0" : authority == "HMI" ? "1" : "2");
        new UpdateDataSteps(this.context).WhenHasUpdateWithTheFollowingValues(variableName, table);
    }

    [When(@"'(.*)' has been amending and I can see notification as '(.*)' for (.*) service tasks")]
    public void WhenAnImportNotificationHasBeenAmendingAndICanSeeNotificationAsInServiceTasks(string alias, string message, int serviceTaskCount)
    {
        this.UpdateNotificationStatusAndAssertNotificationMessage(alias, message, serviceTaskCount, "434800007");
    }

    [When(@"'(.*)' has been amended and I can see notification as '(.*)' for (.*) service tasks")]
    public void WhenAnImportNotificationHasBeenAmendedAndICanSeeNotificationAsForServiceTasks(string alias, string message, int serviceTaskCount)
    {
        this.UpdateNotificationStatusAndAssertNotificationMessage(alias, message, serviceTaskCount, "434800008");
    }

    private void UpdateNotificationStatusAndAssertNotificationMessage(string alias, string message, int serviceTaskCount, string status)
    {
        this.UpdateImportStatusAndAssert(status);
        Navigate("WorkOrder");
        for (var i = 0; i < serviceTaskCount; i++)
        {
            OpenGridRecord(i);

            Policy
                .Handle<Exception>()
                .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(2))
                .Execute(() =>
                {
                    var notifications = XrmApp.Entity.GetFormNotifications();
                    notifications.Any(p => p.Message == message).Should().BeTrue($"{message} not found");
                });

            ClosePopup();
        }
    }

    private static void ClosePopup()
    {
        PopupSteps.WhenICloseThePopup();
        SharedSteps.WaitForScriptProcessing();
    }

    [When(@"an import notification has been amended")]
    public void WhenAnImportNotificationHasBeenAmended()
    {
        this.UpdateImportStatusAndAssert("434800007");
        this.UpdateImportStatusAndAssert("434800008");
    }

    [Then(@"no workorder service tasks are present in '(.*)'")]
    public void ThenNoWorkorderServiceTasksArePresentIn(string alias)
    {
        Navigate("WorkOrder");
        var actualWorkOrderServiceTasks = GetWorkOrderServiceTaskDetails();
        actualWorkOrderServiceTasks.Should().BeEquivalentTo(new List<WorkOrderServiceTaskTable>());
    }

    [Then(@"I can see no charges are created")]
    public void ThenICanSeeNoChargesAreCreated()
    {
        new WorkOrderSteps(this.context).OpenWorkOrder();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        var gridRecords = XrmApp.Grid.GetGridItems();
        gridRecords.Count.Should().Be(0, "Expected charges to be 0");
    }

    public static void Navigate(string formName)
    {
        var entityName = EntityName;
        switch (formName)
        {
            case "Commodity":
                NavigateToCommodityForm(entityName);
                return;
            case "Import":
                NavigateToImport(entityName);
                return;
            case "WorkOrder":
                NavigateToWorkOrderForm(entityName);
                return;
            default:
                throw new InvalidOperationException($"Unexpected argument {formName}");
        }
    }

    private static void NavigateToImport(string entityName)
    {
        switch (entityName)
        {
            case "trd_plantsimportnotification":
                return;
            case "msdyn_workorderservicetask":
                PopupSteps.WhenICloseThePopup();
                break;
        }

        CommandSteps.WhenIgoBack();
        IsItImportNotificationFrom().Should().BeTrue();
    }

    private static void NavigateToWorkOrderForm(string entityName)
    {
        switch (entityName)
        {
            case "msdyn_workorder":
                return;
            case "msdyn_workorderservicetask":
                PopupSteps.WhenICloseThePopup();
                return;
            case "trd_plantsimportcommodityline":
                CommandSteps.WhenIgoBack();
                break;
        }
        EntitySteps.ISelectTab("Summary");
        LookupSteps.WhenISelectARelatedLookupInTheForm("trd_workorderid");
        EntitySteps.ISelectTab("Work Order Tasks");
        IsItWorkOrder().Should().BeTrue();
    }

    private static void NavigateToCommodityForm(string entityName)
    {
        switch (entityName)
        {
            case "trd_plantsimportcommodityline":
            case "msdyn_workorder":
                CommandSteps.WhenIgoBack();
                break;
            case "trd_plantsimportnotification":
                break;
        }

        EntitySteps.ISelectTab("Commodity Lines");
    }

    private static bool IsItImportNotificationFrom()
    {
        return EntityName == "trd_plantsimportnotification";
    }

    private static bool IsItWorkOrder()
    {
        return EntityName == "msdyn_workorder";
    }

    public static void OpenWorkOrderServiceTask(string taskName)
    {
        var serviceTasksList = GetWorkOrderServiceTaskDetails().ToList();
        var index = serviceTasksList.FindIndex(p => p.TaskType == taskName);
        XrmApp.Entity.SubGrid.OpenSubGridRecord("workorderservicetasksgrid", index);
        Driver.WaitForTransaction();
    }

    public static IEnumerable<WorkOrderServiceTaskTable> GetWorkOrderServiceTaskDetails()
    {
        return XrmApp.Entity.SubGrid
            .GetSubGridItems("workorderservicetasksgrid")
            .Select(item => new WorkOrderServiceTaskTable
            {
                TaskType = (string)item["msdyn_name"],
                StatusReason = (string)item["statecode"],
                Complete = (string)item["msdyn_percentcomplete"],
            })
            .ToList();
    }

    private void UpdateImportStatusAndAssert(string statusCode)
    {
        var versionNumber = "#{RandomNumber[10]}#".TokeniseText();
        var table = new Table("Field", "Value");
        table.AddRow("trd_version", versionNumber);
        table.AddRow("statuscode", statusCode);
        new UpdateDataSteps(this.context).WhenHasUpdateWithTheFollowingValues("an import notification", table);
        Navigate("Import");
        CommandSteps.WhenISelectTheCommand("Refresh");
        FormSteps.VerifyValue(statusCode, "statuscode");
    }

    private static void OpenGridRecord(int positionValue)
    {
        Capgemini.PowerApps.SpecFlowBindings.Steps.GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(positionValue);
        SharedSteps.WaitForScriptProcessing();
    }

    private static string GetValue(Dictionary<string, object> attributes, string attributeName)
    {
        return GetValue<string>(attributes, attributeName);
    }

    private static T GetValue<T>(Dictionary<string, object> attributes, string attributeName)
    {
        var taskNameKey = attributes.Keys.SingleOrDefault(p => p.EndsWith(attributeName));
        if (taskNameKey == null)
            return default;
        return (T)Convert.ChangeType(attributes[taskNameKey], typeof(T));
    }
}