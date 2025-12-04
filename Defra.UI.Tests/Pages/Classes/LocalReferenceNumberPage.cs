using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class LocalReferenceNumberPage : ILocalReferenceNumberPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement inputLocalRefNum => _driver.WaitForElement(By.Id("bip-local-reference-number"));
        private IWebElement txtUpdatedStatus => _driver.WaitForElement(By.Id("Status-Label"));
        private IWebElement lnkLocalRefNum => _driver.WaitForElement(By.XPath("//a[normalize-space()='Local reference number']"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("button-save-and-continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public LocalReferenceNumberPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Local reference number");
        }

        public void EnterLocalReferenceNumber(string customDeclarionRef)
        {
            inputLocalRefNum.Click();
            inputLocalRefNum.Clear();
            inputLocalRefNum.SendKeys(customDeclarionRef);
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }
    }
}