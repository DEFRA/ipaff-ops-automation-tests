using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ControlRecordedPage : IControlRecordedPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk')]"), true);
        private IWebElement chedReference => _driver.WaitForElement(By.Id("reference-number"));
        private IWebElement chedReferenceVersion => _driver.WaitForElement(By.Id("version-number"));
        private IWebElement outcomeValue => _driver.WaitForElement(By.XPath("//*[@class='govuk-!-font-weight-bold']"));
        private IWebElement btnViewOrPrintCHED => _driver.WaitForElement(By.Id("view-certificate"));
        private IWebElement lnkReturnToDashboard => _driver.WaitForElement(By.Id("manage-notifications"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ControlRecordedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Your control has been recorded");
        }
        public string GetCHEDReferenceWithVersion()
        {
            Console.WriteLine("CHED Reference: " + chedReference.Text.Trim());
            Console.WriteLine("CHED Reference version: " + chedReferenceVersion.Text.Trim());
            var finalChedReference = chedReference.Text.Trim() + chedReferenceVersion.Text.Trim();
            return finalChedReference;
        }

        public string GetOutcome()
        {
            Console.WriteLine("Record control Outcome value: " + outcomeValue.Text.Trim());
            return outcomeValue.Text.Trim();
        }

        public bool IsViewOrPrintCHEDButtonDisplayed()
        {
            try
            {
                return btnViewOrPrintCHED.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void ClickViewOrPrintCHED()
        {
            btnViewOrPrintCHED.Click();
        }
        
        public void ClickReturnToYourDashboardLink()
        {
            lnkReturnToDashboard.Click();
        }
        #endregion
    }
}
