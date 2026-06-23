namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhatAreYouExportingPage
    {
        bool IsPageLoaded();
        void SelectExportType(string exportType);
        void ClickContinueButton();
    }
}