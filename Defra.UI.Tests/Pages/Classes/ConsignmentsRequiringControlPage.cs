using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ConsignmentsRequiringControlPage : IConsignmentsRequiringControlPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@class='govuk-heading-xl govuk-!-margin-bottom-6 govuk-!-font-size-48 ']"), true);
        private IWebElement lnkChedRefNumSearcResult => _driver.FindElement(By.XPath("//*[normalize-space()='Reference Number']//following-sibling::dd"));
        private IWebElement lnkChedStatusSearcResult => _driver.FindElement(By.XPath("//*[normalize-space()='CHED status']//following-sibling::dd/strong"));
        private IWebElement lnkChedRefNum => _driver.FindElement(By.XPath("//*[normalize-space()='Reference Number']/following-sibling::dd"));
        private IWebElement lblControlStatus => _driver.FindElement(By.XPath("//*[contains(@id,'control-status')]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ConsignmentsRequiringControlPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Consignments requiring control");
        }

        public bool VerifyNotificationStatus(string chedRef, string status)
        {
            return lnkChedRefNumSearcResult.Text.Trim().Contains(chedRef)
                && lnkChedStatusSearcResult.Text.ToUpper().Trim().Equals(status.ToUpper());
        }

        public void ClickCHEDReferencNum()
        {
            lnkChedRefNum.Click();
        }
        
        public bool VerifyControlStatus(string controlStatus)
        {
            return lblControlStatus.Text.Equals(controlStatus);
        }
    }
}