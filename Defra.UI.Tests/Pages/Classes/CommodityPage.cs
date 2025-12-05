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
using Defra.UI.Tests.HelperMethods;
using AngleSharp.Dom;

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
        private IWebElement SpeciesTable => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']"));
        private List<IWebElement> CommodityTreeList => _driver.WaitForElements(By.XPath("//ul[@class='commodity-tree']/li//span[@class='commodity-description-links-container']/button")).ToList();
        private List<IWebElement> ParentCommodityItemList => _driver.WaitForElements(By.XPath("//div[@class='commodity-list']/ul/li")).ToList();
        //private IWebElement btnUpdateTotal => _driver.FindElement(By.XPath("//*[@class='commodity-detail-form-desktop']/div[3]//button"));
        private IWebElement btnUpdateTotal => _driver.FindElement(By.Id("update-total-desktop"));
        private IWebElement txtTotalGrossWeight => _driver.FindElement(By.XPath("//*[@id='gross-weight-desktop']"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//*[@id='button-save-and-continue-desktop']"));
        private IWebElement AddCommodityLink => _driver.WaitForElement(By.Id("add-commodity-desktop"));
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

        public void AddNetWeightForCommodityCode(string netWeight, string commodityCode)
        {
            var netWeightSelector = "//input[@id='" + commodityCode + "-.net-weight-desktop']";
            var netWeightElement = _driver.FindElement(By.XPath(netWeightSelector));
            netWeightElement.SendKeys(netWeight);
            //var typeOfPackageSelectorFull = SpeciesTable.FindElement(By.XPath(typeOfPackageSelector));

            //new SelectElement(netWeightSelector).SelectByText(typeOfPackage);
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
            var typeOfPackageSelectorFull = SpeciesTable.FindElement(By.XPath(typeOfPackageSelector));
            
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

        public void ClickAddCommodityLink() => AddCommodityLink.Click();

        public bool SelectCommodityInTheCommTree(string commodity)
        {
            foreach(var comm in CommodityTreeList)
            {
                var commText = comm.Text;
                Console.WriteLine(commText);
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
            var a = ParentCommodityItemList.Count.Equals(1)
                && CommodityTreeList.Count > 1;

            Console.WriteLine("sub commodity tree displayed" + a);

            return ParentCommodityItemList.Count.Equals(1)
                && CommodityTreeList.Count >= 1;
        }
    }
}