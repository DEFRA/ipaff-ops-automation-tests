using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ViewOperatorPage : IViewOperatorPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement btnDelete => _driver.WaitForElement(By.XPath("//a[@class='govuk-button govuk-button--warning govuk-!-margin-bottom-0' and contains(@href, '/delete')]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ViewOperatorPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string operatorName)
        {
            return primaryTitle.Text.Trim().Equals(operatorName, StringComparison.OrdinalIgnoreCase);
        }

        public void ClickDelete()
        {
            btnDelete.Click();
        }
    }
}