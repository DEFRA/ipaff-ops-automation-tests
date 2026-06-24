using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
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
        private IWebElement txtVarietyAutocomplete => _driver.WaitForElement(By.Id("autocomplete-variety"), true);
        private IWebElement varietyAutocompleteOption(string variety) => _driver.WaitForElement(By.XPath($"//ul[contains(@id,'autocomplete-variety__listbox')]//li[normalize-space()='{variety}'] | //li[contains(@class,'autocomplete__option') and normalize-space()='{variety}']"), true);
        private IWebElement rdoQualityClass(string qualityClass) => _driver.WaitForElementExists(By.XPath($"//input[@name='commodityClass' and @value='{qualityClass}']"), true);
        private IWebElement txtCountryOfOriginAutocomplete => _driver.WaitForElement(By.Id("origin-country"), true);
        private IWebElement countryOfOriginOption(string country) => _driver.WaitForElement(By.XPath($"//li[contains(.,'{country}')] | //div[contains(@class,'autocomplete__option') and contains(.,'{country}')]"), true);
        private IWebElement txtNetWeightPerPackage => _driver.WaitForElement(By.Id("quantityOrWeightPerPackage"), true);
        private IWebElement txtNumberOfPackages => _driver.WaitForElement(By.Id("number-of-packages"), true);
        private IWebElement txtTypeOfPackagingAutocomplete => _driver.WaitForElement(By.Id("packaging-type"), true);
        private IWebElement typeOfPackagingOption(string packagingType) => _driver.WaitForElement(By.XPath($"//li[contains(.,'{packagingType}')] | //div[contains(@class,'autocomplete__option') and contains(.,'{packagingType}')]"), true);
        private IWebElement rdoPackagingReusable(string yesNoValue) => _driver.WaitForElementExists(By.XPath($"//input[@name='packagingReusableOrReclaimed' and @value='{yesNoValue}']"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
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
            SelectVarietyType(varietyType);
            EnterSpecificVariety(specificVariety);
            SelectQualityClass(qualityClass);
            SelectCountryOfOrigin(countryOfOrigin);
            EnterNetWeightPerPackage(netWeightPerPackage);
            EnterNumberOfPackages(numberOfPackages);
            SelectTypeOfPackaging(typeOfPackaging);
            SelectReusablePackagingOption(reusablePackagingOptionValue);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

        #region Private Methods for Data Entry

        private void SelectVarietyType(string varietyType)
        {
            varietyTypeOption(varietyType).Click();
        }

        private void EnterSpecificVariety(string specificVariety)
        {
            txtVarietyAutocomplete.Clear();
            txtVarietyAutocomplete.SendKeys(specificVariety);
            varietyAutocompleteOption(specificVariety).Click();
        }

        private void SelectQualityClass(string qualityClass)
        {
            rdoQualityClass(qualityClass).Click();
        }

        private void SelectCountryOfOrigin(string countryOfOrigin)
        {
            txtCountryOfOriginAutocomplete.Clear();
            txtCountryOfOriginAutocomplete.SendKeys(countryOfOrigin);
            countryOfOriginOption(countryOfOrigin).Click();
        }

        private void EnterNetWeightPerPackage(string netWeightPerPackage)
        {
            txtNetWeightPerPackage.Clear();
            txtNetWeightPerPackage.SendKeys(netWeightPerPackage);
        }

        private void EnterNumberOfPackages(string numberOfPackages)
        {
            txtNumberOfPackages.Clear();
            txtNumberOfPackages.SendKeys(numberOfPackages);
        }

        private void SelectTypeOfPackaging(string typeOfPackaging)
        {
            txtTypeOfPackagingAutocomplete.Clear();
            txtTypeOfPackagingAutocomplete.SendKeys(typeOfPackaging);
            typeOfPackagingOption(typeOfPackaging).Click();
        }

        private void SelectReusablePackagingOption(string reusablePackagingOptionValue)
        {
            var yesNoValue = reusablePackagingOptionValue.ToLower() == "yes" ? "true" : "false";
            rdoPackagingReusable(yesNoValue).Click();
        }

        #endregion
    }
}