// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
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
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Reqnroll;

/// <summary>
/// Step bindings relating to the work order functional area.
/// </summary>
[Binding]
public sealed class WorkOrderSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;

    public WorkOrderSteps(SessionContext sessionContext)
    {
        this.sessionContext = sessionContext;
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