namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reqnroll;

[Binding]
public sealed class QuickCreateFormSteps : PowerAppsStepDefiner
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IWebElement _formContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuickCreateFormSteps"/> class.
    /// </summary>
    /// <param name="scenarioContext">An instance of a secenario context.</param>
    public QuickCreateFormSteps(ScenarioContext scenarioContext)
    {
        this._scenarioContext = scenarioContext;
        this._formContext = Driver.WaitUntilAvailable(By.XPath("//div[contains(@data-uci-dialog, 'true') and contains(@role, 'dialog')]"));
        Driver.WaitForTransaction();
    }

    [When(@"I can see the '(.*)' quick create form")]
    public void WhenICanSeeTheQuickCreateForm(string quickCreateFormName)
    {
        this._formContext.IsVisible().Should().BeTrue();
    }

    [Then(@"I can see the '(.*)' quick create form")]
    public void ThenICanSeeTheQuickCreateForm(string quickCreateFormName, Table table)
    {
        this._formContext.Should().NotBeNull();

        var headerTitle = this._formContext.FindElement(By.XPath(".//h1[contains(@data-id, 'quickHeaderTitle')]"));
        headerTitle.Should().NotBeNull();
        headerTitle.Text.Should().Be($"Quick Create: {quickCreateFormName}");

        foreach (var row in table?.Rows)
        {
            var control = ControlHelper.FindControl(this._formContext, row["fieldName"]);
            control.Should().NotBeNull();
        }
    }

    [Then(@"the following fields (can|cannot) be edited within the quick create form")]
    public void ThenTheFollowingFieldsCanBeEditedWithinTheQuickCreateForm(string canOrCannot, Table table)
    {
        var canEdit = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);
        foreach (var row in table.Rows)
        {
            var control = ControlHelper.FindControl(this._formContext, row["fieldName"]);
            var isReadonly = ControlHelper.IsReadOnly(control);

            if (canEdit)
            {
                isReadonly.Should().BeFalse();
            }
            else
            {
                isReadonly.Should().BeTrue();
            }
        }
    }

    
    [Then(@"I can see the Save And Close command within the quick create form")]
    public void ThenICanSeeTheSaveAndCloseCommandWithinTheQuickCreateForm()
    {
        var commandButton = this._formContext.FindElement(By.XPath(".//button[contains(@data-id, 'quickCreateSaveAndCloseBtn')]"));
        commandButton.Should().NotBeNull();
        commandButton.Text.Should().Be("Save and Close");
    }

    [When(@"I click save and Close button in the quick create")]
    public void WhenIClickSaveAndCloseButtonInTheQuickCreate()
    {
        var commandButton = this._formContext.FindElement(By.XPath(".//button[contains(@data-id, 'quickCreateSaveAndCloseBtn')]"));
        commandButton.Click();
    }


    [Then(@"An error message is displayed in the quick create form '(.*)' is displayed")]
    public void ThenAnErrorMessageIsDisplayedInTheQuickCreateFormIsDisplayed(string message)
    {
        Driver.WaitForTransaction();
        var errorMessage = Driver.WaitUntilVisible(By.XPath($"//span[contains(@data-id, 'error-message')]"));
        errorMessage.Text.Should().Be(message);
    }
}
