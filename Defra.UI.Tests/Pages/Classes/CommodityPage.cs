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
using Faker;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CommodityPage : ICommodityPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement txtCommodityCode => _driver.WaitForElement(By.Id("commodity-code-search"));
        private IWebElement btnSearch => _driver.WaitForElement(By.XPath("//button[normalize-space()='Search']"));
        private IWebElement txtCommodityCodeValue => _driver.WaitForElement(By.Id("commodity-code-value"));
        private IWebElement txtCommodityCodeDesc => _driver.WaitForElement(By.XPath("//*[@id='commodity-code-value']/following-sibling::td"));
        private IWebElement txtCommoditySpecies => _driver.FindElement(By.XPath("//*[@id='Bison bison-checkbox']"));
        private IWebElement rdoYesAnotherCommodity => _driver.FindElement(By.XPath("//*[@id='addCommodity']"));
        private IWebElement rdoNoAnotherCommodity => _driver.FindElement(By.XPath("//*[@id='addCommodity-2']"));
        private IWebElement txtEnteredCommodityValue => _driver.WaitForElement(By.XPath("//*[@class='govuk-table__row  ']/td[1]"));
        private IWebElement txtEnteredCommodityDesc => _driver.WaitForElement(By.XPath("//*[@class='govuk-table__row  ']/td[2]"));
        private IWebElement txtNetWeight => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']//input[@class='govuk-input net-weight ']"));
        private IWebElement txtNumberOfPackages => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']//input[@class='govuk-input number-of-packages ']"));
        private IWebElement ddlPackageType => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']//*[@class='govuk-select type-of-package govuk-!-width-full']"));
        private IWebElement btnUpdateTotal => _driver.FindElement(By.XPath("//*[@class='commodity-detail-form-desktop']/div[3]//button"));
        private IWebElement txtTotalGrossWeight => _driver.FindElement(By.XPath("//*[@id='gross-weight-desktop']"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//*[@id='button-save-and-continue-desktop']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CommodityPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("Commodity");
        }

        public void EnterCommodityCode(string code)
        {
            txtCommodityCode.Click();
            txtCommodityCode.SendKeys(code);
            btnSearch.Click();
        }

        public bool VerifyCommdityDetails(string code, string description)
        {
            return txtCommodityCodeValue.Text.Contains(code)
                && txtCommodityCodeDesc.Text.Contains(description);
        }

        public void SelectCommoditySpecies(string species)
        {
            txtCommoditySpecies.Click();
        }

        public void AddAnotherCommodity(string option)
        {
            if (option.Equals("Yes"))
                rdoYesAnotherCommodity.Click();
            else if (option.Equals("No"))
                rdoNoAnotherCommodity.Click();
        }

        public bool VerifyEnteredCommdityDetails(string code, string description)
        {
            return txtEnteredCommodityValue.Text.Contains(code)
                && txtEnteredCommodityDesc.Text.Contains(description);
        }

       
        public void EnterNetWeight(string weight)
        { 
            txtNetWeight.Click(); 
            txtNetWeight.Clear(); 
            txtNetWeight.SendKeys(weight); 
        }

        
        public void EnterNumberOfPackages(string packages)
        {
            txtNumberOfPackages.Click(); 
            txtNumberOfPackages.Clear(); 
            txtNumberOfPackages.SendKeys(packages); 
        }

        public void SelectPackageType(string type)
        { 
            new SelectElement(ddlPackageType).SelectByText(type); 
        }

        public void ClickUpdateTotal()
        { 
            btnUpdateTotal.Click(); 
        }
      
        public void EnterTotalGrossWeight(string weight)
        { 
            txtTotalGrossWeight.Click(); 
            txtTotalGrossWeight.Clear(); 
            txtTotalGrossWeight.SendKeys(weight); 
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }
    }
}