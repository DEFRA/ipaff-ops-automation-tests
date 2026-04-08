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
/// </summary>
[Binding]
public class BrowserTransitionSteps : PowerAppsStepDefiner
{
    private const string IpaffsDomainFragment = "defra.cloud";

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

        Console.WriteLine($"[BrowserTransition] IPAFFS SSO complete. Driver stored for hand-off. URL: {dynamicsDriver.Url}");
    }

    /// <summary>
    /// Switches the Dynamics driver back to the original Dynamics tab so subsequent
    /// Dynamics step definitions continue to operate on the correct window.
    /// </summary>
    [When("I switch back to the Dynamics tab")]
    public void WhenISwitchBackToDynamicsTab()
    {
        if (!_scenarioContext.TryGetValue("DynamicsWindowHandle", out string dynamicsHandle)
            || string.IsNullOrWhiteSpace(dynamicsHandle))
        {
            throw new InvalidOperationException(
                "No Dynamics window handle found in ScenarioContext. " +
                "Ensure 'When I click IPAFFS from the header ribbon' ran before this step.");
        }

        Driver.SwitchTo().Window(dynamicsHandle);

        Console.WriteLine($"[BrowserTransition] Switched Dynamics driver back to handle: {dynamicsHandle}");
    }
}