// <copyright file="PermanentAddressesSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Polly;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Step bindings for the Permanent Addresses tab and subgrid
/// (Subgrid_3 / defraimp_notificationpermanentaddress) on the Importer Notification form in PIMS.
/// </summary>
[Binding]
public class PermanentAddressesSteps : PowerAppsStepDefiner
{
    /// <summary>
    /// The data-id suffix used by D365 for the Permanent Addresses tab menu item.
    /// Confirmed from HTML: data-id="tablist-permanentaddresses_tab"
    /// </summary>
    private const string PermanentAddressesTabDataId = "tablist-permanentaddresses_tab";

    /// <summary>
    /// The data-control-name of the permanent addresses subgrid as seen in the DOM.
    /// Confirmed from HTML: data-control-name="Subgrid_3"
    /// Entity: defraimp_notificationpermanentaddress
    /// </summary>
    private const string SubGridControlName = "Subgrid_3";

    /// <summary>
    /// AG Grid container locator — targets the outermost div for this subgrid.
    /// </summary>
    private static readonly By SubGridContainerBy =
        By.XPath($"//div[@data-control-name='{SubGridControlName}']");

    /// <summary>
    /// AG Grid data rows — targets rendered rows inside the centre columns container only,
    /// which excludes the column header row.
    /// </summary>
    private static readonly By SubGridDataRowsBy =
        By.XPath(
            ".//div[@class='ag-center-cols-container']" +
            "//div[contains(@class,'ag-row')][@role='row']");

    private const string MultiSpeciesDataKey = "MultiSpeciesData";

    private readonly ScenarioContext scenarioContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermanentAddressesSteps"/> class.
    /// </summary>
    /// <param name="scenarioContext">The Reqnroll scenario context.</param>
    public PermanentAddressesSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Selects the Permanent Addresses tab on the Importer Notification form.
    /// <para>
    /// The standard <c>When I select the '...' tab</c> step targets classic UCI tabs
    /// using <c>li[title="..."]</c>. The Permanent Addresses tab is rendered as a
    /// Fluent UI <c>role="menuitem"</c> element with <c>data-id="tablist-permanentaddresses_tab"</c>
    /// and has no <c>title</c> attribute, so a dedicated locator is required.
    /// </para>
    /// </summary>
    [When("I select the Permanent Addresses tab")]
    public void WhenISelectThePermanentAddressesTab()
    {
        Driver.WaitForTransaction();

        // The tab may be in the visible tab strip or collapsed into the 'More' overflow menu.
        // Try the direct tab first, then fall back to the overflow menu item.
        var tabLocator = By.XPath($"//div[@data-id='{PermanentAddressesTabDataId}']");

        if (!Driver.TryFindElement(tabLocator, out var tabElement))
        {
            // Tab is in the overflow ('More') menu — open it and look there.
            var moreButton = Driver.WaitUntilAvailable(
                By.XPath(AppElements.Xpath[AppReference.Entity.MoreTabs]),
                TimeSpan.FromSeconds(10),
                "Could not find the 'More tabs' button to locate the Permanent Addresses tab.");

            moreButton.Click();
            Driver.WaitForTransaction();

            tabElement = Driver.WaitUntilAvailable(
                tabLocator,
                TimeSpan.FromSeconds(10),
                $"The 'Permanent Addresses' tab (data-id='{PermanentAddressesTabDataId}') " +
                "could not be found in the tab strip or the overflow menu.");
        }

        tabElement.Click();
        Driver.WaitForTransaction();

        // Wait for the subgrid container to confirm the tab has loaded.
        Driver.WaitUntilAvailable(
            SubGridContainerBy,
            TimeSpan.FromSeconds(30),
            $"The Permanent Addresses subgrid did not appear after selecting the tab.");
    }

    /// <summary>
    /// Verifies that every permanent address row in the PIMS subgrid matches
    /// the corresponding address stored per-animal in the scenario context
    /// during the IPAFFS leg.
    /// <para>
    /// Field mapping (mirrors ValidateContains in MultiSpeciesData.ValidateAgainstReviewPage):
    /// <list type="bullet">
    ///   <item>OperatorName      → col-id <c>defraimp_companyname</c></item>
    ///   <item>AddressLine1      → col-id <c>defraimp_addressline1</c></item>
    ///   <item>CityOrTown        → col-id <c>defraimp_addressline2</c></item>
    ///   <item>Postcode          → col-id <c>defraimp_addressline3</c></item>
    ///   <item>TelephoneNumber   → col-id <c>defraimp_addresstelephone</c></item>
    ///   <item>Email             → col-id <c>defraimp_addressemail</c></item>
    /// </list>
    /// </para>
    /// </summary>
    [Then("I verify the permanent address displayed for each animal matches the address entered in IPAFFS")]
    public void ThenIVerifyThePermanentAddressDisplayedForEachAnimalMatchesTheAddressEnteredInIPAFFS()
    {
        Driver.WaitForTransaction();

        scenarioContext.ContainsKey(MultiSpeciesDataKey).Should().BeTrue(
            "MultiSpeciesData must be populated in the scenario context before verifying permanent addresses.");

        var multiSpeciesData = GetMultiSpeciesDataFromContext();
        var expectedAddresses = BuildExpectedAddressList(multiSpeciesData);

        expectedAddresses.Should().NotBeEmpty(
            "At least one animal must have a permanent address stored in the scenario context.");

        var actualRows = GetPermanentAddressRowsWithRetry();

        using (new AssertionScope())
        {
            actualRows.Count.Should().Be(
                expectedAddresses.Count,
                $"Expected {expectedAddresses.Count} permanent address row(s) in the subgrid " +
                $"but found {actualRows.Count}.");

            for (int i = 0; i < expectedAddresses.Count; i++)
            {
                var expected = expectedAddresses[i];
                var actual = i < actualRows.Count ? actualRows[i] : null;

                if (actual is null)
                {
                    false.Should().BeTrue(
                        $"Row {i + 1} (species: '{expected.SpeciesName}', animal index: {expected.AnimalIndex}) " +
                        "is missing from the Permanent Addresses subgrid.");
                    continue;
                }

                var addr = expected.Address;
                var rowLabel = $"Row {i + 1} (species: '{expected.SpeciesName}', animal index: {expected.AnimalIndex})";

                AssertCellContains(actual.CompanyName, addr.OperatorName, rowLabel, "Company Name (defraimp_companyname)");
                AssertCellContains(actual.AddressLine1, addr.AddressLine1, rowLabel, "Address Line 1 (defraimp_addressline1)");
                AssertCellContains(actual.AddressLine2, addr.CityOrTown, rowLabel, "Address Line 2 / City (defraimp_addressline2)");
                AssertCellContains(actual.AddressLine3, addr.Postcode, rowLabel, "Address Line 3 / Postcode (defraimp_addressline3)");
                AssertCellContains(actual.Telephone, addr.TelephoneNumber, rowLabel, "Telephone (defraimp_addresstelephone)");
                AssertCellContains(actual.Email, addr.Email, rowLabel, "Email (defraimp_addressemail)");
            }
        }
    }

    // ─── Private helpers ────────────────────────────────────────────────────────

    private static List<ExpectedPermanentAddress> BuildExpectedAddressList(MultiSpeciesDataSnapshot data)
    {
        var result = new List<ExpectedPermanentAddress>();

        foreach (var (speciesName, speciesData) in data.Species)
        {
            foreach (var (animalIndex, animalData) in speciesData.Animals.OrderBy(kv => kv.Key))
            {
                if (animalData.PermanentAddress is not null)
                {
                    result.Add(new ExpectedPermanentAddress(speciesName, animalIndex, animalData.PermanentAddress));
                }
            }
        }

        return result;
    }

    private List<ActualPermanentAddressRow> ReadSubGridRows()
    {
        var container = Driver.WaitUntilAvailable(
            SubGridContainerBy,
            TimeSpan.FromSeconds(30),
            $"Permanent Addresses subgrid '{SubGridControlName}' was not available.");

        var rows = container.FindElements(SubGridDataRowsBy);

        return rows.Select(row => new ActualPermanentAddressRow(
            CompanyName: GetCellText(row, "defraimp_companyname"),
            AddressLine1: GetCellText(row, "defraimp_addressline1"),
            AddressLine2: GetCellText(row, "defraimp_addressline2"),
            AddressLine3: GetCellText(row, "defraimp_addressline3"),
            Telephone: GetCellText(row, "defraimp_addresstelephone"),
            Email: GetCellText(row, "defraimp_addressemail")
        )).ToList();
    }

    private List<ActualPermanentAddressRow> GetPermanentAddressRowsWithRetry()
    {
        var rows = new List<ActualPermanentAddressRow>();

        Policy
            .Handle<NoSuchElementException>()
            .Or<StaleElementReferenceException>()
            .WaitAndRetry(3, attempt => TimeSpan.FromSeconds(attempt * 2))
            .Execute(() => rows = ReadSubGridRows());

        return rows;
    }

    private static string GetCellText(IWebElement row, string colId)
    {
        try
        {
            return row
                .FindElement(By.XPath($".//div[@col-id='{colId}']"))
                .Text
                .Trim();
        }
        catch (NoSuchElementException)
        {
            return string.Empty;
        }
    }

    private static void AssertCellContains(
        string actualCellText,
        string expectedValue,
        string rowLabel,
        string fieldDescription)
    {
        if (string.IsNullOrWhiteSpace(expectedValue))
            return;

        actualCellText.Should().ContainEquivalentOf(
            expectedValue,
            $"{rowLabel} — {fieldDescription} mismatch.");
    }

    private MultiSpeciesDataSnapshot GetMultiSpeciesDataFromContext()
    {
        var raw = scenarioContext[MultiSpeciesDataKey];
        dynamic d = raw;
        var snapshot = new MultiSpeciesDataSnapshot();

        foreach (var kvp in d.Species)
        {
            var speciesName = (string)kvp.Key;
            var speciesData = new SpeciesDataSnapshot();
            dynamic sd = kvp.Value;

            foreach (var animalKvp in sd.Animals)
            {
                int animalIndex = (int)animalKvp.Key;
                dynamic ad = animalKvp.Value;

                if (ad.PermanentAddress == null)
                    continue;

                dynamic pa = ad.PermanentAddress;
                speciesData.Animals[animalIndex] = new AnimalDataSnapshot
                {
                    PermanentAddress = new PermanentAddressSnapshot
                    {
                        OperatorName = (string)(pa.OperatorName ?? string.Empty),
                        AddressLine1 = (string)(pa.AddressLine1 ?? string.Empty),
                        CityOrTown = (string)(pa.CityOrTown ?? string.Empty),
                        Postcode = (string)(pa.Postcode ?? string.Empty),
                        TelephoneNumber = (string)(pa.TelephoneNumber ?? string.Empty),
                        Email = (string)(pa.Email ?? string.Empty),
                    },
                };
            }

            snapshot.Species[speciesName] = speciesData;
        }

        return snapshot;
    }

    // ─── Private record / class types ───────────────────────────────────────────

    private sealed record ExpectedPermanentAddress(
        string SpeciesName,
        int AnimalIndex,
        PermanentAddressSnapshot Address);

    private sealed record ActualPermanentAddressRow(
        string CompanyName,
        string AddressLine1,
        string AddressLine2,
        string AddressLine3,
        string Telephone,
        string Email);

    private sealed class MultiSpeciesDataSnapshot
    {
        public Dictionary<string, SpeciesDataSnapshot> Species { get; } = [];
    }

    private sealed class SpeciesDataSnapshot
    {
        public Dictionary<int, AnimalDataSnapshot> Animals { get; } = [];
    }

    private sealed class AnimalDataSnapshot
    {
        public PermanentAddressSnapshot? PermanentAddress { get; set; }
    }

    private sealed class PermanentAddressSnapshot
    {
        public string OperatorName { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string CityOrTown { get; set; } = string.Empty;
        public string Postcode { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}