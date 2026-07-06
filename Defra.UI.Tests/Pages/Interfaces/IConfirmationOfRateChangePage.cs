namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConfirmationOfRateChangePage
    {
        bool IsPageLoaded();
        bool IsDefaultRateChangePageLoaded();
        IDictionary<string, string> GetConfirmationDetails();
        IDictionary<string, string> GetHmiImportRateConfirmationDetails();
        void ClickConfirmAndSendButton();
    }
}