namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterAddCommoditiesToYourConsignmentPage
    {
        bool IsPageLoaded();
        void EnterCommodityDetails(
            string varietyType,
            string specificVariety,
            string qualityClass,
            string countryOfOrigin,
            string netWeightPerPackage,
            string numberOfPackages,
            string typeOfPackaging,
            string reusablePackagingOption);
        void DescribeCommodity(
            string commonName,
            string botanicalName,
            string countryOfOrigin,
            string netWeightPerPackage,
            string numberOfPackages,
            string typeOfPackaging,
            string reusablePackagingOption);
        void ClickSaveAndContinueButton();
    }
}