namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Polly;
using System;
using System.Linq;
using Field = Microsoft.Dynamics365.UIAutomation.Api.UCI.Field;

/// <summary>
/// Helper class to find and work with controls on a page.
/// </summary>
public class ControlHelper
{
    /// <summary>
    /// Finds the element for the control from the provided form context.
    /// </summary>
    /// <param name="formContext">An IWebElement instance to be searched.</param>
    /// <param name="fieldName">The schema name of the control to find.</param>
    /// <param name="successCallback">Callback handler.</param>
    /// <param name="exceptionMessage">Friendly error message.</param>
    /// <param name="fieldType">Optional: the field type to get the control for. Possible values include numeric and optionset.</param>
    public static void FindControl(
        IWebElement formContext,
        string fieldName,
        Action<IWebElement> successCallback,
        string exceptionMessage,
        string fieldType = "")
    {
        var control = FindControl(formContext, fieldName, fieldType);
        if (control == null)
        {
            throw new Exception(exceptionMessage);
        }

        successCallback(control);
    }

    /// <summary>
    /// Finds the element for the control from the provided form context.
    /// </summary>
    /// <param name="formContext">An IWebElement instance to be searched.</param>
    /// <param name="fieldName">The schema name of the control to find.</param>
    /// <param name="fieldType">Optional: the field type to get the control for.</param>
    /// <returns>IWebElement control for the desired field.</returns>
    public static IWebElement FindControl(IWebElement formContext, string fieldName, string fieldType = "")
    {
        IWebElement element = null;
        Policy
            .Handle<NoSuchElementException>()
            .WaitAndRetry(new[]
            {
                3.Seconds(),
                6.Seconds(),
                9.Seconds(),
            }).Execute(() =>
            {
                switch (fieldType)
                {
                    case "numeric":
                        {
                            element = formContext.FindElement(By.XPath($".//input[@data-id='{fieldName}.fieldControl-whole-number-text-input']"));

                            if (element == null)
                            {
                                element = formContext.FindElement(By.XPath($".//input[@data-id='{fieldName}.fieldControl-decimal-number-text-input']"));
                            }

                            break;
                        }

                    case "optionset":
                        {
                            element = formContext.FindElement(By.XPath($".//select[@data-id='{fieldName}.fieldControl-option-set-select']"));
                            break;
                        }

                    case "text":
                        {
                            element = formContext.FindElement(By.XPath($".//input[@data-id='{fieldName}.fieldControl-text-box-text']"));
                            break;
                        }

                    default:
                        {
                            element = formContext.FindElement(By.XPath($".//div[@data-id='{fieldName}-FieldSectionItemContainer']"));
                            break;
                        }
                }
            });

        return element;
    }

    /// <summary>
    /// Finds the element for the control from the provided form context.
    /// </summary>
    /// <param name="formContext">An IWebElement instance to be searched.</param>
    /// <param name="fieldName">The schema name of the control to find.</param>
    /// <param name="location">Field or header.</param>
    /// <param name="webDriver">The web driver.</param>
    /// <returns>IWebElement control for the desired field</returns>
    public static IWebElement FindControl(IWebElement formContext, string fieldName, string location, IWebDriver webDriver)
    {
        if (location == "header field")
        {
            var formHeader = formContext.FindElement(By.XPath(".//div[@data-id = 'form-header']"));
            return FindHeaderControl(fieldName, formHeader, webDriver);
        }

        return FindControl(formContext, fieldName);
    }

    /// <summary>
    /// Searches the header of the IWebElement for the specified control.
    /// </summary>
    /// <param name="formContext">An IWebElement instance to be searched.</param>
    /// <param name="fieldName">The schema name of the control to find.</param>
    /// <param name="expectedFieldValue">The value expected in the field - this helps locate the field.</param>
    /// <returns>Whether the header field matches what is expected.</returns>
    public static bool HeaderFieldHasValue(IWebElement formContext, string fieldName, string expectedFieldValue)
    {
        IWebElement headerFieldsExpand = formContext.FindElement(By.XPath(".//button[@data-id='header_overflowButton']"));
        headerFieldsExpand.Click();

        // Headers exist outside of the form context so we need to search the entire page.
        var header = formContext.FindElement(By.XPath($"//div[@data-id='header_{fieldName}-FieldSectionItemContainer']"));

        // We need to find the child control that contains the actual values we care about. If we returned just header, the .text value would include the field dispaly name.
        bool result = header.FindElement(By.XPath($".//div[@title='{expectedFieldValue}']")).Text == expectedFieldValue;
        headerFieldsExpand.Click();

        return result;
    }

    /// <summary>
    /// Locates a field on the eheader.
    /// </summary>
    /// <param name="fieldName">Header field name.</param>
    /// <param name="formHeader">Header context.</param>
    /// <param name="webDriver">Web driver.</param>
    /// <returns>The IWebElement for the desired field.</returns>
    private static IWebElement FindHeaderControl(string fieldName, IWebElement formHeader, IWebDriver webDriver)
    {
        IWebElement headerControl = null;
        var wait = new WebDriverWait(webDriver, new TimeSpan(0, 0, 10));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

        wait.Until(d =>
        {
            var overflowButton = formHeader.FindElement(By.XPath(".//button[@data-id = 'header_overflowButton']"));
            overflowButton.Click();
            d.WaitForTransaction();

            var controlElements = formHeader.FindElements(By.XPath($"//*[@data-id = 'header_{fieldName}']"));

            if (controlElements.Count == 1)
            {
                headerControl = controlElements.Single();
                return true;
            }

            var flyout = d.FindElement(By.Id("__flyoutRootNode"));
            d.MoveToElement(flyout);

            if (flyout.IsVisible() && flyout.IsClickable())
            {
                headerControl = flyout.FindElement(By.XPath($".//div[@data-id = 'header_{fieldName}-FieldSectionItemContainer']"));

                // Wait.Until will end when we return true.
                return true;
            }

            // Returning false means we have to wait
            return false;
        });
        return headerControl;
    }

    /// <summary>
    /// Gets a value indicating whether the IWebElement is read-only.
    /// </summary>
    /// <param name="control">An IWebElement instance to be evaluated.</param>
    /// <returns>true if is read-only; otherwise, false.</returns>
    public static bool IsReadOnly(IWebElement control)
    {
        var dataId = control.GetAttribute("data-id");
        var fieldName = dataId.Substring(0, dataId.IndexOf("-"));

        if (control.HasElement(By.TagName("input")))
        {
            var input = control.FindElement(By.TagName("input"));
            return input.HasAttribute("disabled") || input.HasAttribute("readonly");
        }
        else if (control.HasElement(By.TagName("select")))
        {
            var select = control.FindElement(By.TagName("select"));
            return select.HasAttribute("disabled");
        }
        else if (control.HasElement(By.TagName("textarea")))
        {
            var textarea = control.FindElement(By.TagName("textarea"));
            return textarea.HasAttribute("disbaled");
        }
        else if (control.HasElement(By.XPath($".//div[contains(@data-id, '{fieldName}.fieldControl-toggle-container')]")))
        {
            return control.HasElement(By.XPath(".//button[@aria-disabled='true']"));
        }
        else
        {
            var lookupRecordList = control.FindElement(By.XPath($".//div[contains(@data-id, '{fieldName}.fieldControl-Lookup_{fieldName}')]"));
            var lookupDescription = lookupRecordList.FindElement(By.XPath(".//div[@role='text']"));

            if (lookupDescription != null)
            {
                return lookupDescription.GetAttribute("innerText").ToLowerInvariant().Contains("readonly", StringComparison.OrdinalIgnoreCase);
            }
        }

        return false;
    }

    /// <summary>
    /// Gets a control field.
    /// </summary>
    /// <param name="formContext">The IWebElement context.</param>
    /// <param name="fieldName">The name of the field to get.</param>
    /// <param name="fieldType">The type of field.</param>
    /// <param name="location">The location of the field.</param>
    /// <param name="webDriver">The web driver.</param>
    /// <returns>A field.</returns>
    public static Field GetControlField(IWebElement formContext, string fieldName, string fieldType, string location, IWebDriver webDriver)
    {
        var control = FindControl(formContext, fieldName, location, webDriver);
        var field = new Field() { Id = fieldName, Name = GetControlLabel(control, fieldName) };
       
        switch (fieldType)
        {
            case "lookup":
                field.Value = GetLookupValue(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            case "optionset":
                field.Value = GetOptionSetValue(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            case "buttonset":
                field.Value = GetButtonSetValue(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            case "statecode":
                field.Value = GetStateCode(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            case "boolean":
                field.Value = GetBooleanValue(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            case "numeric":
                field.Value = GetNumericValue(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            case "text":
                field.Value = GetTextValue(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            case "input":
                field.Value = GetTextInputValue(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            case "datetime":
                field.Value = GetDateValue(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            case "inputdatetime":
                field.Value = GetDateInputValue(control, location == "header field" ? $"header_{fieldName}" : fieldName);
                break;
            default:
                return null;
        }

        return field;
    }

    /// <summary>
    /// Sets the value of a string field.
    /// </summary>
    /// <param name="fieldValue">The string value to set the field.</param>
    /// <param name="control">The schema name of the field to retrieve.</param>
    public static void SetValue(string fieldValue, IWebElement control)
    {
        if (control == null)
        {
            throw new ArgumentNullException(nameof(control), "control is invalid.");
        }

        control.SendKeys(fieldValue.ToString());
    }

    /// <summary>
    /// Sets the value of a decimal field.
    /// </summary>
    /// <param name="fieldValue">The decimal value to set the field.</param>
    /// <param name="control">The schema name of the field to retrieve.</param>
    public static void SetValue(decimal fieldValue, IWebElement control)
    {
        if (control == null)
        {
            throw new ArgumentNullException(nameof(control), "control is invalid.");
        }

        SetValue(fieldValue.ToString(), control);
    }

    /// <summary>
    /// Sets the value of a boolean field.
    /// </summary>
    /// <param name="fieldValue">The boolean value to set the field.</param>
    /// <param name="control">The schema name of the field to retrieve.</param>
    public static void SetValue(bool fieldValue, IWebElement control)
    {
        if (control == null)
        {
            throw new ArgumentNullException(nameof(control), "control is invalid.");
        }

        var isChecked = bool.Parse(control.GetAttribute("aria-checked"));
        if (isChecked != fieldValue)
        {
            control.Click();
        }
    }

    /// <summary>
    /// Sets the value of a lookup field.
    /// </summary>
    /// <param name="lookupItem">A lookup item</param>
    /// <param name="control">>An IWebElement of control to set</param>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    public static void SetValue(Microsoft.Dynamics365.UIAutomation.Api.UCI.LookupItem lookupItem, IWebElement control, IWebDriver webDriver)
    {
        if (control == null)
        {
            throw new ArgumentNullException(nameof(control), "control is invalid.");
        }

        // Input the value to the field. We only need to do this once.
        control.SetFocus(webDriver);
        var input = control.FindElement(By.TagName("input"));
        input.SendKeys(lookupItem.Value);

        // Wait for finding the lookup selection elements.
        var wait = new WebDriverWait(webDriver, new TimeSpan(0, 0, 5));
        wait.IgnoreExceptionTypes(new[] { typeof(NoSuchElementException), typeof(InvalidOperationException), typeof(StaleElementReferenceException) });

        wait.Until(d =>
        {
            var lookupResultsPanel = d.WaitUntilVisible(By.XPath($".//div[@data-id = '{lookupItem.Name}.fieldControl-LookupResultsDropdown_{lookupItem.Name}_children']"));
            d.WaitForTransaction();

            if (lookupResultsPanel.IsVisible() && lookupResultsPanel.IsClickable())
            {
                var unorderedList = lookupResultsPanel.WaitUntilVisible(By.XPath(".//ul"));
                var listItems = unorderedList.FindElements(By.XPath(".//li"));

                if (listItems.Any())
                {
                    var itemIndex = listItems.Select((element, index) => new { element, index })
                        .First(i => i.element.Text == lookupItem.Value).index;
                    listItems[itemIndex].Click();
                    d.WaitForTransaction();
                    return true;
                }
            }

            return false;
        });
    }

    /// <summary>
    /// Gets the label for a given control.
    /// </summary>
    /// <param name="formContext">An IWebElement instance to be searched.</param>
    /// <param name="fieldName">The schema name of the control to find.</param>
    /// <param name="expectedFieldValue">The value expected in the field - this helps locate the field.</param>
    /// <returns>Whether the header field matches what is expected.</returns>
    public static IWebElement GetButtonControl(IWebElement formContext, string fieldName)
    {
        var control = FindControl(formContext, fieldName);
        return control.FindElement(By.XPath(".//button"));
    }

    /// <summary>
    /// Gets the label for a given control.
    /// </summary>
    /// <param name="element">IWebElement control.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns></returns>
    public static string GetControlLabel(IWebElement element, string fieldName)
    {
        var labelText = string.Empty;
        var labels = element.FindElements(By.XPath(".//label"));

        foreach (var label in labels)
        {
            var id = label.GetAttribute("id");
            if (id.Contains($"{fieldName}-field-label"))
            {
                labelText = label.Text;
                break;
            }
        }

        return labelText;
    }

    /// <summary>
    /// Gets the available options within a select element.
    /// </summary>
    /// <param name="formContext">An IWebElement instance to be searched.</param>
    /// <param name="fieldName">Attribute logical name.</param>
    /// <param name="webDriver">The web driver.</param>
    /// <returns>An array of option elements.</returns>
    public static IWebElement[] GetOptionSetValues(IWebElement formContext, string fieldName, IWebDriver webDriver)
    {
        var optionSet = FindControl(formContext, fieldName, "optionset");
        if (optionSet == null)
        {
            throw new NotFoundException($"An optionset control with the name {fieldName} could not be found.");
        }

        return optionSet.FindElements(By.TagName("option"))
            .Where(o => o.Text != "---")
            .ToArray();
    }

    private static string GetLookupValue(IWebElement element, string fieldName)
    {
        var lookup = element.FindElement(By.XPath($".//div[@data-id = '{fieldName}.fieldControl-LookupResultsDropdown_{fieldName.Replace("header_", string.Empty)}_selected_tag_text']"));
        return lookup.Text;
    }

    private static string GetOptionSetValue(IWebElement element, string fieldName)
    {
        var optionset = new SelectElement(element.FindElement(By.XPath($".//select[@data-id = '{fieldName}.fieldControl-option-set-select']")));
        return optionset.SelectedOption.Text;
    }

    private static string GetButtonSetValue(IWebElement element, string fieldName)
    {
        var buttonset = element.FindElement(By.XPath($".//button[@data-id = '{fieldName}.fieldControl-option-set-select']"));
        return buttonset.Text;
    }

    private static string GetStateCode(IWebElement element, string fieldName)
    {
        var stateCode = element.FindElement(By.XPath($".//div[@data-id = '{fieldName}.fieldControl-pickliststatus-comboBox']"));
        return stateCode.Text;
    }

    private static string GetBooleanValue(IWebElement element, string fieldName)
    {
        if (element.HasElement(By.TagName("select")))
        {
            var select = new SelectElement(element.FindElement(By.XPath($".//select[@data-id = '{fieldName}.fieldControl-checkbox-select']")));
            return select.SelectedOption.Text;
        }
        else if (element.HasElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-checkbox-toggle']")))
        {
            var checkBox = element.FindElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-checkbox-toggle']"));
            return checkBox.Selected.ToString();
        }
        else if (element.HasElement(By.XPath($".//div[contains(@data-id, '{fieldName}.fieldControl-toggle-container')]")))
        {
            var toggle = element.FindElement(By.XPath($".//div[contains(@data-id, '{fieldName}.fieldControl-toggle-container')]"));
            return toggle.Text;
        }

        return string.Empty;
    }

    private static string GetNumericValue(IWebElement element, string fieldName)
    {
        IWebElement value = null;

        if (element.HasElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-decimal-number-text-input']")))
        {
            value = element.FindElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-decimal-number-text-input']"));
        }
        else if (element.HasElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-whole-number-text-input']")))
        {
            value = element.FindElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-whole-number-text-input']"));
        }

        return (value != null) ? value.GetAttribute("value") : string.Empty;
    }

    private static string GetTextValue(IWebElement element, string fieldName)
    {
        IWebElement input = null;

        if (element.HasElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-text-box-text']")))
        {
            input = element.FindElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-text-box-text']"));
        }
        else if (element.HasElement(By.XPath($".//textarea[@data-id = '{fieldName}.fieldControl-text-box-text']")))
        {
            input = element.FindElement(By.XPath($".//textarea[@data-id = '{fieldName}.fieldControl-text-box-text']"));
        }

        return (input != null) ? input.GetAttribute("value") : string.Empty;
    }

    private static string GetTextInputValue(IWebElement element, string fieldName)
    {
        IWebElement input = null;

        if (element.HasElement(By.XPath($".//div[@data-id = '{fieldName}.fieldControl-pcf-container-id']//input")))
        {
            input = element.FindElement(By.XPath($".//div[@data-id = '{fieldName}.fieldControl-pcf-container-id']//input"));
        }
        else if (element.HasElement(By.XPath($".//div[@data-id = '{fieldName}.fieldControl-pcf-container-id']//textarea")))
        {
            input = element.FindElement(By.XPath($".//div[@data-id = '{fieldName}.fieldControl-pcf-container-id']//textarea"));
        }

        return (input != null) ? input.GetAttribute("value") : string.Empty;
    }

    private static string GetDateValue(IWebElement element, string fieldName)
    {
        if(element.HasElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-date-time-input']")))
            return element.FindElement(By.XPath($".//input[@data-id = '{fieldName}.fieldControl-date-time-input']")).GetAttribute("value");
        return null;
    }

    private static string GetDateInputValue(IWebElement element, string fieldName)
    {
        if (element.HasElement(By.XPath($".//div[@data-id = '{fieldName}.fieldControl._datecontrol-date-container']//input")))
            return element.FindElement(By.XPath($".//div[@data-id = '{fieldName}.fieldControl._datecontrol-date-container']//input")).GetAttribute("value");
        return null;
    }
}
