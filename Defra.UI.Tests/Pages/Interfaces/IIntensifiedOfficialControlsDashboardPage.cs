namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIntensifiedOfficialControlsDashboardPage
    {
        bool IsPageLoaded();
        void ClickCreateNewIntensifiedControlCheck();
        string GetStatusForIOCNumber(string iocNumber);
        void ClickSignOut();
        void FilterByStatusAndCommodity(string status, string commodity);
        bool HasSearchResults();
        void ClickFirstResult();
    }
}