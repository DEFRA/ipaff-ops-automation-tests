using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhatAreYouExportingSelectOneCommodityAtATimePage : IExporterWhatAreYouExportingSelectOneCommodityAtATimePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("choose-form-heading"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("Button-Continue"));
        private IWebElement GetRadioInputByValue(string value) => _driver.WaitForElementExists(By.XPath($"//input[@name='parentCommonName' and @value='{value}']"));
        private IWebElement citrusAutocompleteInput => _driver.WaitForElement(By.Id("autocomplete-citrus-subtype"));
        private IWebElement lettuceAutocompleteInput => _driver.WaitForElement(By.Id("autocomplete-lettuce-subtype"));
        private IWebElement strawberryAutocompleteInput => _driver.WaitForElement(By.Id("autocomplete-strawberry-subtype"));
        private IWebElement autocompleteOption(string option) => _driver.WaitForElement(By.XPath($"//div[contains(@class,'autocomplete__option') and contains(normalize-space(),'{option}')]"));
        #endregion

        private static readonly Dictionary<string, string> CommodityLabelToValueMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "General marketing standards (GMS) commodity", "gms" }
        };

        public ExporterWhatAreYouExportingSelectOneCommodityAtATimePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("What are you exporting?");

        public void SelectCommodity(string commodity)
        {
            // Define commodity categories with their autocomplete subtypes
            var citrusOptions = new[] { "Clementine", "Lemon", "Mandarin", "Sweet orange", "Satsuma", "Tangerine", "Tangelo" };
            var lettuceOptions = new[] { "Cos / Romaine", "Frisee", "Iceberg", "Little Gem", "Lollo Rosso", "Oakleaf", "Round / Flat / Butterhead", "Escarole" };
            var strawberryOptions = new[] { "Wild strawberry", "Green strawberry", "White strawberry", "Red strawberry" };

            // Check if commodity requires autocomplete selection
            if (citrusOptions.Contains(commodity, StringComparer.OrdinalIgnoreCase))
            {
                SelectRadioByValue("Citrus");
                SelectAutocompleteOption(citrusAutocompleteInput, commodity);
            }
            else if (lettuceOptions.Contains(commodity, StringComparer.OrdinalIgnoreCase))
            {
                SelectRadioByValue("Lettuce");
                SelectAutocompleteOption(lettuceAutocompleteInput, commodity);
            }
            else if (strawberryOptions.Contains(commodity, StringComparer.OrdinalIgnoreCase))
            {
                SelectRadioByValue("Strawberry");
                SelectAutocompleteOption(strawberryAutocompleteInput, commodity);
            }
            else
            {
                // Resolve label text to actual value if mapping exists, otherwise use commodity directly
                var radioValue = CommodityLabelToValueMap.TryGetValue(commodity, out var mappedValue)
                    ? mappedValue
                    : commodity;
                SelectRadioByValue(radioValue);
            }
        }

        private void SelectRadioByValue(string value)
        {
            var radio = GetRadioInputByValue(value);
            radio.Click();
        }

        private void SelectAutocompleteOption(IWebElement autocompleteElement, string option)
        {
            autocompleteElement.Clear();
            autocompleteElement.SendKeys(option);
            var optionElement = autocompleteOption(option);
            optionElement.Click();
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}