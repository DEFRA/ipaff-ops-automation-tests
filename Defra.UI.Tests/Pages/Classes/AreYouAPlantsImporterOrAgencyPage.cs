using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AreYouAPlantsImporterOrAgencyPage : IAreYouAPlantsImporterOrAgencyPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement rdoYes => _driver.FindElement(By.XPath("//label[@for='is-plants-organisation']"));
        private IWebElement btnContinue => _driver.FindElement(By.Id("continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AreYouAPlantsImporterOrAgencyPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Manage trade partners")
                && primaryTitle.Text.Contains("Are you a plants importer or agency?");
        }

        public void SelectYesAndClickContinue()
        {
            rdoYes.Click();
            btnContinue.Click();
        }
    }
}