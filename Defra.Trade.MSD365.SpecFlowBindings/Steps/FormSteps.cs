// <copyright file="FormSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Reqnroll;

[Binding]
public class FormSteps : PowerAppsStepDefiner
{
    private readonly SessionContext ctx;
    private readonly ScenarioContext scenContext;

    public FormSteps(SessionContext context, ScenarioContext scenctx)
    {
        this.ctx = context;
        this.scenContext = scenctx;
    }

    /// <summary>
    /// Asserts that a value is not shown in a lookup field.
    /// </summary>
    /// <param name="unexpectedValue">The unexpected value.</param>
    /// <param name="field">The field name.</param>
    /// <param name="fieldLocation">Where the field is located.</param>
    [Then("I cannot see a value of '(.*)' in the '(.*)' lookup (field|header field)")]
    public static void ThenICanSeeAValueOfInTheLookupField(string unexpectedValue, LookupItem field, string fieldLocation)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(1))
            .Execute(() =>
            {
                var actualValue = fieldLocation == "field" ? XrmApp.Entity.GetValue(field) : XrmApp.Entity.GetHeaderValue(field);
                actualValue.Should().NotBe(unexpectedValue);
            });
    }

    /// <summary>
    /// Enters a random value into an option set.
    /// </summary>
    /// <param name="logicalName">The logical name of the option set.</param>
    [When(@"I enter any option into the '(.*)' optionset field")]
    public void WhenIEnterAnyOptionIntoTheOptionsetFieldOnTheForm(string logicalName)
    {
        var fieldContainer = Driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.OptionSetFieldContainer].Replace("[NAME]", logicalName)));

        int[] options;
        if (fieldContainer.TryFindElement(By.TagName("select"), out IWebElement select))
        {
            options = select.FindElements(By.TagName("option")).Select(o => o.GetAttribute<int>("value")).Where(v => v > -1).ToArray();
        }
        else if (fieldContainer.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusCombo].Replace("[NAME]", logicalName)), out var statusCombo))
        {
            statusCombo.Click();
            var listBox = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusComboList].Replace("[NAME]", logicalName)));
            options = listBox.FindElements(By.TagName("li")).Select(o => o.GetAttribute<int>("data-value")).Where(v => v > -1).ToArray();
        }
        else
        {
            throw new InvalidOperationException($"Unable to find '{logicalName}' option set.");
        }

        XrmApp.Entity.SetValue(new OptionSet { Name = logicalName, Value = options[new Random().Next(0, options.Length - 1)].ToString() });
    }

    [Then(@"I can see the following values in the form")]
    public static void ThenICanSeeTheFollowingValuesInTheForm(Table fields)
    {
        fields = fields ?? throw new ArgumentNullException(nameof(fields));

        foreach (DataTableRow row in fields.Rows)
        {
            var field = ControlHelper.GetControlField(FormHelper.GetFormContext(Driver), row["Field"], row["Type"], row["Location"], Driver);
            field.Value.Should().Be(row["Value"]);
        }
    }

    [Then(@"I can see a (readonly|editable) form for the (.*) entity")]
    public static void ICanSeeAForm(string formType, string entityName)
    {
        Driver.WaitForTransaction();

        XrmApp.Entity.GetEntityName().Should().Be(entityName);

        var expectedToBeReadOnly = formType == "readonly";

        // TODO: Xrm.Page is deprecated. This needs to be replaced.
        var formIsReadOnly = (bool)Driver.ExecuteScript("var formType = Xrm.Page.ui.getFormType(); return formType == 3 || formType == 4;");
        formIsReadOnly.Should().Be(expectedToBeReadOnly);
    }

    [When("I(can | can not) see the following fields on the form")]
    [Then("I (can|can not) see the following fields on the form")]
    public static void ICanSeeFields(string canOrCannot, Table fieldList)
    {
        canOrCannot = canOrCannot ?? throw new ArgumentNullException(nameof(canOrCannot));
        bool shouldBeVisible = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);
        var schemaNames = fieldList.Rows.Select((row) => row.Values.Last());
        var formContext = FormHelper.GetFormContext(Driver);

        foreach (var schemaName in schemaNames)
        {
            if (shouldBeVisible)
            {
                var field = XrmApp.Entity.GetField(schemaName);
                field.IsVisible.Should().BeTrue(because: "the field should be visible.");
                var displayLabel = fieldList.Rows.Where(r => r["schemaName"] == schemaName).Select(r => r["displayName"]).FirstOrDefault();
                field.Label.Should().Be(displayLabel);
            }
            else
            {
                EntitySteps.ThenICanNotSeeTheField(schemaName);
            }
        }
    }

    [Then(@"I can see a (readonly|editable) header field named (.*)")]
    public static void ICanSeeAHeaderField(string controlType, string fieldName)
    {
        var isReadonly = new Helpers.HeaderHelper(Driver).IsHeaderFieldReadonly(fieldName);

         var expectedReadOnly = controlType == "readonly";

        isReadonly.Should().Be(expectedReadOnly);
    }

    [Then(@"(text|optionset|multioptionset|boolean|numeric|currency|datetime|lookup) (field|header field) '(.*)' (contains|does not contain) data")]
    public static void FieldContainsData(string fieldType, string fieldLocation, string fieldName, string operation)
    {
        // TODO: Xrm.Page is deprecated - this needs replaced. 
        var value = Driver.ExecuteScript($"var attr = Xrm.Page.getAttribute('{fieldName}'); if (attr) {{ return attr.getValue(); }}");

        if (operation == "contains")
        {
            value.Should().NotBeNull();
        }
        else if (operation == "does not contain")
        {
            value.Should().BeNull();
        }
    }

    [When(@"I enter '(.*)' into the '(.*)' (text|optionset|multioptionset|boolean|numeric|currency|datetime|lookup) (field|header field) on the form and validate the linked fields")]
    public static void WhenIEnterIntoTheOptionsetFieldOnTheFormAndValidateTheFields(string fieldValue, string fieldName, string fieldType, string fieldLocation, Table table)
    {
        foreach (var row in table.Rows)
        {
            EntitySteps.WhenIEnterInTheField(row["fieldValue"], fieldName, fieldType, fieldLocation);
            EntitySteps.ThenICanEditTheField(row["datefield"]);
            EntitySteps.ThenICanSeeTheField(row["datefield"]);
            EntitySteps.ThenTheFieldIsMandatory(row["datefield"], "mandatory");
            EntitySteps.ThenICanEditTheField(row["diagnosisfield"]);
            EntitySteps.ThenICanSeeTheField(row["diagnosisfield"]);
            EntitySteps.ThenTheFieldIsMandatory(row["diagnosisfield"], "mandatory");
            ThenICanSeeTheMaximumLengthOfTheTextFieldIs(row["label"], Convert.ToInt32(row["maxlength"]));
        }
    }

    [When("I click on the '(.*)' field")]
    public static void WhenIClickOnField(string fieldName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var button = ControlHelper.GetButtonControl(formContext, fieldName);
        button.Click();
    }

    [Then(@"I can see the maximum length of the '(.*)' text field is (\d+)")]
    public static void ThenICanSeeTheMaximumLengthOfTheTextFieldIs(string labelName, int expected)
    {
        var textSize = Driver.WaitUntilAvailable(By.XPath($"//*[@aria-label='{labelName}']")).GetAttribute<int>("maxlength");

        textSize.Should().Be(expected);
    }

    [Then(@"datetime (field|header field) '(.*)' contains today's date")]
    public static void FieldContainsTodaysDate(string fieldLocation, string fieldName)
    {
        DateTime? actualDate;
        if (fieldLocation == "header field")
        {
            actualDate = XrmApp.Entity.GetHeaderValue(new DateTimeControl(fieldName));
        }
        else
        {
            actualDate = XrmApp.Entity.GetValue(new DateTimeControl(fieldName));
        }

        actualDate.Value.Date.Should().Be(DateTime.Today);
    }

    [Then(@"datetime field '(.*)' contains '(.*)' plus '(.*)' days")]
    public void ThenDatetimeFieldContainsPlusXDays(string expiryDate, string inspectionDate, int days)
    {
        DateTime? actualInspectionDate;
        DateTime? endDate;
        actualInspectionDate = XrmApp.Entity.GetValue(new DateTimeControl(inspectionDate));
        endDate = XrmApp.Entity.GetValue(new DateTimeControl(expiryDate));
        endDate.Value.Date.Should().Be(actualInspectionDate.Value.AddDays(days));
    }

    /// <summary>
    /// Checks that datetime field value holds a specific date.
    /// </summary>
    /// <param name="fieldName">Logical name of the field to be asserted.</param>
    /// <param name="day">Numerical value for the day [1-31].</param>
    /// <param name="month">Numerical value for the month [1-12].</param>
    [Then(@"the value within the '(.*)' datetime field is the next (.*)/(.*)")]
    public void ThenTheValueWithinTheDatetimeFieldIsInTheMonth(string fieldName, int day, int month)
    {
        var date = new DateTime(DateTime.Today.Year, month, day);

        if (date < DateTime.Today)
        {
            // if date is in the past, add a year
            date = date.AddYears(1);
        }

        var fieldDate = XrmApp.Entity.GetValue(new DateTimeControl(fieldName));
        fieldDate.Value.Day.Should().Be(date.Day);
        fieldDate.Value.Month.Should().Be(date.Month);
        fieldDate.Value.Year.Should().Be(date.Year);
    }

    [Then(@"the record is owned by a user or team called '(.*)'")]
    public static void RecordIsOwnedBy(string expectedOwner)
    {
        GetOwner().Name.Should().Be(expectedOwner);
    }

    [When(@"I open the owner record")]
    public static void OpenOwnerRecord()
    {
        var owner = GetOwner();
        XrmApp.Entity.OpenEntity(owner.LogicalName, owner.Id);
    }

    [Then("I can see chargeid has been created")]
    public static void ThenICanSeeChargeIdHasBeenCreated()
    {
        ThenICanSeeAValueInTheField("trd_chargeid", "lookup", "field");
    }

    [Then("The following fields are (mandatory|optional)")]
    public static void FollowingFieldsAreMandatory(string requiredLevel, Table fields)
    {
        var fieldNames = fields.Rows.Select((row) => row.Values.First());
        foreach (string name in fieldNames)
        {
            XrmApp.Entity.GetField(name).IsRequired.Should().Be(requiredLevel == "mandatory" ? true : false);
        }
    }

    [Then("I can see (|only )the following forms in the form selector")]
    public static void ThenICanSeeOnlyFollowingForms(string only, Table forms)
    {
        var formNames = forms.Rows.Select((row) => row.Values.First()).ToList();
        if (formNames.Count == 1)
        {
            Driver.HasElement(By.XPath("//span[@data-id = 'form-selector']")).Should()
                .BeFalse("because only one form should be visible, so the form selector should not be displayed");
            Driver.FindElement(By.XPath("//span[@data-id = 'entity_name_span']")).GetAttribute("innerText").Should().Be(formNames[0]);
        }
        else
        {
            var actualFormNames = GetFormList();
            formNames.ForEach(x => actualFormNames.Should().Contain(x));
            if (only == "only ")
            {
                actualFormNames.Count.Should().Be(formNames.Count);
            }
        }
    }

    private static List<string> GetFormList()
    {
        if (!Driver.HasElement(By.XPath("//li[contains(@data-id, 'form-selector-item')]")))
        {
            Driver.WaitUntilClickable(By.XPath("//span[@data-id = 'form-selector']")).Click();
        }

        var list = Driver.FindElements(By.XPath("//li[contains(@data-id, 'form-selector-item')]")).Select(x => x.GetAttribute("innerText")).ToList();
        return list;
    }

    [Then("I can see a value in the '(.*)' (lookup|text) (field|header field)")]
    public static void ThenICanSeeAValueInTheField(string field, string fieldType, string fieldLocation)
    {
        string actualValue;
        if (fieldType == "lookup")
        {
            actualValue = fieldLocation == "field" ? XrmApp.Entity.GetValue(new LookupItem() { Name = field }) : XrmApp.Entity.GetHeaderValue(field);
        }
        else
        {
            actualValue = fieldLocation == "field" ? XrmApp.Entity.GetValue(field) : XrmApp.Entity.GetHeaderValue(field);
        }

        actualValue.Should().NotBeNullOrEmpty($"{field} is empty");
    }

    // Multi-option-set support in EasyRepro is currently not working as of 15th October 2021
    [When("I select value '(.*)' in the '(.*)' multioptionset field")]
    public static void WhenISelectMultiOptionValue(string val, string field)
    {
        // Temporarily using JavaScript (deprecated)
        var script = $"Xrm.Page.getAttribute('{field}').setValue([{val}]);Xrm.Page.getAttribute('{field}').fireOnChange();";
        Driver.ExecuteScript(script);

        // replace the above with this when working
        //Capgemini.PowerApps.SpecFlowBindings.Steps.EntitySteps.WhenIEnterInTheField(val, field, "multioptionset", "field");
    }

    [Then("there is a value of '(.*)' in the '(.*)' field")]
    public static void VerifyValue(string val, string fieldName)
    {
        GetFieldValue(fieldName).ToString().Should().Be(val, $"Field name {fieldName}");
    }

    [Scope(Tag = "Pheats")]
    [Then("the label '(.*)' in the '(.*)' field displayed")]
    public static void VerifyOptionsetLabel(string val, string fieldName)
    {
        switch (val)
        {
            case "Passed":
                if (val == "434800000" && fieldName == "statuscode")
                {
                    GetFieldValue(fieldName).ToString().Should().Be(val, $"Field name {fieldName}");
                }

                break;

            case "Failed":
                if (val == "434800001" && fieldName == "statuscode")
                {
                    GetFieldValue(fieldName).ToString().Should().Be(val, $"Field name {fieldName}");
                }

                break;

            case "Site Audit":
                if (val == "434800000" && fieldName == "trd_audittype")
                {
                    GetFieldValue(fieldName).ToString().Should().Be(val, $"Field name {fieldName}");
                }

                break;

            case "First Inspection":
                if (val == "434800001" && fieldName == "trd_audittype")
                {
                    GetFieldValue(fieldName).ToString().Should().Be(val, $"Field name {fieldName}");
                }

                break;
        }
    }

    [Then("an error is displayed next to the '(.*)' field that states '(.*)'")]
    public static void ErrorIsDisplayedOnField(string field, string expectedText)
    {
        var errorBox = Driver.FindElement(By.XPath($".//span[@data-id='{field}-error-message']"));

        errorBox.Text.Should().Contain(expectedText);
    }

    [When("I select the value '(.*)' in the '(.*)' lookup (field|header field) on the form")]
    public static void EnterLookupValue(string value, string fieldName, string location)
    {
        // TODO: check if this is still broken in Capgemini.PowerApps.SpecFlowBindings. Raise an issue on the repository if so.
        if (location == "header field")
        {
            XrmApp.Entity.SetHeaderValue(new LookupItem() { Value = value, Name = fieldName });
        }
        else
        {
            XrmApp.Entity.SetValue(new LookupItem() { Value = value, Name = fieldName });
        }

        // Click to lose focus - So that business rules and other form events can occur
        Driver.FindElement(By.XPath("html")).Click();
        Driver.WaitForTransaction();
    }

    [When("I expand the '(.*)' lookup on the form")]
    public static void WhenIExpandTheLookup(LookupItem control)
    {
        if (control is null)
        {
            throw new ArgumentNullException(nameof(control));
        }

        var fieldContainer = Driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", control.Name)));
        var input = fieldContainer.FindElement(By.TagName("input"));
        input.Click();
        input.SendKeys(Keys.Enter);
    }

    [When("I select the related '(.*)' view")]
    public static void ISelectARelatedView(string relatedView)
    {
        var xpath = $"//div[starts-with(@aria-label, '{relatedView} Related') and @role='menuitem']";
        Driver.FindElement(By.XPath(xpath)).Click();
        SharedSteps.WaitForScriptProcessing();
    }

    [When("I set datetime field '(.*)' to '(.*)' at '(.*)'")]
    public static void SetDateTime(string fieldName, string date, string time)
    {
        SetDateTime(fieldName, date, time, null, 0, null);
    }

    [When("I set datetime field '(.*)' to '(.*)' at '(.*)' (plus|minus) '(.*)' (day|days|hour|hours|minute|minutes)")]
    public static void SetDateTime(string fieldName, string date, string time, string offsetType, int offset, string unit)
    {
        DateTime dt;

        switch (date.ToLower())
        {
            case "today":
                dt = DateTime.Today;
                break;

            case "yesterday":
                dt = DateTime.Today.AddDays(-1);
                break;

            case "yesterday-1":
                dt = DateTime.Today.AddDays(-2);
                break;

            case "yesterday-3":
                dt = DateTime.Today.AddDays(-4);
                break;

            case "tomorrow":
                dt = DateTime.Today.AddDays(1);
                break;

            default:
                dt = DateTime.Parse(date);
                break;
        }

        var now = DateTime.Now;
        TimeSpan ts;

        switch (time)
        {
            case "now":
                dt = new DateTime(dt.Year, dt.Month, dt.Day, now.Hour, now.Minute, now.Second);
                break;

            case "midnight":
            case "":
                break;

            default:
                ts = TimeSpan.Parse(time);
                dt = new DateTime(dt.Year, dt.Month, dt.Day, ts.Hours, ts.Minutes, ts.Seconds);
                break;
        }

        if (offset != 0)
        {
            switch (unit)
            {
                case "day":
                case "days":
                    offset *= 24 * 3600;
                    break;

                case "hour":
                case "hours":
                    offset *= 3600;
                    break;

                case "minute":
                case "minutes":
                    offset *= 60;
                    break;

                default:
                    throw new ApplicationException($"Unexpected unit '{unit}'");
            }

            dt = dt.AddSeconds((offsetType == "minus" ? -1 : 1) * offset);
        }

        // TODO: Xrm.Page is deprecated - this needs replaced. 
        var jsVal = dt.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        // TODO: The APIs should very rarely if ever be used in a 'When' step. This is not representative of user interaction. Needs replaced. 
        Driver.ExecuteScript($"Xrm.Page.getAttribute('{fieldName}').setValue(new Date(Number({(long)jsVal})));Xrm.Page.getAttribute('{fieldName}').fireOnChange();");
    }

    // TODO: check if this is still broken in Capgemini.PowerApps.SpecFlowBindings. Raise an issue on the repository if so.
    [Then("(A warning|An error|An info) notification with the message '(.*)' is displayed")]
    public static void ValidateFormNotifications(string level, string message)
    {
        Driver.WaitForTransaction();

        var type = level.Split(' ')[1].ToUpper();
        var notificationsList = Driver.WaitUntilVisible(By.CssSelector("ul[data-id=notificationList]"));

        string symbolClassName;
        if (type == "INFO")
        {
            symbolClassName = "InformationIcon";
        }
        else if (type == "WARNING")
        {
            symbolClassName = "warningNotification";
        }
        else
        {
            symbolClassName = "MarkAsLost";
        }

        var notifications = notificationsList.FindElements(By.TagName("li")).Where(e => e.FindElements(By.ClassName($"{symbolClassName}-symbol")).Any());
        notifications.Should().Contain(e => e.Text == message);
    }

    // TODO: Variables are refering to the test implementation, not documenting system behaviour. Needs replaced.
    [When("I enter the value from the variable '(.*)' into the '(.*)' text field on the form")]
    public void EnterVariableIntoField(string variable, string field)
    {
        XrmApp.Entity.SetValue(field, this.ctx.GetVariable(variable)?.ToString() ?? string.Empty);
    }

    // TODO: Variables are refering to the test implementation, not documenting system behaviour. Needs replaced.
    [When("I enter the value from the variable '(.*)' into the '(.*)' datetime field on the form")]
    public void EnterDateTimeVariableIntoField(string variable, string field)
    {
        var val = this.ctx.GetVariable(variable);
        if (val is string)
        {
            val = DateTime.Parse(val as string);
        }

        var value = ((DateTime)val).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        // TODO: Xrm.Page is deprecated - this needs replaced. 
        // TODO: The APIs should very rarely if ever be used in a 'When' step. This is not representative of user interaction. Needs replaced. 
        Driver.ExecuteScript($"Xrm.Page.getAttribute('{field}').setValue(new Date(Number({(long)value})));Xrm.Page.getAttribute('{field}').fireOnChange();");
    }

    // TODO: Variables are refering to the test implementation, not documenting system behaviour. Needs replaced.
    // TODO: check if this is still broken in Capgemini.PowerApps.SpecFlowBindings. Raise an issue on the repository if so.
    [When("I select the value from the variable '(.*)' in the '(.*)' lookup (field|header field) on the form")]
    public void EnterLookupValueFromVariable(string variableName, string field, string location)
    {
        var value = this.ctx.GetVariable(variableName)?.ToString() ?? string.Empty;

        if (location == "header field")
        {
            XrmApp.Entity.SetHeaderValue(new LookupItem() { Value = value, Name = field });
        }
        else
        {
            XrmApp.Entity.SetValue(new LookupItem() { Value = value, Name = field });
        }

        // Click to lose focus - So that business rules and other form events can occur
        Driver.FindElement(By.XPath("html")).Click();
    }

    [Then("there is a value of '(.*)' in the '(.*)' field within (.*) seconds if I Refresh periodically")]
    public void WaitForValue(string value, string fieldName, int waitTime)
    {
        int secondsPerLoop = Math.Max(3, waitTime / 10);
        int numLoops = (waitTime / secondsPerLoop) + 2;

        Action waitForValue = () =>
        {
            for (int i = 1; i <= numLoops; i++)
            {
                try
                {
                    VerifyValue(value, fieldName);
                    return;  // done
                }
                catch
                {
                    bool lastLoop = i == numLoops;

                    if (!lastLoop)
                    {
                        if ((i % 2) != 0)
                        {
                            CommandSteps.WhenISelectTheCommand("Refresh", XrmApp.Entity.GetEntityName());
                            SharedSteps.WaitForScriptProcessing();
                        }

                        Thread.Sleep(secondsPerLoop * 1000);
                    }
                    else
                    {
                        throw new Exception("Value did not appear within the field within the given number of seconds.");
                    }
                }
            }
        };

        waitForValue.Should().NotThrow();
    }

    // TODO: Variables are refering to the test implementation, not documenting system behaviour. Needs replaced.
    [When("I store the value from the '(.*)' '(.*)' '(.*)' in a variable named '(.*)'")]
    [When("I store the value from the '(.*)' (text|optionset|multioptionset|boolean|numeric|currency|datetime|lookup) (field|header field) in a variable named '(.*)'")]
    public void StoreFormValueInVariable(string fieldName, string fieldType, string fieldLocation, string variableName)
    {
        if (fieldLocation == "header field" || fieldType == "datetime")
        {
            this.ctx.SetVariable(variableName, GetFieldValue(fieldName));
        }
        else
        {
            var field = XrmApp.Entity.GetField(fieldName);
            this.ctx.SetVariable(variableName, field?.Value ?? GetFieldValue(fieldName)); // sometimes returns null, e.g. msdyn_name - resort to JavaScript if so
        }
    }

    /// <summary>
    /// Asserts that a field is read-only.
    /// </summary>
    /// <param name="fieldName">The name of the field.</param>
    [Then(@"I can't edit the '(.*)' field")]
    public static void ThenICanNotEditTheField(string fieldName)
    {
        XrmApp.Entity.GetField(fieldName).IsReadOnly(Driver).Should().BeTrue(because: "the field should not be editable");
    }

    public static void ThenICanEditTheField(string fieldName)
    {
        XrmApp.Entity.GetField(fieldName).IsReadOnly(Driver).Should().BeFalse(because: "the field should not be editable");
    }

    // TODO: Variables are refering to the test implementation, not documenting system behaviour. Needs replaced.
    [Then("The value in the '(.*)' '(.*)' '(.*)' (matches|does not match) the value in the '(.*)' variable")]
    [Then("The value in the '(.*)' (text|optionset|multioptionset|boolean|numeric|currency|datetime|lookup) (field|header field) (matches|does not match) the value in the '(.*)' variable")]
    public void ValueInFieldMatchesVariable(string fieldName, string fieldType, string fieldLocation, string matchType, string variableName)
    {
        if (fieldLocation == "header field")
        {
            var variable = this.ctx.GetVariable(variableName);

            if (matchType.Contains("not"))
            {
                GetFieldValue(fieldName).Should().NotBe(variable);
            }
            else
            {
                GetFieldValue(fieldName).Should().Be(variable);
            }
        }
        else
        {
            var fieldValue = XrmApp.Entity.GetField(fieldName)?.Value ?? GetFieldValue(fieldName)?.ToString() ?? string.Empty;

            // XrmApp unreliable for some fields, e.g. systemuser.internalemailaddress.
            var variable = this.ctx.GetVariable(variableName)?.ToString();

            // Treat null/empty as the same thing
            if (variable == null || variable == "null")
            {
                variable = string.Empty;
            }

            if (matchType.Contains("not"))
            {
                fieldValue.Should().NotBe(variable);
            }
            else
            {
                fieldValue.Should().Be(variable);
            }
        }
    }

    [When(@"I search for the value '(.*)' in the grid")]
    public void WhenISearchForTheValueInTheGrid(string variableName)
    {
        var varvalue = this.ctx.GetVariable(variableName).ToString();
        XrmApp.Grid.Search(varvalue);

        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Asserts that the current user is the value of a given system user lookup field.
    /// </summary>
    /// <param name="field">The name of the system user lookup field.</param>
    [Then(@"I can see the current user in the '(.*)' system user lookup field")]
    public void ThenICanSeeTheCurrentUserInTheLookupField(string field)
    {
        var currentUser = Driver.ExecuteScript("return Xrm.Utility.getGlobalContext().userSettings.userName") as string;

        if (string.IsNullOrEmpty(currentUser))
        {
            throw new Exception("Unable to get the current user.");
        }

        XrmApp.Entity.GetValue(new LookupItem { Name = field })
            .Should().Be(currentUser, because: "the current user should be visible in the lookup field.");
    }

    [When(@"I scroll until '(.*)' (text|optionset|multioptionset|boolean|numeric|currency|datetime|lookup|subgrid|editablegrid) is visible")]
    public void WhenIScrollUntilSubgridIsInView(string fieldName, string fieldType)
    {
        IWebElement element = null;

        switch (fieldType)
        {
            case "subgrid":
            case "editablegrid":
                element = GridHelper.GetGrid(Driver, fieldName);
                break;
            default:
                element = Driver.WaitUntilAvailable(By.XPath($"//div[contains(@data-id, '{fieldName}-FieldSectionItemContainer')]"));
                break;
        }

        if (element == null)
        {
            throw new NoSuchElementException($"The element for control {fieldName} could not be found.");
        }

        Driver.ScrollIntoView(element);
    }

    public static object GetFieldValue(string fieldName)
    {
        Driver.WaitForTransaction();

        // TODO: Review the need for this. This might have unexpected results as the JavaScript field value is not representative of what is shown in the UI.
        // TODO: Xrm.Page is deprecated - this needs replaced.
        var script = $"var attr = Xrm.Page.getAttribute('{fieldName}'); var val = (attr != null) ? attr.getValue() : null; if (val != null && Array.isArray(val) && val[0] != null) return val[0].name; else return val;";

        return Driver.ExecuteScript(script);
    }
    public static object GetValueOfField(String fieldName)
    {
       return GetFieldValue(fieldName);
    }

    private static EntityReference GetOwner()
    {
        Driver.WaitForTransaction();

        // TODO: Xrm.Page is deprecated - this needs replaced. 
        var script = @"var ownerAttr = Xrm.Page.getAttribute('ownerid');  
                           if (ownerAttr === null) {
                             return null;
                           }
                           var owner = ownerAttr.getValue(); 
                           return owner && owner[0] ? owner[0] : null;";
        var owner = (Dictionary<string, object>)Driver.ExecuteScript(script);
        if (owner == null)
        {
            throw new Exception("Owner is not present on the form");
        }

        return new EntityReference(owner["entityType"].ToString(), new Guid(owner["id"].ToString())) { Name = owner["name"].ToString() };
    }

    private static void CloseTeachingBubbles()
    {
        foreach (var closeButton in Driver.FindElements(By.ClassName("ms-TeachingBubble-closebutton")))
        {
            closeButton.Click();
        }
    }

    // TODO: Variables are refering to the test implementation, not documenting system behaviour. Needs replaced.
    [Scope(Tag = "Pheats")]
    [When("I store the value from the '(.*)' '(.*)' '(.*)' in a variable named '(.*)'")]
    [When("I store the value from the '(.*)' (text|optionset|multioptionset|boolean|numeric|currency|datetime|lookup) (field|header field) in a variable named '(.*)'")]
    public static void StoreFormValueInVariable(string fieldName, string fieldType, string fieldLocation, string variableName, SessionContext context)
    {
        if (fieldLocation == "header field" || fieldType == "datetime")
        {
            context.SetVariable(variableName, GetFieldValue(fieldName));
        }
        else
        {
            var field = XrmApp.Entity.GetField(fieldName);
            context.SetVariable(variableName, field?.Value ?? GetFieldValue(fieldName)); // sometimes returns null, e.g. msdyn_name - resort to JavaScript if so
        }
    }

    // TODO: Variables are refering to the test implementation, not documenting system behaviour. Needs replaced.
    [Scope(Tag = "Pheats")]
    [Then("The value in the '(.*)' '(.*)' '(.*)' (matches|does not match) the value in the '(.*)' variable")]
    [Then("The value in the '(.*)' (text|optionset|multioptionset|boolean|numeric|currency|datetime|lookup) (field|header field) (matches|does not match) the value in the '(.*)' variable")]
    public static void ValueInFieldMatchesVariable(string fieldName, string fieldType, string fieldLocation, string matchType, string variableName, SessionContext context)
    {
        if (fieldLocation == "header field")
        {
            var variable = context.GetVariable(variableName);

            if (matchType.Contains("not"))
            {
                GetFieldValue(fieldName).Should().NotBe(variable);
            }
            else
            {
                GetFieldValue(fieldName).Should().Be(variable);
            }
        }
        else
        {
            var fieldValue = XrmApp.Entity.GetField(fieldName)?.Value ?? GetFieldValue(fieldName)?.ToString() ?? string.Empty;

            // XrmApp unreliable for some fields, e.g. systemuser.internalemailaddress.
            var variable = context.GetVariable(variableName)?.ToString();

            // Treat null/empty as the same thing
            if (variable == null || variable == "null")
            {
                variable = string.Empty;
            }

            if (matchType.Contains("not"))
            {
                fieldValue.Should().NotBe(variable);
            }
            else
            {
                fieldValue.Should().Be(variable);
            }
        }
    }

    [Then(@"the inactive workorder not appeared in any active exports workorder views")]
    public void ThenTheInactiveWorkorderNotAppearedInAnyActiveExportsWorkorderViews()
    {
        Capgemini.PowerApps.SpecFlowBindings.Steps.NavigationSteps.WhenIOpenTheSubAreaUnderTheArea("Work Orders", "Common");
        Capgemini.PowerApps.SpecFlowBindings.Steps.GridSteps.WhenISwitchToTheViewInTheGrid("Active Work Orders - Exports - Trade");
        WhenISearchForTheValueInTheGrid("plntworkorderid");
        GridSteps.CheckGridRowCount(0);
        Capgemini.PowerApps.SpecFlowBindings.Steps.GridSteps.WhenISwitchToTheViewInTheGrid("My Active Work Orders - Exports");
        WhenISearchForTheValueInTheGrid("plntworkorderid");
        GridSteps.CheckGridRowCount(0);
    }

    [When("I select the '(.*)' form if not already selected")]
    public static void WhenISelectForm(string formName)
    {
        if (XrmApp.Entity.GetFormName() != formName)
        {
            XrmApp.Entity.SelectForm(formName);
        }
    }

    [When("I scroll to the '(.*)' section")]
    public static void WhenIScrollToLabel(string label)
    {
        var script = $"(document.querySelector(\"h2[title='{label}']\") || document.querySelector(\"span[title='{label}']\")).scrollIntoView()";
        Driver.ExecuteScript(script);
    }

    [When(@"I select the '(.*)' tab on the current form")]
    public void WhenISelectTheTabOnTheModalForm(string tabName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        formContext.WaitUntilVisible(By.CssSelector($"li[title=\"{tabName}\"]"));
        var tab = formContext.WaitUntilClickable(By.CssSelector($"li[title=\"{tabName}\"]"));
        tab.Click();
        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Asserts if the collection of tabs are currently visible.
    /// </summary>
    /// <param name="canOrCannot">Whether the tab should be visible.</param>
    /// <param name="tabs">Collection of tab labels.</param>
    [When(@"I (can|cannot) see the tabs")]
    [Then(@"I (can|cannot) see the tabs")]
    public void ICanSeeTabs(string canOrCannot, Table tabs)
    {
        Driver.WaitForTransaction();
        canOrCannot = canOrCannot ?? throw new ArgumentNullException(nameof(canOrCannot));
        bool shouldBeVisible = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);
        var tabList = FormHelper.GetTabNames(FormHelper.GetFormContext(Driver));

        if (shouldBeVisible)
        {
            tabList.Should().BeEquivalentTo(tabs.Rows.Select(t => t["tabName"]).ToList());
        }
        else
        {
            tabList.Should().NotBeEquivalentTo(tabs.Rows.Select(t => t["tabName"]).ToList());
        }
    }

    /// <summary>
    /// Asserts of the subgrid is currently visible.
    /// </summary>
    /// <param name="canOrCannot">Whether the tab should be visible.</param>
    /// <param name="subGridName">Unique name of the subgrid.</param>
    [Then(@"I (can|cannot) see the '(.*)' subgrid")]
    public void ICanSeeSubgrid(string canOrCannot, string subGridName)
    {
        canOrCannot = canOrCannot ?? throw new ArgumentNullException(nameof(canOrCannot));
        bool shouldBeVisible = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);

        var subGrid = GridHelper.GetGrid(
            Driver,
            FormHelper.GetFormContext(Driver),
            subGridName
        );

        if (shouldBeVisible)
        {
            subGrid.Should().NotBeNull();
        }
        else
        {
            subGrid.Should().BeNull();
        }
    }

    /// <summary>
    /// Asserts the available options within a OptionSet control.
    /// </summary>
    /// <param name="fieldName">Attribute logical name.</param>
    /// <param name="operators">Operator.</param>
    /// <param name="table">List of option labels.</param>
    [Then(@"the '(.*)' optionset field (contains| does not contain)")]
    public void ThenTheOptionsetFieldContains(string fieldName, string operators, Table table)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        var options = ControlHelper.GetOptionSetValues(formContext, fieldName, Driver)
            .Select(o => o.Text)
            .ToArray();
        var expectedOptionLabels = table.Rows.Select(tableRow => tableRow["displayLabel"]).ToArray();

        if (operators == "contains")
        {
            options.Should().BeEquivalentTo(expectedOptionLabels);
        }
        else if (operators == "does not contain")
        {
            options.Should().NotBeEquivalentTo(expectedOptionLabels);
        }
    }
}
