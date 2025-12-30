namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IBorderNotificationsPage
    {
        void ClickViewDetails();
        bool IsPageLoaded();
        void SearchForNotification(string borderNotification);
        bool VerifyNotificationStatus(string borderNotification, string status);
    }
}
