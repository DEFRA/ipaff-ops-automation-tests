namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmYourDefraAccountPage
    {
        bool IsPageLoaded();
        string GetFullName();
        string GetEmail();
        string GetPhone();
        bool HasExporterAdminAccount(string businessName);
        void CloseAndReturnToExporter(string parentHandle);
    }
}
