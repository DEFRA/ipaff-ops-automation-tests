using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DecisionHubPage : IDecisionHubPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement lnkSaveAndSetAsInProgress => _driver.WaitForElement(By.Id("set-in-progress"));
        private IWebElement txtUpdatedStatus => _driver.WaitForElement(By.Id("Status-Label"));
        private IWebElement lnkLocalRefNum => _driver.WaitForElement(By.XPath("//a[normalize-space()='Local reference number']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public DecisionHubPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Decision Hub");
        }

        public void ClickSaveAndSetAsInProgress()
        {
            lnkSaveAndSetAsInProgress.Click();
        }

        public bool VerifyStatusUpdate(string stausNew, string statusInProgress)
        {
            return txtUpdatedStatus.Text.Trim().Equals(statusInProgress);
        }

        public void ClickLocalReferenceNumberLink()
        {
            lnkLocalRefNum.Click();
        }
    }
}