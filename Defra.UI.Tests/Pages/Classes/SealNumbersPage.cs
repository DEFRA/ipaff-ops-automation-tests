using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SealNumbersPage : ISealNumbersPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement rdoSealNumbersYes => _driver.FindElement(By.Id("radio-container-seals-yes"));
        private IWebElement rdoSealNumbersNo => _driver.FindElement(By.Id("radio-container-seals-no"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SealNumbersPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Seal numbers");
        }

        public bool IsSealNumbersNoPreselected()
        {
            return rdoSealNumbersNo.GetAttribute("checked") != null;
        }
    }
}