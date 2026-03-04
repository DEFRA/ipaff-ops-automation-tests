// <copyright file="AfterScenarioHooks.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Hooks;

using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin;
using Capgemini.PowerApps.SpecFlowBindings;
using Reqnroll;
using System;
using System.IO;
using System.Reflection;
using System.Text;

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
    /// Logs scenario context values to the Extent report at the end of a passing PIMS scenario.
    /// Failed PIMS scenarios already have context attached to the failing step in AfterStepHooks.
    /// </summary>
    [AfterScenario(Order = 50)]
    public void LogContextForPassedPimsScenario()
    {
        if (!Client.BrowserInitiated)
        {
            return;
        }

        if (scenarioContext.TestError == null && scenarioContext.ContainsKey("ExtentScenario"))
        {
            try
            {
                var scenario = scenarioContext.Get<ExtentTest>("ExtentScenario");
                var log = CreateLogForContextValues();
                if (!string.IsNullOrWhiteSpace(log) && log != "<pre></pre>")
                {
                    scenario.CreateNode(new GherkinKeyword("*"), "LOG: Captured Scenario Context Values")
                            .Info(log);
                }
            }
            catch { }
        }
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

    private string CreateLogForContextValues()
    {
        var log = new StringBuilder("<pre>");
        try
        {
            foreach (var context in scenarioContext)
            {
                if (!context.Key.Equals("ExtentScenario") && !context.Key.Equals("IsPimsActive"))
                {
                    log.AppendLine($"{context.Key} : <b>{FormatValue(context.Value)}</b><br>");
                }
            }
        }
        catch (Exception ex)
        {
            log.AppendLine($"Error capturing context values: {ex.Message}<br>");
        }

        log.Append("</pre>");
        return log.ToString();
    }

    private static string FormatValue(object value)
    {
        if (value == null)
            return "null";

        if (value is Array array)
            return string.Join(", ", array.Cast<object>());

        if (value is IEnumerable<object> list)
            return string.Join(", ", list);

        if (value is System.Collections.IEnumerable enumerable && value is not string)
            return string.Join(", ", enumerable.Cast<object>());

        return value.ToString();
    }
}