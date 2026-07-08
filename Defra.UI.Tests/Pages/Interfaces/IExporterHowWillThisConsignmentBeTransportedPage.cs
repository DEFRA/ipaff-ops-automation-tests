namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterHowWillThisConsignmentBeTransportedPage
    {
        bool IsPageLoaded();
        void SelectAnyTransportMethod();
        void ClickSaveAndContinueButton();
    }
}