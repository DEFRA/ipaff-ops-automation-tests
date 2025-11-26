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
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Polly;
using System;
using System.Linq;
using Reqnroll;

[Binding]
public class PopupSteps : PowerAppsStepDefiner
{
    private readonly SessionContext ctx;
    private static readonly string _xpathForPopupTab = "//ul[@aria-label='Work Order Task Form']//li[@role='tab' and contains(@data-id, '{0}')]";
    private static readonly string _xpathForPopupTabOnInspectionForm = "//ul[@aria-label='Inspection Result Form']//li[@role='tab' and contains(@aria-label, '{0}')]";
    public PopupSteps(SessionContext ctx)
    {
        this.ctx = ctx;
    }

    public void ActionDate()
    {
        DateTime today = DateTime.Today;
        var date = today.ToString("dd/MM/yyyy");
        SharedSteps.WaitForScriptProcessing();
        //EntitySteps.WhenIEnterInTheField(date, "trd_actiondate", "DateTimeControl", "field");
        Driver.FindElement(By.XPath("//*[@aria-label='Date of Action Date']")).SendKeys(date);
        SharedSteps.WaitForScriptProcessing();
    }

    [When("I open the '(.*)' tab on the popup dialog")]
    public static void OpenPopupTab(string tabName)
    {
        Driver.WaitForTransaction();
        Driver.WaitUntilClickable(By.XPath(string.Format(_xpathForPopupTab, tabName))).Click();
        Driver.WaitForTransaction();
    }

    [When(@"I open the related '(.*)' tab on popup dialog")]
    public void WhenIOpenTheRelatedTabOnPopupDialog(string tabName)
    {
        Client.SelectTabOnPopup("Related", tabName);
    }

    [When(@"I assign the record in the (.*) popup box")]
    public static void WhenIAssignTheRecordInThePopupBox(string entityLogicalName)
    {
        try
        {
            Driver.WaitUntilClickable(By.XPath($"//*[contains(@id,'{entityLogicalName}|NoRelationship|Form|Mscrm.Assign')]/button")).Click();
        }
        catch
        {
            CommandSteps.ClickCommand("Assign");
        }

        Driver.WaitForTransaction();
    }

    [When(@"I saveAndClose the record in the (.*) popup box")]
    public static void WhenISaveAndCloseTheRecordInThePopupBox(string entityLogicalName)
    {
        Driver.WaitUntilClickable(By.XPath($"//*[contains(@id,'{entityLogicalName}|NoRelationship|Form|Mscrm.SaveAndClosePrimary')]/button")).Click();
        Driver.WaitForTransaction();
    }

    [When(@"I complete the service task")]
    public void WhenICompleteTheServiceTask()
    {
        WhenIMarkCompleteTheRecordInThePopupBox("msdyn_workorderservicetask");
    }

    [When(@"I Mark Complete the record in the (.*) popup box")]
    public static void WhenIMarkCompleteTheRecordInThePopupBox(string entityLogicalName)
    {
        Driver.WaitUntilClickable(By.XPath($"//*[contains(@id,'{entityLogicalName}|NoRelationship|Form|trd.{entityLogicalName}.CommandMainForm.MarkComplete')]/button")).Click();
        Driver.WaitForTransaction();
    }

    [When(@"I refresh the record in the (.*) popup box")]
    public static void WhenIRefreshTheRecordInThePopupBox(string entityLogicalName)
    {
        Driver.WaitUntilClickable(By.XPath($"//*[contains(@id,'{entityLogicalName}|NoRelationship|Form|Mscrm.Modern.refreshCommand')]/button")).Click();
        Driver.WaitForTransaction();
    }

    [When("I save the record in the (.*) popup box")]
    public static void WhenIClickSave(string entityLogicalName)
    {
        Driver.WaitUntilClickable(By.XPath($"//*[contains(@id,'{entityLogicalName}|NoRelationship|Form|Mscrm.SavePrimary')]/button")).Click();
        Driver.WaitForTransaction();
    }

    [When(@"I select the '(.*)' command on the (.*) popup box")]
    public void WhenISelectTheCommandOnThePopupBox(string commandName, string entityName)
    {
        var formContext = Driver.WaitUntilAvailable(By.XPath("//div[contains(@role, 'dialog')]"), "modal element could not be found");
        var commandBar = formContext.WaitUntilClickable(By.XPath("//ul[contains(@data-id, 'CommandBar')]"));
        var commandButton = commandBar.WaitUntilClickable(By.XPath($"//button[starts-with(@id, '{entityName}') and contains(@aria-label, '{commandName}')]"));
        Driver.ExecuteScript("arguments[0].click()", commandButton);
        Driver.WaitForTransaction();
    }

    [When(@"I click the '(.*)' button on the popup dialog")]
    public static void WhenIClickTheButtonOnThePopupDialog(string buttonName)
    {
           Driver.ClickWhenAvailable(By.XPath($"//*[text()='"+buttonName+"']"), "Unable to click the button in the dialog");
           Driver.WaitForTransaction();
    }

    [When(@"I verify the '(.*)' button on the popup dialog")]
    public static void WhenIVerifyButtonInThePopupDialog(string buttonName)
    {
        var buttonElement = Driver.FindElement(By.XPath($"//*[text()='{buttonName}']"));

        // Verify button is visible and enabled
        (buttonElement.Displayed && buttonElement.Enabled).Should().BeTrue(because: "the button should be present in the dialog"); ;
        
    }

    [When(@"I deactivate the record in the (.*) popup box")]
    public void WhenIDeactivateTheRecordInThePopupBox(string entityLogicalName)
    {
        Driver.WaitUntilClickable(By.XPath($"//*[contains(@id,'{entityLogicalName}|NoRelationship|Form|Mscrm.Form.Deactivate')]/button")).Click();
        Driver.WaitForTransaction();
    }

    [When(@"I activate the record in the (.*) popup box")]
    public static void WhenIActivateTheRecordInThePopupBox(string entityLogicalName)
    {
        Driver.WaitUntilClickable(By.XPath($"//*[contains(@id,'{entityLogicalName}|NoRelationship|Form|Mscrm.Form.Activate')]/button")).Click();
        Driver.WaitForTransaction();
    }

    [When("I maximise the popup")]
    public static void WhenIMaximiseThePopup()
    {
        var elements = Driver.FindElements(By.XPath($"//button[@title='Enter full screen mode']"));
        if (elements.Any())
        {
            elements.Single().Click();
        }
    }

    [When(@"I minimise the popup")]
    public static void WhenIMinimiseThePopup()
    {
        Driver.WaitUntilClickable(By.XPath($"//button[@title='Exit full screen mode']")).Click();
    }

    [When(@"I close the popup")]
    public static void WhenICloseThePopup()
    {
        Driver.WaitUntilClickable(By.XPath($"//button[@title='Close']")).Click();
        Driver.WaitForTransaction();
    }

    [When(@"I enter '(.*)' into the '(.*)' (currency|numeric|text|datetime|boolean|optionset) field on the popup box")]
    public static void WhenIEnterIntoTheTextFieldOnThePopupBox(string value, string logicalName, string controlType)
    {
        //var formContext = Driver.WaitUntilAvailable(By.XPath("//*[@data-id='mainFormDialogRoot']"));
        var formContext = Driver.WaitUntilAvailable(By.XPath("//*[@data-id='editFormRoot']")); IWebElement control = null;

        switch (controlType)
        {
            case "optionset":
                control = formContext.WaitUntilAvailable(By.XPath($"//select[contains(@data-id,'{logicalName}.fieldControl')]"));
                control.Should().NotBeNull();
                control.SelectByText(value);
                break;
            case "textarea":
                control = formContext.WaitUntilAvailable(By.XPath($"//textarea[contains(@data-id,'{logicalName}.fieldControl')]"));
                control.Should().NotBeNull($"Unable to find the element {logicalName}");
                control.Click();
                ClearFieldValue(control);
                control.SendKeys(value);
                break;
            default:
                control = formContext.WaitUntilAvailable(By.XPath($"//input[contains(@data-id,'{logicalName}.fieldControl')]"));
                control.Should().NotBeNull($"Unable to find the element {logicalName}");
                control.Click();
                ClearFieldValue(control);
                control.SendKeys(value);
                control.SendKeys(Keys.Tab);
                break;
        }

        Driver.WaitForTransaction();
    }

    [When(@"I Double Click the '(.*)' Grid Record in the Popup")]
    public static void WhenIDoubleClickTheGridRecordOnThePopupBox(string gridName, string headerName, int Index)
    {
        GridHelper.SelectRow(Driver, gridName, Index);
        SharedSteps.WaitForScriptProcessing();

        var headerIndex = GridHelper.GetHeaderIndex(Driver, gridName, headerName);
        var cells = GridHelper.GetCells(Driver, gridName, Index);
        var outerCell = cells[headerIndex];
        outerCell.Click();
        SharedSteps.WaitForScriptProcessing();
        Driver.DoubleClick(outerCell);
        Driver.WaitForTransaction();
    }

    [When(@"I add the GSI destination countries in the Popup")]
    public static void WhenIAddTheGSIDestinationCountriesOnThePopupBox(string gridName)
    {
        GridHelper.SelectAllRows(Driver, gridName);
        SharedSteps.WaitForScriptProcessing();
        var addButton = Driver.WaitUntilVisible(By.XPath("//*[contains (@id,'trd_destinationcountry') and contains(@aria-label, 'Add')]"));
        addButton.Click();
        SharedSteps.WaitForScriptProcessing();
    }
    [Then(@"the modal form titled '(.*)' is displayed")]
    public void ThenTheModalFormTitledIsDisplayed(string title)
    {
        var modalForm = Driver.WaitUntilVisible(By.XPath("//div[contains(@role, 'dialog')]"));
        var isVisible = Driver.IsVisible(By.XPath("//div[contains(@role, 'dialog')]"));
        isVisible.Should().BeTrue(because: "the modal form should be visible");

        var headerTitle = modalForm.FindElement(By.TagName("h1"));
        headerTitle.Text.Should().Be(title);
    }

    public static void ClearFieldValue(IWebElement field)
    {
        if (field.GetAttribute("value").Length > 0)
        {
            field.SendKeys(Keys.Control + "a");
            field.SendKeys(Keys.Backspace);
            Driver.WaitForTransaction();
        }
    }

    /// <summary>
    /// Visit creation in the UI.
    /// </summary>
    [When(@"I create a new visit for the inspection task")]
    public void WhenICreateNewVisitForTheInspectionTask()
    {
        Policy
         .Handle<Exception>()
         .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(1))
         .Execute(() =>
         {
             var visitlookup = Driver.FindElement(By.XPath("//*[@aria-label='Visit, Lookup']"));
             visitlookup.Click();
             SharedSteps.WaitForScriptProcessing();
             var newvisit = Driver.FindElement(By.XPath("//*[@data-id='trd_visitid.fieldControl-LookupResultsDropdown_trd_visitid_addNewBtn_0']"));
             newvisit.Click();
             SharedSteps.WaitForScriptProcessing();
         });

        DateTime scheduleDate;
        using (var context = new PlantsContext(SessionContext.GetServiceClient()))
        {
            scheduleDate = RandomHelper.GetNextAvailableVisitDate(context);
        }

        var visitName = "Visit-" + scheduleDate.ToString("ddMMyyyyHHmmss");
        QuickCreateSteps.WhenIEnterInTheFieldOnTheQuickCreate(visitName, "trd_name", "text");
        SharedSteps.WaitForScriptProcessing();
        Client.SetDateTimeFix("trd_datescheduled", scheduleDate, false, "dd/MM/yyyy", "H:mm");
        SharedSteps.WaitForScriptProcessing();
        QuickCreateSteps.WhenISaveTheQuickCreate();
    }

    [When(@"I add an existing visit for the inspection task")]
    public static void WhenIAddAnExistingVisitForTheInspectionTask()
    {
        Policy
         .Handle<Exception>()
         .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(1))
         .Execute(() =>
         {
             var visitlookup = Driver.WaitUntilClickable(By.XPath("//*[@data-id='trd_visitid.fieldControl-Lookup_trd_visitid']"));
             visitlookup.Click();
             SharedSteps.WaitForScriptProcessing();
             var visitlookupSearch = Driver.WaitUntilClickable(By.XPath("//*[@aria-label='Search records for Visit, Lookup field']"));
             visitlookupSearch.Click();
             SharedSteps.WaitForScriptProcessing();
             var visitlookupResult = Driver.WaitUntilClickable(By.XPath("//*[@aria-label='Visit Lookup results']"));
             visitlookupResult.Click();
         });
    }

    [When(@"I close the (.*) popup if it is displayed")]
    public static void WhenICloseThePopup(string entityName)
    {
        SharedSteps.WaitForScriptProcessing();

        int numLoops = 0;

        // Sometimes takes 2 goes for unknown reason...
        while (XrmApp.Entity.GetEntityName() == entityName && numLoops++ < 2)
        {
            var closeButtons = Driver.FindElements(By.XPath($"//button[@title='Close']"));
            closeButtons.Last().Click();
            SharedSteps.WaitForScriptProcessing();
        }
    }

    [Then(@"I (can|cannot) see the following buttons in the Work Order Service Task popup")]
    public static void ICanSeeTheFollowingButtonsInTheWorkOrderTaskPopupTab(string canOrCannot, Table table)
    {
        var buttons = table.Rows.Select(tableRow => tableRow["Button"]).ToList();
        foreach (var Button in buttons)
        {
            var shouldBeVisible = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);
            //var buttonElement = Driver.WaitUntilAvailable(By.XPath($"//*[@data-id='mainFormDialogRoot']//*[contains(@id,'msdyn_workorderservicetask') and contains(@title,'{Button}')]"));
            Driver.IsVisible(By.XPath($"//*[@data-id='mainFormDialogRoot']//*[contains(@id,'msdyn_workorderservicetask') and contains(@title,'{Button}')]"))
                .Should()
                .Be(
                    shouldBeVisible,
                    because: $"The Button '{Button}' {(shouldBeVisible ? "should" : "should not")} be visible.");
        }
    }

    public static void CloseServiceTask()
    {
        WhenICloseThePopup("msdyn_workorderservicetask");
    }

    public static void MarkCompleteServiceTasks()
    {
        WhenIMarkCompleteTheRecordInThePopupBox("msdyn_workorderservicetask");
    }
    public static void OpenPopupTabonInspectionForm(string tabName)
    {
        Driver.WaitForTransaction();
        Driver.WaitUntilClickable(By.XPath(string.Format(_xpathForPopupTabOnInspectionForm, tabName))).Click();
        Driver.WaitForTransaction();
    }
}
