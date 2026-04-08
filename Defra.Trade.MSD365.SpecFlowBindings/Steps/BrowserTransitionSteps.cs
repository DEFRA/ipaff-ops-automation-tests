namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using Reqnroll.BoDi;
using System.Reflection;

/// <summary>
/// Step bindings that handle driver hand-off when Dynamics opens IPAFFS in a new tab.
/// </summary>
[Binding]
public class BrowserTransitionSteps : PowerAppsStepDefiner
{
    private readonly IObjectContainer _objectContainer;
    private readonly ScenarioContext _scenarioContext;

    public BrowserTransitionSteps(IObjectContainer objectContainer, ScenarioContext scenarioContext)
    {
        _objectContainer = objectContainer;
        _scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Clicks the IPAFFS link in the Dynamics header ribbon, waits for the new tab to open,
    /// then forcibly swaps the IWebDriver instance in both BoDi's registration store AND
    /// its resolved-objects cache so all subsequent IPAFFS page classes resolve the Dynamics
    /// driver focused on the IPAFFS tab.
    /// </summary>
    [When("I click IPAFFS from the header ribbon")]
    public void WhenIClickIPAFFSFromTheHeaderRibbon()
    {
        var handlesBefore = Driver.WindowHandles.ToList();

        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("IPAFFS");
        Driver.WaitForTransaction();

        // Wait for the new IPAFFS tab to open
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
        wait.Until(d => d.WindowHandles.Count > handlesBefore.Count);

        // Switch the Dynamics driver to the new IPAFFS tab
        var newHandle = Driver.WindowHandles.Except(handlesBefore).Single();
        Driver.SwitchTo().Window(newHandle);

        // Wait for IPAFFS to fully load
        wait.Until(d => !string.IsNullOrEmpty(d.Url) && d.Url != "about:blank");

        // Store handles so we can switch back later
        _scenarioContext["DynamicsWindowHandle"] = handlesBefore.Last();
        _scenarioContext["IpaffsInDynamicsBrowserHandle"] = newHandle;

        // Swap IWebDriver in both BoDi's registration store and resolved-objects cache
        SwapRegisteredWebDriver(Driver);

        // Signal to AfterStepHooks that we are now executing IPAFFS steps
        _scenarioContext["IsDynamicsActive"] = false;

        Console.WriteLine($"Switched Dynamics driver to IPAFFS tab: {newHandle}. IPAFFS step definitions will now use this driver.");
    }

    /// <summary>
    /// Switches the Dynamics driver back to the Dynamics tab.
    /// The registered IWebDriver IS the Dynamics driver — switching its active window
    /// is sufficient since BoDi returns the same instance to all consumers.
    /// </summary>
    [When("I switch back to the Dynamics tab")]
    [Then("I switch back to the Dynamics tab")]
    public void WhenISwitchBackToDynamicsTab()
    {
        var dynamicsHandle = _scenarioContext.Get<string>("DynamicsWindowHandle");
        Driver.SwitchTo().Window(dynamicsHandle);

        // Restore Dynamics active flag so AfterStepHooks resumes Dynamics reporting
        _scenarioContext["IsDynamicsActive"] = true;

        Console.WriteLine($"Switched back to Dynamics tab: {dynamicsHandle}.");
    }

    /// <summary>
    /// Bypasses BoDi's "already resolved" guard by updating IWebDriver in both:
    ///   1. The registration store  — so future Resolve() calls return the new driver
    ///   2. The resolved-objects cache — so cached resolutions are also replaced
    /// Both fields must be updated; updating only the registration has no effect once
    /// the interface has been resolved because BoDi serves the cached value.
    /// </summary>
    private void SwapRegisteredWebDriver(IWebDriver newDriver)
    {
        var containerType = _objectContainer.GetType();
        const BindingFlags nonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;

        // ── 1. Swap the instance in the registrations dictionary ──────────────────
        var registrationsField = containerType.GetField("registrations", nonPublicInstance)
                              ?? containerType.GetField("_registrations", nonPublicInstance)
                              ?? throw new InvalidOperationException(
                                     "Could not locate BoDi's internal registrations field. " +
                                     "Check the BoDi version for the correct field name.");

        var registrations = registrationsField.GetValue(_objectContainer)
                            ?? throw new InvalidOperationException("BoDi registrations dictionary is null.");

        var getItemMethod = registrations.GetType().GetMethod("get_Item")
                            ?? throw new InvalidOperationException("Could not find get_Item on registrations dictionary.");

        foreach (var key in (System.Collections.IEnumerable)registrations.GetType().GetProperty("Keys")!.GetValue(registrations)!)
        {
            var registeredType = key.GetType().GetProperty("Type")?.GetValue(key) as Type;
            if (registeredType != typeof(IWebDriver))
                continue;

            var registration = getItemMethod.Invoke(registrations, [key])
                               ?? throw new InvalidOperationException("BoDi IWebDriver registration entry is null.");

            var instanceField = registration.GetType().GetField("instance", nonPublicInstance)
                             ?? registration.GetType().GetField("_instance", nonPublicInstance)
                             ?? throw new InvalidOperationException(
                                    "Could not locate the instance field on BoDi's InstanceRegistration. " +
                                    "Check the BoDi version for the correct field name.");

            instanceField.SetValue(registration, newDriver);
            break;
        }

        // ── 2. Evict the stale entry from BoDi's resolved-objects cache ───────────
        // BoDi caches resolved instances in a Dictionary<RegistrationKey, object>
        // under the field "resolvedObjects" (or "_resolvedObjects").
        // Once cached, Resolve<T>() returns the cached value regardless of the
        // registration, so we must remove the IWebDriver entry to force a fresh
        // lookup that returns our updated registration.
        var resolvedField = containerType.GetField("resolvedObjects", nonPublicInstance)
                         ?? containerType.GetField("_resolvedObjects", nonPublicInstance);

        if (resolvedField != null)
        {
            var resolvedObjects = resolvedField.GetValue(_objectContainer);
            if (resolvedObjects != null)
            {
                var removeMethod = resolvedObjects.GetType().GetMethod("Remove",
                    [resolvedObjects.GetType().GetGenericArguments()[0]]);

                if (removeMethod != null)
                {
                    foreach (var key in ((System.Collections.IEnumerable)resolvedObjects.GetType()
                                           .GetProperty("Keys")!.GetValue(resolvedObjects)!)
                                        .Cast<object>().ToList())
                    {
                        var keyType = key.GetType().GetProperty("Type")?.GetValue(key) as Type;
                        if (keyType == typeof(IWebDriver))
                        {
                            removeMethod.Invoke(resolvedObjects, [key]);
                            break;
                        }
                    }
                }
            }
        }

        Console.WriteLine("Successfully swapped IWebDriver instance in BoDi container (registration + cache).");
    }
}