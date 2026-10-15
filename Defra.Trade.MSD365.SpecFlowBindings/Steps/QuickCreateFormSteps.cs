namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reqnroll;

[Binding]
public sealed class QuickCreateFormSteps : PowerAppsStepDefiner
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IWebElement _formContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuickCreateFormSteps"/> class.
    /// </summary>
    /// <param name="scenarioContext">An instance of a secenario context.</param>
    public QuickCreateFormSteps(ScenarioContext scenarioContext)
    {
        this._scenarioContext = scenarioContext;
        this._formContext = Driver.WaitUntilAvailable(By.XPath("//div[contains(@data-uci-dialog, 'true') and contains(@role, 'dialog')]"));
        Driver.WaitForTransaction();
    }

    [When(@"I can see the '(.*)' quick create form")]
    public void WhenICanSeeTheQuickCreateForm(string quickCreateFormName)
    {
        this._formContext.IsVisible().Should().BeTrue();
    }

    [Then(@"I can see the '(.*)' quick create form")]
    public void ThenICanSeeTheQuickCreateForm(string quickCreateFormName, Table table)
    {
        this._formContext.Should().NotBeNull();

        var headerTitle = this._formContext.FindElement(By.XPath(".//h1[contains(@data-id, 'quickHeaderTitle')]"));
        headerTitle.Should().NotBeNull();
        headerTitle.Text.Should().Be($"Quick Create: {quickCreateFormName}");

        foreach (var row in table?.Rows)
        {
            var control = ControlHelper.FindControl(this._formContext, row["fieldName"]);
            control.Should().NotBeNull();
        }
    }

    [Then(@"the following fields (can|cannot) be edited within the quick create form")]
    public void ThenTheFollowingFieldsCanBeEditedWithinTheQuickCreateForm(string canOrCannot, Table table)
    {
        var canEdit = canOrCannot.Equals("can", StringComparison.InvariantCultureIgnoreCase);
        foreach (var row in table.Rows)
        {
            var control = ControlHelper.FindControl(this._formContext, row["fieldName"]);
            var isReadonly = ControlHelper.IsReadOnly(control);

            if (canEdit)
            {
                isReadonly.Should().BeFalse();
            }
            else
            {
                isReadonly.Should().BeTrue();
            }
        }
    }

    
    [Then(@"I can see the Save And Close command within the quick create form")]
    public void ThenICanSeeTheSaveAndCloseCommandWithinTheQuickCreateForm()
    {
        var commandButton = this._formContext.FindElement(By.XPath(".//button[contains(@data-id, 'quickCreateSaveAndCloseBtn')]"));
        commandButton.Should().NotBeNull();
        commandButton.Text.Should().Be("Save and Close");
    }

    [When(@"I click save and Close button in the quick create")]
    public void WhenIClickSaveAndCloseButtonInTheQuickCreate()
    {
        var commandButton = this._formContext.FindElement(By.XPath(".//button[contains(@data-id, 'quickCreateSaveAndCloseBtn')]"));
        commandButton.Click();
    }


    [Then(@"An error message is displayed in the quick create form '(.*)' is displayed")]
    public void ThenAnErrorMessageIsDisplayedInTheQuickCreateFormIsDisplayed(string message)
    {
        Driver.WaitForTransaction();
        var errorMessage = Driver.WaitUntilVisible(By.XPath($"//span[contains(@data-id, 'error-message')]"));
        errorMessage.Text.Should().Be(message);
    }

    [Then(@"the Quick Create Visit form is displayed")]
    public void ThenTheQuickCreateVisitFormIsDisplayed()
    {
        Driver.WaitForTransaction();

        var headerTitle = Driver.WaitUntilVisible(
            By.XPath("//h1[@data-id='quickHeaderTitle']"),
            TimeSpan.FromSeconds(30),
            "Quick Create form header could not be found.");

        headerTitle.Text.Trim().Should().Be("Quick Create: Visit",
            $"Expected Quick Create form header to be 'Quick Create: Visit' but found '{headerTitle.Text.Trim()}'.");
    }

    [When(@"I add my name to the Quick Create form")]
    public void WhenIAddMyNameToTheQuickCreateForm()
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector", useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var myName = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var nameInput = Driver.WaitUntilAvailable(
            By.XPath("//input[@data-id='trd_name.fieldControl-text-box-text']"),
            "Name field could not be found on the Quick Create form.");

        nameInput.Click();
        nameInput.SendKeys(Keys.Control + "a");
        nameInput.SendKeys(Keys.Delete);
        nameInput.SendKeys(myName);
        nameInput.SendKeys(Keys.Tab);
        Driver.WaitForTransaction();

        this._scenarioContext["VisitName"] = myName;
    }

    /// <summary>
    /// Enters a Date Scheduled at least the given number of days in the future, moving forward
    /// to the next weekday if the calculated date falls on a weekend.
    /// </summary>
    /// <param name="daysInFuture">The minimum number of days in the future.</param>
    [When(@"I add a valid date scheduled to the Quick Create form at least (\d+) days in the future")]
    public void WhenIAddAValidDateScheduledToTheQuickCreateFormAtLeastDaysInTheFuture(int daysInFuture)
    {
        Driver.WaitForTransaction();

        var scheduledDate = DateTime.Today.AddDays(daysInFuture);

        while (scheduledDate.DayOfWeek == DayOfWeek.Saturday || scheduledDate.DayOfWeek == DayOfWeek.Sunday)
        {
            scheduledDate = scheduledDate.AddDays(1);
        }

        var formattedDate = scheduledDate.ToString("dd/MM/yyyy");

        var dateInput = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='trd_datescheduled.fieldControl._datecontrol-date-container']//input"),
            "Date Scheduled input could not be found on the Quick Create form.");

        dateInput.Click();
        dateInput.SendKeys(Keys.Control + "a");
        dateInput.SendKeys(Keys.Delete);
        dateInput.SendKeys(formattedDate);
        dateInput.SendKeys(Keys.Tab);
        Driver.WaitForTransaction();

        // Dismiss the date picker popup if it remained open after tabbing out.
        if (Driver.FindElements(By.XPath("//*[@id='datePicker-popupSurface1']")).Count > 0)
        {
            Driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
            Driver.WaitForTransaction();
        }

        this._scenarioContext["VisitDateScheduled"] = formattedDate;
    }

    [When(@"I add a valid time scheduled to the Quick Create form")]
    public void WhenIAddAValidTimeScheduledToTheQuickCreateForm()
    {
        Driver.WaitForTransaction();

        var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "GMT Standard Time" : "Europe/London");
        var scheduledTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, ukTimeZone).ToString("HH:mm");

        var timeInput = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='trd_datescheduled.fieldControl._timecontrol-datetime-container']//input"),
            "Time Scheduled input could not be found on the Quick Create form.");

        timeInput.Click();
        timeInput.SendKeys(Keys.Control + "a");
        timeInput.SendKeys(Keys.Delete);
        timeInput.SendKeys(scheduledTime);
        timeInput.SendKeys(Keys.Tab);
        Driver.WaitForTransaction();

        this._scenarioContext["VisitTimeScheduled"] = scheduledTime;
    }

    [When(@"I click the Save and Close button on the Quick Create form")]
    public void WhenIClickTheSaveAndCloseButtonOnTheQuickCreateForm()
    {
        Driver.WaitForTransaction();

        var saveAndCloseButton = Driver.WaitUntilClickable(
            By.XPath("//button[@data-id='quickCreateSaveAndCloseBtn']"),
            TimeSpan.FromSeconds(30),
            "Save and Close button could not be found on the Quick Create form.");

        saveAndCloseButton.Click();
        Driver.WaitForTransaction();
    }

    [Then(@"the Quick Create Visit form is closed")]
    public void ThenTheQuickCreateVisitFormIsClosed()
    {
        Driver.WaitForTransaction();

        var deadline = DateTime.UtcNow.AddSeconds(30);
        var headerCount = Driver.FindElements(By.XPath("//h1[@data-id='quickHeaderTitle']")).Count;

        while (headerCount > 0 && DateTime.UtcNow < deadline)
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            headerCount = Driver.FindElements(By.XPath("//h1[@data-id='quickHeaderTitle']")).Count;
        }

        headerCount.Should().Be(0, "Expected the Quick Create: Visit form to be closed but it is still displayed.");
    }
}
