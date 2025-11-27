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
    public class OriginOfImportPage : IOriginOfImportPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.FindElement(By.XPath("//*[@id='page-primary-title']"));
        private IWebElement secondaryTitle => _driver.FindElement(By.XPath("//*[@id='page-secondary-title']"));
        private IWebElement rdoYesRegionCode => _driver.FindElement(By.XPath("//*[@id='region-code-yes']"));
        private IWebElement rdoNoRegionCode => _driver.FindElement(By.XPath("//*[@id='region-code-no']"));
        private IWebElement rdoYesConsignmentConform => _driver.FindElement(By.XPath("//fieldset[legend[contains(text(),'Does this consignment conform')]]//label[normalize-space(.)='Yes']"));
        private IWebElement rdoNoConsignmentConform => _driver.FindElement(By.XPath("//fieldset[legend[contains(text(),'Does this consignment conform')]]//label[normalize-space(.)='No']"));
        private IWebElement rdoYesAfterBCP => _driver.FindElement(By.XPath("//*[@id='transport-details-required']"));
        private IWebElement rdoNoAfterBCP => _driver.FindElement(By.XPath("//*[@id='transport-details-required-2']"));
        private IWebElement txtReferenceNumber => _driver.FindElement(By.XPath("//*[@id='local-reference-number']"));
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

        public void EnterConsignmentRefNum(string refNum)
        {
            txtReferenceNumber.Click();
            txtReferenceNumber.SendKeys(refNum);
        }
    }
}