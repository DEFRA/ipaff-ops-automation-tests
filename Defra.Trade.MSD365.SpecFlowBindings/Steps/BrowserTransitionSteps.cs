namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using Reqnroll.BoDi;

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
    /// then registers the Dynamics WebDriver into the IoC container (overwriting
    /// WebDriverHook's original driver) so all subsequent IPAFFS page classes
    /// resolve it and operate on the correct IPAFFS tab inside the Dynamics browser.
    /// This registration is only attempted once — before any page class has resolved
    /// IWebDriver — so BoDi does not throw on a duplicate registration.
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

        // Replace the original WebDriverHook.Driver in the IoC container with the
        // Dynamics driver (now focused on the IPAFFS tab). This must happen before
        // any IPAFFS page class resolves IWebDriver for the first time — BoDi allows
        // registration until first resolution, then locks the registration key.
        // The @Dynamics tag on the scenario keeps WebDriverHook from launching
        // Browser 1 at all, so IWebDriver is still unresolved at this point.
        _objectContainer.RegisterInstanceAs<IWebDriver>(Driver);

        // Signal to AfterStepHooks that we are now executing IPAFFS steps
        _scenarioContext["IsDynamicsActive"] = false;

        Console.WriteLine($"Switched Dynamics driver to IPAFFS tab: {newHandle}. IPAFFS step definitions will now use this driver.");
    }

    /// <summary>
    /// Switches the Dynamics driver back to the Dynamics tab.
    /// No re-registration is needed: the container already holds the Dynamics driver
    /// as the IWebDriver instance. Switching the active window on that same instance
    /// is immediately visible to all consumers — both Dynamics and IPAFFS step definitions
    /// share the same registered object reference.
    /// </summary>
    [When("I switch back to the Dynamics tab")]
    [Then("I switch back to the Dynamics tab")]
    public void WhenISwitchBackToDynamicsTab()
    {
        var dynamicsHandle = _scenarioContext.Get<string>("DynamicsWindowHandle");

        // The registered IWebDriver IS the Dynamics driver. Switching the tab
        // on it is sufficient — no re-registration required and BoDi won't throw.
        Driver.SwitchTo().Window(dynamicsHandle);

        // Restore Dynamics active flag so AfterStepHooks resumes Dynamics reporting
        _scenarioContext["IsDynamicsActive"] = true;

        Console.WriteLine($"Switched back to Dynamics tab: {dynamicsHandle}.");
    }
}