// <copyright file="TradeEntitySteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Reqnroll;
using System.Linq;

/// <summary>
/// Step bindings related to forms.
/// </summary>
[Binding]
public class TradeEntitySteps : PowerAppsStepDefiner
{
    /// <summary>
    /// Asserts that a table of form notifications are visible on the current form.
    /// </summary>
    /// <param name="table">Table of form notifications to assert against.</param>
    [Then(@"I can see following form notifications")]
    public void ThenICanSeeFollowingFormNotifications(Table table)
    {
        var expectedFormNotifications = table.CreateSet<FormNotification>();
        var actualFormNotifications = XrmApp.Entity.GetFormNotifications().Select(notification => new FormNotification { Message = notification.Message, Type = notification.Type }).ToList();
        actualFormNotifications.Should().BeEquivalentTo(expectedFormNotifications);
    }
}
