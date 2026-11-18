namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterYourApplicationsPage
    {
        bool IsPageLoaded();
        void ClickStartNewApplicationButton();
        bool IsStartNewApplicationAvailable();
        void ClickManageAccountLink();
        void ClickSignOutLink();
        void SearchForApplication(string reference);
        int GetApplicationRowCount();
        void ClickAphaReferenceLink(string reference);
    }
}