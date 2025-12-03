using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SignOutPage : ISignOutPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement signOut => _driver.WaitForElement(By.Id("sign-out-link"));
        private IWebElement btmsSignOut => _driver.WaitForElement(By.XPath("//a[normalize-space()='Sign out']"));
        private IWebElement logOutPageHeading => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl']"), true);
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SignOutPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public void SignedOut()
        {
            signOut.Click();
        }

        public void BTMSSignOut()
        {
            btmsSignOut.Click();
        }

        public bool VerifySignedOutPage()
        {
            return logOutPageHeading.Text.Contains("You have signed out")
               || logOutPageHeading.Text.Contains("Your Defra account");
        }
    }
}