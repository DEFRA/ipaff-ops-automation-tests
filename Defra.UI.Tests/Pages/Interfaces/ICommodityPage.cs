using Faker;
using OpenQA.Selenium;

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
        void ClickUpdateTotal();
        void EnterTotalGrossWeight(string weight);
        void ClickSaveAndContinue();
        string GetSubtotalNetWeight();
        string GetSubtotalPackages();
        string GetTotalNetWeight();
        string GetTotalPackages();
    }
}
