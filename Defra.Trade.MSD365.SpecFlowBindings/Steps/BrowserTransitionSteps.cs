namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using Reqnroll.BoDi;

/// <summary>
/// Step bindings that handle driver hand-off when Dynamics opens IPAFFS in a new tab.
///
/// Architecture:
/// Assembly 1 (Defra.UI.Tests) owns Browser 1 — all IPAFFS page classes resolve IWebDriver
/// from its BoDi container. Assembly 2 (this assembly) owns Browser 2 (the Dynamics/EasyRepro
/// driver). The two containers are siblings with no shared ancestry, so IWebDriver cannot be
/// injected across them. ScenarioContext is the shared channel: the Dynamics driver (switched
/// to the IPAFFS tab) is stored here and re-registered into Assembly 1's container by
/// the "I switch to the IPAFFS tab" step in SignOutSteps.
///
/// Reporting ownership:
/// - Before this step: IsDynamicsActive=true  → Dynamics AfterStepHooks owns screenshots (Browser 2, Dynamics tab)
/// - After this step:  IsDynamicsActive=false → WebDriverHook.AfterStep owns screenshots (Browser 2, IPAFFS tab)
/// - After "I switch back to the Dynamics tab": IsDynamicsActive=true → Dynamics AfterStepHooks resumes
///
/// Tab behaviour:
/// - Clicking IPAFFS in the Dynamics ribbon opens IPAFFS in a NEW tab (Browser 2 switches to it).
/// - Clicking "Return to work order" in IPAFFS navigates the CURRENT tab back to Dynamics
///   in-place — no window handle switch is required on the way back.
/// </summary>
[Binding]
public class BrowserTransitionSteps : PowerAppsStepDefiner
{
    private const string IpaffsDomainFragment = "defra.cloud";
    private const string DynamicsDomainFragment = "dynamics.com";

    private readonly IObjectContainer _objectContainer;
    private readonly ScenarioContext _scenarioContext;

    public BrowserTransitionSteps(IObjectContainer objectContainer, ScenarioContext scenarioContext)
    {
        _objectContainer = objectContainer;
        _scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Clicks the IPAFFS command in the Dynamics ribbon, waits for the new tab to open,
    /// waits for Azure AD SSO to complete, then stores the Dynamics driver (focused on the
    /// IPAFFS tab) in ScenarioContext for Assembly 1 to pick up via the hand-off step.
    ///
    /// Sets IsDynamicsActive=false so that WebDriverHook.AfterStep takes over step reporting
    /// for all subsequent IPAFFS steps, using Browser 2's driver on the IPAFFS tab.
    /// </summary>
    [When("I click IPAFFS from the header ribbon")]
    public void WhenIClickIPAFFSFromTheHeaderRibbon()
    {
        var dynamicsDriver = Driver;

        Driver.WaitForTransaction();
        var handlesBefore = dynamicsDriver.WindowHandles.ToList();

        CommandSteps.WhenISelectTheCommand("IPAFFS");
        Driver.WaitForTransaction();

        // Wait for the new IPAFFS tab to open
        var wait = new WebDriverWait(dynamicsDriver, TimeSpan.FromSeconds(30));
        wait.Until(d => d.WindowHandles.Count > handlesBefore.Count);

        // Switch to the new tab
        var ipaffsHandle = dynamicsDriver.WindowHandles.Except(handlesBefore).Single();
        dynamicsDriver.SwitchTo().Window(ipaffsHandle);

        // Wait for the Azure AD SSO redirect chain to land on the IPAFFS host
        var ipaffsWait = new WebDriverWait(dynamicsDriver, TimeSpan.FromSeconds(60));
        ipaffsWait.Until(d =>
            d.Url.Contains(IpaffsDomainFragment, StringComparison.OrdinalIgnoreCase)
            && ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete");

        _scenarioContext["DynamicsWindowHandle"] = handlesBefore.Last();
        _scenarioContext["IpaffsInDynamicsBrowserHandle"] = ipaffsHandle;
        _scenarioContext["DynamicsIpaffsDriver"] = dynamicsDriver;

        // Hand reporting back to WebDriverHook — the next steps interact with IPAFFS
        // via Browser 2's driver on the IPAFFS tab, which WebDriverHook will use for screenshots
        _scenarioContext["IsDynamicsActive"] = false;

        Console.WriteLine($"[BrowserTransition] IPAFFS SSO complete. Driver stored for hand-off. URL: {dynamicsDriver.Url}");
    }

    /// <summary>
    /// Switches reporting ownership back to Dynamics. Handles two distinct cases:
    ///
    /// 1. In-place navigation: "Return to work order" navigates the current tab back to
    ///    Dynamics — no window handle switch is needed. We simply wait for the Dynamics
    ///    host to load on the same tab.
    ///
    /// 2. Tab closed: The IPAFFS tab has been closed (e.g. after logout). The Dynamics
    ///    driver's current handle is no longer valid, so we switch to the stored
    ///    DynamicsWindowHandle before waiting for the page to load.
    /// </summary>
    [When("I switch back to the Dynamics tab")]
    public void WhenISwitchBackToDynamicsTab()
    {
        if (!_scenarioContext.TryGetValue("DynamicsIpaffsDriver", out IWebDriver dynamicsDriver)
            || dynamicsDriver == null)
        {
            throw new InvalidOperationException(
                "No Dynamics IPAFFS driver found in ScenarioContext. " +
                "Ensure 'When I click IPAFFS from the header ribbon' ran before this step.");
        }

        // Determine whether the current window handle is still open.
        // If the IPAFFS tab has been closed, the handle will no longer appear in WindowHandles,
        // so we must switch to the stored Dynamics handle explicitly before waiting for the page.
        var currentHandleIsOpen = IsHandleOpen(dynamicsDriver, dynamicsDriver.CurrentWindowHandle);

        if (!currentHandleIsOpen)
        {
            if (!_scenarioContext.TryGetValue("DynamicsWindowHandle", out string dynamicsHandle)
                || string.IsNullOrWhiteSpace(dynamicsHandle))
            {
                throw new InvalidOperationException(
                    "The current browser window has been closed and no DynamicsWindowHandle was found " +
                    "in ScenarioContext to switch back to. Ensure 'When I click IPAFFS from the header ribbon' " +
                    "ran before this step.");
            }

            dynamicsDriver.SwitchTo().Window(dynamicsHandle);
            Console.WriteLine($"[BrowserTransition] IPAFFS tab was closed. Switched Dynamics driver to handle: {dynamicsHandle}");
        }

        // Wait for the Dynamics page to be fully loaded on whichever tab is now active.
        var wait = new WebDriverWait(dynamicsDriver, TimeSpan.FromSeconds(60));
        wait.Until(d =>
            d.Url.Contains(DynamicsDomainFragment, StringComparison.OrdinalIgnoreCase)
            && ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete");

        Driver.WaitForTransaction();

        // Restore Dynamics reporting ownership for any steps after this point.
        _scenarioContext["IsDynamicsActive"] = true;

        Console.WriteLine($"[BrowserTransition] Dynamics reporting restored. URL: {dynamicsDriver.Url}");
    }

    /// <summary>
    /// Returns true if the given window handle still exists in the driver's open windows.
    /// Uses a safe check rather than relying on the driver throwing when the handle is invalid,
    /// which varies by browser/driver version.
    /// </summary>
    private static bool IsHandleOpen(IWebDriver driver, string handle)
    {
        try
        {
            return driver.WindowHandles.Contains(handle);
        }
        catch
        {
            return false;
        }
    }
}