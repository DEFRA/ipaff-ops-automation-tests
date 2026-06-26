namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConfirmHMISystemConfigChangesPage
    {
        bool IsPageLoaded();
        string GetAISCountryRate();
        void ClickConfirmAndSendButton();
    }
}
