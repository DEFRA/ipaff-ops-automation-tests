namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

/// <summary>
/// Helper class to interact buttons, flyouts and split buttons.
/// </summary>
public class CommandHelper
{
    private const string FLYOUTLOADING = "Loading...";
    private const string MORECOMMANDS = "More Commands";

    /// <summary>
    /// Gets an instance of the top-level flyout command for the given name.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="flyoutName">Unique name of the flyout command.</param>
    /// <returns>an IWebElement.</returns>
    public static IWebElement GetFlyoutCommand(IWebDriver webDriver, string flyoutName)
    {
        var flyoutCommand = webDriver.WaitUntilAvailable(By.XPath($"//button[(@aria-label='{flyoutName}' or starts-with(@aria-label, '{flyoutName}.'))]"));
        if (flyoutCommand == null)
        {
            throw new NoSuchElementException($"Could not find flyout command within name '{flyoutName}'.");
        }

        return flyoutCommand;
    }

    /// <summary>
    /// Finds the command items within a flyout command.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="commandButton">An instance of the flyout command button.</param>
    /// <returns>a collection of IWebElements.</returns>
    public static Dictionary<string, IWebElement> GetFlyoutCommands(IWebDriver webDriver, IWebElement commandButton)
    {
        Dictionary<string, IWebElement> flyoutCommands = null;
        var wait = new WebDriverWait(webDriver, new TimeSpan(0, 0, 15));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

        webDriver.MoveToElement(commandButton);
        wait.Until(d =>
        {
            if (!d.HasElement(By.Id("__flyoutRootNode")))
            {
                commandButton.Click();
                d.WaitForTransaction();
            }

            var flyout = d.FindElement(By.Id("__flyoutRootNode"));
            d.MoveToElement(flyout);
            if (flyout.IsVisible() && flyout.IsClickable())
            {
                flyoutCommands = flyout
                    .FindElements(By.TagName("button"))
                    .Where(e => e.Displayed)
                    .ToDictionary(e => e.Text, e => e);

                // Returns false if we get the "Loading..." element, which will allow this logic to rerun and pick out the correct elements
                return !flyoutCommands.ContainsKey(FLYOUTLOADING);
            }

            // Returning true means we have to wait
            return true;
        });

        return flyoutCommands;
    }

    /// <summary>
    /// Geta a collection of sub command labels of a flyout commond.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions built will be performed.</param>
    /// <param name="flyoutName">Unique name of the flyout command.</param>
    /// <returns>an array of command labels.</returns>
    public static string[] GetFlyoutCommandLabels(IWebDriver webDriver, string flyoutName)
    {
        var flyoutCommand = GetFlyoutCommand(webDriver, flyoutName);
        return GetFlyoutCommandLabels(webDriver, flyoutCommand);
    }

    public static string[] GetFlyoutCommandLabels(IWebDriver webDriver, IWebElement flyoutCommand)
    {
        var subCommands = GetFlyoutCommands(webDriver, flyoutCommand);
        return subCommands.Select(e => e.Key).ToArray();
    }

    /// <summary>
    /// Get a collection of top level command bar items.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions will be performed.</param>
    /// <param name="formContext">The IWebElement to evaluate.</param>
    /// <returns>The collection of commands.</returns>
    public static Dictionary<string, IWebElement> GetCommands(IWebDriver webDriver, IWebElement formContext)
    {
        webDriver.WaitForTransaction();

        var commandBar = formContext.WaitUntilAvailable(By.XPath(".//ul[@data-id='CommandBar']"));
        var listItems = commandBar.FindElements(By.XPath(".//li"));

        var commandButtons = new Dictionary<string, IWebElement>(listItems.Count);
        try
        {
            foreach (var listItem in listItems)
            {
                var commandButton = listItem.FindElement(By.XPath(".//button[@role='menuitem']"));
                var commandLabel = (!string.IsNullOrEmpty(commandButton.Text)) ? commandButton.Text : "More Commands";

                if (!commandButtons.ContainsKey(commandLabel))
                {
                    commandButtons.Add(commandLabel, commandButton);
                }
            }
        }
        catch
        {

        }

        return commandButtons;
    }

    /// <summary>
    /// Clicks the specified command.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions will be performed.</param>
    /// <param name="formContext">The IWebElement to evaluate.</param>
    /// <param name="commandName">Friendly name of the command.</param>
    public static void ClickCommand(IWebDriver webDriver, IWebElement formContext, string commandName)
    {
        var commandButtons = GetCommands(webDriver, formContext);
        var commandButton = commandButtons.FirstOrDefault(d => d.Key == commandName).Value;

        if (commandButton != null)
        {
            commandButton.Click();
        }
        else if (commandButton == null && commandButtons.ContainsKey(MORECOMMANDS))
        {
            ClickCommand(webDriver, formContext, MORECOMMANDS, commandName);
        }

        webDriver.WaitForTransaction();
    }

    /// <summary>
    /// Click the specified flyout sub-command.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions will be performed.</param>
    /// <param name="formContext">The IWebElement to evaluate.</param>
    /// <param name="flyoutName">Friendly name of the flyout command.</param>
    /// <param name="commandName">Friendly name of the sub-command.</param>
    public static void ClickCommand(IWebDriver webDriver, IWebElement formContext, string flyoutName, string commandName)
    {
        var commandButtons = GetCommands(webDriver, formContext);
        //var commandButtons = GetFlyoutCommands(webDriver, formContext);
        var subCommands = ClickCommand(webDriver, flyoutName, commandButtons);

        if (subCommands.ContainsKey(FLYOUTLOADING))
        {
            ClickCommand(webDriver, formContext, "Refresh");
            ClickCommand(webDriver, FormHelper.GetFormContext(webDriver), flyoutName, commandName);
        }
        else
        {
            var subCommand = subCommands.First(d => d.Key == commandName).Value;
            subCommand.Click();
            webDriver.WaitForTransaction();
        }
    }

    /// <summary>
    /// Click the specified flyout sub-command.
    /// </summary>
    /// <param name="webDriver">The IWebDriver instance on which the actions will be performed.</param>
    /// <param name="flyoutName">Friendly name of the flyout command.</param>
    /// <param name="commandButtons">Friendly name of the sub-command.</param>
    /// <returns>The collection of sub-commands.</returns>
    private static Dictionary<string, IWebElement> ClickCommand(IWebDriver webDriver, string flyoutName, Dictionary<string, IWebElement> commandButtons)
    {
        if (commandButtons.ContainsKey(flyoutName))
        {
            var commandButton = commandButtons.First(d => d.Key == flyoutName).Value;
            return GetFlyoutCommands(webDriver, commandButton);
        }

        commandButtons = ClickCommand(webDriver, MORECOMMANDS, commandButtons);
        return ClickCommand(webDriver, flyoutName, commandButtons);
    }

    /// <summary>
    /// Determines if the element is a flyout command.
    /// </summary>
    /// <param name="element">The IWebElement to evaluate.</param>
    /// <returns>returns true if the button is a flyout otherwise false.</returns>
    private static bool IsFlyout(IWebElement element)
    {
        return element.HasAttribute("data-expanded") || element.HasAttribute("aria-expanded");
    }
}
