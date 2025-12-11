using OpenQA.Selenium;

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

        // Controlled Destination
        string? GetControlledDestinationName();
        string? GetControlledDestinationAddress();
    }
}