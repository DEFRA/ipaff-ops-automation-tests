namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReportsPage
    {
        bool IsPageLoaded();
        void ClickChedPPReportsLink();
        void ClickChedPReportsLink();
        void ClickChedAReportsLink();
        void ClickChedDReportsLink();
    }
}