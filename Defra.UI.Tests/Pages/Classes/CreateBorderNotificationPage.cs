using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class CreateBorderNotificationPage : ICreateBorderNotificationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CreateBorderNotificationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Enter the details of the border notification");
        }

        public void EnterNumberOfPackages(string packages)
        {
            txtNumberOfPackages.Clear();
            txtNumberOfPackages.SendKeys(packages);
        }
    }
}
