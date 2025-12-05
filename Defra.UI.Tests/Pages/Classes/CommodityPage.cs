using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

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
        private IWebElement txtCommodityType => _driver.FindElement(By.Id("type-select"));
        private IWebElement txtCommoditySpecies => _driver.FindElement(By.Id("Bison bison-checkbox"));
        private IWebElement rdoYesAnotherCommodity => _driver.FindElement(By.Id("addCommodity"));
        private IWebElement rdoNoAnotherCommodity => _driver.FindElement(By.Id("addCommodity-2"));
        private IWebElement txtCommoditySpecies(string species) => _driver.FindElement(By.XPath($"//input[@class='govuk-checkboxes__input' and @id='{species}-checkbox']"));
        private IWebElement txtEnteredCommodityValue => _driver.WaitForElement(By.XPath("//*[@class='govuk-table__row  ']/td[1]"));
        private IWebElement txtEnteredCommodityDesc => _driver.WaitForElement(By.XPath("//*[@class='govuk-table__row  ']/td[2]"));
        private IWebElement txtNetWeight => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']//input[@class='govuk-input net-weight ']"));
        private IWebElement txtNumberOfPackages => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']//input[@class='govuk-input number-of-packages ']"));
        private IWebElement ddlPackageType => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']//*[@class='govuk-select type-of-package govuk-!-width-full']"));
        private IWebElement txtNumberOfAnimals => _driver.FindElement(By.XPath("//input[contains(@class, 'number-of-animals') and contains(@id, 'num-animals-desktop')]"));
        private IWebElement txtEarTag => _driver.FindElement(By.XPath("//input[contains(@id, 'ear_tag')]"));
        private IWebElement btnUpdateTotal => _driver.FindElement(By.Id("update-total-desktop"));
        private IWebElement txtTotalGrossWeight => _driver.FindElement(By.Id("gross-weight-desktop"));
        private IWebElement txtSubtotalNetWeight => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']/tfoot//td[2]"));
        private IWebElement txtSubtotalPackages => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']/tfoot//td[3]"));
        private IWebElement txtTotalNetWeight => _driver.FindElement(By.XPath("//*[@id='commodity-details-page']//form[2]//*[normalize-space()='Net weight (kg/units)']/following-sibling::dt"));
        private IWebElement txtTotalPackages => _driver.FindElement(By.XPath("//*[@id='commodity-details-page']//form[2]//*[normalize-space()='Number of packages']/following-sibling::dt"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("button-save-and-continue-desktop"));
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

        public void SelectTypeOfCommodity(string type)
        {
            new SelectElement(txtCommodityType).SelectByText(type);
        }

        public void SelectCommoditySpecies(string species)
        {
            txtCommoditySpecies(species).Click();
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

        public void EnterNumberOfAnimals(string quantity)
        {
            txtNumberOfAnimals.Click();
            txtNumberOfAnimals.Clear();
            txtNumberOfAnimals.SendKeys(quantity);
        }

        public void EnterEarTag(string earTag)
        {
            txtEarTag.Click();
            txtEarTag.Clear();
            txtEarTag.SendKeys(earTag);
        }

        public string GetSubtotalNetWeight()
        {
            return txtSubtotalNetWeight.Text.Trim();
        }

        public string GetSubtotalPackages()
        {
            return txtSubtotalPackages.Text.Trim();
        }

        public string GetTotalNetWeight()
        {
            return txtTotalNetWeight.Text.Trim();
        }

        public string GetTotalPackages()
        {
            return txtTotalPackages.Text.Trim();
        }
    }
}