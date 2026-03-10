namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IControlRecordedPage
    {
        bool IsPageLoaded();
        string GetCHEDReferenceWithVersion();
        string GetOutcome();
        void ClickViewOrPrintCHED();
        void ClickReturnToYourDashboard();
    }
}