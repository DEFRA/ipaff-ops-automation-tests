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
        private IWebElement lnkReturnToDashboard => _driver.FindElement(By.XPath("//a[contains(@id,'return-to-home-page') or contains(@id,'manage-notifications')]"));
        private IWebElement lblErrorMessageTitle => _driver.FindElement(By.XPath("//div[@id='border-notification-banner']/h2"));
        private IReadOnlyCollection<IWebElement> lblErrorMessages => _driver.FindElements(By.XPath("//div[@id='border-notification-banner']/div/ul/li"));
        private IWebElement txtInNextSteps => _driver.FindElement(By.XPath("//*[normalize-space()='Next steps']/following-sibling::p"));
        private IWebElement btnCreateBorderNotification => _driver.FindElement(By.Id("create-border-notification"));
        private IWebElement lblDecisionOutcome => _driver.FindElement(By.Id("decision-outcome"));
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

        public bool IsError(string errorMessage)
        {
            foreach (var element in lblErrorMessages)
            {
                if (element.Text.Contains(errorMessage))
                {
                    return true;
                }
            }
            return false;
        }

        public bool VerifyErrorMessageTitle(string title)
        {
            return lblErrorMessageTitle.Text.Trim().Contains(title);
        }

        public bool VerifyNextStepsMessage(string message)
        {
            return txtInNextSteps.Text.Trim().Contains(message);
        }

        public bool VerifyOutcome(string outcome) => lblDecisionOutcome.Text.Trim().Equals(outcome);

        public void ClickCreateBorderNotiButton() => btnCreateBorderNotification.Click();
    }
}