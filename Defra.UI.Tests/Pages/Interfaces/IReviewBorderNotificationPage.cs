namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReviewBorderNotificationPage
    {
        void ClickDocumentLink();
        void ClickSubmitButton();
        bool IsPageLoaded();
        void OpenDownloadsInNewTab();
        bool VerifyFileDownloaded(string fileName);
    }
}
