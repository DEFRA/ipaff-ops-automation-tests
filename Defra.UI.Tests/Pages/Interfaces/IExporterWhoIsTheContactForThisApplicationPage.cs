namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhoIsTheContactForThisApplicationPage
    {
        bool IsPageLoaded();
        void EnterContactDetails(string name, string email, string telephone);
        void ClickSaveAndContinueButton();
    }
}