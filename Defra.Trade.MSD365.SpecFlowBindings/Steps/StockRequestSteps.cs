// <copyright file="StockRequestSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.MSD365.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Reqnroll;

/// <summary>
/// Step bindings relating to the Stock Request record.
/// </summary>
[Binding]
public sealed class StockRequestSteps : PowerAppsStepDefiner
{
    [Then("the Stock Request page is displayed")]
    public void ThenTheStockRequestPageIsDisplayed()
    {
        Driver.WaitForTransaction();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span']"),
            "Stock Request page header could not be found.");

        var actualPageHeader = pageHeader.Text.Trim();
        actualPageHeader.Should().Be("Stock Request",
            $"Expected page header to be 'Stock Request' but found '{actualPageHeader}'.");
    }

    [Then(@"the Status Reason of the Stock Request should be '(.*)'")]
    public void ThenTheStatusReasonOfTheStockRequestShouldBe(string expectedStatusReason)
    {
        Driver.WaitForTransaction();

        var headerHelper = new DynamicsHeaderHelper(Driver);
        var statusReasonText = headerHelper.GetHeaderFieldValueByLabel("Status Reason");

        statusReasonText.Should().NotBeNull(
            $"Status Reason '{expectedStatusReason}' could not be found in the Stock Request header, even after expanding " +
            "the 'More Header Editable Fields' overflow flyout.");

        statusReasonText.Should().Be(expectedStatusReason,
            $"Expected Stock Request Status Reason to be '{expectedStatusReason}' but found '{statusReasonText}'.");
    }

    [Then(@"the Status of the Stock Request should be '(.*)'")]
    public void ThenTheStatusOfTheStockRequestShouldBe(string expectedStatus)
    {
        Driver.WaitForTransaction();

        var headerHelper = new DynamicsHeaderHelper(Driver);
        var statusText = headerHelper.GetHeaderFieldValueByLabel("Status");

        statusText.Should().NotBeNull(
            $"Status '{expectedStatus}' could not be found in the Stock Request header, even after expanding " +
            "the 'More Header Editable Fields' overflow flyout.");

        statusText.Should().Be(expectedStatus,
            $"Expected Stock Request Status to be '{expectedStatus}' but found '{statusText}'.");
    }
}
