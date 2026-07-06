namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConfirmationOfDefaultRateChangePage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetHmiImportRateConfirmationDetails();
        void ClickConfirmAndSendButton();
    }
}