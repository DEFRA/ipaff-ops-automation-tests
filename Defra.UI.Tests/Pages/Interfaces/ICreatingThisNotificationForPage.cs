namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICreatingThisNotificationForPage
    {
        bool IsPageLoaded();
        void ClickSaveAndReviewButton();
        void SelectNotificationForOption(string option);
    }
}
