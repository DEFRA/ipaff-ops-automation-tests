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
        private List<IWebElement> commodityTreeList => _driver.WaitForElements(By.XPath("//ul[@class='commodity-tree']/li//span/button")).ToList();
        private List<IWebElement> parentCommodityItemList => _driver.WaitForElements(By.XPath("//div[@class='commodity-list']/ul/li")).ToList();
        private IWebElement txtNumberOfAnimals => _driver.FindElement(By.XPath("//input[contains(@class, 'number-of-animals') and contains(@id, 'num-animals-desktop')]"));
        private IWebElement btnUpdateTotal => _driver.FindElement(By.Id("update-total-desktop"));
        private IWebElement addCommodityLink => _driver.WaitForElement(By.XPath("//*[@Id='add-commodity-desktop' or @Id='add-commodity']"));
        private IWebElement txtTotalGrossWeight => _driver.FindElement(By.XPath("//*[@Id='gross-weight-desktop' or @Id='gross-weight']"));
        private IWebElement txtSubtotalNetWeight => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']/tfoot//td[2]"));
        private List<IWebElement> txtSubtotalsNetWeight => _driver.WaitForElements(By.XPath("//*[@class='govuk-table species-table-cheda']/tfoot//td[2]")).ToList();
        private IWebElement txtSubtotalPackages => _driver.FindElement(By.XPath("//*[@class='govuk-table species-table-cheda']/tfoot//td[3]"));
        private List<IWebElement> txtSubtotalsPackages => _driver.FindElements(By.XPath("//*[@class='govuk-table species-table-cheda']/tfoot//td[3]")).ToList();
        private IWebElement txtTotalNetWeight => _driver.FindElement(By.XPath("//*[@id='commodity-details-page']//form[2]//*[normalize-space()='Net weight (kg/units)']/following-sibling::dt"));
        private IWebElement txtTotalPackages => _driver.FindElement(By.XPath("//*[@id='commodity-details-page']//form[2]//*[normalize-space()='Number of packages']/following-sibling::dt"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("button-save-and-continue-desktop"));
        private IWebElement btnSaveAndReturnToHub => _driver.WaitForElement(By.Id("save-and-return-button-desktop"));
        private List<IWebElement> addedCommoditiesList => _driver.FindElements(By.XPath("//table[@id='commodity-table-desktop']/tbody/tr")).ToList();
        private IWebElement rdoManual => _driver.FindElement(By.XPath("//*[@id='input-method']/following-sibling::label"));
        private IWebElement rdoUpload => _driver.FindElement(By.XPath("//*[@id='input-method-2']/following-sibling::label"));
        private IWebElement btnCommCodeSearch => _driver.FindElement(By.Id("commodity-code-search-tab-link"));
        private IWebElement txtDisplayedCHEDPPCommodityCode => _driver.FindElement(By.Id("commodity-code-value"));
        private IWebElement txtDisplayedCHEDPPCommodityDesc => _driver.FindElement(By.XPath("//*[@id='commodity-code-value']/following-sibling::td"));
        private IWebElement txtEPPOCode => _driver.FindElement(By.Id("eppo-code"));
        private IWebElement btnEPPOCodeSearch => _driver.FindElement(By.Id("search"));
        private IWebElement lnkAddEPPOCode(string eppoCode) => _driver.FindElement(By.XPath($"//*[normalize-space()='{eppoCode}']/following-sibling::td/button"));
        private IWebElement txtDisplayedSpeciesTable => _driver.FindElement(By.Id("selected-species-table"));
        private IWebElement drpVariety(string eppoCode) => _driver.FindElement(By.Id($"add-variety-{eppoCode}"));
        private IWebElement drpClass(string eppoCode) => _driver.FindElement(By.Id($"add-class-{eppoCode}"));
        private IWebElement txtSelectedCommodity(string commodity) => _driver.FindElement(By.XPath($"//h2[normalize-space()='{commodity}']"));
        private IWebElement txtSelectedCommodityDetails(string commodity) => _driver.FindElement(By.XPath($"//h2[normalize-space()='{commodity}']/following-sibling::div[1]"));
        private IWebElement chkBoxCommodity(string commodity) => _driver.FindElement(By.XPath($"//td[normalize-space()='{commodity}']/../td[1]//input"));
        private IWebElement txtCHEDPPNetWeight => _driver.FindElement(By.Id("bulk-net-weight"));
        private IWebElement txtCHEDPPNumOfPackages => _driver.FindElement(By.Id("bulk-num-packages"));
        private IWebElement drpCHEDPPTypeOfPackages => _driver.FindElement(By.Id("bulk-package-type"));
        private IWebElement txtCHEDPPQuantity => _driver.FindElement(By.Id("bulk-quantity"));
        private IWebElement drpCHEDPPQuantityType => _driver.FindElement(By.Id("bulk-quantity-type"));
        private IWebElement btnApply => _driver.FindElement(By.Id("apply-bulk-commodity-details"));
        private IWebElement txtTotalNetWeightCHEDPP => _driver.FindElement(By.XPath("//*[normalize-space()='Net weight (kg)']/following-sibling::td"));
        private IWebElement txtTotalPackagesCHEDPP => _driver.FindElement(By.XPath("//*[normalize-space()='Number of packages']/following-sibling::td"));
        private IWebElement lnkCancel => _driver.FindElement(By.LinkText("Cancel"));
        private IReadOnlyCollection<IWebElement> speciesRows => _driver.FindElements(By.XPath("//table[contains(@class, 'species-table-cheda')]//tbody/tr[contains(@id, '-desktop')]"));
        private By speciesNumAnimalsInputBy(string speciesId) => By.XPath($"//input[contains(@id, '{speciesId}.num-animals-desktop')]");
        private By speciesNumPackagesInputBy(string speciesId) => By.XPath($"//input[contains(@id, '{speciesId}.num-packages-desktop')]");
        private IWebElement speciesNetWeightInput(string commodityCode) => _driver.FindElement(By.XPath($"//input[starts-with(@id, '{commodityCode}') and contains(@id, '.net-weight-desktop')]"));
        private IWebElement speciesNumPackagesInput(string commodityCode) => _driver.FindElement(By.XPath($"//input[starts-with(@id, '{commodityCode}') and contains(@id, '.num-packages-desktop')]"));
        private By speciesPackageTypeSelectBy(string commodityCode) => By.XPath($"//select[starts-with(@id, '{commodityCode}-') and contains(@id, '.package-type-desktop')]");
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
            Thread.Sleep(1000);
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
            speciesNetWeightInput(commodityCode).SendKeys(netWeight);
        }

        public void AddNumOfPackagesForCommodityCode(string numOfPackages, string commodityCode)
        {
            speciesNumPackagesInput(commodityCode).SendKeys(numOfPackages);
        }

        public void SelectPackageTypeForCommodityCode(string typeOfPackage, string commodityCode)
        {
            new SelectElement(speciesTable.FindElement(speciesPackageTypeSelectBy(commodityCode))).SelectByText(typeOfPackage);
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

        public void ClickCancelLink()
        {
            lnkCancel.Click();
        }

        public bool SelectCommodityInTheCommTree(string commodity)
        {
            foreach (var comm in commodityTreeList)
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

        public void EnterNumberOfAnimalsForSpecies(string numberOfAnimals, string species)
        {
            var speciesId = GetSpeciesRowId(species);
            var input = _driver.FindElement(speciesNumAnimalsInputBy(speciesId));
            input.Click();
            input.Clear();
            input.SendKeys(numberOfAnimals);
        }

        public void EnterNumberOfPackagesForSpecies(string numberOfPackages, string species)
        {
            var speciesId = GetSpeciesRowId(species);
            var input = _driver.FindElement(speciesNumPackagesInputBy(speciesId));
            input.Click();
            input.Clear();
            input.SendKeys(numberOfPackages);
        }

        /// <summary>
        /// Finds the species row ID (e.g. "01061900-568113") by matching the species name in the desktop table.
        /// </summary>
        private string GetSpeciesRowId(string species)
        {
            var row = speciesRows.FirstOrDefault(r => r.Text.Contains(species))
                ?? throw new NoSuchElementException($"Species row not found for '{species}'");

            var id = row.GetAttribute("id");
            return id.Replace("species-", "").Replace("-desktop", "");
        }

        public string GetSubtotalNetWeight()
        {
            return txtSubtotalNetWeight.Text.Trim();
        }

        public string[] GetSubtotalsOfNetWeight()
        {
            return txtSubtotalsNetWeight.Select(element => element.Text).ToArray();
        }

        public string GetSubtotalPackages()
        {
            return txtSubtotalPackages.Text.Trim();
        }

        public string[] GetSubtotalsOfPackages()
        {
            return txtSubtotalsPackages.Select(element => element.Text).ToArray();
        }

        public string GetTotalNetWeight()
        {
            return txtTotalNetWeight.Text.Trim();
        }

        public string GetTotalPackages()
        {
            return txtTotalPackages.Text.Trim();
        }

        public string GetCHEDPPTotalNetWeight()
        {
            return txtTotalNetWeightCHEDPP.Text.Trim();
        }

        public string GetCHEDPPTotalPackages()
        {
            return txtTotalPackagesCHEDPP.Text.Trim();
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

        public void SelectHowToAddCommodityOption(string option)
        {
            if (option.Equals(rdoManual.Text.Trim()))
                rdoManual.Click();
            else if (option.Equals(rdoUpload.Text.Trim()))
                rdoUpload.Click();
        }

        public bool IsHowToAddCommodityPageLoaded()
        {
            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("How do you want to add your commodity details?");
        }

        public void ClickCommodityCodeSearchTab()
        {
            btnCommCodeSearch.Click();
        }

        public bool VerifyCHEDPPCommodityDetails(string code, string description)
        {
            return txtDisplayedCHEDPPCommodityCode.Text.Contains(code)
                && txtDisplayedCHEDPPCommodityDesc.Text.Contains(description);
        }

        public void SearchEppoCode(string eppoCode)
        {
            txtEPPOCode.SendKeys(eppoCode);
            btnEPPOCodeSearch.Click();
        }

        public void ClickAddLink(string eppoCode)
        {
            lnkAddEPPOCode(eppoCode).Click();
        }

        public bool VerifyGenusSpeciesEPPOCode(string genus, string eppoCode)
        {
            return txtDisplayedSpeciesTable.Text.Contains(genus)
                && txtDisplayedSpeciesTable.Text.Contains(eppoCode);
        }

        public void SelctEPPOCode(string eppoCodeCheckBox)
        {
            txtCommoditySpecies(eppoCodeCheckBox).Click();
        }

        public bool IsVarietyAndClassOfCommodityPageLoaded()
        {
            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("Variety and class of commodity");
        }

        public void SelectVariety(string variety, string eppoCode)
        {
            new SelectElement(drpVariety(eppoCode)).SelectByText(variety);
        }

        public void SelectClass(string classOfEPPO, string eppoCode)
        {
            new SelectElement(drpClass(eppoCode)).SelectByText(classOfEPPO);
        }

        public bool VerifySelectedCommoditiesDisplayed(string firstComm, string secondComm, string firstCode, string secondCode, string firstEPPO, string secondEPPO, string firstGenus, string secondGenus)
        {
            return txtSelectedCommodity(firstComm).Text.Trim().Equals(firstComm)
                && txtSelectedCommodityDetails(firstComm).Text.Contains(firstCode)
                && txtSelectedCommodityDetails(firstComm).Text.Contains(firstEPPO)
                && txtSelectedCommodityDetails(firstComm).Text.Contains(firstGenus)
                && txtSelectedCommodity(secondComm).Text.Trim().Equals(secondComm)
                && txtSelectedCommodityDetails(secondComm).Text.Contains(secondCode)
                && txtSelectedCommodityDetails(secondComm).Text.Contains(secondEPPO)
                && txtSelectedCommodityDetails(secondComm).Text.Contains(secondGenus);
        }

        public void SelectCommodities(string firstCommCode, string secondCommCode)
        {
            chkBoxCommodity(firstCommCode).Click();
            chkBoxCommodity(secondCommCode).Click();
        }

        public void EnterCHEDPPNetWeight(string weight)
        {
            txtCHEDPPNetWeight.Click();
            txtCHEDPPNetWeight.SendKeys(weight);
        }

        public void EnterCHEDPPNumberOfPackages(string numberOfPackages)
        {
            txtCHEDPPNumOfPackages.Click();
            txtCHEDPPNumOfPackages.SendKeys(numberOfPackages);
        }

        public void SelectCHEDPPPackageType(string packageType)
        {
            new SelectElement(drpCHEDPPTypeOfPackages).SelectByText(packageType);
        }

        public void EnterCHEDPPQuantity(string quantity)
        {
            txtCHEDPPQuantity.Click();
            txtCHEDPPQuantity.SendKeys(quantity);
        }

        public void SelectCHEDPPQuanityType(string type)
        {
            new SelectElement(drpCHEDPPQuantityType).SelectByText(type);
        }

        public void ClickApplyButton()
        {
            btnApply.Click();
        }
    }
}