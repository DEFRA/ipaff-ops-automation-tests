namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhatAreYouExportingSelectOneCommodityAtATimePage
    {
        bool IsPageLoaded();
        void SelectCommodity(string commodity);
        void ClickContinueButton();
    }
}