using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdentityAndPhysicalChecksPage : IIdentityAndPhysicalChecksPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement rdoSealCheckOnly => _driver.FindElement(By.Id("identyCheckFull5"));
        private IWebElement rdoFullIdentityCheck => _driver.FindElement(By.Id("identyCheckFull6"));
        private IWebElement rdoNotDone => _driver.FindElement(By.Id("identyCheckFull7"));
        private IWebElement rdoSatisfactorySealCheck => _driver.FindElement(By.Id("identyCheck1-seal-identyCheckFull5"));
        private IWebElement rdoNotSatisfactorySealCheck => _driver.FindElement(By.Id("identyCheck2-seal-identyCheckFull5"));
        private IWebElement rdoSatisfactoryFullIdentity => _driver.FindElement(By.Id("identyCheck1-full-identyCheckFull6"));
        private IWebElement rdoNotSatisfactoryFullIdentity => _driver.FindElement(By.Id("identyCheck2-full-identyCheckFull6"));
        private IWebElement rdoReducedChecks => _driver.FindElement(By.Id("identyChecknotDone8-notdone-identyCheckFull7"));
        private IWebElement rdoNotRequired => _driver.FindElement(By.Id("identyChecknotDone9-notdone-identyCheckFull7"));
        private IWebElement rdoChilledEquineSemen => _driver.FindElement(By.Id("identyChecknotDone10-notdone-identyCheckFull7"));
        private IWebElement rdoNotDonePhysicalCheck => _driver.FindElement(By.Id("physicalCheck7"));
        private IWebElement rdoSatisfactoryPhysicalCheck => _driver.FindElement(By.Id("physicalCheck1"));
        private IWebElement rdoNotSatisfactoryPhysicalCheck => _driver.FindElement(By.Id("physicalCheck2"));
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public IdentityAndPhysicalChecksPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Identity and physical checks");
        }


        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }

        public void ClickIdentityCheckOption(string decision, string checkType)
        {
            if (checkType.Equals("Seal check only"))
            {
                rdoSealCheckOnly.Click();
                if (decision.Equals("Satisfactory"))
                    rdoSatisfactorySealCheck.Click();
                else if (decision.Equals("Not Satisfactory"))
                    rdoNotSatisfactorySealCheck.Click();
            }
            else if (checkType.Equals("Full identity check"))
            {
                rdoFullIdentityCheck.Click();
                if (decision.Equals("Satisfactory"))
                    rdoSatisfactoryFullIdentity.Click();
                else if (decision.Equals("Not Satisfactory"))
                    rdoNotSatisfactoryFullIdentity.Click();

            }
            else if (checkType.Equals("Not done"))
            {
                rdoNotDone.Click();
                if (decision.Equals("Reduced checks regime"))
                    rdoReducedChecks.Click();
                else if (decision.Equals("Not required"))
                    rdoNotRequired.Click();
                else if (decision.Equals("Chilled equine semen facilitation scheme"))
                    rdoChilledEquineSemen.Click();
            }
        }

        public void ClickPhysicalCheckDecision(string decision)
        {
            if (decision.Equals("Not done"))
                rdoNotDonePhysicalCheck.Click();
            else if (decision.Equals("Satisfactory"))
                rdoSatisfactoryPhysicalCheck.Click();
            else if (decision.Equals("Not Satisfactory"))
                rdoNotSatisfactoryPhysicalCheck.Click();
        }
    }
}