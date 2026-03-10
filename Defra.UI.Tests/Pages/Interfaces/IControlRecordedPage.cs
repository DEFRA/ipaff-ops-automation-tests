namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IControlRecordedPage
    {
        bool IsPageLoaded();
        string GetCHEDReferenceWithVersion();
        string GetOutcome();
        bool IsViewOrPrintCHEDButtonDisplayed();
        void ClickViewOrPrintCHED();
        void ClickReturnToYourDashboardLink();
    }
}