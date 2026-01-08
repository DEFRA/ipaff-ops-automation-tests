using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
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
        private IWebElement rdoReentry => _driver.WaitForElement(By.XPath("//*[@id='radio-reimport']/following-sibling::label"));
        private IWebElement rdoNonInternalMarket => _driver.WaitForElement(By.XPath("//*[@id='radio-noninternalmarket']/following-sibling::label"));
        private IWebElement txtExitBCP => _driver.WaitForElement(By.Name("bcp-transit-third-country"));
        private IWebElement txtTransitedCountry => _driver.WaitForElement(By.Id("transit-third-countries-last"));
        private IWebElement txtDestinationCountry => _driver.WaitForElement(By.Id("third-country-transit"));
        private IWebElement txtTranshipmentDestination => _driver.FindElement(By.Id("third-country-transhipment"));
        private IWebElement rdoIMAnimalFeeding => _driver.WaitForElement(By.XPath("//*[@id='internalMarketanimal']/following-sibling::label"));
        private IWebElement rdoIMOther => _driver.WaitForElement(By.XPath("//*[@id='internalMarketother']/following-sibling::label"));
        private IWebElement rdoIMPharma => _driver.WaitForElement(By.XPath("//*[@id='internalMarketpharma']/following-sibling::label"));
        private IWebElement rdoIMTechnicalUse => _driver.WaitForElement(By.XPath("//*[@id='internalMarkettechnical']/following-sibling::label")); 
        private IWebElement GetReasonRadioButton(string reasonText) =>
            _driver.FindElement(By.XPath($"//label[contains(@class, 'govuk-radios__label') and contains(normalize-space(), '{reasonText}')]"));
        private IWebElement GetInternalMarketSubOption(string subOptionText) =>
            _driver.FindElement(By.XPath($"//div[contains(@id,'internalmarket-conditional')]//label[contains(@class, 'govuk-radios__label') and normalize-space()='{subOptionText}']"));
        private IWebElement txtDay => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-date-day"));
        private IWebElement txtMonth => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-date-month"));
        private IWebElement txtYear => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-date-year"));
        private IWebElement txtHours => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-time-hour"));
        private IWebElement txtMinutes => _driver.WaitForElement(By.Id("estimated-arrival-at-port-of-exit-time-minutes"));
        private By txtPointOfExitBy => By.Id("point-of-exit");
        private IWebElement txtPointOfExit => _driver.WaitForElement(txtPointOfExitBy);
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
            txtDay.Click();
            txtDay.SendKeys(day);
            txtMonth.Click();
            txtMonth.SendKeys(month);
            txtYear.Click();
            txtYear.SendKeys(year);
        }

        public void EnterConsignmentLeavingTime(string hours, string minutes)
        {
            txtHours.Click();
            txtHours.SendKeys(hours);
            txtMinutes.Click();
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

        public void SelectTranshipmentDestination(string transhipmentCountry)
        {
            new SelectElement(txtTranshipmentDestination).SelectByText(transhipmentCountry);
        }

        public void AddPlaceOfExit(string placeOfExit)
        {
            _driver.WaitForElementCondition(ExpectedConditions.ElementIsVisible(txtPointOfExitBy));
            txtPointOfExit.SendKeys(placeOfExit);
        }


        
    }
}