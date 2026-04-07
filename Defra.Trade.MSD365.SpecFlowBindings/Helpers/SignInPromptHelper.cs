// <copyright file="SignInPromptHelper.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using OpenQA.Selenium;
using System;
using System.Threading;

/// <summary>
/// Helper for dismissing the two Dynamics 365 MSAL sign-in prompts that can
/// appear at any point during a test session:
///
///   1. "Please sign in again"  – appears immediately after credentials are
///      submitted; must be clicked at once or the dialog gets stuck.
///   2. "Sign in to continue"   – appears when an app component triggers a
///      silent token refresh that requires interaction.
///
/// Both dialogs share the same <c>data-id="okButton"</c> attribute on their
/// dismiss button and <c>data-uci-dialog="true"</c> on their root element.
/// Multiple instances can be stacked simultaneously (Dynamics increments the
/// numeric suffix: _2, _3, …), so every visible button is clicked each pass.
///
/// Stuck-dialog recovery: if clicking the Sign In button repeatedly fails to
/// dismiss the dialog within <c>stuckThresholdSeconds</c>, the page is
/// refreshed. After refresh the dialog either disappears or reappears in a
/// dismissable state and the loop resumes.
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
    /// Dismisses all visible sign-in prompts and waits until none remain.
    /// <para>
    /// Uses a short <paramref name="gracePeriodMs"/> poll on first entry to catch
    /// prompts that appear slightly after a navigation action triggers a token
    /// refresh — avoiding the race condition where the prompt does not yet exist
    /// when the method is first called but appears moments later.
    /// </para>
    /// <para>
    /// Returns immediately after the grace period if no prompt appears at all,
    /// so callers incur only a small fixed delay rather than a full timeout.
    /// </para>
    /// <para>
    /// If the dialog becomes stuck (repeated clicks fail to dismiss it within
    /// <paramref name="stuckThresholdSeconds"/>), the page is refreshed and the
    /// dismiss loop resumes — matching the manual recovery of pressing F5.
    /// </para>
    /// </summary>
    /// <param name="driver">The Selenium WebDriver instance.</param>
    /// <param name="callerContext">Optional label for trace logging.</param>
    /// <param name="timeoutSeconds">
    /// Total time budget for all prompts to clear once one is detected. Defaults to 90 seconds.
    /// </param>
    /// <param name="gracePeriodMs">
    /// How long to poll waiting for a prompt to appear before giving up and
    /// returning. Defaults to 3000 ms.
    /// </param>
    /// <param name="stuckThresholdSeconds">
    /// How long to keep clicking before deciding the dialog is stuck and
    /// triggering a browser refresh. Defaults to 15 seconds.
    /// </param>
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