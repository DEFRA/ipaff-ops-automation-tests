// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using System;
using System.Drawing;
using System.Linq;
using System.Web;
using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Reqnroll;

[Binding]
public class WindowSteps : PowerAppsStepDefiner
{
    private readonly SessionContext context;

    public WindowSteps(SessionContext context)
    {
        this.context = context;
    }

    [When("I switch to the new window that has opened")]
    public static void ISwitchToTheNewWindowThatHasOpened()
    {
        Driver.LastWindow();
        Driver.WaitForPageToLoad();
        Driver.WaitForTransaction();

        if (TestConfig.BrowserOptions.Width.HasValue && TestConfig.BrowserOptions.Height.HasValue)
        {
            Driver.Manage().Window.Size = new Size(TestConfig.BrowserOptions.Width.Value, TestConfig.BrowserOptions.Height.Value);
        }
    }

    [When(@"I close the current window")]
    public void ICloseTheCurrentWindow()
    {
        Driver.LastWindow().Close();
        this.context.SetVariable("WindowCount", Driver.WindowHandles.Count);
    }

    [When("I return to the main window")]
    public static void IReturnToTheMainWindow()
    {
        try
        {
            Driver.SwitchTo().Window(Driver.WindowHandles.First(x => x != Driver.CurrentWindowHandle));
        }
        catch
        {
            // current window handle may have been closed
            Driver.SwitchTo().Window(Driver.WindowHandles.First());
        }
    }

    [When("I close the current window and return to the main window")]
    public void ICloseCurrentWindowAndReturnToTheMainWindow()
    {
        var currentWindowHandle = Driver.CurrentWindowHandle;
        Driver.LastWindow().Close();
        Driver.SwitchTo().Window(Driver.WindowHandles.Count == 1 ? Driver.WindowHandles.First() : Driver.WindowHandles.First(x => x != currentWindowHandle));
        this.context.SetVariable("WindowCount", Driver.WindowHandles.Count);
    }

    [Then(@"current url segment contains '(.*)'")]
    public void ThenCurrentUrlSegmentContains(string key)
    {
        var value = this.context.GetVariable<string>(key);
        if (value != null)
        {
            key = value;
        }

        Driver.LastWindow();
        var url = Driver.Url;
        new Uri(url).Segments.Count(p => HttpUtility.UrlDecode(p).Replace("/", string.Empty) == key).Should().Be(1, $"given segment {key} not found in {url}");
    }

    [Then(@"a new window is opened")]
    public void ThenANewWindowIsOpened()
    {
        this.AssertThatNewWindowsIsOpened();
    }

    // TODO: This seems like an odd assertion for an acceptance test. Review.
    [Then(@"a new window is opened with the following query-string values in the url")]
    public void ThenANewWindowIsOpenedWithTheFollowingValuesInTheUrl(Table table)
    {
        this.AssertThatNewWindowsIsOpened();
        Driver.LastWindow();
        var url = new Uri(Driver.Url);

        foreach (var tableRow in table.Rows)
        {
            if (tableRow.ContainsKey("Type") && tableRow["Type"].ToLower() == "guid")
            {
                new Guid(HttpUtility.ParseQueryString(url.Query).Get(tableRow["Key"])).Should().Be(this.context.GetVariable<Guid>(tableRow["Value"]));
            }
            else
            {
                HttpUtility.ParseQueryString(url.Query).Get(tableRow["Key"]).Should().Be(tableRow["Value"]);
            }
        }
    }

    /// <summary>
    /// Asserts the url is as expected.
    /// </summary>
    /// <param name="url">The expected URL.</param>
    [Then("the url equals '(.*)'")]
    public void ThenTheUrlEquals(string url)
    {
        Driver.Url.Should().Be(url);
    }

    private void AssertThatNewWindowsIsOpened()
    {
        Driver.WaitForTransaction();
        Driver.WindowHandles.Count.Should().Be(1 + (int)(this.context.GetVariable("WindowCount") ?? 1));
        this.context.SetVariable("WindowCount", Driver.WindowHandles.Count);
    }
}
