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
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl ']"), true);
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

            
    }
}