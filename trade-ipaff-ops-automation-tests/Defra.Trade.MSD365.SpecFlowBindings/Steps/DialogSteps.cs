// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using Reqnroll;

[Binding]
public class DialogSteps : PowerAppsStepDefiner
{
    private readonly ScenarioContext _scenarioContext;

    public DialogSteps(ScenarioContext scenarioContext)
    {
        this._scenarioContext = scenarioContext;
    }

    [Then(@"(an alert|a confirm) dialog is displayed with the message '(.*)'")]
    public void ThenDialogIsDisplayedWithTheMessage(string dialogType, string expectedMessage)
    {
        Driver.WaitForTransaction();
        var type = dialogType.Split(' ')[1];
        var alertDialog = Driver.WaitUntilVisible(By.CssSelector($"div[data-id={type}dialog]"), $"Unable to find {type} dialog.");

        var actualMessage = alertDialog.FindElement(By.Id("dialogMessageText")).Text;

        actualMessage.Should().Be(expectedMessage);
    }

    [When(@"I close the alert dialog")]
    public static void WhenICloseTheAlertDialog()
    {
        Driver.WaitForTransaction();
        IWebElement button = null;

        try
        {
            button = Driver.FindElement(By.Id("okButton"));
        }
        catch
        {
            button = Driver.FindElement(By.Id("cancelButton"));
        }

        button.Click();
    }

    [When(@"I click the Close button on the dialog")]
    public static void WhenIClickCloseOnDialog()
    {  
        var diagCloseButton = Driver.FindElement(By.XPath($"//button[@id='dialogCloseIconButton']"));
        diagCloseButton.Click();
    }

    [When(@"I click the Cancel button on the dialog")]
    public static void WhenIClickCancelOnDialog()
    {
        var diagCancelButton = Driver.WaitUntilAvailable(By.Id("cancelButtonTextName"));
        diagCancelButton.Click();
    }

    [When(@"I click the '(Deactivate|OK)' button on the dialog")]
    public static void WhenIClickTheButtonOnTheDialog(string buttonName)
    {
        //if (buttonName.ToLower() == "deactivate" || buttonName.ToLower() == "ok" || buttonName.ToLower() == "activate")
        //{
        //    Driver.ClickWhenAvailable(By.XPath($"//button[@data-id='ok_id']"), "Unable to click the Deactivate button in the dialog");
        //    Driver.WaitForTransaction();
        //}
        if (buttonName.ToLower() == "deactivate" || buttonName.ToLower() == "ok" || buttonName.ToLower() == "activate")
        {
            Driver.ClickWhenAvailable(By.XPath($"//button[@data-id='ok_id']"), "Unable to click the Deactivate button in the dialog");
            Driver.WaitForTransaction();
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    [When(@"I click the '(confirm|cancel)' button on the dialog")]
    public static void WhenIClickTheAlertButtonOnTheDialog(string buttonName)
    {
        if (buttonName.ToLower() == "confirm" || buttonName.ToLower() == "cancel" || buttonName.ToLower() == "ok")
        {
            Driver.ClickWhenAvailable(By.XPath($"//button[@data-id='{buttonName}Button']"));
            Driver.WaitForTransaction();
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    [Scope(Tag = "Trade")]
    [Given("I assign to me on the assign dialog")]
    [When("I assign to me on the assign dialog")]
    public static void WhenIAssignToMeOnTheAssignDialog()
    {
        XrmApp.Dialogs.Assign(Dialogs.AssignTo.Me);
        Driver.WaitForTransaction();
    }

    [When(@"I cancel the Assign dialog")]
    public static void CancelAssign()
    {
        // Click Assign
        Driver.ClickWhenAvailable(
            By.XPath("//button[@data-id='cancel_id']"),
            TimeSpan.FromSeconds(5),
            "Unable to click the Cancel button in the assign dialog");
    }

    [Then(@"The assign dialog defaults to (Me|User or Team)")]
    public static void ThenTheAssignDialogDefaultsTo(string expectedDefault)
    {
        var xpathToToggleButton = By.XPath(AppElements.Xpath[AppReference.Dialogs.AssignDialogToggle]);
        var toggleButton = Driver.WaitUntilClickable(xpathToToggleButton, "Me/UserTeam toggle button unavailable");

        toggleButton.Text.Should().Be(expectedDefault);
    }

    [Then(@"a reject application dialog is displayed with message '(.*)'")]
    public void ThenARejectApplicationDialogIsDisplayedWithMessage(string message)
    {
        this.GetRejectApplicationDialogHeadTextValue().Should().Be(message);
    }

    [Then(@"a dialog is displayed with message '(.*)' with buttons (.*)")]
    public void ThenADialogIsDisplayedWithMessageWithButtonsAsConfirmReset(string message, string buttonDisplayTexts)
    {
        ThenADialogIsDisplayedWithMessageInTheDialogField(message, "dialogMessageText");
        var buttons = buttonDisplayTexts.Split(',');
        buttons.Length.Should().Be(2);
        ThenADialogIsDisplayedWithMessageInTheDialogField(buttons[0], "confirmButtonText");
        ThenADialogIsDisplayedWithMessageInTheDialogField(buttons[1], "cancelButtonTextName");
    }

    [Then(@"a dialog is displayed with message '(.*)' in the '(.*)' dialog field")]
    public static void ThenADialogIsDisplayedWithMessageInTheDialogField(string expectedDialogMessageValue, string dialogId)
    {
        var dialogValue = GetDialogTextValue(dialogId);

        dialogValue.Should().Be(expectedDialogMessageValue);
    }

    [Then(@"a dialog is displayed with message '(.*)'")]
    public void ThenADialogIsDisplayedWithMessage(string expectedDialogHeadMessageValue)
    {
        var dialogValue = this.GetDialogHeadTextValue();

        dialogValue.Should().Be(expectedDialogHeadMessageValue);
    }

    [Then(@"a dialog is displayed with sub message '(.*)'")]
    public static void ThenADialogIsDisplayedWithSubMessage(string expectedDialogHeadMessageValue)
    {
        var dialogValue = GetDialogSubMessage();

        dialogValue.Should().Be(expectedDialogHeadMessageValue);
    }

    [When(@"I click OK on the reject dialog")]
    public static void WhenIClickOKOnTheRejectDialog()
    {
        Driver.WaitUntilAvailable(By.Id("okButton")).Click();
    }

    [Then(@"a reject dialog is displayed with message '(.*)'")]
    public void ThenARejectDialogIsDisplayedWithMessage(string expectedDialogHeadMessageValue)
    {
        var dialogValue = this.GetRejectDialogHeadTextValue();

        dialogValue.Should().Be(expectedDialogHeadMessageValue);
    }

    [When(@"I click OK on the reject application dialog")]
    public void WhenIClickOKOnTheRejectApplicationDialog()
    {
        //Driver.WaitUntilAvailable(By.Id("confirmButton")).Click();
        Driver.WaitUntilAvailable(By.XPath("//*[@data-id='confirmButton']")).Click();
    }

    [When(@"I click Cancel on the reject application dialog")]
    [When(@"I click Cancel on the command's application dialog")]
    [When(@"I click OK on the Business Process error")]
    public static void WhenIClickCancelOnTheRejectApplicationDialog()
    {
        Driver.WaitUntilAvailable(By.Id("cancelButton")).Click();
    }

    [Then(@"a fail dialog is displayed with message '(.*)'")]
    public void ThenAFailDialogIsDisplayedWithMessage(string message)
    {
        this.GetFailApplicationDialogHeadTextValue().Should().Be(message);
    }

    [Then(@"a fail dialog question is displayed with message '(.*)'")]
    public void ThenAFailDialogQuestionIsDisplayedWithMessage(string message)
    {
        this.GetFailApplicationDialogQuestionHeadTextValue().Should().Be(message);
    }

    /// <summary>
    /// Clicks the button within the dialog window.
    /// </summary>
    /// <param name="buttonLabel">The label of the button to be clicked.</param>
    [When(@"I click the '(.*)' button within the dialog")]
    public void WhenIClickTheButtonWithinTheDialog(string buttonLabel)
    {
        IWebElement dialogContext = this.GetDialogContext();
        dialogContext.WaitUntilClickable(By.XPath($".//button[@aria-label='{buttonLabel}']"));
        var button = dialogContext.FindElement(By.XPath($".//button[@aria-label='{buttonLabel}']"));
        button.Click();
        Driver.WaitForTransaction();
    }

    private string GetDialogHeaderText(IWebElement dialogContext)
    {
        var dialogHeader = dialogContext.FindElement(By.Id("dialogTitleText"));
        return dialogHeader.GetAttribute("aria-label");
    }

    public IWebElement DialogHeadText => Driver.WaitUntilAvailable(By.XPath("//h1[@aria-label='Accept Application']"));

    public IWebElement RejectDialogHeadText => Driver.WaitUntilAvailable(By.XPath("//h1[@aria-label='You cannot reject this Application']"));

    public IWebElement RejectApplicationDialogHeadText => Driver.WaitUntilAvailable(By.XPath("//h1[@aria-label='Reject Application']"));

    public IWebElement FailDialogHeadText => Driver.WaitUntilAvailable(By.XPath("//h1[@aria-label='You cannot fail this application']"));

    public IWebElement FailDialogQuestionHeadText => Driver.WaitUntilAvailable(By.XPath("//Span[text() ='Are you sure you want to fail this application?']"));

    public static string GetDialogTextValue(string dialogTextName)
    {
        Driver.WaitForTransaction();
        var dialogMsg = Driver.WaitUntilAvailable(By.XPath($"//span[@data-id='{dialogTextName}']"));
        var dialogValue = dialogMsg.Text;
        return dialogValue;
    }
    public void VerifyDialogMessageValue(string dialogTextName)
    {
        Driver.WaitForTransaction();
        var dialogMsg = Driver.WaitUntilAvailable(By.XPath($"//h2[text()='{dialogTextName}']"));
        var dialogValue = dialogMsg.Text;
        dialogValue.Should().Be(dialogTextName);
    }
    public void VerifyDialogMessage(string dialogTextName)
    {
        Driver.WaitForTransaction();
        var dialogMsg = Driver.WaitUntilAvailable(By.XPath($"//span[text()='{dialogTextName}']"));
        var dialogValue = dialogMsg.Text;
        dialogValue.Should().Be(dialogTextName);
    }
    public static string GetDialogSubMessage()
    {
        Driver.WaitForTransaction();
        var dialogMsg = Driver.WaitUntilAvailable(By.XPath($"//h2[@data-id='errorDialog_subtitle']"));
        var dialogValue = dialogMsg.Text;
        return dialogValue;
    }

    public string GetDialogHeadTextValue()
    {
        var dialogHeadValue = this.DialogHeadText.Text;
        return dialogHeadValue;
    }

    public string GetRejectDialogHeadTextValue()
    {
        var dialogHeadValue = this.RejectDialogHeadText.Text;
        return dialogHeadValue;
    }

    public string GetRejectApplicationDialogHeadTextValue()
    {
        var dialogHeadValue = this.RejectApplicationDialogHeadText.Text;
        return dialogHeadValue;
    }

    public string GetFailApplicationDialogHeadTextValue()
    {
        var dialogHeadValue = this.FailDialogHeadText.Text;
        return dialogHeadValue;
    }

    public string GetFailApplicationDialogQuestionHeadTextValue()
    {
        var dialogHeadValue = this.FailDialogQuestionHeadText.Text;
        return dialogHeadValue;
    }

    private IWebElement GetDialogContext()
    {
        Driver.WaitUntilAvailable(By.XPath("//div[contains(@data-id, 'dialog')]"));
        if (Driver.TryFindElement(By.XPath("//div[contains(@data-id, 'confirmdialog')]"), out var confirmDialog))
        {
            return confirmDialog;
        }

        if (Driver.TryFindElement(By.XPath("//div[contains(@data-id, 'alertdialog')]"), out var alertDialog))
        {
            return alertDialog;
        }

        if (Driver.TryFindElement(By.XPath("//div[contains(@data-id, 'errorDialogdialog')]"), out var errorDialog))
        {
            return errorDialog;
        }

        throw new NoSuchElementException("Cannot find an alert, confirm or error dialog");
    }
}
