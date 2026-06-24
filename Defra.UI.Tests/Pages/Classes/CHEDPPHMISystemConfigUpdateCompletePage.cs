using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDPPHMISystemConfigUpdateCompletePage : ICHEDPPHMISystemConfigUpdateCompletePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='System Configuration Update Complete']"), true);
        private IWebElement systemConfigCompleteText => _driver.WaitForElement(By.XPath("//div[contains(@class,'govuk-panel--confirmation')]/following-sibling::p"));

        #endregion

        public CHEDPPHMISystemConfigUpdateCompletePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("System Configuration Update Complete");

        public bool VerifySystemConfigCompleteText() => systemConfigCompleteText.Text.Contains("System configuration details have been updated successfully.");

    }
}