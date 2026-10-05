using System;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterHowWillThisConsignmentBeTransportedPage : IExporterHowWillThisConsignmentBeTransportedPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='How will this consignment be transported?']"), true);
        private IWebElement rdoAir => _driver.WaitForElementExists(By.Id("air"), true);
        private IWebElement rdoMaritime => _driver.WaitForElementExists(By.Id("maritime"), true);
        private IWebElement rdoRail => _driver.WaitForElementExists(By.Id("rail"), true);
        private IWebElement rdoRoad => _driver.WaitForElementExists(By.Id("road"), true);
        private IWebElement rdoRoadOrFerry => _driver.WaitForElementExists(By.Id("Road"), true);
        private IWebElement txtConsignmentValue => _driver.WaitForElement(By.Id("consignmentValue"), true);
        private IWebElement rdoPointOfEntryYes => _driver.WaitForElementExists(By.Id("poe_selection_yes"), true);
        private IWebElement rdoPointOfEntryNo => _driver.WaitForElementExists(By.Id("poe_selection_no"), true);
        private IWebElement txtPointOfEntry => _driver.WaitForElement(By.Id("port-search-label"), true);
        private IWebElement pointOfEntryOption(string port) => _driver.WaitForElement(By.XPath($"//ul[@id='port-search-label__listbox']/li[contains(.,'{port}')]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterHowWillThisConsignmentBeTransportedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("How will this consignment be transported?");

        public void SelectAnyTransportMethod() => rdoAir.Click();

        public void SelectTransportMethod(string method)
        {
            IWebElement GetRadio() => method.ToLower() switch
            {
                "air" => rdoAir,
                "maritime" => rdoMaritime,
                "rail" => rdoRail,
                "road" => rdoRoad,
                "road or ferry" => rdoRoadOrFerry,
                _ => rdoAir,
            };

            EnsureRadioSelected(GetRadio);
        }

        // Clicking the transport method radio immediately after the point of
        // entry autocomplete commits can land on a stale element if the form
        // re-renders in response to the autocomplete selection. Re-resolve the
        // element and retry the click until it is actually checked.
        private void EnsureRadioSelected(Func<IWebElement> locateRadio)
        {
            const int maxAttempts = 3;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                var radio = locateRadio();

                try
                {
                    if (!radio.Selected)
                        radio.Click();
                }
                catch (StaleElementReferenceException)
                {
                    continue;
                }
                catch (ElementClickInterceptedException)
                {
                    radio = locateRadio();
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", radio);
                }

                for (var i = 0; i < 5; i++)
                {
                    try
                    {
                        if (locateRadio().Selected)
                            return;
                    }
                    catch (StaleElementReferenceException)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }

            throw new WebDriverException($"Failed to select transport method radio button after {maxAttempts} attempts.");
        }

        public void EnterConsignmentValue(decimal value)
        {
            txtConsignmentValue.Clear();
            txtConsignmentValue.SendKeys(value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
        }

        public void SelectDoYouKnowThePointOfEntry(string option)
        {
            if (option.Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase))
                rdoPointOfEntryYes.Click();
            else
                rdoPointOfEntryNo.Click();
        }

        public void EnterPointOfEntry(string pointOfEntry)
        {
            if (string.IsNullOrWhiteSpace(pointOfEntry))
                return;

            const int maxAttempts = 3;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                txtPointOfEntry.Clear();
                txtPointOfEntry.SendKeys(pointOfEntry);

                var option = pointOfEntryOption(pointOfEntry);
                SelectAutocompleteOption(option);

                if (IsAutocompleteCommitted(txtPointOfEntry, pointOfEntry))
                    return;

                try { txtPointOfEntry.SendKeys(Keys.Escape); } catch { /* best effort */ }
            }

            var committed = GetInputValue(txtPointOfEntry);
            throw new WebDriverException(
                $"Point of entry selection failed to commit after {maxAttempts} attempts. " +
                $"Expected the autocomplete value to contain '{pointOfEntry}' but it was '{committed}'.");
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

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
                if (value.Contains(expectedToken, System.StringComparison.OrdinalIgnoreCase)
                    && string.Equals(expanded, "false", System.StringComparison.OrdinalIgnoreCase))
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