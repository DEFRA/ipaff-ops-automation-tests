using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using Defra.UI.Tests.HelperMethods;
using OpenQA.Selenium.Support.UI;

namespace Defra.UI.Tests.Pages.Classes
{
    public class OriginOfImportPage : IOriginOfImportPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement rdoYesRegionCode => _driver.FindElement(By.Id("region-code-yes"));
        private IWebElement rdoNoRegionCode => _driver.FindElement(By.Id("region-code-no"));
        private IWebElement rdoRegionCode(string regionCodeRadioOption) => _driver.FindElement(By.XPath($"//label[contains(text(),'{regionCodeRadioOption}')]/preceding-sibling::input"));
        private IWebElement rdoYesConsignmentConform => _driver.FindElement(By.XPath("//fieldset[legend[contains(text(),'Does this consignment conform')]]//label[normalize-space(.)='Yes']"));
        private IWebElement rdoNoConsignmentConform => _driver.FindElement(By.XPath("//fieldset[legend[contains(text(),'Does this consignment conform')]]//label[normalize-space(.)='No']"));
        private IWebElement rdoYesAfterBCP => _driver.FindElement(By.Id("transport-details-required"));
        private IWebElement rdoNoAfterBCP => _driver.FindElement(By.Id("transport-details-required-2"));
        private IWebElement txtReferenceNumber => _driver.FindElement(By.Id("local-reference-number"));
        private IWebElement btnSaveAndReviewToHub => _driver.FindElement(By.Id("save-and-return-button"));
        private IWebElement originCountrySelectedVal => _driver.FindElement(By.XPath("//select[@id='origin-country']/option[@selected]"));
        private IWebElement consignedCountrySelectedVal => _driver.FindElement(By.XPath("//select[@id='consigned-country']/option[@selected]"));
        private IWebElement ddlConsignedCountry => _driver.FindElement(By.Id("consigned-country"));
        private IWebElement selectedRadioLabel => _driver.FindElement(By.XPath("//input[contains(@class,'govuk-radios__input') and @checked]/following-sibling::label"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public OriginOfImportPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("About the consignment")
                && primaryTitle.Text.Contains("Origin of the import");
        }

        public void IsRegionOfOriginCodeNeeded(string option)
        {
            if (option.Equals("Yes"))
                rdoYesRegionCode.Click();
            else if (option.Equals("No"))
                rdoNoRegionCode.Click();
        }

        public void IsConformToRegulatoryRequirements(string option)
        {
            if (option.Equals("Yes"))
                rdoYesConsignmentConform.Click();
            else if (option.Equals("No"))
                rdoNoConsignmentConform.Click();
        }

        public void IsItAfterBCP(string option)
        {
            if (option.Equals("Yes"))
                rdoYesAfterBCP.Click();
            else if (option.Equals("No"))
                rdoNoAfterBCP.Click();
        }

        public bool IsConsignmentRefNumAdded => !string.IsNullOrEmpty(txtReferenceNumber.GetAttribute("value"));

        public string GetConsignmentRefNum => txtReferenceNumber?.GetAttribute("value").Trim() ?? string.Empty;

        public void EnterConsignmentRefNum(string refNum)
        {
            txtReferenceNumber.SendKeys(refNum);
        }

        public void ClickBrowserForwardButton()
        {
            _driver.ClickBrowserForwardButton();
        }

        public void ClickSaveAndReturnToHub()
        {
            btnSaveAndReviewToHub.Click();
        }

        public string GetOriginCountryText => originCountrySelectedVal.Text.Trim();

        public string GetConsignedCountryText => consignedCountrySelectedVal.Text.Trim();

        public string GetRegionCodeRadioLabelText => selectedRadioLabel.Text.Trim() ?? string.Empty;

        public bool IsRegionCodeRadioSelected(string regionCodeRadioOption)
        {
            return rdoRegionCode(regionCodeRadioOption).GetAttribute("checked") != null;
        }

        public void SelectConsignedCountry(string consignedCountry)
        {
            new SelectElement(ddlConsignedCountry).SelectByText(consignedCountry);
        }

        public bool IsRegionCodeDefaultedToNo => rdoNoRegionCode.GetAttribute("checked") != null;
    }
}