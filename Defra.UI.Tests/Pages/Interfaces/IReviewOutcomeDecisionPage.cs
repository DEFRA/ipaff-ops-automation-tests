namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReviewOutcomeDecisionPage
    {
        bool IsPageLoaded();
        void ClickSubmitDecision();
        void EnterCurrentDateAndTime(string day, string month, string year, string hours, string minutes);
        void SelectCertifyingOfficerRadioButton();

        // Border Control Post
        string? GetBorderControlPostReference();

        // Checks
        string? GetDocumentaryCheckDecision();
        string? GetIdentityCheckType();
        string? GetIdentityCheckDecision();
        string? GetPhysicalCheckDecision();
        string? GetNumberOfAnimalsChecked();
        string? GetWelfareCheckDecision();

        // Impact of transport
        string? GetNumberOfDeadAnimals();
        string? GetNumberOfDeadAnimalsUnit();
        string? GetNumberOfUnfitAnimals();
        string? GetNumberOfUnfitAnimalsUnit();
        string? GetNumberOfBirthsOrAbortions();

        // Seal Numbers
        string? GetSealNumbersStatus();

        // Laboratory Tests
        string? GetLaboratoryTestsRequired();
        string? GetLaboratoryTestsReason();
        string? GetLaboratoryTestAnalysisType(int index = 0);
        string? GetLaboratoryTestCommoditySampled(int index = 0);
        string? GetLaboratoryTestName(int index = 0);
        string? GetLaboratoryTestSampleDate(int index = 0);
        string? GetLaboratoryTestSampleTime(int index = 0);
        string? GetLaboratoryTestSampleUseByDate(int index = 0);
        string? GetLaboratoryTestReleasedDate(int index = 0);
        string? GetLaboratoryTestConclusion(int index = 0);
        bool AreLaboratoryTestDetailsDisplayed();

        // Documents
        string? GetHealthCertificateReference();
        string? GetHealthCertificateDateOfIssue();
        string? GetAdditionalDocumentType();
        string? GetAdditionalDocumentReference();
        string? GetAdditionalDocumentDateOfIssue();
        string? GetHealthCertificateFileName();
        string? GetAdditionalDocumentFileName();

        // Decision
        string? GetAcceptanceDecision();
        string? GetCertifiedFor();
        string? GetConsignmentUse();
        string? GetDeadline();
        string? GetExitBCP();
        string? GetTransitExitBCP();
        string? GetTransitDestinationCountry();
        string? GetRefusalReason();
        string? GetDecisionSubOption();

        // Controlled Destination
        string? GetControlledDestinationName();
        string? GetControlledDestinationAddress();
        bool VerifyReason(string reason);
    }
}