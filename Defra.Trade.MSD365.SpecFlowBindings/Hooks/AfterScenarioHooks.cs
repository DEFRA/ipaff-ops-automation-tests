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
    /// Returns true only when the Dynamics browser is active and owns step reporting.
    /// When IsDynamicsActive is false (e.g. during the IPAFFS tab hand-off phase),
    /// WebDriverHook owns reporting and these hooks must stand down.
    /// </summary>
    private bool IsDynamicsReportingActive =>
        Client.BrowserInitiated
        && scenarioContext.ContainsKey("IsDynamicsActive")
        && scenarioContext.Get<bool>("IsDynamicsActive");

    /// <summary>
    /// Logs scenario context values to the Extent report at the end of a passing Dynamics scenario.
    /// Failed Dynamics scenarios already have context attached to the failing step in AfterStepHooks.
    /// </summary>
    [AfterScenario(Order = 100)]
    public void LogContextForPassedDynamicsScenario()
    {
        if (!IsDynamicsReportingActive)
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
    /// Publishes the screenshot of the Dynamics browser when a Dynamics-owned test fails.
    /// Skipped when IsDynamicsActive is false — in that phase WebDriverHook owns screenshots.
    /// </summary>
    [AfterScenario(Order = 100)]
    public void PublishScreenshotForFailedScenario()
    {
        if (scenarioContext.ScenarioExecutionStatus != ScenarioExecutionStatus.TestError)
        {
            return;
        }

        if (!IsDynamicsReportingActive)
        {
            return;
        }

        var fileName = string.Concat(scenarioContext.ScenarioInfo.Title.Split(Path.GetInvalidFileNameChars()));
        var screenshotPath = Path.Combine(ScreenshotsFolder.FullName, $"{fileName}.jpg");

        if (!File.Exists(screenshotPath))
        {
            return;
        }

        Console.WriteLine(new Uri(screenshotPath));
        var screenshotBase64 = Convert.ToBase64String(File.ReadAllBytes(screenshotPath));
        Console.WriteLine("SCREENSHOT");
        Console.WriteLine($"SCREENSHOT[ {screenshotBase64} ]SCREENSHOT");
    }

    private string CreateLogForContextValues()
    {
        var internalKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ExtentScenario",
            "IsDynamicsActive",
            "DynamicsWindowHandle",
            "IpaffsInDynamicsBrowserHandle",
            "DynamicsIpaffsDriver"
        };

        var log = new StringBuilder("<pre>");
        try
        {
            foreach (var context in scenarioContext)
            {
                if (!internalKeys.Contains(context.Key))
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