namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains a number of methods useful for getting the context of main and modal forms.
/// </summary>
public static class FormHelper
{
    /// <summary>
    /// Finds the desired form by its display name.
    /// </summary>
    /// <param name="webDriver">The web driver to use.</param>
    /// <returns> Returns the modal form on top of the stack, or the default edit form if no modal forms are found.</returns>
    public static IWebElement GetFormContext(IWebDriver webDriver)
    {
        webDriver.WaitForTransaction();

        webDriver.WaitUntilAvailable(By.XPath("//div[(contains(@aria-modal, 'true') and contains (@role, 'dialog')) or @data-id='GridRoot' or @data-id='editFormRoot']"), 10.Seconds());
        var modalForms = webDriver.FindElements(By.XPath("//div[contains(@aria-modal, 'true') and contains (@role, 'dialog')]"));

        if (modalForms.Count > 0)
        {
            return modalForms.Last();
        }
        else if (webDriver.TryFindElement(By.XPath("//div[@data-id='GridRoot']"), out var gridRoot))
        {
            return gridRoot;
        }
        else if (webDriver.TryFindElement(By.XPath("//div[@data-id='editFormRoot']"), out var editFormRoot))
        {
            return editFormRoot;
        }

        return null;
    }

    /// <summary>
    /// Gets the form context of a modal dialog.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="successCallback">Callback handler.</param>
    /// <param name="exceptionMessage">Friendly error message.</param>
    public static void WaitForModalContext(IWebDriver webDriver, Action<IWebElement> successCallback, string exceptionMessage)
    {
        webDriver.WaitUntilAvailable(By.XPath("//div[contains(@aria-modal, 'true') and contains (@role, 'dialog')]"));
        var modalForms = webDriver.FindElements(By.XPath("//div[contains(@aria-modal, 'true') and contains (@role, 'dialog')]"));
        if (modalForms.Count == 0)
        {
            throw new Exception(exceptionMessage);
        }

        successCallback(modalForms.Last());
    }

    /// <summary>
    /// Clears the specified field.
    /// </summary>
    /// <param name="webDriver">The web driver to use.</param>
    /// <param name="field">The field to clear.</param>
    public static void ClearFieldValue(IWebDriver webDriver, IWebElement field)
    {
        if (field.GetAttribute("value")?.Length > 0)
        {
            field.SendKeys(Keys.Control + "a");
            field.SendKeys(Keys.Backspace);
            webDriver.WaitForTransaction();
        }
    }

    /// <summary>
    /// Determines if the form is read-only.
    /// </summary>
    /// <param name="formContext">An IWebElement to search.</param>
    /// <returns>true if read-only otherwise false.</returns>
    public static bool IsReadOnly(IWebElement formContext)
    {
        return formContext.HasElement(By.Id("des-formReadOnlyNotification"));
    }

    /// <summary>
    /// Gets a collection of tabs shown on the form.
    /// </summary>
    /// <param name="formContext">Context of the form to perform the action.</param>
    /// <returns>Array of IWebElement.</returns>
    public static List<string> GetTabNames(IWebElement formContext)
    {
        formContext = formContext ?? throw new ArgumentNullException(nameof(formContext));

        var tabs = formContext
            .FindElement(By.XPath($".//ul[@role='tablist']"))
            .FindElements(By.TagName("li"))
            .ToArray();

        return tabs.Select(tab => tab.Text).ToList();
    }

    /// <summary>
    /// Finds a tab within the form.
    /// </summary>
    /// <param name="formContext">Context of the form to perform the action.</param>
    /// <param name="tabName">Name of tab.</param>
    /// <returns>IWebElement.</returns>
    public static IWebElement FindTab(IWebElement formContext, string tabName)
    {
        return formContext
            .FindElement(By.XPath($".//ul[@role='tablist']"))
            .FindElements(By.TagName("li"))
            .Where(e => e.Text == tabName)
            .First();
    }
}
