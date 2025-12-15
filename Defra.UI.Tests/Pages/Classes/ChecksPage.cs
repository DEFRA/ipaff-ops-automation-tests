using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ChecksPage : IChecksPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects

        private IWebElement pageHeader => _driver.WaitForElement(By.XPath("//div[@id='checks-page']//h1"), true);
        private IWebElement docCheckRadio(string docCheckOption) => _driver.FindElement(By.XPath($"//div[contains(@class,'document-check-result')]//label[contains(text(),'{docCheckOption}')]"));
        private IWebElement identityCheckRadio(string identityCheckOption) => _driver.FindElement(By.XPath($"//div[contains(@class,'identity-check-result-executed')]//label[contains(text(),'{identityCheckOption}')]"));
        private IWebElement identityCheckSubRadio(string identityCheckSubOption) => _driver.FindElement(By.XPath($"//div[contains(@class,'identity-check-result')]//input[contains(@id,'identyCheck')]/following-sibling::label[contains(text(),'{identityCheckSubOption}')]"));
        private IWebElement physicalCheckRadio(string physicalCheckOption) => _driver.FindElement(By.XPath($"//div[contains(@class,'physical-check')]//label[contains(text(),'{physicalCheckOption}')]"));
        private IWebElement physicalCheckSubRadio(string physicalCheckSubOption) => _driver.FindElement(By.XPath($"//div[contains(@class,'physical-check-result')]//input[contains(@id,'physicalCheck')]/following-sibling::label[contains(text(),'{physicalCheckSubOption}')]"));
        private IWebElement saveAndContinueButton => _driver.FindElement(By.Id("button-save-and-continue"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        public ChecksPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsChecksPageLoaded()
        {
            return pageHeader.Text.Contains("Checks");
        }

        public void SelectDocCheckRadio(string docCheckOption)
        {
            docCheckRadio(docCheckOption).Click();
        }

        public void SelectIdentityCheckRadio(string identityCheckOption)
        {
            identityCheckRadio(identityCheckOption).Click();
        }

        public void SelectIdentityCheckSubRadio(string identityCheckSubOption)
        {
            identityCheckSubRadio(identityCheckSubOption).Click();
        }

        public void SelectPhysicalCheckRadio(string physicalCheckOption)
        {
            physicalCheckRadio(physicalCheckOption).Click();
        }

        public void SelectPhysicalCheckSubRadio(string physicalCheckSubOption)
        {
            physicalCheckSubRadio(physicalCheckSubOption).Click();
        }

        public void ClickSaveAndContinueButton() => saveAndContinueButton.Click();

        #endregion
    }
}