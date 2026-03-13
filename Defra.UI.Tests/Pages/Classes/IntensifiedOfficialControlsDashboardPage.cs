using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IntensifiedOfficialControlsDashboardPage : IIntensifiedOfficialControlsDashboardPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement btnCreateNewIntensifiedControlCheck => _driver.FindElement(By.Id("create-re-enforced-check"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public IntensifiedOfficialControlsDashboardPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Intensified official controls");
        }

        public void ClickCreateNewIntensifiedControlCheck()
        {
            btnCreateNewIntensifiedControlCheck.Click();
        }

        #endregion
    }
}