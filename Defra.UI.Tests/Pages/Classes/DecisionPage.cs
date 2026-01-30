using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DecisionPage : IDecisionPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement acceptableForRadio(string acceptableForRadioOption) => _driver.FindElement(By.XPath($"//label[contains(text(),'{acceptableForRadioOption}')]/preceding-sibling::input"));
        private IWebElement internalMarketSubRadio(string internalMarketSubOption) => _driver.FindElement(By.XPath($"//label[contains(text(),'{internalMarketSubOption}')]/preceding-sibling::input"));
        private IWebElement rdoTransit => _driver.FindElement(By.Id("acceptability-transit"));
        private IWebElement drpExitBCP => _driver.FindElement(By.Id("transitExitBipUk"));
        private IWebElement drpTransitedCountry => _driver.FindElement(By.Id("transitCountry"));
        private IWebElement drpDestinationCountry => _driver.FindElement(By.Id("transitDestinationCountry"));
        private IWebElement rdoTranshipment => _driver.FindElement(By.Id("acceptability-transhipment"));
        private IWebElement rdoChannelled => _driver.FindElement(By.Id("acceptability-channelled"));
        private IWebElement rdoNonInternalMarket => _driver.FindElement(By.Id("acceptability-noninternalmarket"));
        private IWebElement rdoInternalMarket => _driver.FindElement(By.Id("acceptability-internalmarket"));
        private IWebElement rdoInternalMarketSubOption(string label) => _driver.FindElement(By.XPath($"//*[@id='internal']/div/fieldset/div/div/label[text()='{label}']"));
        private IWebElement rdoSpecificWarehouse => _driver.FindElement(By.Id("acceptability-nonconforming"));
        private IWebElement rdoNotAcceptable => _driver.FindElement(By.Id("acceptability-refused"));
        private IWebElement rdoDestruction => _driver.FindElement(By.Id("notAcceptAction-destruction"));
        private IWebElement rdoDestructionReason => _driver.FindElement(By.Id("notAcceptableDestructionReason"));
        private IWebElement rdoReDispatching => _driver.FindElement(By.XPath("//input[@id='notAcceptAction-reexport']|//input[@id='notAcceptAction-redispatch']"));
        private IWebElement rdoTransformation => _driver.FindElement(By.Id("notAcceptAction-transformation"));
        private IWebElement rdoOther => _driver.FindElement(By.Id("notAcceptAction-other"));
        private IWebElement txtNotAcceptableDay => _driver.FindElement(By.Id("not-acceptable-day"));
        private IWebElement txtNotAcceptableMonth => _driver.FindElement(By.Id("not-acceptable-month"));
        private IWebElement txtNotAcceptableYear => _driver.FindElement(By.Id("not-acceptable-year"));
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        // Dynamic helper to get radio button by label text
        private IWebElement GetRadioButtonByLabel(string labelText) =>
            _driver.FindElement(By.XPath($"//label[normalize-space(text())='{labelText}']/preceding-sibling::input[@type='radio']"));
        // Dynamic helper to get sub-option radio button within conditional radios
        private IWebElement GetConditionalRadioButtonByLabel(string labelText) =>
            _driver.FindElement(By.XPath($"//div[contains(@class, 'govuk-radios--conditional')]//label[normalize-space(text())='{labelText}']/preceding-sibling::input[@type='radio']"));
        private IWebElement rdoNotAccptableSubOption(string subOption) =>
            _driver.FindElement(By.XPath($"//label[normalize-space(text())='{subOption}']"));
        private IWebElement txtExitDateDay => _driver.FindElement(By.Id("temp-deadline-day"));
        private IWebElement txtExitDateMonth => _driver.FindElement(By.Id("temp-deadline-month"));
        private IWebElement txtExitDateYear => _driver.FindElement(By.Id("temp-deadline-year"));
        private IWebElement ddlExitBCP => _driver.FindElement(By.Id("temporaryExitBipUk"));
        private IWebElement txtDestructionReason => _driver.FindElement(By.Id("notAcceptableDestructionReason"));
        
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public DecisionPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Decision");
        }

        public bool IsAcceptableForRadioSelected(string acceptableForRadioOption)
        {
            return acceptableForRadio(acceptableForRadioOption).GetAttribute("aria-expanded").Contains("true");
        }

        public bool IsInternalMarketSubRadioSelected(string internalMarketSubOption)
        {
            return internalMarketSubRadio(internalMarketSubOption).GetAttribute("checked") != null;
        }

        public void SelectAcceptableFor(string acceptableFor, string subOption)
        {
            // Click the main acceptableFor radio button based on label text
            GetRadioButtonByLabel(acceptableFor).Click();

            // If acceptableFor is "Internal market", click the sub-option
            if (acceptableFor.Equals("Internal market", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(subOption))
                {
                    GetConditionalRadioButtonByLabel(subOption).Click();
                }
            }
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }

        public bool VerifyTransitRadioButtonPrePopulated()
        {
            return rdoTransit.Selected;
        }

        public bool VerifyPrepopulatedTransitDetails(string exitBCP, string transitedCountry, string destinationCountry)
        {
            var parts = exitBCP.Split(' ');
            string updatedexitBCP = parts[0];

            SelectElement exitBCPDropdown = new SelectElement(drpExitBCP);
            var prePopulatedExitBCP = exitBCPDropdown.SelectedOption.Text.Trim();

            SelectElement transitedCountryDropdown = new SelectElement(drpTransitedCountry);
            var prePopulatedTransitedCountry = transitedCountryDropdown.SelectedOption.Text.Trim();

            SelectElement destinationCountryDropdown = new SelectElement(drpDestinationCountry);
            var prePopulatedDestinationCountry = destinationCountryDropdown.SelectedOption.Text.Trim();

            return prePopulatedTransitedCountry.Equals(transitedCountry)
                && prePopulatedDestinationCountry.Equals(destinationCountry)
                && prePopulatedExitBCP.ToUpper().Contains(updatedexitBCP);             
        }

        public void SelectDecision(string subOption, string decision)
        {
            switch (decision)
            {
                case "Transhipment / Onward travel": rdoTranshipment.Click(); break;
                case "Transit": rdoTransit.Click(); break;
                case "Channelled": rdoChannelled.Click(); break;
                case "Non-internal market": rdoNonInternalMarket.Click(); break;
                case "Internal market": 
                    rdoInternalMarket.Click();
                    switch (subOption)
                    {
                        case "Feedingstuff":
                        case "Further process":
                        case "Human consumption": 
                        case "Other": 
                            rdoInternalMarketSubOption(subOption).Click(); break;
                    }
                    break;
                case "Specific warehouse procedure": rdoSpecificWarehouse.Click(); break;
                case "Not acceptable":
                    rdoNotAcceptable.Click();
                    switch (subOption)
                    {
                        case "Destruction": rdoDestruction.Click(); break;
                        case "Re-dispatching": rdoReDispatching.Click(); break;
                        case "Transformation": rdoTransformation.Click(); break;
                        case "Other": rdoOther.Click(); break;
                    }
                    break;
            }
        }

        public void EnterCurrentDateInDecisionPage(string day, string month, string year)
        {
            txtNotAcceptableDay.Click();
            txtNotAcceptableDay.SendKeys(day);
            txtNotAcceptableMonth.Click();
            txtNotAcceptableMonth.SendKeys(month);
            txtNotAcceptableYear.Click();
            txtNotAcceptableYear.SendKeys(year);
        }

        public void EnterReason(string reason)
        {
            rdoDestructionReason.Clear();
            rdoDestructionReason.SendKeys(reason);
        }

        public void SelectNotAcceptableFor(string acceptableFor, string subOption)
        {
            GetRadioButtonByLabel(acceptableFor).Click();
            rdoNotAccptableSubOption(subOption).Click();
        }

        public bool IsRadioButtonPreSelected(string radioButtonName)
        {
            try
            {
                var radioButton = GetRadioButtonByLabel(radioButtonName);
                return radioButton.GetAttribute("checked") != null;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public string GetExitDate()
        {
            var day = txtExitDateDay.GetAttribute("value");
            var month = txtExitDateMonth.GetAttribute("value");
            var year = txtExitDateYear.GetAttribute("value");

            return $"{day}/{month}/{year}";
        }

        public string GetExitBCP()
        {
            var selectElement = new SelectElement(ddlExitBCP);
            return selectElement.SelectedOption.Text;
        }

        public string GetDestinationCountry()
        {
            try
            {
                var selectElement = new SelectElement(drpDestinationCountry);
                return selectElement.SelectedOption.Text.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetTransitExitBCP()
        {
            try
            {
                var selectElement = new SelectElement(drpExitBCP);
                return selectElement.SelectedOption.Text.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public void EnterDestructionReason(String reason)
        {
            txtDestructionReason.SendKeys(reason);
        }
    }
}