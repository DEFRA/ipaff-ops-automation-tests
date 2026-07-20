using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class WhichAnimalCertificationsWillThisCommodityRuleApplyToPage : IWhichAnimalCertificationsWillThisCommodityRuleApplyToPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Which animal certifications will this commodity rule apply to?']"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Continue']"));
        private IWebElement checkboxByLabel(string label) =>
            _driver.WaitForElementExists(By.XPath($"//label[normalize-space()='{label}']/preceding-sibling::input[@type='checkbox'] | //input[@type='checkbox'][following-sibling::label[normalize-space()='{label}']]"));
        #endregion

        public WhichAnimalCertificationsWillThisCommodityRuleApplyToPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Which animal certifications will this commodity rule apply to?");

        public void TickCheckbox(string checkboxLabel)
        {
            var checkbox = checkboxByLabel(checkboxLabel);
            if (!checkbox.Selected)
                checkbox.Click();
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}