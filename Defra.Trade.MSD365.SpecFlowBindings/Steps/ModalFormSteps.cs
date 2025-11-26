namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Polly;
using System;
using System.Linq;
using Reqnroll;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Field = Microsoft.Dynamics365.UIAutomation.Api.UCI.Field;
using LookupItem = Microsoft.Dynamics365.UIAutomation.Api.UCI.LookupItem;

[Binding]
public sealed class ModalFormSteps : PowerAppsStepDefiner
{
    private readonly ScenarioContext scenarioContext;

    public ModalFormSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    [When(@"I select the '(.*)' command under the '(.*)' flyout in the '(.*)' modal form")]
    public void WhenISelectTheCommandUnderTheFlyoutInTheModalForm(string commandName, string flyoutName, string modalFormDisplayName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        CommandHelper.ClickCommand(Driver, formContext, flyoutName, commandName);
    }

    [When(@"I select the '(.*)' tab on the '(.*)' modal form")]
    public void WhenISelectTheTabOnTheModalForm(string tabName, string modalFormDisplayName)
    {
        // Wait for the modal form to display before proceeding
        FormHelper.WaitForModalContext(
            Driver,
            (formContext) =>
            {
                formContext.WaitUntilVisible(By.CssSelector($"li[title=\"{tabName}\"]"));
                var tab = FormHelper.FindTab(formContext, tabName);
                tab.Click();
                Driver.WaitForTransaction();
            },
            "ModalFormContext could not be found.");
    }

    [When(@"I enter '(.*)' into the '(.*)' (currency|numeric|text|datetime|boolean|optionset) field on the '(.*)' modal form")]
    public void WhenIEnterIntoTheFieldOnTheModalForm(string value, string fieldLogicalName, string controlType, string modalFormDisplayName)
    {
        Driver.WaitForTransaction();
        var formContext = FormHelper.GetFormContext(Driver);
        var control = ControlHelper.FindControl(formContext, fieldLogicalName);
        control.Should().NotBeNull($"Unable to find the element {fieldLogicalName}");

        switch (controlType)
        {
            case "numeric":
                var numericControl = ControlHelper.FindControl(control, fieldLogicalName, controlType);
                FormHelper.ClearFieldValue(Driver, numericControl);
                numericControl.SendKeys(value);
                break;
            case "text":
                var textControl = ControlHelper.FindControl(control, fieldLogicalName, controlType);
                FormHelper.ClearFieldValue(Driver, textControl);
                textControl.SendKeys(value);
                break;
            case "optionset":
                var optionSetInputControl = ControlHelper.FindControl(formContext, fieldLogicalName, controlType);
                optionSetInputControl.SelectByText(value);
                break;
            default:
                control.Click();
                FormHelper.ClearFieldValue(Driver, control);
                control.SendKeys(value);
                break;
        }
        Driver.WaitForTransaction();
    }

    [Then(@"I (can|cannot) see the following commands within the '(.*)' modal form")]
    [Obsolete("This method has been deprecated, please use ThenIShouldSeeTheCommands.", true)]
    public void ThenIShouldSeeTheCommandWithinTheModalForm(string canOrCannot, string modalFormDisplayName, Table table)
    {
        ThenIShouldSeeTheCommands(canOrCannot, table);
    }

    [Then(@"I (can|cannot) see the following commands")]
    public void ThenIShouldSeeTheCommands(string canOrCannot, Table table)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var canEdit = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);
        var commandButtons = CommandHelper.GetCommands(Driver, formContext);

        foreach (var row in table.Rows)
        {
            var isVisible = commandButtons.ContainsKey(row["commandName"]);

            if (canEdit)
            {
                isVisible.Should().BeTrue();
            }
            else
            {
                isVisible.Should().BeFalse();
            }
        }
    }

    [Then(@"the following fields (can|cannot) be edited within the '(.*)' modal form")]
    public static void ThenTheFollowingFieldsCanBeEditedWithinTheModalForm(string canOrCannot, string modalFormDisplayName, Table table)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var canEdit = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);
        foreach (var row in table.Rows)
        {
            var field = ControlHelper.FindControl(formContext, row["fieldName"]);
            var isReadonly = ControlHelper.IsReadOnly(field);

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lookupValue"></param>
    /// <param name="fieldName"></param>
    /// <param name="formDisplayName"></param>
    /// <param name="lookupIndex"></param>
    [Given(@"I enter '(.*)' into the '(.*)' lookup field within the (.*) modal form and select item '(.*)'")]
    [When(@"I enter '(.*)' into the '(.*)' lookup field within the (.*) modal form and select item '(.*)'")]
    public void WhenIEnterIntoTheLookupFieldWithinTheModalFormAndSelectItem(string lookupValue, string fieldName, string modalFormDisplayName, int lookupIndex)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var control = ControlHelper.FindControl(formContext, fieldName);
        var lookupItem = new LookupItem() { Name = fieldName, Value = lookupValue, Index = lookupIndex };
        ControlHelper.SetValue(lookupItem, control, Driver);
    }

    [Then(@"I (can|cannot) see the '(.*)' command within the '(.*)' modal form")]
    public void ThenICanSeeTheCommandWithinTheModalForm(string canOrCannot, string commandName, string modalFormDisplayName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var commands = CommandHelper.GetCommands(Driver, formContext);
        var commandLabels = commands.Select(c => c.Key);
        var isVisible = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);

        if (isVisible)
        {
            commandLabels.Should().Contain(commandName);
        }
        else
        {
            commandLabels.Should().NotContain(commandName);
        }
    }

    [Then(@"the following fields (should|should not) contain data within the '(.*)' modal form")]
    public void ThenTheFollowingFieldsShouldContainDataWithinTheModalForm(string shouldOrShouldNot, string modalFormDisplayName, Table table)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var shouldContainData = shouldOrShouldNot.Equals("should", StringComparison.InvariantCultureIgnoreCase);
        foreach (var row in table.Rows)
        {
            var field = ControlHelper.FindControl(formContext, row["fieldName"]);

            if (shouldContainData)
            {
                field.Text.Should().NotBeNullOrEmpty();
            }
            else
            {
                field.Text.Should().BeNullOrEmpty();
            }
        }
    }

    /// <summary>
    /// Clicks the specified command.
    /// </summary>
    /// <param name="commandName">Name of the command to click.</param>
    /// <param name="formDisplayName">Display name of the form visible in Dynamics.</param>
    [When(@"I click the '(.*)' command within the '(.*)' modal form")]
    public void WhenIClickTheCommandWithinTheModalForm(string commandName, string formDisplayName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        CommandHelper.ClickCommand(Driver, formContext, commandName);
    }

    [When(@"I close the '(.*)' modal form")]
    public void WhenICloseTheModalForm(string formDisplayName)
    {
        Driver.WaitForTransaction();

        var formContext = FormHelper.GetFormContext(Driver);
        var closeButton = formContext.FindElement(By.XPath(".//button[@data-id = 'dialogCloseIconButton']"));
        closeButton.Click();

        Driver.WaitForTransaction();
    }

    [Then(@"I can see a value of '(.*)' in the '(.*)' (lookup|numeric|text|input|datetime|inputdatetime|boolean|buttonset|optionset|statecode) (field|header field) within the '(.*)' modal form")]
    public static void ThenICanSeeAValueOfInTheFieldWithinTheModalForm(string expectedValue, string fieldName, string fieldType, string location, string formDisplayName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var field = ControlHelper.GetControlField(formContext, fieldName, fieldType, location, Driver);
        field.Should().NotBeNull();
        field.Value.Should().Be(expectedValue);
    }

    public static void ThenICanSeeAValueTheModalForm(string fieldName, string fieldType, string location, string formDisplayName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var field = ControlHelper.GetControlField(formContext, fieldName, fieldType, location, Driver);
        field.Should().NotBeNull();
    }

    [Then(@"the value of the (lookup|numeric|text|datetime|boolean|optionset|statecode) (field|header field) (.*) and (field|header field) (.*) should be the same on the (.*) modal form")]
    public void ThenTheValueOfTheFieldsShouldMatchOnTheModalForm(string fieldType, string locationA, string fieldAName, string locationB, string fieldBName, string modalFormDisplayName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var fieldA = ControlHelper.GetControlField(formContext, fieldAName, fieldType, locationA, Driver);
        var fieldB = ControlHelper.GetControlField(formContext, fieldBName, fieldType, locationB, Driver);
        fieldA.Should().NotBeNull();
        fieldB.Should().NotBeNull();
        fieldA.Value.Should().Be(fieldB.Value);
    }

    [Then(@"I can see the following values within the '(.*)' modal form")]
    public void ThenICanSeeTheFollowingValuesWithinTheModalForm(string formDisplayName, Table table)
    {
        var formContext = FormHelper.GetFormContext(Driver);

        foreach (var row in table.Rows)
        {
            var field = ControlHelper.GetControlField(formContext, row["schemaName"], row["fieldType"], row["location"], Driver);
            field.Should().NotBeNull();
            field.Value.Should().Be(row["expectedValue"]);
        }
    }

    [Then(@"I can see the '(.*)' subgrid on the '(.*)' modal form")]
    public void ThenICanSeeTheSubgridOnTheModalForm(string subGridName, string formDisplayName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var subGrid = GridHelper.GetGrid(Driver, formContext, subGridName);
        subGrid.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies the fields are visible on the form.
    /// </summary>
    /// <param name="formDisplayName">The friendly display name of the form.</param>
    /// <param name="table"></param>
    [Then(@"I can see the following fields within the '(.*)' modal form")]
    public void ThenICanSeeTheFollowingFieldsWithinTheModalForm(string formDisplayName, Table table)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var formIsReadOnly = FormHelper.IsReadOnly(formContext);

        // Check the state for the form before we attempt to validate the state of fields
        if (formIsReadOnly)
        {
            throw new Exception("Assertions of control states cannot be made when the form is in read-only mode.");
        }

        foreach (var item in table.Rows)
        {
            var element = ControlHelper.FindControl(formContext, item["schemaName"]);
            element.Should().NotBeNull();

            var isReadOnly = ControlHelper.IsReadOnly(element);
            var canEdit = item["mode"].Equals("editable", StringComparison.InvariantCultureIgnoreCase);
            if (canEdit)
            {
                isReadOnly.Should().BeFalse();
            }
            else
            {
                isReadOnly.Should().BeTrue();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canOrCannot">Whether the control should be accept the value.</param>
    /// <param name="formDisplayName">The friendly name of the form.</param>
    /// <param name="table"></param>
    [Then(@"I (can|cannot) enter the following within the '(.*)' modal form")]
    public void ThenIEnterTheFollowingWithinTheModalForm(string canOrCannot, string formDisplayName, Table table)
    {
        canOrCannot = canOrCannot ?? throw new ArgumentNullException(nameof(canOrCannot));

        foreach (var row in table.Rows)
        {
            var value = row["value"];
            var fieldName = row["fieldName"];
            var fieldType = row["fieldType"];

            try
            {
                var field = this.SetControlValue(value, fieldName, fieldType);
                field.Should().NotBeNull();

                if (canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase))
                {
                    field.Value.Should().Be(value);
                }
                else
                {
                    field.Value.Should().BeEmpty();
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new WebDriverTimeoutException($"Setting field value timed out. It's possible that {value} is not a valid value for field {fieldName}, or something else went wrong. Refer to screenshot.", ex);
            }
        }
    }

    /// <summary>
    /// Asserts whether the field can|cannot accept a given value.
    /// </summary>
    /// <param name="canOrCannot">Whether the control should be accept the value.</param>
    /// <param name="value">The value of the control.</param>
    /// <param name="fieldName">The schema name of the field.</param>
    /// <param name="fieldType">The type of field.</param>
    /// <param name="formDisplayName">The friendly name of the form.</param>
    [Then(@"I (can|cannot) enter '(.*)' into the '(.*)' (lookup|optionset) field within the '(.*)' modal form")]
    public void ThenIEnterIntoTheFieldWithinTheModalForm(string canOrCannot, string value, string fieldName, string fieldType, string formDisplayName)
    {
        canOrCannot = canOrCannot ?? throw new ArgumentNullException(nameof(canOrCannot));
        value = value ?? throw new ArgumentNullException(nameof(value));
        fieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
        fieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));

        try
        {
            var field = this.SetControlValue(value, fieldName, fieldType);
            field.Should().NotBeNull();

            if (canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase))
            {
                field.Value.Should().Be(value);
            }
            else
            {
                field.Value.Should().BeEmpty();
            }
        }
        catch (WebDriverTimeoutException)
        {
            canOrCannot.Should().Be("cannot", because: $"{value} is not a valid value for field {fieldName}");
        }
    }

    /// <summary>
    /// Sets the control to the specified value.
    /// </summary>
    /// <param name="value">The value to set within the control.</param>
    /// <param name="fieldName">The schema name of the field.</param>
    /// <param name="fieldType">The type of field.</param>
    /// <param name="location">The location of the field field|header.</param>
    /// <param name="reset">true to clear the contents of the control; otherwise false.</param>
    /// <returns></returns>
    private Field SetControlValue(string value, string fieldName, string fieldType, string location = "field", bool reset = true)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var control = ControlHelper.FindControl(formContext, fieldName);

        if (reset)
        {
            control.Clear(Driver);
        }

        if (fieldType == "lookup")
        {
            var lookupItem = new LookupItem() { Name = fieldName, Value = value, Index = 0 };
            ControlHelper.SetValue(lookupItem, control, Driver);
        }
        else
        {
            ControlHelper.SetValue(value, control);
        }

        return ControlHelper.GetControlField(formContext, fieldName, fieldType, location, Driver);
    }
}
