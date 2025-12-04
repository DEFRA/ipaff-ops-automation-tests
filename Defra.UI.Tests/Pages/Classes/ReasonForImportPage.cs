using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReasonForImportPage : IReasonForImportPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
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