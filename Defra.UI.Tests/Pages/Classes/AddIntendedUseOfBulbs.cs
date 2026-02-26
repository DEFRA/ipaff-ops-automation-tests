using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddIntendedUseOfBulbs : IAddIntendedUseOfBulbs
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);

        private IWebElement chkCommodityCode => _driver.FindElement(By.XPath("//div[contains(@data-target,'hidden-commodity')]"));
        private IWebElement rdoYes => _driver.FindElement(By.XPath("//label[@for='finished-commodity-line']"));
        private IWebElement rdoNo => _driver.FindElement(By.XPath("//label[@for='propagating-commodity-line']"));
        private IWebElement btnApply => _driver.FindElement(By.Id("apply-intended-for-use"));
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        private IWebElement pSuccessMessage => _driver.FindElement(By.Id("number-of-bulk-values-applied"));
        #endregion

        public AddIntendedUseOfBulbs(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Add intended use of bulbs");
        }

        public void SelctCommodityCode()
        {
            chkCommodityCode.Click();
        }

        public void SelectOptionForIntentedFinalUsers(string option)
        {
            switch (option?.Trim().ToUpperInvariant())
            {
                case "YES":
                    rdoYes.Click();
                    break;

                case "NO":
                    rdoNo.Click(); 
                    break;
            }
        }

        public void ClickApplyButton()
        {
            btnApply.Click();
        }

        public bool VerifyMessageOnThePage(string message)
        {
            return pSuccessMessage.Text.Contains(message);
        }

        public void ClickSaveAndContinueButton()
        {
            btnSaveAndContinue.Click();
        }
    }
}
