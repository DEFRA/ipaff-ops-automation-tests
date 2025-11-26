// <copyright file="NavigationSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using System;
using Reqnroll;

/// <summary>
/// Steps relating to navigation of the Dynamics app.
/// </summary>
[Binding]
public class NavigationSteps : PowerAppsStepDefiner
{
    private SessionContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationSteps"/> class.
    /// </summary>
    /// <param name="ctx">SessionContext.</param>
    public NavigationSteps(SessionContext ctx)
    {
        this.ctx = ctx;
    }

    /// <summary>
    /// Asserts that an area of the sitemap is visible.
    /// </summary>
    /// <param name="areaName">The name of the area to assert.</param>
    [Then(@"I see an area called '(.*)'")]
    public static void ThenICanSeeTheArea(string areaName)
    {
        var element = Driver.FindElement(By.Id("areaSwitcherContainer"));
        element.Text.Should().Contain(areaName);
    }

    /// <summary>
    /// Asserts that a group of the sitemap is visible.
    /// </summary>
    /// <param name="areaName">The name of the group to assert.</param>
    [Then(@"I see a group called '(.*)'")]
    public static void ThenICanSeeTheGroup(string areaName)
    {
        var element = Driver.FindElement(By.XPath($@"//span[@data-id='sitemap-sitemapAreaGroup-{areaName.Replace(" ", string.Empty)}']"));
        element.Text.Should().Contain(areaName);
    }

    /// <summary>
    /// Asserts that a subarea of the sitemap is visible.
    /// </summary>
    /// <param name="areaName">The name of the subarea to assert.</param>
    [Then(@"I see a subarea called '(.*)'")]
    public static void ThenICanSeeTheSubArea(string areaName)
    {
        var element = Driver.FindElement(By.XPath($@"//img[@title='{areaName}']"));
        element.Text.Should().NotBeNull();
    }

    [Then(@"I can see the '(.*)' command for the following menu")]
    public void ThenICanSeeTheCommandForTheFollowingMenu(string commandName, Table table)
    {
        foreach (var row in table.Rows)
        {
            Capgemini.PowerApps.SpecFlowBindings.Steps.NavigationSteps.WhenIOpenTheSubAreaUnderTheArea(row["sub_area"], row["main_area"]);
            Capgemini.PowerApps.SpecFlowBindings.Steps.CommandBarSteps.ThenICanSeeTheCommand(commandName);
        }
    }

    [Then(@"I can't see the '(.*)' command for the following menu")]
    public void ThenICanNotSeeTheCommandForTheFollowingMenu(string commandName, Table table)
    {
        foreach (var row in table.Rows)
        {
            Capgemini.PowerApps.SpecFlowBindings.Steps.NavigationSteps.WhenIOpenTheSubAreaUnderTheArea(row["sub_area"], row["main_area"]);
            Capgemini.PowerApps.SpecFlowBindings.Steps.CommandBarSteps.ThenICanNotSeeTheCommand(commandName);
        }
    }

    /// <summary>
    /// Navigates the user to a record created via a single transaction. Falling back to the test driver method.
    /// </summary>
    /// <param name="applicationAlias">The alias of the export application.</param>
    /// <param name="entityLogicalName">The logical name of the entity.</param>
    [Given(@"I have opened the '(.*)' record of type '(.*)' created in a single transaction")]
    public void GivenIHaveOpenedTheRecordCreatedInASingleTransaction(string applicationAlias, string entityLogicalName)
    {
        // Attempt to obtain the reference via the SessionContext first, if there is no output value
        // then attempt to locate via the TestDriver.
        EntityReference application = null;
        if (!this.ctx.TryGetEntityReference(applicationAlias, out application))
        {
            application = TestDriver.GetTestRecordReference(applicationAlias);
        }

        if (application != null)
        {
            Driver.ExecuteScript($"Xrm.Navigation.openForm({{ entityId: \"{application.Id}\", entityName: \"{entityLogicalName}\" }})");
            Driver.WaitForTransaction();
        }
        else
        {
            throw new ArgumentException($"No application  exists with the alis {applicationAlias}");
        }
    }
}