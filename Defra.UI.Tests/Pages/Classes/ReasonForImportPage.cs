using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using Microsoft.VisualBasic.FileIO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;


namespace Defra.UI.Tests.Pages.Classes
{
    public class ReasonForImportPage : IReasonForImportPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement rdoInternalMarket => _driver.WaitForElement(By.XPath("//*[@id='radio-internalmarket']/following-sibling::label"));
        private IWebElement rdoTranshipment => _driver.WaitForElement(By.XPath("//*[@id='radio-tranship']/following-sibling::label"));
        private IWebElement rdoTransit => _driver.WaitForElement(By.XPath("//*[@id='radio-transit']/following-sibling::label"));
        private IWebElement rdoReentry => _driver.WaitForElement(By.XPath("//*[@id='a_impadm2']/following-sibling::label")); 
        private IWebElement rdoTemporaryAdmissionHorses => _driver.WaitForElement(By.XPath("//*[@id='a_impadm3']/following-sibling::label"));
        private IWebElement rdoNonInternalMarket => _driver.WaitForElement(By.XPath("//*[@id='radio-noninternalmarket']/following-sibling::label"));
        private IWebElement txtExitBCP => _driver.WaitForElement(By.Name("bcp-transit-third-country"));
        private IWebElement txtTransitedCountry => _driver.WaitForElement(By.Id("transit-third-countries-last"));
        private IWebElement txtDestinationCountry => _driver.FindElement(By.Id("third-country-transit"));
        private IWebElement txtTranshipmentDestination => _driver.FindElement(By.Id("third-country-transhipment"));
        private IWebElement rdoIMAnimalFeeding => _driver.WaitForElement(By.XPath("//*[@id='internalMarketanimal']/following-sibling::label"));
        private IWebElement rdoIMOther => _driver.WaitForElement(By.XPath("//*[@id='internalMarketother']/following-sibling::label"));
        private IWebElement rdoIMPharma => _driver.WaitForElement(By.XPath("//*[@id='internalMarketpharma']/following-sibling::label"));
        private IWebElement rdoIMTechnicalUse => _driver.WaitForElement(By.XPath("//*[@id='internalMarkettechnical']/following-sibling::label"));
        private IWebElement GetReasonRadioButton(string reasonText) =>
            _driver.FindElement(By.XPath($"//label[contains(@class, 'govuk-radios__label') and contains(normalize-space(), '{reasonText}')]"));
        private IWebElement GetTranitSubOption(string subOption) =>
            _driver.FindElement(By.XPath($"//label[contains(@class, 'govuk-radios__label')and contains(normalize-space(.), 'Transit')]/following::div[1]/*[contains(normalize-space(.), '{subOption}')]"));
        private IWebElement GetInternalMarketSubOption(string subOptionText) =>
            _driver.FindElement(By.XPath($"//div[contains(@id,'internalmarket-conditional')]//label[contains(@class, 'govuk-radios__label') and normalize-space()='{subOptionText}']"));
        private IWebElement txtDay => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-date-day"));
        private IWebElement txtMonth => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-date-month"));
        private IWebElement txtYear => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-date-year"));
        private IWebElement txtHours => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-time-hour"));
        private IWebElement txtMinutes => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-time-minutes"));
        private By txtPointOfExitBy => By.Id("point-of-exit");
        private IWebElement txtPointOfExit => _driver.WaitForElement(txtPointOfExitBy);
        private IWebElement txtExitDateDay => _driver.WaitForElement(By.Id("exit-date-day"));
        private IWebElement txtExitDateMonth => _driver.WaitForElement(By.Id("exit-date-month"));
        private IWebElement txtExitDateYear => _driver.WaitForElement(By.Id("exit-date-year"));
        private IWebElement ddlExitBCP => _driver.FindElement(By.Id("bcp-temporary-admission"));
        private IWebElement internalMarketConditional => _driver.WaitForElement(By.Id("internalmarket-conditional"));
        private IWebElement transhipConditional => _driver.WaitForElement(By.Id("tranship-conditional"));
        private IWebElement transitConditional => _driver.WaitForElement(By.Id("conditional-transit"));
        private By reentryConditionalLocator => By.Id("conditional-reimport");
        private IWebElement temporaryAdmissionConditional => _driver.WaitForElement(By.Id("conditional-temporary-admission"));
        private IWebElement transitExitBCP => _driver.FindElement(By.Id("bcp-transit-third-country"));
        private IWebElement transitDestinationCountry => _driver.WaitForElement(By.Id("third-country-transit"));
        private IReadOnlyCollection<IWebElement> internalMarketSubOptions => internalMarketConditional.FindElements(By.CssSelector("input[type='radio'][name='internal-market']"));
        private IWebElement selectedReasonForImportRadioLabel => _driver.FindElement(By.XPath("//input[contains(@class,'govuk-radios__input') and @checked]/following-sibling::label"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ReasonForImportPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("About the consignment")
                && primaryTitle.Text.Contains("What is the main reason for importing the consignment?");
        }

        public bool IsReasonForImportingAnimalsPageLoaded()
        {
            return secondaryTitle.Text.Contains("About the consignment")
                && primaryTitle.Text.Contains("What is the main reason for importing the animals?");
        }

        public bool IsElementPresent(IWebElement element)
        {
            try
            {
                return !string.IsNullOrEmpty(element.Text);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool AreImportReasonsPresent()
        {
            try
            {
                return IsElementPresent(GetReasonRadioButton("Internal market"))
                    && IsElementPresent(GetReasonRadioButton("Transhipment or onward travel"))
                    && IsElementPresent(GetReasonRadioButton("Transit"))
                    && IsElementPresent(GetReasonRadioButton("Re-entry"));
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool AreImportAnimalsReasonsPresent()
        {
            try
            {
                return AreImportReasonsPresent()
                    && IsElementPresent(GetReasonRadioButton("Temporary admission horses"));
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool AreImportReasonsForCHEDDPresent()
        {
            return IsElementPresent(rdoInternalMarket)
                && IsElementPresent(rdoNonInternalMarket);
        }

        public void SelectReasonForImport(string option)
        {
            var reasonRadioButton = GetReasonRadioButton(option);
            reasonRadioButton.Click();
        }

        public void SelectReasonForImportSubOption(string subOption)
        {
            var subOptionRadioButton = GetInternalMarketSubOption(subOption);
            subOptionRadioButton.Click();
        }

        public void SelectExitBorderControlPost(string exitBCP)
        {
            new SelectElement(txtExitBCP).SelectByText(exitBCP);
        }

        public void EnterConsignmentLeavingDate(string day, string month, string year)
        {
            txtDay.SendKeys(day);
            txtMonth.SendKeys(month);
            txtYear.SendKeys(year);
        }

        public void EnterConsignmentLeavingTime(string hours, string minutes)
        {
            txtHours.SendKeys(hours);
            txtMinutes.SendKeys(minutes);
        }

        public string EnterConsignmentDepartureDate()
        {
            var futureDate = DateTime.Now.AddDays(7);

            txtDay.SendKeys(futureDate.Day.ToString());
            txtMonth.SendKeys(futureDate.Month.ToString());
            txtYear.SendKeys(futureDate.Year.ToString());

            var leavingFromGBDate = futureDate.ToString("dd MMMM yyyy");
            return leavingFromGBDate;
        }

        public string EnterConsignmentDepartureTime()
        {
            var futureDate = DateTime.Now.AddDays(7);
            var leavingFromGBTime = futureDate.ToString("HH:mm");
            
            txtHours.SendKeys(futureDate.Hour.ToString());
            txtMinutes.SendKeys(futureDate.Minute.ToString());

            return leavingFromGBTime;
        }

        public void SelectTransitedCountry(string transitedCountry)
        {
            new SelectElement(txtTransitedCountry).SelectByText(transitedCountry);
        }

        public void SelectDestinationCountry(string destinationCountry)
        {
            new SelectElement(txtDestinationCountry).SelectByText(destinationCountry);
        }

        public void SelectDestinationCountryBasedOnContext(string destinationCountry)
        {
            // Determine which dropdown to use based on what's currently visible
            try
            {
                // Check if Transit dropdown is displayed
                if (txtDestinationCountry.Displayed)
                {
                    new SelectElement(txtDestinationCountry).SelectByText(destinationCountry);
                    return;
                }
            }
            catch (NoSuchElementException) { }
            catch (ElementNotInteractableException) { }

            try
            {
                // Check if Transhipment dropdown is displayed
                if (txtTranshipmentDestination.Displayed)
                {
                    new SelectElement(txtTranshipmentDestination).SelectByText(destinationCountry);
                    return;
                }
            }
            catch (NoSuchElementException) { }
            catch (ElementNotInteractableException) { }

            throw new InvalidOperationException("No visible Destination Country dropdown found. Ensure the correct import reason (Transit or Transhipment) is selected.");
        }

        public void SelectTranshipmentDestination(string transhipmentCountry)
        {
            new SelectElement(txtTranshipmentDestination).SelectByText(transhipmentCountry);
        }

        public void AddPlaceOfExit(string placeOfExit)
        {
            _driver.WaitForElementCondition(ExpectedConditions.ElementIsVisible(txtPointOfExitBy));
            txtPointOfExit.SendKeys(placeOfExit);
        }      
        public void EnterExitDate(int daysFromToday)
        {
            var exitDate = DateTime.Now.AddDays(daysFromToday);
            txtExitDateDay.Click();
            txtExitDateDay.SendKeys(exitDate.Day.ToString());
            txtExitDateMonth.Click();
            txtExitDateMonth.SendKeys(exitDate.Month.ToString());
            txtExitDateYear.Click();
            txtExitDateYear.SendKeys(exitDate.Year.ToString());
        }

        public void SelectExitBCP(string exitBCP)
        {
            new SelectElement(ddlExitBCP).SelectByText(exitBCP);
        }

        public void SelectExitBCPBasedOnContext(string exitBCP)
        {
            // Determine which dropdown to use based on what's currently visible
            try
            {
                // Check if Transit dropdown is displayed
                if (transitExitBCP.Displayed)
                {
                    new SelectElement(transitExitBCP).SelectByText(exitBCP);
                    return;
                }
            }
            catch (NoSuchElementException) { }
            catch (ElementNotInteractableException) { }

            try
            {
                // Check if Temporary Admission dropdown is displayed
                if (ddlExitBCP.Displayed)
                {
                    new SelectElement(ddlExitBCP).SelectByText(exitBCP);
                    return;
                }
            }
            catch (NoSuchElementException) { }
            catch (ElementNotInteractableException) { }

            throw new InvalidOperationException("No visible Exit BCP dropdown found. Ensure the correct import reason is selected.");
        }

        public bool VerifyInternalMarketHasSubOptions(int expectedCount)
        {
            try
            {
                return internalMarketSubOptions.Count == expectedCount;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool VerifyTranshipmentHasDestinationCountryDropdown()
        {
            try
            {
                return IsElementPresent(txtTranshipmentDestination) && txtTranshipmentDestination.TagName == "select";
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool VerifyTransitHasExitBCPAndDestinationDropdowns()
        {
            try
            {
                return IsElementPresent(transitExitBCP) && transitExitBCP.TagName == "select"
                    && IsElementPresent(transitDestinationCountry) && transitDestinationCountry.TagName == "select";
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool VerifyReentryHasNoSubOptions()
        {
            try
            {
                // Verify the radio button exists and is displayed
                bool radioButtonExists = rdoReentry.Displayed;

                // Verify there is no conditional div for Re-entry
                try
                {
                    var conditionalDiv = _driver.FindElement(reentryConditionalLocator);
                    // If we found the conditional div, Re-entry has sub-options (unexpected)
                    return false;
                }
                catch (NoSuchElementException)
                {
                    // No conditional div found - this is expected for Re-entry
                    return radioButtonExists;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool VerifyTemporaryAdmissionHasExitDateAndBCPDropdown()
        {
            try
            {
                return txtExitDateDay.Displayed && txtExitDateMonth.Displayed
                    && txtExitDateYear.Displayed && ddlExitBCP.Displayed
                    && ddlExitBCP.TagName == "select";
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public string GetReasonForImportRadioLabelText => selectedReasonForImportRadioLabel.Text.Trim() ?? string.Empty;

        public bool VerifySubOption(string mainOption, string subOption)
        {
            GetReasonRadioButton(mainOption).Click();
            var subOptions = subOption.Split(',').Select(x => x.Trim()).ToArray();
            if (mainOption == "Internal market")
            {
                var expectedSubOptions = new List<string>();
                foreach (var opt in subOptions)
                {
                    var element = GetInternalMarketSubOption(opt);
                    expectedSubOptions.Add(element.Text);
                }
                return expectedSubOptions
                    .All(el => subOptions.Contains(el));
            }
            else if(mainOption == "Transit")
            {
                var transitSubOptionTexts = new List<string>();

                foreach (var opt in subOptions)
                {
                    var element = GetTranitSubOption(opt);
                    transitSubOptionTexts.Add(element.Text.Split('\r')[0].Trim());                    
                }
                return transitSubOptionTexts.All(el => subOptions.Contains(el));
            }
            return false;
        }
    }
}