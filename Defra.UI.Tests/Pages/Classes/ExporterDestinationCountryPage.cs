using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterDestinationCountryPage : IExporterDestinationCountryPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Destination country']"), true);
        private IWebElement txtCountrySearch => _driver.WaitForElement(By.XPath("//input[contains(@id,'country') or @type='search' or contains(@class,'autocomplete__input')]"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        private IWebElement countryOption(string country) => _driver.WaitForElement(By.XPath($"//li[contains(.,'{country}')] | //div[contains(@class,'autocomplete__option') and contains(.,'{country}')]"), true);
        #endregion

        public ExporterDestinationCountryPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Destination country");

        public void SearchAndSelectCountry(string country)
        {
            txtCountrySearch.Clear();
            txtCountrySearch.SendKeys(country);
            countryOption(country).Click();
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}