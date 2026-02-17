using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ConfirmExemptSpeciesPage : IConfirmExemptSpeciesPage
    {
        private IObjectContainer _objectContainer;
        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement chkNone => _driver.FindElement(By.Id("select-none-checkbox"));

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #endregion

        public ConfirmExemptSpeciesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string pageTitle)
        {
            return primaryTitle.Text.Trim().Equals(pageTitle, StringComparison.OrdinalIgnoreCase);
        }

        public void SelectSpeciesOption(string option)
        {
            if (option.ToLower().Equals("none of the species are exempt"))
            {
                chkNone.Click();
            }
        }
    }
}
