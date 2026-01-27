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
        void EnterEarTag(string earTag);
        string GetSubtotalNetWeight();
        string GetSubtotalPackages();
        string GetTotalNetWeight();
        string GetTotalPackages();
        int GetAddedCommoditiesCount { get; }
        bool VerifyTotalNetWeight(string totalNetWeight);
        bool VerifyNumberOfPackages(string numOfPackages);
        string[] GetSubtotalsOfNetWeight();
        string[] GetSubtotalsOfPackages();
    }
}