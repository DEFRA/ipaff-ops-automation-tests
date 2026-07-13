using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class EUImportCountryRulesPage : IEUImportCountryRulesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(
            By.XPath("//h1[normalize-space()='EU (Import) Country Rules']"), true);
        private IWebElement countryDropdown => _driver.WaitForElement(By.Id("country"));
        private IWebElement rateInput => _driver.WaitForElement(By.Id("rate"));
        private IWebElement permanentCheckbox => _driver.WaitForElementExists(By.Id("permanentRule"));
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(
            By.XPath("//button[@type='submit' and normalize-space()='Confirm and send']"));
        #endregion

        public EUImportCountryRulesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() =>
            pageTitle.Text.Trim().Equals("EU (Import) Country Rules");

        public void SelectCountry(string country)
        {
            var selectElement = new SelectElement(countryDropdown);
            selectElement.SelectByText(country);
        }

        public void SetInspectionRate(int rate)
        {
            rateInput.Clear();
            rateInput.SendKeys(rate.ToString());
        }

        public void EnsurePermanentCheckboxIsChecked()
        {
            if (!permanentCheckbox.Selected)
            {
                permanentCheckbox.Click();
            }
        }

        public void ClickConfirmAndSendButton() => btnConfirmAndSend.Click();
    }
}