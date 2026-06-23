namespace Defra.UI.Tests.Pages.Classes
{
    public interface IConfirmHMISystemConfigChangesPage
    {
        bool IsPageLoaded();
        string GetAISCountryRate();
        void ClickConfirmAndSendButton();
    }
}
