// <copyright file="WebDrivertExtensions.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace OpenQA.Selenium;

using OpenQA.Selenium.Interactions;

/// <summary>
/// Extension class for adding IWebDriver functionality.
/// </summary>
public static class WebDriverExtensions
{
    /// <summary>
    /// Scrolls the element into view.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="element">The element to scroll into view.</param>
    public static void ScrollIntoView(this IWebDriver webDriver, IWebElement element)
    {
        var scrollAction = new Actions(webDriver);
        scrollAction.MoveToElement(element).Perform();
    }

    /// <summary>
    /// Moves mouse to middle of the element.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="element">The element the move the mouse cursor to.</param>
    public static void MoveToElement(this IWebDriver webDriver, IWebElement element)
    {
        ScrollIntoView(webDriver, element);
    }

    /// <summary>
    /// Gives focus to an element.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="element">The element to give focus.</param>
    public static void SetFocus(this IWebDriver webDriver, IWebElement element)
    {
        var setFocusAction = new Actions(webDriver);
        setFocusAction.MoveToElement(element).Click().Perform();
    }

    /// <summary>
    /// Switches the driver to send commands to the SSRS report window.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="windowHandleIndex">The index of the window handle.</param>
    public static void SwitchToReportContext(this IWebDriver webDriver, int windowHandleIndex)
    {
        webDriver.SwitchTo().Window(webDriver.WindowHandles[windowHandleIndex]);
        var iframe = webDriver.FindElement(By.TagName("iframe"));
        var iframeId = iframe.GetAttribute("id");
        webDriver.SwitchTo().Frame(iframeId);
    }
}
