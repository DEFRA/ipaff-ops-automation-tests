// <copyright file="AfterScenarioHooks.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Hooks;

using Capgemini.PowerApps.SpecFlowBindings;
using System;
using System.IO;
using System.Reflection;
using Reqnroll;

/// <summary>
/// After scenario hooks.
/// </summary>
[Binding]
public class AfterScenarioHooks : PowerAppsStepDefiner
{
    private readonly ScenarioContext scenarioContext;
    public static readonly DirectoryInfo ScreenshotsFolder = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "screenshots"));

    /// <summary>
    /// Initializes a new instance of the <see cref="AfterScenarioHooks"/> class.
    /// </summary>
    /// <param name="scenarioContext">The scenario context.</param>
    public AfterScenarioHooks(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }


    /// <summary>
    /// Publishes the screenshot of the browser when a test fails.
    /// </summary>
    [AfterScenario(Order = 100)]
    public void PublishScreenshotForFailedScenario()
    {
        if (this.scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError && Client.BrowserInitiated)
        {
            var fileName = string.Concat(this.scenarioContext.ScenarioInfo.Title.Split(Path.GetInvalidFileNameChars()));
            var screenshotPath = Path.Combine(ScreenshotsFolder.FullName, $"{fileName}.jpg");
            Console.WriteLine(new Uri(screenshotPath));
            var screenshotBase64 = Convert.ToBase64String(File.ReadAllBytes(screenshotPath));
            Console.WriteLine("SCREENSHOT");
            Console.WriteLine($"SCREENSHOT[ {screenshotBase64} ]SCREENSHOT");
        }
    }
}
