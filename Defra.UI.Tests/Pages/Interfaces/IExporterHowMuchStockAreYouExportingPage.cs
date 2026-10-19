namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterHowMuchStockAreYouExportingPage
    {
        bool IsPageLoaded();
        string GetStockSummaryText();
        void EnterQuantity(string quantity);
        void ClickSaveAndContinueButton();
    }
}
