namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterHowWillThisConsignmentBeTransportedPage
    {
        bool IsPageLoaded();
        void SelectAnyTransportMethod();
        void SelectTransportMethod(string method);
        void EnterConsignmentValue(decimal value);
        void SelectDoYouKnowThePointOfEntry(string option);
        void EnterPointOfEntry(string pointOfEntry);
        void EnterContainerNumber(string containerNumber);
        void ClickSaveAndContinueButton();
    }
}