namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICommodityPage
    { 
        bool IsPageLoaded();
        void EnterCommodityCode(string code);
        bool VerifyCommodityDetails(string code, string description);
        void SelectTypeOfCommodity(string type);
        void SelectCommoditySpecies(string species);
        void AddAnotherCommodity(string option);
        bool VerifyEnteredCommdityDetails(List<string> code, List<string> description);
        void EnterNetWeight(List<string> weight);
        void EnterNumberOfPackages(List<string> packages);
        void SelectPackageType(List<string> type);
        void AddNetWeightForCommodityCode(string netWeight, string commodityCode);
        void AddNumOfPackagesForCommodityCode(string numOfPackages, string commodityCode);
        void SelectPackageTypeForCommodityCode(string typeOfPackage, string commodityCode);
        void ClickUpdateTotal();
        void EnterTotalGrossWeight(string weight);
        void ClickSaveAndContinue();
        void ClickSaveAndReturnToHub();
        void ClickBrowserBackButton();
        void ClickAddCommodityLink();
        bool SelectCommodityInTheCommTree(string commodity);
        bool IsSubCommodityListDisplayed();
        void EnterNumberOfAnimals(string numberOfAnimals);
        string GetSubtotalNetWeight();
        string GetSubtotalPackages();
        string GetTotalNetWeight();
        string GetTotalPackages();
        int GetAddedCommoditiesCount { get; }
        bool VerifyTotalNetWeight(string totalNetWeight);
        bool VerifyNumberOfPackages(string numOfPackages);
        string[] GetSubtotalsOfNetWeight();
        string[] GetSubtotalsOfPackages();
        void SelectHowToAddCommodityOption(string option);
        bool IsHowToAddCommodityPageLoaded();
        void ClickCommodityCodeSearchTab();
        bool VerifyCHEDPPCommodityDetails(string code, string description);
        void SearchEppoCode(string eppoCode);
        void ClickAddLink(string eppoCode);
        bool VerifyGenusSpeciesEPPOCode(string genus, string eppoCode);
        void SelctEPPOCode(string eppoCodeCheckBox);
        bool IsVarietyAndClassOfCommodityPageLoaded();
        void SelectVariety(string variety, string eppoCode);
        void SelectClass(string classOfEPPO, string eppoCode);
        bool VerifySelectedCommoditiesDisplayed(string firstComm, string secondComm, string firstCode, string secondCode, string firstEPPO, string secondEPPO, string firstGenus, string secondGenus);
        void SelectCommodities(string firstCommCode, string secondCommCode);
        void EnterCHEDPPNetWeight(string weight);
        void EnterCHEDPPNumberOfPackages(string numberOfPackages);
        void SelectCHEDPPPackageType(string packageType);
        void EnterCHEDPPQuantity(string quantity);
        void SelectCHEDPPQuanityType(string type);
        void ClickApplyButton();
        string GetCHEDPPTotalNetWeight();
        string GetCHEDPPTotalPackages();
        void ClickCancelLink();
    }
}