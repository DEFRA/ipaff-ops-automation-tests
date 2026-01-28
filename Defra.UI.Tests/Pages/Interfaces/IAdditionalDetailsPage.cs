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
        void ClickSaveAndReview();
        bool AreAllCommIntendedForRadioOptionsDisplayed(List<string> commOptionsListExpected);
        string GetCommIntendedForRadioLabelText { get; }
        string GetTemperatureRadioLabelText { get; }
    }
}