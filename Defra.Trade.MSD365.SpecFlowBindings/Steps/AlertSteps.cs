// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Threading;
using Reqnroll;

/// <summary>
/// Steps for Alert related functionality and assertions.
/// </summary>
[Binding]
public class AlertSteps : PowerAppsStepDefiner
{
    /// <summary>
    /// When Step that uses the PowerAppsStepDefiner driver object to switch to the currently opened alert and accept it.
    /// </summary>
    [When(@"I accept the alert")]
    public static void WhenIAcceptTheAlert()
    {
        Driver.SwitchTo().Alert().Accept();
        XrmApp.ThinkTime(2000);
    }

    /// <summary>
    /// Then Step that uses the PowerAppsStepDefiner driver object to assert alert message.
    /// </summary>
    /// <param name="expectedMessage">expected alert message</param>
    [Then(@"an alert is displayed with message '(.*)'")]
    public static void ThenAnAlertIsDisplayedWithMessage(string expectedMessage)
    {
        var message = Driver.SwitchTo().Alert().Text;
        message.Should().Be(expectedMessage);
    }

    [Then(@"I click the close button on the alert")]
    public void ThenIClickTheCloseButtonOnTheAlert()
    {
        Thread.Sleep(TimeSpan.FromSeconds(8));
        var button = Driver.FindElement(By.XPath("//button[@data-id='okButton']"));
        button.Click();
    }

}
