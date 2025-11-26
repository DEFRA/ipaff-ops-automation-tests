

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using FluentAssertions;
using OpenQA.Selenium;
using Reqnroll;

[Binding]
public class InspectionSamplesSummarySteps : PowerAppsStepDefiner
{
    private readonly ScenarioContext _scenarioContext;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scenarioContext"></param>
    public InspectionSamplesSummarySteps(ScenarioContext scenarioContext)
    {
        this._scenarioContext = scenarioContext;
    }

    [Then(@"I should see the table headings within the Inspection Samples Summary report")]
    public static void ThenIShouldSeeTheTableHeadingsWithinTheInspectionSamplesSummaryReport(string ExpectedData)
    {
        var columnHeading = Driver.FindElement(By.XPath($"//div[text()='" + ExpectedData + "']"));
        columnHeading.Should().NotBeNull();
        columnHeading.Text.Should().Be(ExpectedData);
    }
}
