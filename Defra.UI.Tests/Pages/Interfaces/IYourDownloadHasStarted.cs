namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IYourDownloadHasStarted
    {
        bool IsPageLoaded();
        bool IsDownloadedZipFile(string chedReference);
        void ClickReturnToDocuments();
    }
}
