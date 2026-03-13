namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIntensifiedOfficialControlsDashboardPage
    {
        bool IsPageLoaded();
        void ClickCreateNewIntensifiedControlCheck();
        string GetStatusForIOCNumber(string iocNumber);
        void ClickSignOut();
    }
}