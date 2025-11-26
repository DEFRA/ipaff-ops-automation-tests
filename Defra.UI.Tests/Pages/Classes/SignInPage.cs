using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;

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
        private IWebElement SignIn => _driver.WaitForElement(By.Id("continue"));
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

        public bool IsSignedIn(string userName, string password)
        {
            UserId.SendKeys(userName);
            Password.SendKeys(password);
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(SignIn)).Click();
            return _driver.WaitForElement(SignInConfirmBy).Enabled;
        }

        public void ClickCreateSignInDetailsLink() => CreateSignInDetails.Click();

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
            catch (NoSuchElementException e)
            {

            }

        }

        public void CPSignIn(string userName, string password)
        {
            UserId.SendKeys(userName);
            Password.SendKeys(password);
            _driver.WaitForElementCondition(ExpectedConditions.ElementToBeClickable(SignIn)).Click();
        }

        public void ClickPetsTravelApplicationPortalLink()
        {
            lnkPetsTravelPortal.Click();
        }
        public void ClickSignInButton()
        {
            SignIn.Click();
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