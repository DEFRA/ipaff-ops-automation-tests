using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterAddBulbsToYourConsignmentPage : IExporterAddBulbsToYourConsignmentPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Add bulbs to your consignment']"), true);
        private IWebElement rdoDescribeByBotanicalName => _driver.WaitForElementExists(By.Id("commodity-describe-by-autocomplete"), true);
        private IWebElement txtBotanicalNameAutocomplete => _driver.WaitForElement(By.Id("botanical-autocomplete-label"), true);
        private IWebElement botanicalNameOption(string name) => _driver.WaitForElement(By.XPath($"//ul[@id='botanical-autocomplete-label__listbox']//li[contains(.,'{name}')]"), true);
        private IWebElement rdoCommodityType(string type) => _driver.WaitForElementExists(By.Id(type.Trim().ToUpper()), true);
        private IWebElement txtQuantity => _driver.WaitForElement(By.Id("quantityOrWeightPerPackage"), true);
        private IWebElement drpUnitOfMeasurement => _driver.WaitForElement(By.Id("unit-of-measurement"), true);
        private IWebElement txtNumberOfPackages => _driver.WaitForElement(By.Id("number-of-packages"), true);
        private IWebElement txtTypeOfPackagingAutocomplete => _driver.WaitForElement(By.Id("packaging-type"), true);
        private IWebElement typeOfPackagingOption(string packagingType) => _driver.WaitForElement(By.XPath($"//li[contains(.,'{packagingType}')] | //div[contains(@class,'autocomplete__option') and contains(.,'{packagingType}')]"), true);
        private IWebElement chkCountryOfOrigin(string country) => _driver.WaitForElementExists(By.XPath($"//label[normalize-space()='{country}']/preceding-sibling::input[@name='countriesOfOrigin'] | //input[@name='countriesOfOrigin' and @id=//label[normalize-space()='{country}']/@for]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterAddBulbsToYourConsignmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Add bulbs to your consignment");

        public void EnterBotanicalName(string botanicalName)
        {
            if (!rdoDescribeByBotanicalName.Selected)
                rdoDescribeByBotanicalName.Click();

            const int maxAttempts = 3;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                txtBotanicalNameAutocomplete.Clear();
                txtBotanicalNameAutocomplete.SendKeys(botanicalName);

                var option = botanicalNameOption(botanicalName);
                SelectAutocompleteOption(option);

                if (IsAutocompleteCommitted(txtBotanicalNameAutocomplete, botanicalName))
                    return;

                try { txtBotanicalNameAutocomplete.SendKeys(Keys.Escape); } catch { /* best effort */ }
            }

            var committed = GetInputValue(txtBotanicalNameAutocomplete);
            throw new WebDriverException(
                $"Botanical name selection failed to commit after {maxAttempts} attempts. " +
                $"Expected the autocomplete value to contain '{botanicalName}' but it was '{committed}'.");
        }

        public void SelectCommodityType(string commodityType) => rdoCommodityType(commodityType).Click();

        public void EnterQuantity(string quantity)
        {
            txtQuantity.Clear();
            txtQuantity.SendKeys(quantity);
        }

        public void SelectUnitOfMeasurement(string unit) => new SelectElement(drpUnitOfMeasurement).SelectByText(unit);

        public void EnterNumberOfPackages(string numberOfPackages)
        {
            txtNumberOfPackages.Clear();
            txtNumberOfPackages.SendKeys(numberOfPackages);
        }

        public void SelectTypeOfPackaging(string packagingType)
        {
            txtTypeOfPackagingAutocomplete.Clear();
            txtTypeOfPackagingAutocomplete.SendKeys(packagingType);

            var option = typeOfPackagingOption(packagingType);
            SelectAutocompleteOption(option);
        }

        public void SelectCountriesOfOrigin(string countries)
        {
            foreach (var country in countries.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var checkbox = chkCountryOfOrigin(country);
                if (!checkbox.Selected)
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", checkbox);
            }
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

        public string GetSelectedCommodityDescription()
        {
            var committed = GetInputValue(txtBotanicalNameAutocomplete) ?? string.Empty;
            var openBracketIndex = committed.LastIndexOf('(');
            var description = (openBracketIndex > 0 ? committed[..openBracketIndex] : committed).Trim();
            return Regex.Replace(description, @"\s+", " ");
        }

        #region Private Methods for Autocomplete Handling

        // accessible-autocomplete commits the selection on the option's mousedown
        // handler (not click). A plain JS .click() therefore never commits and on
        // blur the widget auto-picks the first suggestion still in the listbox.
        // Dispatch mousedown -> mouseup -> click explicitly so the selection is
        // always committed, regardless of whether a native click would have been
        // intercepted after scroll.
        private void SelectAutocompleteOption(IWebElement option)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center', inline:'center'});", option);

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
                System.Threading.Thread.Sleep(100);
            }
            return false;
        }

        private string GetInputValue(IWebElement input) =>
            (string)((IJavaScriptExecutor)_driver).ExecuteScript("return arguments[0].value;", input);

        #endregion
    }
}
