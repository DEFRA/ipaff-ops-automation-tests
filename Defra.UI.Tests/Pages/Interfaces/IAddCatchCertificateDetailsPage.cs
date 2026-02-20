namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddCatchCertificateDetailsPage
    {
        bool IsPageLoaded(string pageTitle);
        bool VerifyContent(string content);
        bool VerifyCalendar();
        void EnterCatchCertificateReference(string reference, int index = 1);
        void EnterDateOfIssue(string day, string month, string year, int index = 1);
        void EnterFlagStateOfCatchingVessel(string FlagState, int index = 1);
        void SelectSpecies(int species);
        void SelectSpeciesByName(string species);
        void ClickChangeLink();
        void EnterNumberOfCatchCertificates(string noOfCertificateRef);
        bool VerifyNoOfCatchReferenceSections(int numberOfRefBlocks);
        void ClickUpdate();
        void ClickUpdate(int index);
        void ClickSaveAndReturnToManageCertificateLink();
        bool VerifyAttachmentNumberDisplayed(int attachmentNumber, int totalAttachments);
        bool VerifyNumberOfCatchCertificatesWithChangeLinkDisplayed();
        bool VerifyCatchCertificateReferenceFieldDisplayed();
        bool VerifyDateOfIssueFieldsWithCalendarDisplayed();
        bool VerifyFlagStateFieldDisplayed();
        bool VerifySelectSpeciesWithSelectAllDisplayed();
        bool VerifySaveAndContinueButtonDisplayed();
        bool VerifySaveAndReturnToManageCatchCertificatesLinkDisplayed();
        bool VerifySaveAndReturnToHubLinkDisplayed();
        void StartTypingInFlagState(string partialText, int index = 1);
        bool VerifyDropdownOptionsInclude(string optionText);
        void SelectFromDropdown(string optionText);
        bool IsFieldHighlighted(string fieldName, int index = 1);
        (bool allErrorsPresent, string errorMessages) VerifySpecificErrorsDisplayed(params string[] expectedErrors);
        void ClickAddTheCommodityLink();
        void ClickSelectAllCheckbox();
        bool VerifyAllSpeciesAreSelected();
    }
}