using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class HMIImportCountryRulesPage : IHMIImportCountryRulesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='HMI (Import) Country Rules']"), true);
        private IWebElement countryDropdown => _driver.WaitForElement(By.Id("country"));
        private IWebElement rateInput => _driver.WaitForElement(By.Id("rate"));
        private IWebElement approvedInspectionServiceCheckbox => _driver.WaitForElementExists(By.Id("ais-status"));
        private IWebElement permanentCheckbox => _driver.WaitForElementExists(By.Id("permanentRule"));
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Confirm and send']"));
        #endregion

        public HMIImportCountryRulesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("HMI (Import) Country Rules");

        public void SelectCountry(string country)
        {
            var select = new SelectElement(countryDropdown);
            select.SelectByText(country);
        }

        public void SetInspectionRate(int rate)
        {
            rateInput.Clear();
            rateInput.SendKeys(rate.ToString());
        }

        public void EnsureApprovedInspectionServiceIsChecked()
        {
            if (!approvedInspectionServiceCheckbox.Selected)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", approvedInspectionServiceCheckbox);
            }
        }

        public void EnsureApprovedInspectionServiceIsNotChecked()
        {
            if (approvedInspectionServiceCheckbox.Selected)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", approvedInspectionServiceCheckbox);
            }
        }

        public void EnsurePermanentIsChecked()
        {
            if (!permanentCheckbox.Selected)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", permanentCheckbox);
            }
        }

        public void ClickConfirmAndSendButton() => btnConfirmAndSend.Click();
    }
}