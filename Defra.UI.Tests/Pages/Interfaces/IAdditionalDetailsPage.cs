namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAdditionalDetailsPage
    {
        bool IsPageLoaded();
        bool IsAdditionalAnimalDetailsPageLoaded();
        void ClickImportingProduct(string option);
        bool SelectCommodityIntendedForRadio(string commIntendedForOption);
        void SelectAnimalCertification(string certificationOption);
        void SelectUnweanedAnimalsOption(string option);
        bool IsUnweanedAnimalsRadioSelected(string unweanedAnimalsOption);
        void ClickSaveAndReview();
        bool AreAllCommIntendedForRadioOptionsDisplayed(List<string> commOptionsListExpected);
        string GetCommIntendedForRadioLabelText { get; }
        string GetTemperatureRadioLabelText { get; }
        string GetGrossWeightValue();
        string GetNetWeight();
        string GetNumberOfPackages();
        string GetGrossVolumeValue();
    }
}