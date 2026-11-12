namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterStockDetailsPage
    {
        bool IsPageLoaded();
        string GetStockReference();
        string GetSummaryValue(string key);
        void ClickDrawDownButton();
    }
}
