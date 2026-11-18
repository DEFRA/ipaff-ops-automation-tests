namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterRegulationsForExportingPage
    {
        bool IsPageLoaded(string commodity, string country);
        void TickConfirmRegulationsCheckbox();
        void ClickContinueButton();
        bool IsGuidanceDisplayed(string guidance);
    }
}
