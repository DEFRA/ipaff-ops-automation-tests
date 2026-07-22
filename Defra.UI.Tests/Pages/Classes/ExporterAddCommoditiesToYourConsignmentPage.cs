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
        private IWebElement varietyTypeOption(string option) => _driver.WaitForElement(
            By.XPath($"//label[contains(@class,'govuk-radios__label') and normalize-space()='{option}']"), true);
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
        private IWebElement txtCommonName => _driver.WaitForElement(By.Id("commonName"), true);
        private IWebElement txtBotanicalNameAutocomplete => _driver.WaitForElement(By.Id("botanical-autocomplete-label"), true);
        private IWebElement botanicalNameOption(string botanicalName) => _driver.WaitForElement(By.XPath($"//ul[@id='botanical-autocomplete-label__listbox']//li[contains(.,'{botanicalName}')]"), true);
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

        public void DescribeCommodity(
            string commonName,
            string botanicalName,
            string countryOfOrigin,
            string netWeightPerPackage,
            string numberOfPackages,
            string typeOfPackaging,
            string reusablePackagingOptionValue)
        {
            if (!string.IsNullOrWhiteSpace(commonName))
                EnterCommonName(commonName);
            if (!string.IsNullOrWhiteSpace(botanicalName))
                EnterBotanicalName(botanicalName);
            if (!string.IsNullOrWhiteSpace(countryOfOrigin))
                SelectCountryOfOrigin(countryOfOrigin);
            if (!string.IsNullOrWhiteSpace(netWeightPerPackage))
                EnterNetWeightPerPackage(netWeightPerPackage);
            if (!string.IsNullOrWhiteSpace(numberOfPackages))
                EnterNumberOfPackages(numberOfPackages);
            if (!string.IsNullOrWhiteSpace(typeOfPackaging))
                SelectTypeOfPackaging(typeOfPackaging);
            if (!string.IsNullOrWhiteSpace(reusablePackagingOptionValue))
                SelectReusablePackagingOption(reusablePackagingOptionValue);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

        #region Private Methods for Data Entry

        private void SelectVarietyType(string varietyType)
        {
            var label = varietyTypeOption(varietyType);
            ScrollIntoView(label);
            SafeClick(label);
        }

        private void EnterSpecificVariety(string specificVariety)
        {
            txtVarietyAutocomplete.Clear();
            txtVarietyAutocomplete.SendKeys(specificVariety);
            var option = varietyAutocompleteOption(specificVariety);
            ScrollIntoView(option);
            SafeClick(option);
        }

        private void SelectQualityClass(string qualityClass)
        {
            var el = rdoQualityClass(qualityClass);
            ScrollIntoView(el);
            SafeClick(el);
        }

        private void SelectCountryOfOrigin(string countryOfOrigin)
        {
            txtCountryOfOriginAutocomplete.Clear();
            txtCountryOfOriginAutocomplete.SendKeys(countryOfOrigin);
            var option = countryOfOriginOption(countryOfOrigin);
            ScrollIntoView(option);
            SafeClick(option);
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
            var option = typeOfPackagingOption(typeOfPackaging);
            ScrollIntoView(option);
            SafeClick(option);
        }

        private void SelectReusablePackagingOption(string reusablePackagingOptionValue)
        {
            var yesNoValue = reusablePackagingOptionValue.ToLower() == "yes" ? "true" : "false";
            var el = rdoPackagingReusable(yesNoValue);
            ScrollIntoView(el);
            SafeClick(el);
        }

        private void EnterCommonName(string commonName)
        {
            txtCommonName.Clear();
            txtCommonName.SendKeys(commonName);
        }

        private void EnterBotanicalName(string botanicalName)
        {
            txtBotanicalNameAutocomplete.Clear();
            txtBotanicalNameAutocomplete.SendKeys(botanicalName);
            var option = botanicalNameOption($"({botanicalName})");
            ScrollIntoView(option);
            SafeClick(option);
        }

        // Centers the element in the viewport so it can't be covered by the
        // cookie banner at the top of the page.
        private void ScrollIntoView(IWebElement element) =>
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center', inline:'center'});", element);

        // Native click, with a JS-click fallback if something (e.g. the banner
        // during a scroll animation) intercepts the click.
        private void SafeClick(IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
            catch (ElementNotInteractableException)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
        }

        #endregion
    }
}