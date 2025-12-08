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
        private IWebElement rdoNonInternalMarket => _driver.WaitForElement(By.XPath("//*[@id='radio-noninternalmarket']/following-sibling::label"));
        private IWebElement rdoIMAnimalFeeding => _driver.WaitForElement(By.XPath("//*[@id='internalMarketanimal']/following-sibling::label"));
        private IWebElement rdoIMOther => _driver.WaitForElement(By.XPath("//*[@id='internalMarketother']/following-sibling::label"));
        private IWebElement rdoIMPharma => _driver.WaitForElement(By.XPath("//*[@id='internalMarketpharma']/following-sibling::label"));
        private IWebElement rdoIMTechnicalUse => _driver.WaitForElement(By.XPath("//*[@id='internalMarkettechnical']/following-sibling::label"));
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
            return IsElementPresent(rdoInternalMarket)
                && IsElementPresent(rdoTranshipment)
                && IsElementPresent(rdoTransit)
                && IsElementPresent(rdoReentry);
        }

        public bool AreImportReasonsForCHEDDPreset()
        {
            return IsElementPresent(rdoInternalMarket)
                && IsElementPresent(rdoNonInternalMarket);
        }

        public void SelectReasonForImport(string option)
        {
            if(option.Equals(rdoInternalMarket.Text))
                rdoInternalMarket.Click();
            else if (option.Equals(rdoTranshipment.Text))
                rdoTranshipment.Click();
            else if (option.Equals(rdoTransit.Text))
                rdoTransit.Click();
            else if (option.Equals(rdoReentry.Text))
                rdoReentry.Click();
        }

        public void SelectReasonForImportSubOption(string subOption)
        {
            if (subOption.Equals(rdoIMAnimalFeeding.Text))
                rdoIMAnimalFeeding.Click();
            else if (subOption.Equals(rdoIMOther.Text))
                rdoIMOther.Click();
            else if (subOption.Equals(rdoIMPharma.Text))
                rdoIMPharma.Click();
            else if (subOption.Equals(rdoIMTechnicalUse.Text))
                rdoIMTechnicalUse.Click();
        }
    }
}