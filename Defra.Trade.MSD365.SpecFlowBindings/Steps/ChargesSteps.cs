// <copyright file="ChargesSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Linq;

/// <summary>
/// Step bindings relating to the Charges subgrid.
/// </summary>
[Binding]
public class ChargesSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChargesSteps"/> class.
    /// </summary>
    /// <param name="sessionContext">SessionContext.</param>
    public ChargesSteps(SessionContext sessionContext)
    {
        this.sessionContext = sessionContext;
    }

    /// <summary>
    /// Verifies that the Charges subgrid view selector displays the expected view name.
    /// Uses the same ViewSelector span pattern as the Commodity Lines frame check.
    /// </summary>
    /// <param name="expectedViewName">The expected view name e.g. 'Charge Associated View'.</param>
    [When(@"I check that the Charges tab shows '(.*)'")]
    [Then(@"I check that the Charges tab shows '(.*)'")]
    public void ThenICheckThatTheChargesTabShows(string expectedViewName)
    {
        Driver.WaitForTransaction();

        var viewLabel = Driver.WaitUntilAvailable(
            By.XPath($"//span[contains(@id,'ViewSelector_') and contains(@id,'_text-value') and normalize-space(text())='{expectedViewName}']"),
            $"Charges subgrid view label '{expectedViewName}' could not be found.");

        viewLabel.Text.Trim().Should().Be(expectedViewName,
            $"Expected Charges subgrid to show view '{expectedViewName}' but found '{viewLabel.Text.Trim()}'.");
    }

    /// <summary>
    /// Verifies that at least one Charge row exists in the Charges subgrid for the specified
    /// Work Order Task name. Refreshes the subgrid and retries for up to 60 seconds to allow
    /// charge records to be created asynchronously after the work order task completes.
    /// </summary>
    /// <param name="taskName">The Work Order Task name e.g. 'Identity &amp; Physical Check', 'Document Check'.</param>
    [Then(@"the '(.*)' Charges records have been created")]
    public void ThenTheChargesRecordsHaveBeenCreated(string taskName)
    {
        Driver.WaitForTransaction();

        // Charges are created asynchronously after task completion — poll with subgrid
        // refreshes until at least one row for the given task appears, or time out.
        var found = Policy
            .Handle<Exception>()
            .OrResult<bool>(result => !result)
            .WaitAndRetry(
                retryCount: 12,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
                onRetry: (outcome, delay, attempt, _) =>
                {
                    var reason = outcome.Exception != null
                        ? $"exception: {outcome.Exception.GetType().Name} — {outcome.Exception.Message}"
                        : "no matching rows found";

                    Console.WriteLine(
                        $"[ThenTheChargesRecordsHaveBeenCreated] Attempt {attempt}/12: " +
                        $"{reason}. Refreshing subgrid and retrying in {delay.TotalSeconds}s...");

                    Driver.WaitUntilAvailable(
                        By.XPath("//button[@data-id='trd_charge|NoRelationship|SubGridAssociated|Mscrm.SubGrid.trd_charge.RefreshButton']"),
                        "Charges subgrid Refresh button could not be found.")
                        .Click();

                    Driver.WaitForTransaction();
                })
            .Execute(() =>
            {
                Driver.WaitForTransaction();

                var grids = Driver.FindElements(
                    By.XPath("//div[@role='treegrid'][@aria-label='Charge Associated View']"));

                if (grids.Count == 0)
                {
                    return false;
                }

                var matchingLinks = grids[0].FindElements(
                    By.XPath($".//div[@role='row'][@aria-label='Press SPACE to select this row.']" +
                             $"//div[@col-id='trd_workordertaskid']//a[@aria-label='{taskName}']"));

                return matchingLinks.Count > 0;
            });

        found.Should().BeTrue(
            $"Expected at least one Charge record linked to Work Order Task '{taskName}' " +
            $"in the Charges subgrid but none were found after 60 seconds.");
    }
}