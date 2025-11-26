// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Reqnroll;

[Binding]
public class GridPageSteps : PowerAppsStepDefiner
{
    public IWebElement SelectSubgridRecords => Driver.WaitUntilAvailable(By.XPath("(.//div[contains(@data-lp-id,'timerecordings_subgrid')])//button[@title='Select All']"));

    public IWebElement NameForSelectedCommodity => Driver.WaitUntilAvailable(By.XPath("//input[@data-id='trd_name.fieldControl-text-box-text']"));

    [When(@"I click on submit time on subgrid")]
    public void WhenIClickOnSubmitTimeOnSubgrid()
    {
        this.SubmitTime();
    }

    [When(@"I click on submit time on subgrid and it gives validation alerts to be '(.*)'")]
    public void WhenIClickOnSubmitTimeOnSubgridThatNeedsAlertsConfirmedORCancelled(string buttonName)
    {
        this.SubmitTime();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog(buttonName);
    }

    [Then(@"I click on submit time on the '(.*)' subgrid and it gives validation alert as '(.*)'")]
    public void ThenIClickOnSubmitTimeOnSubgridItGivesValidationAlert(string gridName, string message)
    {
        GridSteps.WhenISelectAllRowsInTheGrid(gridName);
        WhenIClickOnSubmitTimeOnSubgrid();
        DialogSteps.ThenADialogIsDisplayedWithMessageInTheDialogField(message, "dialogMessageText");
        DialogSteps.WhenIClickOKOnTheRejectDialog();
    }

    [When(@"I click on remove time for inspector on subgrid")]
    public void WhenIClickOnRemoveTimeForInspectorOnSubgrid()
    {
        this.RemoveTime();
    }

    [When(@"I select the record in time recording subgrid")]
    public void WhenISelectTheRecordInTimeRecordingSubgrid()
    {
        this.SelectSubgrideRecordItems();
    }

    [When(@"I enter name '(.*)'in the selected commodity item")]
    public void WhenIEnterNameInTheSelectedCommodityItem(string name)
    {
        this.EnterNameForSelectedCommodity(name);
    }

    private void SubmitTime()
    {
        var subGrid = this.GetTimeRecordingGrid();
        CommandHelper.ClickCommand(Driver, subGrid, "Submit Time");
    }

    private void RemoveTime()
    {
        var subGrid = this.GetTimeRecordingGrid();
        CommandHelper.ClickCommand(Driver, subGrid, "Remove Time Entries");
    }

    private IWebElement GetTimeRecordingGrid()
    {
        IWebElement subGrid;
        if (!Driver.TryFindElement(By.XPath("//*[contains(translate(@data-control-name, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'timerecording') and contains(translate(@data-control-name, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'grid')]"), out subGrid))
        {
            throw new ElementNotVisibleException("Cannot find a time recording subgrid.");
        }

        return subGrid;
    }

    public void SelectSubgrideRecordItems()
    {
        this.SelectSubgridRecords.Click();
        Driver.WaitForTransaction();
    }

    public void EnterNameForSelectedCommodity(string name)
    {
        this.NameForSelectedCommodity.SendKeys(name);
    }
}
