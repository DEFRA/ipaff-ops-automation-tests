namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterAddToYourConsignmentPage
    {
        bool IsPageLoaded(string commodity);
        void EnterBotanicalName(string botanicalName);
        string GetSelectedCommodityDescription();
        string GetSelectedCommodityEppoCode();
        void SelectCommodityType(string commodityType);
        void EnterVariety(string variety);
        void EnterLotReference(string lotReference);
        void EnterSampleReference(string sampleReference);
        void EnterQuantity(string quantity);
        void SelectUnitOfMeasurement(string unit);
        void SelectChemicallyTreated(string option);
        void EnterChemicalsUsed(string chemicals);
        void SelectCountryWhereGrown(string country);
        void SelectDestinationCountries(string countries);
        void EnterNumberOfPackages(string numberOfPackages);
        void SelectTypeOfPackaging(string packagingType);
        void SelectCountriesOfOrigin(string countries);
        List<string> GetUnitOfMeasurementOptions();
        bool AreSeedCommodityDetailsDisplayed(string botanicalName, string variety, string lotReference, string quantity, string unit,
            string chemicallyTreated, string chemicalsUsed, string countryWhereGrown, string destinationCountries);
        void EnterCommodityDetails(string varietyType, string specificVariety, string qualityClass, string countryOfOrigin,
            string netWeightPerPackage, string numberOfPackages, string typeOfPackaging, string reusablePackagingOptionValue);
        void DescribeCommodity(string commonName, string botanicalName, string countryOfOrigin, string netWeightPerPackage,
            string numberOfPackages, string typeOfPackaging, string reusablePackagingOptionValue);
        void ClickSaveAndContinueButton();
        bool AreRadioOptionsDisplayed(params string[] radioIds);
        string GetRadioLabelText(string radioId);
        bool IsUnitOfMeasurementDropdownDisplayed();
        bool IsTextboxFieldDisplayed(string fieldName);
        void DescribePotato(string potatoType, string stockNumber, string variety, string gsiNumber, string chemicalUsed,
            string quantity, string unit, string numberOfPackages, string typeOfPackaging, string packingMaterial, string countriesOfOrigin);
        bool ArePotatoCommodityDetailsDisplayed(string potatoType, string stockNumber, string variety, string gsiNumber, string chemicalUsed,
            string quantity, string unit, string numberOfPackages, string typeOfPackaging, string packingMaterial, string countriesOfOrigin);
    }
}
