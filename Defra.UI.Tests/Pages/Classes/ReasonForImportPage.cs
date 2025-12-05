using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


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
        private IWebElement rdoIMAnimalFeeding => _driver.WaitForElement(By.XPath("//*[@id='internalMarketanimal']/following-sibling::label"));
        private IWebElement rdoIMOther => _driver.WaitForElement(By.XPath("//*[@id='internalMarketother']/following-sibling::label"));
        private IWebElement rdoIMPharma => _driver.WaitForElement(By.XPath("//*[@id='internalMarketpharma']/following-sibling::label"));
        private IWebElement rdoIMTechnicalUse => _driver.WaitForElement(By.XPath("//*[@id='internalMarkettechnical']/following-sibling::label")); 
        private IWebElement GetReasonRadioButton(string reasonText) =>
            _driver.FindElement(By.XPath($"//label[contains(@class, 'govuk-radios__label') and contains(normalize-space(), '{reasonText}')]"));
        private IWebElement GetInternalMarketSubOption(string subOptionText) =>
            _driver.FindElement(By.XPath($"//div[contains(@id,'internalmarket-conditional')]//label[contains(@class, 'govuk-radios__label') and normalize-space()='{subOptionText}']"));
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
    }
}