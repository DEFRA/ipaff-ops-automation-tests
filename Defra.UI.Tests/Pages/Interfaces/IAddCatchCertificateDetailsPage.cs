namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddCatchCertificateDetailsPage
    {
        bool IsPageLoaded(string pageTitle);
        bool VerifyContent(string content);
        bool VerifyCalendar();
        void EnterCatchCertificateReference(string reference, int index=1);
        void EnterDateOfIssue(string day, string month, string year, int index = 1);
        void EnterFlagStateOfCatchingVessel(string FlagState, int index=1);
        void SelectSpecies(int species);
        void ClickChangeLink();
        void EnterNumberOfCatchCertificates(string noOfCertificateRef);
        bool VerifyNoOfCatchReferenceSections(int numberOfRefBlocks);
        void ClickUpdate();
        void ClickUpdate(int index);
        void ClickSaveAndReturnToManageCertificateLink();
        string[] GetSpeciesDetailsList(int index);
    }
}
