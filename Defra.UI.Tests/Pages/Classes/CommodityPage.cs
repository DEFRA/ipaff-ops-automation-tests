using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using Defra.UI.Tests.HelperMethods;

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
        private IWebElement txtEnteredCommodityValue => _driver.WaitForElement(By.XPath("//*[@class='govuk-table__row  ']/td[1]"));
        private IWebElement txtEnteredCommodityDesc => _driver.WaitForElement(By.XPath("//*[@class='govuk-table__row  ']/td[2]"));
        private IWebElement txtNetWeight => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']//input[@class='govuk-input net-weight ']"));
        private IWebElement txtNumberOfPackages => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']//input[@class='govuk-input number-of-packages ']"));
        private IWebElement ddlPackageType => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']//*[@class='govuk-select type-of-package govuk-!-width-full']"));
        private IWebElement speciesTable => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']"));
        private List<IWebElement> commodityTreeList => _driver.WaitForElements(By.XPath("//ul[@class='commodity-tree']/li//span[@class='commodity-description-links-container']/button")).ToList();
        private List<IWebElement> parentCommodityItemList => _driver.WaitForElements(By.XPath("//div[@class='commodity-list']/ul/li")).ToList();
        private IWebElement btnUpdateTotal => _driver.FindElement(By.Id("update-total-desktop"));
        private IWebElement addCommodityLink => _driver.WaitForElement(By.Id("add-commodity-desktop"));
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

        public void AddNetWeightForCommodityCode(string netWeight, string commodityCode)
        {
            var netWeightSelector = "//input[@id='" + commodityCode + "-.net-weight-desktop']";
            var netWeightElement = _driver.FindElement(By.XPath(netWeightSelector));
            netWeightElement.SendKeys(netWeight);
        }

        public void AddNumOfPackagesForCommodityCode(string numOfPackages, string commodityCode)
        {
            var numOfPackageSelector = "//input[@id='" + commodityCode + "-.num-packages-desktop']";
            var numOfPackagesElement = _driver.FindElement(By.XPath(numOfPackageSelector));
            numOfPackagesElement.SendKeys(numOfPackages);
        }

        public void SelectPackageTypeForCommodityCode(string typeOfPackage, string commodityCode)
        {
            var typeOfPackageSelector = "//select[@id='" + commodityCode + "-.package-type-desktop']";
            var typeOfPackageSelectorFull = speciesTable.FindElement(By.XPath(typeOfPackageSelector));
            
            new SelectElement(typeOfPackageSelectorFull).SelectByText(typeOfPackage);
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

        public void ClickBrowserBackButton()
        {
            _driver.ClickBrowserBackButton();
        }

        public void ClickAddCommodityLink() => addCommodityLink.Click();

        public bool SelectCommodityInTheCommTree(string commodity)
        {
            foreach(var comm in commodityTreeList)
            {
                var commText = comm.Text;
                if (commText.Contains(commodity))
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", comm);
                    return true;
                }  
            }
            return false;
        }

        public bool IsSubCommodityListDisplayed()
        {
            return parentCommodityItemList.Count.Equals(1)
                && commodityTreeList.Count >= 1;
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