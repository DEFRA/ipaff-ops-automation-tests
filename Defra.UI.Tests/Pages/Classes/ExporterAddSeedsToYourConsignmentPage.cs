using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterAddSeedsToYourConsignmentPage : IExporterAddSeedsToYourConsignmentPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Add seeds to your consignment']"), true);
        private IWebElement rdoDescribeByBotanicalName => _driver.WaitForElementExists(By.Id("commodity-describe-by-autocomplete"), true);
        private IWebElement txtBotanicalNameAutocomplete => _driver.WaitForElement(By.Id("botanical-autocomplete-label"), true);
        private IWebElement botanicalNameOption(string name) => _driver.WaitForElement(By.XPath($"//ul[@id='botanical-autocomplete-label__listbox']//li[contains(.,'{name}')]"), true);
        private IWebElement txtVariety => _driver.WaitForElement(By.Id("variety-search"), true);
        private IWebElement txtLotReference => _driver.WaitForElement(By.Id("lot-reference"), true);
        private IWebElement txtSampleReference => _driver.WaitForElement(By.Id("sample-reference"), true);
        private IWebElement txtQuantity => _driver.WaitForElement(By.Id("quantityOrWeightPerPackage"), true);
        private IWebElement drpUnitOfMeasurement => _driver.WaitForElement(By.Id("unit-of-measurement"), true);
        private IWebElement rdoChemicallyTreatedYes => _driver.WaitForElementExists(By.Id("chemically_treated_yes"), true);
        private IWebElement rdoChemicallyTreatedNo => _driver.WaitForElementExists(By.Id("chemically_treated_no"), true);
        private IWebElement txtChemicalsUsed => _driver.WaitForElement(By.Id("chemically_treated_more_detail"), true);
        private IWebElement drpCountryWhereGrown => _driver.WaitForElementExists(By.Id("origin-country"), true);
        private IWebElement autocompleteOption(string text) => _driver.WaitForElement(By.XPath($"//li[normalize-space()='{text}'] | //li[contains(@class,'autocomplete__option') and contains(.,'{text}')]"), true);
        private IWebElement chkDestinationCountry(string country) => _driver.WaitForElementExists(By.XPath($"//input[@name='destinationCountries' and @id=//label[normalize-space()='{country}']/@for]"), true);
        private IWebElement txtNumberOfPackages => _driver.WaitForElement(By.Id("number-of-packages"), true);
        private IWebElement drpTypeOfPackaging => _driver.WaitForElementExists(By.Id("packaging-type"), true);
        private IWebElement chkCountryOfOrigin(string country) => _driver.WaitForElementExists(By.XPath($"//input[@name='countriesOfOrigin' and @id=//label[normalize-space()='{country}']/@for]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterAddSeedsToYourConsignmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Add seeds to your consignment");

        public void EnterBotanicalName(string botanicalName)
        {
            if (!rdoDescribeByBotanicalName.Selected)
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", rdoDescribeByBotanicalName);

            const int maxAttempts = 3;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                txtBotanicalNameAutocomplete.Clear();
                txtBotanicalNameAutocomplete.SendKeys(botanicalName);

                SelectAutocompleteOption(() => botanicalNameOption(botanicalName));

                if (IsAutocompleteCommitted(txtBotanicalNameAutocomplete, botanicalName))
                    return;

                try { txtBotanicalNameAutocomplete.SendKeys(Keys.Escape); } catch { /* best effort */ }
            }

            throw new WebDriverException(
                $"Botanical name selection failed to commit after {maxAttempts} attempts. " +
                $"Expected the autocomplete value to contain '{botanicalName}' but it was '{GetInputValue(txtBotanicalNameAutocomplete)}'.");
        }

        public string GetSelectedCommodityDescription()
        {
            var committed = GetInputValue(txtBotanicalNameAutocomplete) ?? string.Empty;
            var openBracketIndex = committed.LastIndexOf('(');
            var description = (openBracketIndex > 0 ? committed[..openBracketIndex] : committed).Trim();
            return Regex.Replace(description, @"\s+", " ");
        }

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

        public void SelectCountryWhereGrown(string country) => SelectFromSelectOrAutocomplete(drpCountryWhereGrown, "origin-country", country);

        public void SelectDestinationCountries(string countries)
        {
            foreach (var country in SplitList(countries))
                TickCheckbox(chkDestinationCountry(country));
        }

        public void EnterNumberOfPackages(string numberOfPackages) => ClearAndType(txtNumberOfPackages, numberOfPackages);

        public void SelectTypeOfPackaging(string packagingType) => SelectFromSelectOrAutocomplete(drpTypeOfPackaging, "packaging-type", packagingType);

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
                && GetSelectedText(drpCountryWhereGrown, "origin-country") == countryWhereGrown
                && SplitList(destinationCountries).All(c => chkDestinationCountry(c).Selected);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

        #region Private helpers

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

        // The select may be progressively enhanced into an accessible-autocomplete input
        private void SelectFromSelectOrAutocomplete(IWebElement element, string id, string text)
        {
            if (element.TagName.Equals("select", StringComparison.OrdinalIgnoreCase) && element.Displayed)
            {
                new SelectElement(element).SelectByText(text);
                return;
            }

            var input = _driver.WaitForElement(By.Id(id), true);
            ClearAndType(input, text);
            SelectAutocompleteOption(() => autocompleteOption(text));
        }

        private string GetSelectedText(IWebElement element, string id)
        {
            if (element.TagName.Equals("select", StringComparison.OrdinalIgnoreCase))
                return new SelectElement(element).SelectedOption.Text.Trim();
            return (GetInputValue(element) ?? string.Empty).Trim();
        }

        // accessible-autocomplete commits the selection on mousedown, not click
        // The suggestion list re-renders while filtering, so the option is re-located on every attempt
        private void SelectAutocompleteOption(Func<IWebElement> getOption)
        {
            const int maxAttempts = 5;

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
                catch (StaleElementReferenceException) when (attempt < maxAttempts)
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
