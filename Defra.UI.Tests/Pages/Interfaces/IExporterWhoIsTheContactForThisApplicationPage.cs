namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhoIsTheContactForThisApplicationPage
    {
        bool IsPageLoaded();
        void EnterContactDetails(string organisationName, string contactName, string telephone);
        void ClickSaveAndContinueButton();
    }
}