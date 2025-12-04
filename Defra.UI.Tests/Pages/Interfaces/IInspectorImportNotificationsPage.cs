namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IInspectorImportNotificationsPage
    {
        bool IsPageLoaded();
        void SearchForChed(string chedRef);
        void VerifyNotificationStatus(string chedRef, string status);
    }
}
