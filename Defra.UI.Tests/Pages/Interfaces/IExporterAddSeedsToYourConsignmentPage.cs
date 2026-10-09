namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterAddSeedsToYourConsignmentPage
    {
        bool IsPageLoaded();
        void EnterBotanicalName(string botanicalName);
        string GetSelectedCommodityDescription();
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
        void ClickSaveAndContinueButton();
    }
}
