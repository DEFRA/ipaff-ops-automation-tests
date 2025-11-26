namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reqnroll;

[Binding]
public class InspectionTestSummarySteps : PowerAppsStepDefiner
{
    private readonly ScenarioContext scenarioContext;

    public InspectionTestSummarySteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    [Then(@"I should see a report with the heading '(.*)'")]
    public static void ThenIShouldSeeAReportWithTheHeading(string reportTitle)
    {
        Driver.SwitchToReportContext(1);

        var title = Driver.FindElement(By.XPath("//div[text()='" + reportTitle + "']"));
        title.Should().NotBeNull();
        title.Text.Should().Be(reportTitle);
    }

    [Then(@"I should see the subheadings within the Inspection Test Summary report")]
    public static void ThenIShouldSeeTheSubheadingsWithinTheInspectionTestSummaryReport(string ExpectedData)
    {
        var subHeading = Driver.FindElement(By.XPath($"//div[text()='" + ExpectedData + "']"));
        subHeading.Should().NotBeNull();
        subHeading.Text.Should().Be(ExpectedData);
    }
}
