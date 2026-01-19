namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IYourImportNotificationsPage
    {
        bool IsPageLoaded();
        void ClickCreateNotification();
        void SearchForNotification(string notificationNumber);
        bool VerifyNotificationInList(string chedReference);
        void ClickShowNotification(string chedReference);
        bool VerifyCertificateInNewTab();
        bool VerifyDataInCertificate(string chedReference);
        void ClosePDFBrowserTab();
        bool VerifyBrowserTabClosed();
        void ClickAmend(string chedReference);
        void ClickCookiesLink();
        bool VerifyNotificationStatus(string status);
    }
}
