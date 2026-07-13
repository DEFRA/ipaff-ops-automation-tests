using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class WhichCountriesWillThisCommodityRuleApplyToPage : IWhichCountriesWillThisCommodityRuleApplyToPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Which countries will this commodity rule apply to?']"), true);
        private IWebElement countrySelectionInput => _driver.WaitForElement(By.Id("countryselection"));
        private IWebElement countryOption(string country) =>
            _driver.WaitForElement(By.XPath($"//div[@id='countryselection-multiSelectOptions']//div[normalize-space()='{country}']"));
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Continue']"));
        #endregion

        public WhichCountriesWillThisCommodityRuleApplyToPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Which countries will this commodity rule apply to?");

        public void SelectCountry(string country)
        {
            countrySelectionInput.Clear();
            countrySelectionInput.SendKeys(country);
            Thread.Sleep(1000);
            countryOption(country).Click();
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}