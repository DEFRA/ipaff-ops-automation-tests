namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Helper for working with Custom Controls.
/// </summary>
class CustomControlHelper
{
    public static void InputToPenControl(IWebElement formContext, IWebDriver webDriver, string penControlFieldLogicalName)
    {
        IWebElement penControlElement = formContext.FindElement(By.XPath($".//div[@data-id='{penControlFieldLogicalName}.fieldControl_container']"));
        IWebElement confirmButton = penControlElement.FindElement(By.XPath(".//span[@aria-label='Confirm']"));
        Actions actionsProvider = new Actions(webDriver);
        actionsProvider.MoveToElement(penControlElement, 0, 0);

        actionsProvider.ClickAndHold(penControlElement)
            .MoveByOffset(50, 0)
            .MoveByOffset(-25, -50)
            .MoveByOffset(75, 10)
            .Release(penControlElement)
            .MoveToElement(confirmButton, 0, 0)
            .Click()
            .Build()
            .Perform();
    }
}
