using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDOverviewPage : ICHEDOverviewPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement btnRaiseBorderNotification => _driver.FindElement(By.Id("raise-border-notification"));
        private IWebElement btnCopyAsReplacement => _driver.FindElement(By.Id("replace-certificate"));
        private IWebElement lnkReplacedBy => _driver.FindElement(By.Id("replaced-by"));
        private IWebElement lnkReplacedCertificate => _driver.FindElement(By.Id("replaced-certificate"));
        private IWebElement btnShowCHED => _driver.FindElement(By.XPath("//*[@id='show-certificate']/span"));
        private IWebElement lnkClearAll => _driver.FindElement(By.XPath("//a[text()='Clear all']"));
        private IWebElement lblFieldValue(string fieldName) => _driver.FindElement(By.XPath($"(//*[normalize-space(text())='{fieldName}']/following-sibling::td)[1]"));
        private IWebElement lblFieldValueInChecks(string fieldName,string sectionName) => _driver.FindElement(By.XPath($"(//*[normalize-space(text())='{sectionName}']/following::*[normalize-space(text())='{fieldName}']/following-sibling::td)[1]"));
        private IWebElement lblFieldValueInControl(string fieldName,string sectionName) => _driver.FindElement(By.XPath($"(//h2[normalize-space(text())='{sectionName}']/following::*[normalize-space(text())='{fieldName}']/following-sibling::td)[1]"));
        private IWebElement lblFieldValueForTable(string fieldName,string column) => _driver.FindElement(By.XPath($"//th[normalize-space()='{fieldName}']/following::td[{column}]"));
        private IWebElement lnkTab(string tabName) => _driver.FindElement(By.XPath($"//a[@id='tab_{tabName}']"));
        private IWebElement btnRecordControl => _driver.FindElement(By.Id("record-control"));
        private IWebElement txtRiskDecisionPHSIValue => _driver.FindElement(By.XPath("//*[normalize-space()='Risk decision PHSI']/following-sibling::dd/strong"));
        private IWebElement txtRiskDecisionHMIValue => _driver.FindElement(By.XPath("//*[normalize-space()='Risk decision HMI']/following-sibling::dd/strong"));
        private IWebElement txtDocumentCheckValue => _driver.FindElement(By.XPath("//*[normalize-space()='Document check']/following-sibling::dd/strong"));
        private IWebElement lblStatusLabel => _driver.FindElement(By.Id("Status-Label"));
        private IWebElement lblReferenceNumber => _driver.FindElement(By.Id("reference-number"));
        private IWebElement lblChecksCount => _driver.FindElement(By.XPath("//h2[contains(@class,'text-align-right')]"));

        private IReadOnlyCollection<IWebElement> allCheckDecisionTags =>
            _driver.FindElements(By.XPath(
                "//dt[normalize-space()='Document check' or " +
                "normalize-space()='Identity check' or " +
                "normalize-space()='Physical check']" +
                "/following-sibling::dd/strong"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CHEDOverviewPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("CHED overview");
        }

        public void ClickRaiseBorderNotification()
        {
            btnRaiseBorderNotification.Click();
        }

        public void ClickCopyAsReplacement()
        {
            btnCopyAsReplacement.Click();
        }

        public bool VerifyCHEDReference(string type, string chedReference, string replacementChedReference)
        {
            if (type.Equals("original"))
                return primaryTitle.Text.Contains(chedReference);
            else if (type.Equals("replacement"))
                return primaryTitle.Text.Contains(replacementChedReference);
            else
                return false;
        }

        public bool VerifyReplacedByLink(string type, string chedReference, string replacementChedReference)
        {
            if (type.Equals("original"))
                return lnkReplacedCertificate.Text.Contains("Replaced certificate: " + chedReference);
            else if (type.Equals("replacement"))
                return lnkReplacedBy.Text.Contains("Replaced by: " + replacementChedReference);
            else
                return false;
        }

        public void ClickReplacedByLink()
        {
            lnkReplacedBy.Click();
        }
        
        public bool VerifyShowChedButton()
        {
            return btnShowCHED.Text.Equals("Show CHED");
        }
        
        public bool VerifyTab(string tabName)
        {
            return lnkTab((tabName == "Checks" ? "decision" : tabName).ToLower()).Text.Equals(tabName);
        }

        public void SwitchTab(string tabName)
        {
            lnkTab((tabName == "Checks" ? "decision" : tabName).ToLower()).Click();
        }

        public void ClickClearAll()
        {
            lnkClearAll.Click();
        }

        public bool IsFieldValuePresent(string fieldName)
        {
            _driver.ScrollAndClick(lblFieldValue(fieldName));
            return !string.IsNullOrEmpty(lblFieldValue(fieldName).Text);
        }
        
        public bool IsFieldValuePresent(string fieldName, string sectionName)
        {
            if (sectionName.ToLower().Equals("control"))
            {
                _driver.ScrollAndClick(lblFieldValueInControl(fieldName, sectionName));
                return !string.IsNullOrEmpty(lblFieldValueInControl(fieldName, sectionName).Text);
            }
            _driver.ScrollAndClick(lblFieldValueInChecks(fieldName, sectionName));
            return !string.IsNullOrEmpty(lblFieldValueInChecks(fieldName, sectionName).Text);
        }

        public bool IsFieldValuePresentInTable(string fieldName, string column)
        {
            _driver.ScrollAndClick(lblFieldValueForTable(fieldName, column));
            return !string.IsNullOrEmpty(lblFieldValueForTable(fieldName, column).Text);
        }

        public void ClickRecordControlButton() => btnRecordControl.Click();
        
        public bool VerifyDecisionRecordedBy(string fieldName, string status)
        {
            return lblFieldValue(fieldName).Text.Trim().Equals(status);
        }

        public bool VerifyDocumentCheck(string status)
        {
            return txtDocumentCheckValue.Text.Trim().Equals(status);
        }

        public bool VerifyRiskDecisionHMI(string decision)
        {
            return txtRiskDecisionHMIValue.Text.Trim().Equals(decision);
        }

        public bool VerifyRiskDecisionPHSI(string decision)
        {
            return txtRiskDecisionPHSIValue.Text.Trim().Equals(decision);
        }

        public void ClickRecordControl()
        {
            btnRecordControl.Click();
        }

        public bool VerifyNotificationStatus(string expectedStatus, string chedReference)
        {
            return lblReferenceNumber.Text.Trim().Equals(chedReference, StringComparison.OrdinalIgnoreCase)
                   && lblStatusLabel.Text.Trim().Equals(expectedStatus, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyChecksCount(int shown, int total)
        {
            var expectedCount = $"{shown} of {total}";
            return lblChecksCount.Text.Trim()
                .Equals(expectedCount, StringComparison.OrdinalIgnoreCase);
        }

        public (bool AllMatch, int Total, List<string> NonMatchingValues) VerifyAllCheckDecisions(params string[] acceptedDecisions)
        {
            var checkTags = allCheckDecisionTags;
            var nonMatching = checkTags
                .Select(tag => tag.Text.Trim())
                .Where(text => !acceptedDecisions.Any(d => d.Equals(text, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return (checkTags.Count > 0 && nonMatching.Count == 0, checkTags.Count, nonMatching);
        }
    }
}