namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;

[Binding]
public class CommodityLineSteps : PowerAppsStepDefiner
{
    private readonly ScenarioContext scenarioContext;

    #region Page objects
    private static readonly By PageHeading = By.CssSelector("[data-id='entity_name_span']");
    private static readonly Dictionary<string, string> FieldIds = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Container number"] = "trd_containernumber.fieldControl-text-box-text",
        ["Potato Type"] = "trd_potatotype.fieldControl-text-box-text",
        ["Packaging Material"] = "trd_packagingmaterialused.fieldControl-text-box-text",
        ["Distinguishing Marks"] = "trd_distinguishingmarks.fieldControl-text-box-text",
        ["Quantity Exported"] = "trd_finalweight.fieldControl-decimal-number-text-input",
        ["Total Quantity"] = "trd_quantity.fieldControl-decimal-number-text-input",
        ["Stock Number"] = "trd_stocknumber.fieldControl-text-box-text",
        ["GSI Number"] = "trd_gsinumber.fieldControl-text-box-text",
    };
    private static readonly Dictionary<string, string> ContextKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Container number"] = "ExporterContainerNumber",
        ["Stock Number"] = "PotatoStockNumber",
        ["Packaging Material"] = "PotatoPackingMaterial",
        ["GSI Number"] = "PotatoGsiNumber",
    };
    #endregion

    public CommodityLineSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Reads the entity name in the visible form header and asserts it is exactly 'Commodity Line'.
    /// </summary>
    [Then("the Commodity Line page is displayed")]
    public void ThenTheCommodityLinePageIsDisplayed()
    {
        Read(() => Driver.FindElements(PageHeading).Last(e => e.Displayed).Text.Trim())
            .Should().Be("Commodity Line");
    }

    /// <summary>
    /// Maps each field label to its Dynamics schema data-id (e.g. trd_containernumber) and asserts the input control is visible on the form.
    /// </summary>
    [Then("the following fields are displayed on the Commodity Line page {string} {string} {string} {string} {string} {string}")]
    public void ThenTheFollowingFieldsAreDisplayedOnTheCommodityLinePage(string field1, string field2, string field3, string field4, string field5, string field6)
    {
        foreach (var field in new[] { field1, field2, field3, field4, field5, field6 })
        {
            Read(() => GetField(field).Displayed).Should().BeTrue($"'{field}' should be displayed");
        }
    }

    /// <summary>
    /// Compares on-screen coordinates: Quantity Exported must start below Total Quantity in the same column (within 5px),
    /// with no other visible input positioned between them.
    /// </summary>
    [Then("the Quantity Exported field is directly below the Total Quantity field")]
    public void ThenTheQuantityExportedFieldIsDirectlyBelowTheTotalQuantityField()
    {
        var positions = Read(() =>
        {
            var total = GetField("Total Quantity");
            var exported = GetField("Quantity Exported");
            var interveningFields = Driver.FindElements(By.CssSelector("input[data-id], select[data-id], textarea[data-id]"))
                .Count(e => e.Displayed && Math.Abs(e.Location.X - total.Location.X) <= 5
                    && e.Location.Y > total.Location.Y && e.Location.Y < exported.Location.Y);
            return (Total: total.Location, Height: total.Size.Height, Exported: exported.Location, Intervening: interveningFields);
        });
        positions.Exported.Y.Should().BeGreaterThanOrEqualTo(positions.Total.Y + positions.Height);
        Math.Abs(positions.Exported.X - positions.Total.X).Should().BeLessThanOrEqualTo(5,
            "the fields should be in the same column");
        positions.Intervening.Should().Be(0, "no field should intervene between Total Quantity and Quantity Exported");
    }

    /// <summary>
    /// Compares each field's value on the Commodity Line form with the value captured in scenario context when entered in the Exporter Portal
    /// (e.g. 'PotatoStockNumber'), after confirming the named application was created.
    /// </summary>
    [Then("the {string} {string} {string} {string} values match those entered in the Exporter Portal for {string}")]
    public void ThenTheValuesMatchThoseEnteredInTheExporterPortalFor(string field1, string field2, string field3, string field4, string application)
    {
        scenarioContext.ContainsKey($"{application}_APHAReference").Should().BeTrue($"'{application}' must have been created in the Exporter Portal");
        foreach (var field in new[] { field1, field2, field3, field4 })
        {
            var expected = scenarioContext.Get<string>(ContextKeys[field]);
            Read(() => GetField(field).GetAttribute("value").Trim()).Should().Be(expected, $"'{field}' should match '{application}'");
        }
    }

    private static IWebElement GetField(string field) => Driver.FindElements(By.CssSelector($"[data-id='{FieldIds[field]}']"))
        .LastOrDefault(e => e.Displayed) ?? throw new NoSuchElementException($"Commodity Line field '{field}' was not displayed.");

    private static T Read<T>(Func<T> read) => Policy.Handle<StaleElementReferenceException>()
        .Or<NoSuchElementException>().WaitAndRetry(4, _ => TimeSpan.FromSeconds(1)).Execute(() =>
        {
            Driver.WaitForTransaction();
            return read();
        });
}
