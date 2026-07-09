using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DoesThisCommodityRuleHaveAStartOrEndDatePage : IDoesThisCommodityRuleHaveAStartOrEndDatePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Does this commodity rule have a start or end date?']"), true);
        private IWebElement radioYes => _driver.WaitForElementExists(By.Id("yes"));
        private IWebElement radioNo => _driver.WaitForElementExists(By.Id("no"));
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Continue']"));
        #endregion

        public DoesThisCommodityRuleHaveAStartOrEndDatePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Does this commodity rule have a start or end date?");

        public void SelectRadioButton(string option)
        {
            if (option.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                radioYes.Click();
            else
                radioNo.Click();
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}