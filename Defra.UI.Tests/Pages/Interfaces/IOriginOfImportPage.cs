namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IOriginOfImportPage
    {
        bool IsPageLoaded();
        void IsRegionOfOriginCodeNeeded(string option);
        void IsConformToRegulatoryRequirements(string option);
        void IsItAfterBCP(string option);
        bool IsConsignmentRefNumAdded { get; }
        string GetConsignmentRefNum { get; }
        void EnterConsignmentRefNum(string refNum);
        void ClickBrowserForwardButton();
        void ClickSaveAndReturnToHub();
        string GetOriginCountryText { get; }
        string GetConsignedCountryText { get; }
        string GetRegionCodeRadioLabelText { get; }
        bool IsRegionCodeRadioSelected(string regionCodeRadioOption);
        void SelectConsignedCountry(string consignedCountry);
        bool IsRegionCodeDefaultedToNo { get; }
        bool IsCountryOfOriginPrePopulated(string expectedCountry);
        bool IsHeaderBannerDisplayed(string expectedStatus, string expectedChedType);
    }
}