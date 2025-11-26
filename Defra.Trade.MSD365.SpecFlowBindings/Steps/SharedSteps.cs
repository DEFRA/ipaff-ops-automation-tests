// <copyright file="SharedSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Linq;
using Reqnroll;

/// <summary>
/// Steps for generic Dynamics functions used across the solution.
/// </summary>
[Binding]
public class SharedSteps : PowerAppsStepDefiner
{
    private readonly SessionContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="SharedSteps"/> class.
    /// </summary>
    /// <param name="context">Context of the test session.</param>
    public SharedSteps(SessionContext context)
    {
        this.ctx = context;
    }

    [When(@"I wait for (.*) seconds")]
    public static void WaitSeconds(int seconds)
    {
        XrmApp.ThinkTime(seconds * 1000);
    }

    [When(@"I click away")]
    public static void ClickAway()
    {
        Driver.FindElement(By.XPath("//html")).Click();
    }

    [Then(@"there is (an|no) error indicator displayed")]
    public static void VerifyErrorIndicator(string present)
    {
        var containsError = Driver.FindElements(By.ClassName("cc-grid-errorIndicator")).Where(x => x.Displayed).Any();
        if (present == "no")
        {
            containsError.Should().BeFalse();
        }
        else
        {
            containsError.Should().BeTrue();
        }
    }

    [When("I wait for the browser to finish processing events")]
    public static void WaitForScriptProcessing()
    {
        try
        {
            Driver.WaitForTransaction();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    // TODO: review if this is being used, if so it should probably be capturing something like 'today' rather than '11-11-1111' otherwise should be removed.
    [StepArgumentTransformation(@"11-11-1111")]
    public static DateTime Today()
    {
        return DateTime.Today;
    }

    // TODO: Remove. Scenarios shouldn't describe the test implementation.
    [When(@"I store the current record id as '(.*)' for entity '(.*)'")]
    public void WhenIStoreTheCurrentRecordIdAs(string variableName, string entityName)
    {
        var currentEntityName = XrmApp.Entity.GetEntityName();
        currentEntityName.Should().Be(entityName);
        this.ctx.SetVariable(variableName, XrmApp.Entity.GetObjectId());
        this.ctx.SetVariable(variableName + "EntityName", currentEntityName);
    }

    // TODO: Remove. Scenarios shouldn't describe the test implementation.
    [When("I create a unique value prefixed '(.*)' in a variable called '(.*)'")]
    public void CreateUniqueValue(string prefix, string variable)
    {
        this.ctx.SetVariable(variable, prefix + Guid.NewGuid());
    }

    // TODO: Remove. Scenarios shouldn't describe the test implementation.
    [Then(@"The stored record id '(.*)' matches with the current record id")]
    public void ThenTheStoredRecordIdMatchesWithTheCurrentRecordId(string variableName)
    {
        this.ctx.GetVariable<Guid>(variableName).Should().Be(XrmApp.Entity.GetObjectId());
    }
}