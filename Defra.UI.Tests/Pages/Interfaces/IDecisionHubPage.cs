namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDecisionHubPage
    { 
        bool IsPageLoaded();
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
        void ClickViewNotificationOfConsignment();
    }
}
