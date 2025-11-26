namespace Defra.Trade.Plants.SpecFlowBindings.Extensions;

using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extensions to the <see cref="WebClient"/> class.
/// </summary>
public static class WebClientExtensions
{
    public static BrowserCommandResult<bool> SelectTabOnPopup(this WebClient client, string tabName, string subTabName = "", int thinkTime = Constants.DefaultThinkTime)
    {
        return client.Execute(GetOptions($"Select Tab"), driver =>
        {
            IWebElement tabList;
            if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.DialogContext])))
            {
                var dialogContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.DialogContext]));
                tabList = dialogContainer.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TabList]));
            }
            else
            {
                tabList = driver.WaitUntilAvailable(By.XPath("//ul[@aria-label='Work Order Task Form' and contains(@id, 'tablist')]"));
            }

            ClickTab(tabList, AppElements.Xpath[AppReference.Entity.Tab], tabName, driver);

            if (!String.IsNullOrEmpty(subTabName))
            {
                ClickTab(tabList, AppElements.Xpath[AppReference.Entity.SubTab], subTabName, driver);
            }

            driver.WaitForTransaction();
            return true;
        });
    }

    public static void ClickTab(IWebElement tabList, string xpath, string name, IWebDriver driver)
    {
        IWebElement moreTabsButton;
        IWebElement listItem;
        // Look for the tab in the tab list, else in the more tabs menu
        IWebElement searchScope = null;
        if (tabList.HasElement(By.XPath(string.Format(xpath, name))))
        {
            searchScope = tabList;
        }
        else if (tabList.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Entity.MoreTabs]), out moreTabsButton))
        {
            moreTabsButton.Click();

            // No tab to click - subtabs under 'Related' are automatically expanded in overflow menu
            if (name == "Related")
            {
                return;
            }
            else
            {
                searchScope = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.MoreTabsMenu]));
            }
        }

        if (searchScope.TryFindElement(By.XPath(string.Format(xpath, name)), out listItem))
        {
            listItem.Click(true);
        }
        else
        {
            throw new Exception($"The tab with name: {name} does not exist");
        }
    }

    /// <summary>
    /// Sets the value of a Date Field.
    /// </summary>
    /// <param name="field">Date field name.</param>
    /// <param name="value">DateTime value.</param>
    /// <param name="formatDate">Datetime format matching Short Date formatting personal options.</param>
    /// <param name="formatTime">Datetime format matching Short Time formatting personal options.</param>
    /// <example>xrmApp.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
    public static BrowserCommandResult<bool> SetDateTimeFix(this WebClient client, string field, DateTime? value, bool dateOnly, string formatDate = null, string formatTime = null)
    {
        return client.Execute(GetOptions($"Set Date/Time Value: {field}"), driver =>
        {
            driver.WaitForTransaction();

            var container = driver.WaitUntilAvailable(By.XPath("//div[contains(@data-id,'[NAME].fieldControl._datecontrol-date-container')]".Replace("[NAME]", field)), $"Field: {field} does not exist");

            var dateField = container.WaitUntilAvailable(By.XPath(".//input[contains(@id, 'DatePicker')]"), $"Input for {field} does not exist");
            try
            {
                var date = value.HasValue ? formatDate == null ? value.Value.ToShortDateString() : value.Value.ToString(formatDate) : string.Empty;
                driver.RepeatUntil(
                    () =>
                {
                    ClearFieldValue(dateField, client.Browser);
                    dateField.SendKeys(date);
                },
                    d => dateField.GetAttribute("value") == date,
                    new TimeSpan(0, 0, 9), 3
                );
                driver.ClearFocus();
                driver.WaitForTransaction();
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {value}. Actual: {dateField.GetAttribute("value")}", ex);
            }

            // date only fields don't have a time control
            // clearing the date part of a datetime field is enough to clear both
            if (dateOnly || !value.HasValue)
            {
                return true;
            }

            var timeFieldXPath = By.XPath($"//div[contains(@data-id,'{field}.fieldControl._timecontrol-datetime-container')]/div/div/input");
            var timeField = driver.WaitUntilAvailable(timeFieldXPath, TimeSpan.FromSeconds(5), "Time control of datetime field not available");
            try
            {
                var time = value.HasValue ? formatTime == null ? value.Value.ToShortTimeString() : value.Value.ToString(formatTime) : string.Empty;
                driver.RepeatUntil(
                    () =>
                {
                    ClearFieldValue(timeField, client.Browser);
                    timeField.SendKeys(time + Keys.Tab);
                },
                    d => timeField.GetAttribute("value") == time,
                    new TimeSpan(0, 0, 9), 3);
                driver.WaitForTransaction();

            }
            catch (WebDriverTimeoutException ex)
            {
                throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {value}. Actual: {timeField.GetAttribute("value")}", ex);
            }

            return true;
        });
    }

    private static void ClearFieldValue(IWebElement field, InteractiveBrowser browser)
    {
        if (field.GetAttribute("value").Length > 0)
        {
            field.SendKeys(Keys.Control + "a");
            field.SendKeys(Keys.Backspace);
        }
        browser.ThinkTime(2000);
    }

    /// <summary>
    /// Returns the headers for a grid view.
    /// </summary>
    /// <param name="webClient">webClient.</param>
    /// <param name="gridName">gridName.</param>
    /// <returns>List of grid headers.</returns>
    public static BrowserCommandResult<List<string>> GetGridHeaders(this WebClient webClient, string gridName = "")
    {
        try
        {
            return webClient.Execute(GetOptions("Get Grid Headers"), driver =>
            {
                var headersList = new List<string>();
                var scrollPosition = 0;
                while (true)
                {
                    var grid = gridName == string.Empty ? driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.Container])) : GridHelper.GetGrid(driver, gridName);
                    grid.ScrollIntoView(driver);
                    driver.WaitForTransaction();

                    IWebElement columnGroup = grid.WaitUntilAvailable(By.ClassName("ag-header-container"), TimeSpan.FromSeconds(10));
                    IWebElement row;
                    List<IWebElement> rowHeaders = new List<IWebElement>();
                    if (columnGroup != null)
                    {
                        // PowerApps grid
                        row = columnGroup.FindElement(By.ClassName("ag-header-row"));
                        rowHeaders = row.FindElements(By.ClassName("ag-header-cell")).ToList();
                        foreach (var header in rowHeaders)
                        {
                            if (header.GetAttribute("col-id").Contains("__row_status"))
                            {
                                continue;
                            }
                            var rowContent = header.FindElement(By.ClassName("ms-Label"));
                            if (rowContent.Text != string.Empty && !headersList.Contains(rowContent.Text))
                            {
                                headersList.Add(rowContent.Text);
                            }
                        }
                        var scrollBar = grid.FindElement(By.ClassName("ag-body-horizontal-scroll-viewport"));
                        var scrollBarHeight = scrollBar.GetHeight();
                        if (scrollBarHeight == 0)
                        {
                            break;
                        }
                        var difference = columnGroup.GetWidth() - scrollBar.GetWidth();
                        if (scrollPosition >= difference)
                        {
                            break;
                        }
                        scrollPosition = scrollBar.ScrollLeft(driver, scrollBar.GetWidth() / 2);
                    }
                    else if (columnGroup == null)
                    {
                        // Editable grid
                        columnGroup = grid.FindElement(By.ClassName("wj-colheaders"));
                        row = columnGroup.FindElement(By.ClassName("wj-row"));
                        rowHeaders = row.FindElements(By.ClassName("wj-header")).ToList();
                        foreach (var header in rowHeaders)
                        {
                            if (header.GetAttribute("id").Contains("btnheaderselectcolumn"))
                            {
                                continue;
                            }
                            var rowContent = header.GetAttribute("title").ToString();
                            if (rowContent != string.Empty && !headersList.Contains(rowContent))
                            {
                                headersList.Add(rowContent);
                            }
                        }
                        var headerCells = grid.FindElement(By.XPath(".//div[@wj-part='cells']"));
                        var scrollBar = grid.FindElement(By.XPath(".//div[@wj-part='root']"));
                        if (grid.GetWidth() > headerCells.GetWidth())
                        {
                            break;
                        }
                        var difference = (grid.GetWidth() - headerCells.GetWidth()) * -1;
                        if (scrollPosition >= difference)
                        {
                            break;
                        }
                        scrollPosition = scrollBar.ScrollLeft(driver, difference);
                    }
                }
                return headersList;
            });
        }
        catch (Exception ex)
        {
            throw new NotFoundException("Unable to read the grid view headers.", ex);
        }
    }

    private static BrowserCommandOptions GetOptions(string commandName)
    {
        return new BrowserCommandOptions(
            Constants.DefaultTraceSource,
            commandName,
            Constants.DefaultRetryAttempts,
            Constants.DefaultRetryDelay,
            null,
            true,
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException));
    }
}