namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDecisionHubPage
    {
        bool IsPageLoaded();
        bool IsPageLoadedForChedReference(string expectedChedReference);
        void ClickSaveAndSetAsInProgress();
        bool VerifyStatusUpdate(string stausNew, string statusInProgress);
        void ClickLocalRefNumLink();
        void ClickSealNumbersLink();
        void ClickLaboratoryTestsLink();
        void ClickDecisionLink();
        void ClickReviewAndSubmitLink();
        void ClickOverrideRiskDecisionLink();
        bool VerifyInspectionRequiredBox(string msgboxTitle);
        bool VerifyInspectionRequiredMessage(string message);
        void ClickChecksLink();
        void ClickViewNotificationOfConsignment();
        void ClickAttachmentsButton();
        bool VerifyRecordChecksLinksAreClickable(string firstLink, string secondLink);
        void ClickReturnToWorkOrder();
        bool VerifyRecordChecksStatus(string checkName, string expectedStatus);
        void ClickRecordChecksLink(string checkName);
    }
}