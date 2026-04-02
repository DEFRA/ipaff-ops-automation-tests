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

        // The 'Sign in to continue' prompt appears multiple times during the MSAL token refresh
        // cycle. The navbar-container can briefly appear between prompts, so it is not a reliable
        // completion signal on its own. Instead, we wait until the navbar is present AND the
        // prompt has been absent for 5 consecutive seconds before proceeding.
        var signInPromptDeadline = DateTime.UtcNow.AddSeconds(90);
        DateTime? promptAbsentSince = null;

        while (DateTime.UtcNow < signInPromptDeadline)
        {
            var signInPrompt = Driver.FindElements(By.XPath("//h1[text() = 'Sign in to continue']"));

            if (signInPrompt.Count > 0)
            {
                // Prompt visible — reset the stability timer and dismiss it.
                promptAbsentSince = null;

                try
                {
                    Driver.WaitUntilAvailable(
                        By.XPath("//*[normalize-space(text()) ='Sign in']"),
                        5.Seconds())
                        ?.Click();
                }
                catch (StaleElementReferenceException)
                {
                    // DOM replaced between locate and click — next iteration re-locates.
                }
                catch (NoSuchElementException)
                {
                    // no action
                }
            }
            else
            {
                // Prompt not visible — start or check the stability timer.
                promptAbsentSince ??= DateTime.UtcNow;

                var navBar = Driver.FindElements(By.XPath("//div[@data-id='navbar-container']"));
                if (navBar.Count > 0 && (DateTime.UtcNow - promptAbsentSince.Value).TotalSeconds >= 5)
                {
                    // Navbar present and prompt has been absent for 5 seconds — login is stable.
                    break;
                }
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        if (!url.Query.Contains("appid") && appName != "ChargeUI")
        {
            XrmApp.Navigation.OpenApp(appName);
        }

        CloseTeachingBubbles();

        // Final safety net — if the shell still hasn't stabilised, a refresh resolves
        // the stuck token refresh state.
        var shellConfirmed = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='navbar-container']"),
            10.Seconds());

        if (shellConfirmed == null)
        {
            Console.WriteLine("App shell did not load after login — refreshing to recover from stuck token refresh.");
            Driver.Navigate().Refresh();
            Driver.WaitForTransaction();
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