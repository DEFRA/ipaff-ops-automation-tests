using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class HealthCertificatePage : IHealthCertificatePage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("continue-button"));     
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public HealthCertificatePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Health certificate required");
        }

        
        public void ClickContinue()
        {
            btnContinue.Click();
        }
    }
}