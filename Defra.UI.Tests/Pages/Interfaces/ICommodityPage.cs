namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICommodityPage
    { 
        bool IsPageLoaded();
        void EnterCommodityCode(string code);
        bool VerifyCommdityDetails(string code, string description);
        void SelectTypeOfCommodity(string type);
        void SelectCommoditySpecies(string species);
        void AddAnotherCommodity(string option);
        bool VerifyEnteredCommdityDetails(string code, string description);
        void EnterNetWeight(string weight);
        void EnterNumberOfPackages(string packages);
        void SelectPackageType(string type);
        void AddNetWeightForCommodityCode(string netWeight, string commodityCode);
        void AddNumOfPackagesForCommodityCode(string numOfPackages, string commodityCode);
        void SelectPackageTypeForCommodityCode(string typeOfPackage, string commodityCode);
        void ClickUpdateTotal();
        void EnterTotalGrossWeight(string weight);
        void ClickSaveAndContinue();
        void ClickBrowserBackButton();
        void ClickAddCommodityLink();
        bool SelectCommodityInTheCommTree(string commodity);
        bool IsSubCommodityListDisplayed();
        string GetSubtotalNetWeight();
        string GetSubtotalPackages();
        string GetTotalNetWeight();
        string GetTotalPackages();
    }
}