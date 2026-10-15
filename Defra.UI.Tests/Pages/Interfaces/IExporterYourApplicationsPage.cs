namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterYourApplicationsPage
    {
        bool IsPageLoaded();
        void ClickStartNewApplicationButton();
        void SearchForApplication(string reference);
        int GetApplicationRowCount();
        void ClickAphaReferenceLink(string reference);
    }
}