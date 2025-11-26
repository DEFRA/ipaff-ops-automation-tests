namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using Reqnroll;

/// <summary>
/// Specflow binding steps for working with Quick View forms.
/// </summary>
[Binding]
public class QuickViewSteps : PowerAppsStepDefiner
{
    /// <summary>
    /// Checks if the provided fields are present on the specified quick view form.
    /// </summary>
    /// <param name="canOrCannot">Can or cannot.</param>
    /// <param name="quickViewName">The logical name of the quick view form.</param>
    /// <param name="fields">Table list of fields to check.</param>
    [Then("I (can|can not) see the following fields on the '(.*)' Quick View form")]
    public static void ICanSeeFields(string canOrCannot, string quickViewName, Table fields)
    {
        bool canSeeField = canOrCannot == "can" ? true : false;
        var formContext = FormHelper.GetFormContext(Driver);
        var quickFormContainer = formContext.FindElement(By.XPath($".//div[@data-id='{quickViewName}-QuickFormContainer']"));
        quickFormContainer.ScrollIntoView(Driver);

        foreach (DataTableRow row in fields.Rows)
        {
            var fieldName = $"{quickViewName}.{row["schemaName"]}";
            var field = ControlHelper.FindControl(quickFormContainer, fieldName);

            if (field != null && canSeeField)
            {
                field.Displayed.Should().BeTrue(because: $"Field {row["displayName"]} should be visible on the {quickViewName} Quick View form");
                var fieldText = ControlHelper.GetControlLabel(field, row["schemaName"]);
                fieldText.Should().Be(row["displayName"]);
            }
            else if (field != null && !canSeeField)
            {
                field.Displayed.Should().BeFalse(because: $"Field {row["displayName"]} should not be visible on the {quickViewName} Quick View form");
            }
        }
    }
}
