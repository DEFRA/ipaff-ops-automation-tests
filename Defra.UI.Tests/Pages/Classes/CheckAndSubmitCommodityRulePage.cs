using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CheckAndSubmitCommodityRulePage : ICheckAndSubmitCommodityRulePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Check and submit commodity rule']"), true);
        private IWebElement btnConfirmAndSubmitRule => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Confirm and submit rule']"));
        #endregion

        public CheckAndSubmitCommodityRulePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Check and submit commodity rule");

        public void ClickConfirmAndSubmitRuleButton() => btnConfirmAndSubmitRule.Click();
    }
}