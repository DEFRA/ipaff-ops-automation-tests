// <copyright file="BusinessProcessFlowSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using Reqnroll;

/// <summary>
/// Steps for interacting with the business process flow component of Dynamics.
/// </summary>
[Binding]
public class BusinessProcessFlowSteps : PowerAppsStepDefiner
{
    /// <summary>
    /// Temporary replacement for EasyRepro step as it is not working.
    /// </summary>
    /// <param name="expectedValue">Expected content of option set field.</param>
    /// <param name="field">Field to evaluate against.</param>
    [Then("I can see a value of '(.*)' in the '(.*)' optionset field on the business process flow using XPath")]
    public static void ThenICanSeeAValueOfInTheOptionSetFieldOnTheProcessFlow(string expectedValue, string field)
    {
        var element = Driver.FindElement(By.XPath(".//select[contains(@data-id, 'header_process_" + field + "')]"));

        element.GetAttribute("title").Should().Be(expectedValue);
    }

    /// <summary>
    /// Selects a stage on the business process flow.
    /// </summary>
    /// <param name="stageName">The name of the stage.</param>
    [Scope(Tag = "Trade")]
    [When("I select the '(.*)' stage of the business process flow")]
    public static void WhenISelectTheStageOfTheBusinessProcessFlow(string stageName)
    {

        Client.Execute(
            new BrowserCommandOptions(
                Constants.DefaultTraceSource,
                $"Select Stage: {stageName}",
                Constants.DefaultRetryAttempts,
                Constants.DefaultRetryDelay,
                null,
                true,
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException)),
            driver =>
            {
                var processStages = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.NextStage_UCI]));

                foreach (var processStage in processStages)
                {
                    var labels = processStage.FindElements(By.CssSelector("div[data-id*=\"MscrmControls.Containers.ProcessBreadCrumb-processHeaderStageName\"]"));

                    foreach (var label in labels)
                    {
                        if (label.Text.Equals(stageName, StringComparison.OrdinalIgnoreCase))
                        {
                            label.Click();
                        }
                    }
                }

                driver.WaitForTransaction();

                return true;
            });
    }

    [Then(@"I can see a (readonly|editable) business process flow field named (.*)")]
    public static void ICanSeeABPFHeaderField(string controlType, string fieldName)
    {
        Driver.WaitForTransaction();

        IWebElement element = null;
        if (controlType == "readonly")
        {
            element = Driver.FindElement(By.XPath($".//div[contains(@id, 'header_process_{fieldName}-locked-iconWrapper')]"));
        }
        else
        {
            element = Driver.FindElement(By.XPath($".//input[contains(@id, 'header_process_{fieldName}')]"));
        }
        element.Should().NotBeNull();
    }
}
