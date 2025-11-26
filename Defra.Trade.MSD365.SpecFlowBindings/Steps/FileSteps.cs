// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using System;
using System.Runtime.InteropServices;
using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using OpenQA.Selenium;
using Reqnroll;

[Binding]
public class FileSteps : PowerAppsStepDefiner
{
    private const uint WmClose = 0x0010;

    private readonly SessionContext ctx;

    public FileSteps(SessionContext context)
    {
        this.ctx = context;
    }

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [When("I attach a file '(.*)' in the timeline view")]
    public static void IAttachATimeLineFile(string fileName)
    {
        // Click attach button - this creates the file control
        Driver.FindElement(By.XPath("//div[@role='button' and @aria-label='Add an attachment']")).Click();
        System.Threading.Thread.Sleep(500);

        // Find and close any file browser, in case not Headless
        try
        {
            var dialogHWnd = FindWindow(null, "Open");

            if (dialogHWnd != IntPtr.Zero)
            {
                SendMessage(dialogHWnd, WmClose, IntPtr.Zero, IntPtr.Zero);
            }
        }
        catch
        {
            // we assume non-Windows, so not a developer machine and so nothing to close anyway
        }

        // Send keys to file control
        SharedSteps.WaitForScriptProcessing();

        var fileInput = Driver.FindElement(By.XPath("//input[@id='filepickerid']"));

        // If relative path then relative to the data directory
        if (!fileName.Contains("\\"))
        {
            fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);
        }

        fileInput.SendKeys(fileName);

        // Click Add button
        System.Threading.Thread.Sleep(5000);
        SharedSteps.WaitForScriptProcessing();
        Driver.FindElement(By.Id("create_note_add_btn")).Click();

        // Wait for processing
        System.Threading.Thread.Sleep(5000);
        SharedSteps.WaitForScriptProcessing();
    }

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}
