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
        string GetNotificationStatus();
        bool IsAmendLinkPresent(string chedReference);
        bool IsAmendLinkNotPresent(string chedReference);
        bool IsCopyAsNewLinkPresent(string chedReference);
        bool IsViewDetailsLinkPresent(string chedReference);
        bool IsShowNotificationLinkPresent(string chedReference);
    }
}
