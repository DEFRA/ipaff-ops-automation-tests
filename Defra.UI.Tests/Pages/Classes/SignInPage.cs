using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SignInPage : ISignInPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl']"), true);
        private IWebElement UserId => _driver.FindElement(By.Id("user_id"));
        private IWebElement Password => _driver.FindElement(By.Id("password"));
        private IWebElement btnSignIn => _driver.WaitForElement(By.Id("continue"), true);
        private By signInConfirmBy => By.XPath("//h1[contains(@class,'govuk-heading-xl')]");
        private By SignInConfirmBy => By.CssSelector("[href='/User/OSignOut']");
        private IWebElement CreateSignInDetails => _driver.WaitForElement(By.XPath("//a[contains(text(),'Create sign in')]"));
        private IWebElement SignOutGCConfirmMessage => _driver.WaitForElement(By.CssSelector("h1.govuk-heading-xl"));
        private IWebElement DynamicsUserId => _driver.WaitForElement(By.XPath("//*[normalize-space(text())='Sign in']/following::input[1]"));
        private IWebElement BtnNext => _driver.WaitForElement(By.XPath("//*[@value='Next']"));
        private IWebElement DynamicsPassword => _driver.WaitForElement(By.XPath("//*[normalize-space(text())='Enter password']/following::input[1]"));
        private IWebElement BtnSignin => _driver.WaitForElement(By.XPath("//*[@value='Sign in']"));
        private IWebElement Signin => _driver.WaitForElement(By.XPath("//*[normalize-space(text()) ='Sign In']"));
        public IWebElement PageHeaderHomePage => _driver.WaitForElement(By.XPath("//h1[normalize-space( text()='Your Defra account')]"), true);
        public IWebElement lnkPetsTravelPortal => _driver.WaitForElement(By.XPath("//a[normalize-space(text())='Taking a pet from Great Britain to Northern Ireland']"), true);
        private IReadOnlyCollection<IWebElement> lblErrorMessages => _driver.WaitForElements(By.XPath("//div[@class='govuk-error-summary__body']//a"));
        private IWebElement signOut => _driver.WaitForElement(By.Id("link-sign-out"));
        private IReadOnlyCollection<IWebElement> rdoLoginTypes => _driver.WaitForElements(By.ClassName("govuk-radios__label"));
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("continueReplacement"));
        private IWebElement txtLoging => _driver.WaitForElement(By.XPath("//input[@id='password']"), true);
        private IWebElement txtIPAFFSIntInspectorUserId => _driver.FindElement(By.Name("loginfmt"));
        private IWebElement txtIPAFFSIntInspectorPassword => _driver.FindElement(By.Name("passwd"));
        private IReadOnlyCollection<IWebElement> txtUser(string user) => _driver.FindElements(By.XPath($"//div[text()='{user}']"));
        private IWebElement txtUserEmailId(string user) => _driver.FindElement(By.XPath($"//div[text()='{user}']"));
        private IWebElement IOCSignOutConfirmMessage => _driver.WaitForElement(By.Id("login_workload_logo_text"), true);
        private By AadPickAccountHeadingBy => By.CssSelector("div[role='heading'][aria-level='1'][data-bind='text: title']");
        private IWebElement AadAccountTile(string userName) => _driver.FindElement(By.CssSelector($"div[role='button'][data-test-id='{userName}']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SignInPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            Thread.Sleep(5000);
            return PageHeading.Text.Contains("Sign in using Government Gateway");
        }

        public void ClickCreateSignInDetailsLink() => CreateSignInDetails.Click();

        public void SignIn(string userName, string password)
        {
            UserId.SendKeys(userName);
            Password.SendKeys(password);
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(btnSignIn)).Click();
        }

        public void IPAFFSInternalInspectorSignIn(string userName, string password)
        {
            _driver.Wait(1);

            if (txtUser(userName).Count() > 0)
                txtUserEmailId(userName).Click();
            else
                txtIPAFFSIntInspectorUserId.SendKeys(userName);

            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(BtnNext)).Click();
            txtIPAFFSIntInspectorPassword.SendKeys(password);
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(BtnSignin)).Click();
        }

        /// <summary>
        /// Handles the AAD "Pick an account" sign-in flow when IPAFFS opens inside
        /// the Dynamics browser. If the account picker is shown, clicks
        /// the tile matching the IDCOMS username. Then enters the password and submits.
        /// SSO may bypass the picker silently, in which case the wait times out gracefully.
        /// </summary>
        public void IPAFFSSignInViaDynamics(string userName, string password)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            try
            {
                wait.Until(d =>
                {
                    var heading = d.FindElement(AadPickAccountHeadingBy);
                    return heading.Text.Trim()
                        .Contains("Pick an account", StringComparison.OrdinalIgnoreCase);
                });

                AadAccountTile(userName).Click();
            }
            catch (WebDriverTimeoutException)
            {
                // "Pick an account" did not appear — AAD SSO handled authentication silently.
            }

            txtIPAFFSIntInspectorPassword.SendKeys(password);
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(BtnSignin)).Click();
        }

        public void EnterPassword()
        {
            try
            {
                if (_driver.IsVisible(By.Id("continue")))
                {
                    btnSignIn.Click();
                }
            }
            catch
            {

            }
            _driver.Wait(2);
            txtLoging.SendKeys(ConfigSetup.BaseConfiguration.TestConfiguration.EnvLogin);
            btnContinue.Click();
        }

        public void ClickSignedOut()
        {
            _driver.WaitForElement(SignInConfirmBy).Click();
        }

        public bool IsSignedOut()
        {
            ClickSignedOut();
            return PageHeading.Text.Contains("You have signed out") || PageHeading.Text.Contains("Your Defra account");
        }

        public bool IsSignedOutFromYourDefraAccountPage()
        {
            signOut.Click();
            return PageHeading.Text.Contains("You have signed out") || PageHeading.Text.Contains("Your Defra account");
        }

        public bool IsSuccessfullySignedOut()
        {
            ClickSignedOut();
            return SignOutGCConfirmMessage.Text.Contains("You need to sign in again");
        }

        public bool IsSignedOutFromIOC()
        {
            return IOCSignOutConfirmMessage.Text.Contains("You signed out of your account");
        }

        public void SignInToDynamics(string username, string password)
        {
            DynamicsUserId.SendKeys(username);
            BtnNext.Click();
            DynamicsPassword.SendKeys(password);
            BtnSignin.Click();
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(50));
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[text() = 'Please sign in again']")));
                Signin.Click();
            }
            catch (NoSuchElementException)
            {

            }
        }

        public void CPSignIn(string userName, string password)
        {
            UserId.SendKeys(userName);
            Password.SendKeys(password);
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(btnSignIn)).Click();
        }

        public void ClickPetsTravelApplicationPortalLink()
        {
            lnkPetsTravelPortal.Click();
        }

        public void ClickSignInButton()
        {
            btnSignIn.Click();
        }

        public bool IsError(string errorMessage)
        {
            string[] error = errorMessage.Split('&');

            foreach (var element in lblErrorMessages)
            {
                if (element.Text.Contains(error[0]) || element.Text.Contains(error[1]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}