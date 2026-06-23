using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterYourApplicationsPage : IExporterYourApplicationsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Your applications']"), true);
        private IWebElement btnStartNewApplication => _driver.WaitForElement(By.XPath("//button[normalize-space()='Start a new application'] | //a[normalize-space()='Start a new application']"), true);
        #endregion

        public ExporterYourApplicationsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Your applications");

        public void ClickStartNewApplicationButton() => btnStartNewApplication.Click();
    }
}