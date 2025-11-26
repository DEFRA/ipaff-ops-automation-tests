namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Linq;

/// <summary>
/// Helper class for use with the Header.
/// </summary>
public class HeaderHelper
{
    private readonly IWebDriver driver;

    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderHelper"/> class.
    /// </summary>
    /// <param name="driver">Selenium IWebDriver</param>
    public HeaderHelper(IWebDriver driver)
    {
        this.driver = driver;
    }

    /// <summary>
    /// Get a header field from the header flyout.
    /// </summary>
    /// <param name="controlName">Name of the control to return.</param>
    /// <returns>IWebElement</returns>
    public IWebElement GetHeaderField(string controlName)
    {
        IWebElement webElement = null;
        var xpathToContainer = $"//*[text()=\"{controlName}\"]";

        TryExpandHeaderFlyout(driver);

        var xpathToFlyout = AppElements.Xpath[AppReference.Entity.Header.Flyout];
        driver.WaitUntilVisible(By.XPath(xpathToFlyout), TimeSpan.FromSeconds(5),
            flyout =>
            {
                webElement = flyout.FindElement(By.XPath(xpathToContainer));
            });

        return webElement;
    }

    /// <summary>
    /// Checks if a field in the header is readonly.
    /// </summary>
    /// <param name="controlName">Name of the control to return.</param>
    /// <returns>True if the field is readonly.</returns>
    public bool IsHeaderFieldReadonly(string controlName)
    {
        var webElement = GetHeaderField(controlName);
        if (webElement == null)
        {
            throw new NullReferenceException($"Field '{controlName}' could not be found in the header.");
        }

        bool isReadOnly = false;

        var xpathToContainer = $"//div[contains(@id,\"header_statuscode.fieldControl-pcf-container-id\")]";

        TryExpandHeaderFlyout(driver);

        var xpathToFlyout = AppElements.Xpath[AppReference.Entity.Header.Flyout];
        driver.WaitUntilVisible(By.XPath(xpathToFlyout), TimeSpan.FromSeconds(10),
            flyout =>
            {
                isReadOnly = flyout.FindElements(By.XPath(xpathToContainer)).Any();
            });

        return isReadOnly;
    }

    internal void TryExpandHeaderFlyout(IWebDriver driver)
    {
        driver.WaitUntilAvailable(
            By.XPath(AppElements.Xpath[AppReference.Entity.Header.Container]),
            "Unable to find header on the form");

        var xPath = By.XPath(AppElements.Xpath[AppReference.Entity.Header.FlyoutButton]);
        var headerFlyoutButton = driver.FindElement(xPath);
        var expanded = bool.Parse(headerFlyoutButton.GetAttribute("aria-expanded"));

        if (!expanded)
            headerFlyoutButton.Click(true);
    }
}
