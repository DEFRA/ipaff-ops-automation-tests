namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IBorderNotificationSubmittedPage
    {
        void ClickReturnToDashboard();
        string GetBNNumber();
        bool IsPageLoaded();
    }
}