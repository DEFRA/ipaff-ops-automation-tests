namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterYourCommoditiesPage
    {
        bool IsPageLoaded();
        bool IsCommodityLineDisplayed(string commodityName);
        void SelectFinishedAddingCommoditiesOption(string option);
        void ClickSaveAndContinueButton();
    }
}