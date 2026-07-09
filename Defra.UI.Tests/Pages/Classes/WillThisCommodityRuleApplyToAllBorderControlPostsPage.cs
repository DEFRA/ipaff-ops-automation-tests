using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class WillThisCommodityRuleApplyToAllBorderControlPostsPage : IWillThisCommodityRuleApplyToAllBorderControlPostsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Will this commodity rule apply to all border control posts?']"), true);
        private IWebElement radioYes => _driver.WaitForElement(By.Id("nestedradioviewmodel_fieldvalue"));
        private IWebElement radioNo => _driver.WaitForElement(By.Id("NestedRadioOptions_1"));
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Continue']"));
        #endregion

        public WillThisCommodityRuleApplyToAllBorderControlPostsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Will this commodity rule apply to all border control posts?");

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