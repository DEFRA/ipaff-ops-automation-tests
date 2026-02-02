using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class OverrideRiskDecisionPage : IOverrideRiskDecisionPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement btnOverrideRisk => _driver.FindElement(By.Id("submit-button"));
        private IWebElement rdoOverrideAsInspectionRequired => _driver.FindElement(By.Id("override-decision"));
        private IWebElement rdoOverrideAsNoInspection => _driver.FindElement(By.Id("override-decision-2"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public OverrideRiskDecisionPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Override risk decision");
        }

        public void ClickYesOverrideRiskDecisionButton()
        {
            btnOverrideRisk.Click();
        }

        public void ClickOverrideDecisionOption(string option)
        {
            if (option.Equals("Override risk decision as inspection required"))
                rdoOverrideAsInspectionRequired.Click();
            else if (option.Equals("Override risk decision as no inspection"))
                rdoOverrideAsNoInspection.Click();
        }
    }
}