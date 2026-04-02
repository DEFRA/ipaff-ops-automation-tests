namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Extensions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Reqnroll;
using System;
using System.IO;

/// <summary>
/// Step bindings related to logging in.
/// </summary>
[Binding]
public class LoginSteps : PowerAppsStepDefiner
{
    private readonly ScenarioContext scenarioContext;

    public LoginSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Logs in to a given app as a given user.
    /// </summary>
    /// <param name="appName">The name of the app.</param>
    /// <param name="userAlias">The alias of the user.</param>
    //[Scope(Tag = "Trade")]
    [Given("I am logged in to the {string} app as {string}")]
    [When("I am logged in to the {string} app as {string}")]
    public void GivenIAmLoggedInToTheAppAs(string appName, string userAlias)
    {
        SelectApplication(appName);
        GivenIAmLoggedInToTheAppAs1(appName, userAlias);

        scenarioContext["IsDynamicsActive"] = true;
    }

    public static void GivenIAmLoggedInToTheAppAs1(string appName, string userAlias)
    {
        var user = TestConfig.GetUser(userAlias, useCurrentUser: false);

        if (TestConfig.UseProfiles && TestConfig.BrowserOptions.BrowserType.SupportsProfiles())
        {
            SetupScenarioProfile(user.Username);
        }

        Uri url;
        if (appName != "ChargeUI")
            url = TestConfig.GetTestUrl();
        else
            url = TestConfig.GetChargeUIUrl();

        Console.WriteLine("Username logged in with = " + user.Username);

        XrmApp.OnlineLogin.Login(url, user.Username.ToSecureString(), user.Password.ToSecureString());

        Console.WriteLine("Logged-in success with the user =  " + user.Username);

        // Dismiss any 'Sign in to continue' MSAL prompts that appear immediately after login.
        DismissSignInPrompts();

        if (!url.Query.Contains("appid") && appName != "ChargeUI")
        {
            XrmApp.Navigation.OpenApp(appName);
        }

        CloseTeachingBubbles();

        // Dismiss any further prompts triggered by OpenApp() or CloseTeachingBubbles()
        // before handing control back to the test.
        DismissSignInPrompts();
    }

    /// <summary>
    /// Dismisses all 'Sign in to continue' MSAL token refresh prompts that appear after login.
    /// Each prompt is clicked and confirmed closed before the next iteration, handling the case
    /// where multiple prompts appear sequentially during the AAD token refresh cycle.
    /// Also waits for any residual modal overlay to clear before returning.
    /// Times out after 90 seconds if prompts do not resolve.
    /// </summary>
    private static void DismissSignInPrompts()
    {
        var deadline = DateTime.UtcNow.AddSeconds(90);

        while (DateTime.UtcNow < deadline)
        {
            var prompt = Driver.FindElements(By.XPath("//h1[text() = 'Sign in to continue']"));

            if (prompt.Count == 0)
            {
                var modalGone = Driver.FindElements(
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
                Driver.WaitUntilAvailable(
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
                if (Driver.FindElements(By.XPath("//h1[text() = 'Sign in to continue']")).Count == 0)
                {
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }

    private static void CloseTeachingBubbles()
    {
        foreach (var closeButton in Driver.FindElements(By.ClassName("ms-TeachingBubble-closebutton")))
        {
            closeButton.Click();
        }
    }

    private static bool WaitForMainPage(IWebDriver driver, TimeSpan? timeout = null)
    {
        timeout = timeout ?? 10.Seconds();

        var isUCI = driver.HasElement(By.XPath(Elements.Xpath[Reference.Login.CrmUCIMainPage]));
        if (isUCI)
        {
            driver.WaitForTransaction();
        }

        var xpathToMainPage = By.XPath(Elements.Xpath[Reference.Login.CrmMainPage]);
        var element = driver.WaitUntilAvailable(xpathToMainPage, timeout);

        return element != null;
    }

    private static void SetupScenarioProfile(string username)
    {
        var baseProfileDirectory = Path.Combine(UserProfileDirectories[username], "base");
        new DirectoryInfo(baseProfileDirectory).CopyTo(new DirectoryInfo(CurrentProfileDirectory));
    }

    public static void Login(IWebDriver driver, Uri orgUrl, string username, string password)
    {
        driver.Navigate().GoToUrl(orgUrl);
        driver.ClickIfVisible(By.Id("otherTile"));

        bool waitForMainPage = WaitForMainPage(driver);

        if (!waitForMainPage)
        {
            IWebElement usernameInput = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]), 30.Seconds());
            usernameInput.SendKeys(username);
            usernameInput.SendKeys(Keys.Enter);

            IWebElement passwordInput = driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Login.LoginPassword]), 60.Seconds());
            passwordInput.SendKeys(password.ToSecureString().ToString());
            passwordInput.Submit();

            var staySignedIn = driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]), 10.Seconds());
            if (staySignedIn != null)
            {
                staySignedIn.Click();
            }

            try
            {
                driver.WaitUntilAvailable(By.XPath("//h1[text() = 'Please sign in again']"), 30.Seconds());
                driver.WaitUntilAvailable(By.XPath("//*[normalize-space(text()) ='Sign In']")).Click();
            }
            catch (NoSuchElementException e)
            {
                //no action
            }

            WaitForMainPage(driver, 60.Seconds());
        }
    }
}