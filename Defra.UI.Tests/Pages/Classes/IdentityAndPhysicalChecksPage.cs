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
        private IWebElement txtNumberOfAnimalsChecked => _driver.FindElement(By.Id("number-of-animals-checked"));

        private IWebElement txtNumberOfDeadAnimals => _driver.FindElement(By.Id("number-of-dead-animals"));
        private IWebElement ddlNumberOfDeadAnimalsUnit => _driver.FindElement(By.Id("number-of-dead-animals-unit"));
        private IWebElement txtNumberOfUnfitAnimals => _driver.FindElement(By.Id("number-of-unfit-animals"));
        private IWebElement ddlNumberOfUnfitAnimalsUnit => _driver.FindElement(By.Id("number-of-unfit-animals-unit"));
        private IWebElement txtNumberOfBirthsOrAbortions => _driver.FindElement(By.Id("number-of-birth-or-abortion"));
        // Dynamic elements
        private IWebElement GetIdentityCheckRadioButton(string decision) =>
            _driver.FindElement(
                By.XPath($"//fieldset[.//legend[contains(text(),'Identity check')]]//label[text()='{decision}']/preceding-sibling::input[@type='radio']")
            );
        private IWebElement GetPhysicalCheckRadioButton(string decision) =>
            _driver.FindElement(
                By.XPath($"//fieldset[.//legend[contains(text(),'Physical check')]]//label[text()='{decision}']/preceding-sibling::input[@type='radio']")
            );
        private IWebElement GetWelfareCheckRadioButton(string decision) =>
            _driver.FindElement(
                By.XPath($"//fieldset[.//legend[contains(text(),'Welfare check')]]//label[text()='{decision}']/preceding-sibling::input[@type='radio']")
            );        
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

        public bool IsIdentityPhysicalAndWelfareChecksPageLoaded()
        {
            return primaryTitle.Text.Contains("Identity, physical and welfare checks");
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

        public void SelectIdentityCheck(string decision)
        {
            if (string.IsNullOrEmpty(decision))
                return;

            GetIdentityCheckRadioButton(decision).Click();
        }

        public void SelectPhysicalCheck(string decision)
        {
            if (string.IsNullOrEmpty(decision))
                return;

            GetPhysicalCheckRadioButton(decision).Click();
        }

        public void SelectWelfareCheck(string decision)
        {
            if (string.IsNullOrEmpty(decision))
                return;

            GetWelfareCheckRadioButton(decision).Click();
        }

        public void EnterNumberOfAnimalsChecked(string numberOfAnimals)
        {
            if (!string.IsNullOrEmpty(numberOfAnimals))
            {
                txtNumberOfAnimalsChecked.Clear();
                txtNumberOfAnimalsChecked.SendKeys(numberOfAnimals);
            }
        }

        public void EnterNumberOfDeadAnimals(string numberOfDeadAnimals, string unit)
        {
            if (!string.IsNullOrEmpty(numberOfDeadAnimals))
            {
                txtNumberOfDeadAnimals.Clear();
                txtNumberOfDeadAnimals.SendKeys(numberOfDeadAnimals);
            }

            if (!string.IsNullOrEmpty(unit))
            {
                var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(ddlNumberOfDeadAnimalsUnit);
                if (unit.Equals("%") || unit.Equals("percent"))
                    selectElement.SelectByValue("percent");
                else
                    selectElement.SelectByValue("number");
            }
        }

        public void EnterNumberOfUnfitAnimals(string numberOfUnfitAnimals, string unit)
        {
            if (!string.IsNullOrEmpty(numberOfUnfitAnimals))
            {
                txtNumberOfUnfitAnimals.Clear();
                txtNumberOfUnfitAnimals.SendKeys(numberOfUnfitAnimals);
            }

            if (!string.IsNullOrEmpty(unit))
            {
                var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(ddlNumberOfUnfitAnimalsUnit);
                if (unit.Equals("%") || unit.Equals("percent"))
                    selectElement.SelectByValue("percent");
                else
                    selectElement.SelectByValue("number");
            }
        }

        public void EnterNumberOfBirthsOrAbortions(string numberOfBirths)
        {
            if (!string.IsNullOrEmpty(numberOfBirths))
            {
                txtNumberOfBirthsOrAbortions.Clear();
                txtNumberOfBirthsOrAbortions.SendKeys(numberOfBirths);
            }
        }
    }
}