namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
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

    [Then("the Inspector is logged out of Dynamics successfully")]
    public void ThenTheInspectorIsLoggedOutOfDynamicsSuccessfully()
    {
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(Driver, TimeSpan.FromSeconds(30));

        try
        {
            wait.Until(driver =>
            {
                var element = driver.FindElement(
                    By.Id("login_workload_logo_text"));

                return element.Text.Trim()
                    .Contains("You signed out of your account", StringComparison.OrdinalIgnoreCase);
            });
        }
        catch (OpenQA.Selenium.WebDriverTimeoutException)
        {
            throw new InvalidOperationException(
                $"Expected to see 'You signed out of your account' after signing out of Dynamics but the page did not appear within 30 seconds. " +
                $"Current URL: {Driver.Url}");
        }
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

        Console.WriteLine("Logging in with = " + user.Username);

        XrmApp.OnlineLogin.Login(url, user.Username.ToSecureString(), user.Password.ToSecureString());

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-login");

        if (!url.Query.Contains("appid") && appName != "ChargeUI")
        {
            XrmApp.Navigation.OpenApp(appName);
        }

        CloseTeachingBubbles();

        SignInPromptHelper.DismissSignInPrompts(Driver, "post-navigation");
        Console.WriteLine("Logged-in successfully with the user =  " + user.Username);
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