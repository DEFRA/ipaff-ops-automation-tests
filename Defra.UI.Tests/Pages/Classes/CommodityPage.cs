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
        private IWebElement rdoYesAnotherCommodity => _driver.FindElement(By.Id("addCommodity"));
        private IWebElement rdoNoAnotherCommodity => _driver.FindElement(By.Id("addCommodity-2"));
        private IWebElement txtCommoditySpecies(string species) => _driver.FindElement(By.XPath($"//input[@class='govuk-checkboxes__input' and @id='{species}-checkbox']"));
        private IWebElement txtDisplayedCommodityCodeAndDesc => _driver.FindElement(By.Id("commodity-table"));
        private IWebElement txtDisplayedCommodityTable => _driver.FindElement(By.Id("commodity-table-desktop"));
        private IReadOnlyCollection<IWebElement> txtNetWeight => _driver.FindElements(By.XPath("//*[@class='govuk-table species-table-cheda']//input[@class='govuk-input net-weight ']"));
        private IReadOnlyCollection<IWebElement> txtNumberOfPackages => _driver.FindElements(By.XPath("//*[@class='govuk-table species-table-cheda']//input[@class='govuk-input number-of-packages ']"));
        private IReadOnlyCollection<IWebElement> ddlPackageType => _driver.FindElements(By.XPath("//*[@class='govuk-table species-table-cheda']//*[contains(@class,'type-of-package')]"));
        private IWebElement speciesTable => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']"));
        private List<IWebElement> commodityTreeList => _driver.WaitForElements(By.XPath("//ul[@class='commodity-tree']/li//span[@class='commodity-description-links-container']/button")).ToList();
        private List<IWebElement> parentCommodityItemList => _driver.WaitForElements(By.XPath("//div[@class='commodity-list']/ul/li")).ToList();
        private IWebElement txtNumberOfAnimals => _driver.FindElement(By.XPath("//input[contains(@class, 'number-of-animals') and contains(@id, 'num-animals-desktop')]"));
        private IWebElement txtEarTag => _driver.FindElement(By.XPath("//input[contains(@id, 'ear_tag')]"));
        private IWebElement btnUpdateTotal => _driver.FindElement(By.Id("update-total-desktop"));
        private IWebElement addCommodityLink => _driver.WaitForElement(By.Id("add-commodity-desktop"));
        private IWebElement txtTotalGrossWeight => _driver.FindElement(By.Id("gross-weight-desktop"));
        private IWebElement txtSubtotalNetWeight => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']/tfoot//td[2]"));
        private IWebElement txtSubtotalPackages => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']/tfoot//td[3]"));
        private IWebElement txtTotalNetWeight => _driver.FindElement(By.XPath("//*[@id='commodity-details-page']//form[2]//*[normalize-space()='Net weight (kg/units)']/following-sibling::dt"));
        private IWebElement txtTotalPackages => _driver.FindElement(By.XPath("//*[@id='commodity-details-page']//form[2]//*[normalize-space()='Number of packages']/following-sibling::dt"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("button-save-and-continue-desktop"));
        private IWebElement btnSaveAndReturnToHub => _driver.WaitForElement(By.Id("save-and-return-button-desktop"));
        private List<IWebElement> addedCommoditiesList => _driver.FindElements(By.XPath("//table[@id='commodity-table-desktop']/tbody/tr")).ToList();
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
            txtCommodityCode.SendKeys(code);
            btnSearch.Click();
        }

        public bool VerifyCommodityDetails(string code, string description)
        {
            return txtDisplayedCommodityCodeAndDesc.Text.Contains(code)
                && txtDisplayedCommodityCodeAndDesc.Text.Contains(description);
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

        public bool VerifyEnteredCommdityDetails(List<string> code, List<string> description)
        {
            return code.All(item => txtDisplayedCommodityTable.Text.Contains(item));
        }

        public void EnterNetWeight(List<string> weight)
        {
            for (int i = 0; i < weight.Count; i++)
            {
                txtNetWeight.ElementAt(i).Clear();
                txtNetWeight.ElementAt(i).SendKeys(weight[i]);
            }
        }
        public void EnterNumberOfPackages(List<string> packages)
        {
            for (int i = 0; i < packages.Count; i++)
            {
                txtNumberOfPackages.ElementAt(i).Clear();
                txtNumberOfPackages.ElementAt(i).SendKeys(packages[i]);
            }
        }

        public void SelectPackageType(List<string> type)
        {
            for (int i = 0; i < type.Count; i++)
            {
                new SelectElement(ddlPackageType.ElementAt(i)).SelectByText(type[i]);
            }
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
            txtTotalGrossWeight.Clear(); 
            txtTotalGrossWeight.SendKeys(weight); 
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }

        public void ClickSaveAndReturnToHub()
        {
            btnSaveAndReturnToHub.Click();
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

        public int GetAddedCommoditiesCount => addedCommoditiesList?.Count ?? 0;
        
        public bool VerifyTotalNetWeight(string totalNetWeight)
        {
            return txtTotalNetWeight.Text.Trim().Contains(totalNetWeight);
        }

        public bool VerifyNumberOfPackages(string numOfPackages)
        {
            return txtTotalPackages.Text.Trim().Contains(numOfPackages);
        }
    }
}