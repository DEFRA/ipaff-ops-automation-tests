using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DecisionHubPage : IDecisionHubPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//span[@id='page-primary-title' and text()='Decision Hub']"));
        private IWebElement referenceNumber => _driver.WaitForElement(By.Id("reference-number"));
        private IWebElement lnkSaveAndSetAsInProgress => _driver.WaitForElement(By.Id("set-in-progress"));
        private IWebElement txtUpdatedStatus => _driver.WaitForElement(By.Id("Status-Label"));
        private IWebElement lnkLocalRefNum => _driver.WaitForElement(By.XPath("//a[normalize-space()='Local reference number']"));
        private IWebElement lnkSealNumbers => _driver.WaitForElement(By.XPath("//a[normalize-space()='Seal numbers']"));
        private IWebElement lnkLaboratoryTests => _driver.WaitForElement(By.XPath("//a[normalize-space()='Laboratory tests']"));
        private IWebElement lnkDecision => _driver.WaitForElement(By.Id("button-decision-notification"));
        private IWebElement lnkReviewAndSubmit => _driver.WaitForElement(By.Id("button-review-notification"));
        private IWebElement lnkOverrideRiskDecision => _driver.FindElement(By.Id("override-risk-decision-link"));
        private IWebElement txtRiskAssesmentTitle => _driver.FindElement(By.Id("risk-assessment-banner-title"));
        private IWebElement txtRiskAssesmentMessage => _driver.FindElement(By.XPath("//p[@class='govuk-body']"));
        private IWebElement lnkChecks => _driver.FindElement(By.XPath("//a[normalize-space()='Checks']"));
        private IWebElement lnkViewNotification => _driver.WaitForElement(By.Id("view-notification"));
        private IWebElement btnAttachment => _driver.FindElement(By.Id("attachments-link"));
        private IWebElement btnReturnToWorkOrder => _driver.FindElement(By.Id("button-return-work-order"));
        private IReadOnlyCollection<IWebElement> btnRecordChecksLink(string linkText) =>
            _driver.FindElements(By.XPath($"//button[contains(@class,'link-button') and normalize-space()='{linkText}']"));
        private IWebElement btnRecordChecksLinkByName(string checkName) =>
            _driver.FindElement(By.XPath($"//button[contains(@class,'link-button') and normalize-space()='{checkName}']"));
        private IWebElement txtRecordChecksStatus(string checkName) =>
            _driver.FindElement(By.XPath(
                $"//td[normalize-space()='{checkName}' or " +
                $".//button[contains(@class,'link-button') and normalize-space()='{checkName}']]" +
                $"/following-sibling::td//strong[contains(@class,'govuk-tag')]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public DecisionHubPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Decision Hub");
        }

        public bool IsPageLoadedForChedReference(string expectedChedReference)
        {
            return primaryTitle.Text.Contains("Decision Hub") &&
                   referenceNumber.Text.Trim().Equals(expectedChedReference, StringComparison.OrdinalIgnoreCase);
        }

        public void ClickSaveAndSetAsInProgress()
        {
            lnkSaveAndSetAsInProgress.Click();
        }

        public bool VerifyStatusUpdate(string stausNew, string statusInProgress)
        {
            return txtUpdatedStatus.Text.Trim().Equals(statusInProgress, StringComparison.OrdinalIgnoreCase);
        }

        public void ClickLocalRefNumLink()
        {
            lnkLocalRefNum.Click();
        }

        public void ClickSealNumbersLink()
        {
            lnkSealNumbers.Click();
        }

        public void ClickLaboratoryTestsLink()
        {
            lnkLaboratoryTests.Click();
        }

        public void ClickDecisionLink()
        {
            lnkDecision.Click();
        }

        public void ClickReviewAndSubmitLink()
        {
            lnkReviewAndSubmit.Click();
        }

        public void ClickOverrideRiskDecisionLink()
        {
            lnkOverrideRiskDecision.Click();
        }

        public bool VerifyInspectionRequiredBox(string msgboxTitle)
        {
            return txtRiskAssesmentTitle.Text.Trim().Equals(msgboxTitle);
        }

        public bool VerifyInspectionRequiredMessage(string message)
        {
            return txtRiskAssesmentMessage.Text.Trim().Equals(message);
        }

        public void ClickChecksLink()
        {
            lnkChecks.Click();
        }

        public void ClickViewNotificationOfConsignment()
        {
            lnkViewNotification.Click();
        }

        public void ClickAttachmentsButton()
        {
            btnAttachment.Click();
        }

        public bool VerifyRecordChecksLinksAreClickable(string firstLink, string secondLink)
        {
            var phsiButtons = btnRecordChecksLink(firstLink);
            var hmiButtons = btnRecordChecksLink(secondLink);

            return phsiButtons.Count > 0 && phsiButtons.First().Displayed &&
                   hmiButtons.Count > 0 && hmiButtons.First().Displayed;
        }

        public void ClickReturnToWorkOrder()
        {
            btnReturnToWorkOrder.Click();
        }

        public bool VerifyRecordChecksStatus(string checkName, string expectedStatus)
        {
            return txtRecordChecksStatus(checkName).Text.Trim()
                .Equals(expectedStatus, StringComparison.OrdinalIgnoreCase);
        }

        public void ClickRecordChecksLink(string checkName)
        {
            btnRecordChecksLinkByName(checkName).Click();
        }
    }
}