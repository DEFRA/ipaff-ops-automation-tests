using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ControlRecordedPage : IControlRecordedPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//*[@class='govuk-panel__title']"), true);
        private IWebElement chedReference => _driver.WaitForElement(By.Id("reference-number"));
        private IWebElement chedReferenceVersion => _driver.WaitForElement(By.Id("version-number"));
        private IWebElement outcomeValue => _driver.WaitForElement(By.Id("control-outcome"));
        private IWebElement btnViewOrPrintCHED => _driver.WaitForElement(By.Id("view-certificate"));
        private IWebElement lnkReturnToDashboard => _driver.FindElement(By.Id("manage-notifications"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ControlRecordedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Your control has been recorded");
        }

        public string GetCHEDReferenceWithVersion()
        {
            var finalChedReference = chedReference.Text.Trim() + chedReferenceVersion.Text.Trim();
            return finalChedReference;
        }

        public string GetOutcome()
        {
            return outcomeValue.Text.Trim();
        }

        public void ClickViewOrPrintCHED() => btnViewOrPrintCHED.Click();

        public void ClickReturnToYourDashboard() => lnkReturnToDashboard.Click();
    }
}