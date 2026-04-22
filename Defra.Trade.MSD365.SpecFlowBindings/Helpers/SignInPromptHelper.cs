// <copyright file="SignInPromptHelper.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using OpenQA.Selenium;
using System;
using System.Threading;

/// <summary>
/// Helper for dismissing Dynamics 365 MSAL sign-in prompts that may appear during test sessions.
/// Handles both "Please sign in again" and "Sign in to continue" dialogs, including stacked instances.
/// If a dialog cannot be dismissed after repeated attempts, the page is refreshed to recover.
/// </summary>
public static class SignInPromptHelper
{
    // Matches either known sign-in dialog title, regardless of the numeric suffix.
    private static readonly By AnySignInDialogLocator = By.XPath(
        "//div[@data-uci-dialog='true'][.//*[@data-id='dialogTitleText' and " +
        "(normalize-space(text())='Please sign in again' or " +
        "normalize-space(text())='Sign in to continue')]]");

    // Targets every okButton that lives inside a sign-in dialog.
    private static readonly By SignInButtonInsideDialogLocator = By.XPath(
        "//div[@data-uci-dialog='true'][.//*[@data-id='dialogTitleText' and " +
        "(normalize-space(text())='Please sign in again' or " +
        "normalize-space(text())='Sign in to continue')]]" +
        "//button[@data-id='okButton']");

    /// <summary>
    /// Dismisses all visible sign-in prompts, waiting until none remain or until the timeout is reached.
    /// Uses a short grace period to catch late-appearing prompts. If a dialog is stuck, refreshes the page.
    /// </summary>
    /// <param name="driver">Selenium WebDriver instance.</param>
    /// <param name="callerContext">Optional label for trace logging.</param>
    /// <param name="timeoutSeconds">Total time to wait for prompts to clear (default: 90s).</param>
    /// <param name="gracePeriodMs">Time to poll for prompt appearance before returning (default: 3000ms).</param>
    /// <param name="stuckThresholdSeconds">Time to keep clicking before refreshing the page (default: 15s).</param>
    public static void DismissSignInPrompts(
        IWebDriver driver,
        string callerContext = "",
        int timeoutSeconds = 90,
        int gracePeriodMs = 3000,
        int stuckThresholdSeconds = 15)
    {
        // Grace-period poll: wait up to gracePeriodMs for a sign-in dialog to appear.
        var graceDeadline = DateTime.UtcNow.AddMilliseconds(gracePeriodMs);
        while (DateTime.UtcNow < graceDeadline)
        {
            if (driver.FindElements(AnySignInDialogLocator).Count > 0)
            {
                break;
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(200));
        }

        // If no prompt appeared during the grace period, nothing to do.
        if (driver.FindElements(AnySignInDialogLocator).Count == 0)
        {
            return;
        }

        // A prompt is present — dismiss all instances within the timeout budget.
        var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);

        // Track when we first saw the current dialog instance so we can detect
        // whether it has become stuck and needs a page refresh to recover.
        var dialogFirstSeen = DateTime.UtcNow;
        var refreshedOnce = false;

        while (DateTime.UtcNow < deadline)
        {
            var dialogs = driver.FindElements(AnySignInDialogLocator);

            if (dialogs.Count == 0)
            {
                return;
            }

            // Stuck-dialog detection: if the dialog has persisted beyond the
            // threshold despite repeated clicks, refresh the page to unblock it.
            // Only refresh once per DismissSignInPrompts call to avoid a loop.
            if (!refreshedOnce && DateTime.UtcNow - dialogFirstSeen > TimeSpan.FromSeconds(stuckThresholdSeconds))
            {
                driver.Navigate().Refresh();
                refreshedOnce = true;

                // Wait for the page to settle after refresh before re-checking.
                Thread.Sleep(TimeSpan.FromSeconds(3));

                // Reset the stuck timer — the refreshed page may show a fresh prompt.
                dialogFirstSeen = DateTime.UtcNow;
                continue;
            }

            // Click every visible sign-in button in one pass — handles stacked dialogs.
            var buttons = driver.FindElements(SignInButtonInsideDialogLocator);
            var clicked = false;

            foreach (var button in buttons)
            {
                try
                {
                    button.Click();
                    clicked = true;
                }
                catch (StaleElementReferenceException)
                {
                    // Dialog closed mid-iteration — re-check on next loop.
                }
                catch (ElementNotInteractableException)
                {
                    // Button not yet interactive — retry on next loop.
                }
            }

            if (!clicked)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(200));
                continue;
            }

            // Wait for dismissed dialogs to close before looping for the next one.
            var clearDeadline = DateTime.UtcNow.AddSeconds(15);
            while (DateTime.UtcNow < clearDeadline)
            {
                if (driver.FindElements(AnySignInDialogLocator).Count == 0)
                {
                    return;
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(300));
            }
        }
    }
}