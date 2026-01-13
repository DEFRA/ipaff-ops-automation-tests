namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IYourImportNotificationsPage
    {
        bool IsPageLoaded();
        void ClickCreateNotification();
        bool IsSearchNotiByPanelDisplayed { get; }
        bool AreAllSearchFieldsDisplayed();
        void SearchForNotification(string notificationNumber);
        bool VerifyNotificationInList(string chedReference);
        void ClickShowNotification(string chedReference);
        bool VerifyCertificateInNewTab();
        bool VerifyDataInCertificate(string chedReference);
        void ClosePDFBrowserTab();
        bool VerifyBrowserTabClosed();
        void ClickAmend(string chedReference);
        void ClickCookiesLink();
        void ClickContactLink();
        void ClickAddressBookLink();
    }
}