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
            SelectAutocompleteOption(option);
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
            SelectAutocompleteOption(option);
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
            SelectAutocompleteOption(option);
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

        // Hardening: accessible-autocomplete commits selection on the option's
        // mousedown handler. If the commit doesn't happen (e.g. option was briefly
        // covered during scroll, or Chrome's hit-testing raced the listbox
        // re-render), the widget auto-picks the first suggestion on blur — which
        // for "PEBAM" is "Ebenopsis ebano (EBPEB)". We verify the input's value
        // contains "(CODE)" after selection and retry the whole entry a few times
        // before failing this step (rather than the downstream page-check step).
        private void EnterBotanicalName(string botanicalName)
        {
            const int maxAttempts = 3;
            var expectedCodeToken = $"({botanicalName})";

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                txtBotanicalNameAutocomplete.Clear();
                txtBotanicalNameAutocomplete.SendKeys(botanicalName);

                var option = botanicalNameOption(expectedCodeToken);
                SelectAutocompleteOption(option);

                if (IsBotanicalCommitted(expectedCodeToken))
                    return;

                // Not committed — dismiss any open listbox and try again.
                try { txtBotanicalNameAutocomplete.SendKeys(Keys.Escape); } catch { /* best effort */ }
            }

            var committed = GetInputValue(txtBotanicalNameAutocomplete);
            throw new WebDriverException(
                $"Botanical name selection failed to commit after {maxAttempts} attempts. " +
                $"Expected the autocomplete value to contain '{expectedCodeToken}' but it was '{committed}'.");
        }

        private bool IsBotanicalCommitted(string expectedCodeToken)
        {
            for (var i = 0; i < 10; i++)
            {
                var value = GetInputValue(txtBotanicalNameAutocomplete) ?? string.Empty;
                var expanded = txtBotanicalNameAutocomplete.GetAttribute("aria-expanded");
                if (value.Contains(expectedCodeToken, System.StringComparison.OrdinalIgnoreCase)
                    && string.Equals(expanded, "false", System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                Thread.Sleep(100);
            }
            return false;
        }

        private void ScrollIntoView(IWebElement element) =>
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center', inline:'center'});", element);

        // For plain radio/label clicks: native click first, JS click fallback.
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

        // accessible-autocomplete commits the selection on the option's mousedown
        // handler (not click). A plain JS .click() therefore never commits and on
        // blur the widget auto-picks the first suggestion still in the listbox.
        // Dispatch mousedown -> mouseup -> click explicitly so the selection is
        // always committed, regardless of whether a native click would have been
        // intercepted after scroll.
        private void SelectAutocompleteOption(IWebElement option)
        {
            ScrollIntoView(option);
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                @"var el = arguments[0];
                  var rect = el.getBoundingClientRect();
                  var opts = { bubbles: true, cancelable: true, view: window,
                               button: 0, clientX: rect.left + rect.width/2,
                               clientY: rect.top + rect.height/2 };
                  el.dispatchEvent(new MouseEvent('mousedown', opts));
                  el.dispatchEvent(new MouseEvent('mouseup',   opts));
                  el.dispatchEvent(new MouseEvent('click',     opts));",
                option);
        }

        private string GetInputValue(IWebElement input) =>
            (string)((IJavaScriptExecutor)_driver).ExecuteScript("return arguments[0].value;", input);

        #endregion
    }
}