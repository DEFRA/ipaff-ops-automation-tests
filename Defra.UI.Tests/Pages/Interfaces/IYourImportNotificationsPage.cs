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
        void RecordHandlesBeforePdfOpen();
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
        void ClickViewDetails(string chedReference);
        void ClickAddressBookLink();
        void ClickContactLink();
        void ClickViewDetailsLink();
        void ClickCopyAsNewLink();
        bool VerifyNotificationStatus(string status);
        void ClickCloneButton();
        void ClickManageTradePartnersLink();
        string getPDFUrl();
    }
}