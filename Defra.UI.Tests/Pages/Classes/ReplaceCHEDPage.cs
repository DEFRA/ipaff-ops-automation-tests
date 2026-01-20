using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReplaceCHEDPage : IReplaceCHEDPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl govuk-!-margin-bottom-0']"), true);
        private IWebElement btnYesReplaceThisCHED => _driver.WaitForElement(By.Id("replace-certificate"));     
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ReplaceCHEDPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Replace CHED");
        }

        public void ClickYesReplaceThisCHED()
        {
            btnYesReplaceThisCHED.Click();
        }
    }
}