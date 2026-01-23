namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IInspectorImportNotificationsPage
    {
        bool IsPageLoaded();
        void SearchForChed(string chedRef);
        bool VerifyNotificationIsPresent(string chedRef);
        void VerifyNotificationStatusAndClick(string chedRef, string status);
        void ClickViewCHED();
        void ClickRecordControl();
        void ClickCreateNotification();
        void ClickRecordDecision();
        bool VerifyNotificationIsPresentWithStatus(string type, string chedRef, string replacementChedReference, string status);
        bool VerifyNotificationIsNotPresent();
        void ClickNotification();
    }
}
