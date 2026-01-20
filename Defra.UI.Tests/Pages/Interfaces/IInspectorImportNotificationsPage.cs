namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IInspectorImportNotificationsPage
    {
        bool IsPageLoaded();
        void SearchForChed(string chedRef);
        bool VerifyNotificationIsPresent(string chedRef);
        void VerifyNotificationStatus(string chedRef, string status);
        void ClickViewCHED();
        void ClickRecordControl();
        void ClickCreateNotification();
        void ClickRecordDecision();
    }
}
