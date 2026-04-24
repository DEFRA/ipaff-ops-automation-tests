// <copyright file="InspectionResultSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Linq;

/// <summary>
/// Steps relating the processing of inspection results.
/// </summary>
[Binding]
public class InspectionResultSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;
    private readonly ScenarioContext scenarioContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="InspectionResultSteps"/> class.
    /// </summary>
    /// <param name="sessionContext">SessionContext.</param>
    /// <param name="scenarioContext">ScenarioContext.</param>
    public InspectionResultSteps(SessionContext sessionContext, ScenarioContext scenarioContext)
    {
        this.sessionContext = sessionContext;
        this.scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Opens an inspection result record for a related application.
    /// </summary>
    /// <param name="applicationAlias">The application related to the inspection result.</param>
    [Given("I have opened an inspection result for '(.*)'")]
    public void GivenIOpenInspectionResult(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = WorkOrderSteps.GetWorkOrder(svc, application);
            var inspectionresult = svc.WaitForRecords(
            new QueryByAttribute(trd_inspectionresult.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Attributes = { "trd_workorder" },
                Values = { workOrder.Id },
            },
            SpecflowBindingsConstants.DefaultWaitTime,
            SpecflowBindingsConstants.DefaultRetryInterval,
            false).Entities.Select(x => x.ToEntity<trd_inspectionresult>()).ToList().FirstOrDefault();
            context.ClearChanges();
            XrmApp.Entity.OpenEntity(trd_inspectionresult.EntityLogicalName, inspectionresult.Id);
            SharedSteps.WaitForScriptProcessing();
        }
    }

    /// <summary>
    /// Verifies that the specified check's Status field contains one of two expected values.
    /// Each status is a readonly input with aria-label matching '{checkName} Status'.
    /// PHSI checks live in the 'PHSI Inspection Results' section; all others live in 'HMI Inspection Results'.
    /// </summary>
    /// <param name="checkName">The check name parameter from Gherkin e.g. 'PHSI Doc Check'.</param>
    /// <param name="expectedValue">The primary expected status value e.g. 'Compliant'.</param>
    /// <param name="alternateValue">The alternate expected status value e.g. 'Auto cleared'.</param>
    [Then(@"the '(.*)' Status is '(.*)' or '(.*)'")]
    public void ThenTheCheckStatusIsCompliantOrAutoCleared(string checkName, string expectedValue, string alternateValue)
    {
        Driver.WaitForTransaction();

        var ariaLabel = $"{checkName} Status";

        // Derive the containing section from the check name prefix.
        // PHSI checks live in 'PHSI Inspection Results'; everything else is HMI.
        var sectionName = checkName.StartsWith("PHSI", StringComparison.OrdinalIgnoreCase)
            ? "PHSI Inspection Results"
            : "HMI Inspection Results";

        // Both PHSI and HMI sections live on the 'Inspection Results' tab.
        // Dynamics lazily renders tab content — sections are only added to the DOM when
        // the tab is first activated. If the section is not found, re-select the tab to
        // trigger rendering before retrying.
        const string inspectionResultsTabName = "Inspection Results";

        string actualValue = null;
        string debugSectionFound = "not attempted";
        string debugInputFound = "not attempted";
        string debugInputValue = "not attempted";

        var populated = Policy
            .Handle<Exception>()
            .OrResult<bool>(result => !result)
            .WaitAndRetry(
                retryCount: 12,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
                onRetry: (outcome, delay, attempt, _) =>
                {
                    // If the section was not found, the tab panel has not rendered.
                    // Re-select the Inspection Results tab to trigger lazy rendering.
                    if (debugSectionFound == "NOT FOUND")
                    {
                        Capgemini.PowerApps.SpecFlowBindings.Steps.EntitySteps.ISelectTab(inspectionResultsTabName);
                        Driver.WaitForTransaction();
                    }
                })
            .Execute(() =>
            {
                Driver.WaitForTransaction();

                // Re-find the section on every attempt — avoids stale element references.
                var sections = Driver.FindElements(By.XPath($"//section[@aria-label='{sectionName}']"));
                debugSectionFound = sections.Count > 0 ? $"found ({sections.Count})" : "NOT FOUND";

                if (sections.Count == 0)
                {
                    return false;
                }

                var inputs = sections[0].FindElements(By.XPath($".//input[@aria-label='{ariaLabel}']"));
                debugInputFound = inputs.Count > 0 ? $"found ({inputs.Count})" : "NOT FOUND";

                if (inputs.Count == 0)
                {
                    return false;
                }

                debugInputValue = inputs[0].GetAttribute("value")?.Trim() ?? string.Empty;
                actualValue = debugInputValue;

                return !string.IsNullOrEmpty(actualValue);
            });

        populated.Should().BeTrue(
            $"'{ariaLabel}' field did not populate within 60 seconds on the Inspection Result form. " +
            $"Last debug state — section '{sectionName}': {debugSectionFound}, " +
            $"input '{ariaLabel}': {debugInputFound}, " +
            $"last value read: '{debugInputValue}'.");

        actualValue.Should().BeOneOf(
            [expectedValue, alternateValue],
            $"Expected '{ariaLabel}' to be '{expectedValue}' or '{alternateValue}' but found '{actualValue}'.");
    }

    /// <summary>
    /// Verifies that all input fields within the named section on the Inspection Result form are empty.
    /// Sections are identified by their aria-label attribute on the containing section element
    /// e.g. 'PHSI Inspection Results', 'HMI Inspection Results'.
    /// A non-empty field is reported by aria-label and actual value to aid diagnosis.
    /// Retries with Polly to handle PCF controls that render asynchronously after the section
    /// element itself appears — avoids flaky 'inputs not found' failures.
    /// </summary>
    /// <param name="sectionName">The section aria-label e.g. 'HMI Inspection Results'.</param>
    [Then(@"the '(.*)' section is blank")]
    public void ThenTheSectionIsBlank(string sectionName)
    {
        Driver.WaitForTransaction();

        // Both 'PHSI Inspection Results' and 'HMI Inspection Results' live on the
        // 'Inspection Results' tab. Dynamics lazily renders tab content — sections only
        // appear in the DOM after the tab is activated. Never issue a full page Refresh
        // here: it navigates back to the default tab and destroys the lazily-loaded
        // section content, causing all subsequent FindElements calls to return NOT FOUND.
        // Instead, re-select the Inspection Results tab on retry to re-trigger rendering.
        const string inspectionResultsTabName = "Inspection Results";

        string debugSectionFound = "not attempted";
        string debugInputCount = "not attempted";
        System.Collections.Generic.List<string> nonEmptyInputs = null;

        var inputsFound = Policy
            .Handle<Exception>()
            .OrResult<bool>(result => !result)
            .WaitAndRetry(
                retryCount: 12,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
                onRetry: (outcome, delay, attempt, _) =>
                {
                    Capgemini.PowerApps.SpecFlowBindings.Steps.EntitySteps.ISelectTab(inspectionResultsTabName);
                    Driver.WaitForTransaction();
                })
            .Execute(() =>
            {
                Driver.WaitForTransaction();

                // Re-find the section on every attempt to avoid stale element references.
                var sections = Driver.FindElements(By.XPath($"//section[@aria-label='{sectionName}']"));
                debugSectionFound = sections.Count > 0 ? $"found ({sections.Count})" : "NOT FOUND";

                if (sections.Count == 0)
                {
                    return false;
                }

                var inputs = sections[0].FindElements(
                    By.XPath(".//input[contains(@class,'fui-Input__input')]"));

                debugInputCount = inputs.Count.ToString();

                if (inputs.Count == 0)
                {
                    return false;
                }

                nonEmptyInputs = inputs
                    .Where(i => !string.IsNullOrEmpty(i.GetAttribute("value")))
                    .Select(i =>
                        $"aria-label='{i.GetAttribute("aria-label")}' " +
                        $"value='{i.GetAttribute("value")}'")
                    .ToList();

                return true;
            });

        inputsFound.Should().BeTrue(
            $"Input fields within the '{sectionName}' section did not render within the retry window. " +
            $"Last debug state — section: {debugSectionFound}, inputs found: {debugInputCount}.");

        nonEmptyInputs.Should().BeEmpty(
            $"Expected all fields in the '{sectionName}' section to be blank " +
            $"but the following contained values: [{string.Join(", ", nonEmptyInputs)}].");
    }
}