using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterAddCommoditiesToYourConsignmentPage : IExporterAddCommoditiesToYourConsignmentPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Add commodities to your consignment']"), true);
        private IWebElement varietyTypeOption(string option) => _driver.WaitForElement(By.XPath($"//label[contains(normalize-space(),'{option}')]"), true);
        private IWebElement txtVarietyAutocomplete => _driver.WaitForElement(By.XPath("//label[normalize-space()='Search for a variety']/following::input[contains(@class,'autocomplete__input')][1] | //input[@id='autocomplete-variety']"), true);
        private IWebElement varietyAutocompleteOption(string variety) => _driver.WaitForElement(By.XPath($"//ul[contains(@id,'autocomplete-variety__listbox')]//li[normalize-space()='{variety}'] | //li[contains(@class,'autocomplete__option') and normalize-space()='{variety}']"), true);
        private IWebElement drpQualityClass => _driver.WaitForElement(By.XPath("//label[contains(normalize-space(),'What quality class is your commodity?')]/following::select[1]"), true);
        private IWebElement drpCountryOfOrigin => _driver.WaitForElement(By.XPath("//label[contains(normalize-space(),'Country of origin')]/following::select[1]"), true);
        private IWebElement txtNetWeightPerPackage => _driver.WaitForElement(By.XPath("//label[contains(normalize-space(),'Net weight per package')]/following::input[1]"), true);
        private IWebElement txtNumberOfPackages => _driver.WaitForElement(By.XPath("//label[contains(normalize-space(),'Number of packages')]/following::input[1]"), true);
        private IWebElement drpTypeOfPackaging => _driver.WaitForElement(By.XPath("//label[contains(normalize-space(),'Type of packaging')]/following::select[1]"), true);
        private IWebElement reusablePackagingOption(string option) => _driver.WaitForElement(By.XPath($"//label[contains(normalize-space(),'{option}')]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Save and continue'] | //input[@value='Save and continue']"));
        #endregion

        public ExporterAddCommoditiesToYourConsignmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Add commodities to your consignment");

        public void EnterCommodityDetails(
            string varietyType,
            string specificVariety,
            string qualityClass,
            string countryOfOrigin,
            string netWeightPerPackage,
            string numberOfPackages,
            string typeOfPackaging,
            string reusablePackagingOptionValue)
        {
            varietyTypeOption(varietyType).Click();

            txtVarietyAutocomplete.Clear();
            txtVarietyAutocomplete.SendKeys(specificVariety);
            varietyAutocompleteOption(specificVariety).Click();

            new SelectElement(drpQualityClass).SelectByText(qualityClass);
            new SelectElement(drpCountryOfOrigin).SelectByText(countryOfOrigin);

            txtNetWeightPerPackage.Clear();
            txtNetWeightPerPackage.SendKeys(netWeightPerPackage);

            txtNumberOfPackages.Clear();
            txtNumberOfPackages.SendKeys(numberOfPackages);

            new SelectElement(drpTypeOfPackaging).SelectByText(typeOfPackaging);

            reusablePackagingOption(reusablePackagingOptionValue).Click();
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}