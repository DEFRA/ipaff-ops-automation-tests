using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterAddToYourConsignmentPage : IExporterAddToYourConsignmentPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle(string commodity) => _driver.WaitForElement(By.XPath($"//h1[normalize-space()='Add {commodity} to your consignment']"), true);
        private IWebElement rdoDescribeByBotanicalName => _driver.WaitForElementExists(By.Id("commodity-describe-by-autocomplete"), true);
        private IWebElement txtBotanicalNameAutocomplete => _driver.WaitForElement(By.Id("botanical-autocomplete-label"), true);
        private IWebElement botanicalNameOption(string name) => _driver.WaitForElement(By.XPath($"//ul[@id='botanical-autocomplete-label__listbox']//li[contains(.,'{name}')]"), true);
        private IWebElement txtCommonName => _driver.WaitForElement(By.Id("commonName"), true);
        private IWebElement rdoCommodityType(string type) => _driver.WaitForElementExists(By.Id(type.Trim().ToUpper()), true);
        private IWebElement varietyTypeOption(string option) => _driver.WaitForElement(By.XPath($"//label[contains(@class,'govuk-radios__label') and normalize-space()='{option}']"), true);
        private IWebElement txtVarietyAutocomplete => _driver.WaitForElement(By.Id("autocomplete-variety"), true);
        private IWebElement varietyAutocompleteOption(string variety) => _driver.WaitForElement(By.XPath($"//ul[contains(@id,'autocomplete-variety__listbox')]//li[normalize-space()='{variety}'] | //li[contains(@class,'autocomplete__option') and normalize-space()='{variety}']"), true);
        private IWebElement rdoQualityClass(string qualityClass) => _driver.WaitForElementExists(By.XPath($"//input[@name='commodityClass' and @value='{qualityClass}']"), true);
        private IWebElement txtVariety => _driver.WaitForElement(By.Id("variety-search"), true);
        private IWebElement txtLotReference => _driver.WaitForElement(By.Id("lot-reference"), true);
        private IWebElement txtSampleReference => _driver.WaitForElement(By.Id("sample-reference"), true);
        private IWebElement txtQuantity => _driver.WaitForElement(By.Id("quantityOrWeightPerPackage"), true);
        private IWebElement drpUnitOfMeasurement => _driver.WaitForElement(By.Id("unit-of-measurement"), true);
        private IWebElement rdoChemicallyTreatedYes => _driver.WaitForElementExists(By.Id("chemically_treated_yes"), true);
        private IWebElement rdoChemicallyTreatedNo => _driver.WaitForElementExists(By.Id("chemically_treated_no"), true);
        private IWebElement txtChemicalsUsed => _driver.WaitForElement(By.Id("chemically_treated_more_detail"), true);
        private IWebElement drpCountryOfOrigin => _driver.WaitForElementExists(By.Id("origin-country"), true);
        private IWebElement countryOfOriginOption(string country) => _driver.WaitForElement(By.XPath($"//li[contains(.,'{country}')] | //div[contains(@class,'autocomplete__option') and contains(.,'{country}')]"), true);
        private IWebElement autocompleteOption(string text) => _driver.WaitForElement(By.XPath($"//li[normalize-space()='{text}'] | //li[contains(@class,'autocomplete__option') and contains(.,'{text}')]"), true);
        private IWebElement chkDestinationCountry(string country) => _driver.WaitForElementExists(By.XPath($"//input[@name='destinationCountries' and @id=//label[normalize-space()='{country}']/@for]"), true);
        private IWebElement txtNumberOfPackages => _driver.WaitForElement(By.Id("number-of-packages"), true);
        private IWebElement drpTypeOfPackaging => _driver.WaitForElementExists(By.Id("packaging-type"), true);
        private IWebElement typeOfPackagingOption(string packagingType) => _driver.WaitForElement(By.XPath($"//li[contains(.,'{packagingType}')] | //div[contains(@class,'autocomplete__option') and contains(.,'{packagingType}')]"), true);
        private IWebElement rdoPackagingReusable(string yesNoValue) => _driver.WaitForElementExists(By.XPath($"//input[@name='packagingReusableOrReclaimed' and @value='{yesNoValue}']"), true);
        private IWebElement chkCountryOfOrigin(string country) => _driver.WaitForElementExists(By.XPath($"//label[normalize-space()='{country}']/preceding-sibling::input[@name='countriesOfOrigin'] | //input[@name='countriesOfOrigin' and @id=//label[normalize-space()='{country}']/@for]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        private IWebElement rdoPotatoType(string potatoType) => _driver.WaitForElementExists(By.Id($"potato-type-{potatoType.Trim().ToLower()}"), true);
        private IWebElement txtSeedStockNumber => _driver.WaitForElement(By.Id("seed-stock-number"), true);
        private IWebElement rdoSelectedVariety => _driver.WaitForElementExists(By.Id("selected-variety-radio"), true);
        private IWebElement txtGsiApplicationReference => _driver.WaitForElement(By.Id("gsi-application-reference"), true);
        private IWebElement txtChemicalUsedOnTubers => _driver.WaitForElement(By.Id("chemicals-used"), true);
        private IWebElement txtPackingMaterial => _driver.WaitForElement(By.Id("packaging-material"), true);
        private IWebElement fieldLabel(string id) => _driver.WaitForElement(By.CssSelector($"label[for='{id}']"), true);

        private static readonly Dictionary<string, string> potatoTextboxFieldIds = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Growing season inspection number", "gsi-application-reference" },
            { "Chemical used on tubers (optional)", "chemicals-used" },
            { "Quantity", "quantityOrWeightPerPackage" },
            { "Number of packages", "number-of-packages" },
            { "Type of packaging", "packaging-type" },
            { "Packing material (optional)", "packaging-material" },
            { "Distinguishing marks (optional)", "distinguishing-marks" },
            { "Countries of origin", "input-countries-of-origin" }
        };
        #endregion

        public ExporterAddToYourConsignmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string commodity) => pageTitle(commodity).Text.Trim().Equals($"Add {commodity} to your consignment");

        public void EnterBotanicalName(string botanicalName)
        {
            if (!rdoDescribeByBotanicalName.Selected)
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", rdoDescribeByBotanicalName);

            EnterBotanicalNameAutocomplete(botanicalName, botanicalName);
        }

        public string GetSelectedCommodityDescription()
        {
            var committed = GetInputValue(txtBotanicalNameAutocomplete) ?? string.Empty;
            var openBracketIndex = committed.LastIndexOf('(');
            var description = (openBracketIndex > 0 ? committed[..openBracketIndex] : committed).Trim();
            return Regex.Replace(description, @"\s+", " ");
        }

        public string GetSelectedCommodityEppoCode()
        {
            var committed = GetInputValue(txtBotanicalNameAutocomplete) ?? string.Empty;
            var match = Regex.Match(committed, @"\(([^()]*)\)\s*$");
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        public void SelectCommodityType(string commodityType) => rdoCommodityType(commodityType).Click();

        public void EnterVariety(string variety) => ClearAndType(txtVariety, variety);

        public void EnterLotReference(string lotReference) => ClearAndType(txtLotReference, lotReference);

        public void EnterSampleReference(string sampleReference) => ClearAndType(txtSampleReference, sampleReference);

        public void EnterQuantity(string quantity) => ClearAndType(txtQuantity, quantity);

        public void SelectUnitOfMeasurement(string unit) => new SelectElement(drpUnitOfMeasurement).SelectByText(unit);

        public void SelectChemicallyTreated(string option)
        {
            var radio = option.Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase) ? rdoChemicallyTreatedYes : rdoChemicallyTreatedNo;
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", radio);
        }

        public void EnterChemicalsUsed(string chemicals) => ClearAndType(txtChemicalsUsed, chemicals);

        public void SelectCountryWhereGrown(string country) =>
            SelectFromSelectOrAutocomplete(drpCountryOfOrigin, "origin-country", country, autocompleteOption);

        public void SelectDestinationCountries(string countries)
        {
            foreach (var country in SplitList(countries))
                TickCheckbox(chkDestinationCountry(country));
        }

        public void EnterNumberOfPackages(string numberOfPackages) => ClearAndType(txtNumberOfPackages, numberOfPackages);

        public void SelectTypeOfPackaging(string packagingType) =>
            SelectFromSelectOrAutocomplete(drpTypeOfPackaging, "packaging-type", packagingType, typeOfPackagingOption);

        public void SelectCountriesOfOrigin(string countries)
        {
            foreach (var country in SplitList(countries))
                TickCheckbox(chkCountryOfOrigin(country));
        }

        public List<string> GetUnitOfMeasurementOptions() =>
            new SelectElement(drpUnitOfMeasurement).Options
                .Select(o => o.Text.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .ToList();

        public bool AreSeedCommodityDetailsDisplayed(string botanicalName, string variety, string lotReference, string quantity, string unit,
            string chemicallyTreated, string chemicalsUsed, string countryWhereGrown, string destinationCountries)
        {
            var chemicallyTreatedRadio = chemicallyTreated.Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase) ? rdoChemicallyTreatedYes : rdoChemicallyTreatedNo;

            return (GetInputValue(txtBotanicalNameAutocomplete) ?? string.Empty).Contains(botanicalName, StringComparison.OrdinalIgnoreCase)
                && GetInputValue(txtVariety) == variety
                && GetInputValue(txtLotReference) == lotReference
                && GetInputValue(txtQuantity) == quantity
                && new SelectElement(drpUnitOfMeasurement).SelectedOption.Text.Trim() == unit
                && chemicallyTreatedRadio.Selected
                && GetInputValue(txtChemicalsUsed) == chemicalsUsed
                && GetSelectedText(drpCountryOfOrigin) == countryWhereGrown
                && SplitList(destinationCountries).All(c => chkDestinationCountry(c).Selected);
        }

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
            EnterQuantity(netWeightPerPackage);
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
                ClearAndType(txtCommonName, commonName);
            if (!string.IsNullOrWhiteSpace(botanicalName))
                EnterBotanicalNameAutocomplete(botanicalName, $"({botanicalName})");
            if (!string.IsNullOrWhiteSpace(countryOfOrigin))
                SelectCountryOfOrigin(countryOfOrigin);
            if (!string.IsNullOrWhiteSpace(netWeightPerPackage))
                EnterQuantity(netWeightPerPackage);
            if (!string.IsNullOrWhiteSpace(numberOfPackages))
                EnterNumberOfPackages(numberOfPackages);
            if (!string.IsNullOrWhiteSpace(typeOfPackaging))
                SelectTypeOfPackaging(typeOfPackaging);
            if (!string.IsNullOrWhiteSpace(reusablePackagingOptionValue))
                SelectReusablePackagingOption(reusablePackagingOptionValue);
        }

        public bool AreRadioOptionsDisplayed(params string[] radioIds) =>
            radioIds.All(id => _driver.IsElementDisplayed(By.CssSelector($"label[for='{id}']")));

        public string GetRadioLabelText(string radioId) => fieldLabel(radioId).Text.Trim();

        public bool IsUnitOfMeasurementDropdownDisplayed() =>
            drpUnitOfMeasurement.Displayed && fieldLabel("unit-of-measurement").Text.Trim().Equals("Unit of measurement");

        public bool IsTextboxFieldDisplayed(string fieldName)
        {
            if (!potatoTextboxFieldIds.TryGetValue(fieldName.Trim(), out var id))
                return false;

            return fieldLabel(id).Text.Trim().Equals(fieldName.Trim(), StringComparison.OrdinalIgnoreCase)
                && _driver.IsElementDisplayed(By.Id(id));
        }

        public void DescribePotato(string potatoType, string stockNumber, string variety, string gsiNumber, string chemicalUsed,
            string quantity, string unit, string numberOfPackages, string typeOfPackaging, string packingMaterial, string countriesOfOrigin)
        {
            ScrollAndClick(rdoPotatoType(potatoType));
            if (potatoType.Trim().Equals("Seed", StringComparison.OrdinalIgnoreCase))
                ClearAndType(txtSeedStockNumber, stockNumber);
            ScrollAndClick(rdoSelectedVariety);
            EnterSpecificVariety(variety);
            ClearAndType(txtGsiApplicationReference, gsiNumber);
            ClearAndType(txtChemicalUsedOnTubers, chemicalUsed);
            EnterQuantity(quantity);
            SelectUnitOfMeasurement(unit);
            EnterNumberOfPackages(numberOfPackages);
            SelectTypeOfPackaging(typeOfPackaging);
            ClearAndType(txtPackingMaterial, packingMaterial);
            SelectCountriesOfOrigin(countriesOfOrigin);
        }

        public bool ArePotatoCommodityDetailsDisplayed(string potatoType, string stockNumber, string variety, string gsiNumber, string chemicalUsed,
            string quantity, string unit, string numberOfPackages, string typeOfPackaging, string packingMaterial, string countriesOfOrigin)
        {
            var isSeed = potatoType.Trim().Equals("Seed", StringComparison.OrdinalIgnoreCase);

            return rdoPotatoType(potatoType).Selected
                && (!isSeed || GetInputValue(txtSeedStockNumber) == stockNumber)
                && rdoSelectedVariety.Selected
                && GetInputValue(txtVarietyAutocomplete) == variety
                && GetInputValue(txtGsiApplicationReference) == gsiNumber
                && GetInputValue(txtChemicalUsedOnTubers) == chemicalUsed
                && GetInputValue(txtQuantity) == quantity
                && new SelectElement(drpUnitOfMeasurement).SelectedOption.Text.Trim() == unit
                && GetInputValue(txtNumberOfPackages) == numberOfPackages
                && GetSelectedText(drpTypeOfPackaging) == typeOfPackaging
                && GetInputValue(txtPackingMaterial) == packingMaterial
                && SplitList(countriesOfOrigin).All(c => chkCountryOfOrigin(c).Selected);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

        #region Private helpers

        private void SelectVarietyType(string varietyType) => ScrollAndClick(varietyTypeOption(varietyType));

        private void EnterSpecificVariety(string specificVariety)
        {
            ClearAndType(txtVarietyAutocomplete, specificVariety);
            SelectAutocompleteOption(() => varietyAutocompleteOption(specificVariety));
        }

        private void SelectQualityClass(string qualityClass) => ScrollAndClick(rdoQualityClass(qualityClass));

        private void SelectCountryOfOrigin(string countryOfOrigin) =>
            SelectFromSelectOrAutocomplete(drpCountryOfOrigin, "origin-country", countryOfOrigin, countryOfOriginOption);

        private void SelectReusablePackagingOption(string reusablePackagingOptionValue)
        {
            var yesNoValue = reusablePackagingOptionValue.Equals("yes", StringComparison.OrdinalIgnoreCase) ? "true" : "false";
            ScrollAndClick(rdoPackagingReusable(yesNoValue));
        }

        // accessible-autocomplete commits selection on the option's mousedown handler. If the commit doesn't
        // happen the widget auto-picks the first suggestion on blur, so verify the committed value and retry.
        private void EnterBotanicalNameAutocomplete(string botanicalName, string expectedToken)
        {
            const int maxAttempts = 3;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                ClearAndType(txtBotanicalNameAutocomplete, botanicalName);

                SelectAutocompleteOption(() => botanicalNameOption(expectedToken));

                if (IsAutocompleteCommitted(txtBotanicalNameAutocomplete, expectedToken))
                    return;

                try { txtBotanicalNameAutocomplete.SendKeys(Keys.Escape); } catch { /* best effort */ }
            }

            throw new WebDriverException(
                $"Botanical name selection failed to commit after {maxAttempts} attempts. " +
                $"Expected the autocomplete value to contain '{expectedToken}' but it was '{GetInputValue(txtBotanicalNameAutocomplete)}'.");
        }

        private static IEnumerable<string> SplitList(string values) =>
            values.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        private static void ClearAndType(IWebElement element, string value)
        {
            element.Clear();
            element.SendKeys(value);
        }

        private void TickCheckbox(IWebElement checkbox)
        {
            if (!checkbox.Selected)
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", checkbox);
        }

        // For plain radio/label clicks: native click first, JS click fallback.
        private void ScrollAndClick(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center', inline:'center'});", element);
            try
            {
                element.Click();
            }
            catch (Exception ex) when (ex is ElementClickInterceptedException || ex is ElementNotInteractableException)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
        }

        // The select may be progressively enhanced into an accessible-autocomplete input
        private void SelectFromSelectOrAutocomplete(IWebElement element, string id, string text, Func<string, IWebElement> getOption)
        {
            if (element.TagName.Equals("select", StringComparison.OrdinalIgnoreCase) && element.Displayed)
            {
                new SelectElement(element).SelectByText(text);
                return;
            }

            var input = _driver.WaitForElement(By.Id(id), true);
            ClearAndType(input, text);
            SelectAutocompleteOption(() => getOption(text));
        }

        private string GetSelectedText(IWebElement element)
        {
            if (element.TagName.Equals("select", StringComparison.OrdinalIgnoreCase))
                return new SelectElement(element).SelectedOption.Text.Trim();
            return (GetInputValue(element) ?? string.Empty).Trim();
        }

        // accessible-autocomplete commits the selection on mousedown, not click
        // The suggestion list re-renders while filtering (debounced), so the option is re-located on every attempt
        // and both StaleElementReferenceException and NoSuchElementException (list not yet rendered) are retried
        private void SelectAutocompleteOption(Func<IWebElement> getOption)
        {
            const int maxAttempts = 10;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    var option = getOption();

                    ((IJavaScriptExecutor)_driver).ExecuteScript(
                        @"var el = arguments[0];
                          el.scrollIntoView({block:'center', inline:'center'});
                          var rect = el.getBoundingClientRect();
                          var opts = { bubbles: true, cancelable: true, view: window,
                                       button: 0, clientX: rect.left + rect.width/2,
                                       clientY: rect.top + rect.height/2 };
                          el.dispatchEvent(new MouseEvent('mousedown', opts));
                          el.dispatchEvent(new MouseEvent('mouseup',   opts));
                          el.dispatchEvent(new MouseEvent('click',     opts));",
                        option);
                    return;
                }
                catch (Exception ex) when ((ex is StaleElementReferenceException || ex is NoSuchElementException) && attempt < maxAttempts)
                {
                    Thread.Sleep(250);
                }
            }
        }

        private bool IsAutocompleteCommitted(IWebElement input, string expectedToken)
        {
            for (var i = 0; i < 10; i++)
            {
                var value = GetInputValue(input) ?? string.Empty;
                var expanded = input.GetAttribute("aria-expanded");
                if (value.Contains(expectedToken, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(expanded, "false", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                Thread.Sleep(100);
            }
            return false;
        }

        private string GetInputValue(IWebElement input) =>
            (string)((IJavaScriptExecutor)_driver).ExecuteScript("return arguments[0].value;", input);

        #endregion
    }
}
