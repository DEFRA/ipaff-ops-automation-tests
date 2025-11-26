// <copyright file="CommandSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Polly;
using System;
using System.Linq;
using Reqnroll;

/// <summary>
/// Steps for interacting with button ribbons in Dynamics.
/// </summary>
[Binding]
public class CommandSteps : PowerAppsStepDefiner
{
    private readonly SessionContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSteps"/> class.
    /// </summary>
    /// <param name="ctx">Context of the test session.</param>
    public CommandSteps(SessionContext ctx)
    {
        this.ctx = ctx;
    }

    [Given("I select the '(.*)' command")]
    [When("I select the '(.*)' command")]
    [Given("I select the '(.*)' command that may be hidden")]
    [When("I select the '(.*)' command that may be hidden")]
    public static void WhenISelectTheCommand(string commandName)
    {
        var context = FormHelper.GetFormContext(Driver);
        CommandHelper.ClickCommand(Driver, context, commandName);
    }

    [When("I select the '(.*)' command for the '(.*)' entity that may be hidden")]
    public static void WhenISelectTheCommand(string commandName, string entity)
    {
        Driver.WaitForTransaction();

        try
        {
            if (ClickCommandButton(commandName, entity))
            {
                return; // done
            }
        }
        catch
        {
        }

        // OK - look in overflow menus instead
        Policy
            .Handle<NullReferenceException>()
            .Or<InvalidOperationException>()
            .Or<ElementNotInteractableException>()
            .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(2))
            .Execute(() =>
            {
                try
                {
                    // There may be multiple overflow contexts on the page, and we can specify which one to open
                    // First click the overflow button
                    if (entity != null)
                    {
                        Driver.FindElements(By.XPath($@"//button[@data-id='OverflowButton' and starts-with(@aria-label, 'More commands') and contains(@data-lp-id, '{entity}')]")).FirstOrDefault()?.Click();
                    }
                    else
                    {
                        Driver.FindElements(By.XPath(@"//button[@data-id='OverflowButton']")).FirstOrDefault()?.Click();
                    }
                }
                catch (ElementClickInterceptedException)
                {
                }

                if (!ClickCommandButton(commandName, entity, 0))
                {
                    Driver.WaitUntilClickable(By.XPath($"//button[(@aria-label='{commandName}' or starts-with(@aria-label, '{commandName}.'))]")).Click();
                }

                Driver.WaitForTransaction();
            });
    }

    [When(@"I go back")]
    public static void WhenIgoBack()
    {
        Driver.WaitUntilAvailable(By.Id("navigateBackButtontab-id-0")).Click();
        Driver.WaitForTransaction();
    }

    [Scope(Tag = "Trade")]
    [When(@"I select the '(.*)' command")]
    public static void ClickCommand(string commandName)
    {
        CommandHelper.ClickCommand(Driver, FormHelper.GetFormContext(Driver), commandName);
    }

    private static bool ClickCommandButton(string commandName, string entity, int startAtStep = 0)
    {
        bool done = false;
        IWebElement element = null;

        for (int method = startAtStep; method < 2 && !done; method++)
        {
            try
            {
                switch (method)
                {
                    case 0: // top level command-bar
                        if (entity == null)
                        {
                            CommandBarSteps.WhenISelectTheCommand(commandName);
                            done = true;
                        }
                        else
                        {
                            var buttons = Driver.FindElements(By.XPath($"//button[(@aria-label='{commandName}' or starts-with(@aria-label, '{commandName}.')) and starts-with(@data-id, '{entity}|')]"));

                            if (buttons.Any())
                            {
                                buttons.Last().Click();
                                done = true;
                            }
                        }

                        break;

                    case 1: // modal ribbon
                        if (entity != null)
                        {
                            element = Driver.FindElements(By.XPath($@"//button[(@aria-label='{commandName}' or starts-with(@aria-label, '{commandName}.')) and starts-with(@data-id,'{entity}|') and contains(@data-id,'|Form|')]")).FirstOrDefault();

                            if (element == null)
                            {
                                element = Driver.FindElements(By.XPath($@"//button[(@aria-label='{commandName}' or starts-with(@aria-label, '{commandName}.')) and starts-with(@data-id,'{entity}|')]")).FirstOrDefault();
                            }
                        }
                        else
                        {
                            element = Driver.FindElements(By.XPath($@"//button[(@aria-label='{commandName}' or starts-with(@aria-label, '{commandName}.'))]")).FirstOrDefault() ?? Driver.FindElements(By.XPath($@"//button[@title='{commandName}']")).FirstOrDefault();
                        }

                        if (element != null)
                        {
                            element.Click();
                            done = true;
                        }

                        break;
                }
            }
            catch
            {
                // method not successful, try another
            }
        }

        Driver.WaitForTransaction();
        return done;
    }

    [Then(@"I (can|cannot) see the flyout '(.*)' command")]
    [Then(@"I (can|cannot) see the '(.*)' button")]
    public void ThenICanSeeTheFlyyoutCommand(string canOrCannot, string commandName)
    {
        Driver.WaitForTransaction();
        var isVisible = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);
        var commands = XrmApp.CommandBar.GetCommandValues(true).Value;
        commands.Should().NotBeEmpty();

        if (isVisible)
        {
            commands.Should().Contain(commandName);
        }
        else
        {
            commands.Should().NotContain(commandName);
        }
    }

    [Then(@"I can see the following sub commands in the '(.*)' flyout")]
    public void ThenICanSeeTheFollowingSubCommandsInTheFlyout(string commandName, Table table)
    {
        var menuItems = CommandHelper.GetFlyoutCommandLabels(Driver, commandName);
        menuItems.Should().NotBeEmpty();
        menuItems.Should().HaveCountGreaterOrEqualTo(table.Rows.Count);

        foreach (var item in table.Rows)
        {
            menuItems.Should().Contain(item["subCommand"]);
        }
    }
}
