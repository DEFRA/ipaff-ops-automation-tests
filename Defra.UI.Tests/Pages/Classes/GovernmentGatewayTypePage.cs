using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class GovernmentGatewayTypePage: IGovernmentGatewayTypePage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects

        private IWebElement PageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='How do you want to sign in?']"));

        private IReadOnlyCollection<IWebElement> rdoLoginTypes => _driver.WaitForElements(By.ClassName("govuk-radios__label"));

        private IWebElement btnContinue => _driver.WaitForElement(By.Id("continueReplacement"));
        private IWebElement btnSignIn => _driver.WaitForElement(By.XPath("//button[normalize-space()='Sign in'] | //a[normalize-space()='Sign in']"));

        #endregion Page Objects

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public GovernmentGatewayTypePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string pageName)
        {
            return PageHeading.Text.Contains(pageName);
        }

        public void SelectLoginType(string loginType)
        {
            if (rdoLoginTypes.Count > 0)
            {
                if (loginType.ToUpper().Contains("SIGN IN WITH GOV UK ONE LOGIN")
                    || loginType.ToUpper().Contains("DEFRA SINGLE SIGN-ON"))
                {
                    rdoLoginTypes.ElementAt(0)?.Click();
                }
                else if (loginType.ToUpper().Contains("SIGN IN WITH GOVERNMENT GATEWAY")
                    || loginType.ToUpper().Contains("GOVERNMENT GATEWAY"))
                {
                    rdoLoginTypes.ElementAt(1)?.Click();
                }
            }
        }

        public void ClickContinueButton()
        {
            btnContinue.Click();
        }

        public void ClickSignInButton()
        {
            btnSignIn.Click();
        }
    }
}
