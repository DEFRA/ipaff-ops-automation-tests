namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using OpenQA.Selenium.Interactions;
using Reqnroll;

/// <summary>
/// Contains a number of useful steps to perform keyboard actions on the browser.
/// </summary>
[Binding]
public class KeyboardSteps : PowerAppsStepDefiner
{
    /// <summary>
    /// Presses the escape key, useful for closing date time pickers and other popups.
    /// </summary>
    [When("I press the escape key")]
    public void WhenIPressTheEscapeKey()
    {
        Actions pressKey = new Actions(Driver);
        pressKey.SendKeys(OpenQA.Selenium.Keys.Escape).Perform();
    }
}
