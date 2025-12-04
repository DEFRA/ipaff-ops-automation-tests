using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class BillingDetailsPage : IBillingDetailsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IReadOnlyCollection<IWebElement> primaryTitle => _driver.WaitForElements(By.Id("page-primary-title"), true);
        private IReadOnlyCollection<IWebElement> secondaryTitle => _driver.WaitForElements(By.Id("page-secondary-title"), true);
        private IReadOnlyCollection<IWebElement> btnSaveAndContinueList => _driver.WaitForElements(By.XPath("//button[text()='Save and continue']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public BillingDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            if (secondaryTitle.Count > 0 && primaryTitle.Count > 0)
            {
                return secondaryTitle.FirstOrDefault().Text.Contains("Billing")
                    && primaryTitle.FirstOrDefault().Text.Contains("Confirm billing details");
            }
            return true;
        }

        public void ClickSaveAndContinue()
        {
            if (btnSaveAndContinueList.Count > 0)
            {
                btnSaveAndContinueList.FirstOrDefault().Click();
            }
        }
    }
}