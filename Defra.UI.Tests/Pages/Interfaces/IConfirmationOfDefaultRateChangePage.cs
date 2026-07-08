namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConfirmationOfDefaultRateChangePage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetHmiImportRateConfirmationDetails();
        IDictionary<string, string> GetGmsExportRateConfirmationDetails();
        IDictionary<string, string> GetSmsExportRateConfirmationDetails();
        void ClickConfirmAndSendButton();
    }
}