namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterAddBulbsToYourConsignmentPage
    {
        bool IsPageLoaded();
        void EnterBotanicalName(string botanicalName);
        string GetSelectedCommodityDescription();
        void SelectCommodityType(string commodityType);
        void EnterQuantity(string quantity);
        void SelectUnitOfMeasurement(string unit);
        void EnterNumberOfPackages(string numberOfPackages);
        void SelectTypeOfPackaging(string packagingType);
        void SelectCountriesOfOrigin(string countries);
        void ClickSaveAndContinueButton();
    }
}
