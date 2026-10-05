namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhoIsTheContactForThisApplicationPage
    {
        bool IsPageLoaded();
        void EnterContactDetails(string organisationName, string contactName, string telephone);
        void EnterContactName(string contactName);
        void EnterPhoneNumber(string telephone);
        void ClickSaveAndContinueButton();
    }
}