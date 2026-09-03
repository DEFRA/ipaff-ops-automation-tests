namespace Capgemini.PowerApps.SpecFlowBindings.Configuration;

using System;
using System.IO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

/// <summary>
/// Extends the EasyRepro <see cref="BrowserOptions"/> class with additonal support for chrome profiles.
/// </summary>
public class BrowserOptionsWithProfileSupport : BrowserOptions, ICloneable
{
    /// <summary>
    /// Gets or sets the directory to use as the user profile.
    /// </summary>
    public string ProfileDirectory { get; set; }

    /// <inheritdoc/>
    public object Clone()
    {
        return this.MemberwiseClone();
    }

    /// <inheritdoc/>
    public override ChromeOptions ToChrome()
    {
        var options = base.ToChrome();

        // Override the EasyRepro default which sets always_open_pdf_externally=true.
        // This causes PDFs opened via window.open or target="_blank" to immediately
        // close their tab and trigger a download instead of rendering in Chrome's
        // built-in PDF viewer — which breaks IPAFFS Show CHED verification steps.
        options.AddUserProfilePreference("plugins.always_open_pdf_externally", false);

        // Suppress "wants to access devices on your local network" permission popup
        // which blocks the Dynamics 365 command bar from loading during automation.
        // Disable both the prompt feature and the underlying private network blocking.
        options.AddArgument("--disable-features=PrivateNetworkAccessPermissionPrompt,BlockInsecurePrivateNetworkRequests");

        // Pre-grant local network access permission for all origins via Chrome
        // content settings so the popup is never shown regardless of Chrome
        // version behaviour on the feature flag above. URL-agnostic via wildcard.
        // Setting value: 1 = Allow, 2 = Block
        options.AddUserProfilePreference(
            "profile.content_settings.exceptions.local_network_access",
            new Dictionary<string, object>
            {
                ["*,*"] = new Dictionary<string, object>
                {
                    ["setting"] = 1,
                },
            });

        if (!string.IsNullOrEmpty(this.ProfileDirectory))
        {
            options.AddArgument($"--user-data-dir={this.ProfileDirectory}");
        }

        return options;
    }

    /// <inheritdoc/>
    public override FirefoxOptions ToFireFox()
    {
        var options = base.ToFireFox();

        if (!string.IsNullOrEmpty(this.ProfileDirectory))
        {
            this.ProfileDirectory = this.ProfileDirectory.EndsWith("firefox") ? this.ProfileDirectory : Path.Combine(this.ProfileDirectory, "firefox");
            options.AddArgument($"-profile \"{this.ProfileDirectory}\"");
        }

        return options;
    }
}
