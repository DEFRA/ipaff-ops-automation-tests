// <copyright file="FieldExtensions.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Extensions;

using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;
using System;
using System.Linq;

/// <summary>
/// <see cref="Field"/> extension methods.
/// </summary>
public static class FieldExtensions
{
    /// <summary>
    /// This is required as the <see cref="Field.IsReadOnly"/> property does not work.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="driver">The <see cref="IWebDriver"/>.</param>
    /// <returns>Whether the field is read-only.</returns>
    public static bool IsReadOnly(this Field field, IWebDriver driver)
    {
        field = field ?? throw new ArgumentNullException(nameof(field));
        driver = driver ?? throw new ArgumentNullException(nameof(driver));
        
        return driver.FindElement(By.XPath($"(//div[contains(@data-id,'{field.Name}')]//following::input)[1]")).GetAttribute("readonly") != null;
    }
}
