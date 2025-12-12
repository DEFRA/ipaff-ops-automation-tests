using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ChecksSubmittedPage : IChecksSubmittedPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@class='govuk-panel__title']"), true);
        private IWebElement chedReference => _driver.WaitForElement(By.Id("reference-number"));
        private IWebElement chedReferenceVersion => _driver.WaitForElement(By.Id("cved-version-number"));
        private IWebElement outcomeValue => _driver.WaitForElement(By.XPath("//*[@class='govuk-!-font-weight-bold']"));
        private IWebElement btnViewOrPrintCHED => _driver.WaitForElement(By.Id("print"));
        private IWebElement lnkReturnToDashboard => _driver.FindElement(By.Id("return-to-home-page"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ChecksSubmittedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Your checks have been submitted");
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
            Console.WriteLine("Outcome value: " + outcomeValue.Text.Trim());
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

        public void ClickReturnToYourDashboard()
        {
            lnkReturnToDashboard.Click();
        }
    }
}