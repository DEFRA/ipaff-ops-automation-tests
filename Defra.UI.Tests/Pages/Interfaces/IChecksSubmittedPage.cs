namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IChecksSubmittedPage
    {
        bool IsPageLoaded();
        string GetCHEDReferenceWithVersion();
        string GetOutcome();
        bool IsViewOrPrintCHEDButtonDisplayed();
        void ClickReturnToYourDashboard();
        void ClickViewOrPrintCHED();
        bool IsError(string errorMessage);
        bool VerifyErrorMessageTitle(string title);
        bool VerifyNextStepsMessage(string message);
        void ClickCreateBorderNotiButton();
        bool VerifyOutcome(string outcome);
        void ClickReturnToDecisionHub();
    }
}