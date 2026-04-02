// <copyright file="SignInPromptHelper.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Threading;

/// <summary>
/// Helper for dismissing MSAL 'Sign in to continue' token refresh prompts
/// that can appear during any Dynamics 365 navigation event.
/// </summary>
public static class SignInPromptHelper
{
    /// <summary>
    /// Dismisses all 'Sign in to continue' MSAL token refresh prompts and waits
    /// for each one to fully close before proceeding. Loops for up to 90 seconds
    /// to handle multiple prompts appearing sequentially during the AAD token
    /// refresh cycle. Also waits for any residual modal overlay to clear before
    /// returning. Times out silently after 90 seconds.
    /// </summary>
    /// <param name="driver">The Selenium WebDriver instance.</param>
    /// <param name="callerContext">Optional label used to prefix log output for traceability.</param>
    public static void DismissSignInPrompts(IWebDriver driver, string callerContext = "")
    {
        var deadline = DateTime.UtcNow.AddSeconds(90);

        while (DateTime.UtcNow < deadline)
        {
            var prompt = driver.FindElements(By.XPath("//h1[text() = 'Sign in to continue']"));

            if (prompt.Count == 0)
            {
                var modalGone = driver.FindElements(
                    By.XPath("//div[contains(@id,'modalDialogRoot')]")).Count == 0;

                if (modalGone)
                {
                    return;
                }

                // Modal still present — wait briefly in case a further prompt is about to appear.
                Thread.Sleep(TimeSpan.FromSeconds(2));
                continue;
            }

            try
            {
                driver.WaitUntilAvailable(
                    By.XPath("//*[normalize-space(text()) ='Sign in']"),
                    5.Seconds())
                    ?.Click();
            }
            catch (StaleElementReferenceException)
            {
                // DOM replaced mid-click — next iteration re-locates.
                continue;
            }
            catch (NoSuchElementException)
            {
                continue;
            }

            // Wait for this prompt instance to fully disappear before looping again.
            var promptGoneDeadline = DateTime.UtcNow.AddSeconds(15);
            while (DateTime.UtcNow < promptGoneDeadline)
            {
                if (driver.FindElements(By.XPath("//h1[text() = 'Sign in to continue']")).Count == 0)
                {
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}