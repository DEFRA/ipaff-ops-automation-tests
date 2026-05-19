namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICommodityRulesForCHEDAPage
    {
        bool IsPageLoaded();
        bool IsFileSubmissionInProgressSectionPresent();
        string GetFirstInProgressFileName();
        string GetFirstInProgressStatus();
        bool WaitForFirstInProgressStatus(string expectedStatus, int timeoutSeconds = 60);
        bool WaitForFileSubmissionInProgressSectionToBeRemoved(int timeoutSeconds = 60);
        void ClickConfirmAndSubmitLinkForFirstInProgress();
        string GetFirstPreviousSubmissionStatus();
        void ClickViewSummaryLinkForFirstPreviousSubmission();
        void RefreshPage();
    }
}