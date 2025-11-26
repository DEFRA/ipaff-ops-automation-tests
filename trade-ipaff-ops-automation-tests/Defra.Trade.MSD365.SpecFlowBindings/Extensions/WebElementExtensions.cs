// <copyright file="WebElementExtensions.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace OpenQA.Selenium;

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

/// <summary>
/// Extension class for adding IWebElement functionality.
/// </summary>
public static class WebElementExtensions
{
    /// <summary>
    /// Inputs text into a given IWebElement using SendKeys.
    /// </summary>
    /// <param name="that">The element to be input into.</param>
    /// <param name="text">The text to input into the element.</param>
    /// <param name="shouldClear">Flag to indicate whether or not to clear the field before inputting the text.</param>
    public static void InputText(this IWebElement that, string text, bool shouldClear = true)
    {
        ValidateNotNull(that);
        if (shouldClear)
        {
            that.Clear();
        }

        that.SendKeys(text);
    }

    /// <summary>
    /// Scrolls the element into view.
    /// </summary>
    /// <param name="element">The element to scroll into view.</param>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    public static void ScrollIntoView(this IWebElement element, IWebDriver webDriver)
    {
        webDriver.ScrollIntoView(element);
    }

    /// <summary>
    /// Select all options by the text displayed.
    /// </summary>
    /// <param name="element">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="value">The text of the option to be selected.</param>
    public static void SelectByText(this IWebElement element, string value)
    {
        if (element.TagName != "select")
        {
            throw new ArgumentException($"Elements of type '{element.TagName}' are not supported");
        }

        var selectElement = new SelectElement(element);
        selectElement.SelectByText(value);
    }

    /// <summary>
    /// Gives focus to an element.
    /// </summary>
    /// <param name="element">The element to give focus.</param>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    public static void SetFocus(this IWebElement element, IWebDriver webDriver)
    {
        webDriver.SetFocus(element);
    }

    /// <summary>
    /// Clears the contents of an element.
    /// </summary>
    /// <param name="element">The element to clear.</param>
    /// <param name="webDriver">The web driver.</param>
    public static void Clear(this IWebElement element, IWebDriver webDriver)
    {
        if (element.TagName == "input")
        {
            if (element.HasAttribute("value") && element.GetAttribute("value").Length > 0)
            {
                element.SendKeys(Keys.Control + "a");
                element.SendKeys(Keys.Backspace);
            }
        }
        else if (element.TagName == "div")
        {
            if (element.HasElement(By.TagName("ul")))
            {
                var lookupItem = element.FindElement(By.XPath(".//ul"))
                    .FindElements(By.XPath(".//li"))
                    .FirstOrDefault();

                lookupItem.ScrollIntoView(webDriver);
                lookupItem.FindElement(By.XPath(".//button")).Click();
            }
        }

        webDriver.WaitForTransaction();
    }

    /// <summary>
    /// Scrolls the element content horizontally.
    /// </summary>
    /// <param name="element">The element to perform the action.</param>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="pixels">The number of pixels the element's content to scrolled horizontally.</param>
    /// <returns>The number of pixels the element's content is scrolled horizontally.</returns>
    public static int ScrollLeft(this IWebElement element, IWebDriver webDriver, int pixels = 1000)
    {
        var position = Convert.ToInt32(double.Parse(element.GetAttribute("scrollLeft")));
        ((IJavaScriptExecutor)webDriver).ExecuteScript($"arguments[0].scrollLeft = {position + pixels}", element);
        return Convert.ToInt32(double.Parse(element.GetAttribute("scrollLeft")));
    }

    /// <summary>
    /// Gets the width of the element.
    /// </summary>
    /// <param name="element">The element to perform the action.</param>
    /// <returns>The number of pixels.</returns>
    public static int GetWidth(this IWebElement element)
    {
        var cssValue = element.GetCssValue("width");
        return Convert.ToInt32(double.Parse(cssValue.Replace("px", string.Empty)));
    }

    /// <summary>
    /// Gets the height of the element.
    /// </summary>
    /// <param name="element">The element to perform the action.</param>
    /// <returns>The number of pixels.</returns>
    public static int GetHeight(this IWebElement element)
    {
        var cssValue = element.GetCssValue("height");
        return Convert.ToInt32(double.Parse(cssValue.Replace("px", string.Empty)));
    }

    /// <summary>
    /// Validates that a given IWebElement is not null.
    /// </summary>
    /// <param name="that">The IWebElement to evaluate.</param>
    private static void ValidateNotNull(IWebElement that)
    {
        if (that == null)
        {
            throw new ArgumentNullException(nameof(that), "WebElement is null.");
        }
    }
}
