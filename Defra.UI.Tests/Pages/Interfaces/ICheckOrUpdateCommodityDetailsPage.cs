namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICheckOrUpdateCommodityDetailsPage
    {
        bool IsPageLoaded();
        bool VerifyTotalGrossVolumeIsOptional(string text);
        void EnterGrossWeight(string grossWeight);
        void ClickSaveAndReviewButton();
        string GetControlledAtmosphereContainer();
        string GetGrossVolume();
        string GetGrossVolumeUnit();
    }
}
